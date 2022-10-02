using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AddButtonToWindows
{
    public class Processes
    {
        public void AddProcess()
        {
            Process[] localAll = Process.GetProcesses();
            foreach (Process pro in localAll)
            {

                    ProcessList.Items.Add(new PItem(pro.ProcessName, pro.MainWindowTitle));

            }
        }

        public void WindowFind()
        {
            PItem pro = ProcessList.Items.Add as PItem;

            string ModuleName = pro.ProcessName;
            string MainWindowTitle = pro.Title;

            var TargetWnd = Helpers.Find(ModuleName, MainWindowTitle);

            if (!TargetWnd.Equals(IntPtr.Zero))
                Log(ModuleName + " Window: " + TargetWnd.ToString());
            else
                Log(ModuleName + " Not found");
        }

        public void HoverControl()

        {
            if (OnTopControl != null)
                OnTopControl.Close();
            //Creates new instance of HoverControl
            HoverControl OnTopControl = new HoverControl();
            OnTopControl.Show();
            //Search for HoverControl handle
            IntPtr OnTopHandle = Helpers.Find(OnTopControl.Name, OnTopControl.Title);

            //Set the new location of the control (on top the titlebar)
            OnTopControl.Left = left;
            OnTopControl.Top = top;

            //Change target window to be parent of HoverControl.
            Helpers.SetWindowLong(OnTopHandle, Helpers.GWLParameter.GWL_HWNDPARENT,
            TargetWnd.ToInt32());

            Log("Hover Control Added!");
        }

        //void SetControl(bool log)
        //{
        //    if (OnTopControl != null)
        //        OnTopControl.Close();
        //    OnTopControl = new HoverControl();
        //    OnTopControl.Show();
        //    IntPtr OnTopHandle = Helpers.Find(OnTopControl.Name, OnTopControl.Title);

        //    OnTopControl.Left = left;
        //    OnTopControl.Top = top;

        //    if (log)
        //        Log("Hover Control Added!");

        //    Helpers.SetWindowLong(OnTopHandle,
        //    Helpers.GWLParameter.GWL_HWNDPARENT, TargetWnd.ToInt32());
        //}

        //void GetWindowPosition(bool log)
        //{
        //    var pos = Helpers.GetWindowPosition(TargetWnd);

        //    left = pos.Left;
        //    right = pos.Right;
        //    bottom = pos.Bottom;
        //    top = pos.Top;

        //    if (log)
        //        Log(string.Format("Left:{0} , Top:{1} , Top:{2} , Top:{3}",
        //        left, top, right, bottom));

        //    Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
        //}
    }
}
