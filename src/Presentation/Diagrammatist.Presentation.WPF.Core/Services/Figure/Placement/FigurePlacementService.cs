using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;

namespace Diagrammatist.Presentation.WPF.Core.Services.Figure.Placement
{
    /// <summary>
    /// A class that implements <see cref="IFigurePlacementService"/>.
    /// </summary>
    public class FigurePlacementService : IFigurePlacementService
    {
        private readonly ITrackableCommandManager _commandManager;
        private readonly IConnectionService _connectionService;

        private bool _change;

        /// <summary>
        /// Initializes figure placement service.
        /// </summary>
        /// <param name="commandManager"></param>
        /// <param name="connectionService"></param>
        public FigurePlacementService(ITrackableCommandManager commandManager, IConnectionService connectionService)
        {
            _commandManager = commandManager;
            _connectionService = connectionService;
        }

        public void AddFigureWithUndo(FigureModel figure, ICollection<FigureModel> figures, ICollection<ConnectionModel> connections)
        {
            TrackChanges(figure);

            var connection = ConnectionHelper.GetConnection(connections, figure, _connectionService);

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    figures.Add(figure);
                    if (connection is not null && !connections.Contains(connection))
                        _connectionService.AddConnection(connections, connection);
                },
                () =>
                {
                    figures.Remove(figure);
                    if (connection is not null && connections.Contains(connection))
                        _connectionService.RemoveConnection(connections, connection);
                });

            _commandManager.Execute(command);
        }

        public void TrackChanges(FigureModel figure)
        {
            figure.ExtendedPropertyChanged += (sender, e) =>
            {
                if (sender is not FigureModel f || e.PropertyName is null || _change)
                    return;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => SetFigureProperty(f, e.PropertyName, e.NewValue),
                    () => SetFigureProperty(f, e.PropertyName, e.OldValue));

                _commandManager.Execute(command);
            };
        }

        private void SetFigureProperty(FigureModel figure, string propertyName, object? value)
        {
            _change = true;
            try
            {
                var property = figure.GetType().GetProperty(propertyName);
                property?.SetValue(figure, value);
            }
            finally
            {
                _change = false;
            }
        }
    }
}
