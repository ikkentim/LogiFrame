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

using System.Collections.Generic;

namespace CoD4
{
    public class ServerResponse
    {
        /// <summary>
        /// Gets or sets the server variables.
        /// </summary>
        public Dictionary<string, string> Variables { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        public List<Player> Players { get; set; }
    }
}