using DiagramApp.Domain.Diagram;
using DiagramEntity = DiagramApp.Domain.Diagram.Diagram;

namespace DiagramApp.Application.AppServices.Services.Diagram
{
    public interface IDiagramBuilder
    {
        void Reset();
        void SetHead(Component component, out bool successFlag);
        void AddObject(Component component, out Component? head);
        void RemoveObject(Component component, out Component? head);
        void AddConnection(Component component);
        void RemoveConnection(Connection connection);
        DiagramEntity Build();
    }
}
