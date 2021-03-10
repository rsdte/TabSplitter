using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp3.Controls
{
    public class SwitchButton : RadioButton
    {
        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }
        public SolidColorBrush SelectedBackground
        {
            get { return (SolidColorBrush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register("SelectedBackground", typeof(SolidColorBrush), typeof(SwitchButton), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

    }
}
