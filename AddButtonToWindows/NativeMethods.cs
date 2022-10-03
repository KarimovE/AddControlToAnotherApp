using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AddButtonToWindows
{
    static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        internal static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool GetTitleBarInfo(IntPtr hwnd, ref TITLEBARINFO pti);

        [DllImport("coredll.dll", SetLastError = true)]
        internal static extern Int32 GetLastError();

        [StructLayout(LayoutKind.Sequential)]
        internal struct TITLEBARINFO
        {
            public const int CCHILDREN_TITLEBAR = 5;
            public uint cbSize; 

            public RECT rcTitleBar; 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]

            public AccessibleStates[] rgstate;

        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        internal static extern int SetWindowLong32(IntPtr windowHandle, AddButtonToWindows.Helpers.GWLParameter nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr windowHandle, AddButtonToWindows.Helpers.GWLParameter nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);


        [DllImport("user32.dll")]
        internal static extern IntPtr SetWinEventHook(
            AccessibleEvents eventMin, 
            AccessibleEvents eventMax, 
            IntPtr eventHookAssemblyHandle, 
            WinEventProc eventHookHandle, 
            uint processId, 
            uint threadId,
            SetWinEventHookParameter parameterFlags 
            );

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        internal static extern bool UnhookWinEvent(IntPtr eventHookHandle);

        internal delegate void WinEventProc(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

        [Flags]
        internal enum SetWinEventHookParameter
        {
            WINEVENT_INCONTEXT = 4,
            WINEVENT_OUTOFCONTEXT = 0,
            WINEVENT_SKIPOWNPROCESS = 2,
            WINEVENT_SKIPOWNTHREAD = 1

        }
    }
}
