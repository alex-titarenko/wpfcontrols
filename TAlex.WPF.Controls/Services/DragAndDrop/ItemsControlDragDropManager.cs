using System;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using TAlex.WPF.Helpers;


namespace TAlex.WPF.Services.DragAndDrop
{
    /// <summary>
    /// Provides the ability to drag items for target items control.
    /// </summary>
    public class ItemsControlDragDropManager
    {
        #region Fields

        private const double ScrollDataBound = 4.0;


        private ItemsControl _targetElement;

        private AdornerLayer _adornerLayer;
        
        private DragAdorner _draggedAdorner;

        private InsertionAdorner _insertionAdorner;

        private double _dragAdornerOpacity = 0.6;


        private static Point _startPoint;
        private static Point _itemStartPoint;

        private static UIElement _draggedElement;

        private static ItemsControl _sourceItemsControl;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value that indicates whether a drag operation is currently.
        /// </summary>
        public bool IsDragInProgress
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the target items control for which realized the possibility of dragging items.
        /// </summary>
        public ItemsControl TargetElement
        {
            get
            {
                return _targetElement;
            }

            set
            {
                if (IsDragInProgress)
                {
                    throw new InvalidOperationException("Cannot set the ItemsContorl property during a drag operation.");
                }

                if (_targetElement != null)
                {
                    _targetElement.DragEnter -= itemsControl_DragEnter;
                    _targetElement.DragOver -= itemsControl_DragOver;
                    _targetElement.DragLeave -= itemsControl_DragLeave;
                    _targetElement.Drop -= itemsControl_Drop;
                    _targetElement.PreviewMouseLeftButtonDown -= itemsControl_PreviewMouseLeftButtonDown;
                    _targetElement.MouseMove -= itemsControl_MouseMove;
                }

                _targetElement = value;

                if (_targetElement != null)
                {
                    if (!_targetElement.AllowDrop)
                    {
                        _targetElement.AllowDrop = true;
                    }

                    _targetElement.DragEnter += itemsControl_DragEnter;
                    _targetElement.DragOver += itemsControl_DragOver;
                    _targetElement.DragLeave += itemsControl_DragLeave;
                    _targetElement.Drop += itemsControl_Drop;
                    _targetElement.PreviewMouseLeftButtonDown += itemsControl_PreviewMouseLeftButtonDown;
                    _targetElement.MouseMove += itemsControl_MouseMove;

                    if (String.IsNullOrEmpty(DataFormat))
                    {
                        DataFormat = Guid.NewGuid().ToString();
                    }
                }

                _adornerLayer = null;
            }
        }


        /// <summary>
        /// Gets or sets opacity value for dragging element.
        /// </summary>
        public double DragAdornerOpacity
        {
            get
            {
                return _dragAdornerOpacity;
            }

            set
            {
                if (IsDragInProgress)
                    throw new InvalidOperationException("Cannot set the DragAdornerOpacity property during a drag operation.");

                if (value < 0.0 || value > 1.0)
                    throw new ArgumentOutOfRangeException("DragAdornerOpacity", value, "Must be between 0 and 1.");

                _dragAdornerOpacity = value;
            }
        }

        /// <summary>
        /// Gets or sets a string representing data format for target items control.
        /// Initial value is randomly, presenting a guid identifier.
        /// </summary>
        public string DataFormat { get; set; }

        private AdornerLayer AdornerLayer
        {
            get
            {
                if (_adornerLayer == null)
                {
                    _adornerLayer = AdornerLayer.GetAdornerLayer(_targetElement);
                }

                return _adornerLayer;
            }
        }

        private IList GetItems(ItemsControl targetElement)
        {
            if (targetElement != null)
            {
                if (targetElement.ItemsSource != null)
                {
                    if (targetElement.ItemsSource is ICollectionView)
                    {
                        var collectionView = ((ICollectionView)targetElement.ItemsSource);
                        if (collectionView.SourceCollection is IList)
                        {
                            if (((IList)collectionView.SourceCollection).Count != (collectionView).Count())
                            {
                                return null;
                            }
                            return collectionView.SourceCollection as IList;
                        }
                        return null;
                    }
                    return targetElement.ItemsSource as IList;
                }
                else
                {
                    return targetElement.Items;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Services.DragAndDrop.ItemsControlDragDropManager"/> class.
        /// </summary>
        public ItemsControlDragDropManager()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.Services.DragAndDrop.ItemsControlDragDropManager"/> class.
        /// </summary>
        /// <param name="targetElement">The target items control for which the drag operation is implemented.</param>
        public ItemsControlDragDropManager(ItemsControl targetElement)
            : this()
        {
            TargetElement = targetElement;
        }

        #endregion

        #region Methods

        #region Event Handlers

        private void itemsControl_DragEnter(object sender, DragEventArgs e)
        {
            if (CanDragItem(e.Data, sender))
            {
                e.Effects = DragDropEffects.Move;
                UpdateDraggedAdorner(e.GetPosition(TargetElement));
            }
        }

        private void itemsControl_DragOver(object sender, DragEventArgs e)
        {
            if (CanDragItem(e.Data, sender))
            {
                Point currentPosition = e.GetPosition(TargetElement);
                DependencyObject origSource = (DependencyObject)e.OriginalSource;

                UpdateDraggedAdorner(currentPosition);
                UpdateInsertionAdorner(currentPosition, origSource);

                AutoScrollToItem(currentPosition, Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>(origSource));
            }
        }

        private void itemsControl_DragLeave(object sender, DragEventArgs e)
        {
            if (!IsMouseOver(TargetElement))
            {
                RemoveAdorners();
            }
            e.Handled = true;
        }

        private void itemsControl_Drop(object sender, DragEventArgs e)
        {
            RemoveAdorners();

            if (CanDragItem(e.Data, sender))
            {
                object dragedObject = e.Data.GetData(DataFormat);
                ItemsControl targetItemsControl = sender as ItemsControl;

                IList sourceList = GetItems(_sourceItemsControl);
                IList targetList = GetItems(targetItemsControl);

                lock (targetItemsControl)
                {
                    ListBoxItem listViewItem = Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                    if (listViewItem != null || targetList.Count == 0)
                    {
                        if (listViewItem == null)
                            listViewItem = new ListBoxItem();

                        // Find the data behind the ListViewItem
                        object newDataObject = (listViewItem != null) ? listViewItem.DataContext ?? listViewItem : null;

                        if (dragedObject == newDataObject)
                        {
                            IsDragInProgress = false;
                            return;
                        }

                        Point pos = e.GetPosition(listViewItem);
                        double halfHeight = listViewItem.ActualHeight / 2;
                        
                        if (targetList != null)
                        {
                            int oldIndex = sourceList.IndexOf(dragedObject);
                            int newIndex = targetList.IndexOf(newDataObject);

                            if (newIndex > oldIndex && _sourceItemsControl == targetItemsControl) newIndex--;
                            if (pos.Y > halfHeight) newIndex++;

                            // Coerce new index
                            newIndex = Math.Max(0, Math.Min(targetList.Count, newIndex));

                            if (oldIndex != newIndex || sourceList != targetList)
                            {
                                sourceList.RemoveAt(oldIndex);
                                targetList.Insert(newIndex, dragedObject);
                            }

                            // Refresh source and target items controls
                            if (_sourceItemsControl != targetItemsControl)
                            {
                                if (!(sourceList is ItemCollection) && !(sourceList is INotifyCollectionChanged))
                                {
                                    _sourceItemsControl.Items.Refresh();
                                }
                            }
                            if (!(targetList is ItemCollection) && !(targetList is INotifyCollectionChanged))
                            {
                                targetItemsControl.Items.Refresh();
                            }
                        }
                    }
                }

                IsDragInProgress = false;
            }
        }

        private void itemsControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Store the mouse position
            _startPoint = e.GetPosition(TargetElement);

            _sourceItemsControl = (ItemsControl)sender;
            _draggedElement = Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
            if (_draggedElement != null)
                _itemStartPoint = e.GetPosition(_draggedElement);
        }

        private void itemsControl_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListBoxItem listViewItem = Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);

                if (listViewItem != null)
                {
                    // Find the data behind the ListViewItem
                    object data = listViewItem.DataContext ?? listViewItem;

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject(DataFormat, data);
                    DragDrop.DoDragDrop(sender as DependencyObject, dragData, DragDropEffects.Move);
                }
            }
        }

        #endregion

        private void UpdateDraggedAdorner(Point currentPosition)
        {
            if (_draggedElement != null)
            {
                if (_draggedAdorner == null)
                {
                    _draggedAdorner = new DragAdorner(TargetElement, _draggedElement);
                    _draggedAdorner.Opacity = _dragAdornerOpacity;
                    AdornerLayer.Add(_draggedAdorner);
                }

                double left = currentPosition.X - _itemStartPoint.X;
                double top = currentPosition.Y - _itemStartPoint.Y;
                _draggedAdorner.SetPosition(left, top);
            }
            else
            {
                RemoveAdorners();
            }
        }

        private void UpdateInsertionAdorner(Point currentPosition, DependencyObject obj)
        {
            FrameworkElement listViewItem = Helpers.WPFVisualHelper.FindAncestor<ListBoxItem>(obj);

            if (listViewItem != null)
            {
                if (_insertionAdorner == null)
                {
                    _insertionAdorner = new InsertionAdorner(listViewItem);
                    AdornerLayer.Add(_insertionAdorner);
                }

                currentPosition = TargetElement.TranslatePoint(currentPosition, listViewItem);
                double halfHeight = listViewItem.ActualHeight / 2;

                _insertionAdorner.Position = (currentPosition.Y <= halfHeight) ?
                        InsertionAdorner.InsertionPosition.Top : InsertionAdorner.InsertionPosition.Bottom;
            }
            else
            {
                RemoveAdorners();
            }
        }

        private void RemoveAdorners()
        {
            if (_draggedAdorner != null)
                AdornerLayer.Remove(_draggedAdorner);
            _draggedAdorner = null;

            if (_insertionAdorner != null)
                AdornerLayer.Remove(_insertionAdorner);
            _insertionAdorner = null;
        }

        private bool IsMouseOver(Visual target)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(target);
            Point mousePos = Mouse.GetPosition(target as IInputElement);
            return bounds.Contains(mousePos);
        }

        private void AutoScrollToItem(Point currentPosition, FrameworkElement targetItem)
        {
            IList items = GetItems(TargetElement);
            if (TargetElement is ListBox && items != null && items.Count > 0)
            {
                ListBox listBox = TargetElement as ListBox;

                if (targetItem != null)
                {
                    FrameworkElement itemsPanel = VisualTreeHelper.GetParent(targetItem) as FrameworkElement;
                    Point currentPositionRelativeItemsPanel = TargetElement.TranslatePoint(currentPosition, itemsPanel);

                    object item = targetItem.DataContext;
                    if (item == null) item = targetItem;

                    int currentIndex = items.IndexOf(item);

                    if (currentPositionRelativeItemsPanel.Y < ScrollDataBound)
                    {
                        listBox.ScrollIntoView(items[Math.Max(0, currentIndex - 1)]);
                    }
                    else if (currentPositionRelativeItemsPanel.Y >= itemsPanel.ActualHeight - ScrollDataBound)
                    {
                        listBox.ScrollIntoView(items[Math.Min(items.Count - 1, currentIndex + 1)]);
                    }
                }
            }
        }

        private bool CanDragItem(IDataObject data, object sender)
        {
            bool canDrag = data.GetDataPresent(DataFormat) && GetItems((ItemsControl)sender) != null;
            if (!canDrag) DragDrop.DoDragDrop(null, null, DragDropEffects.None);

            return canDrag;
        }

        #endregion
    }
}
