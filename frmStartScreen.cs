using OwOrdPad.Properties;
using System.Diagnostics;

namespace OwOrdPad {
    public partial class frmStartScreen : Form {
        private string document = "";
        string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        public frmStartScreen() {
            InitializeComponent();
            LoadRecentDocuments();
            contextMenuStrip1.Renderer = new MenuRenderer();
        }
        private class MenuRenderer : ToolStripProfessionalRenderer {
            public MenuRenderer() : base(new CustomColors(new Themes())) { }
        }
        private void LoadRecentDocuments() {
            string[] documents = File.ReadAllLines(defaultDirectory + "\\Settings\\history");

            foreach (string document in documents) {
                if (File.Exists(document)) {
                    Bitmap icon = Icon.ExtractAssociatedIcon(document).ToBitmap();
                    if (!imageList1.Images.ContainsKey(document)) {
                        imageList1.Images.Add(document, icon);
                    }

                    ListViewItem item = new ListViewItem() {
                        Text = Path.GetFileName(document),
                        ImageKey = document,
                        Tag = document,
                    };
                    listView1.Items.Add(item);
                }
            }
        }
        private void listView1_ItemActivate(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0) {
                ListViewItem item = listView1.SelectedItems[0];

                if (File.Exists(item.Tag.ToString())) {
                    this.document = item.Tag.ToString();
                    DialogResult = DialogResult.OK;
                }
            }
        }
        private void btnOpen_Click(object sender, EventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog() {
                Filter = "Rich Text File|*.rtf|OwOdPad Markdown|*.owo|Plain Text File|*.txt|All Files|*.*",
                Title = "Open document"
            };
            if (ofd.ShowDialog() == DialogResult.OK) {
                if (ofd.CheckFileExists == true) {
                    this.document = ofd.FileName;
                    DialogResult = DialogResult.OK;
                }
            }
        }
        private void btnRestore_Click(object sender, EventArgs e) {
            document = this.defaultDirectory + "\\Autosave\\save.rtf";
            if (File.Exists(document)) {
                DialogResult = DialogResult.OK;
            }
        }
        public string getDocument() {
            return ShowDialog() == DialogResult.OK ? document : null;
        }
        private void closeDialog(object sender, EventArgs e) {
            this.Close();
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0) {
                ListViewItem item = listView1.SelectedItems[0];
                Clipboard.SetText(item.Tag.ToString());
            }
        }

        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listView1.SelectedItems.Count > 0) {
                ListViewItem item = listView1.SelectedItems[0];
                Process.Start("explorer.exe", "/select,\"" + item.Tag + "\"");
            }
        }
    }
}
