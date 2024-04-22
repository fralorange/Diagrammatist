using CommunityToolkit.Maui.Layouts;
using DiagramApp.Application.AppServices.Helpers;
using DiagramApp.Client.ViewModels;
using DiagramApp.Client.ViewModels.Wrappers;

namespace DiagramApp.Client.Components.Specific.Properties;

public partial class TextBoxPropertiesView : UniformItemsLayout
{
    public TextBoxPropertiesView()
    {
        InitializeComponent();
    }

    private void OnTextEntryCompleted(object sender, EventArgs e) 
        => PropertiesView.HandleEntryCompleted(BindingContext, sender, "Text", null);

    private void OnTextSizeEntryCompleted(object sender, EventArgs e) 
        => PropertiesView.HandleEntryCompleted(BindingContext, sender, "FontSize", value => Convert.ToDouble(value));

    private void OnOutlineCheckboxChanged(object sender, CheckedChangedEventArgs e)
        => HandleCheckboxCompleted("HasOutline", e);

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        => HandleCheckboxCompleted("HasBackground", e);

    private void HandleCheckboxCompleted(string propertyName, CheckedChangedEventArgs e)
    {
        if (BindingContext is MainViewModel { CurrentCanvas: { SelectedFigure: ObservableTextFigure figure } canvas })
        {
            var oldText = PropertiesView.GetPropertyValue(figure, propertyName);
            if (oldText is bool oldBool && oldBool == e.Value || oldText is null) return;
            var newText = e.Value;

            var action = new Action(() => PropertiesView.SetPropertyValue(figure, propertyName, newText));
            var undoAction = new Action(() => PropertiesView.SetPropertyValue(figure, propertyName, oldText));

            UndoableCommandHelper.ExecuteAction(canvas, action, undoAction);
        }
    }
}