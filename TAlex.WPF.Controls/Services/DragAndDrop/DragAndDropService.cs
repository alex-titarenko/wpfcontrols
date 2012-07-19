using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace TAlex.WPF.Services.DragAndDrop
{
    /// <summary>
    /// Provides attached property for <see cref="System.Windows.Controls.ItemsControl"/> that implement drag and drop operation for items.
    /// </summary>
    public static class DragAndDropService
    {
        #region Fields

        /// <summary>
        /// Identifies the DragAndDropService.DragAndDropManager attached property.
        /// </summary>
        public static readonly DependencyProperty DragAndDropManagerProperty;

        #endregion

        #region Constructors

        static DragAndDropService()
        {
            DragAndDropManagerProperty = DependencyProperty.RegisterAttached("DragAndDropManager", typeof(ItemsControlDragDropManager),
                typeof(DragAndDropService), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, PropertyChangedCallback));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of the DragAndDropService.DragAndDropManager attached property
        /// from a given <see cref="System.Windows.DependencyObject"/>.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The value of the DragAndDropService.DragAndDropManager attached property.</returns>
        [AttachedPropertyBrowsableForType(typeof(ItemsControl))]
        public static ItemsControlDragDropManager GetDragAndDropManager(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (ItemsControlDragDropManager)element.GetValue(DragAndDropManagerProperty);
        }

        /// <summary>
        /// Sets the value of the DragAndDropService.DragAndDropManager attached property
        /// to a given <see cref="System.Windows.UIElement"/>.
        /// </summary>
        /// <param name="element">The element on which to set the attached property.</param>
        /// <param name="value">The property value to set.</param>
        public static void SetDragAndDropManager(DependencyObject element, ItemsControlDragDropManager value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(DragAndDropManagerProperty, value);
        }


        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                ItemsControl control = d as ItemsControl;

                if (control != null)
                {
                    ((ItemsControlDragDropManager)e.NewValue).TargetElement = control;
                }
            }
        }

        #endregion
    }
}
