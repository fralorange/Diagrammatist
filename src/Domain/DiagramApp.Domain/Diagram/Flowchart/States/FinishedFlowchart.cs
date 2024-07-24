namespace DiagramApp.Domain.Diagram.Flowchart.States
{
    public class FinishedFlowchart : DiagramState
    {
        public FinishedFlowchart(Diagram diagram, Component head) : base(diagram, head) { }

        public override void AddObject(Component component)
        {
            throw new InvalidOperationException(nameof(FinishedFlowchart));
        }

        public override void RemoveObject(Component component)
        {
            if (component is FlowchartComponent { FlowType: FlowchartType.End } flowchartComponent)
            {
                base.RemoveObject(flowchartComponent);
                _diagram.ChangeState(new AwaitingConnectionFlowchart(_diagram, null));
            }
        }

        public override void AddConnection(Component component)
        {
            throw new InvalidOperationException(nameof(FinishedFlowchart));
        }

        public override void RemoveConnection(Connection connection)
        {
            throw new InvalidOperationException(nameof(FinishedFlowchart));
        }
    }
}
