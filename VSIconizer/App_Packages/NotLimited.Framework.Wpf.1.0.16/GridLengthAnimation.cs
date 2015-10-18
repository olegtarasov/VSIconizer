//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace NotLimited.Framework.Wpf
{
    public class GridLengthAnimation : AnimationTimeline
    {
        #region GridLength From

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof (GridLength), typeof (GridLengthAnimation), new PropertyMetadata(default(GridLength)));

        public GridLength From
        {
            get { return (GridLength) GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        #endregion

        #region GridLength To

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof (GridLength), typeof (GridLengthAnimation), new PropertyMetadata(default(GridLength)));

        public GridLength To
        {
            get { return (GridLength) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }

        #endregion

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override Type TargetPropertyType
        {
            get { return typeof(GridLength); }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromVal = ((GridLength)GetValue(FromProperty)).Value;
            double toVal = ((GridLength)GetValue(ToProperty)).Value;

            if (fromVal > toVal)
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) *
                    (fromVal - toVal) + toVal, GridUnitType.Star);
            }
            else
            {
                return new GridLength(animationClock.CurrentProgress.Value *
                    (toVal - fromVal) + fromVal, GridUnitType.Star);
            }

        }
    }
}