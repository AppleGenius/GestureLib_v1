#region using...
using Microsoft.Kinect;
using System;
#endregion

namespace GestureLib.Gestures
{
    public class TranslateLeftSegments1 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Right and left hand in front of the Elbow
            if (skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.ElbowLeft].Position.Z && skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ElbowRight].Position.Z)
            {
                // Hands between shoulder and hip
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                    skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                    // Hands on ShoulderCenter Right
                    if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.ShoulderCenter].Position.X && skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderCenter].Position.X)
                    {
                        // Hands very close  the distance threshold can be changed 
                        if (Math.Abs(skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.HandLeft].Position.X) < 0)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Undetermined;
                    }
                    return GesturePartResult.Failed;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    public class TranslateLeftSegments2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Right and left hand in front of the Elbow
            if (skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.ElbowLeft].Position.Z && skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ElbowRight].Position.Z)
            {
                // Hands between shoulder and hip
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                    skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                    // Hands between ShoulderLeft and ShoulderRight 
                    if (skeleton.Joints[JointType.HandLeft].Position.X > skeleton.Joints[JointType.ShoulderLeft].Position.X && skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ShoulderRight].Position.X &&
                        skeleton.Joints[JointType.HandRight].Position.X > skeleton.Joints[JointType.ShoulderLeft].Position.X && skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ShoulderRight].Position.X)
                    {
                        // Hands very close  the distance threshold can be changed 
                        if (Math.Abs(skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.HandLeft].Position.X) < 0)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Undetermined;
                    }
                    return GesturePartResult.Failed;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

    public class TranslateLeftSegments3 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Right and left hand in front of the Elbow
            if (skeleton.Joints[JointType.HandLeft].Position.Z < skeleton.Joints[JointType.ElbowLeft].Position.Z && skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ElbowRight].Position.Z)
            {
                // Hands between shoulder and hip
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y &&
                    skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                    // Hands on ShoulderCenter left 
                    if (skeleton.Joints[JointType.HandLeft].Position.X < skeleton.Joints[JointType.ShoulderCenter].Position.X && skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ShoulderCenter].Position.X)
                    {
                        // Hands very close  the distance threshold can be changed 
                        if (Math.Abs(skeleton.Joints[JointType.HandRight].Position.X - skeleton.Joints[JointType.HandLeft].Position.X) < 0)
                        {
                            return GesturePartResult.Succeeded;
                        }
                        return GesturePartResult.Undetermined;
                    }
                    return GesturePartResult.Failed;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }
}
