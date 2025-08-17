using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MyFences.Util
{
    public static class IconHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private const uint SHGFI_SYSICONINDEX = 0x000004000;
        private const uint SHGFI_ICON = 0x000000100;
        private const uint SHGFI_LARGEICON = 0x000000000; // 32x32
        private const uint SHGFI_SMALLICON = 0x000000001; // 16x16

        // ---------------------------
        // For 256x256 jumbo icons
        // ---------------------------
        [ComImport]
        [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IImageList
        {
            [PreserveSig]
            int GetIcon(int i, int flags, out IntPtr picon);
        }

        private enum SHIL : int
        {
            SHIL_LARGE = 0x0,
            SHIL_EXTRALARGE = 0x2,
            SHIL_JUMBO = 0x4
        }

        private const int ILD_TRANSPARENT = 0x00000001;

        [DllImport("shell32.dll", EntryPoint = "#727")]
        private static extern int SHGetImageList(
            int iImageList,
            ref Guid riid,
            out IImageList ppv);

        // ---------------------------
        // Public API
        // ---------------------------
        public static ImageSource? GetSystemIconMaxResolution(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            var shinfo = new SHFILEINFO();

            // Only try jumbo for executables or icons
            string ext = System.IO.Path.GetExtension(path)?.ToLowerInvariant() ?? "";
            bool canUseJumbo = ext == ".exe" || ext == ".ico" || ext == ".dll";

            if (canUseJumbo)
            {
                try
                {
                    Guid iidImageList = typeof(IImageList).GUID;
                    if (SHGetImageList((int)SHIL.SHIL_JUMBO, ref iidImageList, out IImageList iml) == 0)
                    {
                        SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_SYSICONINDEX);
                        if (shinfo.iIcon >= 0 && iml.GetIcon(shinfo.iIcon, ILD_TRANSPARENT, out IntPtr hIcon) == 0 && hIcon != IntPtr.Zero)
                        {
                            try
                            {
                                var img = Imaging.CreateBitmapSourceFromHIcon(
                                    hIcon,
                                    Int32Rect.Empty,
                                    BitmapSizeOptions.FromEmptyOptions());
                                img.Freeze();
                                return img;
                            }
                            finally
                            {
                                DestroyIcon(hIcon);
                            }
                        }
                    }
                }
                catch
                {
                    // fallback silently
                }
            }

            // Fallback: large icon (32x32)
            SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_LARGEICON);
            if (shinfo.hIcon != IntPtr.Zero)
            {
                try
                {
                    var img = Imaging.CreateBitmapSourceFromHIcon(
                        shinfo.hIcon,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                    img.Freeze();
                    return img;
                }
                finally
                {
                    DestroyIcon(shinfo.hIcon);
                }
            }

            // Fallback: small icon (16x16)
            SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);
            if (shinfo.hIcon != IntPtr.Zero)
            {
                try
                {
                    var img = Imaging.CreateBitmapSourceFromHIcon(
                        shinfo.hIcon,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                    img.Freeze();
                    return img;
                }
                finally
                {
                    DestroyIcon(shinfo.hIcon);
                }
            }

            return null;
        }


    }
}
