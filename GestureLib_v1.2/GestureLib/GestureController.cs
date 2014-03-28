#region using...
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using GestureLib.Gestures;
#endregion

namespace GestureLib
{
    public class GestureController
    {
        #region Members

        private List<Gesture> _gestures = new List<Gesture>();

        #endregion

        #region Constructors

        public GestureController()
        {
        }

        public GestureController(GestureType type)
        {
            if (type == GestureType.All)
            {
                foreach (GestureType t in Enum.GetValues(typeof(GestureType)))
                {
                    if (t != GestureType.All)
                    {
                        AddGesture(t);
                    }
                }
            }
            else
            {
                AddGesture(type);
            }
        }

        #endregion

        #region Events

        public event EventHandler<GestureEventArgs> GestureRecognized;

        #endregion

        #region Methods

        public void Update(Skeleton skeleton)
        {
            foreach (Gesture gesture in _gestures)
            {
                gesture.Update(skeleton);
            }
        }

        public void AddGesture(GestureType type)
        {
            IGestureSegment[] segments = null;

            switch (type)
            {
                case GestureType.StatusChange:
                    segments = new IGestureSegment[20];
                    StatusChangeSegment1 statusChangeSegment = new StatusChangeSegment1();
                    for (int i = 0; i < 20; i++)
                    {
                        segments[i] = statusChangeSegment;
                    }
                    break;

                case GestureType.Exit:
                    segments = new IGestureSegment[20];
                    ExitSegment exitSegment = new ExitSegment();
                    for (int i = 0; i < 20; i++)
                    {
                        segments[i] = exitSegment;
                    }
                    break;
                
                case GestureType.Restore:
                    segments = new IGestureSegment[20];
                    RestoreSegment restoreSegment = new RestoreSegment();
                    for (int i = 0; i < 20; i++)
                    {
                        segments[i] = restoreSegment;
                    }
                    break;

                case GestureType.SwipeDown:
                    segments = new IGestureSegment[11];
                    //the original Method
                    //segments[0] = new SwipeDownSegment1();
                    //segments[1] = new SwipeDownSegment2();
                    //On:140327
                    //To test the pause segments to configure the conflict gestures
                    SwipeDownSegment1 swipeDownSegment1 = new SwipeDownSegment1();
                    for (int i = 0; i < 10; i++)
                    {
                        segments[i] = swipeDownSegment1;
                    }
                    segments[11] = new SwipeDownSegment2();
                    break;

                case GestureType.SwipeLeft:
                    segments = new IGestureSegment[3];
                    segments[0] = new SwipeLeftSegment1();
                    segments[1] = new SwipeLeftSegment2();
                    segments[2] = new SwipeLeftSegment3();
                    break;

                case GestureType.SwipeRight:
                    segments = new IGestureSegment[3];
                    segments[0] = new SwipeRightSegment1();
                    segments[1] = new SwipeRightSegment2();
                    segments[2] = new SwipeRightSegment3();
                    break;

                case GestureType.SwipeUp:
                    segments = new IGestureSegment[2];
                    segments[0] = new SwipeUpSegment1();
                    segments[1] = new SwipeUpSegment2();
                    break;

                case GestureType.WaveLeft:
                    segments = new IGestureSegment[6];
                    WaveLeftSegment1 waveLeftSegment1 = new WaveLeftSegment1();
                    WaveLeftSegment2 waveLeftSegment2 = new WaveLeftSegment2();
                    segments[0] = waveLeftSegment1;
                    segments[1] = waveLeftSegment2;
                    segments[2] = waveLeftSegment1;
                    segments[3] = waveLeftSegment2;
                    segments[4] = waveLeftSegment1;
                    segments[5] = waveLeftSegment2;
                    break;

                case GestureType.WaveRight:
                    segments = new IGestureSegment[6];
                    WaveRightSegment1 waveRightSegment1 = new WaveRightSegment1();
                    WaveRightSegment2 waveRightSegment2 = new WaveRightSegment2();
                    segments[0] = waveRightSegment1;
                    segments[1] = waveRightSegment2;
                    segments[2] = waveRightSegment1;
                    segments[3] = waveRightSegment2;
                    segments[4] = waveRightSegment1;
                    segments[5] = waveRightSegment2;
                    break;

                case GestureType.ZoomOut:
                    segments = new IGestureSegment[3];
                    segments[0] = new ZoomSegment1();
                    segments[1] = new ZoomSegment2();
                    segments[2] = new ZoomSegment3();
                    break;

                case GestureType.ZoomIn:
                    segments = new IGestureSegment[3];
                    segments[0] = new ZoomSegment3();
                    segments[1] = new ZoomSegment2();
                    segments[2] = new ZoomSegment1();
                    break;
                case GestureType.RotateClock:
                    segments = new IGestureSegment[3];
                    segments[0] = new RotateClockSegment1();
                    segments[0] = new RotateClockSegment2();
                    segments[0] = new RotateClockSegment3();
                    break;

                case GestureType.RotateAntiClock:
                    segments = new IGestureSegment[3];
                    segments[0] = new RotateAntiClockSegment1();
                    segments[1] = new RotateAntiClockSegment2();
                    segments[2] = new RotateAntiClockSegment3();
                    break;

                case GestureType.TranslateLeft:
                    segments = new IGestureSegment[2];
                    segments[0] = new TranslateLeftSegment1();
                    segments[1] = new TranslateLeftSegment2();
                    //segments[2] = new TranslateLeftSegment3();
                    break;

                case GestureType.TranslateRight:
                    segments = new IGestureSegment[2];
                    segments[0] = new TranslateRightSegment1();
                    segments[1] = new TranslateRightSegment2();
                    //segments[2] = new TranslateRightSegment3();
                    break;

                case GestureType.All:
                case GestureType.None:

                default:
                    break;
            }

            if (type != GestureType.None)
            {
                Gesture gesture = new Gesture(type, segments);
                gesture.GestureRecognized += OnGestureRecognized;
                _gestures.Add(gesture);
            }
        }

        //public void AddGesture(string name, IGestureSegment[] segments)
        //{
        //    Gesture gesture = new Gesture(name, segments);
        //    gesture.GestureRecognized += OnGestureRecognized;

        //    _gestures.Add(gesture);
        //}

        #endregion

        #region Event handler

        private void OnGestureRecognized(object sender, GestureEventArgs e)
        {
            if (GestureRecognized != null)
            {
                GestureRecognized(this, e);
            }

            foreach (Gesture gesture in _gestures)
            {
                gesture.Reset();
            }
        }

        #endregion
    }
}
