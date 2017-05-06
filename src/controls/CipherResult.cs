/*
    Chiper - A program to chiper passwords based on rijndael encryption.

    Copyright(C) 2017 Guido Lucassen

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher
{
    public partial class CipherResult : UserControl
    {
        private CipherTimer timer = new CipherTimer();
        private CipherTask task;

        public CipherResult()
        {
            InitializeComponent();
            code.Click += new EventHandler(code_selection);
        }

        private void code_selection(object sender, EventArgs e)
        {
            Clipboard.SetText(code.Text);
            if (task != null)
            {
                task.run();
                task.stop();
                this.task = null;
            }

            this.task = new CipherTask(timer, 30);
            this.task.start();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }

    }

    class CipherTimer : CipherRunnable
    {
        public void run()
        {
            Clipboard.Clear();
        }
    }
}
