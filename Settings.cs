namespace OwOrdPad {
    public class Settings {
        public static void SaveSetting(string name, string value) {
            string filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) + "\\settings", name);
            try {
                File.WriteAllText(filePath, value);
            }
            catch (Exception ex) {
                MessageBox.Show("Failed to save setting: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
