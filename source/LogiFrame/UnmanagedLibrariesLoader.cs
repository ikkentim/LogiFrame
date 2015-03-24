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

namespace LogiFrame
{
    internal static class UnmanagedLibrariesLoader
    {
        public static bool IsLoaded = false;

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        public static void Load()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();

            string[] libraries = Assembly.GetExecutingAssembly().GetManifestResourceNames()
                .Where(s => s.StartsWith(String.Format("{0}.{1}.", assemblyName.Name, "libs")))
                .ToArray();

            string directoryPath = Path.Combine(Path.GetTempPath(),
                String.Format("{1}.{0}", assemblyName.Version, assemblyName.Name));

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            foreach (string lib in libraries)
            {
                string dllPath = Path.Combine(directoryPath, String.Join(".", lib.Split('.').Skip(2)));

                if (!File.Exists(dllPath))
                {
                    using (Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(lib))
                    {
                        try
                        {
                            using (FileStream outFile = File.Create(dllPath))
                            {
                                if (stm != null) stm.CopyTo(outFile);
                            }
                        }
                        catch
                        {
                            /* Assume assembly has already been loaded. */
                        }
                    }
                }

                LoadLibrary(dllPath);
            }
        }
    }
}