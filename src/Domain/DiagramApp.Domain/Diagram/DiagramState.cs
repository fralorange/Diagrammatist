namespace DiagramApp.Domain.Diagram
{
    public abstract class DiagramState
    {
        protected Diagram _diagram;
        protected Component? _head;

        public DiagramState(Diagram diagram, Component? head)
        {
            _diagram = diagram;
            _head = head;
        }

        public virtual void AddObject(Component component)
        {
            _diagram.Components.Add(component);
            _head = component;
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
