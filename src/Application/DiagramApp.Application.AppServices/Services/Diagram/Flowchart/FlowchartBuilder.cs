using DiagramApp.Domain.Diagram;
using DiagramEntity = DiagramApp.Domain.Diagram.Diagram;
using FlowchartEntity = DiagramApp.Domain.Diagram.Flowchart.Flowchart;

namespace DiagramApp.Application.AppServices.Services.Diagram.Flowchart
{
    public class FlowchartBuilder : IDiagramBuilder
    {
        private DiagramEntity _flowchart = new FlowchartEntity();

        public void Reset()
        {
            _flowchart = new FlowchartEntity();
        }

        public void AddObject(Component component)
        {
            _flowchart.AddObject(component);
        }

        public void RemoveObject(Component component)
        {
            _flowchart.RemoveObject(component);
        }

        public void AddConnection(Component component)
        {
            _flowchart.AddConnection(component);
        }

        public void RemoveConnection(Connection connection)
        {
            _flowchart.RemoveConnection(connection);
        }

        public DiagramEntity Build()
        {
            return _flowchart;
        }
    }
}
