using DiagramApp.Domain.Diagram;

namespace DiagramApp.Application.AppServices.Services.Diagram
{
    public interface IDiagramObserver
    {
        void UpdateComponents(IReadOnlyList<Component> components);
        void UpdateConnections(IReadOnlyList<Connection> connections);
    }
}
