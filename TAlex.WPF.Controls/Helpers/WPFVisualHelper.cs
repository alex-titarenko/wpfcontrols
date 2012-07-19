using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace TAlex.WPF.Helpers
{
    /// <summary>
    /// Provides helper methods that perform common tasks involving visual elements.
    /// </summary>
    public class WPFVisualHelper
    {
        /// <summary>
        /// Helper to search up the VisualTree.
        /// </summary>
        public static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }

                if (current is Visual || current is Visual3D)
                    current = VisualTreeHelper.GetParent(current);
                else
                    current = null;
            }
            while (current != null);

            return null;
        }
    }
}
