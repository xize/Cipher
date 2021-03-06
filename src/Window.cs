﻿using Cipher.src.encryption;
using Cipher.src.libs.zxing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher.src
{
    public partial class Window : Form
    {
        private QRPrompt qrprompt;

        public Window()
        {
            InitializeComponent();
            if(this.qrprompt == null)
            {
                this.qrprompt = new QRPrompt();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cipherResult1.code.Text = ""; //clear it.
            cipherResult1.code.Hide();

            DialogResult r = qrprompt.ShowDialog();
            if(r == DialogResult.OK)
            {
                string formatQR = (qrprompt.GetAccount() == null ? Crypto.Decrypt(textBox1.Text, textBox2.Text) : "otpauth://totp/" + qrprompt.GetServiceName() + ":" + qrprompt.GetAccount() + "?secret="+ Crypto.Decrypt(textBox1.Text, textBox2.Text) + "&issuer=" + qrprompt.GetServiceName());

                if (textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
                {
                    cipherResult1.qrpanel.BackgroundImage = QRCode.getFactory().generateQR(formatQR, 130, 130);
                }
                else
                {
                    MessageBox.Show("both fields \"text\" and \"phrase\" needs to be filled in!", "Error!");
                }
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

        private void Window_Load(object sender, EventArgs e)
        {

        }
    }
}
