namespace DiagramApp.Domain.Diagram
{
    public class Connection
    {
        public Component PrimaryComp { get; }
        public Component SecondaryComp { get; }

        public Connection(Component primaryComp, Component secondaryComp)
        {
            PrimaryComp = primaryComp;
            SecondaryComp = secondaryComp;
        }
    }
}
