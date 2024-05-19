using System;
using System.IO;
using System.Windows.Forms;

namespace OwOrdPad {
    internal static class Program {
        [STAThread]
        static void Main(string[] args) {
            ApplicationConfiguration.Initialize();
            Form1 owordpad = new();

            if (args.Length > 0) {
                string path = args[0];
                if (Directory.Exists(path)) {
                    owordpad.LoadDirectory(path);
                }
                else if (File.Exists(path)) {
                    try {
                        owordpad.rtb.LoadFile(path);
                    }
                    catch {
                        owordpad.rtb.Text = File.ReadAllText(path);
                    }
                    owordpad.filePath = path;
                    owordpad.Text = Path.GetFileName(path) + " - OwOrdPad";
                    owordpad.Icon = Icon.ExtractAssociatedIcon(path);
                }
            }

            owordpad.register();
            Application.Run(owordpad);
        }
    }
}
