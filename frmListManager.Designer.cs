namespace OwOrdPad {
    partial class frmListManager {
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
            listItems = new ListBox();
            btnOK = new Button();
            btnAdd = new Button();
            btnRemove = new Button();
            panel1 = new Panel();
            btnDown = new Button();
            btnUp = new Button();
            btnSlctAll = new Button();
            toolTip1 = new ToolTip(components);
            panel2 = new Panel();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // listItems
            // 
            listItems.BorderStyle = BorderStyle.None;
            listItems.Dock = DockStyle.Fill;
            listItems.DrawMode = DrawMode.OwnerDrawFixed;
            listItems.FormattingEnabled = true;
            listItems.IntegralHeight = false;
            listItems.ItemHeight = 22;
            listItems.Location = new Point(2, 2);
            listItems.Margin = new Padding(2);
            listItems.Name = "listItems";
            listItems.SelectionMode = SelectionMode.MultiExtended;
            listItems.Size = new Size(371, 179);
            listItems.TabIndex = 4;
            listItems.DrawItem += listItems_DrawItem;
            listItems.KeyDown += frmFavoriteFonts_KeyDown;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.save;
            btnOK.Location = new Point(340, 0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 3;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnOK, "Save (Ctrl+S)\r\nSave changes and quit");
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            btnOK.KeyDown += frmFavoriteFonts_KeyDown;
            // 
            // btnAdd
            // 
            btnAdd.Dock = DockStyle.Left;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnAdd.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Image = Properties.Resources.add;
            btnAdd.Location = new Point(0, 0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(35, 35);
            btnAdd.TabIndex = 0;
            toolTip1.SetToolTip(btnAdd, "Add (Ctrl+N)\r\nInsert a new item to the list");
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            btnAdd.KeyDown += frmFavoriteFonts_KeyDown;
            // 
            // btnRemove
            // 
            btnRemove.Dock = DockStyle.Left;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnRemove.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Image = Properties.Resources.minus;
            btnRemove.Location = new Point(35, 0);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(35, 35);
            btnRemove.TabIndex = 1;
            toolTip1.SetToolTip(btnRemove, "Remove (Delete)\r\nRemove a selected item from the list");
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            btnRemove.KeyDown += frmFavoriteFonts_KeyDown;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(btnDown);
            panel1.Controls.Add(btnUp);
            panel1.Controls.Add(btnSlctAll);
            panel1.Controls.Add(btnRemove);
            panel1.Controls.Add(btnOK);
            panel1.Controls.Add(btnAdd);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 183);
            panel1.Name = "panel1";
            panel1.Size = new Size(375, 35);
            panel1.TabIndex = 5;
            // 
            // btnDown
            // 
            btnDown.Dock = DockStyle.Left;
            btnDown.FlatAppearance.BorderSize = 0;
            btnDown.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnDown.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnDown.FlatStyle = FlatStyle.Flat;
            btnDown.Image = Properties.Resources.down;
            btnDown.Location = new Point(140, 0);
            btnDown.Name = "btnDown";
            btnDown.Size = new Size(35, 35);
            btnDown.TabIndex = 5;
            toolTip1.SetToolTip(btnDown, "Move down (Alt+Down)\r\nMove the selection down\r\n");
            btnDown.UseVisualStyleBackColor = true;
            btnDown.Click += btnDown_Click;
            // 
            // btnUp
            // 
            btnUp.Dock = DockStyle.Left;
            btnUp.FlatAppearance.BorderSize = 0;
            btnUp.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnUp.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnUp.FlatStyle = FlatStyle.Flat;
            btnUp.Image = Properties.Resources.up;
            btnUp.Location = new Point(105, 0);
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(35, 35);
            btnUp.TabIndex = 4;
            toolTip1.SetToolTip(btnUp, "Move up (Alt+Up)\r\nMove the selection up");
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // btnSlctAll
            // 
            btnSlctAll.Dock = DockStyle.Left;
            btnSlctAll.FlatAppearance.BorderSize = 0;
            btnSlctAll.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnSlctAll.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnSlctAll.FlatStyle = FlatStyle.Flat;
            btnSlctAll.Image = Properties.Resources.selectAll;
            btnSlctAll.Location = new Point(70, 0);
            btnSlctAll.Name = "btnSlctAll";
            btnSlctAll.Size = new Size(35, 35);
            btnSlctAll.TabIndex = 6;
            toolTip1.SetToolTip(btnSlctAll, "Select all (Ctrl+A)\r\nSelect all items\r\n");
            btnSlctAll.UseVisualStyleBackColor = true;
            btnSlctAll.Click += btnSlctAll_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(listItems);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Padding = new Padding(2);
            panel2.Size = new Size(375, 183);
            panel2.TabIndex = 6;
            // 
            // frmListManager
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(375, 218);
            Controls.Add(panel2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmListManager";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "frmListManager";
            Load += frmFavoriteFonts_Load;
            KeyDown += frmFavoriteFonts_KeyDown;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListBox listItems;
        private Button btnOK;
        private Button btnAdd;
        private Button btnRemove;
        private Panel panel1;
        private ToolTip toolTip1;
        private Button btnUp;
        private Button btnDown;
        private Button btnSlctAll;
        private Panel panel2;
    }
}