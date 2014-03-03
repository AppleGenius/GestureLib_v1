#region using...
using System;
using System.Windows.Media;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;
using System.IO;
using System.Windows.Media.Imaging;
using System.Diagnostics;
#endregion

namespace GestureLib.WPF
{
    public static class ColorExtensions
    {
        #region Public methods

        public static ImageSource ToBitmap(this ColorImageFrame frame, System.Windows.Media.PixelFormat format)
        {
            byte[] pixels = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixels);

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        public static ImageSource ToBitmap(this ColorImageFrame frame)
        {
            return frame.ToBitmap(PixelFormats.Bgr32);
        }

        public static ImageSource ToBitmap(this BackgroundRemovedColorFrame frame, System.Windows.Media.PixelFormat format)
        {
            byte[] pixels = new byte[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixels);

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        public static ImageSource ToBitmap(this BackgroundRemovedColorFrame frame)
        {
            return frame.ToBitmap(PixelFormats.Bgra32);
        }

        #endregion
    }
}
