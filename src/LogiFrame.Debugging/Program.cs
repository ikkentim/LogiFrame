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

using System;
using System.Data.Common;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
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
            var f = new LCDApp("LCDApp", false, false, false);

            var label = new LCDLabel
            {
                Font = PixelFonts.Small,
                Size = f.Size,
                Text = "Align me!",
                TextAlign = ContentAlignment.BottomRight,
            };
//            var label = new LCDLabel
//            {
//                Font = PixelFonts.Small,
//                Location = new Point(2, 2),
//                AutoSize = true,
//                Size = f.Size,
//                MergeMethod = MergeMethods.Transparent,
//                Text = "Push ze button."
//            };
            var line = new LCDLine
            {
                Start = new Point(0, 0),
                End = new Point(f.Width-1, f.Height-1)
            };
            var rectangle = new LCDRectangle
            {
                Location = new Point(0, 0),
                Size = new Size(40, 40),
                Style = RectangleStyle.Bordered
            };

            var ellipse = new LCDEllipse
            {
                Location = new Point(40, 20),
                Size = new Size(50, 20)
            };

            var progressBar = new LCDProgressBar
            {
                Location = new Point(12, 14),
                Size = new Size(136, 6),
                Style = BorderStyle.Border,
                Direction = ProgressBarDirection.Right,
                Value = 50
            };

            var picture = new LCDPicture
            {
                Location = new Point(100, 10),
                Size = Resources.gtech.Size,
                Image = Resources.gtech,
                MergeMethod = MergeMethods.Overlay
            };

            var marq = new LCDMarquee
            {
                Text = "Lorem Ipsum Lorem Ipsum Lorem Ipsum Lorem Ipsum",
                Size = new Size(LCDApp.DefaultSize.Width, 10),
                Location = new Point(0, 10),
            };

            var graph = new LCDSimpleGraph
            {
                Location = new Point(70, 30),
                Size = new Size(40, 10),
                Style = BorderStyle.Border
            };

            (new Timer {Interval = 500, Enabled = true}).Tick += (sender, args) =>
            {
                graph.PushValue(new Random().Next(0, 100));
            };

            var tabControl = new LCDTabControl();

            var tabPage = new LCDTabPage
            {
                Icon = new LCDLabel
                {
                    AutoSize = true,
                    Text = "A",
                    Font = PixelFonts.Title
                }
            };

            var tabPage2 = new LCDTabPage
            {
                Icon = new LCDLabel
                {
                    AutoSize = true,
                    Text = "B",
                    Font = PixelFonts.Title
                }
            };

            tabPage.Controls.Add(label);
//            tabPage.Controls.Add(line);
//            tabPage.Controls.Add(marq);

            tabPage2.Controls.Add(rectangle);
            tabPage2.Controls.Add(ellipse);
            tabPage2.Controls.Add(progressBar);
            tabPage2.Controls.Add(picture);
            tabPage2.Controls.Add(graph);

            tabControl.TabPages.Add(tabPage);
            tabControl.TabPages.Add(tabPage2);
            tabControl.SelectedTab = tabPage;
            
            f.Controls.Add(tabControl);

            f.ButtonDown += (sender, args) =>
            {
                if (args.Button == 2) tabControl.ShowMenu();
                if (args.Button == 3) f.Dispose();
            };

            f.PushToForeground(true);

            f.WaitForClose();
        }
    }
}