#region using...

using GestureLib;
using GestureLib.WPF;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#endregion

namespace GestureLibTest
{
    public partial class MainWindow : Window
    {
       Mode _mode = Mode.Color;

        GestureController _gestureController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            KinectSensor sensor = SensorExtensions.Default();

            if (sensor != null)
            {
                try
                {
                    sensor.EnableAllStreams();
                    sensor.ColorFrameReady += Sensor_ColorFrameReady;
                    sensor.DepthFrameReady += Sensor_DepthFrameReady;
                    sensor.SkeletonFrameReady += Sensor_SkeletonFrameReady;

                    _gestureController = new GestureController(GestureType.All);
                    _gestureController.GestureRecognized += GestureController_GestureRecognized;

                    sensor.Start();
                }
                catch(InvalidOperationException)
                {

                }
            }
        }

        void Sensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Color)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }
        }

        void Sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (var frame = e.OpenDepthImageFrame())
            {
                if (frame != null)
                {
                    if (_mode == Mode.Depth)
                    {
                        camera.Source = frame.ToBitmap();
                    }
                }
            }
        }

        void Sensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    canvas.ClearSkeletons();

                    var skeletons = frame.Skeletons().Where(s => s.TrackingState == SkeletonTrackingState.Tracked);

                    foreach (var skeleton in skeletons)
                    {
                        if (skeleton != null)
                        {
                            _gestureController.Update(skeleton);
                            canvas.DrawSkeleton(skeleton);
                        }
                    }
                }
            }
        }

        void GestureController_GestureRecognized(object sender, GestureEventArgs e)
        {
            tblGestures.Text = e.Name;

            switch (e.Type)
            {
                case GestureType.StatusChange:
                    break;
                case GestureType.Exit:
                    break;
                case GestureType.SwipeDown:
                    break;
                case GestureType.SwipeLeft:
                    break;
                case GestureType.SwipeRight:
                    break;
                case GestureType.SwipeUp:
                    break;
                case GestureType.WaveLeft:
                    break;
                case GestureType.WaveRight:
                    break;
                case GestureType.ZoomIn:
                    break;
                case GestureType.ZoomOut:
                    break;
                case GestureType.RotateClock:
                    break;
                case GestureType.RotateAntiClock:
                    break;
                case GestureType.TranslateLeft:
                    break;
                case GestureType.TranslateRight:
                    break;

                default:
                    break;
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Color;
        }

        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            _mode = Mode.Depth;
        }
    }
    
    public enum Mode
    {
        Color,
        Depth
    }
}
