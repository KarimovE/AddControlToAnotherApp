public class Helper
{
    public static void AddButtonToApp()
    {
        var mainWindow = ExternalMethods.FindWindow("AdImpApplicationFrame", null);

        if (mainWindow != IntPtr.Zero)
        {
            var child = ExternalMethods.FindWindowEx(mainWindow, IntPtr.Zero, "ATL:6549C390", null);

            if (child != IntPtr.Zero)
            {
                var button = new Button { Text = "Button", Left = 150, Top = 10, Width = 70, Height = 20 };
                button.Click += (o, args) => { Console.WriteLine("Clicked successfully"); };

                ExternalMethods.SetParent(button.Handle, child);
            }
        }

        //var currentProc = Process.GetCurrentProcess();
        //string name = currentProc.ProcessName;

     
        Console.WriteLine("test");
    }



}

