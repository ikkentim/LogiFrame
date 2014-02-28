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

using System.Drawing;
using LogiFrame;
using LogiFrame.Components;
using Size = LogiFrame.Size;

namespace Spotify
{
    internal static class Program
    {
        private static void Main()
        {
            var frame = new Frame("Spotify", true, true);
            var reader = new SpotifyReader();

            //Setup the style of the Marquees
            var trackMarquee = new Marquee
            {
                Font = new Font("Arial", 8f),
                Location = new Location(0, 20),
                Size = new Size(Frame.LCDSize.Width, 15),
                UseCache = true,
                EndSteps = 5,
                MarqueeStyle = MarqueeStyle.Visibility
            };
            var artistMarquee = new Marquee
            {
                Font = new Font("Arial", 8f, FontStyle.Bold),
                Location = new Location(0, 5),
                Size = new Size(Frame.LCDSize.Width, 15),
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
                }
                else
                {
                    //Set the artist and track labels.
                    frame.UpdatePriority = UpdatePriority.Normal; //Show the application
                    artistMarquee.Text = !reader.Playing ? "Spotify is not playing." : reader.Artist;
                    trackMarquee.Text = !reader.Playing ? "" : reader.Track;
                }
            };

            //By default, set the priority of the application to NoShow, untill it detects spotify
            frame.UpdatePriority = UpdatePriority.IdleNoShow;
            frame.Components.Add(artistMarquee);
            frame.Components.Add(trackMarquee);
            frame.Components.Add(timer);

            //Let the current thread wait untill the Frame is Closed by disposure. (Frame.Dispose, or process ended)
            frame.WaitForClose();
        }
    }
}