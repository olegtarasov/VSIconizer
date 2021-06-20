//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Windows;

namespace NotLimited.Framework.Wpf
{
	public class SizeChangedReactor
	{
		private readonly Queue<SizeChangedEventHandler> _handlers = new Queue<SizeChangedEventHandler>();

		public SizeChangedReactor Then(SizeChangedEventHandler handler)
		{
			_handlers.Enqueue(handler);
			return this;
		}

		public static SizeChangedReactor AfterSizeChanged(FrameworkElement element, SizeChangedEventHandler handler)
		{
			return new SizeChangedReactor(element, handler);
		}

		private SizeChangedReactor(FrameworkElement element, SizeChangedEventHandler handler)
		{
			_handlers.Enqueue(handler);
			element.SizeChanged += OnSizeChanged;
		}

		private void OnSizeChanged(object sender, SizeChangedEventArgs args)
		{
			var handler = _handlers.Dequeue();
			if (_handlers.Count == 0)
				((FrameworkElement)sender).SizeChanged -= OnSizeChanged;

			handler(sender, args);
		}
	}
}