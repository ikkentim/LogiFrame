using LogiFrame;
using LogiFrame.Components;
using System.Diagnostics;

namespace Test
{
    static class Program
    {
        static void Main()
        {
            //Test application

            Frame frame = new Frame("LogiFrame test application", false, false, false, true)
            {
                UpdatePriority = UpdatePriority.Alert
            };


            frame.ButtonDown += frame_ButtonDown;

            Line line = new Line
            {
                Start = new Location(0, 30),
                End = new Location(130, 15),
                Transparent = true,
                TopEffect = true
            };

            Square sq = new Square {Location = new Location(100, 5), Size = new Size(30, 30), Fill = true};

            Label l = new Label
            {
                Location = new Location(10,10),
                Size = new Size(50, 20),
                Font = new System.Drawing.Font("Arial", 7),
                Text = "I am a test",
                AutoSize = true
            };

            frame.Components.Add(sq);
            frame.Components.Add(line);
            frame.Components.Add(l);

            Debug.WriteLine("\nApplication initialized\n");
            frame.WaitForClose();
        }

        static void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == 0)
                ((Frame)sender).Dispose();
            if (e.Button == 1)
                ((Frame)sender).Refresh(true);

            Debug.WriteLine("Button pressed: " + e.Button);
        }
    }
}
