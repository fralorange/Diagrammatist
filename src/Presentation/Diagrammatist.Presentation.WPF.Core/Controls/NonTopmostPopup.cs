using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

namespace Diagrammatist.Presentation.WPF.Core.Controls
{
    /// <summary>  
    /// A Popup control that does not stay on top of other windows.  
    /// </summary>  
    public partial class NonTopmostPopup : Popup
    {
        /// <inheritdoc/>  
        protected override void OnOpened(EventArgs e)
        {
            var hwnd = ((HwndSource)PresentationSource.FromVisual(Child)).Handle;

            if (GetWindowRect(hwnd, out RECT rect))
            {
                _ = SetWindowPos(hwnd, -2, rect.Left, rect.Top, (int)Width, (int)Height, 0);
            }
        }

        #region P/Invoke imports & definitions  

        /// <summary>  
        /// Represents a rectangle structure used in Windows API calls.  
        /// </summary>  
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the specified window.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpRect"></param>
        /// <returns></returns>
        [LibraryImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static partial bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        /// <summary>
        /// Sets the window position.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hwndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="wFlags"></param>
        /// <returns></returns>
        [LibraryImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        private static partial int SetWindowPos(IntPtr hWnd, int hwndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        #endregion
    }
}
