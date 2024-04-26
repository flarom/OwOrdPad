using System.Windows.Forms;

namespace OwOrdPad {
    public partial class frmFavoriteFonts : Form {
        string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath);
        public frmFavoriteFonts() {
            InitializeComponent();
            LoadFavoriteFonts();
        }
        private void LoadFavoriteFonts() {
            string[] fonts = File.ReadAllLines(defaultDirectory + "\\settings\\favoriteFonts");
            foreach (string fontName in fonts) {
                listFonts.Items.Add(fontName);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            string font = Microsoft.VisualBasic.Interaction.InputBox("Insert a font name: ", "Add font - OwOrdPad");
            if (!font.Equals("")) {
                listFonts.Items.Add(font);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {
            for (int i = listFonts.SelectedItems.Count - 1; i >= 0; i--) {
                listFonts.Items.Remove(listFonts.SelectedItems[i]);
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            string[] fonts = new string[listFonts.Items.Count];

            for (int i = 0; i < listFonts.Items.Count; i++) {
                fonts[i] = listFonts.Items[i].ToString();
            }

            File.WriteAllLines(defaultDirectory + "\\settings\\favoriteFonts", fonts);
            this.Close();
        }
        private void MoveListBoxItems(int direction) {
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
            int selectedIndex = listFonts.SelectedIndex;
            if (selectedIndex > 0) {
                object selectedItem = listFonts.SelectedItem;
                listFonts.Items.RemoveAt(selectedIndex);
                listFonts.Items.Insert(0, selectedItem);
                listFonts.SelectedIndex = 0;
            }
        }

        private void MoveListBoxItemToBottom() {
            int selectedIndex = listFonts.SelectedIndex;
            if (selectedIndex < listFonts.Items.Count - 1) {
                object selectedItem = listFonts.SelectedItem;
                listFonts.Items.RemoveAt(selectedIndex);
                listFonts.Items.Add(selectedItem);
                listFonts.SelectedIndex = listFonts.Items.Count - 1;
            }
        }
        private void selectAll() {
            for (int i = 0; i < listFonts.Items.Count; i++) {
                listFonts.SelectedIndices.Add(i);
            }
        }
        private void frmFavoriteFonts_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.S when e.Modifiers == Keys.Control:
                    btnOK.PerformClick();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.N when e.Modifiers == Keys.Control:
                    btnAdd.PerformClick();
                    break;
                case Keys.Delete:
                    btnDelete.PerformClick();
                    break;
                case Keys.A when e.Modifiers == Keys.Control:
                    e.SuppressKeyPress = true;
                    selectAll();
                    break;
                case Keys.Up when e.Modifiers == Keys.Alt:
                    MoveListBoxItems(-1);
                    break;
                case Keys.Down when e.Modifiers == Keys.Alt:
                    MoveListBoxItems(1);
                    break;
                case Keys.Up when e.Modifiers == Keys.Control:
                    e.SuppressKeyPress = true;
                    MoveListBoxItemToTop();
                    break;
                case Keys.Down when e.Modifiers == Keys.Control:
                    e.SuppressKeyPress = true;
                    MoveListBoxItemToBottom();
                    break;
            }
        }

        private void btnUp_Click(object sender, EventArgs e) {
            MoveListBoxItems(-1);
        }

        private void btnDown_Click(object sender, EventArgs e) {
            MoveListBoxItems(1);
        }

        private void btnSlctAll_Click(object sender, EventArgs e) {
            selectAll();
        }
    }
}
