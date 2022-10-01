using Microsoft.TeamFoundation.Common.Internal;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class ExternalMethods
{


        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string ClassN, string WindN);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


}

