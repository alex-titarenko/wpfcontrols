using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;


namespace TAlex.WPF.Services.DragAndDrop
{
    /// <summary>
    /// Represents the adorner for the inserted element.
    /// </summary>
    public class InsertionAdorner : Adorner
    {
        #region Fields

        private InsertionPosition _position;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the insertion position for dragged element.
        /// </summary>
        public InsertionPosition Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;

                AdornerLayer adornerLayer = Parent as AdornerLayer;
                if (adornerLayer != null)
                {
                    adornerLayer.Update(AdornedElement);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Services.DragAndDrop.InsertionAdorner"/> class.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner to.</param>
        /// <exception cref="System.ArgumentNullException">Raised when adornedElement is null.</exception>
        public InsertionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Renders the contents of an <see cref="TAlex.WPF.Services.DragAndDrop.InsertionAdorner"/>.
        /// </summary>
        /// <param name="drawingContext">
        /// The drawing instructions for a specific element. This context is provided to the layout system.
        /// </param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            double width = AdornedElement.RenderSize.Width;
            double height = AdornedElement.RenderSize.Height;

            Point startPoint = new Point();
            Point endPoint = new Point();

            endPoint.X = width;

            switch (Position)
            {
                case InsertionPosition.Bottom:
                    startPoint.Y = height;
                    endPoint.Y = height;
                    break;
            }

            drawingContext.DrawLine(new Pen(Brushes.Black, 2), startPoint, endPoint);
        }

        #endregion

        #region Nested Types

        /// <summary>
        /// Specifies the insertion position for dragged element.
        /// </summary>
        public enum InsertionPosition
        {
            /// <summary>
            /// Top insertion position.
            /// </summary>
            Top,

            /// <summary>
            /// Bottom insertion position.
            /// </summary>
            Bottom
        }

        #endregion
    }
}
