namespace DiagramApp.Domain.Diagram
{
    public abstract class DiagramState
    {
        public Component? Head { get; private set; }
        protected readonly Diagram _diagram;

        public DiagramState(Diagram diagram, Component? head)
        {
            _diagram = diagram;
            Head = head;
        }

        public void SetHead(Component head)
        {
            Head = head;
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
