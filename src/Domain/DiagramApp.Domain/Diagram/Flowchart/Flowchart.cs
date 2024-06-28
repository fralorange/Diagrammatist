using DiagramApp.Domain.Diagram.Flowchart.States;

namespace DiagramApp.Domain.Diagram.Flowchart
{
    public class Flowchart : Diagram
    {
        public Flowchart() => _state = new EmptyFlowchart(this, null);

        public override void UpdateSize()
        {
            if (Components.Count == 0)
            {
                Width = 0;
                Height = 0;
                return;
            }

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (var component in Components) 
            { 
                if (component.XPos < minX)
                {
                    minX = component.XPos;
                }
                if (component.YPos < minY)
                {
                    minY = component.YPos;
                }
                if (component.XPos + component.Width > maxX)
                {
                    maxX = component.XPos + component.Width;
                }
                if (component.YPos + component.Height > maxY)
                {
                    maxY = component.YPos + component.Height;
                }
            }

            Width = maxX - minX;
            Height = maxY - minY;
        }

        public override void AddObject(Component component)
        {
            _state?.AddObject(component);
        }

        public override void RemoveObject(Component component)
        {
            _state?.RemoveObject(component);
        }

        public override void AddConnection(Component component)
        {
            _state?.AddConnection(component);
        }

        public override void RemoveConnection(Connection connection)
        {
            _state?.RemoveConnection(connection);
        }
    }
}
