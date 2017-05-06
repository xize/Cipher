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
        private static Icon ico = new Window().Icon;

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
            item.Image = ico.ToBitmap();
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
            CipherEntry a = new CipherEntry();
            a.Show();
            ///Window window = new Window();
            ///window.Show();
        }

        public static Icon getIcon()
        {
            return ico;
        }
    }

    class CipherEntry : Form
    {

        private Label textboxtext;
        private TextBox phrase;
        private Panel panel;
        private Button nextbtn;

        public CipherEntry()
        {
            initialize();
        }

        private void initialize()
        {
            this.Height = 200;
            this.Width = 500;
            this.Text = "Cipher the password with a secret phrase!";
            this.Icon = CipherKeepassExt.getIcon();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.textboxtext = new Label();
            this.textboxtext.Text = "secret phrase: ";
            this.textboxtext.Dock = DockStyle.Left;

            this.phrase = new TextBox();
            this.phrase.PasswordChar = '*';
            this.phrase.Text = "password";
            this.phrase.BackColor = Color.White;
            this.phrase.Dock = DockStyle.Left;

            this.nextbtn = new Button();
            this.nextbtn.Text = "Next";
            this.nextbtn.Dock = DockStyle.Bottom;
            this.nextbtn.Click += new EventHandler(btnOnClick);

            this.panel = new Panel();
            this.panel.Left = 125;
            this.panel.Top = 50;
            this.panel.Height = 20;
            this.panel.Controls.Add(phrase);
            this.panel.Controls.Add(textboxtext);

            this.Controls.Add(panel);
            this.Controls.Add(nextbtn);

            this.SuspendLayout();
            this.ResumeLayout();
        }

        private void btnOnClick(Object sender, EventArgs e)
        {

            switch(nextbtn.Text)
            {
                case "next":
                    //make copy of secret phrase
                    string secret = nextbtn.Text;
                    string rawtext = "test";

                    break;
                case "back":

                    break;
                case "finish":
                    break;
            }
        }
    }
}
