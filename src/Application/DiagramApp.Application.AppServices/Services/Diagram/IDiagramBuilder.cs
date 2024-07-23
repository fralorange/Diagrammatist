using DiagramApp.Domain.Diagram;
using DiagramEntity = DiagramApp.Domain.Diagram.Diagram;

namespace DiagramApp.Application.AppServices.Services.Diagram
{
    public interface IDiagramBuilder
    {
        void Reset();
        void SetHead(Component component);
        void AddObject(Component component, out Component? head);
        void RemoveObject(Component component);
        void AddConnection(Component component);
        void RemoveConnection(Connection connection);
        DiagramEntity Build();
    }
}
