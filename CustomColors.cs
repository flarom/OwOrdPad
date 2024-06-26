﻿namespace OwOrdPad {
    public class CustomColors : ProfessionalColorTable {
        public CustomColors(Themes theme) {
            highlight = theme.selectionHigh;
            outline = theme.selectionBorder;
            blank = theme.menuBack;
            border = theme.menuBorder;
        }
        readonly Color highlight = Color.FromArgb(49, 215, 193);
        readonly Color outline = Color.FromArgb(37, 163, 147);
        readonly Color blank = Color.FromArgb(255, 255, 255);
        readonly Color border = Color.FromArgb(189, 189, 189);

        // item borders
        public override Color SeparatorDark {
            get { return border; }
        }
        public override Color SeparatorLight {
            get { return blank; }
        }
        public override Color MenuBorder {
            get { return border; }
        }
        public override Color MenuItemBorder {
            get { return outline; }
        }
        public override Color ButtonSelectedBorder {
            get { return outline; }
        }
        public override Color ToolStripBorder {
            get { return blank; }
        }
        // item selected
        public override Color MenuItemPressedGradientBegin {
            get { return blank; }
        }
        public override Color MenuItemPressedGradientEnd {
            get { return blank; }
        }
        public override Color ButtonPressedGradientBegin {
            get { return outline; }
        }
        public override Color ButtonPressedGradientEnd {
            get { return outline; }
        }
        // item hover
        public override Color MenuItemSelectedGradientBegin {
            get { return highlight; }
        }
        public override Color MenuItemSelectedGradientEnd {
            get { return highlight; }
        }
        public override Color ButtonSelectedGradientBegin {
            get { return highlight; }
        }
        public override Color ButtonSelectedGradientMiddle {
            get { return highlight; }
        }
        public override Color ButtonSelectedGradientEnd {
            get { return highlight; }
        }
        // item checked
        public override Color ButtonCheckedGradientBegin {
            get { return highlight; }
        }
        public override Color ButtonCheckedGradientEnd {
            get { return highlight; }
        }
        public override Color CheckBackground {
            get { return highlight; }
        }
        public override Color CheckSelectedBackground {
            get { return outline; }
        }

        // image margin
        public override Color ImageMarginGradientBegin {
            get { return blank; }
        }
        public override Color ImageMarginGradientMiddle {
            get { return blank; }
        }
        public override Color ImageMarginGradientEnd {
            get { return blank; }
        }
        // overflow menu
        public override Color OverflowButtonGradientBegin {
            get { return blank; }
        }
        public override Color OverflowButtonGradientMiddle {
            get { return blank; }
        }
        public override Color OverflowButtonGradientEnd {
            get { return blank; }
        }
    }
}
