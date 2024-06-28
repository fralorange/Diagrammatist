namespace DiagramApp.Domain.Diagram.Flowchart
{
    public class FlowchartComponent : Component
    {
        public FlowchartType FlowType { get; init; }
        public string Text { get; set; } = string.Empty;
    }
}
