using System;
using LogiFrame;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace Test
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Bytemap bm = Bytemap.FromBitmap((Bitmap)Image.FromFile("test.bmp"));
            ((Bitmap)bm).Save("test2.bmp");
            Frame frame = new Frame("LogiFrame test application", false, false, false);
            frame.ButtonDown += new Frame.ButtonDownEventHandler(frame_ButtonDown);
            frame.ButtonUp += new Frame.ButtonUpEventHandler(frame_ButtonUp);
            frame.Configure += new Frame.ConfigureEventHandler(frame_Configure);
            frame.FrameClosed += new Frame.FrameClosedEventHandler(frame_FrameClosed);
            frame.FramePush += new Frame.FramePushEventHandler(frame_FramePush);
            frame.UpdateScreen(bm);
            frame.WaitForClose();

        }

        static void frame_FramePush(object sender, FramePushEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void frame_FrameClosed(object sender, FrameClosedEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void frame_Configure(object sender, ConfigureEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void frame_ButtonUp(object sender, ButtonUpEventArgs e)
        {
            if (e.Button == 0)
                ((Frame)sender).Dispose();
            Debug.WriteLine("Button released: " + e.Button);
        }

        static void frame_ButtonDown(object sender, ButtonDownEventArgs e)
        {
            Debug.WriteLine("Button pressed: " + e.Button);
        }
    }
}
