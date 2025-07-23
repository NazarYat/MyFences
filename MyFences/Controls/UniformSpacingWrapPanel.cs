using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MyFences.Controls
{
    public class UniformSpacingWrapPanel : Panel
    {
        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(
                nameof(ItemWidth),
                typeof(double),
                typeof(UniformSpacingWrapPanel),
                new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(
                nameof(ItemHeight),
                typeof(double),
                typeof(UniformSpacingWrapPanel),
                new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty HorizontalSpacingProperty =
            DependencyProperty.Register(
                nameof(HorizontalSpacing),
                typeof(double),
                typeof(UniformSpacingWrapPanel),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty VerticalSpacingProperty =
            DependencyProperty.Register(
                nameof(VerticalSpacing),
                typeof(double),
                typeof(UniformSpacingWrapPanel),
                new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double ItemWidth
        {
            get => (double)GetValue(ItemWidthProperty);
            set => SetValue(ItemWidthProperty, value);
        }

        public double ItemHeight
        {
            get => (double)GetValue(ItemHeightProperty);
            set => SetValue(ItemHeightProperty, value);
        }

        public double HorizontalSpacing
        {
            get => (double)GetValue(HorizontalSpacingProperty);
            set => SetValue(HorizontalSpacingProperty, value);
        }

        public double VerticalSpacing
        {
            get => (double)GetValue(VerticalSpacingProperty);
            set => SetValue(VerticalSpacingProperty, value);
        }
        private static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());

        protected override Size MeasureOverride(Size availableSize)
        {
            if (IsInDesignMode)
                return base.MeasureOverride(availableSize); // or return fixed size

            foreach (UIElement child in InternalChildren)
                child.Measure(new Size(ItemWidth, ItemHeight));

            int columns = Math.Max(1, (int)((availableSize.Width + HorizontalSpacing) / (ItemWidth + HorizontalSpacing)));
            int rows = (int)Math.Ceiling((double)InternalChildren.Count / columns);

            double width = columns * ItemWidth + (columns - 1) * HorizontalSpacing;
            double height = rows * ItemHeight + (rows - 1) * VerticalSpacing;


            width = Math.Max(width, 0);
            height = Math.Max(height, 0);

            return new Size(width, height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int columns = Math.Max(1, (int)((finalSize.Width + HorizontalSpacing) / (ItemWidth + HorizontalSpacing)));

            double totalSpacing = finalSize.Width - columns * ItemWidth;
            double spacing = columns > 1 ? totalSpacing / (columns - 1) : 0;

            for (int i = 0; i < InternalChildren.Count; i++)
            {
                int col = i % columns;
                int row = i / columns;

                double x = col * (ItemWidth + spacing);
                double y = row * (ItemHeight + VerticalSpacing);

                InternalChildren[i].Arrange(new Rect(new Point(x, y), new Size(ItemWidth, ItemHeight)));
            }

            return finalSize;
        }
    }
}
