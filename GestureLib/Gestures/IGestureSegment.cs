#region using...
using System;
using Microsoft.Kinect;
#endregion

namespace GestureLib
{
    public interface IGestureSegment
    {
        GesturePartResult Update(Skeleton skeleton);
    }
}
