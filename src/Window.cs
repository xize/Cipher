using Cipher.src.encryption;
using Cipher.src.libs.QRnet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cipherResult1.code.Text = ""; //clear it.
            cipherResult1.code.Hide();
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                cipherResult1.qrpanel.BackgroundImage = QRCode.getFactory().generateQR(Crypto.Decrypt(textBox1.Text, textBox2.Text), 100, 100);
            } else
            {
                MessageBox.Show("both fields \"text\" and \"phrase\" needs to be filled in!", "Error!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cipherResult1.qrpanel.BackgroundImage = null;
            cipherResult1.code.Show();
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                string decrypted = Crypto.Decrypt(textBox1.Text, textBox2.Text);
                cipherResult1.code.Text = decrypted;
            } else
            {
                MessageBox.Show("both fields \"text\" and \"phrase\" needs to be filled in!", "Error!");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox1.PasswordChar = '*';
            } else
            {
                textBox1.PasswordChar = '\0';
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cipherResult1.qrpanel.BackgroundImage = null;
            cipherResult1.code.Show();
            if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                string encrypted = Crypto.Encrypt(textBox1.Text, textBox2.Text);
                cipherResult1.code.Text = encrypted;
            } else
            {
                MessageBox.Show("both fields \"text\" and \"phrase\" needs to be filled in!", "Error!");
            }
        }
    }
}
