using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Domain.Toolbox
{
    public class ToolboxItem
    {
        public ToolboxCategory Category { get; set; } = ToolboxCategory.Shapes;
        public required Figure Figure { get; set; }
    }
}
