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
            panel1 = new Panel();
            txtOutput = new TextBox();
            btnUndo = new Button();
            btnOK = new Button();
            btnClear = new Button();
            toolTip1 = new ToolTip(components);
            contextMenuStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // categoryListBox
            // 
            categoryListBox.BackColor = Color.FromArgb(243, 243, 243);
            categoryListBox.BorderStyle = BorderStyle.None;
            categoryListBox.Dock = DockStyle.Left;
            categoryListBox.DrawMode = DrawMode.OwnerDrawFixed;
            categoryListBox.ForeColor = Color.Black;
            categoryListBox.FormattingEnabled = true;
            categoryListBox.ItemHeight = 22;
            categoryListBox.Location = new Point(0, 0);
            categoryListBox.Name = "categoryListBox";
            categoryListBox.Size = new Size(200, 426);
            categoryListBox.Sorted = true;
            categoryListBox.TabIndex = 0;
            categoryListBox.DrawItem += categoryListBox_DrawItem;
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
            charListView.Location = new Point(200, 0);
            charListView.MultiSelect = false;
            charListView.Name = "charListView";
            charListView.Size = new Size(584, 426);
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
            // panel1
            // 
            panel1.Controls.Add(txtOutput);
            panel1.Controls.Add(btnUndo);
            panel1.Controls.Add(btnOK);
            panel1.Controls.Add(btnClear);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 426);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 35);
            panel1.TabIndex = 2;
            // 
            // txtOutput
            // 
            txtOutput.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtOutput.BackColor = Color.FromArgb(243, 243, 243);
            txtOutput.BorderStyle = BorderStyle.None;
            txtOutput.Dock = DockStyle.Fill;
            txtOutput.Font = new Font("Segoe UI Symbol", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtOutput.Location = new Point(70, 0);
            txtOutput.Margin = new Padding(0);
            txtOutput.Name = "txtOutput";
            txtOutput.ReadOnly = true;
            txtOutput.Size = new Size(679, 28);
            txtOutput.TabIndex = 5;
            txtOutput.TextAlign = HorizontalAlignment.Center;
            // 
            // btnUndo
            // 
            btnUndo.Dock = DockStyle.Left;
            btnUndo.FlatAppearance.BorderSize = 0;
            btnUndo.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnUndo.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnUndo.FlatStyle = FlatStyle.Flat;
            btnUndo.Image = Properties.Resources.undo;
            btnUndo.Location = new Point(35, 0);
            btnUndo.Name = "btnUndo";
            btnUndo.Size = new Size(35, 35);
            btnUndo.TabIndex = 6;
            btnUndo.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnUndo, "Undo\r\nRemove last selected item");
            btnUndo.UseVisualStyleBackColor = true;
            btnUndo.Click += btnUndo_Click;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.ok;
            btnOK.Location = new Point(749, 0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 4;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnOK, "Insert\r\nInsert the selected characters to the document");
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnClear
            // 
            btnClear.Dock = DockStyle.Left;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnClear.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Image = Properties.Resources.delete;
            btnClear.Location = new Point(0, 0);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(35, 35);
            btnClear.TabIndex = 7;
            btnClear.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnClear, "Clear\r\nRemove all selected items");
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // frmSpecialCharacter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(243, 243, 243);
            ClientSize = new Size(784, 461);
            Controls.Add(charListView);
            Controls.Add(categoryListBox);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            MinimumSize = new Size(100, 100);
            Name = "frmSpecialCharacter";
            ShowIcon = false;
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Special Characters - OwOrdPad";
            Load += frmSpecialCharacter_Load;
            KeyDown += frmSpecialCharacter_KeyDown;
            contextMenuStrip1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox categoryListBox;
        private ListView charListView;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem insertToolStripMenuItem;
        private Panel panel1;
        private Button btnOK;
        private TextBox txtOutput;
        private Button btnUndo;
        private Button btnClear;
        private ToolTip toolTip1;
    }
}