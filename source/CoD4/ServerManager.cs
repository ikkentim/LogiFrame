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

using System;
using System.Collections.Generic;
using System.Linq;
using CoD4.Properties;

namespace CoD4
{
    internal class ServerManager
    {
        private static readonly List<Server> ServerList = new List<Server>();

        public static IReadOnlyCollection<Server> Servers
        {
            get { return ServerList.AsReadOnly(); }
        }

        /// <summary>
        /// Called when a server has been added/removed
        /// </summary>
        public static event EventHandler ServersChanged;

        /// <summary>
        /// Saves servers to settings.
        /// </summary>
        private static void Save()
        {
            Settings.Default.Servers = string.Join(";", ServerList.Select(s => s.IP + ":" + s.Port));
            Settings.Default.Save();
        }

        /// <summary>
        /// Loads servers from settigns
        /// </summary>
        /// <returns></returns>
        public static int Load()
        {
            //Clear previous list
            ServerList.Clear();

            short tmp; //unused

            //Split ip:port;ip:port to Server objects
            ServerList.AddRange(
                Settings.Default.Servers.Split(';')
                    .Select(server => server.Split(':'))
                    .Where(sp => sp.Length == 2)
                    .Where(sp => short.TryParse(sp[1], out tmp))
                    .Select(sp => new Server(sp[0], short.Parse(sp[1]))));

            //Call event
            if (ServersChanged != null)
                ServersChanged(null, EventArgs.Empty);

            //Return count of loaded servers
            return ServerList.Count;
        }

        /// <summary>
        /// Adds server to list.
        /// </summary>
        /// <param name="server">Server to add.</param>
        public static void Add(Server server)
        {
            //Add and save
            ServerList.Add(server);
            Save();

            //Call event
            if (ServersChanged != null)
                ServersChanged(null, EventArgs.Empty);
        }

        /// <summary>
        /// Removes server from list.
        /// </summary>
        /// <param name="server">Server to remove.</param>
        public static void Remove(Server server)
        {
            //Remove and save
            ServerList.Remove(server);
            Save();

            //Call event
            if (ServersChanged != null)
                ServersChanged(null, EventArgs.Empty);
        }
    }
}