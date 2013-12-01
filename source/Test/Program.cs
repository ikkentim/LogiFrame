using System;
using LogiFrame;
using LogiFrame.Components;
using System.Diagnostics;
using Label = LogiFrame.Components.Label;

namespace Test
{
    static class Program
    {
        private static Animation ani;
        private static ProgressBar prog;

        private static void Main()
        {
            //Test application
            Frame frame = new Frame("LogiFrame test application", false, false, true, true)
            {
                UpdatePriority = UpdatePriority.Alert
            };

            frame.ButtonDown += frame_ButtonDown;
            frame.Configure += frame_Configure;
            Line line = new Line
            {
                Start = new Location(130, 30),
                End = new Location(150, 10),
                Transparent = true,
                TopEffect = true
            };

            Square sq = new Square
            {
                Location = new Location(149, 1),
                Size = new Size(10, 10),
                Fill = true
            };

            Label l = new Label
            {
                Location = new Location(110, 30),
                Size = new Size(50, 20),
                Font = new System.Drawing.Font("Arial", 7),
                Text = "I am a test",
                AutoSize = true
            };

            Picture pic = new Picture
            {
                Location = new Location(100, 2),
                AutoSize = true,
                Image = Properties.Resources.test
            };

            prog = new ProgressBar
            {
                Location = new Location(40, 0),
                Size = new System.Drawing.Size(60, 15),
                Value = 30,
                ProgressBarStyle = ProgressBarStyle.WhiteSpacedBorder
            };
            ani = new Animation
            {
                Location = new Location(0, -3),
                AutoSize = true,
                ConversionMethod = ConversionMethod.QuarterByte,
                Image = Properties.Resources.banana,
                Run = true
            };


            frame.Components.Add(sq);
            frame.Components.Add(line);
            frame.Components.Add(l);
            frame.Components.Add(pic);
            frame.Components.Add(ani);
            frame.Components.Add(prog);

            Debug.WriteLine("\nApplication initialized\n");
            frame.WaitForClose();
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
                ani.Frame++;
            if (e.Button == 3)
                prog.Value = 100 - prog.Value;
            Debug.WriteLine("Button pressed: " + e.Button);
        }

    }
}
