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
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CoD4
{
    public class Server
    {
        /// <summary>
        /// Initializes a new instance of Server.
        /// </summary>
        /// <param name="ip">IP</param>
        /// <param name="port">Port</param>
        public Server(string ip, short port)
        {
            //Assign properties
            IP = ip;
            Port = port;
        }

        /// <summary>
        /// Gets the IP of this Server.
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// Gets the Port of this Server.
        /// </summary>
        public short Port { get; private set; }

        /// <summary>
        /// Returns server information.
        /// </summary>
        /// <returns>Server information</returns>
        public ServerResponse Query()
        {
            //Send getstatus using UdpClient to IP:Port
            IPEndPoint endPoint = null; //unused
            var client = new UdpClient(IP, Port);

            var timeToWait = TimeSpan.FromSeconds(1); //Timeout: 1 sec
            var sendBytes = new byte[]
            {
                0xff, 0xff, 0xff, 0xff, 0x67, 0x65, 0x74, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73, 0x00
                //????getstatus command
            };
            //Send command
            client.Send(sendBytes, sendBytes.Length);

            //Receive response
            var asyncResult = client.BeginReceive(null, null);
            asyncResult.AsyncWaitHandle.WaitOne(timeToWait);
            try
            {
                //Decode by ASCII
                string data = Encoding.ASCII.GetString(client.EndReceive(asyncResult, ref endPoint));

                //Split lines and verify response
                var lines = data.Split('\n');
                if (lines.FirstOrDefault() != "????statusResponse" || lines.Length < 2) return null;

                //Build response
                var response = new ServerResponse
                {
                    Variables = new Dictionary<string, string>(),
                    Players = new List<Player>()
                };

                //Split vars
                var vars = lines[1].Split(@"\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (var i = 1; i < vars.Length; i += 2)
                    response.Variables.Add(vars[i - 1], vars[i]);

                //Split players
                for (var i = 2; i < lines.Length; i++)
                {
                    var sp = lines[i].Split(" ".ToCharArray(), 3);
                    if (sp.Length != 3 || sp[2].Length < 3) continue;
                    response.Players.Add(new Player
                    {
                        Score = sp[0],
                        Ping = sp[1],
                        Name = sp[2].Substring(1, sp[2].Length - 2)
                    });
                }

                return response;
            }
            catch (Exception)
            {
                //Failure, NULL
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", IP, Port);
        }
    }
}