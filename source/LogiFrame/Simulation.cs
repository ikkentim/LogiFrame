using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogiFrame
{
    internal partial class Simulation : Form
    {
        private Frame frame;

        public Simulation(Frame frame)
        {
            InitializeComponent();

            this.frame = frame;

            frame.Pushing += frame_Pushing;
            frame.FrameClosed += frame_FrameClosed;
            this.FormClosing += Simulation_FormClosing;
        }

        public static void Start(Frame frame)
        {
            Application.Run(new Simulation(frame));
        }

        void Simulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!frame.Disposed)
                frame.Dispose();
        }

        void frame_Pushing(object sender, PushingEventArgs e)
        {
            this.Invoke((MethodInvoker) delegate
            {
                this.Text = frame.ApplicationName + " (" + frame.UpdatePriority.ToString() + ")";
                displayPictureBox.Image = e.Frame;
            });
        }

        void frame_FrameClosed(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Buttons
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            frame.PerformButtonDown(0);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            frame.PerformButtonUp(0);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            frame.PerformButtonDown(1);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            frame.PerformButtonUp(1);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            frame.PerformButtonDown(2);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            frame.PerformButtonUp(2);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            frame.PerformButtonDown(3);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            frame.PerformButtonUp(3);
        }
        #endregion

        #region Keys
        private int buttonsDown;
        private void Simulation_KeyDown(object sender, KeyEventArgs e)
        {
            int button = -1;

            switch (e.KeyCode)
            {
                case Keys.D1:
                    button = 0;
                    break;
                case Keys.D2:
                    button = 1;
                    break;
                case Keys.D3:
                    button = 2;
                    break;
                case Keys.D4:
                    button = 3;
                    break;
            }

            if (button == -1)
                return;

            int power = (int)Math.Pow(2, button);

            //Isdown
            if ((buttonsDown & power) == power)
                return;

            buttonsDown = buttonsDown | power;
            frame.PerformButtonDown(button);
        }

        private void Simulation_KeyUp(object sender, KeyEventArgs e)
        {
            int button = -1;
            switch (e.KeyCode)
            {
                case Keys.D1:
                    button = 0;
                    break;
                case Keys.D2:
                    button = 1;
                    break;
                case Keys.D3:
                    button = 2;
                    break;
                case Keys.D4:
                    button = 3;
                    break;
            }

            if (button == -1)
                return;

            int power = (int)Math.Pow(2, button);

            if ((buttonsDown & power) == 0)
                return;

            frame.PerformButtonUp(button);

            buttonsDown = buttonsDown ^ power;
        }
        #endregion
    }
}
