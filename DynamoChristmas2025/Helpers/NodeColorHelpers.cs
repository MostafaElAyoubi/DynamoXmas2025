using Dynamo.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Xmas
{
    internal static class NodeColorHelpers
    {
        internal static void SetNodeColor(NodeView nv, SolidColorBrush color)
        {
            var nodeBackground = FindChildByName<Rectangle>(nv, "nodeBackground");
            var nameBackground = FindChildByName<Border>(nv, "nameBackground");
            if (nodeBackground != null) nodeBackground.Fill = color;
            if (nameBackground != null) nameBackground.Background = color;
        }

        private static T FindChildByName<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && typedChild.Name == name)
                {
                    return typedChild;
                }

                var result = FindChildByName<T>(child, name);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}