// LogiFrame
// Copyright 2015 Tim Potze
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace LogiFrame
{
    internal partial class Simulation : Form
    {
        private readonly Frame _frame;

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
            if (!_frame.IsDisposed)
                _frame.Dispose();
        }

        private void frame_Pushing(object sender, PushingEventArgs e)
        {
            try
            {
                if (IsHandleCreated)
                    Invoke((MethodInvoker) delegate
                    {
                        displayGroupBox.Text = "Display (" + _frame.UpdatePriority + ")";
                        displayPictureBox.Image = e.Frame;
                    });
            }
            catch (Exception)
            {
            }
        }

        private void frame_FrameClosed(object sender, EventArgs e)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker) Close);
            else
                Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.ikkentim.com");
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