// ResponsivePanel.cs
using System;
using System.Collections.Generic;
using System.Windows;

namespace System.Windows.Controls
{
    public class ResponsivePanel : Panel
    {
        #region Dependency Properties
        
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                nameof(Columns),
                typeof(int),
                typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(12, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(
                nameof(Spacing),
                typeof(double),
                typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(8.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty BreakpointsProperty =
            DependencyProperty.Register(
                nameof(Breakpoints),
                typeof(BreakpointCollection),
                typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(null, OnBreakpointsChanged));

        // Attached property for child column span
        public static readonly DependencyProperty SpanProperty =
            DependencyProperty.RegisterAttached(
                "Span",
                typeof(int),
                typeof(ResponsivePanel),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsMeasure));

        #endregion

        #region Properties

        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            set => SetValue(ColumnsProperty, value);
        }

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public BreakpointCollection Breakpoints
        {
            get => (BreakpointCollection)GetValue(BreakpointsProperty);
            set => SetValue(BreakpointsProperty, value);
        }

        #endregion

        #region Attached Property Accessors

        public static void SetSpan(UIElement element, int value)
            => element.SetValue(SpanProperty, value);

        public static int GetSpan(UIElement element)
            => (int)element.GetValue(SpanProperty);

        #endregion

        private static void OnBreakpointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ResponsivePanel panel)
            {
                panel.UpdateBreakpoint();
                panel.InvalidateMeasure();
            }
        }

        private void UpdateBreakpoint()
        {
            if (Breakpoints == null || Breakpoints.Count == 0)
                return;

            var width = ActualWidth;
            var activeBreakpoint = Breakpoints[0];

            foreach (var breakpoint in Breakpoints)
            {
                if (width >= breakpoint.Width)
                    activeBreakpoint = breakpoint;
            }

            Columns = activeBreakpoint.Columns;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            UpdateBreakpoint();

            var columnWidth = (availableSize.Width - (Spacing * (Columns - 1))) / Columns;
            var currentRow = 0;
            var currentCol = 0;
            var maxHeight = 0.0;
            var rowHeight = 0.0;

            foreach (UIElement child in Children)
            {
                var span = GetSpan(child);
                span = Math.Min(span, Columns); // Ensure span doesn't exceed columns

                // Check if we need to move to next row
                if (currentCol + span > Columns)
                {
                    currentCol = 0;
                    currentRow++;
                    maxHeight += rowHeight + Spacing;
                    rowHeight = 0;
                }

                // Calculate child's available width including spacing
                var childWidth = (columnWidth * span) + (Spacing * (span - 1));
                child.Measure(new Size(childWidth, availableSize.Height));

                rowHeight = Math.Max(rowHeight, child.DesiredSize.Height);
                currentCol += span;
            }

            maxHeight += rowHeight; // Add last row height

            return new Size(availableSize.Width, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var columnWidth = (finalSize.Width - (Spacing * (Columns - 1))) / Columns;
            var currentRow = 0;
            var currentCol = 0;
            var currentY = 0.0;
            var rowHeight = 0.0;

            // First pass: calculate row heights
            var rowHeights = new List<double>();
            var currentRowHeight = 0.0;
            var elementsInRow = 0;

            foreach (UIElement child in Children)
            {
                var span = GetSpan(child);
                span = Math.Min(span, Columns);

                if (currentCol + span > Columns)
                {
                    rowHeights.Add(currentRowHeight);
                    currentRowHeight = 0;
                    currentCol = 0;
                    elementsInRow = 0;
                }

                currentRowHeight = Math.Max(currentRowHeight, child.DesiredSize.Height);
                currentCol += span;
                elementsInRow++;
            }

            if (elementsInRow > 0)
                rowHeights.Add(currentRowHeight);

            // Second pass: arrange elements
            currentRow = 0;
            currentCol = 0;
            currentY = 0;

            foreach (UIElement child in Children)
            {
                var span = GetSpan(child);
                span = Math.Min(span, Columns);

                if (currentCol + span > Columns)
                {
                    currentCol = 0;
                    currentRow++;
                    currentY += rowHeights[currentRow - 1] + Spacing;
                }

                var childWidth = (columnWidth * span) + (Spacing * (span - 1));
                var x = currentCol * (columnWidth + Spacing);

                child.Arrange(new Rect(x, currentY, childWidth, rowHeights[currentRow]));
                currentCol += span;
            }

            return finalSize;
        }
    }

    public class Breakpoint
    {
        public double Width { get; set; }
        public int Columns { get; set; }
    }

    public class BreakpointCollection : List<Breakpoint> { }
}