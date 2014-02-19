#region using...
using Microsoft.Kinect;
#endregion

namespace GestureLib.Gestures
{
    public class TranslateLeftSegments1:IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            return GesturePartResult.Failed;
        }
    }

    public class TranslateLeftSegments2 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            return GesturePartResult.Failed;
        }
    }

    public class TranslateLeftSegments3 : IGestureSegment
    {
        public GesturePartResult Update(Skeleton skeleton)
        {
            return GesturePartResult.Failed;
        }
    }
}
