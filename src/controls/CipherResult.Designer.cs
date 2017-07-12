namespace Cipher.src.controls
{
    partial class CipherResult
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.qrpanel = new System.Windows.Forms.Panel();
            this.code = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.qrpanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DimGray;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.ForeColor = System.Drawing.Color.DimGray;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 213);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.qrpanel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 16);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 197);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // qrpanel
            // 
            this.qrpanel.Controls.Add(this.code);
            this.qrpanel.Location = new System.Drawing.Point(71, 35);
            this.qrpanel.Name = "qrpanel";
            this.qrpanel.Size = new System.Drawing.Size(130, 130);
            this.qrpanel.TabIndex = 0;
            // 
            // code
            // 
            this.code.BackColor = System.Drawing.Color.Black;
            this.code.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.code.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code.ForeColor = System.Drawing.Color.White;
            this.code.Location = new System.Drawing.Point(-3, 55);
            this.code.Name = "code";
            this.code.ReadOnly = true;
            this.code.Size = new System.Drawing.Size(130, 20);
            this.code.TabIndex = 0;
            this.code.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.code.TextChanged += new System.EventHandler(this.code_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "result:";
            // 
            // CipherResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "CipherResult";
            this.Size = new System.Drawing.Size(284, 213);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.qrpanel.ResumeLayout(false);
            this.qrpanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox code;
        public System.Windows.Forms.Panel qrpanel;
    }
}
