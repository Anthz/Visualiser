using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    /// <summary>
    /// Interaction logic for OpenTKFull.xaml
    /// </summary>
    public partial class OpenTKFull : Window
    {
        public OpenTKFull()
        {
            InitializeComponent();
        }

        private void FullScreenWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                DialogResult = false;
            }
        }

        private void FullScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            openTKFullHost.Child = OpenTKControl.openTKWindow;
        }
    }
}
