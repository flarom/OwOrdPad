using OwOrdPad.Properties;
using System.Drawing.Text;

namespace OwOrdPad {
    public partial class frmFavoriteFonts : Form {
        string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        public frmFavoriteFonts() {
            InitializeComponent();
            LoadFavoriteFonts();
        }
        #region save/load
        private void LoadFavoriteFonts() {
            // show all items from favoriteFonts on listFonts
            string[] fonts = File.ReadAllLines(defaultDirectory + "\\settings\\favoriteFonts");
            foreach (string fontName in fonts) {
                listFonts.Items.Add(fontName);
            }
        }
        private void SaveFavoriteFonts() {
            // save all items in listFonts on favoriteFonts
            string[] fonts = new string[listFonts.Items.Count];

            for (int i = 0; i < listFonts.Items.Count; i++) {
                fonts[i] = listFonts.Items[i].ToString();
            }

            File.WriteAllLines(defaultDirectory + "\\settings\\favoriteFonts", fonts);
        }
        #endregion save/load
        #region buttons
            frmInputBox input = new frmInputBox();
        private void btnAdd_Click(object sender, EventArgs e) {
            // new item
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            FontFamily[] fontFamilies = installedFonts.Families;
            string[] fontNames = new string[fontFamilies.Length];

            for (int i = 0; i < fontFamilies.Length; i++) {
                fontNames[i] = fontFamilies[i].Name;
            }

            string font = input.GetInput("Insert a font name: ", "Add font - OwOrdPad", fontNames, Resources.add);
            if (font != null) {
                listFonts.Items.Add(font);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            // delete selection
            for (int i = listFonts.SelectedItems.Count - 1; i >= 0; i--) {
                listFonts.Items.Remove(listFonts.SelectedItems[i]);
            }
        }
        private void btnOK_Click(object sender, EventArgs e) {
            // save and close
            SaveFavoriteFonts();
            this.Close();
        }
        private void btnUp_Click(object sender, EventArgs e) {
            // move selection up
            MoveListBoxItems(-1);
        }

        private void btnDown_Click(object sender, EventArgs e) {
            // move selection down
            MoveListBoxItems(1);
        }

        private void btnSlctAll_Click(object sender, EventArgs e) {
            // select all items from the list
            selectAll();
        }
        #endregion buttons
        #region selection
        private void selectAll() {
            // select all items from the list
            for (int i = 0; i < listFonts.Items.Count; i++) {
                listFonts.SelectedIndices.Add(i);
            }
        }
        private void MoveListBoxItems(int direction) {
            // move selection up/down
            int selectedIndex = listFonts.SelectedIndex;
            if (selectedIndex != -1) {
                int newIndex = selectedIndex + direction;
                if (newIndex >= 0 && newIndex < listFonts.Items.Count) {
                    object selectedItem = listFonts.SelectedItem;
                    listFonts.Items.RemoveAt(selectedIndex);
                    listFonts.Items.Insert(newIndex, selectedItem);
                    listFonts.SelectedIndex = newIndex;
                }
            }
        }
        private void MoveListBoxItemToTop() {
            // move selection to the top
            int selectedIndex = listFonts.SelectedIndex;
            if (selectedIndex > 0) {
                object selectedItem = listFonts.SelectedItem;
                listFonts.Items.RemoveAt(selectedIndex);
                listFonts.Items.Insert(0, selectedItem);
                listFonts.SelectedIndex = 0;
            }
        }

        private void MoveListBoxItemToBottom() {
            // move selection to the bottom
            int selectedIndex = listFonts.SelectedIndex;
            if (selectedIndex < listFonts.Items.Count - 1) {
                object selectedItem = listFonts.SelectedItem;
                listFonts.Items.RemoveAt(selectedIndex);
                listFonts.Items.Add(selectedItem);
                listFonts.SelectedIndex = listFonts.Items.Count - 1;
            }
        }

        #endregion selection
        private void frmFavoriteFonts_KeyDown(object sender, KeyEventArgs e) {
            // keyboard shortcuts
            switch (e.KeyCode) {
                case Keys.S when e.Modifiers == Keys.Control:   // Ctrl+S       Save and close
                    btnOK.PerformClick();
                    break;
                case Keys.Escape:                               // Esc          Close
                    this.Close();
                    break;
                case Keys.N when e.Modifiers == Keys.Control:   // Ctrl+N       New item
                    btnAdd.PerformClick();
                    break;
                case Keys.Delete:                               // Del          Delete item
                    btnDelete.PerformClick();
                    break;
                case Keys.A when e.Modifiers == Keys.Control:   // Ctrl+A       Select all items
                    e.SuppressKeyPress = true;
                    selectAll();
                    break;
                case Keys.Up when e.Modifiers == Keys.Alt:      // Alt+Up       Move selection up
                    MoveListBoxItems(-1);
                    break;
                case Keys.Down when e.Modifiers == Keys.Alt:    // Alt+Down     Move selection down
                    MoveListBoxItems(1);
                    break;
                case Keys.Up when e.Modifiers == Keys.Control:  // Ctrl+Up      Move selection to top
                    e.SuppressKeyPress = true;
                    MoveListBoxItemToTop();
                    break;
                case Keys.Down when e.Modifiers == Keys.Control:// Ctrl+Down    Move selection to bottom
                    e.SuppressKeyPress = true;
                    MoveListBoxItemToBottom();
                    break;
            }
        }
    }
}
