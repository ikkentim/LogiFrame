using System;
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

            Frame frame = new Frame("LogiFrame test application", false, false, false, true);
            frame.UpdatePriority = UpdatePriority.Alert;


            frame.ButtonDown += new Frame.ButtonDownEventHandler(frame_ButtonDown);

            Line line = new Line();
            line.Start = new Location(0, 30);
            line.End = new Location(130, 15);
            line.Transparent = true;
            line.TopEffect = true;

            Square sq = new Square();
            sq.Location = new Location(100, 5);
            sq.Size= new Size(30, 30);
            sq.Fill = true;

            Label l = new Label();
            l.Size = new Size(50, 20);

            frame.Components.Add(sq);
            frame.Components.Add(line);
            frame.Components.Add(l);

            Debug.WriteLine("\n\nApplication initialized\n\n");
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
