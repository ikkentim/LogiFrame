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
using System.Linq;
using System.Windows.Forms;

namespace CoD4
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();

            //Add initial range
            listBox.Items.AddRange(ServerManager.Servers.ToArray());
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            //Close form
            Close();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //Cast to Server
            var server = listBox.SelectedItem as Server;
            if (server == null) return;

            //Remove
            ServerManager.Remove(server);

            //Rebuild
            listBox.Items.Clear();
            listBox.Items.AddRange(ServerManager.Servers.ToArray());
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            //Create a AddForm to ask for an ip
            var form = new AddForm();
            if (form.ShowDialog() != DialogResult.OK) return;

            //Add
            ServerManager.Add(form.Server);

            //Rebuild
            listBox.Items.Clear();
            listBox.Items.AddRange(ServerManager.Servers.ToArray());
        }
    }
}