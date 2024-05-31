using OwOrdPad.Properties;
using System.Threading;

namespace OwOrdPad {
    partial class frmInputBox : Form {
        private string[] _items = [];
        private string _title = "";
        private string _message = "";
        private Image _okIcon = Resources.ok;
        private string _defaultResponse = "";
        ContextMenuStrip contextMenu = new ContextMenuStrip() {
            Renderer = new MenuRenderer(),
            ShowImageMargin = false,
            MaximumSize = new Size(0, 400),
        };

        public frmInputBox() {
            InitializeComponent();
            Load += FrmInputBox_Load;
        }

        private async void FrmInputBox_Load(object sender, EventArgs e) {
            Text = _title;
            labelMessage.Text = _message;
            btnOK.Image = _okIcon;

            var autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(_items);
            txtInput.AutoCompleteCustomSource = autoComplete;
            txtInput.AutoCompleteSource = AutoCompleteSource.CustomSource;

            if (_items.Length > 0) {
                btnOptions.Show();
                contextMenu.Items.Clear();
                await Task.Run(() => PopulateContextMenu());
            } else {
                btnOptions.Hide();
            }

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

            return ShowDialog() == DialogResult.OK ? txtInput.Text : string.Empty;
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
                    e.SuppressKeyPress = true;
                    Close();
                    break;
                case Keys.F1:
                    e.SuppressKeyPress = true;
                    btnOptions.PerformClick();
                    break;
            }
        }
        private void btnOptions_Click(object sender, EventArgs e) {
            if (contextMenu.Visible) {
                contextMenu.Hide();
                return;
            }
            contextMenu.Show(new Point(this.Location.X + 313, this.Location.Y + 145));
        }
        private async void PopulateContextMenu() {
            try {
                foreach (string item in _items) {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(item);
                    tsmi.Click += Tsmi_Click;
                    this.Invoke((MethodInvoker)delegate {
                        contextMenu.Items.Add(tsmi);
                    });
                    await Task.Delay(1);
                }
            }
            catch { }
        }
        private void Tsmi_Click(object? sender, EventArgs e) {
            if(sender is ToolStripMenuItem tsmi) {
                string selection = tsmi.Text;
                if (!string.IsNullOrEmpty(selection)) {
                    txtInput.Text = selection;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
        private class MenuRenderer : ToolStripProfessionalRenderer {
            public MenuRenderer() : base(new CustomColors()) { }
        }
    }
}
