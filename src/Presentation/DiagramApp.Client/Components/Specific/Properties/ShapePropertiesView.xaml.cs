using CommunityToolkit.Maui.Layouts;

namespace DiagramApp.Client.Components.Specific.Properties;

public partial class ShapePropertiesView : UniformItemsLayout
{
	public ShapePropertiesView()
	{
		InitializeComponent();
	}

    private void OnShapeEntryCompleted(object sender, EventArgs e)
		=> PropertiesView.HandleEntryCompleted(BindingContext, sender, "Size", value => Convert.ToDouble(value));
}