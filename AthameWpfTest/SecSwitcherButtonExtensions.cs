using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AthameWPF
{
    public class SecSwitcherButtonExtensions : DependencyObject
    {
        public static readonly DependencyProperty PathStyleProperty = DependencyProperty.RegisterAttached(
    "PathStyle", typeof(Style), typeof(SecSwitcherButtonExtensions), new PropertyMetadata(default(Style)));

        public static Style GetPathStyle(UIElement element)
        {
            return (Style)element.GetValue(PathStyleProperty);
        }

        public static void SetPathStyle(UIElement element, Style value)
        {
            element.SetValue(PathStyleProperty, value);
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text", typeof(string), typeof(SecSwitcherButtonExtensions), new PropertyMetadata(default(string)));


        public static string GetText(UIElement element)
        {
            
            return (string)element.GetValue(TextProperty);
        }

        public static void SetText(UIElement element, string value)
        {
            element.SetValue(TextProperty, value);
        }
    }
}
