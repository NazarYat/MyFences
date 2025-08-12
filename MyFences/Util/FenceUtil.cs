using MyFences.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFences.Util
{
    public static class FenceUtil
    {
        public static Fence CopyStyleFrom(this Fence dest, Fence src)
        {
            dest.BorderThickness = src.BorderThickness;
            dest.ScrollBarBorderThickness = src.ScrollBarBorderThickness;
            dest.ScrollBarCornerRadius = src.ScrollBarCornerRadius;
            dest.ScrollBarColor = src.ScrollBarColor;
            dest.ScrollBarBorderColor = src.ScrollBarBorderColor;
            dest.BackgroundColor = src.BackgroundColor;
            dest.WindowBorderColor = src.WindowBorderColor;
            dest.HeaderColor = src.HeaderColor;
            dest.HeaderHeight = src.HeaderHeight;
            dest.ItemColor = src.ItemColor;
            dest.ItemBorderColor = src.ItemBorderColor;
            dest.ItemBorderThickness = src.ItemBorderThickness;
            dest.HighlightedItemColor = src.HighlightedItemColor;
            dest.HighlightedItemBorderColor = src.HighlightedItemBorderColor;
            dest.HighlightedItemBorderThickness = src.HighlightedItemBorderThickness;
            dest.SelectedItemColor = src.SelectedItemColor;
            dest.SelectedItemBorderColor = src.SelectedItemBorderColor;
            dest.SelectedItemBorderThickness = src.SelectedItemBorderThickness;
            dest.ItemTextColor = src.ItemTextColor;
            dest.HeaderTextColor = src.HeaderTextColor;
            dest.ItemFontSize = src.ItemFontSize;
            dest.HeaderFontSize = src.HeaderFontSize;
            dest.ItemHeight = src.ItemHeight;
            dest.ItemWidth = src.ItemWidth;
            dest.ItemIconSize = src.ItemIconSize;
            dest.Spacing = src.Spacing;
            dest.UseBlur = src.UseBlur;

            return dest;
        }
    }
}
