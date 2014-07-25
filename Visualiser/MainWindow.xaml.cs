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
        static string format;
        OpenTKFull openTKFull;
        List<string> frameData;
        private int _currentFrame = 0;

        public int CurrentFrame
        {
            get { return _currentFrame; }

            set
            {
                _currentFrame = value;
                currentFrameTextBlock.Text = "Current Frame: " + _currentFrame;
                if(_currentFrame > 0)
                    dataTextBox.Text = frameData[_currentFrame - 1];
                else
                    dataTextBox.Text = "";
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            //new event handler for text input as "textinput" doesn't fire
            dataTextBox.AddHandler(TextBox.TextInputEvent,
                                    new TextCompositionEventHandler(dataTextBox_TextInput), 
                                    true);
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
            dataGrid.Visibility = Visibility.Visible;
            animationGrid.Visibility = Visibility.Hidden;
            animationStatsGrid.Visibility = Visibility.Hidden;
            CurrentFrame = 0;
            frameCountTextBlock.Text = "0 Frames Loaded";
            loadDataButton.Content = "Load Data";
        }

        private void dataFormatButton_Click(object sender, RoutedEventArgs e)
        {
            DataFormater formatWindow = new DataFormater();
            Nullable<bool> result = formatWindow.ShowDialog();
            string tempFormat = format;
            format = formatWindow.formatTextBox.Text;

            if(result == true)
            {
                //format set - if frame count != 0 check against frames
                //else check on creation of frame
                int i = 0;
                
            }
            else
            {
                //format cancelled
                format = tempFormat;
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
            int prevCount = frameData.Count;

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

                    //frameList.add(new Frame(fileNames[i], combinedText (or line array), lines.length));

                    frameData.Add(combinedText);
                }
                if(prevCount == 0 && frameData.Count > 0)
                {
                    CurrentFrame = 1;
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
            dataGrid.Visibility = Visibility.Hidden;
            animationGrid.Visibility = Visibility.Visible;
            animationStatsGrid.Visibility = Visibility.Visible;
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
                dataGrid.Visibility = Visibility.Visible;
                animationGrid.Visibility = Visibility.Hidden;
                animationStatsGrid.Visibility = Visibility.Hidden;
                loadDataButton.Content = "Load Data";
            }
        }

        private void stepBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (frameData.Count == 0)
            {
                //do nothing
            }
            else
            {
                if (CurrentFrame == 1)
                {
                    CurrentFrame = frameData.Count;
                }
                else
                {
                    CurrentFrame--;
                }
            }
        }

        private void stepForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if(frameData.Count == 0)
            {
                //do nothing
            }
            else
            {
                if(CurrentFrame == frameData.Count)
                {
                    CurrentFrame = 1;
                }
                else
                {
                    CurrentFrame++;
                }
            }
        }

        private void viewFrameDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (animationGrid.Visibility == Visibility.Visible)
            {
                animationGrid.Visibility = Visibility.Hidden;
                dataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                animationGrid.Visibility = Visibility.Visible;
                dataGrid.Visibility = Visibility.Hidden;
            }
        }

        private void saveDataButton_Click(object sender, RoutedEventArgs e)
        {
            //save text box to frame data
            if(frameData.Count == 0)
            {
                Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
                //if no frames loaded, open save dialog with blank name
                //if frames found, set directory and name of current frame
            }
        }

        private void dataTextBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            if(saveDataButton.Visibility == Visibility.Hidden)
                saveDataButton.Visibility = Visibility.Visible;
        }

        private void customModelSelection_Selected(object sender, RoutedEventArgs e)
        {
            //model loader from project
        }
    }
}
