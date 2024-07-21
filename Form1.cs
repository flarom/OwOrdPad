using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using OwOrdPad.Properties;
using System.Diagnostics;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net;
using System.Text;

namespace OwOrdPad {
    public partial class Form1 : Form {
        Settings settings = new Settings();
        static Themes theme = new Themes();

        string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        string defaultFont = "Calibri";
        public string filePath; // full file path to the opened/saved file
        public bool isDocumentModified = false; // checks if the document was modified by the user
        frmInputBox inputbox = new();
        frmListManager listManager = new();
        frmOptionBox optionBox = new();

        public Form1() {
            // load program and settings
            InitializeComponent();
            LoadSettings();
            loadHistoryList();
            updateHistoryMenu();
        }
        private void Form1_Load(object sender, EventArgs e) {
            // load custom visuals
            rtb.SelectionCharOffset = 1;
            loadVisuals();

            // load fonts
            InstalledFontCollection installedFontCollection = new();
            FontFamily[] fontFamilies = installedFontCollection.Families;
            foreach (FontFamily fontFamily in fontFamilies) {
                cbFonts.Items.Add(fontFamily.Name);
                cbFonts.AutoCompleteCustomSource.Add(fontFamily.Name);
            }
        }
        private void loadVisuals() {
            this.BackColor = theme.documentBack;
            inputbox.Themes = theme;
            listManager.Themes = theme;
            optionBox.Themes = theme;

            // document
            rtb.BackColor = theme.documentBack;
            rtb.ForeColor = theme.documentFore;
            rtb.SelectionBackColor = theme.documentBack;
            rtb.SelectionColor = theme.documentFore;
            splitter1.BackColor = theme.documentBack;
            tsTool.BackColor = theme.documentBack;
            tsTool.ForeColor = theme.documentFore;
            tsFormat.BackColor = theme.documentBack;
            tsFormat.ForeColor = theme.documentFore;
            cbFonts.BackColor = theme.documentBack;
            cbFonts.ForeColor = theme.documentFore;
            cbFontSize.BackColor = theme.documentBack;
            cbFontSize.ForeColor = theme.documentFore;

            // header
            treeExplorer.BackColor = theme.headerBack;
            treeExplorer.ForeColor = theme.headerFore;
            menuStrip.BackColor = theme.headerBack;
            menuStrip.ForeColor = theme.headerFore;
            statusStrip.BackColor = theme.headerBack;
            statusStrip.ForeColor = theme.headerFore;

            // menus
            foreach (ToolStripItem item in menuStrip.Items) {
                if (item is ToolStripMenuItem menuitem) {
                    ApplyMenuColors(menuitem);
                }
            }
            contextMenuStrip1.BackColor = theme.menuBack;
            contextMenuStrip1.ForeColor = theme.menuFore;
            foreach (ToolStripItem item in contextMenuStrip1.Items) {
                if (item is ToolStripMenuItem menuitem) {
                    ApplyMenuColors(menuitem);
                }
            }
            contextMenuStrip2.BackColor = theme.menuBack;
            contextMenuStrip2.ForeColor = theme.menuFore;
            foreach (ToolStripItem item in contextMenuStrip2.Items) {
                if (item is ToolStripMenuItem menuitem) {
                    ApplyMenuColors(menuitem);
                }
            }
            contextMenuStrip3.BackColor = theme.menuBack;
            contextMenuStrip3.ForeColor = theme.menuFore;
            foreach (ToolStripItem item in contextMenuStrip3.Items) {
                if (item is ToolStripMenuItem menuitem) {
                    ApplyMenuColors(menuitem);
                }
            }
            foreach (ToolStripItem item in tsTool.Items) {
                if (item is ToolStripButton menuitem) {
                    menuitem.Image = theme.paintBitmap(menuitem.Image, theme.icons);
                }
                if (item is ToolStripSplitButton menuitem2) {
                    menuitem2.Image = theme.paintBitmap(menuitem2.Image, theme.icons);
                    menuitem2.DropDown.BackColor = theme.menuBack;
                    menuitem2.DropDown.ForeColor = theme.menuFore;
                }
            }
            foreach (ToolStripItem item in tsFormat.Items) {
                if (item is ToolStripButton menuitem) {
                    menuitem.Image = theme.paintBitmap(menuitem.Image, theme.icons);
                }
                if (item is ToolStripSplitButton menuitem2) {
                    menuitem2.Image = theme.paintBitmap(menuitem2.Image, theme.icons);
                    menuitem2.DropDown.BackColor = theme.menuBack;
                    menuitem2.DropDown.ForeColor = theme.menuFore;
                }
            }
            foreach (ToolStripStatusLabel item in statusStrip.Items) {
                if (item.Image != null) {
                    item.Image = theme.paintBitmap(item.Image, theme.icons);
                }
            }

            menuStrip.Renderer = new MenuRenderer();
            tsTool.Renderer = new MenuRenderer();
            tsFormat.Renderer = new MenuRenderer();
            contextMenuStrip1.Renderer = new MenuRenderer();
            contextMenuStrip2.Renderer = new MenuRenderer();
            contextMenuStrip3.Renderer = new MenuRenderer();

            themeToolStripMenuItem.ShortcutKeyDisplayString = theme.themeName;
        }

        private void ApplyMenuColors(ToolStripMenuItem menuItem) {
            menuItem.DropDown.BackColor = theme.menuBack;
            menuItem.DropDown.ForeColor = theme.menuFore;

            if (menuItem.Image != null) {
                menuItem.Image = theme.paintBitmap(menuItem.Image, theme.icons);
            }

            foreach (ToolStripItem subItem in menuItem.DropDown.Items) {
                if (subItem is ToolStripMenuItem subMenuItem) {
                    ApplyMenuColors(subMenuItem);
                }
            }
        }

        private class MenuRenderer : ToolStripProfessionalRenderer {
            public MenuRenderer() : base(new CustomColors(theme)) { }
        }
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new();
            if (fbd.ShowDialog() == DialogResult.OK) {
                LoadDirectory(fbd.SelectedPath.ToString());
                updateHistory(fbd.SelectedPath.ToString());
                saveHistory();
                updateHistoryMenu();
            }
        }
        private void manageWorkspacesToolStripMenuItem_Click(object sender, EventArgs e) {
            string[] nodesPath = new string[treeExplorer.Nodes.Count];

            for (int i = 0; i < treeExplorer.Nodes.Count; i++) {
                nodesPath[i] = treeExplorer.Nodes[i].Tag.ToString();
            }

            string[] newNodesPath = listManager.getList("Manage workspaces - OwOrdPad", nodesPath, null, frmListManager.addType.FromFolder);

            treeExplorer.Nodes.Clear();

            foreach (string node in newNodesPath) {
                LoadDirectory(node);
            }
        }
        private void clearFoldersToolStripMenuItem_Click(object sender, EventArgs e) {
            treeExplorer.Nodes.Clear();
        }
        public async Task LoadDirectory(string path) {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException();

            var rootDirectoryInfo = new DirectoryInfo(path);
            treeExplorer.Nodes.Add(await Task.Run(() => CreateDirectoryNode(rootDirectoryInfo)));
            treeExplorer.Nodes[0].Expand();
        }

        private TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo) {
            int imageIndex = 0; // Default image index

            string colorFilePath = Path.Combine(directoryInfo.FullName, ".owordpadFolderColor");
            if (File.Exists(colorFilePath)) {
                try {
                    string color = File.ReadAllText(colorFilePath).Trim().ToLower();

                    imageIndex = color switch {
                        "red" => 4,
                        "org" => 5,
                        "ylw" => 6,
                        "grn" => 7,
                        "blu" => 8,
                        "prp" => 9,
                        _ => 0,// Default image for unknown or no color specified
                    };
                }
                catch {
                    sendNotification("Failed to read folder color config", "error");
                    imageIndex = 0;
                }
            }

            var directoryNode = new TreeNode(directoryInfo.Name, imageIndex, imageIndex) { Tag = directoryInfo.FullName };

            try {
                foreach (var directory in directoryInfo.GetDirectories()) {
                    if ((directory.Attributes & FileAttributes.Hidden) == 0) {
                        directoryNode.Nodes.Add(CreateDirectoryNode(directory));
                    }
                }
            }
            catch { return new TreeNode(); }

            foreach (var file in directoryInfo.GetFiles()) {
                if ((file.Attributes & FileAttributes.Hidden) == 0) {
                    string fileExtension = Path.GetExtension(file.Name).ToLower();
                    int fileImageIndex;

                    switch (fileExtension) {
                        // text documents
                        case ".rtf":
                        case ".owo":
                        case ".txt":
                        case ".md":
                        case ".html":
                        case ".xml":
                        case ".json":
                        case ".log":
                        case ".ini":
                        case ".conf":
                        case ".config":
                        case ".properties":
                        case ".php":
                        case ".java":
                        case ".py":
                        case ".rb":
                        case ".js":
                        case ".ts":
                        case ".cpp":
                        case ".c":
                        case ".cs":
                        case ".go":
                        case ".rs":
                        case ".bat":
                        case ".sql":
                            fileImageIndex = 3;
                            break;
                        // image files
                        case ".bmp":
                        case ".dib":
                        case ".rle":
                        case ".jpg":
                        case ".jpeg":
                        case ".jpe":
                        case ".jfif":
                        case ".gif":
                        case ".emf":
                        case ".wmf":
                        case ".tif":
                        case ".tiff":
                        case ".png":
                        case ".ico":
                            fileImageIndex = 2;
                            break;
                        default:
                            // unknown
                            fileImageIndex = 1;
                            break;
                    }

                    directoryNode.Nodes.Add(new TreeNode(file.Name, fileImageIndex, fileImageIndex) { Tag = file.FullName });
                }
            }

            return directoryNode;
        }

        public void register() {
            // add owordpad to App Paths registry, so it can be launched from the Win+R run command.
            string regKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths\owordpad.exe";

            using RegistryKey key = Registry.CurrentUser.CreateSubKey(regKey);
            key.SetValue("", defaultDirectory + "\\OwOrdPad.exe");
            key.SetValue("owordpad", defaultDirectory + "\\OwOrdPad.exe owordpad");
            key.SetValue("Path", Application.StartupPath);
        }
        private void aboutOwOrdPadToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 about = new();
            about.ShowDialog(this);
        }
        #region settings
        // contains events related to writing and reading configuration files 
        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            LoadFavoriteFonts();
        }
        private void manageFontsToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                InstalledFontCollection installedFonts = new InstalledFontCollection();
                FontFamily[] fontFamilies = installedFonts.Families;
                string[] fontNames = new string[fontFamilies.Length];

                for (int i = 0; i < fontFamilies.Length; i++) {
                    fontNames[i] = fontFamilies[i].Name;
                }
                string favFontsPath = defaultDirectory + "\\settings\\favoriteFonts";
                File.WriteAllLines(favFontsPath, listManager.getList("Favorite fonts - OwOrdPad", File.ReadAllLines(favFontsPath), fontNames));
            }
            catch { }
        }
        private void LoadFavoriteFonts() {
            favoriteFontsToolStripMenuItem.DropDownItems.Clear();

            string[] fonts = File.ReadAllLines(defaultDirectory + "\\settings\\favoriteFonts");

            foreach (string fontName in fonts) {
                ToolStripMenuItem fontItem = new ToolStripMenuItem() {
                    Text = fontName,
                    Tag = fontName,
                    Font = new Font(fontName, 10),
                };
                fontItem.Click += selectFont;

                favoriteFontsToolStripMenuItem.DropDownItems.Add(fontItem);
            }

            favoriteFontsToolStripMenuItem.DropDownItems.Add(toolStripSeparator13);
            favoriteFontsToolStripMenuItem.DropDownItems.Add(manageFontsToolStripMenuItem);
        }
        private void LoadSettings() {
            try {
                tsFormat.Visible = bool.Parse(Settings.GetSetting("showFormatBar"));
                statusStrip.Visible = bool.Parse(Settings.GetSetting("showStatusBar"));
                tsTool.Visible = bool.Parse(Settings.GetSetting("showToolBar"));
                pnlExplorer.Visible = bool.Parse(Settings.GetSetting("showExplorer"));
                splitter1.Visible = bool.Parse(Settings.GetSetting("showExplorer"));
                rtb.WordWrap = bool.Parse(Settings.GetSetting("wordWrap"));
                rtb.ShowSelectionMargin = bool.Parse(Settings.GetSetting("selectionMargin"));
                rtb.Font = new Font(Settings.GetSetting("defaultFont"), 12);
                defaultFont = Settings.GetSetting("defaultFont");
                theme.setTheme(Settings.GetSetting("theme"));

                UpdateToolTipsForMenuItems(menuStrip.Items, bool.Parse(Settings.GetSetting("showToolTips")));

                if (bool.Parse(Settings.GetSetting("wasClosedByUser")) == false) {
                    sendNotification("Use Ctrl+Shift+T to restore autosave", "warning");
                }
                Settings.SaveSetting("wasClosedByUser", "False");

                //indicators
                formatBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showFormatBar"));
                statusBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showStatusBar"));
                toolBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showToolBar"));
                explorerToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showExplorer"));
                closeBracketsToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("closeBrackets"));
                wordWrapToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("wordWrap"));
                selectionMarginToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("selectionMargin"));
                toolTipsToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showToolTips"));
                spellCheckerToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("spellCheck"));
                defaultFontToolStripMenuItem.ShortcutKeyDisplayString = Settings.GetSetting("defaultFont");
            }
            catch {
                //MessageBox.Show("Failed to load settings: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sendNotification("An error occurred while loading settings", "error");
            }
        }
        #endregion
        #region file manipulation
        // contais events related to write and read documents

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.Text != string.Empty && isDocumentModified && MessageBox.Show("Are you sure you want to open a document?\nYou will lose any unsaved changes on your current file.", "Open document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            OpenFileDialog ofd = new() {
                Filter = "Rich Text File|*.rtf|OwOdPad Markdown|*.owo|Plain Text File|*.txt|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*",
                Title = "Open document"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK && ofd.CheckFileExists) {
                updateHistory(ofd.FileName);
                saveHistory();
                updateHistoryMenu();

                OpenFile(ofd.FileName);
                treeExplorer.Nodes.Clear();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(filePath)) {
                switch (Path.GetExtension(filePath)) {
                    case ".rtf":
                    case ".owo":
                        SaveFile(filePath, true);
                        break;
                    default:
                        SaveFile(filePath, false);
                        break;
                }
            }
            else {
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) {
                saveFileDialog.Filter = "Rich Text File|*.rtf|OwOdPad Markdown|*.owo|Plain Text File|*.txt|All Files|*.*";
                saveFileDialog.Title = "Save As";
                saveFileDialog.FileName = "New document.rtf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    filePath = saveFileDialog.FileName;

                    bool saveAsRTF = Path.GetExtension(filePath).Equals(".rtf", StringComparison.OrdinalIgnoreCase) ||
                  Path.GetExtension(filePath).Equals(".owo", StringComparison.OrdinalIgnoreCase);

                    SaveFile(filePath, saveAsRTF);
                    this.Text = Path.GetFileName(saveFileDialog.FileName) + " - OwOrdPad";
                }
            }
        }
        private void SaveFile(string path, bool saveAsRTF) {
            try {
                if (saveAsRTF) {
                    rtb.SaveFile(path, RichTextBoxStreamType.RichText); // save as rich text
                }
                else {
                    File.WriteAllText(path, rtb.Text); // save as plain text
                }
                isDocumentModified = false;
                lblSaveState.Text = "Saved";
                this.Icon = Icon.ExtractAssociatedIcon(path);
                updateHistory(path);
                saveHistory();
                updateHistoryMenu();
                sendNotification("File saved as " + Path.GetFileName(path), "succes");
            }
            catch { sendNotification("Failed to save file", "error"); }
        }
        public void OpenFile(string path) {
            string pathExtension = Path.GetExtension(path);

            switch (pathExtension) {
                // Rich Text
                case ".rtf":
                case ".owo":
                    try { rtb.LoadFile(path); }
                    catch { rtb.Text = File.ReadAllText(path); }
                    filePath = path;
                    break;

                // Image OLE Object
                case ".bmp":
                case ".dib":
                case ".rle":
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".jfif":
                case ".gif":
                case ".emf":
                case ".wmf":
                case ".tif":
                case ".tiff":
                case ".png":
                case ".ico":
                    rtb.Clear();
                    //rtb.ClearUndo();

                    Clipboard.SetImage(Bitmap.FromFile(path));
                    rtb.Paste();
                    filePath = "";
                    break;

                // Plain Text
                default:
                    rtb.Text = File.ReadAllText(path);
                    rtb.Font = new Font("Consolas", rtb.Font.Size);
                    filePath = path;
                    break;
            }

            this.Text = Path.GetFileName(path) + " - OwOrdPad";
            this.Icon = Icon.ExtractAssociatedIcon(path);

            lblSaveState.Text = "Saved";
            isDocumentModified = false;
        }

        int autoSaveCooldown = 0;
        private void rtb_TextChanged(object sender, EventArgs e) {
            // word count
            lblCharAndWord.Text = rtb.Text.Length + " Characters, " +
                rtb.Text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length + " Words";

            // auto save
            autoSaveCooldown = 0;
            tmrTypeStop.Start();
            if (rtb.Text != string.Empty) {
                isDocumentModified = true;
                lblSaveState.Text = "Not saved";
            }
            else {
                isDocumentModified = false;
                lblSaveState.Text = "-";
            }
        }
        private void tmrTypeStop_Tick(object sender, EventArgs e) {
            autoSaveCooldown++;
            if (autoSaveCooldown >= 100) {
                tmrTypeStop.Stop();
                autoSaveCooldown = 0;

                // auto save
                rtb.SaveFile(defaultDirectory + "\\Autosave\\save.rtf");

                // calculate estimated file size
                int fileSize = rtb.Rtf.Length;

                switch (fileSize) {
                    case var _ when fileSize < 1024:
                        lblFileSize.Text = fileSize + "B";
                        break;
                    case var _ when fileSize < Math.Pow(1024, 2):
                        double sizeInKB = fileSize / 1024.0;
                        lblFileSize.Text = sizeInKB.ToString("0.00") + "KB";
                        break;
                    case var _ when fileSize < Math.Pow(1024, 3):
                        double sizeInMB = fileSize / Math.Pow(1024, 2);
                        lblFileSize.Text = sizeInMB.ToString("0.00") + "MB";
                        break;
                    default:
                        double sizeInGB = fileSize / Math.Pow(1024, 3);
                        lblFileSize.Text = sizeInGB.ToString("0.00") + "GB";
                        break;
                }
            }
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (isDocumentModified) {
                if (MessageBox.Show("Are you sure you want to create a new document?\nYou will lose any unsaved changes on your current file.", "New document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    return;
                }
            }
            isDocumentModified = false;
            lblSaveState.Text = "Saved";
            filePath = string.Empty;
            Icon = Resources.favicon;
            Text = "New document - OwOrdPad";
            rtb.Clear();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (isDocumentModified) {
                DialogResult result = MessageBox.Show("Do you want to save changes to this document?", "Save your work", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) {
                    SaveFile(filePath, true);
                }
                else if (result == DialogResult.Cancel) {
                    e.Cancel = true;
                }
            }

            Settings.SaveSetting("wasClosedByUser", "True");
        }
        #endregion
        #region search
        // contais events related to search by line/string on the rtb editor
        private int lastSearchIndex = 0;
        private void findToolStripMenuItem_Click(object sender, EventArgs e) {
            string searchFor;

            if (!string.IsNullOrEmpty(rtb.SelectedText)) {
                searchFor = rtb.SelectedText;
            }
            else {
                searchFor = Clipboard.GetText();
            }

            string searchString = inputbox.GetInput("Search for a word, character or symbol:", "Find - OwOrdPad", [], Resources.search, searchFor);

            if (!string.IsNullOrEmpty(searchString)) {
                int startIndex = rtb.SelectionStart + rtb.SelectionLength;
                int index = rtb.Find(searchString, startIndex, RichTextBoxFinds.None);

                if (index != -1) {
                    rtb.Select(index, searchString.Length);
                    lastSearchIndex = index + searchString.Length;
                    findToolStripMenuItem_Click(sender, e);
                }
                else {
                    lastSearchIndex = 0;
                    sendNotification("No results", "warning");
                }
            }
        }
        private void searchForDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            List<string> allFiles = getAllNodes(treeExplorer.Nodes);
            string searchText = inputbox.GetInput("Search for a document name:", "Explorer - OwOrdPad", allFiles.ToArray(), Resources.search);

            if (!string.IsNullOrWhiteSpace(searchText)) {
                TreeNode foundNode = searchNode(treeExplorer.Nodes, searchText);

                if (foundNode != null) {
                    treeExplorer.SelectedNode = foundNode;
                    treeExplorer.SelectedNode.Expand();
                    treeExplorer.SelectedNode.EnsureVisible();
                    LoadSelectedFile();
                }
            }
        }
        private TreeNode searchNode(TreeNodeCollection nodes, string searchText) {
            foreach (TreeNode node in nodes) {
                if (node.Text.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) {
                    return node;
                }
                if (node.Tag.ToString().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) {
                    return node;
                }

                TreeNode foundNode = searchNode(node.Nodes, searchText);
                if (foundNode != null) {
                    return foundNode;
                }
            }
            return null;
        }
        private List<string> getAllNodes(TreeNodeCollection nodes, string parentPath = "") {
            List<string> pathsAndNames = new List<string>();

            foreach (TreeNode node in nodes) {
                string currentPath = string.IsNullOrEmpty(parentPath) ? node.Text : $"{parentPath}\\{node.Text}";
                pathsAndNames.Add(currentPath);
                pathsAndNames.Add(node.Text);

                if (node.Nodes.Count > 0) {
                    pathsAndNames.AddRange(getAllNodes(node.Nodes, currentPath));
                }
            }

            return pathsAndNames;
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e) {
            string inputLineNumber = inputbox.GetInput("Search for a line number (1 - " + rtb.Lines.Count() + "):", "Go to - OwOrdPad", [], Resources.search, (rtb.GetLineFromCharIndex(rtb.SelectionStart) + 1).ToString());

            if (int.TryParse(inputLineNumber, out int lineNumber) && lineNumber > 0) {
                SearchByLine(lineNumber);
            }
        }
        private void SearchByLine(int lineNumber) {
            string[] lines = rtb.Lines;

            if (lineNumber <= lines.Length) {
                int index = rtb.GetFirstCharIndexFromLine(lineNumber - 1);
                rtb.Select(index, lines[lineNumber - 1].Length);
                rtb.Focus();
            }
            else {
                sendNotification("No results", "warning");
            }
        }
        #endregion
        #region basic text manipulation
        // contains basic text manipulation events
        private void undoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.Undo(); // undo, ctrl+z
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.Redo(); // redo, ctrl+y
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.Cut(); // cut, ctrl+x
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            // Copy, ctrl+c
            if (rtb.SelectionType == RichTextBoxSelectionTypes.Text) {
                rtb.Copy();
                return;
            }

            var selectedRtf = rtb.SelectedRtf;

            DataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Rtf, selectedRtf);

            var selectedText = rtb.SelectedText;
            if (!string.IsNullOrEmpty(selectedText)) {
                dataObject.SetData(DataFormats.Text, selectedText);
            }

            if (rtb.SelectionType.HasFlag(RichTextBoxSelectionTypes.Object)) {
                try {
                    var clipboardImage = Clipboard.GetImage();
                    if (clipboardImage != null) {
                        dataObject.SetData(DataFormats.Bitmap, clipboardImage);
                    }
                }
                catch {

                }
            }

            Clipboard.SetDataObject(dataObject, true);
        }


        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.Paste(); //paste, ctrl+v
        }
        private void pasteWithoutFormattingToolStripMenuItem_Click(object sender, EventArgs e) {
            string plainText = Clipboard.GetText(TextDataFormat.UnicodeText);
            rtb.AppendText(plainText); // paste no format, ctrl+shift+v
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectedRtf = string.Empty; // delete, delete
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectAll(); // select all, ctrl+a
        }
        #endregion
        #region view
        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.ZoomFactor < 5) {
                rtb.ZoomFactor += 0.1f;
            }
            zoomToolStripMenuItem.ShowDropDown();
            toolStripMenuItem1.Select();
            lblZoomFactor.Text = (rtb.ZoomFactor * 100).ToString("0") + "%";
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.ZoomFactor > 0.2f) {
                rtb.ZoomFactor -= 0.1f;
            }
            zoomToolStripMenuItem.ShowDropDown();
            toolStripMenuItem2.Select();
            lblZoomFactor.Text = (rtb.ZoomFactor * 100).ToString("0") + "%";
        }

        private void restoreZoomToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.ZoomFactor = 1f;
            lblZoomFactor.Text = (rtb.ZoomFactor * 100).ToString("0") + "%";
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.WordWrap = wordWrapToolStripMenuItem.Checked;
            Settings.SaveSetting("wordWrap", wordWrapToolStripMenuItem.Checked.ToString());
        }
        private void selectionMarginToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.ShowSelectionMargin = selectionMarginToolStripMenuItem.Checked;
            Settings.SaveSetting("selectionMargin", selectionMarginToolStripMenuItem.Checked.ToString());
        }
        private void statusBarToolStripMenuItem_Click(object sender, EventArgs e) {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
            Settings.SaveSetting("showStatusBar", statusBarToolStripMenuItem.Checked.ToString());
        }

        private void toolBarToolStripMenuItem_Click(object sender, EventArgs e) {
            tsTool.Visible = toolBarToolStripMenuItem.Checked;
            Settings.SaveSetting("showToolBar", toolBarToolStripMenuItem.Checked.ToString());
        }

        private void formatBarToolStripMenuItem_Click(object sender, EventArgs e) {
            tsFormat.Visible = formatBarToolStripMenuItem.Checked;
            Settings.SaveSetting("showFormatBar", formatBarToolStripMenuItem.Checked.ToString());
        }
        private void spellCheckerToolStripMenuItem_Click(object sender, EventArgs e) {
            Settings.SaveSetting("spellCheck", spellCheckerToolStripMenuItem.Checked.ToString());
        }
        private void homeScreenToolStripMenuItem_Click(object sender, EventArgs e) {
            Settings.SaveSetting("showHomeScreen", homeScreenToolStripMenuItem.Checked.ToString());
        }
        private void explorerToolStripMenuItem_Click(object sender, EventArgs e) {
            pnlExplorer.Visible = explorerToolStripMenuItem.Checked;
            splitter1.Visible = explorerToolStripMenuItem.Checked;

            if (explorerToolStripMenuItem.Checked) {
                treeExplorer.Select();
            }

            Settings.SaveSetting("showExplorer", explorerToolStripMenuItem.Checked.ToString());
        }
        private void closeBracketsToolStripMenuItem_Click(object sender, EventArgs e) {
            Settings.SaveSetting("closeBrackets", closeBracketsToolStripMenuItem.Checked.ToString());
        }
        #endregion
        #region formatting
        private void fontToolStripMenuItem_Click(object sender, EventArgs e) {
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            FontFamily[] fontFamilies = installedFonts.Families;
            string[] fontNames = new string[fontFamilies.Length];

            for (int i = 0; i < fontFamilies.Length; i++) {
                fontNames[i] = fontFamilies[i].Name;
            }

            string font = inputbox.GetInput("Insert a font name: ", "Text font - OwOrdPad", fontNames, Resources.ok);
            if (font != null) {
                rtb.SelectionFont = new Font(font, rtb.SelectionFont.Size);
            }
        }

        private void leftToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void rightToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void tabulateRightToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionIndent += 24;
        }

        private void tabulateLeftToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionIndent -= 24;
        }

        private void clearFormattingToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(defaultFont, 12f);
            rtb.SelectionBackColor = rtb.BackColor;
            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionIndent = 0;
            rtb.SelectionCharOffset = int.Parse(lnSpace1.Tag.ToString()); // lnSpace1 = 1px
            //rtb.SelectionCharOffset = 1; // i could make it just set the char offset to 1, but somehow it is becoming 0px :/

            cbFonts.Text = defaultFont;
            cbFontSize.Text = "12";

            btnBold.Checked = rtb.SelectionFont.Bold;
            btnItalic.Checked = rtb.SelectionFont.Italic;
            btnUnderline.Checked = rtb.SelectionFont.Underline;
            btnStrikeout.Checked = rtb.SelectionFont.Strikeout;
        }
        private void boldToolStripMenuItem_Click(object sender, EventArgs e) {
            ToggleStyle(FontStyle.Bold);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e) {
            ToggleStyle(FontStyle.Italic);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e) {
            ToggleStyle(FontStyle.Underline);
        }

        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e) {
            ToggleStyle(FontStyle.Strikeout);
        }

        private void ToggleStyle(FontStyle style) {
            try {
                if (rtb.SelectionFont.Style.HasFlag(style)) {
                    rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style & ~style);
                }
                else {
                    rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style | style);
                }

                btnBold.Checked = rtb.SelectionFont.Bold;
                btnItalic.Checked = rtb.SelectionFont.Italic;
                btnUnderline.Checked = rtb.SelectionFont.Underline;
                btnStrikeout.Checked = rtb.SelectionFont.Strikeout;
            }
            catch {
                SystemSounds.Exclamation.Play();
            }
        }

        private void superscriptToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionCharOffset = rtb.SelectionCharOffset == 0 ? 8 : 0;
        }
        private void subscriptToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionCharOffset = rtb.SelectionCharOffset == 0 ? -8 : 0;
        }
        private void rtb_SelectionChanged(object sender, EventArgs e) {
            lblCursorPos.Text = "Ln " + (rtb.GetLineFromCharIndex(rtb.SelectionStart) + 1) +
                ", Col " + (rtb.SelectionStart - rtb.GetFirstCharIndexOfCurrentLine() + 1);
            try {
                cbFontSize.Text = rtb.SelectionFont.Size.ToString();
                cbFonts.Text = rtb.SelectionFont.Name.ToString();

                // effects indicators
                btnBold.Checked = rtb.SelectionFont.Bold;
                btnItalic.Checked = rtb.SelectionFont.Italic;
                btnUnderline.Checked = rtb.SelectionFont.Underline;
                btnStrikeout.Checked = rtb.SelectionFont.Strikeout;

                // line spacing indicators
                foreach (ToolStripMenuItem menuItem in spltbtnLineSpace.DropDownItems) {
                    menuItem.Checked = false;
                }
                lnSpaceOther.Visible = false;
                switch (rtb.SelectionCharOffset) {
                    case 1:
                        lnSpace1.Checked = true;
                        break;
                    case 4:
                        lnSpace4.Checked = true;
                        break;
                    case 8:
                        lnSpace8.Checked = true;
                        break;
                    case 16:
                        lnSpace16.Checked = true;
                        break;
                    default:
                        lnSpaceOther.ShortcutKeyDisplayString = rtb.SelectionCharOffset.ToString() + " px";
                        lnSpaceOther.Checked = true;
                        lnSpaceOther.Visible = true;
                        break;
                }
            }
            catch { }
            showSimilarStrings(rtb.SelectedText);
        }

        private void cbFonts_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                rtb.SelectionFont = new Font(cbFonts.Text, rtb.SelectionFont.Size);
                rtb.Focus();
            }
            catch { }
        }

        private void cbFontSize_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                float size = float.Parse(cbFontSize.Text);
                if (size > 0 && size <= 96) {
                    rtb.SelectionFont = new Font(rtb.SelectionFont.Name, size);
                }
                else {
                    sendNotification("Invalid font size", "warning");
                }
            }
            catch { }
        }
        int caseCycle = 0;
        private void toggleCaseToolStripMenuItem_Click(object sender, EventArgs e) {
            // switch case
            if (rtb.SelectionLength > 0) {
                int selectionStart = rtb.SelectionStart;
                string selectedText = rtb.SelectedText;

                UpdateLetterCycle(selectedText);

                switch (caseCycle) {
                    case 0:
                        rtb.SelectedText = selectedText.ToLower(); // all lower case
                        break;
                    case 1:
                        rtb.SelectedText = char.ToUpper(selectedText[0]) + selectedText.Substring(1).ToLower(); // Only first character upper case
                        break;
                    case 2:
                        rtb.SelectedText = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rtb.SelectedText.ToLower()); // Title Case
                        break;
                    case 3:
                        rtb.SelectedText = selectedText.ToUpper(); // ALL CAPS
                        break;
                    default:
                        break;
                }

                caseCycle = (caseCycle + 1) % 4;
                rtb.Select(selectionStart, selectedText.Length);
            }
        }

        private void UpdateLetterCycle(string text) {
            if (text.Equals(text.ToLower())) {
                caseCycle = 1;
            }
            else if (text.Equals(text.ToUpper())) {
                caseCycle = 0;
            }
            else if (text.Equals(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower()))) {
                caseCycle = 3;
            }
            else {
                caseCycle = 2;
            }
        }

        private bool hasPreset = false;
        private void rtb_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    if (hasPreset) { // clear presets on next line
                        hasPreset = false;
                        rtb.SelectionFont = new Font(cbFonts.Text, 12f, FontStyle.Regular);
                        rtb.SelectionColor = rtb.ForeColor;
                    }
                    break;

                case Keys.Escape:
                    rtb.Select(); // return to text editor
                    break;

                case Keys.A when e.Modifiers == Keys.Alt: // open align menu
                    e.SuppressKeyPress = true;
                    allignToolStripMenuItem.ShowDropDown();
                    allignToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.T when e.Modifiers == Keys.Alt: // open effects menu
                    e.SuppressKeyPress = true;
                    effectsToolStripMenuItem.ShowDropDown();
                    effectsToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.Y when e.Modifiers == Keys.Alt: // open text style menu
                    e.SuppressKeyPress = true;
                    styleToolStripMenuItem.ShowDropDown();
                    styleToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.S when e.Modifiers == Keys.Alt: // open search menu
                    e.SuppressKeyPress = true;
                    spltbtnSearch.ShowDropDown();
                    spltbtnSearch.DropDownItems[0].Select();
                    break;

                case Keys.H when e.Modifiers == Keys.Alt: // open history menu
                    e.SuppressKeyPress = true;
                    documentHistoryToolStripMenuItem.ShowDropDown();
                    documentHistoryToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.F when e.Modifiers == Keys.Alt: // open file menu
                    e.SuppressKeyPress = true;
                    fileToolStripMenuItem.ShowDropDown();
                    fileToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.E when e.Modifiers == Keys.Alt: // open edit menu
                    e.SuppressKeyPress = true;
                    editToolStripMenuItem.ShowDropDown();
                    editToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.V when e.Modifiers == Keys.Alt: // open view menu
                    e.SuppressKeyPress = true;
                    viewToolStripMenuItem.ShowDropDown();
                    viewToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.I when e.Modifiers == Keys.Alt: // open insert menu
                    e.SuppressKeyPress = true;
                    insertToolStripMenuItem.ShowDropDown();
                    insertToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.O when e.Modifiers == Keys.Alt: // open format menu
                    e.SuppressKeyPress = true;
                    formatToolStripMenuItem.ShowDropDown();
                    formatToolStripMenuItem.DropDownItems[0].Select();
                    break;

                case Keys.C when e.Modifiers == Keys.Alt: // open color selector
                    e.SuppressKeyPress = true;
                    spltbtnForeColor.ShowDropDown();
                    spltbtnForeColor.DropDownItems[0].Select();
                    break;

                case Keys.M when e.Modifiers == Keys.Alt: // open color marker selector
                    e.SuppressKeyPress = true;
                    spltbtnBackColor.ShowDropDown();
                    spltbtnBackColor.DropDownItems[0].Select();
                    break;

                case Keys.Z when e.Modifiers == Keys.Alt: // open zoom menu
                    e.SuppressKeyPress = true;
                    zoomToolStripMenuItem.ShowDropDown();
                    zoomToolStripMenuItem.DropDownItems[0].Select();
                    break;
                case Keys.OemPeriod when e.Modifiers == Keys.Alt:
                    specialCharacterToolStripMenuItem.ShowDropDown();
                    specialCharacterToolStripMenuItem.DropDownItems[0].Select();
                    break;
            }
            lblZoomFactor.Text = (rtb.ZoomFactor * 100).ToString("0") + "%";
        }
        private void rtb_KeyPress(object sender, KeyPressEventArgs e) {
            if (Settings.GetSetting("closeBrackets") == "True") {
                switch (e.KeyChar) {
                    case '(':
                        closeBracket("(", ")");   // (
                        e.Handled = true;
                        break;
                    case '[':
                        closeBracket("[", "]");   // [
                        e.Handled = true;
                        break;
                    case '{':
                        closeBracket("{", "}");   // {
                        e.Handled = true;
                        break;
                    case '\'':
                        closeBracket("'", "'");   // '
                        e.Handled = true;
                        break;
                    case '\"':
                        closeBracket("\"", "\""); // "
                        e.Handled = true;
                        break;
                    case '<':
                        closeBracket("<", ">");   // <
                        e.Handled = true;
                        break;
                }
            }
        }
        private void closeBracket(string opening, string closing) {
            int selectionIndex = rtb.SelectionStart;
            rtb.Text = rtb.Text.Insert(selectionIndex, opening + closing);
            rtb.SelectionStart = selectionIndex + 1;
        }
        private void titleToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 24f, FontStyle.Regular);
            rtb.SelectionColor = Color.Black;
            rtb.SelectionAlignment = HorizontalAlignment.Center;
            hasPreset = true;
        }

        private void captionToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 18f, FontStyle.Regular);
            rtb.SelectionColor = Color.FromArgb(102, 102, 102);
            rtb.SelectionAlignment = HorizontalAlignment.Center;
            hasPreset = true;
        }

        private void title1ToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 24f, FontStyle.Regular);
            rtb.SelectionColor = theme.documentTitle;
            hasPreset = true;
        }

        private void title2ToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 18f, FontStyle.Regular);
            rtb.SelectionColor = theme.documentTitle;
            hasPreset = true;
        }

        private void title3ToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 14f, FontStyle.Regular);
            rtb.SelectionColor = theme.documentTitle;
            hasPreset = true;
        }
        #endregion
        #region misc
        // miscellaneous items related to UI interactions
        private void rtb_LinkClicked(object sender, LinkClickedEventArgs e) {
            if (MessageBox.Show("Open " + e.LinkText + " on a new window?", "Open link", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                try {
                    Process.Start(new ProcessStartInfo(e.LinkText) { UseShellExecute = true });
                }
                catch {
                    sendNotification("Failed to open link", "error");
                }
            }
        }
        private void Form1_Resize(object sender, EventArgs e) {
            if (this.Size.Width < 327) {
                menuStrip.Hide();
                tsTool.Hide();
                tsFormat.Hide();
                statusStrip.Hide();
                pnlExplorer.Hide();
                splitter1.Hide();
                TopMost = true;
                MinimizeBox = false;
                MaximizeBox = false;
            }
            else {
                menuStrip.Show();
                if (toolBarToolStripMenuItem.Checked) {
                    tsTool.Show();
                }
                if (formatBarToolStripMenuItem.Checked) {
                    tsFormat.Show();
                }
                if (statusBarToolStripMenuItem.Checked) {
                    statusStrip.Show();
                }
                if (explorerToolStripMenuItem.Checked) {
                    pnlExplorer.Show();
                    splitter1.Show();
                }
                TopMost = false;
                MinimizeBox = true;
                MaximizeBox = true;
            }

            if (splitter1.SplitPosition >= this.Size.Width) {
                splitter1.SplitPosition = splitter1.SplitPosition;
            }
        }
        private void stickyNoteToolStripMenuItem_Click(object sender, EventArgs e) {
            this.WindowState = FormWindowState.Normal;
            this.Size = new Size(326, this.Size.Height);
        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (rtb.SelectedText != "") {
                cutToolStripMenuItem1.Enabled = true;
                copyToolStripMenuItem1.Enabled = true;
                deleteToolStripMenuItem1.Enabled = true;
                clearFormattingToolStripMenuItem1.Enabled = true;
            }
            else {
                cutToolStripMenuItem1.Enabled = false;
                copyToolStripMenuItem1.Enabled = false;
                deleteToolStripMenuItem1.Enabled = false;
                clearFormattingToolStripMenuItem1.Enabled = false;
            }
        }
        #endregion
        #region paper margins
        private void SetRightMargin(double width) {
            // convert to pixels (1 mm = 3.78 pixels)
            int margemDireitaPixels = (int)(width * 3.78);

            rtb.RightMargin = margemDireitaPixels;
        }
        private void UncheckAllMenuItems() {
            foreach (ToolStripMenuItem item in marginToolStripMenuItem.DropDownItems) {
                item.Checked = false;
            }
        }
        private void a4ToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllMenuItems();
            a4ToolStripMenuItem.Checked = true;
            SetRightMargin(210);
        }

        private void cardPaperToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllMenuItems();
            cardPaperToolStripMenuItem.Checked = true;
            SetRightMargin(8.5 * 25.4);
        }
        private void noneToolStripMenuItem_Click(object sender, EventArgs e) {
            UncheckAllMenuItems();
            noneToolStripMenuItem.Checked = true;
            SetRightMargin(0);
        }
        #endregion
        #region printing
        private void printDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            sendNotification("Printing failed", "error");
        }
        #endregion
        #region insert
        private void insertObject(string objectName) {
            try {
                string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                string obj = Path.Combine(defaultDirectory, "presets", objectName);

                if (File.Exists(obj)) {
                    int currentSelectionStart = rtb.SelectionStart;
                    string rtfContent = File.ReadAllText(obj);
                    rtb.SelectedRtf = rtfContent;
                }
                else {
                    sendNotification("Object not found", "error");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Failed to insert object: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chartToolStripMenuItem_Click(object sender, EventArgs e) {
            insertObject("chart.owo");
        }

        private void equationToolStripMenuItem_Click(object sender, EventArgs e) {
            insertObject("equation.owo");
        }
        private void drawingToolStripMenuItem_Click(object sender, EventArgs e) {
            insertObject("drawing.owo");
        }
        private void spreadsheetToolStripMenuItem_Click(object sender, EventArgs e) {
            int columns = 0; // number of horizontal columns
            int lines = 0; // number of vertical lines

            try {
                // tries to set the number of columns and lines from user input
                columns = int.Parse(inputbox.GetInput("Insert the number of columns:", "Spreadsheet size - OwOrdPad", [], Resources.spreadsheet));
                lines = int.Parse(inputbox.GetInput("Insert the number of lines:", "Spreadsheet size - OwOrdPad", [], Resources.spreadsheet));
            }
            catch {
                sendNotification("Invalid size", "error");
                return;
            }

            if (lines <= 0 || columns <= 0 || lines > 100 || columns > 100) {
                // checks if the number of lines or columns is invalid (less than or equal to 0, or greater than 100)
                sendNotification("Invalid size", "warning");
                return;
            }

            string rtfHeader = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1046"; // start of the RTF format file
            StringBuilder tableBuilder = new StringBuilder();

            int cellWidth = 5000 / columns; // horizontal size of each cell

            for (int i = 0; i < lines; i++) {
                tableBuilder.Append(@"\trowd\trgaph108"); // starts a new table row

                for (int j = 0; j < columns; j++) {
                    int cellPosition = (j + 1) * cellWidth; // position of the cell in the current row
                    tableBuilder.Append($@"\clvertalc\cellx{cellPosition}"); // adds cell definition
                }

                tableBuilder.Append(@"\pard\intbl"); // starts a paragraph within the table

                for (int j = 0; j < columns; j++) {
                    tableBuilder.Append(@"\cell"); // adds an empty cell
                }

                tableBuilder.Append(@"\row"); // ends the table row
            }
            string rtfFooter = @"}"; // end of the RTF format file

            rtb.SelectedRtf = rtfHeader + tableBuilder.ToString() + rtfFooter;
        }

        private void fromMyComputerToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string imagePath = openFileDialog.FileName;
                try {
                    Image image = Image.FromFile(imagePath);
                    InsertImage(image);
                }
                catch {
                    sendNotification("Invalid image file", "error");
                }
            }
        }
        private void fromURLToolStripMenuItem_Click(object sender, EventArgs e) {
            string imageUrl = inputbox.GetInput("Insert an image URL: ", "Open URL - OwOrdPad", [], Resources.search, Clipboard.GetText());

            if (!string.IsNullOrEmpty(imageUrl)) {
                try {
                    using (WebClient webClient = new()) {
                        byte[] data = webClient.DownloadData(imageUrl);
                        Image image = Image.FromStream(new MemoryStream(data));
                        InsertImage(image);
                    }
                }
                catch {
                    sendNotification("Image download failed", "error");
                }
            }
        }
        private void InsertImage(Image image) {
            Clipboard.SetImage(image); // i hate this so fucking much
            rtb.Paste();
        }
        private void timeAndDateToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectedText = DateTime.Now.ToString("HH:mm dd/MM/yyyy") + " ";
        }
        private void specialCharacterToolStripMenuItem_Click(object sender, EventArgs e) {
            frmSpecialCharacter sc = new();
            rtb.SelectedText = sc.getChars();
        }
        private void insertSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectedText = toolStripComboBox1.Text;
        }
        private void linkToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = inputbox.GetInput("Insert the website name", "Insert link - OwOrdPad", [], Resources.ok, Clipboard.GetText());
            if (name != "") {
                string link = inputbox.GetInput("Insert " + name + "'s URL", "Insert link - OwOrdPad", [], Resources.ok, Clipboard.GetText());

                string linkDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + "\\presets", "link.owo");

                if (name != "" && link != "") {
                    File.WriteAllText(linkDir, "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1046{\\fonttbl{\\f0\\fswiss\\fprq2\\fcharset0 Calibri;}{\\f1\\fnil Calibri;}}\r\n{\\colortbl ;\\red0\\green102\\blue255;\\red5\\green99\\blue204;}\r\n{\\*\\generator Riched20 10.0.22621}\\viewkind4\\uc1 \r\n\\pard\\widctlpar\\sa160\\sl252\\slmult1 {\\f0\\fs22{\\field{\\*\\fldinst{HYPERLINK \"" + link + "\" }}{\\fldrslt{\\ul\\cf1\\cf2\\ul " + name + "}}}}\\f1\\fs24\\par\r\n}");
                    insertObject("link.owo");
                    // optional, resets the default link file to a placeholder, so the last link inserted isn't saved
                    File.WriteAllText(linkDir, "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang1046{\\fonttbl{\\f0\\fswiss\\fprq2\\fcharset0 Calibri;}{\\f1\\fnil Calibri;}}\r\n{\\colortbl ;\\red0\\green102\\blue255;\\red5\\green99\\blue204;}\r\n{\\*\\generator Riched20 10.0.22621}\\viewkind4\\uc1 \r\n\\pard\\widctlpar\\sa160\\sl252\\slmult1 {\\f0\\fs22{\\field{\\*\\fldinst{HYPERLINK \"about:blank\" }}{\\fldrslt{\\ul\\cf1\\cf2\\ul blank}}}}\\f1\\fs24\\par\r\n}");
                }
            }
        }

        private void rTFDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new() {
                Filter = "Rich Text File|*.rtf|OwOrdPad Markdown|*.OwO"
            };
            ofd.ShowDialog();
            if (File.Exists(ofd.FileName)) {
                insertObject(ofd.FileName);
            }
        }
        #endregion  
        private async void sendNotification(string message, string type = "") {
            switch (type) {
                case "succes":
                    lblNotification.ForeColor = theme.sucess;
                    lblNotification.Image = Resources.succes;
                    lblNotification.Image = theme.paintBitmap(lblNotification.Image, theme.sucess);
                    break;
                case "warning":
                    lblNotification.ForeColor = theme.warning;
                    lblNotification.Image = Resources.warning;
                    lblNotification.Image = theme.paintBitmap(lblNotification.Image, theme.warning);
                    break;
                case "error":
                    lblNotification.ForeColor = theme.error;
                    lblNotification.Image = Resources.error;
                    lblNotification.Image = theme.paintBitmap(lblNotification.Image, theme.error);
                    break;
                default:
                    lblNotification.ForeColor = Color.Black;
                    lblNotification.Image = null;
                    break;
            }
            lblNotification.ToolTipText = message;
            lblNotification.Text = message;
            lblNotification.Visible = true;

            await Task.Delay(3000);
            lblNotification.Visible = false;
        }
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e) {
            Process.Start("owordpad");
        }

        private void selectLineToolStripMenuItem_Click(object sender, EventArgs e) {
            int currentLineIndex = rtb.GetLineFromCharIndex(rtb.SelectionStart);
            int lineStart = rtb.GetFirstCharIndexFromLine(currentLineIndex);
            int lineEnd = rtb.GetFirstCharIndexFromLine(currentLineIndex + 1);

            if (lineEnd == -1) {
                lineEnd = rtb.Text.Length;
            }

            rtb.Select(lineStart, lineEnd - lineStart);
        }

        private void openAutoSaveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (isDocumentModified && MessageBox.Show("Are you sure you want to restore a document?\nYou may lose unsaved changes on your current file.", "Restore document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }
            try {
                OpenFile(defaultDirectory + "\\Autosave\\save.rtf");
                treeExplorer.Nodes.Clear();
            }
            catch {
                sendNotification("Failed to load autosave file", "error");
            }
        }
        private void cbFonts_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                contextMenuStrip2.Show(Cursor.Position);
            }
        }

        private void selectFont(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;
            string fontName = itm.Tag.ToString();

            if (FontFamily.Families.Any(f => f.Name == fontName)) {
                rtb.SelectionFont = new Font(fontName, rtb.SelectionFont.Size);
                cbFonts.Text = fontName;
            }
            else {
                sendNotification("The font '" + fontName + "' doesn't exists", "warning");
            }
        }

        private void selectLineSpacing(object sender, EventArgs e) {
            foreach (ToolStripMenuItem menuItem in spltbtnLineSpace.DropDownItems) {
                menuItem.Checked = false; // uncheck all menu items
            }
            ToolStripMenuItem itm = sender as ToolStripMenuItem;
            rtb.SelectionCharOffset = int.Parse(itm.Tag.ToString()); // set the selected char offset as the clicked menu item tag says
            itm.Checked = true; // check the selected menu item
        }

        private void viewInFileExplorerToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(filePath)) {
                Process.Start("explorer.exe", "/select,\"" + filePath + "\"");
            }
            else {
                sendNotification("This document doesn't have a file", "warning");
            }
        }
        private void cloneDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(filePath)) {
                string directory = Path.GetDirectoryName(filePath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
                string fileExtension = Path.GetExtension(filePath);

                int copyIndex = 1;
                string newFilePath = Path.Combine(directory, $"{fileNameWithoutExtension} [{copyIndex}]{fileExtension}");

                while (File.Exists(newFilePath)) {
                    copyIndex++;
                    newFilePath = Path.Combine(directory, $"{fileNameWithoutExtension} ({copyIndex}){fileExtension}");
                }

                try {
                    File.Copy(filePath, newFilePath);
                    filePath = newFilePath;
                    Text = Path.GetFileName(newFilePath) + " - OwOrdPad";
                    sendNotification("Document cloned as " + Path.GetFileName(newFilePath), "succes");
                }
                catch {
                    sendNotification("Error cloning document", "error");
                }
            }
            else {
                sendNotification("This document doesn't have a file", "warning");
            }
        }

        string[] knownColors = Enum.GetNames(typeof(KnownColor));
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e) {
            try {
                string color = inputbox.GetInput("Insert a color #HEX, R;G;B or Name value: ", "Text color - OwOrdPad", knownColors, Resources.color, ColorTranslator.ToHtml(rtb.SelectionColor));
                rtb.SelectionColor = getColor(color, rtb.SelectionColor);
            }
            catch { }
        }
        private void textHighlightColorToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                string color = inputbox.GetInput("Insert a color #HEX, R;G;B or Name value: ", "Highlight color - OwOrdPad", knownColors, Resources.color, ColorTranslator.ToHtml(rtb.SelectionBackColor));
                rtb.SelectionBackColor = getColor(color, rtb.SelectionBackColor);
            }
            catch { }
        }
        private void toolStripSplitButton1_ButtonClick_1(object sender, EventArgs e) {
            try {
                string color = inputbox.GetInput("Insert a color #HEX, R;G;B or Name value: ", "Text color - OwOrdPad", knownColors, Resources.color, ColorTranslator.ToHtml(rtb.SelectionColor));
                rtb.SelectionColor = getColor(color, rtb.SelectionColor);
            }
            catch { }
        }
        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e) {
            try {
                string color = inputbox.GetInput("Insert a color #HEX, R;G;B or Name value: ", "Highlight color - OwOrdPad", knownColors, Resources.color, ColorTranslator.ToHtml(rtb.SelectionBackColor));
                rtb.SelectionBackColor = getColor(color, rtb.SelectionBackColor);
            }
            catch { }
        }
        private void selectForeColor(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;

            string[] rgbValues = itm.Tag.ToString().Split(' ');
            int r = int.Parse(rgbValues[0]);
            int g = int.Parse(rgbValues[1]);
            int b = int.Parse(rgbValues[2]);

            rtb.SelectionColor = Color.FromArgb(r, g, b);
        }

        private void selectBackColor(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;

            string[] rgbValues = itm.Tag.ToString().Split(' ');
            int r = int.Parse(rgbValues[0]);
            int g = int.Parse(rgbValues[1]);
            int b = int.Parse(rgbValues[2]);

            rtb.SelectionBackColor = Color.FromArgb(r, g, b);
        }
        private Color getColor(string input, Color blankColor) {
            try {
                if (input.StartsWith('#') && input.Length == 7) {
                    // HEX
                    return ColorTranslator.FromHtml(input);
                }
                else if (input.Contains(';')) {
                    // RGB
                    string[] rgbValues = input.Split(';');
                    if (rgbValues.Length == 3) {
                        int r = int.Parse(rgbValues[0]);
                        int g = int.Parse(rgbValues[1]);
                        int b = int.Parse(rgbValues[2]);
                        if (r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255) {
                            return Color.FromArgb(r, g, b);
                        }
                    }
                }
                else {
                    // Known Color
                    Color color = Color.FromName(input);
                    if (color.IsKnownColor) {
                        return color;
                    }
                }
            }
            catch { }
            return blankColor;
        }
        private void selectWordToolStripMenuItem_Click(object sender, EventArgs e) {
            int start = rtb.SelectionStart;
            while (start > 0 && !char.IsWhiteSpace(rtb.Text[start - 1])) {
                start--;
            }

            int end = rtb.SelectionStart;
            while (end < rtb.Text.Length - 1 && !char.IsWhiteSpace(rtb.Text[end + 1])) {
                end++;
            }

            rtb.Select(start, end - start + 1);
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.SelectionBullet == false) {
                rtb.SelectionBullet = true;
            }
            else {
                rtb.SelectionBullet = false;
            }
        }

        private void lineSpacingToolStripMenuItem_Click(object sender, EventArgs e) {
            spltbtnLineSpace.ShowDropDown();
        }

        #region history
        private List<string> history = new List<string>();
        private const string historyFile = "Settings\\history";
        private void saveHistory() {
            File.WriteAllLines(historyFile, history);
        }
        private void loadHistoryList() {
            if (File.Exists(historyFile)) {
                history = new List<string>(File.ReadAllLines(historyFile));
            }
        }
        private void updateHistory(string path) {
            if (history.Contains(path)) {
                history.Remove(path); // remove the item from the list
            }

            history.Insert(0, path); // add/re-add the item, to the top of the list

            if (history.Count > 20) {
                history.RemoveAt(history.Count - 1); // remove an item, in case of it being past the 20 items hard-limit
            }
        }
        private void updateHistoryMenu() {
            documentHistoryToolStripMenuItem.DropDownItems.Clear();

            foreach (var filePath in history) {
                try {
                    string fileName = Path.GetFileName(filePath);
                    if (fileName == string.Empty) {
                        fileName = filePath;
                    }

                    DateTime lastWriteTime = File.GetLastWriteTime(filePath);
                    TimeSpan timeAgo = DateTime.Now - lastWriteTime;
                    string timeAgoString;

                    if (timeAgo.TotalDays >= 1) {
                        timeAgoString = $"{(int)timeAgo.TotalDays} days ago";
                    }
                    else if (timeAgo.TotalHours >= 1) {
                        timeAgoString = $"{(int)timeAgo.TotalHours} hours ago";
                    }
                    else if (timeAgo.TotalMinutes >= 1) {
                        timeAgoString = $"{(int)timeAgo.TotalMinutes} minutes ago";
                    }
                    else {
                        timeAgoString = "Just now";
                    }

                    ToolStripMenuItem recentFileItem = new(fileName) {
                        Tag = filePath,
                        ToolTipText = ("Full path: " + filePath +
                            "\nDate modified: " + File.GetLastWriteTime(filePath).ToString("dd/MM/yyyy HH:mm") +
                            "\nDate created: " + File.GetCreationTime(filePath).ToString("dd/MM/yyyy HH:mm")),
                        ShortcutKeyDisplayString = timeAgoString,
                    };
                    if (File.Exists(filePath)) {
                        recentFileItem.Image = Resources.document;
                    }
                    else if (Directory.GetDirectoryRoot(filePath) == filePath) {
                        recentFileItem.Image = Resources.diskSpace;
                    }
                    else if (Directory.Exists(filePath)) {
                        recentFileItem.Image = Resources.folder;
                    }
                    else {
                        recentFileItem.Image = Resources.questionMark;
                    }

                    recentFileItem.Click += historyItemClick;
                    documentHistoryToolStripMenuItem.DropDownItems.Add(recentFileItem);
                }
                catch { }
            }
            documentHistoryToolStripMenuItem.DropDownItems.Add(toolStripSeparator46);
            documentHistoryToolStripMenuItem.DropDownItems.Add(manageHistoryToolStripMenuItem);
        }
        private void manageHistoryToolStripMenuItem_Click(object sender, EventArgs e) {
            string historyPath = defaultDirectory + "\\Settings\\history";
            string[] history = File.ReadAllLines(historyPath);

            try {
                history = listManager.getList("History - OwOrdPad", history, null, frmListManager.addType.FromString, false, true, false);
            }
            catch { return; }

            File.WriteAllLines(historyPath, history);
            loadHistoryList();
            updateHistoryMenu();
        }
        private async void historyItemClick(object sender, EventArgs e) {
            if (isDocumentModified && MessageBox.Show("Are you sure you want to open a document?\nYou will lose any unsaved changes on your current file.",
                "Open document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string path = menuItem.Tag.ToString();

            try {
                OpenFile(path);
                treeExplorer.Nodes.Clear();
            }
            catch {
                try { await LoadDirectory(path); }
                catch {
                    if (MessageBox.Show("This document doesn't exists\nDo you want to remove this item from the list?", "Document not found",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                        history.Remove(path);
                        saveHistory();
                        updateHistoryMenu();
                    }
                }
            }
        }
        #endregion

        private void indentationSizeToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                int[] i = [int.Parse(optionBox.getOption("Tab spacing - OwOrdPad", "Select the tab character's size:", ["10", "20", "30", "40", "50", "60", "70", "80"]))];
                rtb.SelectionTabs = i;
            }
            catch { }
        }

        private void copyFormattingToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.SelectionLength > 0) {
                Font selectionFont = rtb.SelectionFont;
                Color selectionColor = rtb.SelectionColor;
                Color selectionBackColor = rtb.SelectionBackColor;

                DataFormats.Format format = DataFormats.GetFormat(typeof(Font).FullName);
                DataObject dataObject = new DataObject();
                dataObject.SetData(format.Name, false, selectionFont);
                dataObject.SetData(DataFormats.Text, rtb.SelectedText);
                dataObject.SetData("RTF", rtb.SelectedRtf);
                dataObject.SetData("SelectionColor", ColorTranslator.ToHtml(selectionColor));
                dataObject.SetData("SelectionBackColor", ColorTranslator.ToHtml(selectionBackColor));
                Clipboard.SetDataObject(dataObject, true);
            }
        }

        private void pasteFormattingToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                Font font = (Font)Clipboard.GetData(typeof(Font).FullName);
                string selectionColorHex = (string)Clipboard.GetData("SelectionColor");
                string selectionBackColorHex = (string)Clipboard.GetData("SelectionBackColor");
                Color selectionColor = ColorTranslator.FromHtml(selectionColorHex);
                Color selectionBackColor = ColorTranslator.FromHtml(selectionBackColorHex);

                rtb.SelectionFont = font;
                rtb.SelectionColor = selectionColor;
                rtb.SelectionBackColor = selectionBackColor;
            }
            catch {
                sendNotification("Invalid format", "error");
            }
        }

        private void toolTipsToolStripMenuItem_Click(object sender, EventArgs e) {
            bool showToolTips = toolTipsToolStripMenuItem.Checked;

            UpdateToolTipsForMenuItems(menuStrip.Items, showToolTips);

            Settings.SaveSetting("showToolTips", showToolTips.ToString());
        }

        private void UpdateToolTipsForMenuItems(ToolStripItemCollection items, bool showToolTips) {
            menuStrip.ShowItemToolTips = showToolTips;
            tsTool.ShowItemToolTips = showToolTips;
            tsFormat.ShowItemToolTips = showToolTips;
            statusStrip.ShowItemToolTips = showToolTips;

            foreach (ToolStripItem item in items) {
                if (item is ToolStripMenuItem menuItem) {
                    if (menuItem.HasDropDownItems) {
                        menuItem.DropDown.ShowItemToolTips = showToolTips;
                        UpdateToolTipsForMenuItems(menuItem.DropDownItems, showToolTips);
                    }
                }
            }
        }

        private void defaultFontToolStripMenuItem_Click(object sender, EventArgs e) {
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            FontFamily[] fontFamilies = installedFonts.Families;
            string[] fontNames = new string[fontFamilies.Length];

            for (int i = 0; i < fontFamilies.Length; i++) {
                fontNames[i] = fontFamilies[i].Name;
            }
            string fontName = inputbox.GetInput("Select the default OwOrdPad font:", "Default font - OwOrdPad", fontNames, Resources.ok, Settings.GetSetting("defaultFont"));

            if (fontName != string.Empty) {
                Settings.SaveSetting("defaultFont", fontName);
            }
            defaultFontToolStripMenuItem.ShortcutKeyDisplayString = Settings.GetSetting("defaultFont");
        }
        private void themeToolStripMenuItem_Click(object sender, EventArgs e) {
            string themesPath = defaultDirectory + "\\Themes\\";
            string[] installedThemesPaths = Directory.GetFiles(themesPath);

            string[] installedThemes = installedThemesPaths.Select(Path.GetFileName).ToArray();

            string selectedTheme = optionBox.getOption("Theme - OwOrdPad", "Select a theme to OwOrdPad:", installedThemes);

            if (selectedTheme == string.Empty) return;

            if (File.Exists(themesPath + selectedTheme)) {
                Settings.SaveSetting("theme", selectedTheme);
                theme.setTheme(selectedTheme);
                loadVisuals();
            }
            else {
                sendNotification("Unknown theme", "warning");
            }
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.SelectionFont.Size < 96) {
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, rtb.SelectionFont.Size + 1);
            }
            else {
                sendNotification("Font cannot be greater than 96", "warning");
            }
            cbFontSize.Text = rtb.SelectionFont.Size.ToString();
            fontSizeToolStripMenuItem.ShowDropDown();
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e) {

            if (rtb.SelectionFont.Size > 8) {
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, rtb.SelectionFont.Size - 1);
            }
            else {
                sendNotification("Font cannot be smaller than 8", "warning");
            }
            cbFontSize.Text = rtb.SelectionFont.Size.ToString();
            fontSizeToolStripMenuItem.ShowDropDown();
        }
        private void ShowHome(object sender, EventArgs e) {
            if (Settings.GetSetting("showHomeScreen") == "True") {
                frmStartScreen home = new();
                string path = home.getDocument();
                if (File.Exists(path)) {
                    try {
                        OpenFile(path);

                        updateHistory(path);
                        updateHistoryMenu();
                        saveHistory();
                    }
                    catch { }
                }
            }
            else { homeScreenToolStripMenuItem.Checked = false; }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                treeExplorer.SelectedNode.BeginEdit();
            }
            else { sendNotification("Nothing to rename", "warning"); }
        }

        private void treeFiles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if (e.Node.Tag is string oldFilePath && !string.IsNullOrEmpty(e.Label)) {
                string newFilePath = Path.Combine(Path.GetDirectoryName(oldFilePath), e.Label);

                try {
                    if (File.Exists(oldFilePath)) {
                        File.Move(oldFilePath, newFilePath);
                        e.Node.Tag = newFilePath;
                        if (filePath == oldFilePath) {
                            filePath = newFilePath;
                            this.Text = Path.GetFileName(newFilePath) + " - OwOrdPad";
                        }
                    }
                    else if (Directory.Exists(oldFilePath)) {
                        Directory.Move(oldFilePath, newFilePath);
                        e.Node.Tag = newFilePath;
                    }
                }
                catch {
                    sendNotification("Invalid file name", "error");
                    e.CancelEdit = true;
                }
            }
            else {
                e.CancelEdit = true;
            }
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                string directoryPath = treeExplorer.SelectedNode.Tag as string;
                if (directoryPath != null && Directory.Exists(directoryPath)) {
                    string newFileName = "New document";
                    string newFilePath = Path.Combine(directoryPath, newFileName);
                    int counter = 1;

                    while (File.Exists(newFilePath)) {
                        newFileName = $"New document{counter}";
                        newFilePath = Path.Combine(directoryPath, newFileName);
                        counter++;
                    }

                    File.Create(newFilePath).Close();
                    filePath = newFilePath;

                    var newNode = new TreeNode(newFileName, 1, 1) { Tag = newFilePath };
                    treeExplorer.SelectedNode.Nodes.Add(newNode);
                    treeExplorer.SelectedNode.Expand();
                    treeExplorer.SelectedNode = newNode;
                    newNode.BeginEdit();
                }
                else { sendNotification("Can not create a file here", "warning"); }
            }
            else { sendNotification("No folder selected", "warning"); }
        }
        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                string directoryPath = treeExplorer.SelectedNode.Tag as string;
                if (directoryPath != null && Directory.Exists(directoryPath)) {
                    string newFolderName = "New folder";
                    string newFolderPath = Path.Combine(directoryPath, newFolderName);
                    int counter = 1;

                    while (Directory.Exists(newFolderPath)) {
                        newFolderName = $"New folder{counter}";
                        newFolderPath = Path.Combine(directoryPath, newFolderName);
                        counter++;
                    }

                    Directory.CreateDirectory(newFolderPath);

                    var newNode = new TreeNode(newFolderName, 0, 0) { Tag = newFolderPath };
                    treeExplorer.SelectedNode.Nodes.Add(newNode);
                    treeExplorer.SelectedNode.Expand();
                    treeExplorer.SelectedNode = newNode;
                    newNode.BeginEdit();
                }
                else { sendNotification("Can not create a folder here", "warning"); }
            }
            else { sendNotification("No folder selected", "warning"); }
        }

        private string clipboardPath;
        private bool cuttingFile;
        private void cutToolStripMenuItem2_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                clipboardPath = treeExplorer.SelectedNode.Tag as string;
                cuttingFile = true;
            }
        }

        private void copyToolStripMenuItem2_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                clipboardPath = treeExplorer.SelectedNode.Tag as string;
                cuttingFile = false;
            }
        }

        private void pasteToolStripMenuItem2_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null && clipboardPath != null) {
                string destinationPath = treeExplorer.SelectedNode.Tag as string;
                if (Directory.Exists(destinationPath)) {
                    string fileName = Path.GetFileName(clipboardPath);
                    string destinationFilePath = Path.Combine(destinationPath, fileName);

                    try {
                        if (cuttingFile) {
                            if (File.Exists(clipboardPath)) {
                                File.Move(clipboardPath, destinationFilePath);
                            }
                            else if (Directory.Exists(clipboardPath)) {
                                Directory.Move(clipboardPath, destinationFilePath);
                            }
                            updateExplorer();
                        }
                        else {
                            if (File.Exists(clipboardPath)) {
                                File.Copy(clipboardPath, destinationFilePath);
                            }
                            else if (Directory.Exists(clipboardPath)) {
                                CopyDirectory(clipboardPath, destinationFilePath);
                            }
                            updateExplorer();
                        }
                    }
                    catch (System.IO.IOException) {
                        sendNotification("Invalid file name", "warning");
                    }
                    catch (Exception ex) {
                        sendNotification($"Failed to paste here {ex.Message}", "warning");
                    }
                }
            }
        }
        private TreeNode CreateTreeNode(DirectoryInfo directoryInfo) {
            var directoryNode = new TreeNode(directoryInfo.Name, 0, 0) { Tag = directoryInfo.FullName };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(CreateTreeNode(directory));
            foreach (var file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name, 1, 1) { Tag = file.FullName });
            return directoryNode;
        }

        private void CopyDirectory(string sourceDir, string destDir) {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(destDir);
            foreach (FileInfo file in dir.GetFiles()) {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }
            foreach (DirectoryInfo subdir in dirs) {
                string tempPath = Path.Combine(destDir, subdir.Name);
                CopyDirectory(subdir.FullName, tempPath);
            }
        }

        private void selectFolderColor(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;
            string selectedColor = itm.Tag.ToString();

            if (treeExplorer.SelectedNode != null) {
                string directoryPath = treeExplorer.SelectedNode.Tag as string;
                if (directoryPath != null && Directory.Exists(directoryPath)) {
                    string filePath = Path.Combine(directoryPath, ".owordpadFolderColor");

                    try {
                        if (!File.Exists(filePath)) {
                            File.WriteAllText(filePath, selectedColor);
                            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
                        }
                        else {
                            File.SetAttributes(filePath, FileAttributes.Normal);
                            File.WriteAllText(filePath, selectedColor);
                            File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
                        }
                        updateExplorer();
                    }
                    catch {
                        sendNotification("Failed to change folder color", "error");
                    }
                }
            }
        }

        private void noColorToolStripMenuItem_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                string directoryPath = treeExplorer.SelectedNode.Tag as string;
                if (directoryPath != null && Directory.Exists(directoryPath)) {
                    string colorFilePath = Path.Combine(directoryPath, ".owordpadFolderColor");

                    try {
                        if (File.Exists(colorFilePath)) {
                            File.Delete(colorFilePath);
                            updateExplorer();
                        }
                    }
                    catch { }
                }
            }
        }

        private void deleteToolStripMenuItem2_Click(object sender, EventArgs e) {
            if (treeExplorer.SelectedNode != null) {
                string path = treeExplorer.SelectedNode.Tag as string;
                if (!string.IsNullOrEmpty(path)) {
                    if (MessageBox.Show("Are you sure you want to move '" + treeExplorer.SelectedNode.Tag + "' into the recycle bin?", "Delete file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                        return;
                    }
                    try {
                        if (File.Exists(path)) {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        else if (Directory.Exists(path)) {
                            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                        }
                        treeExplorer.SelectedNode.Remove();
                    }
                    catch {
                        sendNotification("Can not delete this file", "error");
                    }
                }
                else { sendNotification("Can not delete this file", "error"); }
            }
            else { sendNotification("Nothing to delete", "warning"); }

        }

        private void updateExplorer() {
            if (treeExplorer.SelectedNode != null) {
                List<string> rootPaths = new List<string>();
                foreach (TreeNode node in treeExplorer.Nodes) {
                    string path = node.Tag as string;
                    if (!string.IsNullOrEmpty(path)) {
                        rootPaths.Add(path);
                    }
                }

                bool allPathsExist = true;
                treeExplorer.Nodes.Clear();

                foreach (string rootPath in rootPaths) {
                    if (Directory.Exists(rootPath)) {
                        LoadDirectory(rootPath);
                        foreach (TreeNode node in treeExplorer.Nodes) {
                            if ((node.Tag as string) == rootPath) {
                                node.Expand();
                            }
                        }
                    }
                    else {
                        allPathsExist = false;
                        break;
                    }
                }

                if (!allPathsExist) {
                    treeExplorer.Nodes.Clear();
                    sendNotification("Failed to relocate directory", "error");
                }
            }
        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            updateExplorer();
        }

        string selectedFile = "";
        private void treeFiles_AfterSelect(object sender, TreeViewEventArgs e) {
            selectedFile = e.Node.Tag as string;
        }

        private void LoadSelectedFile() {
            if (selectedFile != null && File.Exists(selectedFile)) {
                if (isDocumentModified == true) {
                    if (MessageBox.Show("Are you sure you want to open this document?\nYou will lose any unsaved changes on your current file.", "Open document", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) {
                        return;
                    }
                }
                OpenFile(selectedFile);
            }
        }
        private void treeFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
            LoadSelectedFile();
        }

        private void treeFiles_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                e.SuppressKeyPress = true;
                if (treeExplorer.SelectedNode.IsExpanded) {
                    treeExplorer.SelectedNode.Collapse();
                }
                else {
                    treeExplorer.SelectedNode.Expand();
                }
                LoadSelectedFile();
            }
            if (e.KeyCode == Keys.Escape) {
                e.SuppressKeyPress = true;
                rtb.Select();
            }
        }

        private void expandAllToolStripMenuItem_Click(object sender, EventArgs e) {
            treeExplorer.ExpandAll();
        }

        private void collapseAllToolStripMenuItem_Click(object sender, EventArgs e) {
            treeExplorer.CollapseAll();
        }
        #region order lines
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e) {
            int currentLineIndex = rtb.GetLineFromCharIndex(rtb.SelectionStart); // line with the carret in

            if (currentLineIndex == 0) {
                // returns if the carret is at the first line, can't go upper
                SystemSounds.Exclamation.Play();
                return;
            }

            int upperLineIndex = currentLineIndex - 1; // first line above the carret

            int currentLineStart = rtb.GetFirstCharIndexFromLine(currentLineIndex); // position of the first character of the line in the doc
            int upperLineStart = rtb.GetFirstCharIndexFromLine(upperLineIndex);     // position of the upper line

            rtb.Select(currentLineStart, rtb.Lines[currentLineIndex].Length);
            string currentLineRtf = rtb.SelectedRtf; // rtf format info of the current line

            rtb.Select(upperLineStart, rtb.Lines[upperLineIndex].Length);
            string upperLineRtf = rtb.SelectedRtf; // rtf format info of the upper line

            // switches the two lines
            rtb.Select(upperLineStart, rtb.Lines[upperLineIndex].Length);
            rtb.SelectedRtf = currentLineRtf;
            currentLineStart = rtb.GetFirstCharIndexFromLine(currentLineIndex - 1);
            rtb.Select(currentLineStart + rtb.Lines[upperLineIndex].Length + 1, rtb.Lines[currentLineIndex].Length);
            rtb.SelectedRtf = upperLineRtf;

            // move selection to the new position
            rtb.Select(currentLineStart + rtb.Lines[upperLineIndex].Length, 0);
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e) {
            int currentLineIndex = rtb.GetLineFromCharIndex(rtb.SelectionStart); // line with the carret in number

            if (currentLineIndex >= rtb.Lines.Length - 1) {
                // returns if the carret is at the last line, can't go lower
                SystemSounds.Exclamation.Play();
                return;
            }

            int lowerLineIndex = currentLineIndex + 1; // first line bellow the carret number

            int currentLineStart = rtb.GetFirstCharIndexFromLine(currentLineIndex); // position of the first character of the line in the doc
            int currentLineLength = rtb.Lines[currentLineIndex].Length;             // lenght of the line with the carret on
            int lowerLineStart = rtb.GetFirstCharIndexFromLine(lowerLineIndex);     // position of the first characer of the next line in the doc
            int lowerLineLength = rtb.Lines[lowerLineIndex].Length;                 // lenght of the next line

            rtb.Select(currentLineStart, currentLineLength);
            string currentLineRtf = rtb.SelectedRtf;    // rtf format info of the current line

            rtb.Select(lowerLineStart, lowerLineLength);
            string lowerLineRtf = rtb.SelectedRtf;      // rtf format info of the next line

            // switches the two lines
            rtb.Select(lowerLineStart, lowerLineLength);
            rtb.SelectedRtf = currentLineRtf;
            int newCurrentLineStart = rtb.GetFirstCharIndexFromLine(currentLineIndex);
            rtb.Select(newCurrentLineStart, currentLineLength);
            rtb.SelectedRtf = lowerLineRtf;

            // move selection to the new position
            rtb.Select(newCurrentLineStart + lowerLineLength + 1, 0);
        }
        #endregion order lines
        private void treeFiles_DrawNode(object sender, DrawTreeNodeEventArgs e) {
            if (e.Node.IsSelected && treeExplorer.Focused) {
                e.Graphics.FillRectangle(new SolidBrush(theme.selectionHigh), e.Bounds);

                Rectangle borderRect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
                using (Pen borderPen = new Pen(theme.selectionBorder, 1)) {
                    e.Graphics.DrawRectangle(borderPen, borderRect);
                }
            }
            else {
                e.Graphics.FillRectangle(new SolidBrush(e.Node.TreeView.BackColor), e.Bounds);
            }

            TextRenderer.DrawText(e.Graphics, e.Node.Text, e.Node.NodeFont ?? e.Node.TreeView.Font, e.Bounds, treeExplorer.ForeColor);

            e.DrawDefault = false;
        }

        #region spell check
        private static int LevenshteinDistance(string s, string t) {
            int n = s.Length;
            int m = t.Length;
            var d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++) {
                for (int j = 1; j <= m; j++) {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
        public static List<string> GetMostSimilarStrings(string input, List<string> filePaths) {
            var lines = new List<string>();

            foreach (var filePath in filePaths) {
                if (File.Exists(filePath)) {
                    lines.AddRange(File.ReadAllLines(filePath));
                }
            }

            var similarities = new List<Tuple<string, int>>();

            foreach (var line in lines) {
                int distance = LevenshteinDistance(input, line);
                similarities.Add(new Tuple<string, int>(line, distance));
            }

            var sortedSimilarities = similarities.OrderBy(t => t.Item2).Take(4);
            return sortedSimilarities.Select(t => t.Item1).ToList();
        }
        private void showSimilarStrings(string input) {
            addWordToPersonalDictionaryToolStripMenuItem.Text = "Add '" + input + "' to dictionary";
            addWordToPersonalDictionaryToolStripMenuItem.Tag = input;
            addWordToDictionaryToolStripMenuItem.Text = "Add '" + input + "' to dictionary";
            addWordToDictionaryToolStripMenuItem.Tag = input;

            if (input.Length < 2 || bool.Parse(Settings.GetSetting("spellCheck")) == false) {
                spellcheck1.Visible = false;
                spellcheck2.Visible = false;
                spellcheck3.Visible = false;
                spellcheck4.Visible = false;
                toolStripSeparator51.Visible = false;
                spellCheckToolStripMenuItem1.Visible = false;
                return;
            }

            List<string> dictionaries = Directory.GetFiles(defaultDirectory + "\\Dictionaries\\").ToList();//[defaultDirectory + "\\Dictionaries\\default.txt", defaultDirectory + "\\Dictionaries\\personal.txt"];
            List<string> strings = GetMostSimilarStrings(input, dictionaries);

            spellcheck1.Text = strings[0];
            spellcheck2.Text = strings[1];
            spellcheck3.Text = strings[2];
            spellcheck4.Text = strings[3];
            spellcheck1.Visible = true;
            spellcheck2.Visible = true;
            spellcheck3.Visible = true;
            spellcheck4.Visible = true;
            toolStripSeparator51.Visible = true;
            spellCheckToolStripMenuItem1.Visible = true;
        }

        private void completeWord(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;
            rtb.SelectedText = itm.Text;
        }

        private void addWordToPersonalDictionaryToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem itm = sender as ToolStripMenuItem;
            if (itm.Tag.ToString().Length > 2) {
                List<string> per = File.ReadAllLines(defaultDirectory + "\\Dictionaries\\personal.txt").ToList();

                foreach(string s in per) {
                    if (s.Equals(itm.Tag.ToString())) {
                        sendNotification("This word is already in the dictionary", "warning");
                        return;
                    }
                }

                per.Add(itm.Tag.ToString());

                File.WriteAllLines(defaultDirectory + "\\Dictionaries\\personal.txt", per);
                sendNotification(itm.Tag.ToString() + " was added to the dictionary", "succes");
            }
            else {
                sendNotification("Needs to be at least 2 characters long", "warning");
            }
        }
        #endregion

        private void manageDictionaryToolStripMenuItem_Click(object sender, EventArgs e) {
            string[] per = File.ReadAllLines(defaultDirectory + "\\Dictionaries\\personal.txt");

            string[] newPer = listManager.getList("Personal dictionary - OwOrdPad", per, [], frmListManager.addType.FromString);

            File.WriteAllLines(defaultDirectory + "\\Dictionaries\\personal.txt", newPer);
        }
    }
}
