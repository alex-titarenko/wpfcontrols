using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Shapes;


namespace TAlex.WPF.Services.DragAndDrop
{
    /// <summary>
    /// Represents the adorner for the dragged element.
    /// </summary>
    public class DragAdorner : Adorner
    {
        #region Fields

        private Rectangle _visual = null;

        private double _leftOffset;

        private double _topOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Services.DragAndDrop.DragAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner to.</param>
        /// <param name="dragedElement">The element that should be dragged.</param>
        /// <exception cref="System.ArgumentNullException">Raised when adornedElement is null.</exception>
        public DragAdorner(UIElement adornedElement, UIElement dragedElement)
            : base(adornedElement)
        {
            Size size = dragedElement.RenderSize;

            _visual = new Rectangle();
            _visual.Fill = new VisualBrush(dragedElement);
            _visual.Width = size.Width;
            _visual.Height = size.Height;
            _visual.IsHitTestVisible = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.Windows.Media.Transform"/> for the adorner, based on the transform
        /// that is currently applied to the adorned element.
        /// </summary>
        /// <param name="transform">The transform that is currently applied to the adorned element.</param>
        /// <returns>A transform to apply to the adorner.</returns>
        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));

            return result;
        }

        /// <summary>
        /// Called to remeasure a control.
        /// </summary>
        /// <param name="availableSize">
        /// The available size that this element can give to child elements. Infinity
        /// can be specified as a value to indicate that the element will size to whatever
        /// content is available.
        /// </param>
        /// <returns>
        /// The size that this element determines it needs during layout, based on its
        /// calculations of child element sizes.
        /// </returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            _visual.Measure(availableSize);
            return _visual.DesiredSize;
        }

        /// <summary>
        /// Called to arrange and size the content of a <see cref="TAlex.WPF.Services.DragAndDrop.DragAdorner"/> object.
        /// </summary>
        /// <param name="finalSize">
        /// The final area within the parent that this element should use to arrange
        /// itself and its children.
        /// </param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            _visual.Arrange(new Rect(finalSize));
            return finalSize;
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Overrides System.Windows.Media.Visual.GetVisualChild(System.Int32), and returns
        /// a child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the requested child element in the collection.
        /// This parameter is not used.
        /// </param>
        /// <returns>The requested child element.</returns>
        protected override Visual GetVisualChild(int index)
        {
            return _visual;
        }

        /// <summary>
        /// Sets the specified position for the dragged element.
        /// </summary>
        /// <param name="left">Left offset for the dragged element.</param>
        /// <param name="top">Top offset for the dragged element.</param>
        public void SetPosition(double left, double top)
        {
            _leftOffset = left;
            _topOffset = top;
            UpdateLocation();
        }

        #region Helpers

        private void UpdateLocation()
        {
            AdornerLayer adornerLayer = Parent as AdornerLayer;
            if (adornerLayer != null)
            {
                adornerLayer.Update(AdornedElement);
            }
        }

        #endregion

        #endregion
    }
}
