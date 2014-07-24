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

namespace Visualiser
{
    /// <summary>
    /// Interaction logic for DataFormater.xaml
    /// </summary>
    public partial class DataFormater : Window
    {
        public DataFormater()
        {
            InitializeComponent();
        }

        private void dataFormatCancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dataFormatOKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
