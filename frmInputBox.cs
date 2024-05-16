using OwOrdPad.Properties;

namespace OwOrdPad {
    partial class frmInputBox : Form {
        private string[] _items;
        private string _title;
        private string _message;
        private Image _okIcon;
        private string _defaultResponse;

        public frmInputBox() {
            InitializeComponent();
            Load += FrmInputBox_Load;
        }
        private void FrmInputBox_Load(object sender, EventArgs e) {
            Text = _title;
            labelMessage.Text = _message;
            btnOK.Image = _okIcon;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(_items);
            txtInput.AutoCompleteCustomSource = autoComplete;
            txtInput.AutoCompleteSource = AutoCompleteSource.CustomSource;
            if (_defaultResponse != null) {
                txtInput.Text = _defaultResponse;
                txtInput.SelectAll();
            }
            txtInput.Select();
        }

        public string GetInput(string message, string title, string[] items, Image okIcon = null, string defaultResponse = "") {
            _title = title;
            _message = message;
            _okIcon = okIcon;
            _items = items;
            _defaultResponse = defaultResponse;

            return ShowDialog() == DialogResult.OK ? txtInput.Text : null;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void frmInputBox_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Enter:
                    btnOK.PerformClick();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }
    }
}
