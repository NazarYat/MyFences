using MyFences.Util;
using MyFences.ViewModels;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace MyFences.Windows
{
    /// <summary>
    /// Interaction logic for Fence.xaml
    /// </summary>
    public partial class FenceWindow : Window
    {
        public FenceViewModel? ViewModel => DataContext as FenceViewModel;

        private bool _useBlur = false;
        public bool UseBlur
        {
            get => _useBlur;
            set
            {
                if (_useBlur == value) return;
                _useBlur = value;
                if (value) EnableBlur();
                else DisableBlur();
            }
        }
        public FenceWindow()
        {
            InitializeComponent();

            var chrome = new WindowChrome
            {
                CaptionHeight = 0,
                ResizeBorderThickness = new Thickness(6),
                GlassFrameThickness = new Thickness(0),
                UseAeroCaptionButtons = false
            };

            WindowChrome.SetWindowChrome(this, chrome); 
        }

        private void TopBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed && e.ChangedButton == MouseButton.Left)
                this.DragMove(); // Makes the window draggable
        }

        private DateTime _lastClick = DateTime.Now;
        private void NameTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DateTime.Now - _lastClick < TimeSpan.FromMilliseconds(300))
            {
                if (ViewModel != null)
                {
                    ViewModel.NameEditing = true;
                    NameTextBox.Focus();
                    NameTextBox.CaretIndex = NameTextBox.Text.Length;
                }
            }
            else
            {
                _lastClick = DateTime.Now;
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject clickedElement = e.OriginalSource as DependencyObject;

            if (!IsInsideTextBox(clickedElement))
            {
                Keyboard.ClearFocus();
            }
        }

        private bool IsInsideTextBox(DependencyObject element)
        {
            while (element != null)
            {
                if (element is TextBox)
                    return true;

                element = VisualTreeHelper.GetParent(element);
            }

            return false;
        }
        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel != null)
            {
                ViewModel.NameEditing = false;
                ViewModel.Name = NameTextBox.Text;
            }
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private void FenceWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
            SetWindowPos(hwnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);

            if (ViewModel != null && ViewModel.Fence.UseBlur) EnableBlur();
        }
        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
            var accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND
            };

            int accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        private void DisableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
            var accent = new AccentPolicy
            {
                AccentState = AccentState.ACCENT_DISABLED
            };

            int accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };

            SetWindowCompositionAttribute(windowHelper.Handle, ref data);
            Marshal.FreeHGlobal(accentPtr);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        private enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_ENABLE_ACRYLICBLURBEHIND = 4 
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        private enum WindowCompositionAttribute
        {
            WCA_ACCENT_POLICY = 19
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOACTIVATE = 0x0010;
        private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
            int X, int Y, int cx, int cy, uint uFlags);

        private void ListView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;

            e.Handled = true;
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    ViewModel?.AddFile(file);
                }
            }
        }
        private void ListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var listView = sender as ListView;
                if (listView?.SelectedItems == null || listView.SelectedItems.Count == 0)
                    return;

                var selectedItems = listView.SelectedItems.Cast<ItemViewModel>().ToList();

                foreach (var item in selectedItems)
                {
                    ViewModel?.RemoveFile(item);
                }

                e.Handled = true;
            }
        }
        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel != null)
                {
                    ViewModel.NameEditing = false;
                }
            }
        }

        private DateTime _lastItemMouseDown = DateTime.MinValue;
        private string _lastMouseDownItem = string.Empty;

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left) return;

            if (sender is FrameworkElement element && element.DataContext is ItemViewModel itemViewModel)
            {
                if (DateTime.Now - _lastItemMouseDown < TimeSpan.FromMilliseconds(300) && itemViewModel.Path == _lastMouseDownItem)
                {

                    _lastMouseDownItem = itemViewModel.Path;

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = itemViewModel.Path,
                        UseShellExecute = true
                    });
                }
                else
                {
                    _lastMouseDownItem = itemViewModel.Path;
                    _lastItemMouseDown = DateTime.Now;
                }
            }
        }

        private void StackPanel_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is ItemViewModel itemViewModel)
            {
                ShellContextMenuHelper.ShowShellContextMenu(itemViewModel.Path, this);
            }
        }
        private Thickness _margin => ViewModel?._applicationViewModel.AppData.GridMargin ?? new Thickness(0, 0, 0, 0);
        private int Columns => ViewModel?._applicationViewModel.AppData.GridColumns ?? 0;
        private int Rows => ViewModel?._applicationViewModel.AppData.GridRows ?? 0;
        private bool UseGrid => ViewModel?._applicationViewModel.AppData.UseGrid ?? false;

        private const int WM_MOVING = 0x0216;
        private const int WM_SIZING = 0x0214;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(WndProc);
        }

        private Rect GetScreenBounds()
        {
            return new Rect(
                SystemParameters.VirtualScreenLeft,
                SystemParameters.VirtualScreenTop,
                SystemParameters.VirtualScreenWidth,
                SystemParameters.VirtualScreenHeight);
        }

        private double GetXStep(double screenWidth)
        {
            return (screenWidth - _margin.Left - _margin.Right) / (Columns - 1);
        }

        private double GetYStep(double screenHeight)
        {
            return (screenHeight - _margin.Top - _margin.Bottom) / (Rows - 1);
        }

        private double SnapToNearest(double value, double step, double offset, double min, double max)
        {
            double snapped = Math.Round((value - offset) / step) * step + offset;
            return Math.Max(min, Math.Min(snapped, max));
        }

        private DpiScale GetDpi()
        {
            return VisualTreeHelper.GetDpi(this);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (!UseGrid) return IntPtr.Zero;

            var screen = GetScreenBounds();
            var dpi = GetDpi();
            double scaleX = dpi.DpiScaleX;
            double scaleY = dpi.DpiScaleY;

            double xStep = GetXStep(screen.Width);
            double yStep = GetYStep(screen.Height);

            if (msg == WM_MOVING)
            {
                unsafe
                {
                    RECT* rect = (RECT*)lParam;

                    double width = (rect->Right - rect->Left) / scaleX;
                    double height = (rect->Bottom - rect->Top) / scaleY;

                    double left = rect->Left / scaleX;
                    double top = rect->Top / scaleY;

                    double snappedLeft = SnapToNearest(left, xStep, _margin.Left,
                        _margin.Left, screen.Right - _margin.Right - width);
                    double snappedTop = SnapToNearest(top, yStep, _margin.Top,
                        _margin.Top, screen.Bottom - _margin.Bottom - height);

                    rect->Left = (int)(snappedLeft * scaleX);
                    rect->Top = (int)(snappedTop * scaleY);
                    rect->Right = (int)((snappedLeft + width) * scaleX);
                    rect->Bottom = (int)((snappedTop + height) * scaleY);
                }

                handled = true;
            }
            else if (msg == WM_SIZING)
            {
                unsafe
                {
                    RECT* rect = (RECT*)lParam;
                    int edge = wParam.ToInt32();

                    double left = rect->Left / scaleX;
                    double top = rect->Top / scaleY;
                    double right = rect->Right / scaleX;
                    double bottom = rect->Bottom / scaleY;

                    switch (edge)
                    {
                        case 1: // Left
                            left = SnapToNearest(left, xStep, _margin.Left, _margin.Left, right - xStep);
                            break;
                        case 2: // Right
                            right = SnapToNearest(right, xStep, _margin.Left, left + xStep, screen.Right - _margin.Right);
                            break;
                        case 3: // Top
                            top = SnapToNearest(top, yStep, _margin.Top, _margin.Top, bottom - yStep);
                            break;
                        case 6: // Bottom
                            bottom = SnapToNearest(bottom, yStep, _margin.Top, top + yStep, screen.Bottom - _margin.Bottom);
                            break;
                        case 4: // Top-left
                            left = SnapToNearest(left, xStep, _margin.Left, _margin.Left, right - xStep);
                            top = SnapToNearest(top, yStep, _margin.Top, _margin.Top, bottom - yStep);
                            break;
                        case 5: // Top-right
                            right = SnapToNearest(right, xStep, _margin.Left, left + xStep, screen.Right - _margin.Right);
                            top = SnapToNearest(top, yStep, _margin.Top, _margin.Top, bottom - yStep);
                            break;
                        case 7: // Bottom-left
                            left = SnapToNearest(left, xStep, _margin.Left, _margin.Left, right - xStep);
                            bottom = SnapToNearest(bottom, yStep, _margin.Top, top + yStep, screen.Bottom - _margin.Bottom);
                            break;
                        case 8: // Bottom-right
                            right = SnapToNearest(right, xStep, _margin.Left, left + xStep, screen.Right - _margin.Right);
                            bottom = SnapToNearest(bottom, yStep, _margin.Top, top + yStep, screen.Bottom - _margin.Bottom);
                            break;
                    }

                    rect->Left = (int)(left * scaleX);
                    rect->Top = (int)(top * scaleY);
                    rect->Right = (int)(right * scaleX);
                    rect->Bottom = (int)(bottom * scaleY);
                }

                handled = true;
            }

            return IntPtr.Zero;
        }

        public void SnapToScreenGrid()
        {
            var screen = GetScreenBounds();
            var dpi = GetDpi();
            double scaleX = dpi.DpiScaleX;
            double scaleY = dpi.DpiScaleY;

            double xStep = GetXStep(screen.Width);
            double yStep = GetYStep(screen.Height);

            double physicalWidth = Width * scaleX;
            double physicalHeight = Height * scaleY;

            double snappedLeft = SnapToNearest(Left * scaleX, xStep * scaleX, _margin.Left * scaleX,
                _margin.Left * scaleX, (screen.Right - _margin.Right) * scaleX - physicalWidth) / scaleX;

            double snappedTop = SnapToNearest(Top * scaleY, yStep * scaleY, _margin.Top * scaleY,
                _margin.Top * scaleY, (screen.Bottom - _margin.Bottom) * scaleY - physicalHeight) / scaleY;

            double snappedWidth = SnapToNearest(Width * scaleX, xStep * scaleX, 0,
                xStep * scaleX, (screen.Width - _margin.Left - _margin.Right) * scaleX) / scaleX;

            double snappedHeight = SnapToNearest(Height * scaleY, yStep * scaleY, 0,
                yStep * scaleY, (screen.Height - _margin.Top - _margin.Bottom) * scaleY) / scaleY;

            Left = snappedLeft;
            Top = snappedTop;
            Width = snappedWidth;
            Height = snappedHeight;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is FenceViewModel fenceViewModel)
            {
                fenceViewModel.OpenSetupDialog();
            }
        }
        private void MenuItem_Click2(object sender, RoutedEventArgs e)
        {
            if (DataContext is FenceViewModel fenceViewModel)
            {
                fenceViewModel.CreateNewFence();
            }
        }
        private void MenuItem_Click3(object sender, RoutedEventArgs e)
        {
            if (DataContext is FenceViewModel fenceViewModel)
            {
                fenceViewModel.DeleteFence();
            }
        }
    }
}
