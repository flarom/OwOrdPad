namespace OwOrdPad {
    public class Settings {
        static string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        public static void SaveSetting(string name, string value) {
            string filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings", name);
            try {
                File.WriteAllText(filePath, value);
            }
            catch (Exception ex) {
                MessageBox.Show("Failed to save setting: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public static string GetSetting(string name) {
            try {
                return File.ReadAllText(defaultDirectory + "\\settings\\" + name);
            }
            catch {
                return "false";
            }
        }
    }
}
