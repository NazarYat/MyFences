using MyFences.Models;
using System.Windows.Media;
using System.Runtime.CompilerServices;

namespace MyFences.ViewModels
{
    public class SetupViewModel : ViewModelBase
    {
        private readonly Action? _settingChangedCallback;
        public SetupViewModel() { }
        public ApplicationData ApplicationData { get; set; }
        public Fence Fence { get; set; }

        public string Name
        {
            get => Fence.Name;
            set
            {
                if (Fence.Name == value) return;

                Fence.Name = value;

                NotifyOfPropertyChanged();
            }
        }

        public float Width
        {
            get => Fence.Width;
            set
            {
                if (Fence.Width == value) return;

                Fence.Width = value;

                NotifyOfPropertyChanged();
            }
        }

        public float Height
        {
            get => Fence.Height;
            set
            {
                if (Fence.Height == value) return;

                Fence.Height = value;

                NotifyOfPropertyChanged();
            }
        }

        public float Left
        {
            get => Fence.Left;
            set
            {
                if (Fence.Left == value) return;

                Fence.Left = value;

                NotifyOfPropertyChanged();
            }
        }

        public float Top
        {
            get => Fence.Top;
            set
            {
                if (Fence.Top == value) return;

                Fence.Top = value;

                NotifyOfPropertyChanged();
            }
        }

        public bool UseBlur
        {
            get => Fence.UseBlur;
            set
            {
                if (Fence.UseBlur == value) return;

                Fence.UseBlur = value;

                NotifyOfPropertyChanged();
            }
        }

        public int BorderThickness
        {
            get => Fence.BorderThickness;
            set
            {
                if (Fence.BorderThickness == value) return;

                Fence.BorderThickness = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color BackgroundColor
        {
            get => Fence.BackgroundColor;
            set
            {
                if (Fence.BackgroundColor == value) return;

                Fence.BackgroundColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color WindowBorderColor
        {
            get => Fence.WindowBorderColor;
            set
            {
                if (Fence.WindowBorderColor == value) return;

                Fence.WindowBorderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color HeaderColor
        {
            get => Fence.HeaderColor;
            set
            {
                if (Fence.HeaderColor == value) return;

                Fence.HeaderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int HeaderHeight
        {
            get => Fence.HeaderHeight;
            set
            {
                if (Fence.HeaderHeight == value) return;

                Fence.HeaderHeight = value;

                NotifyOfPropertyChanged();
            }
        }

        public int Spacing
        {
            get => Fence.Spacing;
            set
            {
                if (Fence.Spacing == value) return;

                Fence.Spacing = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ItemHeight
        {
            get => Fence.ItemHeight;
            set
            {
                if (Fence.ItemHeight == value) return;

                Fence.ItemHeight = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ItemWidth
        {
            get => Fence.ItemWidth;
            set
            {
                if (Fence.ItemWidth == value) return;

                Fence.ItemWidth = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ItemIconSize
        {
            get => Fence.ItemIconSize;
            set
            {
                if (Fence.ItemIconSize == value) return;

                Fence.ItemIconSize = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color ItemColor
        {
            get => Fence.ItemColor;
            set
            {
                if (Fence.ItemColor == value) return;

                Fence.ItemColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color ItemBorderColor
        {
            get => Fence.ItemBorderColor;
            set
            {
                if (Fence.ItemBorderColor == value) return;

                Fence.ItemBorderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ItemBorderThickness
        {
            get => Fence.ItemBorderThickness;
            set
            {
                if (Fence.ItemBorderThickness == value) return;

                Fence.ItemBorderThickness = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color HighlightedItemColor
        {
            get => Fence.HighlightedItemColor;
            set
            {
                if (Fence.HighlightedItemColor == value) return;

                Fence.HighlightedItemColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color HighlightedItemBorderColor
        {
            get => Fence.HighlightedItemBorderColor;
            set
            {
                if (Fence.HighlightedItemBorderColor == value) return;

                Fence.HighlightedItemBorderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int HighlightedItemBorderThickness
        {
            get => Fence.HighlightedItemBorderThickness;
            set
            {
                if (Fence.HighlightedItemBorderThickness == value) return;

                Fence.HighlightedItemBorderThickness = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color SelectedItemColor
        {
            get => Fence.SelectedItemColor;
            set
            {
                if (Fence.SelectedItemColor == value) return;

                Fence.SelectedItemColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color SelectedItemBorderColor
        {
            get => Fence.SelectedItemBorderColor;
            set
            {
                if (Fence.SelectedItemBorderColor == value) return;

                Fence.SelectedItemBorderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int SelectedItemBorderThickness
        {
            get => Fence.SelectedItemBorderThickness;
            set
            {
                if (Fence.SelectedItemBorderThickness == value) return;

                Fence.SelectedItemBorderThickness = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color ItemTextColor
        {
            get => Fence.ItemTextColor;
            set
            {
                if (Fence.ItemTextColor == value) return;

                Fence.ItemTextColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color HeaderTextColor
        {
            get => Fence.HeaderTextColor;
            set
            {
                if (Fence.HeaderTextColor == value) return;

                Fence.HeaderTextColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ItemFontSize
        {
            get => Fence.ItemFontSize;
            set
            {
                if (Fence.ItemFontSize == value) return;

                Fence.ItemFontSize = value;

                NotifyOfPropertyChanged();
            }
        }

        public int HeaderFontSize
        {
            get => Fence.HeaderFontSize;
            set
            {
                if (Fence.HeaderFontSize == value) return;

                Fence.HeaderFontSize = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color ScrollBarColor
        {
            get => Fence.ScrollBarColor;
            set
            {
                if (Fence.ScrollBarColor == value) return;

                Fence.ScrollBarColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public Color ScrollBarBorderColor
        {
            get => Fence.ScrollBarBorderColor;
            set
            {
                if (Fence.ScrollBarBorderColor == value) return;

                Fence.ScrollBarBorderColor = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ScrollBarBorderThickness
        {
            get => Fence.ScrollBarBorderThickness;
            set
            {
                if (Fence.ScrollBarBorderThickness == value) return;

                Fence.ScrollBarBorderThickness = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ScrollBarCornerRadius
        {
            get => Fence.ScrollBarCornerRadius;
            set
            {
                if (Fence.ScrollBarCornerRadius == value) return;

                Fence.ScrollBarCornerRadius = value;

                NotifyOfPropertyChanged();
            }
        }

        public int ScrollBarWidth
        {
            get => Fence.ScrollBarWidth;
            set
            {
                if (Fence.ScrollBarWidth == value) return;

                Fence.ScrollBarWidth = value;

                NotifyOfPropertyChanged();
            }
        }


        public SetupViewModel(ApplicationData applicationData, Fence fence, Action? settingChangedCallback = null)
        {
            ApplicationData = applicationData;
            Fence = fence;
            _settingChangedCallback = settingChangedCallback;
        }

        protected override void NotifyOfPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.NotifyOfPropertyChanged(propertyName);

            _settingChangedCallback?.Invoke();
        }
    }
}
