
using DiagramApp.Client.ViewModels.Wrappers;

namespace DiagramApp.Client.Components
{
    public class FigureDataTemplateSelector : ToolboxItemTemplateSelector
    {
        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            return item switch
            {
                ObservablePathFigure => PathFigureTemplate,
                ObservablePolylineFigure => PolylineFigureTemplate,
                ObservableTextFigure => TextFigureTemplate,
                _ => null,
            };
        }
    }
}
