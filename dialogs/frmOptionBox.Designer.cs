namespace OwOrdPad {
    partial class frmOptionBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            flpOptions = new FlowLayoutPanel();
            panel1 = new Panel();
            btnOK = new Button();
            lblMessage = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flpOptions
            // 
            flpOptions.AutoSize = true;
            flpOptions.Dock = DockStyle.Fill;
            flpOptions.FlowDirection = FlowDirection.TopDown;
            flpOptions.Location = new Point(0, 25);
            flpOptions.Name = "flpOptions";
            flpOptions.Size = new Size(302, 304);
            flpOptions.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(btnOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 329);
            panel1.Name = "panel1";
            panel1.Size = new Size(302, 35);
            panel1.TabIndex = 6;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.ok;
            btnOK.Location = new Point(267, 0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 3;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Top;
            lblMessage.Location = new Point(0, 0);
            lblMessage.MaximumSize = new Size(302, 100);
            lblMessage.Name = "lblMessage";
            lblMessage.Padding = new Padding(5);
            lblMessage.Size = new Size(48, 25);
            lblMessage.TabIndex = 7;
            lblMessage.Text = "label1";
            // 
            // frmOptionBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(302, 364);
            Controls.Add(flpOptions);
            Controls.Add(lblMessage);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmOptionBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmOptionBox";
            Load += frmOptionBox_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flpOptions;
        private Panel panel1;
        private Button btnOK;
        private Label lblMessage;
    }
}