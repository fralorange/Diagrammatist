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

        public void SetHead(Component head, out bool successFlag)
        {
            successFlag = _flowchart.SetHead(head);
        }

        public void AddObject(Component component, out Component? head)
        {
            _flowchart.AddObject(component, out head);
            subscribers.ForEach(sub => sub.UpdateComponents(_flowchart.Components));
        }

        public void RemoveObject(Component component, out Component? head)
        {
            _flowchart.RemoveObject(component, out head);
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
