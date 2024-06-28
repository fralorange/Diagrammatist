namespace DiagramApp.Domain.Diagram.Flowchart.States
{
    public class AwaitingConnectionFlowchart : DiagramState
    {
        public AwaitingConnectionFlowchart(Diagram diagram, Component head) : base(diagram, head) { }

        public override void AddObject(Component component)
        {
            if (component is not FlowchartComponent flowchartComponent) return;
            if (flowchartComponent.FlowType == FlowchartType.End)
                _diagram.ChangeState(new FinishedFlowchart(_diagram, _head!));

            var oldObject = _head!;
            base.AddObject(flowchartComponent);  
            
            AddConnection(oldObject);
        }

        public override void RemoveObject(Component component)
        {
            if (component is not FlowchartComponent flowchartComponent || component is FlowchartComponent { FlowType: FlowchartType.Start }) return;
            
            base.RemoveObject(flowchartComponent);
        }

        public override void AddConnection(Component component)
        {
            if (component is not FlowchartComponent primaryFlowComp || _head is not FlowchartComponent secondaryFlowComp) return;
            if (primaryFlowComp.FlowType == FlowchartType.Decision && !_diagram.Connections.Any(conn => conn.PrimaryComp.Equals(primaryFlowComp))) 
                _diagram.ChangeState(new AwaitingConnectionFlowchart(_diagram, primaryFlowComp));

            _diagram.Connections.Add(new(primaryFlowComp, secondaryFlowComp));
        }

        public override void RemoveConnection(Connection connection)
        {
            _diagram.Connections.Remove(connection);
        }
    }
}
