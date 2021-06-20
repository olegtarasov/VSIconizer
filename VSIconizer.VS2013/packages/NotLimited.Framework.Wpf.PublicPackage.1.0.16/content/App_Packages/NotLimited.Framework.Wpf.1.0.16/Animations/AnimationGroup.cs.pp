//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace NotLimited.Framework.Wpf.Animations
{
	public class AnimationGroup
	{
		private readonly Storyboard _storyboard;

		public Action BeginAction { get; set; }
		public Action EndAction { get; set; }

		public AnimationGroup()
		{
			_storyboard = new Storyboard();
			_storyboard.Completed += _storyboard_Completed;
		}

		public void AddAnimation(DependencyObject control, string propertyName, Timeline animation)
		{
			Storyboard.SetTarget(animation, control);
			Storyboard.SetTargetProperty(animation, new PropertyPath(propertyName));

			_storyboard.Children.Add(animation);
		}

		public void Start()
		{
			if (BeginAction != null)
				BeginAction();

			_storyboard.Begin();
		}

		public void Stop()
		{
			_storyboard.Stop();
		}

		private void _storyboard_Completed(object sender, EventArgs e)
		{
			_storyboard.Completed -= _storyboard_Completed;

			if (EndAction != null)
				EndAction();
		}
	}
}