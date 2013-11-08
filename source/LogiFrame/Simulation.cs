//     Simulation.cs
// 
//     LogiFrame rendering library.
//     Copyright (C) 2013  Tim Potze
// 
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
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

        private void Simulation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_frame.Disposed)
                _frame.Dispose();
        }

        private void frame_Pushing(object sender, PushingEventArgs e)
        {
            if (this.IsHandleCreated)
                Invoke((MethodInvoker)delegate
                {
                    Text = _frame.ApplicationName + " (" + _frame.UpdatePriority + ")";
                    displayPictureBox.Image = e.Frame;
                });
        }

        private void frame_FrameClosed(object sender, EventArgs e)
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