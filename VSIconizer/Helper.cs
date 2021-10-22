using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace VSIconizer
{
    public static class Helper
    {
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
    }
}