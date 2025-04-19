using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation
{
    /// <summary>
    /// A class that implements <see cref="IFigureManipulationService"/>.
    /// </summary>
    public class FigureManipulationService : IFigureManipulationService
    {
        private readonly ITrackableCommandManager _commandManager;
        private readonly IClipboardService<FigureModel> _clipboardService;
        private readonly IConnectionService _connectionService;

        public FigureManipulationService(ITrackableCommandManager commandManager,
                                         IClipboardService<FigureModel> clipboardService,
                                         IConnectionService connectionService)
        {
            _commandManager = commandManager;
            _clipboardService = clipboardService;
            _connectionService = connectionService;
        }

        /// <inheritdoc/>
        public void Copy(FigureModel figure)
        {
            _clipboardService.CopyToClipboard(figure);
        }

        /// <inheritdoc/>
        public void Cut(FigureModel figure, ICollection<FigureModel> figures)
        {
            _clipboardService.CopyToClipboard(figure);

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () => figures.Remove(figure),
                () => figures.Add(figure));

            _commandManager.Execute(command);
        }

        /// <inheritdoc/>
        public void Duplicate(FigureModel figure, ICollection<FigureModel> figures)
        {
            var clone = figure.Clone();

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () => figures.Add(clone),
                () => figures.Remove(clone));

            _commandManager.Execute(command);
        }

        /// <inheritdoc/>
        public void Paste(ICollection<FigureModel> figures)
        {
            if (_clipboardService.GetFromClipboard() is not { } pastedFigure)
                return;

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () => figures.Add(pastedFigure),
                () => figures.Remove(pastedFigure));

            _commandManager.Execute(command);
        }

        /// <inheritdoc/>
        public void Paste(ICollection<FigureModel> figures, object pos)
        {
            if (_clipboardService.GetFromClipboard() is not { } pastedFigure || pos is not Point point)
                return;

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    figures.Add(pastedFigure);
                    pastedFigure.PosX = point.X;
                    pastedFigure.PosY = point.Y;
                },
                () =>
                {
                    figures.Remove(pastedFigure);
                });

            _commandManager.Execute(command);
        }

        /// <inheritdoc/>
        public void Delete(FigureModel figure, ICollection<FigureModel> figures, ICollection<ConnectionModel> connections)
        {
            var connection = ConnectionHelper.GetConnection(connections, figure, _connectionService);

            var command = CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    figures.Remove(figure);
                    if (connection is not null)
                        _connectionService.RemoveConnection(connections, connection);
                },
                () =>
                {
                    figures.Add(figure);
                    if (connection is not null)
                        _connectionService.AddConnection(connections, connection);
                }
            );

            _commandManager.Execute(command);
        }

        public void SendBackward(FigureModel figure)
        {
            AdjustZIndex(figure, -1);
        }

        public void BringForward(FigureModel figure)
        {
            AdjustZIndex(figure, 1);
        }

        private void AdjustZIndex(FigureModel figure, int delta, int minZ = 1, int maxZ = 100)
        {
            var command = CommonUndoableHelper.CreateUndoableCommand(
                () =>
                {
                    var newZ = figure.ZIndex + delta;
                    if (newZ >= minZ && newZ <= maxZ)
                        figure.ZIndex = newZ;
                },
                () =>
                {
                    var revertZ = figure.ZIndex - delta;
                    if (revertZ >= minZ && revertZ <= maxZ)
                        figure.ZIndex = revertZ;
                });

            _commandManager.Execute(command);
        }
    }
}
