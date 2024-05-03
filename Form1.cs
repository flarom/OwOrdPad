using Microsoft.Win32;
using OwOrdPad.Properties;
using System.Diagnostics;
using System.Drawing.Text;
using System.Globalization;
using System.Media;
using System.Net;
using System.Text.RegularExpressions;

namespace OwOrdPad {
    public partial class Form1 : Form {
        Settings settings = new Settings();

        string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        public string filePath; // full file path to the opened/saved file
        public bool isDocumentModified = false; // checks if the document was modified by the user
        frmInputBox inputbox = new frmInputBox();

        public Form1() {
            // load program and settings
            InitializeComponent();
            LoadSettings();
            loadHistoryList();
            updateHistoryMenu();
            // load fonts
            InstalledFontCollection installedFontCollection = new();
            FontFamily[] fontFamilies = installedFontCollection.Families;

            foreach (FontFamily fontFamily in fontFamilies) {
                cbFonts.Items.Add(fontFamily.Name);
                cbFonts.AutoCompleteCustomSource.Add(fontFamily.Name);
            }

            // load custom visuals
            rtb.SelectionCharOffset = 1;

            menuStrip.Renderer = new MenuRenderer();
            tsTool.Renderer = new MenuRenderer();
            tsFormat.Renderer = new MenuRenderer();
            contextMenuStrip1.Renderer = new MenuRenderer();
            contextMenuStrip2.Renderer = new MenuRenderer();
        }
        private class MenuRenderer : ToolStripProfessionalRenderer {
            public MenuRenderer() : base(new CustomColors()) { }
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
            frmFavoriteFonts frm = new();
            frm.ShowDialog(this);

            //Process.Start("notepad", defaultDirectory + "\\settings\\favoriteFonts");
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
                rtb.WordWrap = bool.Parse(Settings.GetSetting("wordWrap"));
                rtb.ShowSelectionMargin = bool.Parse(Settings.GetSetting("selectionMargin"));
                rtb.Font = new Font(Settings.GetSetting("defaultFont"), 12);

                if (bool.Parse(Settings.GetSetting("wasClosedByUser")) == false) {
                    sendNotification("Use Ctrl+Shift+T to restore autosave", "warning");
                }
                Settings.SaveSetting("wasClosedByUser", "False");

                //indicators
                formatBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showFormatBar"));
                statusBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showStatusBar"));
                toolBarToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("showToolBar"));
                wordWrapToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("wordWrap"));
                selectionMarginToolStripMenuItem.Checked = bool.Parse(Settings.GetSetting("selectionMargin"));
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
                Filter = "Rich Text File|*.rtf|OwOdPad Markdown|*.owo|Plain Text File|*.txt|All Files|*.*",
                Title = "Open document"
            };

            if (ofd.ShowDialog(this) == DialogResult.OK && ofd.CheckFileExists) {
                this.Text = ofd.SafeFileName + " - OwOrdPad";
                this.Icon = Icon.ExtractAssociatedIcon(ofd.FileName);
                filePath = ofd.FileName;
                isDocumentModified = false;

                updateHistory(ofd.FileName);
                saveHistory();
                updateHistoryMenu();

                try {
                    rtb.LoadFile(ofd.FileName);
                }
                catch {
                    rtb.Text = File.ReadAllText(ofd.FileName);
                    rtb.Font = new Font("Consolas", rtb.Font.Size);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(filePath)) {
                switch (Path.GetExtension(filePath)) {
                    case ".rtf":
                    case ".owo":
                        SaveFile(filePath, true);
                        isDocumentModified = false;
                        break;
                    default:
                        SaveFile(filePath, false);
                        isDocumentModified = false;
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
                    isDocumentModified = false;
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
                this.Icon = Icon.ExtractAssociatedIcon(path);
                updateHistory(path);
                saveHistory();
                updateHistoryMenu();
                sendNotification("File saved as " + Path.GetFileName(path), "succes");
            }
            catch { sendNotification("Failed to save file", "error"); }
        }
        private void rtb_ModifiedChanged(object sender, EventArgs e) {
            isDocumentModified = true;
        }
        private void rtb_TextChanged(object sender, EventArgs e) {
            // word count
            lblCharAndWord.Text = rtb.Text.Length + " Characters, " +
                rtb.Text.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length + " Words";

            // estimated file size
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

            // auto save
            autoSaveCooldown = 0;
            tmrAutoSave.Start();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            if (isDocumentModified) {
                if (MessageBox.Show("Are you sure you want to create a new document?\nYou will lose any unsaved changes on your current file.", "New document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) {
                    return;
                }
            }
            isDocumentModified = false;
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

            string searchString = inputbox.GetInput("Search for a word, character or symbol:", "Find - OwOrdPad", [""], Resources.search, searchFor);

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

        private void goToToolStripMenuItem_Click(object sender, EventArgs e) {
            string inputLineNumber = inputbox.GetInput("Search for a line number (1 - " + rtb.Lines.Count() + "):", "Go to - OwOrdPad", [""], Resources.search, (rtb.GetLineFromCharIndex(rtb.SelectionStart) + 1).ToString());

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
            rtb.Copy(); // copy, ctrl+c
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
            rtb.SelectionFont = new Font("Calibri", 12f);
            rtb.SelectionBackColor = rtb.BackColor;
            rtb.SelectionColor = rtb.ForeColor;
            rtb.SelectionIndent = 0;
            rtb.SelectionCharOffset = 1;

            cbFonts.Text = "Calibri";
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

        int autoSaveCooldown = 0;
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
            }
            lblZoomFactor.Text = (rtb.ZoomFactor * 100).ToString("0") + "%";
        }
        private void tmrAutoSave_Tick(object sender, EventArgs e) {
            autoSaveCooldown++;
            if (autoSaveCooldown > 100) {
                tmrAutoSave.Stop();
                autoSaveCooldown = 0;
                rtb.SaveFile(defaultDirectory + "\\Autosave\\save.rtf");
            }
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
            rtb.SelectionColor = Color.FromArgb(37, 163, 147);
            hasPreset = true;
        }

        private void title2ToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 18f, FontStyle.Regular);
            rtb.SelectionColor = Color.FromArgb(37, 163, 147);
            hasPreset = true;
        }

        private void title3ToolStripMenuItem_Click(object sender, EventArgs e) {
            rtb.SelectionFont = new Font(cbFonts.Text, 14f, FontStyle.Regular);
            rtb.SelectionColor = Color.FromArgb(37, 163, 147);
            hasPreset = true;
        }
        #endregion
        #region misc
        // miscellaneous items related to UI interactions
        private void rtb_LinkClicked(object sender, LinkClickedEventArgs e) {
            if (MessageBox.Show("Open " + e.LinkText + " on a new window?", "Open link", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                Process.Start(new ProcessStartInfo(e.LinkText) { UseShellExecute = true });
            }
        }
        private void Form1_Resize(object sender, EventArgs e) {
            if (this.Size.Width < 327) {
                menuStrip.Hide();
                tsTool.Hide();
                tsFormat.Hide();
                statusStrip.Hide();
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
                TopMost = false;
                MinimizeBox = true;
                MaximizeBox = true;
            }
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
            insertObject("spreadsheet.owo");
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
            string imageUrl = inputbox.GetInput("Insert an image URL: ", "Open URL - OwOrdPad", [""], Resources.search, Clipboard.GetText());

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
            frmSpecialCharacter spec = new(this);
            spec.Show();
        }
        private void linkToolStripMenuItem_Click(object sender, EventArgs e) {
            string name = inputbox.GetInput("Insert the website name", "Insert link - OwOrdPad", [""], Resources.ok, Clipboard.GetText());
            if (name != "") {
                string link = inputbox.GetInput("Insert " + name + "'s URL", "Insert link - OwOrdPad", [""], Resources.ok, Clipboard.GetText());

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
                    lblNotification.ForeColor = Color.FromArgb(37, 163, 147);
                    lblNotification.Image = Resources.succes;
                    break;
                case "warning":
                    lblNotification.ForeColor = Color.FromArgb(205, 127, 0);
                    lblNotification.Image = Resources.warning;
                    break;
                case "error":
                    lblNotification.ForeColor = Color.FromArgb(215, 0, 31);
                    lblNotification.Image = Resources.error;
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
                rtb.LoadFile(defaultDirectory + "\\Autosave\\save.rtf");
                isDocumentModified = false;
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

            if (history.Count > 10) {
                history.RemoveAt(history.Count - 1); // remove an item, in case of it being on the 11° line
            }
        }
        private void updateHistoryMenu() {
            documentHistoryToolStripMenuItem.DropDownItems.Clear();

            try {
                foreach (var filePath in history) {
                    ToolStripMenuItem recentFileItem = new(Path.GetFileName(filePath)) {
                        Tag = filePath,
                        ToolTipText = filePath,
                        ShortcutKeyDisplayString = File.GetLastWriteTime(filePath).ToString("dd/MM/yyyy HH:mm"),
                        Image = Icon.ExtractAssociatedIcon(filePath).ToBitmap()
                    };
                    recentFileItem.Click += historyItemClick;
                    documentHistoryToolStripMenuItem.DropDownItems.Add(recentFileItem);
                }
            }
            catch { }
        }
        private void historyItemClick(object sender, EventArgs e) {
            if (isDocumentModified && MessageBox.Show("Are you sure you want to open a document?\nYou will lose any unsaved changes on your current file.",
                "Open document", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
                return;
            }

            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string path = menuItem.Tag.ToString();

            try {
                this.Text = Path.GetFileName(path) + " - OwOrdPad";
                this.Icon = Icon.ExtractAssociatedIcon(path);
                filePath = path;

                try {
                    rtb.LoadFile(path);
                }
                catch {
                    rtb.Text = File.ReadAllText(path);
                    rtb.Font = new Font("Consolas", rtb.Font.Size);
                }
                isDocumentModified = false;
            }
            catch {
                if (MessageBox.Show("This document doesn't exists\nDo you want to remove this item from the list?", "Document not found",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes) {
                    history.Remove(path);
                    saveHistory();
                    updateHistoryMenu();
                }
            }
        }
        #endregion

        private void indentationSizeToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                int[] i = [int.Parse(inputbox.GetInput("Insert an indentation size number (1 - 8):", "Tab spacing - OwOrdPad", [""], Resources.tabSpace, "4")) * 10];
                if (i[0] <= 80) {
                    rtb.SelectionTabs = i;
                }
                else {
                    sendNotification("Invalid size", "warning");
                }
            }
            catch {
                sendNotification("Invalid size", "warning");
            }
        }

        private void snippetToolStripMenuItem_Click(object sender, EventArgs e) {
            int select = rtb.SelectionStart;
            string snipPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\Snippets\\";

            string[] snippets = Directory.GetFiles(snipPath);

            string[] snippetNames = new string[snippets.Length];
            for (int i = 0; i < snippets.Length; i++) {
                snippetNames[i] = Path.GetFileName(snippets[i]);
            }

            string snipFile = inputbox.GetInput("Insert a snippet collection name:", "Snippets - OwOrdPad", snippetNames, Resources.folder);
            string fullPath;
            if (snipFile != null) {
                fullPath = Path.Combine(snipPath, snipFile);
            }
            else { return; }

            if (File.Exists(fullPath)) {
                string snipKey = inputbox.GetInput("Insert a snippet key:", "Snippets - OwOrdPad", [""], Resources.snippet);
                string[] lines = File.ReadAllLines(fullPath);

                foreach (string line in lines) {
                    string[] parts = line.Split('\t');

                    if (parts[0] == snipKey) {
                        string snippetContent = Regex.Unescape(parts[1]);

                        rtb.SelectedText = snippetContent;
                        rtb.Select(int.Parse(parts[2]) + select, int.Parse(parts[3]));
                        break;
                    }
                }
            }
            else {
                sendNotification("Collection not found", "error");
            }
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
            // WIP
            menuStrip.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            tsTool.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            tsFormat.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            statusStrip.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            fileToolStripMenuItem.DropDown.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            editToolStripMenuItem.DropDown.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            viewToolStripMenuItem.DropDown.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            insertToolStripMenuItem.DropDown.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
            formatToolStripMenuItem.DropDown.ShowItemToolTips = toolTipsToolStripMenuItem.Checked;
        }

        private void defaultFontToolStripMenuItem_Click(object sender, EventArgs e) {
            string fontName;
            FontDialog fd = new();
            fd.Font = new Font(Settings.GetSetting("defaultFont"), 12);
            if (fd.ShowDialog() == DialogResult.OK) {
                fontName = fd.Font.Name;
                Settings.SaveSetting("defaultFont", fontName);
            }
            defaultFontToolStripMenuItem.ShortcutKeyDisplayString = Settings.GetSetting("defaultFont");
        }

        private void increaseToolStripMenuItem_Click(object sender, EventArgs e) {
            if (rtb.SelectionFont.Size < 96) {
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, rtb.SelectionFont.Size + 1);
            } else {
                sendNotification("Font cannot be greater than 96", "warning");
            }
            cbFontSize.Text = rtb.SelectionFont.Size.ToString();
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e) {
            
            if (rtb.SelectionFont.Size > 8) {
                rtb.SelectionFont = new Font(rtb.SelectionFont.Name, rtb.SelectionFont.Size - 1);
            } else {
                sendNotification("Font cannot be smaller than 8", "warning");
            }
            cbFontSize.Text = rtb.SelectionFont.Size.ToString();
        }
    }
}
