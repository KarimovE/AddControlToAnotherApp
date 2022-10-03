namespace AddButtonToWindows
{
    public class WindowPosition
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public WindowPosition()
        {
        }

        internal WindowPosition(NativeMethods.TITLEBARINFO pti)
        {
            this.Left = pti.rcTitleBar.left;
            this.Top = pti.rcTitleBar.top;
            this.Right = pti.rcTitleBar.right;
            this.Bottom = pti.rcTitleBar.bottom;
        }
    }
}
