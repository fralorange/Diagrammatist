using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Canvas.Interaction
{
    /// <summary>
    /// A class that implements <see cref="ICanvasInteractionService"/>.
    /// </summary>
    public class CanvasInteractionService : ICanvasInteractionService
    {
        private readonly IConnectionService _connectionService;
        private readonly ITrackableCommandManager _commandManager;

        /// <summary>
        /// Initializes canvas interaction service.
        /// </summary>
        /// <param name="connectionService"></param>
        /// <param name="commandManager"></param>
        public CanvasInteractionService(IConnectionService connectionService, ITrackableCommandManager commandManager)
        {
            _connectionService = connectionService;
            _commandManager = commandManager;
        }

        /// <inheritdoc/>
        public void MoveFigure(FigureModel figure, Point initPos, Point oldPos, Point newPos, ICollection<ConnectionModel> connections)
        {
            if (figure == null)
                return;

            var lineUpdater = GetLineUpdaterIfNeeded(figure, connections);

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    SetFigurePosition(figure, newPos);
                    UpdateConnections(figure, newPos, oldPos, connections);
                    oldPos = initPos;
                    lineUpdater?.Invoke(false);
                },
                () =>
                {
                    SetFigurePosition(figure, initPos);
                    UpdateConnections(figure, initPos, newPos, connections);
                    lineUpdater?.Invoke(true);
                });

            _commandManager.Execute(command);
        }

        /// <inheritdoc/>
        public void MoveFigureVisuals(FigureModel figure, Point oldPos, Point newPos, ICollection<ConnectionModel> connections)
        {
            if (figure is not ShapeFigureModel shapeFigure || connections == null)
                return;

            var deltaX = newPos.X - oldPos.X;
            var deltaY = newPos.Y - oldPos.Y;

            var shapeConnections = _connectionService.GetConnections(connections, shapeFigure);

            foreach (var connection in shapeConnections)
            {
                var source = connection.SourceMagneticPoint;
                var dest = connection.DestinationMagneticPoint;

                var isSource = source?.Owner == shapeFigure;
                var currentPoint = isSource ? source!.Position : dest!.Position;
                var newPoint = new Point(currentPoint.X + deltaX, currentPoint.Y + deltaY);

                if (isSource)
                {
                    source!.Position = newPoint;
                    connection.Line.Points[0] = newPoint;
                }
                else
                {
                    dest!.Position = newPoint;
                    connection.Line.Points[^1] = newPoint;
                }
            }
        }

        private void SetFigurePosition(FigureModel figure, Point pos)
        {
            figure.PosX = pos.X;
            figure.PosY = pos.Y;
        }

        private void UpdateConnections(FigureModel figure, Point newPos, Point oldPos, ICollection<ConnectionModel> connections)
        {
            if (figure is not ShapeFigureModel shapeFigure)
                return;

            var shapeConnections = _connectionService.GetConnections(connections, shapeFigure);

            foreach (var connection in shapeConnections)
            {
                var deltaX = newPos.X - oldPos.X;
                var deltaY = newPos.Y - oldPos.Y;

                var source = connection.SourceMagneticPoint;
                var dest = connection.DestinationMagneticPoint;

                var isSource = source?.Owner == shapeFigure;
                var currentPoint = isSource ? source!.Position : dest!.Position;
                var nextPos = new Point(currentPoint.X + deltaX, currentPoint.Y + deltaY);

                if (isSource)
                {
                    source!.Position = nextPos;
                    connection.Line.Points[0] = nextPos;
                }
                else
                {
                    dest!.Position = nextPos;
                    connection.Line.Points[^1] = nextPos;
                }
            }
        }

        private Action<bool>? GetLineUpdaterIfNeeded(FigureModel figure, ICollection<ConnectionModel> connections)
        {
            if (figure is not LineFigureModel lineFigure)
                return null;

            var lineConnections = _connectionService.GetConnection(connections, lineFigure);

            if (lineConnections is null)
                return null;

            return (revert) =>
            {
                if (revert)
                    _connectionService.AddConnection(connections, lineConnections);
                else
                    _connectionService.RemoveConnection(connections, lineConnections);
            };
        }
    }
}
