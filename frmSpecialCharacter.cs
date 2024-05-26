namespace OwOrdPad {
    public partial class frmSpecialCharacter : Form {
        private Dictionary<string, List<char>> unicodeCategories;
        public frmSpecialCharacter() {
            InitializeComponent();
            InitializeUnicodeCategories();

            charListView.ItemActivate += charListView_ItemActivate;
            categoryListBox.Items.AddRange(unicodeCategories.Keys.ToArray());
            contextMenuStrip1.Renderer = new MenuRenderer();
        }
        private class MenuRenderer : ToolStripProfessionalRenderer {
            public MenuRenderer() : base(new CustomColors()) { }
        }
        public string getChars() {
            return ShowDialog() == DialogResult.OK ? txtOutput.Text : null;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }
        public void InitializeUnicodeCategories() {
            // dictionary of all unicode characters, categorized
            unicodeCategories = new Dictionary<string, List<char>>();

            //AddUnicodeCategory("Basic Latin", Enumerable.Range(0x00, 0x7F + 1).Select(c => (char)c)); // disabled by default, unnecessary
            AddUnicodeCategory("Latin Supplement", Enumerable.Range(0x80, 0xFF - 0x80 + 1).Select(c => (char)c));
            AddUnicodeCategory("Latin Extended-A", Enumerable.Range(0x0100, 0x017F - 0x0100 + 1).Select(c => (char)c));
            AddUnicodeCategory("Latin Extended-B", Enumerable.Range(0x0180, 0x024F - 0x0180 + 1).Select(c => (char)c));
            AddUnicodeCategory("IPA Extensions", Enumerable.Range(0x0250, 0x02AF - 0x0250 + 1).Select(c => (char)c));
            AddUnicodeCategory("Spacing Modifier Letters", Enumerable.Range(0x02B0, 0x02FF - 0x02B0 + 1).Select(c => (char)c));
            AddUnicodeCategory("Greek", Enumerable.Range(0x0370, 0x03FF - 0x0370 + 1).Select(c => (char)c));
            AddUnicodeCategory("Cyrillic", Enumerable.Range(0x0400, 0x04FF - 0x0400 + 1).Select(c => (char)c));
            AddUnicodeCategory("Latin Extended Additional", Enumerable.Range(0x1E00, 0x1EFF - 0x1E00 + 1).Select(c => (char)c));
            AddUnicodeCategory("Greek Extended", Enumerable.Range(0x1F00, 0x1FFF - 0x1F00 + 1).Select(c => (char)c));
            AddUnicodeCategory("Punctuation", Enumerable.Range(0x2000, 0x206F - 0x2000 + 1).Select(c => (char)c));
            AddUnicodeCategory("Superscripts and Subscripts", Enumerable.Range(0x2070, 0x209F - 0x2070 + 1).Select(c => (char)c));
            AddUnicodeCategory("Currency Symbols", Enumerable.Range(0x20A0, 0x20CF - 0x20A0 + 1).Select(c => (char)c));
            AddUnicodeCategory("Letterlike Symbols", Enumerable.Range(0x2100, 0x214F - 0x2100 + 1).Select(c => (char)c));
            AddUnicodeCategory("Number Forms", Enumerable.Range(0x2150, 0x218F - 0x2150 + 1).Select(c => (char)c));
            AddUnicodeCategory("Arrows", Enumerable.Range(0x2190, 0x21FF - 0x2190 + 1).Select(c => (char)c));
            AddUnicodeCategory("Mathematical Operators", Enumerable.Range(0x2200, 0x22FF - 0x2200 + 1).Select(c => (char)c));
            AddUnicodeCategory("Miscellaneous Technical", Enumerable.Range(0x2300, 0x23FF - 0x2300 + 1).Select(c => (char)c));
            AddUnicodeCategory("Box Drawing", Enumerable.Range(0x2500, 0x257F - 0x2500 + 1).Select(c => (char)c));
            AddUnicodeCategory("Block Elements", Enumerable.Range(0x2580, 0x259F - 0x2580 + 1).Select(c => (char)c));
            AddUnicodeCategory("Geometric Shapes", Enumerable.Range(0x25A0, 0x25FF - 0x25A0 + 1).Select(c => (char)c));
            AddUnicodeCategory("Miscellaneous Symbols", Enumerable.Range(0x2600, 0x26FF - 0x2600 + 1).Select(c => (char)c));
            AddUnicodeCategory("Presentation Forms", Enumerable.Range(0xFB00, 0xFB4F - 0xFB00 + 1).Select(c => (char)c));
            //AddUnicodeCategory("Emojis", Enumerable.Range(0x1F300, 0x1F9FF - 0x1F300 + 1).Select(c => (char)c)); // disabled by default, there's no emojis TwT
            //AddUnicodeCategory("Private use Area", Enumerable.Range(0xE000, 0xF8FF - 0xE000 + 1).Select(c => (char)c)); // disabled by default, laggy
            //AddUnicodeCategory("Specials", Enumerable.Range(0xFE00, 0xFE0F - 0xFE00 + 1).Select(c => (char)c)); // disabled by default, empty 
        }
        private void AddUnicodeCategory(string categoryName, IEnumerable<char> characters) {
            unicodeCategories.Add(categoryName, characters.ToList());
        }

        private void categoryListBox_SelectedIndexChanged(object sender, EventArgs e) {
            charListView.Items.Clear();

            string selectedCategory = categoryListBox.SelectedItem?.ToString();

            if (selectedCategory != null && unicodeCategories.ContainsKey(selectedCategory)) {
                foreach (char character in unicodeCategories[selectedCategory]) {
                    charListView.Invoke((Action)(() => charListView.Items.Add(character.ToString())));
                }
            }
        }

        private void charListView_ItemActivate(object sender, EventArgs e) {
            string selectedCharacter = charListView.SelectedItems[0].Text;
            txtOutput.SelectedText = selectedCharacter;
        }

        private void frmSpecialCharacter_Load(object sender, EventArgs e) {
            categoryListBox.SelectedIndex = 0;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {
            Clipboard.SetText(charListView.SelectedItems[0].Text);
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            if (charListView.SelectedItems.Count > 0) {
                copyToolStripMenuItem.Enabled = true;
            }
            else {
                copyToolStripMenuItem.Enabled = false;
            }
        }

        private void frmSpecialCharacter_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }

        private void categoryListBox_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0)
                return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(49, 215, 193)), e.Bounds);

                Rectangle borderRect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
                using (Pen borderPen = new Pen(Color.FromArgb(37, 163, 147), 1)) {
                    e.Graphics.DrawRectangle(borderPen, borderRect);
                }
            }
            else {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            }

            TextRenderer.DrawText(e.Graphics, categoryListBox.Items[e.Index].ToString(),
                                    e.Font, new Point(e.Bounds.Left, e.Bounds.Top + 3),
                                    SystemColors.ControlText, TextFormatFlags.Left);

            e.DrawFocusRectangle();
        }

        private void btnUndo_Click(object sender, EventArgs e) {
            if (txtOutput.Text.Length == 0) return;
            txtOutput.Text = txtOutput.Text.Remove(txtOutput.Text.Length - 1);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            txtOutput.Text = "";
        }
    }
}
