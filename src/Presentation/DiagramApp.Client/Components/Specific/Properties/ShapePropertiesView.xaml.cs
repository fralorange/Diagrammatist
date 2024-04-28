using CommunityToolkit.Maui.Layouts;

namespace DiagramApp.Client.Components.Specific.Properties;

public partial class ShapePropertiesView : UniformItemsLayout
{
	public ShapePropertiesView()
	{
		InitializeComponent();
	}

    private void OnWidthEntryCompleted(object sender, EventArgs e)
	{
		if (ShapeWidthValidator.IsNotValid)
			return;
        PropertiesView.HandleEntryCompleted(BindingContext, sender, "Width", value => Convert.ToDouble(value));
    }

    private void OnHeightEntryCompleted(object sender, EventArgs e)
    {
        if (ShapeHeightValidator.IsNotValid)
            return;
        PropertiesView.HandleEntryCompleted(BindingContext, sender, "Height", value => Convert.ToDouble(value));
    }
}