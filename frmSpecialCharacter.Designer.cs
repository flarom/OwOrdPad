namespace OwOrdPad {
    partial class frmSpecialCharacter {
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
            components = new System.ComponentModel.Container();
            categoryListBox = new ListBox();
            charListView = new ListView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            insertToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // categoryListBox
            // 
            categoryListBox.BackColor = Color.FromArgb(243, 243, 243);
            categoryListBox.BorderStyle = BorderStyle.None;
            categoryListBox.Dock = DockStyle.Left;
            categoryListBox.ForeColor = Color.Black;
            categoryListBox.FormattingEnabled = true;
            categoryListBox.ItemHeight = 15;
            categoryListBox.Location = new Point(0, 0);
            categoryListBox.Name = "categoryListBox";
            categoryListBox.Size = new Size(221, 364);
            categoryListBox.Sorted = true;
            categoryListBox.TabIndex = 0;
            categoryListBox.SelectedIndexChanged += categoryListBox_SelectedIndexChanged;
            categoryListBox.KeyDown += frmSpecialCharacter_KeyDown;
            // 
            // charListView
            // 
            charListView.Activation = ItemActivation.OneClick;
            charListView.Alignment = ListViewAlignment.Left;
            charListView.BorderStyle = BorderStyle.None;
            charListView.ContextMenuStrip = contextMenuStrip1;
            charListView.Dock = DockStyle.Fill;
            charListView.Font = new Font("Segoe UI Symbol", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            charListView.ImeMode = ImeMode.NoControl;
            charListView.Location = new Point(221, 0);
            charListView.MultiSelect = false;
            charListView.Name = "charListView";
            charListView.Size = new Size(629, 364);
            charListView.TabIndex = 1;
            charListView.UseCompatibleStateImageBehavior = false;
            charListView.KeyDown += frmSpecialCharacter_KeyDown;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.BackColor = Color.White;
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { copyToolStripMenuItem, insertToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.RenderMode = ToolStripRenderMode.System;
            contextMenuStrip1.Size = new Size(104, 48);
            contextMenuStrip1.Opening += contextMenuStrip1_Opening;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Image = Properties.Resources.copy;
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(103, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // insertToolStripMenuItem
            // 
            insertToolStripMenuItem.Image = Properties.Resources.pencil;
            insertToolStripMenuItem.Name = "insertToolStripMenuItem";
            insertToolStripMenuItem.Size = new Size(103, 22);
            insertToolStripMenuItem.Text = "Insert";
            insertToolStripMenuItem.Click += charListView_ItemActivate;
            // 
            // frmSpecialCharacter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(850, 364);
            Controls.Add(charListView);
            Controls.Add(categoryListBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(100, 100);
            Name = "frmSpecialCharacter";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Special Characters - OwOrdPad";
            TopMost = true;
            Load += frmSpecialCharacter_Load;
            KeyDown += frmSpecialCharacter_KeyDown;
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox categoryListBox;
        private ListView charListView;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem insertToolStripMenuItem;
    }
}