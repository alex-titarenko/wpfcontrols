using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Xaml;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using TAlex.WPF.Helpers;
using NUnit.Framework;


namespace TAlex.WPF.Controls.Tests.Helpers
{
    [TestFixture]
    public class WPFVisualHelperTests
    {
        #region FindAncestor

        [Test]
        [STAThread]
        public void FindAncestor_SomeDependencyObject_Ancestor()
        {
            //arrange
            FrameworkElement visualTree = XamlServices.Parse(@"
                <Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
		            <StackPanel Orientation=""Horizontal"">
			            <Image x:Name=""itemImage"" />
			            <TextBlock Text=""Some Text"" />
		            </StackPanel>
                </Grid>
                ") as FrameworkElement;

            //action
            DependencyObject foundElement = WPFVisualHelper.FindAncestor<Grid>(visualTree.FindName("itemImage") as DependencyObject);
            
            //assert
            Assert.IsNotNull(foundElement);
        }

        [Test]
        [STAThread]
        public void FindAncestor_FindNotExistingAncestor_Null()
        {
            //arrange
            FrameworkElement visualTree = XamlServices.Parse(@"
                <Grid xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
		            <StackPanel Orientation=""Horizontal"">
			            <Image x:Name=""itemImage"" />
			            <TextBlock Text=""Some Text"" />
		            </StackPanel>
                </Grid>
                ") as FrameworkElement;

            //action
            DependencyObject notFoundElement = WPFVisualHelper.FindAncestor<WrapPanel>(visualTree.FindName("itemImage") as DependencyObject);

            //assert
            Assert.IsNull(notFoundElement);
        }

        [Test]
        public void FindAncestor_NotVisualElement_Null()
        {
            //arrange
            DependencyObject visualTree = XamlServices.Parse(@"
                <Hyperlink xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
		            <Span>Some Text</Span>
                </Hyperlink>
                ") as DependencyObject;

            //action
            DependencyObject item = WPFVisualHelper.FindAncestor<TextBlock>(visualTree);

            //assert
            Assert.IsNull(item);
        }

        #endregion
    }
}
