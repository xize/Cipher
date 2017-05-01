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

namespace QRCipher
{
    public class QRCipherExt : Plugin
    {

        private IPluginHost m_host = null;
        private ToolStripMenuItem m_tsmiMenuItem;
        private ToolStripSeparator m_tsSeparator;

        public override bool Initialize(IPluginHost host)
        {
            this.m_host = host;
            // Get a reference to the 'Tools' menu item container
            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;

            this.m_tsSeparator = new ToolStripSeparator();
            tsMenu.Add(m_tsSeparator);

            // Add menu item 'Do Something'
            this.m_tsmiMenuItem = new ToolStripMenuItem();
            this.m_tsmiMenuItem.Text = "Cipher";
            this.m_tsmiMenuItem.Click += this.OnMenuDoSomething;
            tsMenu.Add(m_tsmiMenuItem);
            return true;
        }

        private void OnMenuDoSomething(object sender, EventArgs e)
        {
            Window window = new Window();
            window.Show();
        }

        public override void Terminate()
        {
            ToolStripItemCollection tsMenu = m_host.MainWindow.ToolsMenu.DropDownItems;
            m_tsmiMenuItem.Click -= this.OnMenuDoSomething;
            tsMenu.Remove(m_tsmiMenuItem);
            tsMenu.Remove(m_tsSeparator);
        }

    }
}
