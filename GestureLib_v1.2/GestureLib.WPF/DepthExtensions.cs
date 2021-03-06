﻿#region using...
using System;
using System.Windows.Media;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics;
#endregion

namespace GestureLib.WPF
{
    public static class DepthExtensions
    {
        #region Constants

        static readonly int BLUE_INDEX = 0;
        static readonly int GREEN_INDEX = 1;
        static readonly int RED_INDEX = 2;

        static readonly float MAX_DEPTH_DISTANCE = 4095;
        static readonly float MIN_DEPTH_DISTANCE = 850;
        static readonly float MAX_DEPTH_DISTANCE_OFFSET = MAX_DEPTH_DISTANCE - MIN_DEPTH_DISTANCE;

        #endregion

        #region Public methods

        public static ImageSource ToBitmap(this DepthImageFrame frame, PixelFormat format, DepthImageMode mode)
        {
            short[] pixelData = new short[frame.PixelDataLength];
            frame.CopyPixelDataTo(pixelData);

            byte[] pixels;

            switch (mode)
            {
                case DepthImageMode.Raw:
                    pixels = GenerateRawFrame(frame, pixelData);
                    break;
                case DepthImageMode.Dark:
                    pixels = GenerateDarkFrame(frame, pixelData);
                    break;
                case DepthImageMode.Colors:
                    pixels = GenerateColoredFrame(frame, pixelData);
                    break;
                case DepthImageMode.Player:
                    pixels = GeneratePlayerFrame(frame, pixelData);
                    break;
                default:
                    pixels = GenerateRawFrame(frame, pixelData);
                    break;
            }

            return pixels.ToBitmap(frame.Width, frame.Height, format);
        }

        public static ImageSource ToBitmap(this DepthImageFrame frame, PixelFormat format)
        {
            return frame.ToBitmap(format, DepthImageMode.Raw);
        }

        public static ImageSource ToBitmap(this DepthImageFrame frame, DepthImageMode mode)
        {
            return frame.ToBitmap(PixelFormats.Bgr32, mode);
        }

        public static ImageSource ToBitmap(this DepthImageFrame frame)
        {
            return frame.ToBitmap(PixelFormats.Bgr32, DepthImageMode.Raw);
        }

        #endregion

        #region Helpers

        private static byte[] GenerateRawFrame(DepthImageFrame frame, short[] pixelData)
        {
            byte[] pixels = new byte[frame.Height * frame.Width * 4];

            for (int depthIndex = 0, colorIndex = 0; depthIndex < pixelData.Length && colorIndex < pixels.Length; depthIndex++, colorIndex += 4)
            {
                int player = pixelData[depthIndex] & DepthImageFrame.PlayerIndexBitmask;

                int depth = pixelData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // .9M very close
                if (depth <= 900) 
                {
                    pixels[colorIndex + BLUE_INDEX] = 255;
                    pixels[colorIndex + GREEN_INDEX] = 0;
                    pixels[colorIndex + RED_INDEX] = 0;

                }
                // .9M - 2M a bit further away
                else if (depth > 900 && depth < 2000)
                {
                    pixels[colorIndex + BLUE_INDEX] = 0;
                    pixels[colorIndex + GREEN_INDEX] = 255;
                    pixels[colorIndex + RED_INDEX] = 0;
                }
                // 2M+ farthest
                else if (depth > 2000) 
                {
                    pixels[colorIndex + BLUE_INDEX] = 0;
                    pixels[colorIndex + GREEN_INDEX] = 0;
                    pixels[colorIndex + RED_INDEX] = 255;
                }

                byte intensity = CalculateIntensityFromDepth(depth);
                pixels[colorIndex + BLUE_INDEX] = intensity;
                pixels[colorIndex + GREEN_INDEX] = intensity;
                pixels[colorIndex + RED_INDEX] = intensity;

                if (player > 0)
                {
                    pixels[colorIndex + BLUE_INDEX] = Colors.Gold.B;
                    pixels[colorIndex + GREEN_INDEX] = Colors.Gold.G;
                    pixels[colorIndex + RED_INDEX] = Colors.Gold.R;
                }
            }

            return pixels;
        }

        private static byte[] GeneratePlayerFrame(DepthImageFrame frame, short[] pixelData)
        {
            byte[] pixels = new byte[frame.Height * frame.Width * 4];

            for (int depthIndex = 0, colorIndex = 0; depthIndex < pixelData.Length && colorIndex < pixels.Length; depthIndex++, colorIndex += 4)
            {
                int player = pixelData[depthIndex] & DepthImageFrame.PlayerIndexBitmask;

                int depth = pixelData[depthIndex] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (player > 0)
                {
                    pixels[colorIndex + BLUE_INDEX] = Colors.Gold.B;
                    pixels[colorIndex + GREEN_INDEX] = Colors.Gold.G;
                    pixels[colorIndex + RED_INDEX] = Colors.Gold.R;
                }
                else
                {
                    pixels[colorIndex + BLUE_INDEX] = 0;
                    pixels[colorIndex + GREEN_INDEX] = 0;
                    pixels[colorIndex + RED_INDEX] = 0;
                }
            }

            return pixels;
        }

        private static byte[] GenerateDarkFrame(DepthImageFrame depthFrame, short[] pixelData)
        {
            int depth;
            int gray;
            int loThreshold = 1220;
            int hiThreshold = 3048;
            int bytesPerPixel = 4;
            byte[] enhPixelData = new byte[depthFrame.Width * depthFrame.Height * bytesPerPixel];

            for (int i = 0, j = 0; i < pixelData.Length; i++, j += bytesPerPixel)
            {
                depth = pixelData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (depth < loThreshold || depth > hiThreshold)
                {
                    gray = 0xFF;
                }
                else
                {
                    gray = (255 * depth / 0xFFF);
                }

                enhPixelData[j] = (byte)gray;
                enhPixelData[j + 1] = (byte)gray;
                enhPixelData[j + 2] = (byte)gray;
            }

            return enhPixelData;
        }

        private static byte[] GenerateColoredFrame(DepthImageFrame depthFrame, short[] pixelData)
        {
            int depth;
            double hue;
            int loThreshold = 1220;
            int hiThreshold = 3048;
            int bytesPerPixel = 4;
            byte[] rgb = new byte[3];
            byte[] enhPixelData = new byte[depthFrame.Width * depthFrame.Height * bytesPerPixel];

            for (int i = 0, j = 0; i < pixelData.Length; i++, j += bytesPerPixel)
            {
                depth = pixelData[i] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                if (depth < loThreshold || depth > hiThreshold)
                {
                    enhPixelData[j] = 0x00;
                    enhPixelData[j + 1] = 0x00;
                    enhPixelData[j + 2] = 0x00;
                }
                else
                {
                    hue = ((360 * depth / 0xFFF) + loThreshold);
                    ConvertHslToRgb(hue, 100, 100, rgb);

                    enhPixelData[j] = rgb[2];  //Blue
                    enhPixelData[j + 1] = rgb[1];  //Green
                    enhPixelData[j + 2] = rgb[0];  //Red
                }
            }

            return enhPixelData;
        }

        private static void ConvertHslToRgb(double hue, double saturation, double lightness, byte[] rgb)
        {
            double red = 0.0;
            double green = 0.0;
            double blue = 0.0;
            hue = hue % 360.0;
            saturation = saturation / 100.0;
            lightness = lightness / 100.0;

            if (saturation == 0.0)
            {
                red = lightness;
                green = lightness;
                blue = lightness;
            }
            else
            {
                double huePrime = hue / 60.0;
                int x = (int)huePrime;
                double xPrime = huePrime - (double)x;
                double L0 = lightness * (1.0 - saturation);
                double L1 = lightness * (1.0 - (saturation * xPrime));
                double L2 = lightness * (1.0 - (saturation * (1.0 - xPrime)));

                switch (x)
                {
                    case 0:
                        red = lightness;
                        green = L2;
                        blue = L0;
                        break;
                    case 1:
                        red = L1;
                        green = lightness;
                        blue = L0;
                        break;
                    case 2:
                        red = L0;
                        green = lightness;
                        blue = L2;
                        break;
                    case 3:
                        red = L0;
                        green = L1;
                        blue = lightness;
                        break;
                    case 4:
                        red = L2;
                        green = L0;
                        blue = lightness;
                        break;
                    case 5:
                        red = lightness;
                        green = L0;
                        blue = L1;
                        break;
                }
            }

            rgb[0] = (byte)(255.0 * red);
            rgb[1] = (byte)(255.0 * green);
            rgb[2] = (byte)(255.0 * blue);
        }

        private static byte CalculateIntensityFromDepth(int distance)
        {
            return (byte)(255 - (255 * Math.Max(distance - MIN_DEPTH_DISTANCE, 0) / (MAX_DEPTH_DISTANCE_OFFSET)));
        }

        #endregion
    }


    public enum DepthImageMode
    {
        Raw,

        Dark,

        Colors,

        Player
    }
}
