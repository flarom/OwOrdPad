using OwOrdPad.Properties;

namespace OwOrdPad {
    public partial class frmListManager : Form {
        private string[] _items;            // determines the items shown on the listItems control
        private string _title;              // determines the title of the window
        private string[] _addAutoComplete;  // determines the items used on the auto-complete of the input box
        private bool _allowAdd;             // determines if the user can use de 'add' button
        private bool _allowRemove;          // determines if the user can use the 'remove' button
        private bool _allowMove;            // determines if the user can use the 'up' and 'down' buttons
        private addType _addType;           // determines the way the 'add' button works
        public frmListManager() {
            InitializeComponent();
        }
        public enum addType {
            FromFile,   // add a string from a selected file path
            FromFolder, // add a string from a selected folder path
            FromString  // add a string from a input dialog
        }
        private void frmFavoriteFonts_Load(object sender, EventArgs e) {
            listItems.Items.Clear();

            Text = this._title;
            listItems.Items.AddRange(this._items);
            btnAdd.Visible = this._allowAdd;
            btnRemove.Visible = this._allowRemove;
            btnUp.Visible = this._allowMove;
            btnDown.Visible = this._allowMove;
        }
        public string[] getList(string title, string[] items = null, string[] addAutoComplete = null, addType addType = addType.FromString, bool allowAdd = true, bool allowRemove = true, bool allowMove = true) {
            _title = title;
            _items = items;
            _addAutoComplete = addAutoComplete;
            _allowAdd = allowAdd;
            _allowRemove = allowRemove;
            _allowMove = allowMove;
            _addType = addType;

            if (ShowDialog() == DialogResult.OK) {
                return listItems.Items.Cast<string>().ToArray();
            }
            else {
                return items;
            }
        }
        #region buttons
        private void btnAdd_Click(object sender, EventArgs e) {
            // new item
            string newItem = "";

            switch (this._addType) {
                case (addType.FromString):
                    // add a written string to the list
                    frmInputBox ib = new frmInputBox();
                    newItem = ib.GetInput("Insert a item name: ", "Add item - OwOrdPad", _addAutoComplete, Resources.add);
                    break;

                case (addType.FromFile):
                    // add a file path to the list
                    OpenFileDialog ofd = new();
                    if (ofd.ShowDialog(this) == DialogResult.OK) {
                        newItem = ofd.FileName;
                    }
                    break;

                case (addType.FromFolder):
                    // add a folder path to the list
                    FolderBrowserDialog fbd = new();
                    if (fbd.ShowDialog(this) == DialogResult.OK) {
                        newItem = fbd.SelectedPath;
                    }
                    break;
            }

            if (newItem != null) {
                listItems.Items.Add(newItem);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            // delete selection
            for (int i = listItems.SelectedItems.Count - 1; i >= 0; i--) {
                listItems.Items.Remove(listItems.SelectedItems[i]);
            }
        }
        private void btnOK_Click(object sender, EventArgs e) {
            // return and close
            DialogResult = DialogResult.OK;
            Close();
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
            for (int i = 0; i < listItems.Items.Count; i++) {
                listItems.SelectedIndices.Add(i);
            }
        }
        private void MoveListBoxItems(int direction) {
            // move selection up/down
            int selectedIndex = listItems.SelectedIndex;
            if (selectedIndex != -1) {
                int newIndex = selectedIndex + direction;
                if (newIndex >= 0 && newIndex < listItems.Items.Count) {
                    object selectedItem = listItems.SelectedItem;
                    listItems.Items.RemoveAt(selectedIndex);
                    listItems.Items.Insert(newIndex, selectedItem);
                    listItems.SelectedIndex = newIndex;
                }
            }
        }
        private void MoveListBoxItemToTop() {
            // move selection to the top
            int selectedIndex = listItems.SelectedIndex;
            if (selectedIndex > 0) {
                object selectedItem = listItems.SelectedItem;
                listItems.Items.RemoveAt(selectedIndex);
                listItems.Items.Insert(0, selectedItem);
                listItems.SelectedIndex = 0;
            }
        }

        private void MoveListBoxItemToBottom() {
            // move selection to the bottom
            int selectedIndex = listItems.SelectedIndex;
            if (selectedIndex < listItems.Items.Count - 1) {
                object selectedItem = listItems.SelectedItem;
                listItems.Items.RemoveAt(selectedIndex);
                listItems.Items.Add(selectedItem);
                listItems.SelectedIndex = listItems.Items.Count - 1;
            }
        }

        #endregion selection
        #region keyboard shortcuts
        private void frmFavoriteFonts_KeyDown(object sender, KeyEventArgs e) {
            // keyboard shortcuts
            switch (e.KeyCode) {
                case Keys.S when e.Modifiers == Keys.Control:   // Ctrl+S       Save and close
                case Keys.Enter:                                // Enter        Save and close
                    btnOK.PerformClick();
                    break;
                case Keys.Escape:                               // Esc          Close
                    this.Close();
                    break;
                case Keys.N when e.Modifiers == Keys.Control:   // Ctrl+N       New item
                    btnAdd.PerformClick();
                    break;
                case Keys.Delete:                               // Del          Delete item
                    btnRemove.PerformClick();
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
        #endregion keyboard shortcuts

        private void listItems_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index < 0)
                return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(49, 215, 193)), e.Bounds);

                e.Graphics.DrawLine(new Pen(Color.Black, 5), new PointF(0, 5), new PointF(15, 40));

                Rectangle borderRect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width - 1, e.Bounds.Height - 1);
                using (Pen borderPen = new Pen(Color.FromArgb(37, 163, 147), 1)) {
                    e.Graphics.DrawRectangle(borderPen, borderRect);
                }
            }
            else {
                e.Graphics.FillRectangle(new SolidBrush(e.BackColor), e.Bounds);
            }

            TextRenderer.DrawText(e.Graphics, listItems.Items[e.Index].ToString(),
                                    e.Font, new Point(e.Bounds.Left, e.Bounds.Top + 3),
                                    SystemColors.ControlText, TextFormatFlags.Left);

            e.DrawFocusRectangle();
        }
    }
}
