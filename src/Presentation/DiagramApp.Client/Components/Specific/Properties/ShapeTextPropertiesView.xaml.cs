namespace DiagramApp.Client.Components.Specific.Properties;

public partial class ShapeTextPropertiesView : ShapePropertiesView
{
    public ShapeTextPropertiesView()
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
}