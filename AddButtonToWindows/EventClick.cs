using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace AddButtonToWindows
{
    public class EventClick
    {
        IntPtr g_hook;
        private void btn_set_event_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> < accessibleevents > events =
                            InitializeWinEventToHandlerMap();

            NativeMethods.WinEventProc eventHandler =
                new NativeMethods.WinEventProc(events[AccessibleEvents.LocationChange].Invoke);

            GCHandle gch = GCHandle.Alloc(eventHandler);

            g_hook = NativeMethods.SetWinEventHook(AccessibleEvents.LocationChange,
                AccessibleEvents.LocationChange, IntPtr.Zero, eventHandler
                , 0, 0, NativeMethods.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT);

            eventHandler = new NativeMethods.WinEventProc
                    (events[AccessibleEvents.Destroy].Invoke);

            gch = GCHandle.Alloc(eventHandler);

            g_hook = NativeMethods.SetWinEventHook(AccessibleEvents.Destroy,
                AccessibleEvents.Destroy, IntPtr.Zero, eventHandler
                , 0, 0, NativeMethods.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT);

        }
    }
}
