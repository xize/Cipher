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
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace Cipher.src.controls
{
    public partial class CipherResult : UserControl
    {

        private volatile string codea;

        public CipherResult()
        {
            InitializeComponent();
            code.Click += new EventHandler(code_selectionAsync);
        }

        private async void code_selectionAsync(object sender, EventArgs e)
        {
            if (code.Text.Length == 0)
            {
                return;
            }

            this.codea = code.Text;

            Clipboard.SetText(codea);
            await Task.Delay(5000);
            Clipboard.Clear();

        }

        private void clear(object o)
        {
            Clipboard.Clear();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void code_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
