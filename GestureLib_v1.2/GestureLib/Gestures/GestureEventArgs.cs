#region using...
using System;
#endregion

namespace GestureLib
{
    public class GestureEventArgs : EventArgs
    {
        #region Properties

        public GestureType Type { get; private set; }

        public string Name { get; private set; }        

        public int TrackingID { get; private set; }

        #endregion

        #region Constructors

        public GestureEventArgs()
        {
        }

        public GestureEventArgs(GestureType type, int trackingID)
        {
            Type = type;
            TrackingID = trackingID;
        }

        public GestureEventArgs(string name, int trackingID)
        {
            Name = name;
            TrackingID = trackingID;
        }

        #endregion
    }
}
