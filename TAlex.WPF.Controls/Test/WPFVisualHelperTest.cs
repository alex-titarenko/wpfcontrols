using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xaml;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

using TAlex.WPF.Helpers;


namespace TAlex.WPF.Test
{
    [TestClass]
    public class WPFVisualHelperTest
    {
        [TestMethod]
        public void FindAncestor()
        {
            FrameworkElement visualTree = XamlServices.Parse(@"
                <Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
		            <StackPanel Orientation=""Horizontal"">
			            <Image x:Name=""itemImage"" />
			            <TextBlock Text=""Some Text"" />
		            </StackPanel>
                </Grid>
                ") as FrameworkElement;

            DependencyObject foundElement = WPFVisualHelper.FindAncestor<Grid>(visualTree.FindName("itemImage") as DependencyObject);
            DependencyObject notFoundElement = WPFVisualHelper.FindAncestor<WrapPanel>(visualTree.FindName("itemImage") as DependencyObject);
            
            Assert.IsNotNull(foundElement);
            Assert.IsNull(notFoundElement);
        }

        [TestMethod]
        public void FindAncestor_NotVisualElement()
        {
            DependencyObject visualTree = XamlServices.Parse(@"
                <Hyperlink xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
		            <Span>Some Text</Span>
                </Hyperlink>
                ") as DependencyObject;

            DependencyObject item = WPFVisualHelper.FindAncestor<TextBlock>(visualTree);
            Assert.IsNull(item);
        }
    }
}
