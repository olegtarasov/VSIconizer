//////////////////////////////////////////////////////////////////////////
// This file is a part of NotLimited.Framework.Wpf NuGet package.
// You are strongly discouraged from fiddling with it.
// If you do, all hell will break loose and living will envy the dead.
//////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace NotLimited.Framework.Wpf
{
	internal class BindingEvaluator : DependencyObject
	{
		public static readonly DependencyProperty TargetProperty =
			DependencyProperty.Register("Target", typeof(object), typeof(BindingEvaluator), new PropertyMetadata(default(object)));

		public object Target
		{
			get { return GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
	}

	public static class WpfExtensions
	{
		public static T GetContainerAtPoint<T>(this ItemsControl control, Point p) where T : DependencyObject
		{
			var hitTest = VisualTreeHelper.HitTest(control, p);
			var container = hitTest.VisualHit;

			if (container is T)
				return (T)container;

			while ((container = VisualTreeHelper.GetParent(container)) != null)
			{
				var result = container as T;
				if (result != null)
					return result;
			}

			return null;
		}

		public static object EvaluateProperty(this object source, PropertyPath path)
		{
			var evaluator = new BindingEvaluator();
			BindingOperations.SetBinding(
				evaluator,
				BindingEvaluator.TargetProperty,
				new Binding
				{
					Source = source,
					Path = path,
					Mode = BindingMode.OneTime
				});
			return evaluator.Target;
		}

		public static T EvaluateProperty<T>(this object source, PropertyPath path)
		{
			return (T)EvaluateProperty(source, path);
		}

		public static void SetProperty(this object source, PropertyPath path, object value)
		{
			var evaluator = new BindingEvaluator();
			BindingOperations.SetBinding(
				evaluator,
				BindingEvaluator.TargetProperty,
				new Binding
				{
					Source = source,
					Path = path,
					Mode = BindingMode.TwoWay
				});
			evaluator.Target = value;
		}

		public static Window GetParentWindow(this UserControl control)
		{
		    return GetParentElement<Window>(control);
		}

        public static T GetParentElement<T>(this FrameworkElement element) where T : FrameworkElement
        {
            var cur = element;

            while (cur != null && !(cur is T))
            {
				if (cur.TemplatedParent != null)
					cur = cur.TemplatedParent as FrameworkElement;
				else if (cur.Parent != null)
					cur = cur.Parent as FrameworkElement;
				else
					cur = VisualTreeHelper.GetParent(cur) as FrameworkElement;
            }

            return cur as T;
        }

		public static BindingExpressionBase SetBinding(this FrameworkElement element, DependencyProperty property, object source, string path, BindingMode mode = BindingMode.TwoWay)
		{
			return element.SetBinding(property, new Binding { Source = source, Path = new PropertyPath(path), Mode = mode });
		}

		public static IEnumerable<Binding> GetBindings(this DependencyObject obj)
		{
			var lve = obj.GetLocalValueEnumerator();

			while (lve.MoveNext())
			{
				var entry = lve.Current;

				if (BindingOperations.IsDataBound(obj, entry.Property))
					yield return (entry.Value as BindingExpression).ParentBinding;
			}
		}

		public static void UpdateAllBindingSources(this DependencyObject obj)
		{
			foreach (var binding in GetAllBindings(obj))
				binding.UpdateSource();
		}

        public static void UpdateAllBindingTargets(this DependencyObject obj)
        {
            foreach (var binding in GetAllBindings(obj))
                binding.UpdateTarget();
        }

		public static IEnumerable<BindingExpression> GetAllBindings(this DependencyObject obj)
		{
			var stack = new Stack<DependencyObject>();

			stack.Push(obj);

			while (stack.Count > 0)
			{
				var cur = stack.Pop();
				var lve = cur.GetLocalValueEnumerator();

				while (lve.MoveNext())
					if (BindingOperations.IsDataBound(cur, lve.Current.Property))
						yield return lve.Current.Value as BindingExpression;

				int count = VisualTreeHelper.GetChildrenCount(cur);
				for (int i = 0; i < count; ++i)
				{
					var child = VisualTreeHelper.GetChild(cur, i);
					if (child is FrameworkElement)
						stack.Push(child);
				}
			}
		}

		public static T FirstVisualChild<T>(this DependencyObject parent)
		{
			return GetVisualDescendants(parent).OfType<T>().FirstOrDefault();
		}

		public static T FirstVisualParent<T>(this DependencyObject child) where T : DependencyObject
		{
			var cur = VisualTreeHelper.GetParent(child);
			while (cur != null && cur.GetType() != typeof(T))
				cur = VisualTreeHelper.GetParent(cur);

			return (T)cur;
		}

		public static IEnumerable<DependencyObject> GetVisualDescendants(this DependencyObject root, bool includeSelf = false)
		{
			return GetVisualDescendants<DependencyObject>(root, includeSelf);
		}

		public static IEnumerable<T> GetVisualDescendants<T>(this DependencyObject root, bool includeSelf = false) where T : DependencyObject
		{
			var source = new Queue<DependencyObject>();

			if (includeSelf)
				yield return (T)root;

			source.Enqueue(root);
			while (source.Any())
			{
				var reference = source.Dequeue();
				int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
				for (int i = 0; i < childrenCount; i++)
				{
					var child = VisualTreeHelper.GetChild(reference, i);
					if (child != null)
					{
						source.Enqueue(child);
						yield return (T)child;
					}
				}
			}
		}

		public static IEnumerable<DependencyObject> GetVisualAncestorsAndSelf(this DependencyObject root)
		{
			yield return root;

			var cur = root;
			while ((cur = VisualTreeHelper.GetParent(cur)) != null)
				yield return cur;
		}


		public static IEnumerable<T> FindAllChildren<T, S>(this FrameworkElement root, Func<S, FrameworkElement> mapper) where T : FrameworkElement
		{
			var tree = new Queue<FrameworkElement>();
			tree.Enqueue(root);

			while (tree.Count > 0)
			{
				FrameworkElement current = tree.Dequeue();
				if (current == null)
					continue;
				if (current is T)
					yield return (T)current;

				int count = VisualTreeHelper.GetChildrenCount(current);

				if (count > 0)
				{
					for (int i = 0; i < count; ++i)
					{
						DependencyObject child = VisualTreeHelper.GetChild(current, i);
						if (child is FrameworkElement)
							tree.Enqueue((FrameworkElement)child);
					}
				}
				else
				{
					IEnumerable content = Enumerable.Empty<object>();

					if (current is ContentControl)
						content = new[] { ((ContentControl)current).Content };
					else if (current is ContentPresenter)
						content = new[] { ((ContentPresenter)current).Content };
					else if (current is ItemsControl)
						content = ((ItemsControl)current).Items;

					foreach (var obj in content)
						if (obj is FrameworkElement)
							tree.Enqueue((FrameworkElement)obj);
						else if (obj is S)
							tree.Enqueue(mapper((S)obj));
				}
			}
		}

		public static T FindChildByName<T>(this FrameworkElement root, string name) where T : FrameworkElement
		{
			Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
			tree.Push(root);

			while (tree.Count > 0)
			{
				FrameworkElement current = tree.Pop();
				if (current == null)
					continue;
				if (current.Name == name)
					return (T)current;

				int count = VisualTreeHelper.GetChildrenCount(current);

				if (count > 0)
				{
					for (int i = 0; i < count; ++i)
					{
						DependencyObject child = VisualTreeHelper.GetChild(current, i);
						if (child is FrameworkElement)
							tree.Push((FrameworkElement)child);
					}
				}
				else
				{
					var cc = current as ContentControl;

					if (cc != null)
						tree.Push((FrameworkElement)cc.Content);
				}


			}
			return null;
		}

		public static void ResizeColumns(this ListView lv, params double[] percent)
		{
			double totalWidth = lv.ActualWidth - 10; // Magic number, YARRRRRRRRRR!!!!!!
			GridView gv = (GridView)lv.View;

			// Go through all fixed widths
			for (int i = 0; i < percent.Length; i++)
			{
				if (percent[i] > 1)
				{
					totalWidth -= percent[i];
					gv.Columns[i].Width = percent[i];
				}
			}

			// Now iterate again, calculating variable widths
			for (int i = 0; i < percent.Length; i++)
			{
				if (percent[i] <= 1)
					gv.Columns[i].Width = (totalWidth < 0) ? 0 : totalWidth * percent[i];
			}
		}

		public static SizeChangedReactor AfterSizeChanged(this FrameworkElement element, SizeChangedEventHandler handler)
		{
			return SizeChangedReactor.AfterSizeChanged(element, handler);
		}
	}
}