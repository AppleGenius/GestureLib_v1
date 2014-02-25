#region using...
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
#endregion

namespace GestureLib
{
    public class CursorAdorner : Adorner
    {
        private readonly UIElement _adorningElement;
        private VisualCollection _visualChildren;
        private Canvas _cursorCanvas;
        protected FrameworkElement _cursor;
        private bool _isVisible;
        private bool _isOverridden;
        Storyboard _gradientStopAnimationStoryboard;

        readonly static Color _backColor = Colors.White;
        readonly static Color _foreColor = Colors.Gray;

        public CursorAdorner(FrameworkElement adorningElement) : base(adorningElement)
        {
            if (adorningElement == null)
                throw new ArgumentNullException("adorningElement");
            this._adorningElement = adorningElement;
            CreateCursorAdorner();
            this.IsHitTestVisible = false;
        }

        public CursorAdorner(FrameworkElement adorningElement, FrameworkElement innerCursor) : base(adorningElement)
        {
            if (adorningElement == null)
                throw new ArgumentNullException("Adorning Element parameter empty");
            this._adorningElement = adorningElement;
            CreateCursorAdorner(innerCursor);
            this.IsHitTestVisible = false;
        }

        public void CreateCursorAdorner()
        {
            var innerCursor = CreateCursor();
            CreateCursorAdorner(innerCursor);
        }

        public void UpdateCursor(Point position, bool isOverride)
        {
            _isOverridden = isOverride;
            // center the cursor visual at the new position
            _cursor.SetValue(Canvas.LeftProperty, position.X - (_cursor.ActualWidth / 2));
            _cursor.SetValue(Canvas.TopProperty, position.Y - (_cursor.ActualHeight / 2));
        }

        public void UpdateCursor(Point position)
        {
            if (_isOverridden)
                return;
            // center the cursor visual at the new position
            _cursor.SetValue(Canvas.LeftProperty, position.X - (_cursor.ActualWidth / 2));
            _cursor.SetValue(Canvas.TopProperty, position.Y - (_cursor.ActualHeight / 2));
        }

        protected FrameworkElement CreateCursor()
        {
            var brush = new LinearGradientBrush();
            brush.EndPoint = new Point(0, 1);
            brush.StartPoint = new Point(0, 0);
            brush.GradientStops.Add(new GradientStop(_backColor, 1));
            brush.GradientStops.Add(new GradientStop(_foreColor, 1));
            var cursor = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Fill = brush
            };
            return cursor;
        }

        public void CreateCursorAdorner(FrameworkElement innerCursor)
        {
            _visualChildren = new VisualCollection(this);
            _cursorCanvas = new Canvas();
            _cursor = innerCursor;
            _cursorCanvas.Children.Add(_cursor);
            _visualChildren.Add(this._cursorCanvas);
            AdornerLayer layer = AdornerLayer.GetAdornerLayer(_adorningElement);
            layer.Add(this);
        }

        #region Overridden methods

        protected override int VisualChildrenCount
        {
            get
            {
                return _visualChildren.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }

        protected override Size MeasureOverride(Size constraint)
        {
            this._cursorCanvas.Measure(constraint);
            return this._cursorCanvas.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            this._cursorCanvas.Arrange(new Rect(finalSize));
            return finalSize;
        }

        public void SetVisibility(bool isVisible)
        {
            if (this._isVisible && !isVisible)
                _cursorCanvas.Visibility = Visibility.Hidden;
            if (!this._isVisible && isVisible)
                _cursorCanvas.Visibility = Visibility.Visible;
            this._isVisible = isVisible;
        }

        public virtual void AnimateCursor(double milliSeconds)
        {
            CreateGradientStopAnimation(milliSeconds);
            if (_gradientStopAnimationStoryboard != null)
                _gradientStopAnimationStoryboard.Begin(this, true);
        }

        public virtual void StopCursorAnimation()
        {
            if (_gradientStopAnimationStoryboard != null)
                _gradientStopAnimationStoryboard.Stop(this);
        }

        public FrameworkElement CursorVisual
        {
            get
            {
                return _cursor;
            }

        }

        protected virtual void CreateGradientStopAnimation(double milliSeconds)
        {
            NameScope.SetNameScope(this, new NameScope());

            var cursor = _cursor as Shape;
            if (cursor == null)
                return;
            var brush = cursor.Fill as LinearGradientBrush;
            var stop1 = brush.GradientStops[0];
            var stop2 = brush.GradientStops[1];
            this.RegisterName("GradientStop1", stop1);
            this.RegisterName("GradientStop2", stop2);

            DoubleAnimation offsetAnimation = new DoubleAnimation();
            offsetAnimation.From = 1.0;
            offsetAnimation.To = 0.0;
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(milliSeconds);


            Storyboard.SetTargetName(offsetAnimation, "GradientStop1");
            Storyboard.SetTargetProperty(offsetAnimation,
                new PropertyPath(GradientStop.OffsetProperty));


            DoubleAnimation offsetAnimation2 = new DoubleAnimation();
            offsetAnimation2.From = 1.0;
            offsetAnimation2.To = 0.0;

            offsetAnimation2.Duration = TimeSpan.FromMilliseconds(milliSeconds);

            Storyboard.SetTargetName(offsetAnimation2, "GradientStop2");
            Storyboard.SetTargetProperty(offsetAnimation2,
                new PropertyPath(GradientStop.OffsetProperty));

            _gradientStopAnimationStoryboard = new Storyboard();
            _gradientStopAnimationStoryboard.Children.Add(offsetAnimation);
            _gradientStopAnimationStoryboard.Children.Add(offsetAnimation2);
            _gradientStopAnimationStoryboard.Completed += delegate { _gradientStopAnimationStoryboard.Stop(this); };

        }

        #endregion
    }
}
