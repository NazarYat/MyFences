using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyFences.Controls
{
    /// <summary>
    /// Interaction logic for ThicknessEditor.xaml
    /// </summary>
    public partial class ThicknessEditor : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static readonly DependencyProperty TextBoxStyleProperty = DependencyProperty.Register(nameof(TextBoxStyle), typeof(Style), typeof(ThicknessEditor), new PropertyMetadata(null));

        public Style TextBoxStyle
        {
            get => (Style)GetValue(TextBoxStyleProperty);
            set => SetValue(TextBoxStyleProperty, value);
        }
        public ThicknessEditor()
        {
            InitializeComponent();
        }

        public Thickness Value
        {
            get => (Thickness)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(Thickness),
                typeof(ThicknessEditor),
                new FrameworkPropertyMetadata(new Thickness(0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public double Left
        {
            get => Value.Left;
            set => Value = new Thickness(value, Value.Top, Value.Right, Value.Bottom);
        }

        public double Top
        {
            get => Value.Top;
            set => Value = new Thickness(Value.Left, value, Value.Right, Value.Bottom);
        }

        public double Right
        {
            get => Value.Right;
            set => Value = new Thickness(Value.Left, Value.Top, value, Value.Bottom);
        }

        public double Bottom
        {
            get => Value.Bottom;
            set => Value = new Thickness(Value.Left, Value.Top, Value.Right, value);
        }
        public void ValueChanged()
        {
            OnPropertyChanged(nameof(Left));
            OnPropertyChanged(nameof(Top));
            OnPropertyChanged(nameof(Right));
            OnPropertyChanged(nameof(Bottom));
        }
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ThicknessEditor)d;
            var thickness = (Thickness)e.NewValue;

            // Sync individual properties
            control.Left = thickness.Left;
            control.Top = thickness.Top;
            control.Right = thickness.Right;
            control.Bottom = thickness.Bottom;

            control.ValueChanged();
        }
    }
}
