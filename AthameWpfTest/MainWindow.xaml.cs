using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AthameWpfTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double ScreenSizeFillPercent = 0.6;

        public MainWindow()
        {
            InitializeComponent();
            var workArea = SystemParameters.WorkArea;
            Width = (workArea.Width * ScreenSizeFillPercent);
            Height = (workArea.Height * ScreenSizeFillPercent);
        }

        #region ' Main Switcher Anims '
        
        private const int SwitcherAnimTimeMs = 200;
        private RadioButton selectedMainRadioButton;

        private double GetWidthIncludingMargin(FrameworkElement element)
        {
            return element.ActualWidth + element.Margin.Left + element.Margin.Right;
        }

        private void SwitcherButtonOnChecked(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton) sender;
            selectedMainRadioButton = radioButton;
            LineUpIndicatorToButton(true);
        }

        private void LineUpIndicatorToButton(bool doAnimation)
        {
            if (MainSwitcherSelectionIndicator == null || selectedMainRadioButton == null) return;
            var widthIncludingMargin = GetWidthIncludingMargin(selectedMainRadioButton);
            var parent = VisualTreeHelper.GetParent(selectedMainRadioButton) as FrameworkElement;
            // Left margin = X position of selected radio button relative to parent
            var leftMargin = selectedMainRadioButton.TranslatePoint(new Point(0, 0), parent).X;

            // Before we animated the indicator's margin, we need to align it to the radio button's hori alignment
            // Before we do that, however, we need to calculate the empty space in the parent to the left and right of the indicator,
            // so that when we set the alignment it doesn't align itself before we begin the transition
            var relLeft = MainSwitcherSelectionIndicator.TranslatePoint(new Point(0, 0), parent).X;
            var preparedMargin = new Thickness(relLeft, 0, parent.ActualWidth - (relLeft + MainSwitcherSelectionIndicator.Width), 0);
            MainSwitcherSelectionIndicator.Margin = preparedMargin;
            MainSwitcherSelectionIndicator.HorizontalAlignment = selectedMainRadioButton.HorizontalAlignment;
            
            // Calculate the new margin to animate to.
            var rightMargin = parent.ActualWidth - (leftMargin + MainSwitcherSelectionIndicator.Width);
            var newMargin = new Thickness(leftMargin, 0, rightMargin, 0);

            if (doAnimation)
            {
                var widthAnimation = new DoubleAnimation(widthIncludingMargin,
                    TimeSpan.FromMilliseconds(SwitcherAnimTimeMs))
                {
                    EasingFunction = new CubicEase()
                };
                
                var marginAnimation = new ThicknessAnimation(newMargin,
                    TimeSpan.FromMilliseconds(SwitcherAnimTimeMs), FillBehavior.Stop)
                {
                    EasingFunction = new CubicEase()
                };
                marginAnimation.Completed += (sender, args) =>
                {
                    
                    ResetIndicatorMargin(newMargin);
                };
                MainSwitcherSelectionIndicator.BeginAnimation(WidthProperty, widthAnimation);
                MainSwitcherSelectionIndicator.BeginAnimation(MarginProperty, marginAnimation);
                
            }
            else
            {
                MainSwitcherSelectionIndicator.Width = widthIncludingMargin;
                MainSwitcherSelectionIndicator.Margin = newMargin;
                ResetIndicatorMargin(newMargin);
            }
        }

        private void ResetIndicatorMargin(Thickness target)
        {
            
            MainSwitcherSelectionIndicator.HorizontalAlignment = selectedMainRadioButton.HorizontalAlignment;
            if (MainSwitcherSelectionIndicator.HorizontalAlignment == HorizontalAlignment.Right)
            {
                MainSwitcherSelectionIndicator.Margin = new Thickness(0, 0,
                    target.Right, 0);
            }
            else
            {
                MainSwitcherSelectionIndicator.Margin = new Thickness(target.Left, 0, 0, 0);
            }
        }

        private void MainSwitcherSelectionIndicator_Loaded(object sender, RoutedEventArgs e)
        {
            LineUpIndicatorToButton(false);
        }
        #endregion

        private void MainSwitcherSettingsButton_OnChecked(object sender, RoutedEventArgs e)
        {
            MainContentFrame?.Navigate(new SettingsPage());
        }

        private void MainSwitcherQueueButton_OnChecked(object sender, RoutedEventArgs e)
        {
            MainContentFrame?.Navigate(new QueuePage());
        }

        private void MainSwitcherSearchButton_OnChecked(object sender, RoutedEventArgs e)
        {
            MainContentFrame?.Navigate(new SearchPage());
        }

        private void MainContentFrame_OnLoaded(object sender, RoutedEventArgs e)
        {
            MainContentFrame.Navigate(new SearchPage());
        }
    }
}
