namespace OwOrdPad {
    partial class AboutBox1 {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent() {
            components = new System.ComponentModel.Container();
            logoPictureBox = new PictureBox();
            textBoxDescription = new TextBox();
            okButton = new Button();
            labelVersion = new TextBox();
            panel1 = new Panel();
            btnGitHub = new Button();
            btnCopy = new Button();
            btnOK = new Button();
            toolTip1 = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // logoPictureBox
            // 
            logoPictureBox.BackColor = Color.FromArgb(243, 243, 243);
            logoPictureBox.Dock = DockStyle.Top;
            logoPictureBox.Image = Properties.Resources.logo;
            logoPictureBox.Location = new Point(0, 0);
            logoPictureBox.Margin = new Padding(0);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(451, 225);
            logoPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            logoPictureBox.TabIndex = 12;
            logoPictureBox.TabStop = false;
            // 
            // textBoxDescription
            // 
            textBoxDescription.BackColor = Color.White;
            textBoxDescription.BorderStyle = BorderStyle.None;
            textBoxDescription.Location = new Point(0, 225);
            textBoxDescription.Margin = new Padding(7, 0, 4, 3);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.ScrollBars = ScrollBars.Both;
            textBoxDescription.Size = new Size(451, 311);
            textBoxDescription.TabIndex = 23;
            textBoxDescription.TabStop = false;
            textBoxDescription.Text = "Description";
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(966, 657);
            okButton.Margin = new Padding(4, 3, 4, 3);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 24;
            okButton.Text = "&OK";
            // 
            // labelVersion
            // 
            labelVersion.BackColor = Color.FromArgb(243, 243, 243);
            labelVersion.BorderStyle = BorderStyle.None;
            labelVersion.Location = new Point(12, 12);
            labelVersion.Name = "labelVersion";
            labelVersion.ReadOnly = true;
            labelVersion.Size = new Size(96, 16);
            labelVersion.TabIndex = 26;
            labelVersion.Text = "Version";
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(btnGitHub);
            panel1.Controls.Add(btnCopy);
            panel1.Controls.Add(btnOK);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 542);
            panel1.Name = "panel1";
            panel1.Size = new Size(451, 35);
            panel1.TabIndex = 27;
            // 
            // btnGitHub
            // 
            btnGitHub.Dock = DockStyle.Left;
            btnGitHub.FlatAppearance.BorderSize = 0;
            btnGitHub.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnGitHub.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnGitHub.FlatStyle = FlatStyle.Flat;
            btnGitHub.Image = Properties.Resources.newWindow;
            btnGitHub.Location = new Point(35, 0);
            btnGitHub.Margin = new Padding(0);
            btnGitHub.Name = "btnGitHub";
            btnGitHub.Size = new Size(35, 35);
            btnGitHub.TabIndex = 4;
            btnGitHub.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnGitHub, "Open OwOrdPad's GitHub page");
            btnGitHub.UseVisualStyleBackColor = true;
            btnGitHub.Click += btnGitHub_Click;
            // 
            // btnCopy
            // 
            btnCopy.Dock = DockStyle.Left;
            btnCopy.FlatAppearance.BorderSize = 0;
            btnCopy.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnCopy.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Image = Properties.Resources.copy;
            btnCopy.Location = new Point(0, 0);
            btnCopy.Margin = new Padding(0);
            btnCopy.Name = "btnCopy";
            btnCopy.Size = new Size(35, 35);
            btnCopy.TabIndex = 3;
            btnCopy.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnCopy, "Copy description to the clipboard");
            btnCopy.UseVisualStyleBackColor = true;
            btnCopy.Click += btnCopy_Click;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.ok;
            btnOK.Location = new Point(416, 0);
            btnOK.Margin = new Padding(0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 2;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnOK, "Close this dialog");
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // AboutBox1
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            CancelButton = okButton;
            ClientSize = new Size(451, 577);
            Controls.Add(panel1);
            Controls.Add(labelVersion);
            Controls.Add(logoPictureBox);
            Controls.Add(okButton);
            Controls.Add(textBoxDescription);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutBox1";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About OwOrdPad";
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button okButton;
        private TextBox labelVersion;
        private Panel panel1;
        private Button btnOK;
        private Button btnCopy;
        private Button btnGitHub;
        private ToolTip toolTip1;
    }
}
