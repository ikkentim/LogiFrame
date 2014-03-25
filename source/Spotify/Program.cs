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
using System.Diagnostics;
using System.Drawing;
using System.Security.Principal;
using LogiFrame;
using LogiFrame.Components;
using Spotify.Properties;
using Size = LogiFrame.Size;

namespace Spotify
{
    internal static class Program
    {
        private static void Main()
        {
            //Pre-load images
            var pause = Resources.Pause;
            var play = Resources.Play;
            var stop = Resources.Stop;

            //Setup frame and spotify reader
            var frame = new Frame("Spotify", true, true, false, true);
            var reader = new SpotifyReader();

            //Setup the style of the Marquees
            var statePicture = new Picture
            {
                Image = stop,
                AutoSize = true,
                Location = new Location(5, 5)
            };
            var trackMarquee = new Marquee
            {
                Font = new Font("Arial", 8f),
                Location = new Location(0, 25),
                Size = new Size(Frame.LCDSize.Width, 15),
                UseCache = true,
                EndSteps = 5,
                MarqueeStyle = MarqueeStyle.Visibility
            };
            var artistMarquee = new Marquee
            {
                Font = new Font("Arial", 8f, FontStyle.Bold),
                Location = new Location(26, 5),
                Size = new Size(Frame.LCDSize.Width - 26, 15),
                Interval = 500,
                StepSize = 4,
                EndSteps = 12,
                UseCache = true,
                MarqueeStyle = MarqueeStyle.Visibility,
                Run = true,
                SyncedMarquees = {trackMarquee}
            };
            var timer = new Timer
            {
                Interval = 500,
                Enabled = true
            };

            //Listen to the Tick-event of the Timer and set the interval
            timer.Tick += delegate
            {
                //Update our spotify reader
                reader.Update();

                //Check if spotify is running
                if (!reader.Running)
                {
                    //If the tracks is empty, spotify isn't running.
                    frame.UpdatePriority = UpdatePriority.IdleNoShow; //Hide the application
                    artistMarquee.Text = "Spotify is not running.";
                    trackMarquee.Text = "";
                    statePicture.Image = stop;
                }
                else
                {
                    //Set the artist and track labels.
                    frame.UpdatePriority = UpdatePriority.Normal; //Show the application
                    artistMarquee.Text = string.IsNullOrEmpty(reader.Artist) ? "Spotify is not playing." : reader.Artist;
                    trackMarquee.Text = string.IsNullOrEmpty(reader.Track) ? "" : reader.Track;
                    statePicture.Image = reader.Playing ? play : pause;
                }
            };

            //By default, set the priority of the application to NoShow, untill it detects spotify
            frame.UpdatePriority = UpdatePriority.IdleNoShow;
            frame.Components.Add(statePicture);
            frame.Components.Add(artistMarquee);
            frame.Components.Add(trackMarquee);
            frame.Components.Add(timer);

            //Render first frame
            timer.OnTick(EventArgs.Empty);

            //Let the current thread wait untill the Frame is Closed by disposure. (Frame.Dispose, or process ended)
            frame.WaitForClose();
        }
    }
}