using System;
using LogiFrame;
using LogiFrame.Components;
using System.Diagnostics;
using System.Windows.Forms;
namespace Test
{
    static class Program
    {
        static Circle sq = new Circle();
        static void Main()
        {
            Frame frame = new Frame("LogiFrame test application", false, false, false, true);
            frame.UpdatePriority = UpdatePriority.Alert;

            frame.ButtonDown += new Frame.ButtonDownEventHandler(frame_ButtonDown);
            frame.ButtonUp += new Frame.ButtonUpEventHandler(frame_ButtonUp);
            frame.Pushing += new Frame.PushingEventHandler(frame_FramePush);
            frame.Configure += new EventHandler(frame_Configure);
            frame.FrameClosed += new EventHandler(frame_FrameClosed);



            Debug.WriteLine("Draw square");
            sq.Size = new Size(30, 30);
            frame.MainContainer.Components.Add(sq);

            frame.WaitForClose();

        }

        static void frame_FramePush(object sender, PushingEventArgs e)
        {
        }

        static void frame_FrameClosed(object sender, EventArgs e)
        {
            Debug.WriteLine("Frame closed.");
        }

        static void frame_Configure(object sender, EventArgs e)
        {
            Debug.WriteLine("Frame config opened.");
        }

        static void frame_ButtonUp(object sender, ButtonEventArgs e)
        {

            Debug.WriteLine("Button released: " + e.Button);
        }

        static void frame_ButtonDown(object sender, ButtonEventArgs e)
        {
            if (e.Button == 0)
                ((Frame)sender).Dispose();

            if (e.Button == 1)
                sq.Fill = !sq.Fill;
            Debug.WriteLine("Button pressed: " + e.Button);
        }
    }
}
