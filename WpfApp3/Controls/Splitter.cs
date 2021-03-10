using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WpfApp3.Controls
{
    public class Splitter : Control
    {
        private Style verticalStyle;
        private Style horizontalStyle;

        private Grid partPanel;
        private SwitchButtonGroup partGroup;
        private Thumb partThumb;
        private StackPanel partRight;
        private RadioButton partVerticalModeButton;
        private RadioButton partHorizontalModeButton;
        private CheckBox partToDownButton;

        public delegate void DragDelta(object sender, DragDeltaEventArgs e);
        public event DragDelta DragDeltaEvent;

        public delegate void UpCheched(object sender, RoutedEventArgs e);
        public event UpCheched OnUpCheched;

        public delegate void DownCheched(object sender, RoutedEventArgs e);
        public event DownCheched OnDownCheched;

        public delegate void SwitchButtonClick(object sender, RoutedEventArgs e);
        public event SwitchButtonClick OnSwitchButtonClick;

        public delegate void SwitchChanged(object sender, SwitchEventArgs e);
        public event SwitchChanged OnSwitchChanged;

        public delegate void HorizontalModeEvent(object sender, RoutedEventArgs e);
        public event HorizontalModeEvent OnHorizontalMode;

        public delegate void VerticalModeEvent(object sender, RoutedEventArgs e);
        public event VerticalModeEvent OnVerticalMode;

        public delegate void ToDown(object sender, RoutedEventArgs e);
        public event ToDown OnToDown;

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(Direction), typeof(Splitter), new PropertyMetadata(Direction.Horizontal, OnDirectionChanged));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate(); 
            partPanel = GetTemplateChild("PART_Panel") as Grid;
            partGroup = GetTemplateChild("PART_SwitchButtonGroup") as SwitchButtonGroup;
            partThumb = GetTemplateChild("PART_Thumb") as Thumb;
            partRight = GetTemplateChild("PART_Right") as StackPanel;
            partHorizontalModeButton = GetTemplateChild("PART_HorizontalMode") as RadioButton;
            partVerticalModeButton = GetTemplateChild("PART_VerticalMode") as RadioButton;
            partToDownButton = GetTemplateChild("PART_ToDown") as CheckBox;
            partThumb.DragDelta += OnPartThumbDragDelta;
            partGroup.OnUpCheched += OnSplitterUpChecked;
            partGroup.OnDownCheched += OnSplitterDownChecked;
            partGroup.OnSwitchButtonClick += OnSplitterSwitchButtonClick;
            partGroup.OnSwitchChanged += OnSplitterSwitchChanged;
            partVerticalModeButton.Checked += PartVerticalModeButton_Checked;
            partHorizontalModeButton.Checked += PartHorizontalModeButton_Checked;
            partToDownButton.Checked += PartToDownButton_Checked;
            if (verticalStyle is null) {
                verticalStyle = FindResource("VerticalSplitterStyle") as Style;
                horizontalStyle = FindResource("HorizontalSplitterStyle") as Style;
               
                ChangedMode();
            }
        }

        private void PartToDownButton_Checked(object sender, RoutedEventArgs e)
        {
            e.Source = this;
            OnToDown?.Invoke(this, e);
        }

        private void PartHorizontalModeButton_Checked(object sender, RoutedEventArgs e)
        {
            e.Source = this;
            OnHorizontalMode?.Invoke(this, e);
        }

        private void PartVerticalModeButton_Checked(object sender, RoutedEventArgs e)
        {
            e.Source = this;
            OnVerticalMode?.Invoke(this, e);
        }

        private void OnSplitterUpChecked(object sender, RoutedEventArgs args)
        {
            args.Source = this;
            OnUpCheched?.Invoke(this, args);
        }

        private void OnSplitterDownChecked(object sender, RoutedEventArgs args)
        {
            args.Source = this;
            OnDownCheched?.Invoke(this, args);
        }

        private void OnSplitterSwitchButtonClick(object sender, RoutedEventArgs args)
        {
            args.Source = this;
            OnSwitchButtonClick?.Invoke(this, args);
        }

        private void OnSplitterSwitchChanged(object sender, SwitchEventArgs args)
        {
            OnSwitchChanged?.Invoke(this, args);
        }

        private void OnPartThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            e.Source = this;
            DragDeltaEvent?.Invoke(this, e);
        }

        public void ChangedMode()
        {
            if (Direction == Direction.Horizontal && Style != horizontalStyle) {
                Style = horizontalStyle;
                partThumb.Cursor = Cursors.SizeNS;
            } else if (Direction == Direction.Vertical && Style != verticalStyle) {
                Style = verticalStyle;
                partThumb.Cursor = Cursors.SizeWE;
            }
        }

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is Splitter s) {
                s.ChangedMode();
            }
        }

        static Splitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Splitter), new FrameworkPropertyMetadata(typeof(Splitter)));
        }
    }
}
