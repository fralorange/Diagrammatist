namespace DiagramApp.Domain.Diagram
{
    public abstract class Diagram
    {
        protected DiagramState? _state;
        public List<Component> Components { get; } = [];
        public List<Connection> Connections { get; } = [];
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public void ChangeState(DiagramState? state) => _state = state;
        public abstract void UpdateSize();
        public abstract void AddObject(Component component);
        public abstract void RemoveObject(Component component);
        public abstract void AddConnection(Component secondaryComp);
        public abstract void RemoveConnection(Connection connection);
    }
}
