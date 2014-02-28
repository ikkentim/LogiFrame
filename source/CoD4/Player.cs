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

namespace CoD4
{
    public class Player
    {
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// Gets or sets the ping.
        /// </summary>
        public string Ping { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            //Build a string
            return string.Format("{0} (Score: {1}, Ping: {2})", Name, Score, Ping);
        }
    }
}