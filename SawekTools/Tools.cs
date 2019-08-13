using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace SawekTools {
    public static class Tools {
        public static IEnumerable<T> FindInWindowLogical<T>(this DependencyObject depObj) where T : DependencyObject {
            if (depObj == null) yield break;

            foreach (var child in LogicalTreeHelper.GetChildren(depObj)) {
                if (child is T dependency_object) {
                    yield return dependency_object;
                }

                foreach (T childOfChild in FindInWindowLogical<T>(child as DependencyObject)) {
                    yield return childOfChild;
                }
            }
        }


        public static IEnumerable<T> FindInWindow<T>(this DependencyObject depObj) where T : DependencyObject {
            if (depObj == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T dependency_object) {
                    yield return dependency_object;
                }

                foreach (T childOfChild in FindInWindow<T>(child)) {
                    yield return childOfChild;
                }
            }
        }

        public static IEnumerable<UIElement> FindInWindow(this DependencyObject depObj) {
            if (depObj == null)
                yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                yield return child as UIElement;

                foreach (UIElement childOfChild in FindInWindow(child)) {
                    yield return childOfChild;
                }
            }
        }
    }
}