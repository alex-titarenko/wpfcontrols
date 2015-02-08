using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;


namespace TAlex.WPF.Services.PushBinding
{
    /// <remarks>
    /// Based on code from http://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/
    /// </remarks>
    public class PushBindingManager
    {
        #region Fields

        public static DependencyProperty PushBindingsProperty;

        #endregion

        #region Constructors

        static PushBindingManager()
        {
            PushBindingsProperty = DependencyProperty.RegisterAttached("PushBindingsInternal", typeof(PushBindingCollection), typeof(PushBindingManager), new UIPropertyMetadata(null));
        }

        #endregion

        #region Methods

        public static PushBindingCollection GetPushBindings(FrameworkElement obj)
        {
            if (obj.GetValue(PushBindingsProperty) == null)
            {
                obj.SetValue(PushBindingsProperty, new PushBindingCollection(obj));
            }
            return (PushBindingCollection)obj.GetValue(PushBindingsProperty);
        }

        public static void SetPushBindings(FrameworkElement obj, PushBindingCollection value)
        {
            obj.SetValue(PushBindingsProperty, value);
        }

        #endregion
    }
}
