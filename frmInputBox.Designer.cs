namespace OwOrdPad {
    partial class frmInputBox {
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
            panel1 = new Panel();
            txtInput = new TextBox();
            btnOK = new Button();
            labelMessage = new Label();
            btnOptions = new Button();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(btnOptions);
            panel1.Controls.Add(txtInput);
            panel1.Controls.Add(btnOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(375, 35);
            panel1.TabIndex = 2;
            // 
            // txtInput
            // 
            txtInput.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtInput.BackColor = Color.FromArgb(243, 243, 243);
            txtInput.BorderStyle = BorderStyle.None;
            txtInput.Location = new Point(12, 9);
            txtInput.Name = "txtInput";
            txtInput.Size = new Size(290, 16);
            txtInput.TabIndex = 2;
            txtInput.KeyDown += frmInputBox_KeyDown;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.ok;
            btnOK.Location = new Point(340, 0);
            btnOK.Margin = new Padding(0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 1;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // labelMessage
            // 
            labelMessage.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelMessage.Location = new Point(12, 9);
            labelMessage.Name = "labelMessage";
            labelMessage.Size = new Size(351, 70);
            labelMessage.TabIndex = 3;
            labelMessage.Text = "label1";
            // 
            // btnOptions
            // 
            btnOptions.Dock = DockStyle.Right;
            btnOptions.FlatAppearance.BorderSize = 0;
            btnOptions.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOptions.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOptions.FlatStyle = FlatStyle.Flat;
            btnOptions.Image = Properties.Resources.down;
            btnOptions.Location = new Point(305, 0);
            btnOptions.Margin = new Padding(0);
            btnOptions.Name = "btnOptions";
            btnOptions.Size = new Size(35, 35);
            btnOptions.TabIndex = 3;
            btnOptions.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnOptions.UseVisualStyleBackColor = true;
            btnOptions.Click += btnOptions_Click;
            // 
            // frmInputBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(375, 114);
            Controls.Add(labelMessage);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmInputBox";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmInputBox";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnOK;
        private Label labelMessage;
        private TextBox txtInput;
        private Button btnOptions;
    }
}