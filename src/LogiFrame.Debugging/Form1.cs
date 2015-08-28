using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogiFrame.Debugging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitLayout();
            Controls.Add(null);
            ResumeLayout();
            SuspendLayout();
            InitializeComponent();
            Refresh();
        }

        #region Overrides of Form

        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data. </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        #endregion
    }
}
