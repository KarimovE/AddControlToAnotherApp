using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddButtonToWindows
{
    public static class Helpers
    {
        public static IntPtr Find(string ModuleName, string MainWindowTitle)
        {
            IntPtr WndToFind = NativeMethods.FindWindow(ModuleName, MainWindowTitle);
            if (WndToFind.Equals(IntPtr.Zero))
            {
                if (!string.IsNullOrEmpty(MainWindowTitle))
                {
                    WndToFind = NativeMethods.FindWindowByCaption(WndToFind, MainWindowTitle);
                    if (WndToFind.Equals(IntPtr.Zero))
                        return new IntPtr(0);
                }
            }
            return WndToFind;
        }

        public static WinPosition GetWindowPosition(IntPtr wnd)
        {
            NativeMethods.TITLEBARINFO pti = new NativeMethods.TITLEBARINFO();
            pti.cbSize = (uint)Marshal.SizeOf(pti); 

            bool result = NativeMethods.GetTitleBarInfo(wnd, ref pti);

            WinPosition winpos;
            if (result)
                winpos = new WinPosition(pti);
            else
                winpos = new WinPosition();

            return winpos;
        }

        public enum GWLParameter
        {
            GWL_EXSTYLE = -20, 
            GWL_HINSTANCE = -6, 
            GWL_HWNDPARENT = -8,
            GWL_ID = -12, 
            GWL_STYLE = -16,
            GWL_USERDATA = -21, 
            GWL_WNDPROC = -4 
        }

        public static int SetWindowLong(IntPtr windowHandle, GWLParameter nIndex, int dwNewLong)
        {
            if (IntPtr.Size == 8) //Check if this window is 64bit
            {
                return (int)NativeMethods.SetWindowLongPtr64
                (windowHandle, nIndex, new IntPtr(dwNewLong));
            }
            return NativeMethods.SetWindowLong32(windowHandle, nIndex, dwNewLong);
        }
    }
}
