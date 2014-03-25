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
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using LogiFrame;
using LogiFrame.Components;
using Size = LogiFrame.Size;

namespace CoD4
{
    internal class Program
    {
        private static Server _currentServer; //Server currently watching.
        private static ServerResponse _currentResponse; //Latest response from server.

        private static int _playerIndex; // Index of player1 we're watchin on the LCD.
        private static Marquee _serverLabel; // Label for server name (Marquee>scrolls duo to long names).
        private static Label _mapLabel; // Label containing map name.
        private static Label _playersLabel; // Label containing 'players online' counter.
        private static Label _player1Label; // Label containing a player name
        private static Label _score1Label; // Label containing a player score
        private static Label _ping1Label; // Label containing a player ping
        private static Label _player2Label; // Label containing a player name
        private static Label _score2Label; // Label containing a player score
        private static Label _ping2Label; // Label containing a player ping

        private static void Main()
        {
            //Load servers, if none in list, show editor and reload.
            if (ServerManager.Load() == 0)
            {
                new EditorForm().ShowDialog();
                ServerManager.Load();
            }

            //Open a frame, start a 30 sec timer.
            var frame = new Frame("Call of Duty 4", true, true, true);
            var timer = new Timer
            {
                Enabled = true,
                Interval = 30*1000
            };
            frame.Components.Add(timer);

            //Create components.
            frame.Components.Add(_serverLabel = new Marquee
            {
                Location = new Location(0, 0),
                Size = new Size(Frame.LCDSize.Width, 15),
                Font = new Font("Arial", 8, FontStyle.Bold),
                MarqueeStyle = MarqueeStyle.Visibility,
                EndSteps = 20,
                StepSize = 4,
                Interval = 500,
                Run = true
            });

            frame.Components.Add(_mapLabel = new Label
            {
                Location = new Location(0, 11),
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_playersLabel = new Label
            {
                Location = new Location(Frame.LCDSize.Width, 11),
                HorizontalAlignment = Alignment.Right,
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_player1Label = new Label
            {
                Location = new Location(0, 21),
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_score1Label = new Label
            {
                Location = new Location(Frame.LCDSize.Width-60, 21),
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_ping1Label = new Label
            {
                Location = new Location(Frame.LCDSize.Width, 21),
                HorizontalAlignment = Alignment.Right,
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_player2Label = new Label
            {
                Location = new Location(0, 31),
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_score2Label = new Label
            {
                Location = new Location(Frame.LCDSize.Width - 60, 31),
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            frame.Components.Add(_ping2Label = new Label
            {
                Location = new Location(Frame.LCDSize.Width, 31),
                HorizontalAlignment = Alignment.Right,
                AutoSize = true,
                Transparent = true,
                Font = new Font("Arial", 7),
            });

            //Event when server list has changed.
            ServerManager.ServersChanged += (sender, eventArgs) =>
            {
                //Set current server to first in list.
                _currentServer = ServerManager.Servers.FirstOrDefault();
                Query(); //Requery server.
            };

            //When timer ticks, Requery the server.
            timer.Tick += (sender, eventArgs) => Query();

            //When Configure is clicked (trough LCD manager), open EditorForm.
            frame.Configure += (sender, eventArgs) => new EditorForm().ShowDialog();

            //Button pressed event...
            frame.ButtonDown += (sender, eventArgs) =>
            {
                if (_currentServer == null) //If no server's selected, open EditorForm.
                {
                    new EditorForm().ShowDialog();
                    return;
                }
                switch (eventArgs.Button)
                {
                    case 0:
                        //Go down 2 players in the list.
                        _playerIndex -= 2;
                        UpdatePlayerInfo();
                        break;
                    case 1:
                        //Go up 2 players in the list.
                        _playerIndex += 2;
                        UpdatePlayerInfo();
                        break;
                    case 2:
                        //Find the next server in the list.
                        var index = Array.IndexOf(ServerManager.Servers.ToArray(), _currentServer) + 1;
                        if (index >= ServerManager.Servers.Count) index = 0;
                        _currentServer = ServerManager.Servers.ElementAt(index);

                        //Requery the server.
                        Query();
                        break;
                    case 3:
                        //Requery the server.
                        Query();
                        break;
                }
            };

            //Load first server and query it.
            _currentServer = ServerManager.Servers.FirstOrDefault();
            Query();

            //Wait for the frame to close.
            frame.WaitForClose();
        }

        private static string ClearColors(string input)
        {
            //Remove ^1 ^2 etc...
            return new Regex(@"(\^[0-9])").Replace(input, "");
        }

        private static void Query()
        {
            //When no server is selected. Show message to open config.
            if (_currentServer == null)
            {
                _serverLabel.Text = "No CoD4 server in serverlist!";
                _mapLabel.Text = "Press any button to open Config.";
                _playersLabel.Text = "";
                _player1Label.Text = "";
                _score1Label.Text = "";
                _ping1Label.Text = "";
                _player2Label.Text = "";
                _score2Label.Text = "";
                _ping2Label.Text = "";
                return;
            }

            //Query.
            _currentResponse = _currentServer.Query();

            //Couldn't connect! :(
            if (_currentResponse == null)
            {
                _serverLabel.Text = "Couldn't connect to...";
                _mapLabel.Text = _currentServer.ToString();
                _playersLabel.Text = "";
                _player1Label.Text = "";
                _score1Label.Text = "";
                _ping1Label.Text = "";
                _player2Label.Text = "";
                _score2Label.Text = "";
                _ping2Label.Text = "";
                return;
            }

            //Set info to labels.
            _serverLabel.Text = ClearColors(_currentResponse.Variables["sv_hostname"]);
            _mapLabel.Text = _currentResponse.Variables["mapname"];
            _playersLabel.Text = _currentResponse.Players.Count + "/" + _currentResponse.Variables["sv_maxclients"];

            //Update player info.
            UpdatePlayerInfo();
        }

        private static void UpdatePlayerInfo()
        {
            //Check for upper bound.
            if (_playerIndex >= _currentResponse.Players.Count)
                _playerIndex = _currentResponse.Players.Count - 1;

            //Check for lower bound.
            if (_playerIndex < 0)
                _playerIndex = 0;

            //Show info for player 1.
            if (_currentResponse.Players.Count > _playerIndex)
            {
                _player1Label.Text = _playerIndex + ": " + _currentResponse.Players[_playerIndex].Name;
                _score1Label.Text = _currentResponse.Players[_playerIndex].Score;
                _ping1Label.Text = _currentResponse.Players[_playerIndex].Ping;
            }
            else
            {
                _player1Label.Text = "";
                _score1Label.Text = "";
                _ping1Label.Text = "";
            }
            //_player1Label.Text = _currentResponse.Players.Count > _playerIndex
            //    ? _playerIndex + ": " + _currentResponse.Players[_playerIndex]
            //    : "";

            //Show info for player 2.
            if (_currentResponse.Players.Count > _playerIndex + 1)
            {
                _player2Label.Text = (_playerIndex + 1) + ": " + _currentResponse.Players[_playerIndex + 1].Name;
                _score2Label.Text = _currentResponse.Players[_playerIndex + 1].Score;
                _ping2Label.Text = _currentResponse.Players[_playerIndex + 1].Ping;
            }
            else
            {
                _player2Label.Text = "";
                _score2Label.Text = "";
                _ping2Label.Text = "";
            }
            //_player2Label.Text = _currentResponse.Players.Count > _playerIndex + 1
            //    ? (_playerIndex + 1) + ": " + _currentResponse.Players[_playerIndex + 1]
            //    : "";
        }
    }
}