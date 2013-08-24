using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Data;

namespace TAlex.WPF.Services.PushBinding
{
    public class PushBinding : FreezableBinding
    {
        #region Dependency Properties

        public static DependencyProperty TargetPropertyMirrorProperty =
            DependencyProperty.Register("TargetPropertyMirror",
                                        typeof(object),
                                        typeof(PushBinding));
        public static DependencyProperty TargetPropertyListenerProperty =
            DependencyProperty.Register("TargetPropertyListener",
                                        typeof(object),
                                        typeof(PushBinding),
                                        new UIPropertyMetadata(null, OnTargetPropertyListenerChanged));

        private static void OnTargetPropertyListenerChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PushBinding pushBinding = sender as PushBinding;
            pushBinding.TargetPropertyValueChanged();
        }

        #endregion // Dependency Properties

        #region Constructor

        public PushBinding()
        {
            Mode = BindingMode.OneWayToSource;
        }

        #endregion // Constructor

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

        #endregion // Properties

        public void SetupTargetBinding(FrameworkElement targetObject)
        {
            if (targetObject == null)
            {
                return;
            }
            
            // Prevent the designer from reporting exceptions since
            // changes will be made of a Binding in use if it is set
            if (DesignerProperties.GetIsInDesignMode(this) == true)
                return;

            // Bind to the selected TargetProperty, e.g. ActualHeight and get
            // notified about changes in OnTargetPropertyListenerChanged
            Binding listenerBinding = new Binding
            {
                Source = targetObject,
                Path = new PropertyPath(TargetProperty),
                Mode = BindingMode.OneWay
            };
            BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);

            // Set up a OneWayToSource Binding with the Binding declared in Xaml from
            // the Mirror property of this class. The mirror property will be updated
            // everytime the Listener property gets updated
            BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);
            TargetPropertyValueChanged();
        }

        private void TargetPropertyValueChanged()
        {
            object targetPropertyValue = GetValue(TargetPropertyListenerProperty);
            this.SetValue(TargetPropertyMirrorProperty, targetPropertyValue);
        }
    }
}
