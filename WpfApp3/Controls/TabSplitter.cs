using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3.Controls
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }
    public class TabSplitter : Control
    {
        private Style herizontalStyle;
        private Style verticalStyle;
        private Grid partPanel;
        private ContentControl topControl;
        private ContentControl downControl;
        private Splitter splitter;

        static TabSplitter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabSplitter), new FrameworkPropertyMetadata(typeof(TabSplitter)));
        }

        public object TopContent
        {
            get { return (object)GetValue(TopContentProperty); }
            set { SetValue(TopContentProperty, value); }
        }

        public object DownContent
        {
            get { return (object)GetValue(DownContentProperty); }
            set { SetValue(DownContentProperty, value); }
        }

        public Direction Direction
        {
            get { return (Direction)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }


        public void ChangedMode()
        {
            if (verticalStyle is null)
                return;
            if (Direction == Direction.Horizontal && Style != herizontalStyle) {
                Style = herizontalStyle;
            } 
            else if (Direction == Direction.Vertical && Style != verticalStyle) {
                Style = verticalStyle;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (TopContent is null) {
                TopContent = new Grid { Background = Brushes.LightPink };
            }
            if (DownContent is null) {
                DownContent = new Grid { Background = Brushes.LightSalmon };
            }
            partPanel = GetTemplateChild("PART_Panel") as Grid;
            topControl = GetTemplateChild("PART_Topcontent") as ContentControl;
            downControl = GetTemplateChild("PART_Downcontent") as ContentControl;
            splitter = GetTemplateChild("PART_Splitter") as Splitter;
            splitter.DragDeltaEvent += OnDragDelta;
            splitter.OnDownCheched += OnDownCheched;
            splitter.OnSwitchChanged += OnSwitchChanged;
            splitter.OnUpCheched += OnUpCheched;
            splitter.OnToDown += OnToDown;
            splitter.OnHorizontalMode += OnHorizontalMode;
            splitter.OnVerticalMode += OnVerticalMode;

            if (verticalStyle is null) {
                verticalStyle = this.FindResource("VerticalTabSplitterStyle") as Style;
                herizontalStyle = this.FindResource("HorizontalTabSplitterStyle") as Style;
                ChangedMode();
            }
        }
        private void OnDragDelta(object sender, DragDeltaEventArgs e)
        {
            // changed > 0 右移/下移
            var obj = sender as Splitter;
            if (Direction == Direction.Horizontal) {
                var row1 = partPanel.RowDefinitions[0];
                var height = GetRowDefinitionActualHeight(row1);
                var actualHeight = height + e.VerticalChange;
                if (height + obj.ActualHeight >= this.ActualHeight && e.VerticalChange > 0) {
                    actualHeight = this.ActualHeight - obj.ActualHeight;
                }
                if (actualHeight < 0)
                    actualHeight = 0;
                row1.Height = new GridLength(actualHeight);
                return;
            }
            var colAbove = partPanel.ColumnDefinitions[0];
            var width = GetColumnDefinitionActualWidth(colAbove);
            var actualWidth = width + e.HorizontalChange;
            if (width + obj.ActualWidth >= this.ActualWidth && e.HorizontalChange > 0) {
                actualWidth = this.ActualWidth - obj.ActualWidth;
            }
            if (actualWidth < 0)
                actualWidth = 0;
            colAbove.Width = new GridLength(actualWidth);
            return;
        }
        private void OnVerticalMode(object sender, RoutedEventArgs e)
        {
            Direction = Direction.Vertical;
        }

        private void OnHorizontalMode(object sender, RoutedEventArgs e)
        {
            Direction = Direction.Horizontal;
        }

        private void OnToDown(object sender, RoutedEventArgs e)
        {
        }

        private void OnUpCheched(object sender, RoutedEventArgs e)
        {
            topControl?.Focus();
        }

        private void OnSwitchChanged(object sender, SwitchEventArgs e)
        {
            if (Direction == Direction.Horizontal) {
                var ret = Grid.GetRow(topControl);
                if (ret == 0) {
                    Grid.SetColumn(downControl, 0);
                    Grid.SetColumnSpan(downControl, 3);
                    Grid.SetRowSpan(downControl, 1);
                    Grid.SetRow(downControl, 0);

                    Grid.SetColumn(topControl, 0);
                    Grid.SetColumnSpan(topControl, 3);
                    Grid.SetRowSpan(topControl, 1);
                    Grid.SetRow(topControl, 2);
                } else {
                    Grid.SetColumn(topControl, 0);
                    Grid.SetColumnSpan(topControl, 3);
                    Grid.SetRowSpan(topControl, 1);
                    Grid.SetRow(topControl, 0);

                    Grid.SetColumn(downControl, 0);
                    Grid.SetColumnSpan(downControl, 3);
                    Grid.SetRowSpan(topControl, 1);
                    Grid.SetRow(downControl, 2);
                }
            } else {
                if (Grid.GetColumn(topControl) == 0) {
                    Grid.SetColumn(downControl, 0);
                    Grid.SetColumnSpan(downControl, 1);
                    Grid.SetRowSpan(downControl, 3);
                    Grid.SetRow(downControl, 0);

                    Grid.SetColumn(topControl, 2);
                    Grid.SetColumnSpan(topControl, 1);
                    Grid.SetRowSpan(topControl, 3);
                    Grid.SetRow(topControl, 0);
                } else {
                    Grid.SetColumn(topControl, 0);
                    Grid.SetColumnSpan(topControl, 1);
                    Grid.SetRowSpan(topControl, 3);
                    Grid.SetRow(topControl, 0);

                    Grid.SetColumn(downControl, 2);
                    Grid.SetColumnSpan(downControl, 1);
                    Grid.SetRowSpan(downControl, 3);
                    Grid.SetRow(downControl, 0);
                }
            }
        }

        private void OnDownCheched(object sender, RoutedEventArgs e)
        {
            downControl?.Focus();
        }

        /// <summary>
        /// 获取 grid 控件的某一行的实际高度
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private double GetRowDefinitionActualHeight(RowDefinition row)
        {
            if (row.Height.IsAbsolute)
                return row.Height.Value;
            if (RowDefinitionActualHeightProperty is null)
                RowDefinitionActualHeightProperty = row.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualHeight");
            return (double)RowDefinitionActualHeightProperty.GetValue(row);
        }

        /// <summary>
        /// 获取 grid 控件的某一行的实际宽度
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private double GetColumnDefinitionActualWidth(ColumnDefinition column)
        {
            if (column.Width.IsAbsolute)
                return column.Width.Value;
            if (ColumnDefinitionActualWidthProperty == null)
                ColumnDefinitionActualWidthProperty = column.GetType().GetRuntimeProperties().First((p) => p.Name == "ActualWidth");
            return (double)ColumnDefinitionActualWidthProperty.GetValue(column);
        }


        private static PropertyInfo RowDefinitionActualHeightProperty;
        private static PropertyInfo ColumnDefinitionActualWidthProperty;
        
        public static readonly DependencyProperty DownContentProperty= DependencyProperty.Register("TopContent", typeof(object), typeof(TabSplitter), new PropertyMetadata(null));
        public static readonly DependencyProperty TopContentProperty = DependencyProperty.Register("DownContent", typeof(object), typeof(TabSplitter), new PropertyMetadata(null));
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction", typeof(Direction), typeof(TabSplitter), new PropertyMetadata(Direction.Horizontal, OnDirectionChanged));

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabSplitter splitter) {
                splitter.ChangedMode();
            }
        }
    }
}
