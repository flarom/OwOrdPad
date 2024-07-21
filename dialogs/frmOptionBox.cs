namespace OwOrdPad {
    public partial class frmOptionBox : Form {
        private string[] _options;      // determines the options displayed to the user
        private string _title;          // determines the title of the window
        private string _message;        // determines the message shown
        private bool _multiOption;      // determines if multiple options can be selected by the user
        private string _selected;       // determines the selected option
        private string[] _mselected;    // determines a list of selected options

        public Themes Themes {
            get; set;
        }  // theme
        public frmOptionBox() {
            InitializeComponent();
        }

        private void frmOptionBox_Load(object sender, EventArgs e) {
            flpOptions.Controls.Clear();

            Text = this._title;
            lblMessage.Text = this._message;

            if (_multiOption) {
                foreach (string option in _options) {
                    CheckBox cbOption = new CheckBox();

                    cbOption.Text = option;
                    cbOption.Size = new Size(295, 35);
                    cbOption.TextAlign = ContentAlignment.MiddleLeft;

                    cbOption.FlatStyle = FlatStyle.Flat;

                    cbOption.Click += CbOption_Click;

                    flpOptions.Controls.Add(cbOption);
                }
            }
            else 
            {
                foreach (string option in _options) {
                    Button btnOption = new Button();

                    btnOption.Text = option;
                    btnOption.Size = new Size(295, 35);
                    btnOption.TextAlign = ContentAlignment.MiddleLeft;

                    btnOption.FlatStyle = FlatStyle.Flat;
                    btnOption.FlatAppearance.BorderSize = 0;
                    btnOption.FlatAppearance.BorderColor = Themes.selectionBorder;
                    btnOption.FlatAppearance.MouseOverBackColor = Themes.selectionHigh;

                    btnOption.Click += BtnOption_Click;
                    btnOption.GotFocus += BtnOption_MouseEnter;
                    btnOption.MouseEnter += BtnOption_MouseEnter;
                    btnOption.LostFocus += BtnOption_MouseLeave;
                    btnOption.MouseLeave += BtnOption_MouseLeave;

                    flpOptions.Controls.Add(btnOption);
                }
            }

            BackColor = Themes.documentBack;
            ForeColor = Themes.documentFore;
            flpOptions.BackColor = Themes.documentBack;
            flpOptions.ForeColor = Themes.documentFore;
            panel1.BackColor = Themes.headerBack;
            btnOK.Image = Themes.paintBitmap(btnOK.Image, Themes.icons);
            btnOK.FlatAppearance.MouseOverBackColor = Themes.selectionHigh;
        }

        private void CbOption_Click(object? sender, EventArgs e) {
            List<string> selectedList = [];

            foreach (CheckBox cb in flpOptions.Controls) {
                if (cb.Checked) {
                    selectedList.Add(cb.Text);
                }
            }

            _mselected = selectedList.ToArray();
        }

        private void BtnOption_MouseLeave(object? sender, EventArgs e) {
            if (sender is Button btnOption) {
                btnOption.FlatAppearance.BorderSize = 0;
                btnOption.BackColor = Themes.documentBack;
            }
        }

        private void BtnOption_MouseEnter(object? sender, EventArgs e) {
            if (sender is Button btnOption) {
                btnOption.FlatAppearance.BorderSize = 1;
                btnOption.BackColor = Themes.selectionHigh;
            }
        }

        private void BtnOption_Click(object? sender, EventArgs e) {
            if (sender is Button btnOption) {
                _selected = btnOption.Text;
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        public string getOption(string title, string message, string[] options) {
            _title = title;
            _options = options;
            _message = message;
            _multiOption = false;

            if (ShowDialog() == DialogResult.OK) {
                return _selected;
            }
            else {
                return "";
            }
        }
        public string[] getOptions(string title, string message, string[] options) {
            _title = title;
            _options = options;
            _message = message;
            _multiOption = true;

            if (ShowDialog() == DialogResult.OK) {
                return _mselected;
            }
            else {
                return [];
            }
        }

        private void btnOK_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
