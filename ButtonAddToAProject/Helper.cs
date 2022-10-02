using System.Diagnostics;

public class Helper
{
    public static void AddButtonToApp()
    {
        //Process currentProc = Process.GetCurrentProcess();
        //string name = currentProc.ProcessName;
        //string title = currentProc.MainWindowTitle;
        //var mainWindow = ExternalMethods.FindWindow(name, title);
        var mainWindow = ExternalMethods.FindWindow("AcrobatSDIWindow", null);

        if (mainWindow != IntPtr.Zero)
        {
            var child = ExternalMethods.FindWindowEx(mainWindow, IntPtr.Zero, "AVL_AVView", null);

            var button = new Button { Text = "Button", Left = 150, Top = 10, Width = 70, Height = 20 };
            
            button.Click += (o, args) => { Console.WriteLine("Clicked successfully"); };

            ExternalMethods.SetParent(button.Handle, child);
        }

        Console.WriteLine("test");
    }



}

