#region using...
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Diagnostics;
using Microsoft.Kinect;
#endregion 

namespace GestureLib.WPF
{
    public static class ImageExtensions
    {
        #region Constants

        static readonly double DPI = 96;

        #endregion

        #region Public methods

        public static ImageSource ToBitmap(this byte[] pixels, int width, int height, System.Windows.Media.PixelFormat format)
        {
            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, DPI, DPI, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this byte[] pixels, int width, int height)
        {
            return pixels.ToBitmap(width, height, PixelFormats.Bgr32);
        }

        public static bool Capture(this ImageSource bitmap, string path)
        {
            if (bitmap == null || path == null || string.IsNullOrWhiteSpace(path)) return false;

            try
            {
                BitmapEncoder encoder;

                switch (Path.GetExtension(path))
                {
                    case ".jpg":
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    default:
                        return false;
                }

                encoder.Frames.Add(BitmapFrame.Create(bitmap as BitmapSource));

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    encoder.Save(stream);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public static bool Capture(this ImageFrame frame, string path)
        {
            if (frame == null) return false;

            if (frame is ColorImageFrame)
            {
                return Capture((frame as ColorImageFrame).ToBitmap(), path);
            }
            else if (frame is DepthImageFrame)
            {
                return Capture((frame as DepthImageFrame).ToBitmap(), path);
            }

            return false;
        }

        #endregion    
    }
}
