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

            f.Controls.Add(rectangle);
            f.Controls.Add(label);
            f.Controls.Add(line);

            var font = "small";
            var fontFn = PixelFonts.Small;
            f.ButtonDown += (sender, args) =>
            {
                if (fontFn.Equals(PixelFonts.Small))
                {
                    font = "big";
                    fontFn = PixelFonts.Capitals;
                }
                else if (fontFn.Equals(PixelFonts.Capitals))
                {
                    font = "title";
                    fontFn = PixelFonts.Title;
                }
                else if (fontFn.Equals(PixelFonts.Title))
                {
                    font = "small";
                    fontFn = PixelFonts.Small;
                }
                label.Font = fontFn;
                label.Text = $"Hello, World! This font is >{font}<";
            };

            while (true)
                Thread.Sleep(1000);
        }
    }
}