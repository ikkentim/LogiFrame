using System;
using LogiFrame;
using LogiFrame.Components;
using System.Diagnostics;
using System.Windows.Forms;
namespace Test
{
    static class Program
    {
        static void Main()
        {
            Frame frame = new Frame("LogiFrame test application", false, false, false, true);
            frame.UpdatePriority = UpdatePriority.Alert;


            frame.ButtonDown += new Frame.ButtonDownEventHandler(frame_ButtonDown);

            Line line = new Line();
            line.Start = new Location(0, 30);
            line.End = new Location(100, 0);

            Square sq = new Square();
            sq.Location = new Location(100, 5);
            sq.Size= new Size(30, 30);
            frame.Components.Add(sq);
            frame.Components.Add(line);

            frame.WaitForClose();

        }

        static void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == 0)
                ((Frame)sender).Dispose();
            if (e.Button == 1)
                ((Frame)sender).Refresh();
            Debug.WriteLine("Button pressed: " + e.Button);
        }
    }
}
