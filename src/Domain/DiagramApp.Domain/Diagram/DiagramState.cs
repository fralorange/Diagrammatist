using DiagramApp.Domain.Diagram.Flowchart;

namespace DiagramApp.Domain.Diagram
{
    public abstract class DiagramState
    {
        public Component? Head { get; protected set; }
        protected readonly Diagram _diagram;

        public DiagramState(Diagram diagram, Component? head)
        {
            _diagram = diagram;
            Head = head;
        }

        public bool SetHead(Component newHead)
        {
            var connectionCond = !_diagram.Connections.Any(conn => conn.PrimaryComp == newHead);
            var decisionCond = newHead is FlowchartComponent { FlowType: FlowchartType.Decision } comp && (_diagram.Connections.Count(conn => conn.PrimaryComp == comp) < 2);
            
            var flag = connectionCond || decisionCond;
            if (flag)
                Head = newHead;
            return flag;
        }

        public virtual void AddObject(Component component)
        {
            component.XPos = Head?.XPos ?? 0;
            component.YPos = Head?.YPos + 100 ?? 0;

            _diagram.Components.Add(component);
            Head = component;
        }

        public virtual void RemoveObject(Component component)
        {
            _diagram.Components.Remove(component);
            _diagram.Connections.RemoveAll(conn => conn.PrimaryComp.Equals(component) || conn.SecondaryComp.Equals(component));
        }

        public abstract void AddConnection(Component component);
        public abstract void RemoveConnection(Connection connection);
    }
}
