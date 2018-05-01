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
using Cipher.src.libs.zxing;
using ZXing;
using ZXing.Common;
using KeePassLib.Interfaces;

namespace CipherKeepass
{
    public class CipherKeepassExt : Plugin
    {

        private IPluginHost pl = null;
        private ToolStripMenuItem menuitem;
        private ToolStripSeparator menuseperator;
        private static Icon ico = new Window().Icon;
        private ToolStripMenuItem decipher_pass;
        private ToolStripMenuItem decipher_as_qr;
        private ToolStripMenuItem cipher;
        private ToolStripMenuItem cipherren;
        private Cipher c;
        private DeCipher d;
        private DeCipherQR dq;

        public override bool Initialize(IPluginHost pl)
        {
            this.pl = pl;
            this.c = new Cipher(this.pl);
            this.d = new DeCipher(this.pl);
            this.dq = new DeCipherQR();

            c.FormClosing += new FormClosingEventHandler(OnCloseCipherWindow);
            d.FormClosing += new FormClosingEventHandler(OnCloseDecipherWindow);
            dq.FormClosing += new FormClosingEventHandler(OnCloseDecipherQRWindow);

            
            setupContextMenu();

            return true;
        }

        private void OnCloseDecipherQRWindow(object sender, FormClosingEventArgs e)
        {
            //cleanup everything.
            dq.Hide();
            e.Cancel = true;
        }

        private void OnCloseDecipherWindow(object sender, FormClosingEventArgs e)
        {
            d.phrase.Text = "";
            d.Hide();
            e.Cancel = true;
        }

        private void OnCloseCipherWindow(object sender, FormClosingEventArgs e)
        {
            c.Hide();
            e.Cancel = true;
        }

        private void setupContextMenu()
        {
            ToolStripItemCollection menu = pl.MainWindow.EntryContextMenu.Items;

            menu.Add(new ToolStripSeparator());

            this.cipher = new ToolStripMenuItem();
            cipher.Enabled = true;
            cipher.Text = "Cipher password";
            cipher.Click += new EventHandler(Cipher_clickEvent);
            cipher.Image = ico.ToBitmap();
            menu.Add(cipher);

            this.cipherren = new ToolStripMenuItem();
            cipherren.Enabled = true;
            cipherren.Text = "Change cipher";
            cipherren.Click += new EventHandler(CipherrenEvent);
            cipherren.Image = ico.ToBitmap();
            menu.Add(cipherren);

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

        public bool IsCiphered()
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry != null)
            {
                return KeepassHelper.GetKeepass(pl).IsTagged(entry);
            }
            return false;
        }

        public void SetCipherStatus()
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry != null)
            {
               entry.AddTag("cipher");
            }
        }

        public void RemoveCipherStatus()
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry != null)
            {
                if(entry.HasTag("cipher"))
                {
                    entry.RemoveTag("cipher");
                }
            }
        }

        private void CipherrenEvent(object sender, EventArgs args)
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }
            ProtectedString pass = entry.Strings.Get("Password");
            ValidateCipher val = new ValidateCipher(c, pass.ReadString());
            val.Show();
        }

        private void Cipher_clickEvent(object sender, EventArgs args)
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }
            ProtectedString pass = entry.Strings.Get("Password");
            Cipher cipherwindow = c;
            cipherwindow.SetPassword(pass.ReadString());
            cipherwindow.ShowDialog();
        }

        private void decipher_pass_ClickEvent(object sender, EventArgs args)
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if(entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }

            //ProtectedString user = entry.Strings.Get("UserName");
            ProtectedString pass = entry.Strings.Get("Password");

            DeCipher decipherwindow = d;
            decipherwindow.SetEncryptedPassword(pass.ReadString());
            decipherwindow.ShowDialog();
            
        }

        private void decipher_as_qr_ClickEvent(object sender, EventArgs args)
        {
            PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
            if (entry == null)
            {
                MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                return;
            }

            ProtectedString user = entry.Strings.Get("UserName");
            ProtectedString pass = entry.Strings.Get("Password");

            DeCipherQR decipherwindowqr = dq;
            decipherwindowqr.SetEncryptedPassword(pass.ReadString());
            decipherwindowqr.ShowDialog();
        }

        public override void Terminate()
        {
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

        public static Icon getIcon()
        {
            return ico;
        }
    }

    class DeCipher : Form
    {
        private IPluginHost pl;
        public TextBox phrase;
        private Button nextbtn;
        private Label label1;
        private string pass;
        private CheckBox checkBox1;

        public DeCipher(IPluginHost pl)
        {
            InitializeComponent();
            this.pl = pl;
            ToolTip tip = new ToolTip();
            tip.SetToolTip(checkBox1, "this will not update the phrase inside keepass, however it will be re-encrypted to make it look its changed");
            this.KeyDown += new KeyEventHandler(OnInvokeNextEvent);
            this.phrase.KeyDown += new KeyEventHandler(OnInvokeNextEvent);
        }

        private void OnInvokeNextEvent(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                nextbtn.PerformClick();
            }
        }

        public void SetEncryptedPassword(string pass)
        {
            this.pass = pass;
        }

        private async void btnOnClick(Object sender, EventArgs e)
        {
            string password = Crypto.Decrypt(pass, phrase.Text);
            string p = phrase.Text;
            if (password.Length == 0)
            {
                return;
            }
            this.DialogResult = DialogResult.OK;
            Clipboard.SetText(password, TextDataFormat.Text);
            MessageBox.Show("The password has been deciphered and added to the clipboard"+ (checkBox1.Checked ? ", make sure to save the database again since the entry has been changed!" : ""), "success!");

            if (checkBox1.Checked)
            {
                PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
                if (entry == null)
                {
                    MessageBox.Show("the selection is empty!", "error no entry has been selected!");
                }
                else
                {
                    //entry.Strings.Set("Password", new ProtectedString(true, Crypto.Encrypt(password, p)));
                    KeepassHelper.GetKeepass(pl).SetPassword(entry, password, p);
                }
            }

            await Task.Delay(18000);
            password = null;
            p = null;
            Clipboard.Clear();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.phrase = new System.Windows.Forms.TextBox();
            this.nextbtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "secret phrase:";
            // 
            // phrase
            // 
            this.phrase.Location = new System.Drawing.Point(13, 30);
            this.phrase.Name = "phrase";
            this.phrase.PasswordChar = '*';
            this.phrase.Size = new System.Drawing.Size(100, 20);
            this.phrase.TabIndex = 1;
            // 
            // nextbtn
            // 
            this.nextbtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.nextbtn.Location = new System.Drawing.Point(0, 64);
            this.nextbtn.Name = "nextbtn";
            this.nextbtn.Size = new System.Drawing.Size(212, 23);
            this.nextbtn.TabIndex = 2;
            this.nextbtn.Text = "decrypt";
            this.nextbtn.UseVisualStyleBackColor = true;
            this.nextbtn.Click += new System.EventHandler(this.btnOnClick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(119, 30);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(91, 17);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "update entry?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // DeCipher
            // 
            this.ClientSize = new System.Drawing.Size(212, 87);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.nextbtn);
            this.Controls.Add(this.phrase);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DeCipher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decrypt cipher";
            this.ResumeLayout(false);
            this.PerformLayout();

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

            if (pass.Length == 0)
            {
                return;
            }

            string formatQR = (!otpcheckbox.Checked ? Crypto.Decrypt(password, phrase.Text) : "otpauth://totp/" + servicetextbox.Text + ":" + accounttextbox.Text + "?secret=" + Crypto.Decrypt(password, phrase.Text) + "&issuer=" + servicetextbox.Text);
         
            this.picture.Image = generateQR(formatQR, 130, 130);
        }

        public Image generateQR(string text, int width, int height)
        {
            if (text.Length == 0)
            {
                return null;
            }
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };

            return writer.Write(text);
        }
    }

    class Cipher : Form
    {
        private Label label1;
        public TextBox textBox1;
        public TextBox textBox2;
        private Button button1;
        private Label label3;
        private Label label4;
        private Label label2;
        private string password;
        private IPluginHost pl;
        private ValidateAddedCipher validate;

        public Cipher(IPluginHost pl)
        {
            this.pl = pl;
            this.validate = new ValidateAddedCipher(this, pl);
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnInvokeNext);
            this.textBox1.KeyDown += new KeyEventHandler(OnInvokeNext);
            this.textBox2.KeyDown += new KeyEventHandler(OnInvokeNext);
            this.FormClosing += new FormClosingEventHandler(OnClose);
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.Hide();
            e.Cancel = true;
        }

        private void OnInvokeNext(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "phrase:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 66);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "retype phrase:";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Cipher password";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(116, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(118, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "X";
            // 
            // Cipher
            // 
            this.ClientSize = new System.Drawing.Size(143, 120);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Cipher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cipher password";
            this.Load += new System.EventHandler(this.Cipher_Load);
            this.Visible = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private bool IsMatched()
        {
            return textBox1.Text == textBox2.Text && textBox1.Text.Length > 0 && textBox2.Text.Length > 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (IsMatched())
            {
                label3.Text = "Y";
                label3.ForeColor = Color.Green;
                label4.Text = "Y";
                label4.ForeColor = Color.Green;
            }
            else
            {
                label3.Text = "X";
                label3.ForeColor = Color.Red;
                label4.Text = "X";
                label4.ForeColor = Color.Red;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (IsMatched())
            {
                label3.Text = "Y";
                label3.ForeColor = Color.Green;
                label4.Text = "Y";
                label4.ForeColor = Color.Green;
            }
            else
            {
                label3.Text = "X";
                label3.ForeColor = Color.Red;
                label4.Text = "X";
                label4.ForeColor = Color.Red;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsMatched())
            {
                //encrypt the phrase with the password as a cipher.
                string encrypted = Crypto.Encrypt(password, textBox1.Text);

                //now place the encrypted password inside the validate window.
                validate.SetCipher(encrypted);
                this.Visible = false;
                validate.Show();
            }
            else
            {
                MessageBox.Show("the secret phrases need to match or your decryption will fail!", "the phrases did not match!");
            }
            this.Visible = false;
            this.password = "";
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.DialogResult = DialogResult.OK;
        }

        private void Cipher_Load(object sender, EventArgs e)
        {

        }
    }

    class ValidateAddedCipher : Form
    {
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private string cipher;
        private Cipher cipherwindw;
        private IPluginHost pl;

        public ValidateAddedCipher(Cipher win, IPluginHost pl)
        {
            this.cipherwindw = win;
            this.FormClosing += new FormClosingEventHandler(OnValidateAddedCipherCloseEvent);
            this.pl = pl;
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnInvokeNext);
            textBox1.KeyDown += new KeyEventHandler(OnInvokeNext);
            this.FormClosing += new FormClosingEventHandler(OnClose);
        }

        private void OnClose(object sender, FormClosingEventArgs e)
        {
            this.textBox1.Text = "";
            this.Hide();
            e.Cancel = true;
        }

        private void OnInvokeNext(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void OnValidateAddedCipherCloseEvent(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }

        public void SetCipher(string cipher)
        {
            this.cipher = cipher;
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "test decryption";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(119, 20);
            this.textBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "re-type phrase:";
            // 
            // ValidateCipher
            // 
            this.ClientSize = new System.Drawing.Size(143, 85);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ValidateCipher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cipher password";
            this.Visible = false;
            this.Load += new System.EventHandler(this.Validate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Validate_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string decrypted = Crypto.Decrypt(this.cipher, textBox1.Text);
            if (decrypted != null && decrypted.Length > 0)
            {
                //success
                PwEntry entry = pl.MainWindow.GetSelectedEntry(true);
                if (entry != null)
                {
                    MessageBox.Show("updated password with cipher!\n\nplease make sure you save it since we have not figured out to tell keepass that the db is changed", "success!");
                    //entry.Strings.Set("Password", new ProtectedString(true, cipher));
                    KeepassHelper.GetKeepass(pl).SetPassword(entry, decrypted, textBox1.Text);
                    this.Visible = false;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("no password entry has been checked!", "error!");
                }
            }
            else
            {
                MessageBox.Show("failed to decrypt password, please redo the phrases!", "error!");
                cipherwindw.Visible = true;
            }
            this.cipher = "";
            this.Visible = false;
        }
    }

    class ValidateCipher : Form
    {
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private string ciphertext;
        private Cipher cipher;

        public ValidateCipher(Cipher cipher, string ciphertext)
        {
            this.cipher = cipher;
            this.ciphertext = ciphertext;
            InitializeComponent();
            this.textBox1.KeyDown += new KeyEventHandler(OnInvokeNext);
            this.KeyDown += new KeyEventHandler(OnInvokeNext);
        }

        private void OnInvokeNext(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(143, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "decrypt!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(119, 20);
            this.textBox1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "type your phrase:";
            // 
            // ValidateCipher
            // 
            this.ClientSize = new System.Drawing.Size(143, 85);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ValidateCipher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cipher password";
            this.Visible = false;
            this.Load += new System.EventHandler(this.Validate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Validate_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string decrypted = Crypto.Decrypt(ciphertext, textBox1.Text);
            if(decrypted != null && decrypted.Length > 0)
            {
                cipher.SetPassword(decrypted);
                cipher.Show();
                this.Visible = false;
            } else
            {
                MessageBox.Show("could not decrypt the password, have you tried to type the cipher correctly?", "error!");
            }
        }
    }

    class KeepassHelper
    {
        private static KeepassHelper helper;
        private IPluginHost pl;

        private KeepassHelper(IPluginHost pl) {
            this.pl = pl;
        }

        public void SetPassword(PwEntry entry, string password, string phrase)
        {
            PwDatabase db = pl.Database;
            entry.CreateBackup();
            entry.AddTag("cipher");
            entry.Strings.Set("Password", new ProtectedString(true, Crypto.Encrypt(password, phrase)));
            db.Modified = true;
            pl.MainWindow.UpdateUI(false, null, false, null, false, null, true);
        }

        public bool IsTagged(PwEntry entry)
        {
            return entry.HasTag("cipher");
        }

        public void RemoveTag(PwEntry entry)
        {
            entry.RemoveTag("cipher");
        }

        public static KeepassHelper GetKeepass(IPluginHost pl)
        {
            if(helper == null)
            {
                helper = new KeepassHelper(pl);
            }
            return helper;
        }
    }
}
