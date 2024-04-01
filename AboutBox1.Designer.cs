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
            logoPictureBox = new PictureBox();
            textBoxDescription = new TextBox();
            okButton = new Button();
            labelProductName = new TextBox();
            labelVersion = new TextBox();
            labelCopyright = new TextBox();
            labelCompanyName = new TextBox();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            SuspendLayout();
            // 
            // logoPictureBox
            // 
            logoPictureBox.Dock = DockStyle.Left;
            logoPictureBox.Image = Properties.Resources.OwOrdPadBanner;
            logoPictureBox.Location = new Point(0, 0);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(276, 319);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 12;
            logoPictureBox.TabStop = false;
            // 
            // textBoxDescription
            // 
            textBoxDescription.BackColor = Color.White;
            textBoxDescription.Location = new Point(286, 128);
            textBoxDescription.Margin = new Padding(7, 3, 4, 3);
            textBoxDescription.Multiline = true;
            textBoxDescription.Name = "textBoxDescription";
            textBoxDescription.ReadOnly = true;
            textBoxDescription.ScrollBars = ScrollBars.Both;
            textBoxDescription.Size = new Size(325, 179);
            textBoxDescription.TabIndex = 23;
            textBoxDescription.TabStop = false;
            textBoxDescription.Text = "Descrição";
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(1139, 399);
            okButton.Margin = new Padding(4, 3, 4, 3);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 24;
            okButton.Text = "&OK";
            // 
            // labelProductName
            // 
            labelProductName.BackColor = Color.White;
            labelProductName.Location = new Point(286, 12);
            labelProductName.Name = "labelProductName";
            labelProductName.ReadOnly = true;
            labelProductName.Size = new Size(325, 23);
            labelProductName.TabIndex = 25;
            labelProductName.Text = "Nome do Produto";
            // 
            // labelVersion
            // 
            labelVersion.BackColor = Color.White;
            labelVersion.Location = new Point(286, 41);
            labelVersion.Name = "labelVersion";
            labelVersion.ReadOnly = true;
            labelVersion.Size = new Size(325, 23);
            labelVersion.TabIndex = 26;
            labelVersion.Text = "Versão";
            // 
            // labelCopyright
            // 
            labelCopyright.BackColor = Color.White;
            labelCopyright.Location = new Point(286, 70);
            labelCopyright.Name = "labelCopyright";
            labelCopyright.ReadOnly = true;
            labelCopyright.Size = new Size(325, 23);
            labelCopyright.TabIndex = 27;
            labelCopyright.Text = "Copyright";
            // 
            // labelCompanyName
            // 
            labelCompanyName.BackColor = Color.White;
            labelCompanyName.Location = new Point(286, 99);
            labelCompanyName.Name = "labelCompanyName";
            labelCompanyName.ReadOnly = true;
            labelCompanyName.Size = new Size(325, 23);
            labelCompanyName.TabIndex = 28;
            labelCompanyName.Text = "Nome da Companhia";
            // 
            // AboutBox1
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(624, 319);
            Controls.Add(labelCompanyName);
            Controls.Add(labelCopyright);
            Controls.Add(labelVersion);
            Controls.Add(labelProductName);
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
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Button okButton;
        private TextBox labelProductName;
        private TextBox labelVersion;
        private TextBox labelCopyright;
        private TextBox labelCompanyName;
    }
}
