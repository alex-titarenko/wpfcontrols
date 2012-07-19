using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.ComponentModel;

using Microsoft.Win32;

namespace TAlex.WPF.CommonDialogs
{
    /// <summary>
    /// Prompts the user to select a folder.
    /// </summary>
    public class FolderBrowserDialog : CommonDialog
    {
        #region Fields

        private UnsafeNativeMethods.BrowseCallbackProc _callback;
        private string _descriptionText;
        private string _selectedPath;
        private bool _selectedPathNeedsCheck;
        private bool _showNewFolderButton;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        public string Description
        {
            get
            {
                return _descriptionText;
            }
            set
            {
                _descriptionText = (value == null) ? string.Empty : value;
            }
        }

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        public string SelectedPath
        {
            get
            {
                if (((_selectedPath != null) && (_selectedPath.Length != 0)) && _selectedPathNeedsCheck)
                {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, _selectedPath).Demand();
                }
                return _selectedPath;
            }
            set
            {
                _selectedPath = (value == null) ? string.Empty : value;
                _selectedPathNeedsCheck = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box.
        /// </summary>
        public bool ShowNewFolderButton
        {
            get
            {
                return _showNewFolderButton;
            }
            set
            {
                _showNewFolderButton = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TAlex.WPF.CommonDialogs.FolderBrowserDialog"/> class. 
        /// </summary>
        public FolderBrowserDialog()
        {
            ShowNewFolderButton = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset()
        {
            _descriptionText = String.Empty;
            _selectedPath = String.Empty;
            _selectedPathNeedsCheck = false;
            _showNewFolderButton = true;
        }

        /// <summary>
        /// When overridden in a derived class, specifies a common dialog box.
        /// </summary>
        /// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
        /// <returns>true if the dialog box was successfully run; otherwise, false.</returns>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            IntPtr root = IntPtr.Zero;
            bool flag = false;

            UnsafeNativeMethods.Shell32.SHGetSpecialFolderLocation(hwndOwner, (int)Environment.SpecialFolder.Desktop, ref root);

            int browseInfoFlags = NativeMethods.BIF_NEWDIALOGSTYLE;

            if (!_showNewFolderButton)
            {
                browseInfoFlags += NativeMethods.BIF_NONEWFOLDERBUTTON;
            }

            IntPtr pidl = IntPtr.Zero;
            IntPtr hglobal = IntPtr.Zero;
            IntPtr pszPath = IntPtr.Zero;

            try
            {
                hglobal = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                pszPath = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                _callback = new UnsafeNativeMethods.BrowseCallbackProc(OnBrowseCallbackProc);

                UnsafeNativeMethods.BROWSEINFO lpbi = new UnsafeNativeMethods.BROWSEINFO();

                lpbi.pidlRoot = root;
                lpbi.hwndOwner = hwndOwner;
                lpbi.pszDisplayName = hglobal;
                lpbi.lpszTitle = _descriptionText;
                lpbi.ulFlags = browseInfoFlags;
                lpbi.lpfn = _callback;
                lpbi.lParam = IntPtr.Zero;
                lpbi.iImage = 0;

                pidl = UnsafeNativeMethods.Shell32.SHBrowseForFolder(lpbi);

                if (pidl != IntPtr.Zero)
                {
                    UnsafeNativeMethods.Shell32.SHGetPathFromIDList(pidl, pszPath);
                    _selectedPathNeedsCheck = true;
                    _selectedPath = Marshal.PtrToStringAuto(pszPath);
                    flag = true;
                }
            }
            finally
            {
                UnsafeNativeMethods.IMalloc sHMalloc = GetSHMalloc();
                sHMalloc.Free(root);

                if (pidl != IntPtr.Zero)
                    sHMalloc.Free(pidl);
                if (pszPath != IntPtr.Zero)
                    Marshal.FreeHGlobal(pszPath);
                if (hglobal != IntPtr.Zero)
                    Marshal.FreeHGlobal(hglobal);

                _callback = null;
            }

            return flag;
        }


        private static UnsafeNativeMethods.IMalloc GetSHMalloc()
        {
            UnsafeNativeMethods.IMalloc[] ppMalloc = new UnsafeNativeMethods.IMalloc[1];
            UnsafeNativeMethods.Shell32.SHGetMalloc(ppMalloc);
            return ppMalloc[0];
        }

        private int OnBrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
        {
            switch (msg)
            {
                case NativeMethods.BFFM_INITIALIZED:
                    if (!String.IsNullOrEmpty(_selectedPath))
                    {
                        if (Marshal.SystemDefaultCharSize == 1)
                            UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), NativeMethods.BFFM_SETSELECTIONA, 1, lpData);
                        else if (Marshal.SystemDefaultCharSize == 2)
                            UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), NativeMethods.BFFM_SETSELECTIONW, 1, _selectedPath);
                    }
                    break;

                case NativeMethods.BFFM_SELCHANGED:
                    {
                        IntPtr pidl = lParam;
                        if (pidl != IntPtr.Zero)
                        {
                            IntPtr pszPath = Marshal.AllocHGlobal((int)(260 * Marshal.SystemDefaultCharSize));
                            bool flag = UnsafeNativeMethods.Shell32.SHGetPathFromIDList(pidl, pszPath);
                            Marshal.FreeHGlobal(pszPath);
                            UnsafeNativeMethods.SendMessage(new HandleRef(null, hwnd), NativeMethods.BFFM_ENABLEOK, 0, flag ? 1 : 0);
                        }
                        break;
                    }
            }

            return 0;
        }

        #endregion
    }


    internal static class NativeMethods
    {
        public const int BIF_NEWDIALOGSTYLE = 0x0040;      // Use the new dialog layout with the ability to resize
        public const int BIF_NONEWFOLDERBUTTON = 0x0200;   // Do not add the "New Folder" button to the dialog.  Only applicable with BIF_NEWDIALOGSTYLE.

        public const int WM_USER = 0x400;
        public const int BFFM_SETSELECTIONA = WM_USER + 102;
        public const int BFFM_SETSELECTIONW = WM_USER + 103;

        public const int BFFM_INITIALIZED = 1;
        public const int BFFM_SELCHANGED = 2;
        public const int BFFM_ENABLEOK = 0x465;
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class UnsafeNativeMethods
    {
        public delegate int BrowseCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);


        [DllImport("user32.dll", PreserveSig = true)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint msg, long wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);


        [SuppressUnmanagedCodeSecurity]
        internal class Shell32
        {
            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SHBrowseForFolder([In] UnsafeNativeMethods.BROWSEINFO lpbi);

            [DllImport("shell32.dll")]
            public static extern int SHGetMalloc([Out, MarshalAs(UnmanagedType.LPArray)] UnsafeNativeMethods.IMalloc[] ppMalloc);

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

            [DllImport("shell32.dll")]
            public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class BROWSEINFO
        {
            public IntPtr hwndOwner;
            public IntPtr pidlRoot;
            public IntPtr pszDisplayName;
            public string lpszTitle;
            public int ulFlags;
            public UnsafeNativeMethods.BrowseCallbackProc lpfn;
            public IntPtr lParam;
            public int iImage;
        }

        [ComImport, Guid("00000002-0000-0000-c000-000000000046"), SuppressUnmanagedCodeSecurity, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IMalloc
        {
            [PreserveSig]
            IntPtr Alloc(int cb);
            [PreserveSig]
            IntPtr Realloc(IntPtr pv, int cb);
            [PreserveSig]
            void Free(IntPtr pv);
            [PreserveSig]
            int GetSize(IntPtr pv);
            [PreserveSig]
            int DidAlloc(IntPtr pv);
            [PreserveSig]
            void HeapMinimize();
        }
    }
}
