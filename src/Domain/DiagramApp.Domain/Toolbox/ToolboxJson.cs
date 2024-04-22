namespace DiagramApp.Domain.Toolbox
{
    public class ToolboxJson
    {
        public string Name { get; set; } = string.Empty;
        public ToolboxCategory Category { get; set; } = ToolboxCategory.Shapes;
        public string Data { get; set; } = string.Empty;
    }
}
