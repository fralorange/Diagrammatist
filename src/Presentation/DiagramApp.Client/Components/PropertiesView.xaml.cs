using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Client.ViewModels;

namespace DiagramApp.Client.Components;
// if performance problems occur cuz of reflection then get rid of it
public partial class PropertiesView : Frame
{
    public PropertiesView()
    {
        InitializeComponent();
    }

    private void OnNameEntryCompleted(object sender, EventArgs e)
    {
        HandleEntryCompleted(BindingContext, sender, "Name", null);
    }

    private void OnRotationEntryCompleted(object sender, EventArgs e)
    {   //move display alert to service or smth
        if (RotationValidator.IsNotValid)
            return;
        HandleEntryCompleted(BindingContext, sender, "Rotation", value => Convert.ToDouble(value));
    }

    public static void HandleEntryCompleted(object BindingContext, object sender, string propertyName, Func<string, object>? conversionDelegate)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { SelectedFigure: { } figure } canvas } && sender is Entry entry)
        {
            var oldText = GetPropertyValue(figure, propertyName);
            if (oldText is null) return;

            var newText = conversionDelegate != null ? conversionDelegate(entry.Text) : entry.Text;

            var action = new Action(() => SetPropertyValue(figure, propertyName, newText));
            var undoAction = new Action(() => SetPropertyValue(figure, propertyName, oldText));

            UndoableCommandHelper.ExecuteAction(canvas, action, undoAction);
        }
    }

    public static object? GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
    }

    public static void SetPropertyValue(object obj, string propertyName, object value)
    {
        obj.GetType().GetProperty(propertyName)?.SetValue(obj, value);
    }
}