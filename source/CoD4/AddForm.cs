// LogiFrame rendering library.
// Copyright (C) 2014 Tim Potze
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>. 

using System;
using System.Windows.Forms;

namespace CoD4
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the entered Server.
        /// </summary>
        public Server Server { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            //Check valid length, contains one ':'
            short port;
            if (textBox1.Text.Length <= 10 || textBox1.Text.Split(':').Length != 2 ||
                !short.TryParse(textBox1.Text.Split(':')[1], out port))
            {
                MessageBox.Show("Invalid IP address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check connection
            var ip = textBox1.Text.Split(':')[0];
            var server = new Server(ip, port);
            if (server.Query() == null)
            {
                MessageBox.Show("Could not connect to IP address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Respond
            Server = server;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}