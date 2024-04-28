using CommunityToolkit.Maui.Layouts;

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
    {
        if (TextSizeValidator.IsNotValid)
            return;
        PropertiesView.HandleEntryCompleted(BindingContext, sender, "FontSize", value => Convert.ToDouble(value));
    }

    private void OnOutlineCheckboxChanged(object sender, CheckedChangedEventArgs e)
        => PropertiesView.HandleCheckboxCompleted(BindingContext, "HasOutline", e);

    private void OnBackgroundCheckboxChanged(object sender, CheckedChangedEventArgs e)
        => PropertiesView.HandleCheckboxCompleted(BindingContext, "HasBackground", e);
}