using DiagramApp.Domain.Diagram;
using DiagramEntity = DiagramApp.Domain.Diagram.Diagram;
using FlowchartEntity = DiagramApp.Domain.Diagram.Flowchart.Flowchart;

namespace DiagramApp.Application.AppServices.Services.Diagram.Flowchart
{
    public class FlowchartBuilder : IDiagramBuilder
    {
        private DiagramEntity _flowchart = new FlowchartEntity();
        private List<IDiagramObserver> subscribers = new();

        public void Reset()
        {
            _flowchart = new FlowchartEntity();
        }

        public void AddObject(Component component)
        {
            _flowchart.AddObject(component);
            subscribers.ForEach(sub => sub.UpdateComponents(_flowchart.Components));
        }

        public void RemoveObject(Component component)
        {
            _flowchart.RemoveObject(component);
            subscribers.ForEach(sub => sub.UpdateComponents(_flowchart.Components));
        }

        public void AddConnection(Component component)
        {
            _flowchart.AddConnection(component);
            subscribers.ForEach(sub => sub.UpdateConnections(_flowchart.Connections));
        }

        public void RemoveConnection(Connection connection)
        {
            _flowchart.RemoveConnection(connection);
            subscribers.ForEach(sub => sub.UpdateConnections(_flowchart.Connections));
        }

        public void Subscribe(IDiagramObserver subscriber)
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe(IDiagramObserver subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public DiagramEntity Build()
        {
            return _flowchart;
        }
    }
}
