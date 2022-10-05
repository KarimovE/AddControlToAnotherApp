using System.Windows;
using System.Windows.Input;

namespace AddButtonToWindows
{
    public partial class HoverControl : Window
    {
        public HoverControl()
        {
            InitializeComponent();
        }

        public void rectangle1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
