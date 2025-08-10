using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyFences.Controls
{
    /// <summary>
    /// Interaction logic for NumericUpToDown.xaml
    /// </summary>
    public partial class NumericUpToDown : UserControl
    {
        public NumericUpToDown()
        {
            InitializeComponent();
            PART_TextBox.PreviewMouseWheel += TextBox_PreviewMouseWheel;
        }
        public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register(nameof(TextBoxStyle), typeof(Style), typeof(NumericUpToDown), new PropertyMetadata(null));

        public Style TextBoxStyle
        {
            get => (Style)GetValue(TextBoxStyleProperty);
            set => SetValue(TextBoxStyleProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(NumericUpToDown),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(int), typeof(NumericUpToDown), new PropertyMetadata(0));

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(NumericUpToDown), new PropertyMetadata(100));

        public int Value
        {
            get => (int)GetValue(ValueProperty);
            set
            {
                if (value < Minimum) value = Minimum;
                if (value > Maximum) value = Maximum;
                SetValue(ValueProperty, value);
            }
        }

        public int Minimum
        {
            get => (int)GetValue(MinimumProperty);
            set
            {
                if (value > Maximum) value = Maximum;
                SetValue(MinimumProperty, value);
            }
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set
            {
                if (value < Minimum) value = Minimum;

                SetValue(MaximumProperty, value);
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        //private void Up_Click(object sender, RoutedEventArgs e)
        //{
        //    Value++;
        //}

        //private void Down_Click(object sender, RoutedEventArgs e)
        //{
        //    Value--;
        //}

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                Value++;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                Value--;
                e.Handled = true;
            }
        }

        private void TextBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0) Value++;
            else Value--;
            e.Handled = true;
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Value < Minimum) Value = Minimum;
            if (Value > Maximum) Value = Maximum;
        }
    }
}
