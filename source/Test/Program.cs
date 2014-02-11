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
using LogiFrame.Components.Book;
using Size = LogiFrame.Size;

namespace Test
{
    internal static class Program
    {
        #region Basic Test

        private static Animation _animation;
        private static ProgressBar _progressBar;
        private static Line _line;
        private static Square _square;
        private static Label _label;
        private static Picture _picture;
        private static Marquee _marquee;
        private static ScrollBar _scrollBar;

        private static void SetupBasicTest(Frame frame)
        {
            frame.ButtonDown += delegate(object sender, ButtonEventArgs e)
            {
                if (e.Button == 0)
                    _scrollBar.Value--;
                if (e.Button == 1)
                    _scrollBar.Value++;
            };
            frame.Configure += delegate { };

            _line = new Line
            {
                Start = new Location(130, 30),
                End = new Location(150, 10),
                Transparent = true,
                TopEffect = true
            };

            _square = new Square
            {
                Location = new Location(149, 1),
                Size = new Size(10, 10),
                Fill = true
            };

            _label = new Label
            {
                Location = new Location(110, 30),
                Size = new Size(50, 20),
                Font = new System.Drawing.Font("Arial", 7),
                Text = "I am a test",
                AutoSize = true
            };

            _picture = new Picture
            {
                Location = new Location(100, 2),
                AutoSize = true,
                Image = Properties.Resources.test
            };

            _progressBar = new ProgressBar
            {
                Location = new Location(40, 0),
                Size = new System.Drawing.Size(60, 15),
                Value = 5,
                MaximumValue = 25,
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder
            };
            /*_animation = new Animation
            {
                Location = new Location(0, -3),
                AutoSize = true,
                ConversionMethod = ConversionMethod.QuarterByte,
                Image = Properties.Resources.banana,
                Run = true
            };
            
            _marquee = new Marquee
            {
                Location = new Location(10, 30),
                Size = new Size(50, 10),
                Text = "Test 1 2 3 4 5 6 7 8 9 10... Test Completed!",
                Interval = 200,
                StepSize = 2,
                Run = true,
                UseCache = true
            };*/

            _scrollBar = new ScrollBar
            {
                Location = new Location(0, 0),
                Size = new Size(8, 43),
                Value = 5,
                MaximumValue = 25,
                Horizontal = false
            };

            frame.Components.Add(_square);
            frame.Components.Add(_line);
            frame.Components.Add(_label);
            frame.Components.Add(_picture);
            //frame.Components.Add(_animation);
            frame.Components.Add(_progressBar);
            //frame.Components.Add(_marquee);
            frame.Components.Add(_scrollBar);
        }

        #endregion

        #region Books Test

        private static void SetupBookTest(Frame frame)
        {
            Book book = new Book(frame) {MenuButton = 3};

            book.Pages.Add(new CustomPage());
            book.Pages.Add(new CustomPage2());
            book.Pages.Add(new CustomPage3());
            book.Pages.Add(new CustomPage4());
            book.SwitchTo<CustomPage>();
        }

        private class CustomPage : Page
        {
            public CustomPage()
            {
                Components.Add(new Label
                {
                    AutoSize = true,
                    Text = base.ToString()
                });
            }

            public override string GetName()
            {
                return base.ToString();
            }

            protected override PageIcon GetPageIcon()
            {
                return new PageIcon(new Component[]
                {
                    new Label
                    {
                        AutoSize = true,
                        Text = "T",
                        Font = new Font("Arial", 10f, FontStyle.Bold),
                        UseCache = true
                    }
                });
            }
        }
        private class CustomPage2 : CustomPage
        {
        }
        private class CustomPage3 : CustomPage
        {
        }
        private class CustomPage4 : CustomPage
        {
        }
        #endregion

        private static void Main()
        {
            var frame = new Frame("LogiFrame test application", false, false, true, true);
            SetupBasicTest(frame);
            frame.WaitForClose();
        }

    }
}