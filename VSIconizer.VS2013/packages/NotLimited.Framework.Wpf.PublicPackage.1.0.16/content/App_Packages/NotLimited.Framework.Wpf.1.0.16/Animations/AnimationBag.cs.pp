//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace NotLimited.Framework.Wpf.Animations
{
	public class AnimationBag
    {
        private readonly Dictionary<string, AnimationItem> _storyboards = new Dictionary<string, AnimationItem>();

        public void AddAnimation(string name, DependencyObject control, string propertyName, Timeline animation, Action beginAction = null, Action endAction = null)
        {
            var board = new Storyboard();
            var item = new AnimationItem(board)
	            {
		            BeginAction = beginAction, 
					EndAction = endAction,
					Control = control,
					PropertyPath = new PropertyPath(propertyName)
	            };

            Storyboard.SetTarget(animation, control);
            Storyboard.SetTargetProperty(animation, item.PropertyPath);
            board.Children.Add(animation);
			_storyboards[name] = item;
        }

		public void StartAnimation(string name)
        {
            if (!_storyboards.ContainsKey(name))
                return;

            _storyboards[name].Start();
        }

        public void StopAnimation(string name)
        {
            if (!_storyboards.ContainsKey(name))
                return;

            _storyboards[name].Stop();
		}

        public static DoubleAnimation CreateDoubleAnimation(double? to, long durationMs, double? from = null, bool repeat = false)
        {
            var ret = new DoubleAnimation { To = to, From = from, Duration = new Duration(TimeSpan.FromMilliseconds(durationMs)) };

            if (repeat)
                ret.RepeatBehavior = RepeatBehavior.Forever;

            return ret;
        }
    }

}