using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Interop;
using System.Windows;

namespace MyFences.Util
{
    public static class NativeMethods
    {
        public const uint CMF_NORMAL = 0x00000000;
        public const uint GCS_VERBA = 0x00000000;
        public const uint GCS_HELPTEXTA = 0x00000001;
        public const uint GCS_VALIDATE = 0x00000002;
        public const uint GCS_VERBW = 0x00000004;
        public const uint GCS_HELPTEXTW = 0x00000005;
        public const uint CMIC_MASK_UNICODE = 0x00004000;
        public const uint SEE_MASK_INVOKEIDLIST = 0x0000000C;

        [StructLayout(LayoutKind.Sequential)]
        public struct CMINVOKECOMMANDINFOEX
        {
            public int cbSize;
            public int fMask;
            public IntPtr hwnd;
            public IntPtr lpVerb;
            public IntPtr lpParameters;
            public IntPtr lpDirectory;
            public int nShow;
            public int dwHotKey;
            public IntPtr hIcon;
            public IntPtr lpTitle;
            public IntPtr lpVerbW;
            public IntPtr lpParametersW;
            public IntPtr lpDirectoryW;
            public IntPtr lpTitleW;
            public POINT ptInvoke;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int SHParseDisplayName(string name, IntPtr bindingContext, out IntPtr pidl, uint sfgaoIn, out uint psfgaoOut);

        [DllImport("shell32.dll")]
        public static extern int SHBindToParent(IntPtr pidl, ref Guid riid, out IShellFolder ppv, out IntPtr ppidlLast);

        [DllImport("user32.dll")]
        public static extern IntPtr CreatePopupMenu();

        [DllImport("user32.dll")]
        public static extern bool DestroyMenu(IntPtr hMenu);

        [DllImport("user32.dll")]
        public static extern bool TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hwnd, IntPtr prcRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMenuItemID(IntPtr hMenu, int nPos);

        [DllImport("user32.dll")]
        public static extern int GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hwnd, IntPtr lptpm);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetMenuItemCount(IntPtr hMenu);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetMenuString(IntPtr hMenu, uint uIDItem, [Out] StringBuilder lpString, int nMaxCount, uint uFlag);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        public const uint MF_BYPOSITION = 0x00000400;
        public const uint MF_STRING = 0x00000000;

        public const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
        public const uint WINEVENT_OUTOFCONTEXT = 0;

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(
            uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc,
            uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        public delegate void WinEventDelegate(
            IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild,
            uint dwEventThread, uint dwmsEventTime);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e6-0000-0000-c000-000000000046")]
    public interface IShellFolder
    {
        void ParseDisplayName(IntPtr hwnd, IntPtr pbc, [MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName,
            out uint pchEaten, out IntPtr ppidl, ref uint pdwAttributes);

        void EnumObjects(IntPtr hwnd, int grfFlags, out IntPtr ppenumIDList);

        void BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        void BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);

        void CompareIDs(int lParam, IntPtr pidl1, IntPtr pidl2);

        void CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

        void GetAttributesOf(uint cidl, [In] IntPtr apidl, ref uint rgfInOut);

        void GetUIObjectOf(IntPtr hwndOwner, uint cidl, [In] ref IntPtr apidl, [In] ref Guid riid,
            IntPtr rgfReserved, out IContextMenu ppv);

        void GetDisplayNameOf(IntPtr pidl, uint uFlags, out IntPtr pName);

        void SetNameOf(IntPtr hwnd, IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszName, uint uFlags, out IntPtr ppidlOut);
    }

    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e4-0000-0000-c000-000000000046")]
    public interface IContextMenu
    {
        int QueryContextMenu(IntPtr hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags);

        void InvokeCommand(ref NativeMethods.CMINVOKECOMMANDINFOEX pici);

        void GetCommandString(
    uint idCmd,
    uint uFlags,
    IntPtr pReserved,
    [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName,
    uint cchMax);
    }
    public class ShellContextMenuHelper
    {
        public static void ShowShellContextMenu(string path, Window owner)
        {
            IntPtr pidl;
            uint attrs;
            var hr = NativeMethods.SHParseDisplayName(path, IntPtr.Zero, out pidl, 0, out attrs);
            if (hr != 0 || pidl == IntPtr.Zero) return;

            Guid IID_IShellFolder = typeof(IShellFolder).GUID;
            IShellFolder parentFolder;
            IntPtr relativePIDL;

            hr = NativeMethods.SHBindToParent(pidl, ref IID_IShellFolder, out parentFolder, out relativePIDL);
            if (hr != 0) return;

            IContextMenu contextMenu;
            Guid IID_IContextMenu = typeof(IContextMenu).GUID;
            parentFolder.GetUIObjectOf(IntPtr.Zero, 1, ref relativePIDL, ref IID_IContextMenu, IntPtr.Zero, out contextMenu);

            IntPtr hMenu = NativeMethods.CreatePopupMenu();

            int idCmdFirst = 1;
            int idCmdLast = 0x7FFF;

            int itemsAdded = contextMenu.QueryContextMenu(hMenu, 0, (uint)idCmdFirst, (uint)idCmdLast, NativeMethods.CMF_NORMAL);

            //for (int pos = NativeMethods.GetMenuItemCount(hMenu) - 1; pos >= 0; pos--)
            //{
            //    int cmdId = NativeMethods.GetMenuItemID(hMenu, pos);
            //    uint relativeCmd = (uint)(cmdId - idCmdFirst);
            //    StringBuilder verb = new StringBuilder(256);
            //    try
            //    {
            //        contextMenu.GetCommandString(relativeCmd, NativeMethods.GCS_VERBW, IntPtr.Zero, verb, (uint)verb.Capacity);
            //        string verbStr = verb.ToString().ToLowerInvariant();

            //        if (verbStr == "cut" || verbStr == "copy" || verbStr == "delete")
            //        {
            //            NativeMethods.DeleteMenu(hMenu, (uint)pos, NativeMethods.MF_BYPOSITION);
            //        }
            //    }
            //    catch
            //    {
            //        // Ignore non-verbs
            //    }
            //}

            NativeMethods.POINT pt;
            NativeMethods.GetCursorPos(out pt);

            var hwnd = new WindowInteropHelper(owner).Handle;

            const uint TPM_LEFTALIGN = 0x0000;
            const uint TPM_RETURNCMD = 0x0100;

            int selectedCmd = NativeMethods.TrackPopupMenuEx(hMenu, TPM_LEFTALIGN | TPM_RETURNCMD, pt.x, pt.y, hwnd, IntPtr.Zero);

            if (selectedCmd != 0)
            {
                var invoke = new NativeMethods.CMINVOKECOMMANDINFOEX
                {
                    cbSize = Marshal.SizeOf(typeof(NativeMethods.CMINVOKECOMMANDINFOEX)),
                    fMask = (int)NativeMethods.CMIC_MASK_UNICODE,
                    hwnd = hwnd,
                    lpVerb = (IntPtr)(selectedCmd - 1),
                    nShow = 1,
                    ptInvoke = pt
                };

                contextMenu.InvokeCommand(ref invoke);
            }

            NativeMethods.DestroyMenu(hMenu);
            Marshal.ReleaseComObject(contextMenu);
            Marshal.FreeCoTaskMem(pidl);
        }
    }
}
