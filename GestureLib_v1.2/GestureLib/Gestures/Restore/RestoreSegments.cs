#region using...
using Microsoft.Kinect;
#endregion

namespace GestureLib.Gestures
{
    public class RestoreSegment : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            // Left and right hands below hip left and hip right
            if (skeleton.Joints[JointType.HandLeft].Position.Y < skeleton.Joints[JointType.HipLeft].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y < skeleton.Joints[JointType.HipRight].Position.Y)
            {
                // Left and right hands up hip left and hip right
                if (skeleton.Joints[JointType.HandLeft].Position.Y > skeleton.Joints[JointType.KneeLeft].Position.Y && skeleton.Joints[JointType.HandRight].Position.Y > skeleton.Joints[JointType.KneeRight].Position.Y)
                {
                    return GesturePartResult.Succeeded;   
                }
                return GesturePartResult.Undetermined;
            }
            return GesturePartResult.Failed;
        }
    }
}

