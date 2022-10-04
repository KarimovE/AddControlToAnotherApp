using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace AddButtonToWindows
{
    public partial class MainWindow : Window
    {
        private delegate void Execute();
        private IntPtr TargetWnd = new IntPtr(0);
        int left, top, right, bottom;
        private BackgroundWorker bg_loadprocess;

        public MainWindow()
        {
            InitializeComponent();
            bg_loadprocess = new BackgroundWorker();
            bg_loadprocess.DoWork += new DoWorkEventHandler(bg_loadprocess_DoWork);
            bg_loadprocess.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_loadprocess_RunWorkerCompleted);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            bg_loadprocess.RunWorkerAsync();
        }

        private void btn_find_Click(object sender, RoutedEventArgs e)
        {
            ProcessItem pro = ProcessList.SelectedItem as ProcessItem;

            string ModuleName = pro.ProcessName;
            string MainWindowTitle = pro.Title; ;

            TargetWnd = Helpers.Find(ModuleName, MainWindowTitle);

            if (!TargetWnd.Equals(IntPtr.Zero))
                Log(ModuleName + " Window: " + TargetWnd.ToString());
            else
                Log(ModuleName + " Not found");

        }

        void Log(string txt)
        {
            Execute ex = delegate ()
            {
                LogList.Items.Add(string.Format("[{0}] {1}", DateTime.Now, txt));
            };
            this.Dispatcher.Invoke(ex, null);
        }

        #region Process Actions
        void bg_loadprocess_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Process[] pro_list = e.Result as Process[];
            foreach (Process pro in pro_list)
            {
                try
                {
                    ProcessList.Items.Add(new ProcessItem(pro.ProcessName, pro.MainWindowTitle));
                }
                catch (Exception)
                {
                }
            }
        }

        void bg_loadprocess_DoWork(object sender, DoWorkEventArgs e)
        {
            Process[] pro_list = Process.GetProcesses();
            e.Result = pro_list;
        }

        private void btn_pro_clear_Click(object sender, RoutedEventArgs e)
        {
            ProcessList.Items.Clear();
        }

        private void btn_pro_refresh_Click(object sender, RoutedEventArgs e)
        {
            ProcessList.Items.Clear();
            if (!bg_loadprocess.IsBusy)
                bg_loadprocess.RunWorkerAsync();
        }
        #endregion

        private void btn_get_pos_Click(object sender, RoutedEventArgs e)
        {
            GetWindowPosition(true);
        }

        void GetWindowPosition(bool log)
        {
            var pos = Helpers.GetWindowPosition(TargetWnd);

            left = pos.Left;
            right = pos.Right;
            bottom = pos.Bottom;
            top = pos.Top;

            if (log)
                Log(string.Format("Left:{0} , Top:{1} , Top:{2} , Top:{3}", left, top, right, bottom));

            //retrieves the last system error.
            Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        }

        private HoverControl OnTopControl;
        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            SetControl(true);
        }

        void SetControl(bool log)
        {
            if (OnTopControl != null)
                OnTopControl.Close();
            OnTopControl = new HoverControl();
            OnTopControl.Show();
            IntPtr OnTopHandle = Helpers.Find(OnTopControl.Name, OnTopControl.Title);

            OnTopControl.Left = left;
            OnTopControl.Top = top;

            if (log)
                Log("Hover Control Added!");

            Helpers.SetWindowLong(OnTopHandle, Helpers.GWLParameter.GWL_HWNDPARENT, TargetWnd.ToInt32());
        }

        IntPtr g_hook;
        private void btn_set_event_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> events = InitializeWinEventToHandlerMap();

            NativeMethods.WinEventProc eventHandler =
                new NativeMethods.WinEventProc(events[AccessibleEvents.LocationChange].Invoke);

            GCHandle gch = GCHandle.Alloc(eventHandler);

            g_hook = NativeMethods.SetWinEventHook(AccessibleEvents.LocationChange,
                AccessibleEvents.LocationChange, IntPtr.Zero, eventHandler
                , 0, 0, NativeMethods.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT);

            eventHandler = new NativeMethods.WinEventProc(events[AccessibleEvents.Destroy].Invoke);

            gch = GCHandle.Alloc(eventHandler);

            g_hook = NativeMethods.SetWinEventHook(AccessibleEvents.Destroy,
                AccessibleEvents.LocationChange, IntPtr.Zero, eventHandler
                , 0, 0, NativeMethods.SetWinEventHookParameter.WINEVENT_OUTOFCONTEXT);

        }

        private Dictionary<AccessibleEvents, NativeMethods.WinEventProc> InitializeWinEventToHandlerMap()
        {
            Dictionary<AccessibleEvents, NativeMethods.WinEventProc> dictionary = new Dictionary<AccessibleEvents, NativeMethods.WinEventProc>();
            dictionary.Add(AccessibleEvents.LocationChange, new NativeMethods.WinEventProc(this.LocationChangedCallback));
            dictionary.Add(AccessibleEvents.Destroy, new NativeMethods.WinEventProc(this.DestroyCallback));

            return dictionary;
        }

        private void DestroyCallback(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            if (accEvent == AccessibleEvents.Destroy && windowHandle.ToInt32() == TargetWnd.ToInt32())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DestroyHelper));
            }
        }

        private void DestroyHelper(object state)
        {
            Execute ex = delegate ()
            {
                NativeMethods.UnhookWinEvent(g_hook);
                OnTopControl.Close();
            };
            this.Dispatcher.Invoke(ex, null);
        }

        private void LocationChangedCallback(IntPtr winEventHookHandle, AccessibleEvents accEvent, IntPtr windowHandle, int objectId, int childId, uint eventThreadId, uint eventTimeInMilliseconds)
        {
            if (accEvent == AccessibleEvents.LocationChange && windowHandle.ToInt32() == TargetWnd.ToInt32())
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.LocationChangedHelper));
            }
        }

        private void LocationChangedHelper(object state)
        {
            Execute ex = delegate ()
            {
                if (OnTopControl != null)
                    OnTopControl.Close();
                GetWindowPosition(false);
                SetControl(false);
            };
            this.Dispatcher.Invoke(ex, null);
        }
    }
}
