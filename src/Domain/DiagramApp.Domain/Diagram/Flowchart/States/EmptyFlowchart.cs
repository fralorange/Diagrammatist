namespace DiagramApp.Domain.Diagram.Flowchart.States
{
    public class EmptyFlowchart : DiagramState
    {
        public EmptyFlowchart(Diagram diagram, Component? head) : base(diagram, head) { }

        public override void AddObject(Component component)
        {
            if (component is FlowchartComponent { FlowType: FlowchartType.Start } flowchartComponent)
            {
                base.AddObject(flowchartComponent);
                _diagram.ChangeState(new AwaitingConnectionFlowchart(_diagram, _head!));
            }
        }
        public override void RemoveObject(Component component)
        {
            throw new InvalidOperationException(nameof(EmptyFlowchart));
        }

        public override void AddConnection(Component component)
        {
            throw new InvalidOperationException(nameof(EmptyFlowchart));
        }

        public override void RemoveConnection(Connection connection)
        {
            throw new InvalidOperationException(nameof(EmptyFlowchart));
        }
    }
}
