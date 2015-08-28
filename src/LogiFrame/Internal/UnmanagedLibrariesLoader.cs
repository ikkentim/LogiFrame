// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace LogiFrame.Internal
{
    internal static class UnmanagedLibrariesLoader
    {
        private static bool _isLoaded;

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Load()
        {
            if (_isLoaded) return;
            _isLoaded = true;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var internalFolderPath = $"{assemblyName.Name}.libs.";
            var libraries = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(s => s.StartsWith(internalFolderPath))
                .ToArray();

            var directoryPath = Path.Combine(Path.GetTempPath(), $"{assemblyName.Name}.{assemblyName.Version}");

            Directory.CreateDirectory(directoryPath);

            foreach (var libraryName in libraries)
            {
                var internalLibraryPath = Path.Combine(directoryPath, libraryName.Substring(internalFolderPath.Length));

                if (!File.Exists(internalLibraryPath))
                {
                    try
                    {
                        using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(libraryName))
                        using (var outStream = File.Create(internalLibraryPath))
                            stream?.CopyTo(outStream);
                    }
                    catch
                    {
                    }
                }

                LoadLibrary(internalLibraryPath);
            }
        }
    }
}