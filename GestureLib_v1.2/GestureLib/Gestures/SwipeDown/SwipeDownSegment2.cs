#region using...
using Microsoft.Kinect;
#endregion

namespace GestureLib.Gestures
{
    public class SwipeDownSegment2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // right hand in front of right shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ShoulderRight].Position.Z)
            {
                // right hand below shoulder center and right hand up the hip center
                if (skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.ShoulderCenter].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.HipCenter].Position.Y)
                {
                    // right elbow right of right shoulder and right hand
                    if (skeleton.Joints[JointType.ElbowRight].Position.X > skeleton.Joints[JointType.HandRight].Position.X && skeleton.Joints[JointType.ElbowRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X)
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