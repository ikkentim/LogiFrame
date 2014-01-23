using System;
using System.Drawing;
using LogiFrame;
using LogiFrame.Components;
using System.Diagnostics;
using LogiFrame.Components.Book;

namespace Test
{
    internal static class Program
    {
        #region Basic Test

        /*
        private static Animation _animation;
        private static ProgressBar _progressBar;
        private static Line _line;
        private static Square _square;
        private static Label _label;
        private static Picture _picture;
        
        private static void SetupBasicTest(Frame frame)
        {
            frame.ButtonDown += frame_ButtonDown;
            frame.Configure += frame_Configure;

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
                Value = 30,
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder
            };
            _animation = new Animation
            {
                Location = new Location(0, -3),
                AutoSize = true,
                ConversionMethod = ConversionMethod.QuarterByte,
                Image = Properties.Resources.banana,
                Run = true
            }; 
           
            frame.Components.Add(_square);
            frame.Components.Add(_line);
            frame.Components.Add(_label);
            frame.Components.Add(_picture);
            frame.Components.Add(_animation);
            frame.Components.Add(_progressBar);
        }
        
        static void frame_Configure(object sender, EventArgs e)
        {
            Debug.WriteLine("CONFIGURE");
        }

        static void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == 0)
                ((Frame)sender).Dispose();
            if (e.Button == 1)
                ((Frame)sender).Refresh(true);
            if (e.Button == 2)
                _animation.Frame++;
            if (e.Button == 3)
                _progressBar.Value = 100 - _progressBar.Value;
            Debug.WriteLine("Button pressed: " + e.Button);
        }
        */

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

        #endregion

        private static void Main()
        {
            //Test application
            Frame frame = new Frame("LogiFrame test application", false, false, true, true)
            {
                UpdatePriority = UpdatePriority.Normal
            };

            SetupBookTest(frame);

            frame.WaitForClose();
        }

        [PageInfo("Test page")]
        private class CustomPage : Page
        {
            public CustomPage()
            {
                var l = new Label
                {
                    AutoSize = true,
                    Text = ToString()
                };

                Components.Add(l);
            }

            protected override PageIcon GetPageIcon()
            {
                var icon = new PageIcon();

                icon.Components.Add(new Label
                {
                    AutoSize = true,
                    Text = "T",
                    Font = new Font("Arial", 10f, FontStyle.Bold)
                });

                return icon;
            }
        }

        [PageInfo("Test page 2")]
        private class CustomPage2 : CustomPage
        {
        }

        [PageInfo("Test page 3")]
        private class CustomPage3 : CustomPage
        {
        }

        [PageInfo("Test page 4")]
        private class CustomPage4 : CustomPage
        {
        }
    }
}
