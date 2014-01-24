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

namespace Spotify
{
    internal class SpotifyReader
    {
        /// <summary>
        /// Contains the Spotify process.
        /// </summary>
        private Process _process;

        /// <summary>
        /// Initializes a new instance of the SpotifyReader class.
        /// </summary>
        public SpotifyReader()
        {
            //Update the information for the first time
            Update();
        }

        /// <summary>
        /// Gets the name of the currently playing Artist.
        /// </summary>
        public string Artist { get; private set; }

        /// <summary>
        /// Gets the name of the currently playing Track.
        /// </summary>
        public string Track { get; private set; }

        /// <summary>
        /// Updates the information, Artist and Track.
        /// </summary>
        public void Update()
        {
            //If no process has been found yet or the process has exited...
            if (_process == null || _process.HasExited)
            {
                //Clear currently known data
                _process = null;
                Artist = null;
                Track = null;

                //Find the process process
                _process = Process.GetProcesses().First(process => process.ProcessName == "spotify");

                //If the process hasn't been found, stop updating.
                if (_process == null)
                    return;
            }

            //Reload proccess information
            _process.Refresh();

            //Find the track artist from the title of the Spotify window, in the format "Spotify - <Artist> - <Track>"
            string title = _process.MainWindowTitle;
            title = title.Replace("Spotify - ", ""); //Remove the beginning bit, Leaving us with "<Artist> - <Track>"

            int position = title.IndexOf('–'); //Get the index of the '–' in between the Artist and Track.

            //If the '–' hasn't been found, stop updating.
            if (position < 0)
                return;

            //Get a substring from the window title and trim the remaining spaces off.
            Artist = title.Substring(0, position).Trim();
            Track = title.Substring(position + 1).Trim();
        }
    }
}