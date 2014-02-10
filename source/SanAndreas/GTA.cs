// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System.Diagnostics;
using System.Linq;

namespace SanAndreas
{
    internal static class GTA
    {
        private static Process _process;

        public static Process Process
        {
            get
            {
                Refresh();
                return _process;
            }
        }

        public static Memory Memory
        {
            get
            {
                Refresh();
                return Process;
            }
        }

        public static bool IsRunning
        {
            get
            {
                Refresh();
                return _process != null && !_process.HasExited;
            }
        }

        private static void Refresh()
        {
            if (_process != null && !_process.HasExited) return;
            _process = Process.GetProcessesByName("gta_sa").FirstOrDefault();
        }
    }
}