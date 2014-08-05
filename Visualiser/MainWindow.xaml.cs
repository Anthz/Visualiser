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
        public static List<Frame> frames;
        public static Dictionary<string, int> format;
        public static int currentFrame = 0;
        public static bool correctFormat;
        private string status;
        int statusDelay = 2000;

        public int CurrentFrame
        {
            get { return currentFrame; }

            set
            {
                currentFrame = value;
                currentFrameTextBlock.Text = "Current Frame: " + currentFrame;
                dataTextBox.Text = currentFrame > 0 ? frames[currentFrame - 1].displayData : "";
            }
        }

        public string Status
        {
            get { return status; }

            set
            {
                status = value;
                ShowStatus();
            }
        }

        private async void ShowStatus()
        {
            statusTextBlock.Text = status;
            await Task.Delay(statusDelay);
            statusTextBlock.Text = "";
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

            OpenTKControl.ChangeShader(0);
            OpenTKControl.ModelCollectionInit();
        }

        private void fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            if(riftCheckBox.IsChecked == true)
            {
                OpenTKControl.RiftEnabled = true;
            }

            openTKFull = new OpenTKFull();
            bool? result = openTKFull.ShowDialog();

            if(result == true || result == false)
            {
                OpenTKControl.RiftEnabled = false;
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
            if(format != null)
                format.Clear();
            dataTextBox.Clear();
            correctFormat = false;
            leapCheckBox.IsChecked = false;
            riftCheckBox.IsChecked = false;
            animatedCheckBox.IsChecked = false;
            modelComboBox.SelectedIndex = 0;
            dataGrid.Visibility = Visibility.Visible;
            animationGrid.Visibility = Visibility.Hidden;
            animationStatsGrid.Visibility = Visibility.Hidden;
            saveDataButton.Visibility = Visibility.Hidden;
            CurrentFrame = 0;
            frameCountTextBlock.Text = "Frame Count = 0";
            loadDataButton.Content = "Load Data";
            status = "";
            OpenTKControl.openTKWindow.Invalidate();
        }

        private void dataFormatButton_Click(object sender, RoutedEventArgs e)
        {
            DataFormater formatWindow = new DataFormater();
            bool? result = formatWindow.ShowDialog();
            Dictionary<string, int> tempFormat = format;
            
            if(result == true)
            {
                string inputFormat = formatWindow.formatTextBox.Text.ToLower();
                format = new Dictionary<string, int>();
                correctFormat = true;
                string[] splitFormat = inputFormat.Split(' ');
                for(int i = 0; i < splitFormat.Length; i++)
                {
                    format.Add(splitFormat[i], i);
                }

                //if format doesn't contain any axis, prompt for new format
                if(!splitFormat.Contains("x") && !splitFormat.Contains("y") && !splitFormat.Contains("z"))
                {
                    correctFormat = false;
                    ShowFailedFormatDialog(0);
                }

                //format correct up to now - if frame count != 0 check against frames
                if(correctFormat)
                {
                    if(frames.Count > 0)
                    {
                        List<int> failedFrames = new List<int>();

                        for(int i = 0; i < frames.Count; i++)
                        {
                            if(!frames[i].FormatData())
                            {
                                failedFrames.Add(i + 1); //make list of frames it failed to format for error message
                                correctFormat = false;
                            }
                        }
                        if(failedFrames.Count > 0)
                        {
                            ShowFailedFormatDialog(1, failedFrames);
                        }
                        else
                        {
                            Status = "Format successfully loaded";
                        }
                    }
                    else
                    {
                        //check on creation of frame
                        Status = "Format preloaded";
                    }
                }
                else
                {
                    format = tempFormat;    //if not correct then reset to prev
                    correctFormat = false;
                }

                OpenTKControl.openTKWindow.Invalidate();
            }
            else
            {
                //format cancelled
            }
        }

        private void loadDataButton_Click(object sender, RoutedEventArgs e)
        {
            FileLoader(loadDataButton.Content.ToString() != "Load Data");

            frameCountTextBlock.Text = "Frame Count: " + frames.Count;
            OpenTKControl.openTKWindow.Invalidate();
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

                Status = fileNames.Length + " Frame(s) Loaded";

                if(prevCount == 0 && frames.Count > 0)
                {
                    CurrentFrame = 1;
                    if(correctFormat)
                    {
                        List<int> failedFrames = new List<int>();

                        for (int i = 0; i < frames.Count; i++)
                        {
                            if(!frames[i].FormatData())
                            {
                                failedFrames.Add(i + 1); //make list of frames it failed to format for error message
                                correctFormat = false;
                            }
                        }

                        if(failedFrames.Count > 0)
                        {
                            ShowFailedFormatDialog(1, failedFrames);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays various error messages relating to formatting
        /// </summary>
        /// <param name="errorID">0 - No axis, 1 - Failed to format certain frames (use overrided func)</param>
        public void ShowFailedFormatDialog(int errorID)
        {
            ShowFailedFormatDialog(errorID, null);
        }

        public void ShowFailedFormatDialog(int errorID, List<int> failedFrames)
        {
            string messageText = "";
            const string messageTitle = "Formatting Error";
            const MessageBoxButton button = MessageBoxButton.OK;
            const MessageBoxImage icon = MessageBoxImage.Error;

            switch(errorID)
            {
                case 0:
                    messageText = "Please enter at least one axis to allow the data to be placed in the virtual scene.";
                    Status = "No axes";
                    break;
                case 1:
                    messageText = "The format provided failed to parse:\r\nFrame " + string.Join(", Frame ", failedFrames.ToArray());
                    Status = "Formatting failed";
                    break;
            }

            MessageBoxResult result = MessageBox.Show(messageText, messageTitle, button, icon);

            if(result == MessageBoxResult.OK)
            {
                format.Clear();
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
            frameCountTextBlock.Text = "Frame Count: " + frames.Count;
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

                MessageBoxResult result = MessageBox.Show(Application.Current.MainWindow, messageText, messageTitle, button, icon);

                if(result == MessageBoxResult.Yes)
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
                saveDataButton.Visibility = Visibility.Visible;
        }

        private void modelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectionName = ((ComboBoxItem)modelComboBox.SelectedItem).Content.ToString().ToLower();

            if(selectionName == "custom")
            {
                //new window to set name/file path of custom model
            }
            else
            {
                OpenTKControl.SetModel(selectionName);
            }
        }
    }
}
