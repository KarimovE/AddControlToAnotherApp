using System;
using System.Runtime.InteropServices;

namespace AddButtonToWindows
{
    public class Helpers
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

        public static WindowPosition GetWindowPosition(IntPtr wnd)
        {
            NativeMethods.TITLEBARINFO pti = new NativeMethods.TITLEBARINFO();

            pti.cbSize = (uint)Marshal.SizeOf(pti);

            bool result = NativeMethods.GetTitleBarInfo(wnd, ref pti);

            WindowPosition winpos;
            if (result)
                winpos = new WindowPosition(pti);
            else
                winpos = new WindowPosition();

            return winpos;
        }

        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }
            return new IntPtr(NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
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
            if (IntPtr.Size == 8)
            {
                return (int)NativeMethods.SetWindowLongPtr64(windowHandle, nIndex, new IntPtr(dwNewLong));
            }
            return NativeMethods.SetWindowLong32(windowHandle, nIndex, dwNewLong);
        }
    }
}
