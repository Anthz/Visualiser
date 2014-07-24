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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Visualiser
{
    public partial class MainWindow : Window
    {
        OpenTKFull openTKFull;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenTKControl.Initialise();
            openTKHost.Child = OpenTKControl.openTKWindow;
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            openTKFull = new OpenTKFull();
            Nullable<bool> result = openTKFull.ShowDialog();

            if(result == false)
            {
                openTKHost.Child = OpenTKControl.openTKWindow;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dataFormatButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog filePicker = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = filePicker.ShowDialog();

            if(result == true)
            {
                string fileName = filePicker.FileName;
                string[] lines = System.IO.File.ReadAllLines(fileName);
                string combinedText = "";
                for(int i = 0; i < lines.Length; i++)
                {
                    if(i != lines.Length - 1)
                    {
                        combinedText += lines[i] + "\r\n";
                    }
                    else
                    {
                        combinedText += lines[i];   //dont add carriage return on last entry
                    }
                }

                dataTextBox.Text = combinedText;
            }
        }

        private void riftCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string messageText = "To enable Oculus Rift stereoscopy and head tracking, full screen is required.\r\n" +
                                    "Do you wish to enter full screen mode now?";
            string messageTitle = "Switch To Full Screen?";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            MessageBoxResult result = MessageBox.Show(messageText, messageTitle, button, icon);

            if(result == MessageBoxResult.Yes)
            {
                fullScreenButton_Click(null, null);
            }
        }

        private void modelComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            modelComboBox.SelectedIndex = 0;
        }
    }
}
