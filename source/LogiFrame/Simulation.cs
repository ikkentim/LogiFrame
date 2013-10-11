using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogiFrame
{
    internal partial class Simulation : Form
    {
        private Frame _frame;

        private Simulation(Frame frame)
        {
            InitializeComponent();

            propertyGrid.SelectedObject = frame;

            _frame = frame;

            frame.Pushing += frame_Pushing;
            frame.FrameClosed += frame_FrameClosed;
            FormClosing += Simulation_FormClosing;

            displayPictureBox.Image = _frame.Bytemap;
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
                Text = _frame.ApplicationName + " (" + _frame.UpdatePriority + ")";
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
    }
}
