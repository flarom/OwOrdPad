using System.IO;

namespace OwOrdPad {
    internal static class Program {
        [STAThread]
        static void Main(string[] args) {
            ApplicationConfiguration.Initialize();

            Form1 owordpad = new();

            if (args.Length > 0) {
                try {
                    owordpad.rtb.LoadFile(args[0]);
                }
                catch {
                    owordpad.rtb.Text = File.ReadAllText(args[0]);
                }
                owordpad.filePath = args[0];
                owordpad.Text = Path.GetFileName(args[0]) + " - OwOrdPad";
                owordpad.Icon = Icon.ExtractAssociatedIcon(args[0]);
            }

            owordpad.register();
            Application.Run(owordpad);
        }
    }
}
