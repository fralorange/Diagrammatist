namespace DiagramApp.Domain.Toolbox
{
    public class ToolboxItem
    {
        public string Name { get; set; } = string.Empty;
        public ToolboxCategory Category { get; set; } = ToolboxCategory.Shapes;
        public string PathData { get; set; } = string.Empty;
    }
}
