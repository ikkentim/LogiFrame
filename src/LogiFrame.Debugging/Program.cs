// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Drawing;
using System.Threading;
using LogiFrame.Debugging.Properties;
using LogiFrame.Drawing;

namespace LogiFrame.Debugging
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            var f = new Frame("Frame", false, false, false);
            f.UpdatePriority = UpdatePriority.Alert;
            var label = new FrameLabel
            {
                Font = PixelFonts.Small,
                Location = new Point(2, 2),
                Size = f.Size,
                MergeMethod = MergeMethods.Transparent,
                Text = "Push ze button."
            };
            var line = new FrameLine
            {
                Start = new Point(100, 2),
                End = new Point(159, 42)
            };
            var rectangle = new FrameRectangle
            {
                Location = new Point(0, 0),
                Size = new Size(40, 40),
                Style = RectangleStyle.Bordered
            };

            var ellipse = new FrameEllipse
            {
                Location = new Point(40, 20),
                Size = new Size(50, 20)
            };

            var progressBar = new FrameProgressBar
            {
                Location = new Point(12, 14),
                Size = new Size(136, 6),
                Style = BorderStyle.Border,
                Direction = ProgressBarDirection.Right,
                Value = 50
            };

            var picture = new FramePicture
            {
                Location = new Point(100, 10),
                Size = Resources.gtech.Size,
                Image = Resources.gtech,
                MergeMethod = MergeMethods.Overlay
            };

            f.Controls.Add(rectangle);
            f.Controls.Add(label);
            f.Controls.Add(line);
            f.Controls.Add(ellipse);
            f.Controls.Add(progressBar);
            f.Controls.Add(picture);

            f.ButtonDown += (sender, args) =>
            {
                if (picture.MergeMethod == MergeMethods.Transparent)
                {
                    picture.MergeMethod = MergeMethods.Invert;
                }
                else if (picture.MergeMethod == MergeMethods.Invert)
                {
                    picture.MergeMethod = MergeMethods.Override;
                }
                else if (picture.MergeMethod == MergeMethods.Override)
                {
                    picture.MergeMethod = MergeMethods.Overlay;
                }
                else if (picture.MergeMethod == MergeMethods.Overlay)
                {
                    picture.MergeMethod = MergeMethods.Transparent;
                }

                label.Text = picture.MergeMethod.ToString();
            };
            
            f.PushToForeground(true);

            var n = 0;
            var t = new Timer
            {
                Enabled = true,
                Interval = 100
            };
            t.Tick += (sender, args) =>
            {
                label.Text = $"Tick: {++n}";
            };
            while (true)
                Thread.Sleep(1000);
        }
    }
}