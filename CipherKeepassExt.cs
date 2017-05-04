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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeePass.Plugins;
using System.Windows.Forms;
using Cipher;
using System.Drawing;

namespace CipherKeepass
{
    public class CipherKeepassExt : Plugin
    {

        private IPluginHost pl = null;
        private ToolStripMenuItem menuitem;
        private ToolStripSeparator menuseperator;

        public override bool Initialize(IPluginHost pl)
        {
            this.pl = pl;
            //this.pl.MainWindow.EntryContextMenu.BackColor = Color.Black;

            object[] toolstripdata = addMenuItem("Cipher", OnMenuDoSomething);

            this.menuitem = (ToolStripMenuItem)toolstripdata[1];
            this.menuseperator = (ToolStripSeparator)toolstripdata[0];

            return true;
        }

        public override void Terminate()
        {
            removeMenuItem(menuitem, menuseperator, OnMenuDoSomething);
        }

        private object[] addMenuItem(string name, EventHandler e)
        {
            ToolStripItemCollection menu = pl.MainWindow.ToolsMenu.DropDownItems;
            ToolStripSeparator seperator = new ToolStripSeparator();
            menu.Add(seperator);
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Text = name;
            item.Click += e;
            item.Image = new Window().Icon.ToBitmap();
            menu.Add(item);
            return new object[] { seperator, item };
        }

        private void removeMenuItem(ToolStripMenuItem item, ToolStripSeparator seperator, EventHandler e)
        {
            ToolStripItemCollection menu = pl.MainWindow.ToolsMenu.DropDownItems;
            item.Click -= e;
            menu.Remove(seperator);
            menu.Remove(item);
        }

        private void OnMenuDoSomething(object sender, EventArgs e)
        {
            Window window = new Window();
            window.Show();
        }
    }
}
