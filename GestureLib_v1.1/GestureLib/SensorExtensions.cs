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
            TransformSmoothParameters tsp = new TransformSmoothParameters();
            tsp.Smoothing = 0.5f;
            tsp.Correction = 0.5f;
            tsp.Prediction = 0.5f;
            tsp.JitterRadius = 0.05f;
            tsp.MaxDeviationRadius = 0.04f;

            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable(tsp);
        }

        public static KinectSensor Default()
        {
            return KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
        }

        #endregion
    }
}
