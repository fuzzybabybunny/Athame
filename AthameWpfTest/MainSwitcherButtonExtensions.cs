using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AthameWPF
{
    public class MainSwitcherButtonExtensions : DependencyObject
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached(
            "Image", typeof(ImageSource), typeof(MainSwitcherButtonExtensions), new PropertyMetadata(default(ImageSource)));

        public static ImageSource GetImage(UIElement element)
        {
            return (ImageSource) element.GetValue(ImageProperty);
        }

        public static void SetImage(UIElement element, ImageSource value)
        {
            element.SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof(string), typeof(MainSwitcherButtonExtensions), new PropertyMetadata(default(string)));


        public static string GetText(UIElement element)
        {
            return (string) element.GetValue(TextProperty);
        }

        public static void SetText(UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }
    }
}
