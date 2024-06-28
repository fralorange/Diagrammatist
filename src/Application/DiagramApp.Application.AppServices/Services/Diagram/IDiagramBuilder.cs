using DiagramApp.Domain.Diagram;
using DiagramEntity = DiagramApp.Domain.Diagram.Diagram;

namespace DiagramApp.Application.AppServices.Services.Diagram
{
    public interface IDiagramBuilder
    {
        void Reset();
        void AddObject(Component component);
        void RemoveObject(Component component);
        void AddConnection(Component component);
        void RemoveConnection(Connection connection);
        DiagramEntity Build();
    }
}
