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
using System.Drawing;
using Cipher.src.encryption;
using Cipher.src;
using KeePassLib;
using KeePassLib.Collections;
using KeePassLib.Security;
using System.IO;

namespace CipherKeepass
{
    public class CipherKeepassExt : Plugin
    {

        private IPluginHost pl = null;
        private ToolStripMenuItem menuitem;
        private ToolStripSeparator menuseperator;
        private static Icon ico = new Window().Icon;
        private DeCipher decipherwindow;
        private DeCipherQR decipherwindowqr;

        private ToolStripMenuItem decipher_pass;
        private ToolStripMenuItem decipher_as_qr;

        public override bool Initialize(IPluginHost pl)
        {
            this.pl = pl;

            this.decipherwindow = new DeCipher();
            this.decipherwindowqr = new DeCipherQR();

            setupContextMenu();

            object[] toolstripdata = addMenuItem("Cipher", OnMenuDoSomething);

            this.menuitem = (ToolStripMenuItem)toolstripdata[1];
            this.menuseperator = (ToolStripSeparator)toolstripdata[0];

            return true;
        }

        private void setupContextMenu()
        {
            ToolStripItemCollection menu = this.pl.MainWindow.EntryContextMenu.Items;

            menu.Add(new ToolStripSeparator());
            this.decipher_pass = new ToolStripMenuItem();
            decipher_pass.Enabled = true;
            decipher_pass.Text = "Decipher password";
            decipher_pass.Click += new EventHandler(decipher_pass_ClickEvent);
            decipher_pass.Image = ico.ToBitmap();
            menu.Add(decipher_pass);

            this.decipher_as_qr = new ToolStripMenuItem();
            decipher_as_qr.Enabled = true;
            decipher_as_qr.Text = "Decipher as qr";
            decipher_as_qr.Click += new EventHandler(decipher_as_qr_ClickEvent);
            decipher_as_qr.Image = ico.ToBitmap();
            menu.Add(decipher_as_qr);
        }

        private void decipher_pass_ClickEvent(object sender, EventArgs args)
        {
            PwEntry entry = this.pl.MainWindow.GetSelectedEntry(true);
            if(entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }

            //ProtectedString user = entry.Strings.Get("UserName");
            ProtectedString pass = entry.Strings.Get("Password");

            decipherwindow.SetEncryptedPassword(pass.ReadString());
            decipherwindow.ShowDialog();
            
        }

        private void decipher_as_qr_ClickEvent(object sender, EventArgs args)
        {
            PwEntry entry = this.pl.MainWindow.GetSelectedEntry(true);
            if (entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }

            ProtectedString user = entry.Strings.Get("UserName");
            ProtectedString pass = entry.Strings.Get("Password");

            decipherwindowqr.SetEncryptedPassword(pass.ReadString());
            decipherwindowqr.ShowDialog();
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
            Window window = new Window();
            window.Show();
        }

        public static Icon getIcon()
        {
            return ico;
        }
    }

    class DeCipher : Form
    {

        private Label textboxtext;
        public TextBox phrase;
        private Panel panel;
        private Button nextbtn;
        private string pass;

        public DeCipher()
        {
            initialize();
        }

        private void initialize()
        {
            this.Height = 200;
            this.Width = 500;
            this.Text = "fill in your secret phrase in order to decrypt!";
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
            this.nextbtn.Text = "decrypt!";
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

        public void SetEncryptedPassword(string pass)
        {
            this.pass = pass;
        }

        private async void btnOnClick(Object sender, EventArgs e)
        {
            string password = Crypto.Decrypt(pass, phrase.Text);
            if(password.Length == 0)
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            Clipboard.SetText(password, TextDataFormat.Text);
            MessageBox.Show("The password has been deciphered and added to the clipboard", "success!");
            await Task.Delay(18000);
            Clipboard.Clear();
        }
    }



    class DeCipherQR : Form
    {
        private TextBox phrase;
        private Label label1;
        private Panel panel2;
        private PictureBox picture;
        private CheckBox otpcheckbox;
        private Label servicelabel;
        private TextBox servicetextbox;
        private TextBox accounttextbox;
        private Label accountlabel;
        private Button generate;
        private Panel panel1;
        private string password;

        public DeCipherQR()
        {
            InitializeComponent();
            this.servicetextbox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
            this.accounttextbox.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
        }

        public void SetEncryptedPassword(string password)
        {
            this.password = password;
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.otpcheckbox = new System.Windows.Forms.CheckBox();
            this.phrase = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picture = new System.Windows.Forms.PictureBox();
            this.servicelabel = new System.Windows.Forms.Label();
            this.servicetextbox = new System.Windows.Forms.TextBox();
            this.accounttextbox = new System.Windows.Forms.TextBox();
            this.accountlabel = new System.Windows.Forms.Label();
            this.generate = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.picture);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 250);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.DimGray;
            this.panel2.Controls.Add(this.otpcheckbox);
            this.panel2.Controls.Add(this.phrase);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(462, 36);
            this.panel2.TabIndex = 4;
            // 
            // otpcheckbox
            // 
            this.otpcheckbox.AutoSize = true;
            this.otpcheckbox.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.otpcheckbox.Location = new System.Drawing.Point(370, 11);
            this.otpcheckbox.Name = "otpcheckbox";
            this.otpcheckbox.Size = new System.Drawing.Size(81, 17);
            this.otpcheckbox.TabIndex = 1;
            this.otpcheckbox.Text = "OTP code?";
            this.otpcheckbox.UseVisualStyleBackColor = true;
            this.otpcheckbox.CheckedChanged += new System.EventHandler(this.otpcheckbox_CheckedChanged);
            // 
            // phrase
            // 
            this.phrase.Location = new System.Drawing.Point(88, 9);
            this.phrase.Name = "phrase";
            this.phrase.PasswordChar = '*';
            this.phrase.Size = new System.Drawing.Size(111, 20);
            this.phrase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "secret phrase:";
            // 
            // picture
            // 
            this.picture.Location = new System.Drawing.Point(160, 72);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(130, 130);
            this.picture.TabIndex = 3;
            this.picture.TabStop = false;
            // 
            // servicelabel
            // 
            this.servicelabel.AutoSize = true;
            this.servicelabel.Enabled = false;
            this.servicelabel.Location = new System.Drawing.Point(11, 257);
            this.servicelabel.Name = "servicelabel";
            this.servicelabel.Size = new System.Drawing.Size(73, 13);
            this.servicelabel.TabIndex = 1;
            this.servicelabel.Text = "service name:";
            // 
            // servicetextbox
            // 
            this.servicetextbox.Enabled = false;
            this.servicetextbox.Location = new System.Drawing.Point(11, 274);
            this.servicetextbox.Name = "servicetextbox";
            this.servicetextbox.Size = new System.Drawing.Size(100, 20);
            this.servicetextbox.TabIndex = 2;
            // 
            // accounttextbox
            // 
            this.accounttextbox.Enabled = false;
            this.accounttextbox.Location = new System.Drawing.Point(117, 274);
            this.accounttextbox.Name = "accounttextbox";
            this.accounttextbox.Size = new System.Drawing.Size(100, 20);
            this.accounttextbox.TabIndex = 3;
            // 
            // accountlabel
            // 
            this.accountlabel.AutoSize = true;
            this.accountlabel.Enabled = false;
            this.accountlabel.Location = new System.Drawing.Point(114, 257);
            this.accountlabel.Name = "accountlabel";
            this.accountlabel.Size = new System.Drawing.Size(78, 13);
            this.accountlabel.TabIndex = 4;
            this.accountlabel.Text = "account/email:";
            // 
            // generate
            // 
            this.generate.Location = new System.Drawing.Point(376, 266);
            this.generate.Name = "generate";
            this.generate.Size = new System.Drawing.Size(75, 23);
            this.generate.TabIndex = 5;
            this.generate.Text = "generate";
            this.generate.UseVisualStyleBackColor = true;
            this.generate.Click += new System.EventHandler(this.generate_Click);
            // 
            // QR
            // 
            this.ClientSize = new System.Drawing.Size(462, 301);
            this.Controls.Add(this.generate);
            this.Controls.Add(this.accountlabel);
            this.Controls.Add(this.accounttextbox);
            this.Controls.Add(this.servicetextbox);
            this.Controls.Add(this.servicelabel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(478, 340);
            this.MinimumSize = new System.Drawing.Size(478, 340);
            this.Name = "QR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decrypt Cipher as QR code!";
            this.Icon = CipherKeepassExt.getIcon();
            this.Load += new System.EventHandler(this.QR_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void QR_Load(object sender, EventArgs e)
        {

        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void otpcheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (otpcheckbox.Checked)
            {
                servicelabel.Enabled = true;
                servicetextbox.Enabled = true;
                accountlabel.Enabled = true;
                accounttextbox.Enabled = true;
            }
            else
            {
                servicelabel.Enabled = false;
                servicetextbox.Enabled = false;
                accountlabel.Enabled = false;
                accounttextbox.Enabled = false;
            }
        }

        private void generate_Click(object sender, EventArgs e)
        {

            if (phrase.Text.Length == 0)
            {
                MessageBox.Show("you need to use a secret phrase in order to decipher the password!", "error!");
                return;
            }

            string pass = Crypto.Decrypt(password, this.phrase.Text);

            if (otpcheckbox.Checked)
            {

            }
            else
            {

            }
        }
    }


}
