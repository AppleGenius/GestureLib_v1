#region using...
using Microsoft.Kinect;
#endregion

namespace GestureLib.Gestures
{
    public class SwipeUpSegment2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // right hand in front of right shoulder
            if (skeleton.Joints[JointType.HandRight].Position.Z < skeleton.Joints[JointType.ShoulderRight].Position.Z)
            {
                // right hand up the head
                if (skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.Head].Position.Y)
                {
                    // right eblow right of right shoulder and right hand
                    if (skeleton.Joints[JointType.ElbowRight].Position.X > skeleton.Joints[JointType.ShoulderRight].Position.X && skeleton.Joints[JointType.ElbowRight].Position.X > skeleton.Joints[JointType.HandRight].Position.X)
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