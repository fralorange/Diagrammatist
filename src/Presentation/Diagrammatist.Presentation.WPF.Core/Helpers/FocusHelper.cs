using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps with selection and focus.
    /// </summary>
    public static class FocusHelper
    {
        /// <summary>
        /// Clears focus.
        /// </summary>
        /// <param name="focus">New focus, if exists.</param>
        public static void ClearFocus(IInputElement? focus = null)
        {
            if (focus is null)
            {
                Keyboard.ClearFocus();
            }
            else
            {
                Keyboard.Focus(focus);
            }
        }

        /// <summary>
        /// Clears selection.
        /// </summary>
        /// <param name="listBox"></param>
        public static void ClearSelection(ListBox listBox)
        {
            listBox.UnselectAll();
        }

        /// <summary>
        /// Clears both focus and selection.
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="focus">New focus.</param>
        public static void ClearFocusAndSelection(ListBox listBox, IInputElement? focus = null)
        {
            ClearFocus(focus);
            ClearSelection(listBox);
        }
    }
}
