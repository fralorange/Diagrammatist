using Microsoft.UI.Input;
using System.Reflection;
using UIElementType = Microsoft.UI.Xaml.UIElement;

namespace DiagramApp.Client.Extensions.UIElement
{
    public static class UIElementExtensions
    {
        public static void ChangeCursor(this UIElementType uiElement, InputCursor input)
        {
            Type type = typeof(UIElementType);
            type.InvokeMember("ProtectedCursor", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance, null, uiElement, new object[] { input });
        }
    }
}
