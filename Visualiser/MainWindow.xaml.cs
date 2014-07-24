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
        List<string> frameData;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenTKControl.Initialise();
            openTKHost.Child = OpenTKControl.openTKWindow;
            frameData = new List<string>();
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
            Reset();
        }

        private void Reset()
        {
            frameData.Clear();
            dataTextBox.Clear();
            leapCheckBox.IsChecked = false;
            riftCheckBox.IsChecked = false;
            animatedCheckBox.IsChecked = false;
            modelComboBox.SelectedIndex = 0;
            dataTextBox.Visibility = Visibility.Visible;
            animationGrid.Visibility = Visibility.Hidden;
            loadDataButton.Content = "Load Data";
        }

        private void dataFormatButton_Click(object sender, RoutedEventArgs e)
        {
            DataFormater formatWindow = new DataFormater();
            Nullable<bool> result = formatWindow.ShowDialog();

            if(result == true)
            {
                //format set
            }
            else
            {
                //format cancelled
            }
        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            if(loadDataButton.Content.ToString() == "Load Data")
            {
                FileLoader(false);
            }
            else
            {
                FileLoader(true);
            }

            frameCountTextBlock.Text = frameData.Count + " Frames Loaded";
        }

        private void FileLoader(bool multi)
        {
            Microsoft.Win32.OpenFileDialog filePicker = new Microsoft.Win32.OpenFileDialog();

            if(multi)
                filePicker.Multiselect = true;

            Nullable<bool> result = filePicker.ShowDialog();

            if(result == true)
            {
                string[] fileNames = filePicker.FileNames;
                for(int i = 0; i < fileNames.Length; i++)
                {
                    string[] lines = System.IO.File.ReadAllLines(fileNames[i]);
                    string combinedText = "";
                    for (int j = 0; j < lines.Length; j++)
                    {
                        if (j != lines.Length - 1)
                        {
                            combinedText += lines[j] + "\r\n";
                        }
                        else
                        {
                            combinedText += lines[j];   //dont add carriage return on last entry
                        }
                    }

                    frameData.Add(combinedText);

                    if(frameData.Count == 1)
                        dataTextBox.Text = combinedText;
                }
                
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

        private void animatedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            dataTextBox.Visibility = Visibility.Hidden;
            animationGrid.Visibility = Visibility.Visible;
            loadDataButton.Content = "Load Frames";
            frameCountTextBlock.Text = frameData.Count + " Frames Loaded";
        }

        private void animatedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (frameData.Count > 1)
            {
                string messageText = "Multiple frames are currently loaded. To return to single data mode, please start a new session.\r\n" +
                                        "Do you wish to start a new session now?";
                string messageTitle = "Start New Session?";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;

                MessageBoxResult result = MessageBox.Show(messageText, messageTitle, button, icon);

                if (result == MessageBoxResult.Yes)
                {
                    Reset();
                }
            }
            else
            {
                dataTextBox.Visibility = Visibility.Visible;
                animationGrid.Visibility = Visibility.Hidden;
                loadDataButton.Content = "Load Data";
            }
        }
    }
}
