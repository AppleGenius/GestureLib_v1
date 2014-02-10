#region using...
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#endregion

namespace GestureLib
{
    public static class SensorExtensions
    {
        #region Public methods

        public static void EnableAllStreams(this KinectSensor sensor)
        {
            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable();
        }

        public static KinectSensor Default()
        {
            return KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
        }

        #endregion
    }
}
