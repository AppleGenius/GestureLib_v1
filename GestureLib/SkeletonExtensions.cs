#region using...
using System;
using Microsoft.Kinect;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
#endregion

namespace GestureLib
{
    public static class SkeletonExtensions
    {
        #region Members

        static Skeleton[] _skeletons = new Skeleton[6];

        #endregion

        #region Public methods

        public static Skeleton[] Skeletons(this SkeletonFrame frame)
        {
            frame.CopySkeletonDataTo(_skeletons);

            return _skeletons;
        }

        public static Skeleton Default(this Skeleton[] skeletons)
        {
            return skeletons.Where(s => s.TrackingState == SkeletonTrackingState.Tracked).FirstOrDefault();
        }

        public static double Height(this Skeleton skeleton)
        {
            const double HEAD_DIVERGENCE = 0.1;

            var head = skeleton.Joints[JointType.Head];
            var neck = skeleton.Joints[JointType.ShoulderCenter];
            var spine = skeleton.Joints[JointType.Spine];
            var waist = skeleton.Joints[JointType.HipCenter];
            var hipLeft = skeleton.Joints[JointType.HipLeft];
            var hipRight = skeleton.Joints[JointType.HipRight];
            var kneeLeft = skeleton.Joints[JointType.KneeLeft];
            var kneeRight = skeleton.Joints[JointType.KneeRight];
            var ankleLeft = skeleton.Joints[JointType.AnkleLeft];
            var ankleRight = skeleton.Joints[JointType.AnkleRight];
            var footLeft = skeleton.Joints[JointType.FootLeft];
            var footRight = skeleton.Joints[JointType.FootRight];

            int legLeftTrackedJoints = NumberOfTrackedJoints(hipLeft, kneeLeft, ankleLeft, footLeft);
            int legRightTrackedJoints = NumberOfTrackedJoints(hipRight, kneeRight, ankleRight, footRight);

            double legLength = legLeftTrackedJoints > legRightTrackedJoints ? Distance(hipLeft, kneeLeft, ankleLeft, footLeft) : Distance(hipRight, kneeRight, ankleRight, footRight);

            return Distance(head, neck, spine, waist) + legLength + HEAD_DIVERGENCE;
        }

        public static double UpperHeight(this Skeleton skeleton)
        {
            var head = skeleton.Joints[JointType.Head];
            var neck = skeleton.Joints[JointType.ShoulderCenter];
            var spine = skeleton.Joints[JointType.Spine];
            var waist = skeleton.Joints[JointType.HipCenter];

            return Distance(head, neck, spine, waist);
        }

        public static double Distance(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2) +
                Math.Pow(p1.Position.Z - p2.Position.Z, 2));
        }

        public static double Distance(params Joint[] joints)
        {
            double length = 0;

            for (int index = 0; index < joints.Length - 1; index++)
            {
                length += Distance(joints[index], joints[index + 1]);
            }

            return length;
        }

        public static double DistanceFrom(this Joint p1, Joint p2)
        {
            return Distance(p1, p2);
        }

        public static int NumberOfTrackedJoints(params Joint[] joints)
        {
            int trackedJoints = 0;

            foreach (var joint in joints)
            {
                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    trackedJoints++;
                }
            }

            return trackedJoints;
        }

        public static Joint ScaleTo(this Joint joint, double width, double height, float skeletonMaxX, float skeletonMaxY)
        {
            joint.Position = new SkeletonPoint()
            {
                X = Scale(width, skeletonMaxX, joint.Position.X),
                Y = Scale(height, skeletonMaxY, -joint.Position.Y),
                Z = joint.Position.Z
            };

            return joint;
        }

        public static Joint ScaleTo(this Joint joint, double width, double height)
        {
            return ScaleTo(joint, width, height, 1.0f, 1.0f);
        }

        #endregion

        #region Helpers

        private static float Scale(double maxPixel, double maxSkeleton, float position)
        {
            float value = (float)((((maxPixel / maxSkeleton) / 2) * position) + (maxPixel / 2));

            if (value > maxPixel)
            {
                return (float)maxPixel;
            }

            if (value < 0)
            {
                return 0;
            }

            return value;
        }

        #endregion
    }
}
