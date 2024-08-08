using DiagramApp.Domain.Figures;

namespace DiagramApp.Domain.Toolbox
{
    public class ToolboxItem
    {
        public ToolboxCategory Category { get; set; } = ToolboxCategory.Shapes;
        public required Figure Figure { get; set; }
    }
}
