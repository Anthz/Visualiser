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
    public struct Format
    {
        public int x;
        public int y;
        public int z;
        public int data1;
        public int data2;
        public int data3;
        public bool correctFormat;
    }

    public partial class MainWindow : Window
    {
        OpenTKFull openTKFull;
        //List<string> frameData;
        private List<Frame> frames;
        public static Format format;
        private int _currentFrame = 0;

        public int CurrentFrame
        {
            get { return _currentFrame; }

            set
            {
                _currentFrame = value;
                currentFrameTextBlock.Text = "Current Frame: " + _currentFrame;
                dataTextBox.Text = _currentFrame > 0 ? frames[_currentFrame - 1].displayData : "";
            }
        }
        
        public MainWindow()
        {
            InitializeComponent();

            //new event handler for text input as "textinput" doesn't fire
            dataTextBox.AddHandler(TextInputEvent,
                                    new TextCompositionEventHandler(dataTextBox_TextInput), 
                                    true);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            OpenTKControl.Initialise();
            openTKHost.Child = OpenTKControl.openTKWindow;
            frames = new List<Frame>();
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            openTKFull = new OpenTKFull();
            bool? result = openTKFull.ShowDialog();

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
            frames.Clear();
            dataTextBox.Clear();
            leapCheckBox.IsChecked = false;
            riftCheckBox.IsChecked = false;
            animatedCheckBox.IsChecked = false;
            modelComboBox.SelectedIndex = 0;
            dataGrid.Visibility = Visibility.Visible;
            animationGrid.Visibility = Visibility.Hidden;
            animationStatsGrid.Visibility = Visibility.Hidden;
            saveDataButton.Visibility = Visibility.Hidden;
            CurrentFrame = 0;
            frameCountTextBlock.Text = "0 Frames Loaded";
            loadDataButton.Content = "Load Data";
        }

        private void dataFormatButton_Click(object sender, RoutedEventArgs e)
        {
            DataFormater formatWindow = new DataFormater();
            bool? result = formatWindow.ShowDialog();
            Format tempFormat = format;
            
            if(result == true)
            {
                string inputFormat = formatWindow.formatTextBox.Text.ToLower();
                format = new Format();
                format.correctFormat = true;
                string[] splitFormat = inputFormat.Split(' ');
                for(int i = 0; i < splitFormat.Length; i++)
                {
                    switch(splitFormat[i])
                    {
                        case "x":
                            format.x = i + 1;
                            break;
                        case "y":
                            format.y = i + 1;
                            break;
                        case "z":
                            format.z = i + 1;
                            break;
                        case "1":
                            format.data1 = i + 1;
                            break;
                        case "2":
                            format.data2 = i + 1;
                            break;
                        case "3":
                            format.data3 = i + 1;
                            break;
                        default:
                            format.correctFormat = false;
                            break;
                    }
                }

                //if format doesn't contain any axis, prompt for new format
                if(!splitFormat.Contains("x") && !splitFormat.Contains("y") && !splitFormat.Contains("z"))
                {
                    format.correctFormat = false;
                }

                List<int> failedFrames = new List<int>();

                //format correct up to now - if frame count != 0 check against frames
                if(format.correctFormat)
                {
                    if(frames.Count > 0)
                    {
                        for(int i = 0; i < frames.Count; i++)
                        {
                            if(!frames[i].FormatData())
                            {
                                failedFrames.Add(i + 1); //make list of frames it failed to format for error message
                                format.correctFormat = false;
                            }
                        }
                    }
                    else
                    {
                        //check on creation of frame
                    }
                }
                else
                {
                    format = tempFormat;    //if not correct then reset to prev
                }
                    

            }
            else
            {
                //format cancelled
                
            }
        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            FileLoader(loadDataButton.Content.ToString() != "Load Data");

            frameCountTextBlock.Text = frames.Count + " Frames Loaded";
        }

        private void FileLoader(bool multi)
        {
            int prevCount = frames.Count;

            Microsoft.Win32.OpenFileDialog filePicker = new Microsoft.Win32.OpenFileDialog();

            if(multi)
                filePicker.Multiselect = true;

            bool? result = filePicker.ShowDialog();

            if(result == true)
            {
                string[] fileNames = filePicker.FileNames;
                foreach (string t in fileNames)
                {
                    string[] lines = System.IO.File.ReadAllLines(t);
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

                    frames.Add(new Frame(t, lines, combinedText));
                }
                if(prevCount == 0 && frames.Count > 0)
                {
                    CurrentFrame = 1;
                }
            }
        }

        private void riftCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            const string messageText = "To enable Oculus Rift stereoscopy and head tracking, full screen is required.\r\n" +
                                       "Do you wish to enter full screen mode now?";
            const string messageTitle = "Switch To Full Screen?";
            const MessageBoxButton button = MessageBoxButton.YesNo;
            const MessageBoxImage icon = MessageBoxImage.Question;

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
            frameCountTextBlock.Text = frames.Count + " Frames Loaded";
        }

        private void animatedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (frames.Count > 1)
            {
                const string messageText = "Multiple frames are currently loaded. To return to single data mode, please start a new session.\r\n" +
                                           "Do you wish to start a new session now?";
                const string messageTitle = "Start New Session?";
                const MessageBoxButton button = MessageBoxButton.YesNo;
                const MessageBoxImage icon = MessageBoxImage.Question;

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
            if (frames.Count == 0)
            {
                //do nothing
            }
            else
            {
                if (CurrentFrame == 1)
                {
                    CurrentFrame = frames.Count;
                }
                else
                {
                    CurrentFrame--;
                }
            }
        }

        private void stepForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if(frames.Count == 0)
            {
                //do nothing
            }
            else
            {
                if(CurrentFrame == frames.Count)
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
            if(frames.Count == 0)
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

        private void dataTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back)
                saveButton.Visibility = Visibility.Visible;
        }
    }
}
