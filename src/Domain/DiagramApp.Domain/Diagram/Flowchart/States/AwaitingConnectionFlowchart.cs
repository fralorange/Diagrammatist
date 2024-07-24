namespace DiagramApp.Domain.Diagram.Flowchart.States
{
    public class AwaitingConnectionFlowchart : DiagramState
    {
        public AwaitingConnectionFlowchart(Diagram diagram, Component? head) : base(diagram, head) { }

        public override void AddObject(Component component)
        {
            if (component is not FlowchartComponent flowchartComponent || component is FlowchartComponent { FlowType: FlowchartType.Start } || Head is null) 
                return;
            if (flowchartComponent.FlowType == FlowchartType.End)
                _diagram.ChangeState(new FinishedFlowchart(_diagram, Head!));

            var oldObject = Head;
            base.AddObject(flowchartComponent);  
            
            AddConnection(oldObject);
        }

        public override void RemoveObject(Component component)
        {
            if (component is not FlowchartComponent flowchartComponent || component is FlowchartComponent { FlowType: FlowchartType.Start }) return;
            if (Head == flowchartComponent)
                Head = null;
            base.RemoveObject(flowchartComponent);
        }

        public override void AddConnection(Component component)
        {
            if (component is not FlowchartComponent primaryFlowComp || Head is not FlowchartComponent secondaryFlowComp) 
                return;
            if (primaryFlowComp.FlowType == FlowchartType.Decision && _diagram.Connections.Count(conn => conn.PrimaryComp == primaryFlowComp) < 2)
            {
                var hasLeftConnection = _diagram.Connections.Any(conn => conn.PrimaryComp == primaryFlowComp && conn.SecondaryComp.XPos < primaryFlowComp.XPos);
                var hasRightConnection = _diagram.Connections.Any(conn => conn.PrimaryComp == primaryFlowComp && conn.SecondaryComp.XPos > primaryFlowComp.XPos);

                secondaryFlowComp.XPos = hasLeftConnection ? primaryFlowComp.XPos + 100 : primaryFlowComp.XPos - 100;
                if (!(hasLeftConnection || hasRightConnection)) _diagram.ChangeState(new AwaitingConnectionFlowchart(_diagram, primaryFlowComp));
            }

            _diagram.Connections.Add(new(primaryFlowComp, secondaryFlowComp));
        }

        public override void RemoveConnection(Connection connection)
        {
            _diagram.Connections.Remove(connection);
        }
    }
}
