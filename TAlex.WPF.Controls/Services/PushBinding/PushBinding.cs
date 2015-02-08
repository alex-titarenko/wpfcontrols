using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;


namespace TAlex.WPF.Services.PushBinding
{
    /// <summary>
    /// Helper for OneWayToSource binding for read-only dependency properties.
    /// </summary>
    /// <remarks>
    /// Based on code from http://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/
    /// </remarks>
    public class PushBinding : FreezableBinding
    {
        #region Fields

        public static DependencyProperty TargetPropertyMirrorProperty;
        public static DependencyProperty TargetPropertyListenerProperty;

        #endregion

        #region Properties

        public object TargetPropertyMirror
        {
            get { return GetValue(TargetPropertyMirrorProperty); }
            set { SetValue(TargetPropertyMirrorProperty, value); }
        }

        public object TargetPropertyListener
        {
            get { return GetValue(TargetPropertyListenerProperty); }
            set { SetValue(TargetPropertyListenerProperty, value); }
        }

        [DefaultValue(null)]
        public string TargetProperty
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        static PushBinding()
        {
            TargetPropertyMirrorProperty = DependencyProperty.Register("TargetPropertyMirror", typeof(object), typeof(PushBinding));
            TargetPropertyListenerProperty = DependencyProperty.Register("TargetPropertyListener", typeof(object), typeof(PushBinding), new UIPropertyMetadata(null, OnTargetPropertyListenerChanged));
        }

        public PushBinding()
        {
            Mode = BindingMode.OneWayToSource;
        }

        #endregion

        #region Methods

        public void SetupTargetBinding(FrameworkElement targetObject)
        {
            if (targetObject == null)
            {
                return;
            }

            if (DesignerProperties.GetIsInDesignMode(this) == true)
                return;

            Binding listenerBinding = new Binding
            {
                Source = targetObject,
                Path = new PropertyPath(TargetProperty),
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);

            BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);
            TargetPropertyValueChanged();
        }

        private static void OnTargetPropertyListenerChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PushBinding pushBinding = sender as PushBinding;
            pushBinding.TargetPropertyValueChanged();
        }

        private void TargetPropertyValueChanged()
        {
            object targetPropertyValue = GetValue(TargetPropertyListenerProperty);
            this.SetValue(TargetPropertyMirrorProperty, targetPropertyValue);
        }

        #endregion
    }
}
