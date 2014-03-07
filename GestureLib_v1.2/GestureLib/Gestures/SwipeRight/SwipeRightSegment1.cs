#region using...
using Microsoft.Kinect;
#endregion

namespace GestureLib.Gestures
{
    public class SwipeRightSegment1 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Right hand in front of right Shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ElbowRight].Position.Z && skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y)
            {
                // right hand below shoulder height but above hip height
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                    // right hand left of center hip
                    if (skeleton.Joints[JointType.HandRight].Position.X < skeleton.Joints[JointType.ShoulderLeft].Position.X)
                    {
                        return GesturePartResult.Succeeded;
                    }
                    return GesturePartResult.Undetermined;
                }
                return GesturePartResult.Failed;
            }
            return GesturePartResult.Failed;
        }
    }

}
