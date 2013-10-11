using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogiFrame
{
    internal partial class Simulation : Form
    {
        private Frame _frame;
        private int _buttonsDown;

        public Simulation(Frame frame)
        {
            InitializeComponent();

            this._frame = frame;

            frame.Pushing += frame_Pushing;
            frame.FrameClosed += frame_FrameClosed;
            FormClosing += Simulation_FormClosing;
        }

        public static void Start(Frame frame)
        {
            Application.Run(new Simulation(frame));
        }

        void Simulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_frame.Disposed)
                _frame.Dispose();
        }

        void frame_Pushing(object sender, PushingEventArgs e)
        {
            Invoke((MethodInvoker) delegate
            {
                Text = _frame.ApplicationName + " (" + _frame.UpdatePriority.ToString() + ")";
                displayPictureBox.Image = e.Frame;
            });
        }

        void frame_FrameClosed(object sender, EventArgs e)
        {
            Close();
        }

        #region Buttons
        private void button1_MouseDown(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonDown(0);
        }

        private void button1_MouseUp(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonUp(0);
        }

        private void button2_MouseDown(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonDown(1);
        }

        private void button2_MouseUp(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonUp(1);
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonDown(2);
        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonUp(2);
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonDown(3);
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            _frame.PerformButtonUp(3);
        }
        #endregion

        #region Keys
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
            if ((_buttonsDown & power) == power)
                return;

            _buttonsDown = _buttonsDown | power;
            _frame.PerformButtonDown(button);
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

            if ((_buttonsDown & power) == 0)
                return;

            _frame.PerformButtonUp(button);

            _buttonsDown = _buttonsDown ^ power;
        }
        #endregion
    }
}
