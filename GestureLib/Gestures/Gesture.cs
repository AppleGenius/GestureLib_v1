#region using...
using Microsoft.Kinect;
using System;
#endregion

namespace GestureLib
{
    class Gesture
    {
        #region Constants

        readonly int WINDOW_SIZE = 50;
        readonly int MAX_PAUSE_COUNT = 10; // The max frames for a paused gesture.

        #endregion

        #region Members

        IGestureSegment[] _segments;    
        int _currentSegment = 0;        // The current gesture segment are matching.
        int _pausedFrameCount = 10;     // The number of frames to paused.
        int _frameCount = 0;            // Current frame.
        bool _paused = false;           // Paused status.
        string _name;                   // Gesture name.
        GestureType _type;              

        #endregion

        #region Events

        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Constructor

        public Gesture(string name, IGestureSegment[] segments)
        {
            _name = name;
            _segments = segments;
        }

        public Gesture(GestureType type, IGestureSegment[] segments)
        {
            _type = type;
            _segments = segments;

            _name = type.ToString();
        }

        #endregion

        #region Methods

        public void Update(Skeleton skeleton)
        {
            if (_paused)
            {
                if (_frameCount == _pausedFrameCount)
                {
                    _paused = false;
                }

                _frameCount++;
            }

            GesturePartResult result = _segments[_currentSegment].Update(skeleton);

            if (result == GesturePartResult.Succeeded)
            {
                if (_currentSegment + 1 < _segments.Length)
                {
                    _currentSegment++;
                    _frameCount = 0;
                    _pausedFrameCount = MAX_PAUSE_COUNT;
                    _paused = true;
                }
                else
                {
                    if (GestureRecognized != null)
                    {
                        GestureRecognized(this, new GestureEventArgs(_name, skeleton.TrackingId));   // Event Publication
                        Reset();
                    }
                }
            }
            else if (result == GesturePartResult.Failed || _frameCount == WINDOW_SIZE)
            {
                Reset();
            }
            else
            {
                _frameCount++;
                _pausedFrameCount = MAX_PAUSE_COUNT / 2;
                _paused = true;
            }
        }

        public void Reset()
        {
            _currentSegment = 0;
            _frameCount = 0;
            _pausedFrameCount = MAX_PAUSE_COUNT / 2;
            _paused = true;
        }

        #endregion
    }
}
