using CommunityToolkit.Maui.Views;

namespace DiagramApp.Client.Views.Base;

public partial class BuildDiagramPopupView : Popup
{
    public BuildDiagramPopupView()
    {
        InitializeComponent();
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();
    private void OnPreviewContentChildAdded(object sender, ElementEventArgs e) => (PreviewScroll as IView).InvalidateMeasure();

    public void InsertFirstChild(View child)
    {
        ToolsWithBorderLayout.Insert(0, child);
    }

    public void SetupPreview<T>(DataTemplate dataTemplate, IEnumerable<T> source)
    {
        BindableLayout.SetItemsSource(PreviewContent, source);
        BindableLayout.SetItemTemplate(PreviewContent, dataTemplate);
    }
}