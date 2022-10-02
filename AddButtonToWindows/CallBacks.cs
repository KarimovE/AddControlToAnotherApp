using FluentAssertions.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddButtonToWindows
{
    public class CallBacks
    {
        private Dictionary<AccessibleEvents, NativeMethods.WinEventProc>
       InitializeWinEventToHandlerMap()
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> dictionary =
                new Dictionary<AccessibleEvents, NativeMethods.WinEventProc>();
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.accessibleevents.aspx
            new NativeMethods.WinEventProc(this.ValueChangedCallback));
            dictionary.Add(AccessibleEvents.LocationChange,
                new NativeMethods.WinEventProc(this.LocationChangedCallback));
            dictionary.Add(AccessibleEvents.Destroy,
                new NativeMethods.WinEventProc(this.DestroyCallback));

            return dictionary;
        }

        private void DestroyCallback(IntPtr winEventHookHandle,
            AccessibleEvents accEvent, IntPtr windowHandle, int objectId,
            int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            //Make sure AccessibleEvents equals to LocationChange and the 
            //current window is the Target Window.
            if (accEvent == AccessibleEvents.Destroy && windowHandle.ToInt32() ==
                TargetWnd.ToInt32())
            {
                //Queues a method for execution. The method executes when a thread pool 
                //thread becomes available.
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DestroyHelper));
            }
        }

        //private void DestroyHelper(object state)
        //{
        //    Execute ex = delegate ()
        //    {
        //        NativeMethods.UnhookWinEvent(g_hook);
        //        OnTopControl.Close();
        //    };
        //    this.Dispatcher.Invoke(ex, null);
        //}

        private void LocationChangedCallback(IntPtr winEventHookHandle,
            AccessibleEvents accEvent, IntPtr windowHandle, int objectId,
            int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            if (accEvent == AccessibleEvents.LocationChange && windowHandle.ToInt32() ==
                TargetWnd.ToInt32())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LocationChangedHelper));
            }
        }

        //private void LocationChangedHelper(object state)
        //{
        //    Execute ex = delegate ()
        //    {
        //        if (OnTopControl != null)
        //            OnTopControl.Close();
        //        GetWindowPosition(false);
        //        SetControl(false);
        //    };
        //    this.Dispatcher.Invoke(ex, null);
        //}


    }
}
