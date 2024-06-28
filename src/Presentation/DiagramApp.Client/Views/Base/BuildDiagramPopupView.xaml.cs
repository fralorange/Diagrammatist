using CommunityToolkit.Maui.Views;

namespace DiagramApp.Client.Views.Base;

public partial class BuildDiagramPopupView : Popup
{
    public BuildDiagramPopupView()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();

    public void InsertFirstChild(View child)
    {
        ToolsWithBorderLayout.Insert(0, child);
    }
}