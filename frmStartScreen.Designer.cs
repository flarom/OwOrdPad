namespace OwOrdPad {
    partial class frmStartScreen {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStartScreen));
            panel1 = new Panel();
            btnOK = new Button();
            btnRestore = new Button();
            btnOpen = new Button();
            btnNew = new Button();
            pictureBox1 = new PictureBox();
            groupBox1 = new GroupBox();
            listView1 = new ListView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            copyPathToolStripMenuItem = new ToolStripMenuItem();
            openFileLocationToolStripMenuItem = new ToolStripMenuItem();
            imageList1 = new ImageList(components);
            toolTip1 = new ToolTip(components);
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(243, 243, 243);
            panel1.Controls.Add(btnOK);
            panel1.Controls.Add(btnRestore);
            panel1.Controls.Add(btnOpen);
            panel1.Controls.Add(btnNew);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 364);
            panel1.Name = "panel1";
            panel1.Size = new Size(353, 35);
            panel1.TabIndex = 4;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.FlatAppearance.BorderSize = 0;
            btnOK.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOK.FlatStyle = FlatStyle.Flat;
            btnOK.Image = Properties.Resources.ok;
            btnOK.Location = new Point(318, 0);
            btnOK.Margin = new Padding(0);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(35, 35);
            btnOK.TabIndex = 3;
            btnOK.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnOK, "Close this dialog");
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += closeDialog;
            // 
            // btnRestore
            // 
            btnRestore.Dock = DockStyle.Left;
            btnRestore.FlatAppearance.BorderSize = 0;
            btnRestore.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnRestore.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnRestore.FlatStyle = FlatStyle.Flat;
            btnRestore.Image = Properties.Resources.restore;
            btnRestore.Location = new Point(70, 0);
            btnRestore.Margin = new Padding(0);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(35, 35);
            btnRestore.TabIndex = 2;
            btnRestore.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnRestore, "Open the last automatically saved document");
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // btnOpen
            // 
            btnOpen.Dock = DockStyle.Left;
            btnOpen.FlatAppearance.BorderSize = 0;
            btnOpen.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnOpen.FlatStyle = FlatStyle.Flat;
            btnOpen.Image = Properties.Resources.folder;
            btnOpen.Location = new Point(35, 0);
            btnOpen.Margin = new Padding(0);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(35, 35);
            btnOpen.TabIndex = 1;
            btnOpen.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnOpen, "Open an existing document");
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // btnNew
            // 
            btnNew.Dock = DockStyle.Left;
            btnNew.FlatAppearance.BorderSize = 0;
            btnNew.FlatAppearance.MouseDownBackColor = Color.FromArgb(49, 215, 193);
            btnNew.FlatAppearance.MouseOverBackColor = Color.FromArgb(49, 215, 193);
            btnNew.FlatStyle = FlatStyle.Flat;
            btnNew.Image = Properties.Resources.document;
            btnNew.Location = new Point(0, 0);
            btnNew.Margin = new Padding(0);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(35, 35);
            btnNew.TabIndex = 0;
            btnNew.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnNew, "Create a new document");
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += closeDialog;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(353, 125);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(listView1);
            groupBox1.Location = new Point(12, 131);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(329, 227);
            groupBox1.TabIndex = 5;
            groupBox1.TabStop = false;
            groupBox1.Text = "Recently edited";
            // 
            // listView1
            // 
            listView1.BorderStyle = BorderStyle.None;
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.Dock = DockStyle.Fill;
            listView1.LargeImageList = imageList1;
            listView1.Location = new Point(3, 19);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(323, 205);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.ItemActivate += listView1_ItemActivate;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { copyPathToolStripMenuItem, openFileLocationToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(169, 48);
            // 
            // copyPathToolStripMenuItem
            // 
            copyPathToolStripMenuItem.Image = Properties.Resources.copy;
            copyPathToolStripMenuItem.Name = "copyPathToolStripMenuItem";
            copyPathToolStripMenuItem.Size = new Size(168, 22);
            copyPathToolStripMenuItem.Text = "Copy path";
            copyPathToolStripMenuItem.Click += copyPathToolStripMenuItem_Click;
            // 
            // openFileLocationToolStripMenuItem
            // 
            openFileLocationToolStripMenuItem.Image = Properties.Resources.openFolder;
            openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            openFileLocationToolStripMenuItem.Size = new Size(168, 22);
            openFileLocationToolStripMenuItem.Text = "Open file location";
            openFileLocationToolStripMenuItem.Click += openFileLocationToolStripMenuItem_Click;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "folder");
            // 
            // frmStartScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            CancelButton = btnOK;
            ClientSize = new Size(353, 399);
            Controls.Add(groupBox1);
            Controls.Add(pictureBox1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmStartScreen";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Home - OwOrdPad";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Button btnRestore;
        private Button btnOpen;
        private Button btnNew;
        private GroupBox groupBox1;
        private ToolTip toolTip1;
        private ListView listView1;
        private ImageList imageList1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem copyPathToolStripMenuItem;
        private ToolStripMenuItem openFileLocationToolStripMenuItem;
        private Button btnOK;
    }
}