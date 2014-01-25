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
using LogiFrame;
using LogiFrame.Components;

namespace Spotify
{
    internal static class Program
    {
        //Initialize all components

        /// <summary>
        /// Contains the name of the artist.
        /// </summary>
        private static readonly Label ArtistLabel = new Label();

        /// <summary>
        /// Contains the name of the track.
        /// </summary>
        private static readonly Label TrackLabel = new Label();

        /// <summary>
        /// Ticks whenever we want to update the title. (Works just like System.Windows.Forms.Timer)
        /// </summary>
        private static readonly Timer Timer = new Timer();

        /// <summary>
        /// The main LogiFrame.Frame who's pushing the frames to the display.
        /// </summary>
        private static readonly Frame Frame = new Frame("Spotify", true, true);

        /// <summary>
        /// The reader who's task it is to read the current track and artist.
        /// </summary>
        private static readonly SpotifyReader Reader = new SpotifyReader();

        private static void Main()
        {
            //Setup the style of the Labels
            ArtistLabel.AutoSize = true;
            ArtistLabel.Font = new Font("Arial", 8f, FontStyle.Bold);
            ArtistLabel.Location = new Location(0, 5);
            ArtistLabel.Text = "";

            TrackLabel.AutoSize = true;
            TrackLabel.Font = new Font("Arial", 8f);
            TrackLabel.Location = new Location(0, 20);
            TrackLabel.Text = "";

            //Listen to the Tick-event of the Timer and set the interval
            Timer.Tick += Timer_Tick;
            Timer.Interval = 250;
            Timer.Enabled = true;

            //By default, set the priority of the application to NoShow, untill it detects spotify
            Frame.UpdatePriority = UpdatePriority.IdleNoShow;
            Frame.Components.Add(ArtistLabel);
            Frame.Components.Add(TrackLabel);

            //Let the current thread wait untill the Frame is Closed by disposure. (Frame.Dispose, or process ended)
            Frame.WaitForClose();
        }

        /// <summary>
        /// The event listener contianing the update logics
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            //Update our spotify reader
            Reader.Update();

            if (string.IsNullOrEmpty(Reader.Track))
            {
                //If the tracks is empty, spotify isn't running.
                ArtistLabel.Text = "Spotify is not running.";
                Frame.UpdatePriority = UpdatePriority.IdleNoShow; //Hide the application
            }
            else
            {
                //Set the artist and track labels.
                ArtistLabel.Text = Reader.Artist;
                TrackLabel.Text = Reader.Track;
                Frame.UpdatePriority = UpdatePriority.Normal; //Show the application
            }
        }
    }
}