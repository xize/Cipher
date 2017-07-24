using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cipher.src
{
    public class QRPrompt : Form
    {
        private TextBox textBox1;
        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
        private string account = null;
        private string servicename = null;
        private CheckBox checkBox1;

        public QRPrompt()
        {
            InitializeComponent();
            this.textBox2.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
            this.textBox1.KeyPress += new KeyPressEventHandler(TextBox_KeyPress);
        }

        public string GetServiceName()
        {
            return this.servicename;
        }

        public string GetAccount()
        {
            return this.account;
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 68);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(260, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "account name or email:";
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 122);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(284, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "name of service:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 29);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(260, 20);
            this.textBox2.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 99);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "is OTP secret?";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // QRPrompt
            // 
            this.ClientSize = new System.Drawing.Size(284, 145);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "QRPrompt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "add QR account name:";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.account = null;
            this.servicename = null;

            if (this.checkBox1.Checked)
            {
                this.servicename = textBox2.Text;
                this.account = textBox1.Text;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == (char)Keys.Space);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == false)
            {
                textBox1.Enabled = false;
                textBox2.Enabled = false;
            } else
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
            }
        }
    }
}
