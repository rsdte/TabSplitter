using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3.Controls
{
    public class SwitchButtonGroup : Control
    {
        private Style verticalSwitchButtonGroupStyle;
        private Style horizontalSwitchButtonGroupStyle;
        private Grid partPanel;
        private SwitchButton upButton;
        private SwitchButton downButton;
        private Button switchButton;

        public delegate void UpCheched(object sender, RoutedEventArgs e);
        public event UpCheched OnUpCheched;

        public delegate void DownCheched(object sender, RoutedEventArgs e);
        public event DownCheched OnDownCheched;

        public delegate void SwitchButtonClick(object sender, RoutedEventArgs e);
        public event SwitchButtonClick OnSwitchButtonClick;

        public delegate void SwitchChanged(object sender, SwitchEventArgs e);
        public event SwitchChanged OnSwitchChanged;

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }


        public void ChangedMode()
        {
            if(Direction == Direction.Horizontal && Style != horizontalSwitchButtonGroupStyle) {
                Style = horizontalSwitchButtonGroupStyle;
            }
            else if(Direction == Direction.Vertical && Style != verticalSwitchButtonGroupStyle) {
                Style = verticalSwitchButtonGroupStyle;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            partPanel = GetTemplateChild("PART_Panel") as Grid;
            upButton = GetTemplateChild("PART_UpButton") as SwitchButton;
            downButton = GetTemplateChild("PART_DownButton") as SwitchButton;
            switchButton = GetTemplateChild("PART_SwitchButton") as Button;
            upButton.AddHandler(RadioButton.CheckedEvent, new RoutedEventHandler(UpRadioChecked));
            downButton.AddHandler(RadioButton.CheckedEvent, new RoutedEventHandler(DownRadioChecked));
            switchButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(ContentSwitchChanged));
            if (verticalSwitchButtonGroupStyle is null) {
                
                verticalSwitchButtonGroupStyle = FindResource("VerticalSwitchButtonGroupStyle") as Style;
                horizontalSwitchButtonGroupStyle = FindResource("HorizontalSwitchButtonGroupStyle") as Style;
                ChangedMode();
            }
        }
        private void UpRadioChecked(object sender, RoutedEventArgs e)
        {
            OnUpCheched?.Invoke(this, e);
        }
        private void DownRadioChecked(object sender, RoutedEventArgs e)
        {
            this.OnDownCheched?.Invoke(this, e);
            InvalidateArrange();
        }
        private void SwitchButtonClicked(object sender, RoutedEventArgs e)
        {
            OnSwitchButtonClick?.Invoke(this, e);
        }

        private void ContentSwitchChanged(object sender, RoutedEventArgs e)
        {
            var args = new SwitchEventArgs() {
                CheckedType = CheckedType.Down,
            };
            if (upButton.IsChecked ?? false)
                args.CheckedType = CheckedType.Up;
            this.OnSwitchChanged?.Invoke(this, args);
        }

        static SwitchButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButtonGroup), new FrameworkPropertyMetadata(typeof(SwitchButtonGroup)));
        }
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(SwitchButtonGroup), new PropertyMetadata(Direction.Horizontal, OnDirectionChanged));

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is SwitchButtonGroup s) {
                s.ChangedMode();
            }
        }



    }
}
