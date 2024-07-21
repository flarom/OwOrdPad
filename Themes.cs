using System.Drawing.Imaging;

namespace OwOrdPad {
    public class Themes {
        static string defaultDirectory = Path.GetDirectoryName(Application.ExecutablePath); // \OwOrdPad\bin\Debug\net8.0-windows\
        public Color
            // document
            documentBack = Color.White,
            documentFore = Color.Black,
            documentTitle = Color.Blue,
            // menu selection
            selectionBorder = Color.FromArgb(37, 163, 147),
            selectionHigh = Color.FromArgb(49, 215, 193),
            // menus
            menuBack = Color.White,
            menuFore = Color.Black,
            menuBorder = Color.FromArgb(243, 243, 243),
            icons = Color.Black,
            // UI
            headerBack = Color.FromArgb(243, 243, 243),
            headerFore = Color.Black,
            // Notifications
            sucess = Color.FromArgb(37, 163, 147),
            warning = Color.FromArgb(205, 127, 0),
            error = Color.FromArgb(215, 0, 31);
        public string themeName = "";

        public void setTheme(string themeName) {
            this.themeName = themeName;
            string theme = defaultDirectory + "\\Themes\\" + themeName;
            string[] lines = File.ReadAllLines(theme);

            Color parseColor(string colorString) {
                return ColorTranslator.FromHtml(colorString.Trim());
            }

            var colorMappings = new Dictionary<string, Action<Color>> {
                { "document background", color => documentBack = color },
                { "document foreground", color => documentFore = color },
                { "document title", color => documentTitle = color },
                { "selection border", color => selectionBorder = color },
                { "selection highlight", color => selectionHigh = color },
                { "icons", color => icons = color },
                { "header background", color => headerBack = color },
                { "header foreground", color => headerFore = color },
                { "menu background", color => menuBack = color },
                { "menu foreground", color => menuFore = color },
                { "menu border", color => menuBorder = color },
                { "succes", color => sucess = color },
                { "warning", color => warning = color },
                { "error", color => error = color }
            };

            foreach (string line in lines) {
                var splitLine = line.Split(':');
                if (splitLine.Length >= 2) {
                    string key = splitLine[0].Trim();
                    string value = splitLine[1].Trim();
                    if (colorMappings.ContainsKey(key)) {
                        Color color = parseColor(value);
                        colorMappings[key](color);
                    }
                }
            }
        }

        public Bitmap paintBitmap(Image image, Color color) {
            Bitmap bmp = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(bmp)) {
                g.Clear(Color.Transparent);

                float[][] colorMatrixElements = {
                    [0, 0, 0, 0, 0],
                    [0, 0, 0, 0, 0],
                    [0, 0, 0, 0, 0],
                    [0, 0, 0, 1, 0],
                    [color.R / 255f, color.G / 255f, color.B / 255f, 0, 1]
                };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
            }
            return bmp;
        }
    }
}
