// LogiFrame
// Copyright (C) 2014 Tim Potze
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// 
// For more information, please refer to <http://unlicense.org>

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