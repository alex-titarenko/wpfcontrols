using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Specialized;


namespace TAlex.WPF.Services.PushBinding
{
    /// <remarks>
    /// Based on code from http://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/
    /// </remarks>
    public class PushBindingCollection : FreezableCollection<PushBinding>
    {
        #region Properties

        public FrameworkElement TargetObject
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        public PushBindingCollection(FrameworkElement targetObject)
        {
            TargetObject = targetObject;
            ((INotifyCollectionChanged)this).CollectionChanged += CollectionChanged;
        }

        #endregion

        #region Methods

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (PushBinding pushBinding in e.NewItems)
                {
                    pushBinding.SetupTargetBinding(TargetObject);
                }
            }
        }

        #endregion
    }
}
