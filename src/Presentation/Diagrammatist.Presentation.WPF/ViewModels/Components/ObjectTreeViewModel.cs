using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Contracts.Figures;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Framework.Extensions.ObservableCollection;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient
    {
        private readonly IUndoableCommandManager _undoableCommandManager;

        private ObservableCollection<FigureDto>? _figures;

        /// <summary>
        /// Gets collection of <see cref="FigureDto"/>
        /// </summary>
        /// <remarks>
        /// This property used to display figures in UI.
        /// </remarks>
        public ObservableCollection<FigureDto>? Figures
        {
            get => _figures;
            private set => SetProperty(ref _figures, value);
        }

        /// <summary>
        /// Gets selected figure from collection <see cref="Figures"/>
        /// </summary>
        /// <remarks>
        /// This property used to store selected figure.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private FigureDto? _selectedFigure;

        public ObjectTreeViewModel(IUndoableCommandManager undoableCommandManager)
        {
            _undoableCommandManager = undoableCommandManager;

            IsActive = true;
        }

        /// <summary>
        /// Deletes item from object tree.
        /// </summary>
        /// <remarks>
        /// Also deletes item from canvas.
        /// </remarks>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void DeleteItem(FigureDto figure)
        {
            var command = DeleteItemHelper.CreateDeleteItemCommand(Figures, figure);

            _undoableCommandManager.Execute(command);
        }

        /// <summary>
        /// Brings item forward on canvas.
        /// </summary>
        /// <remarks>
        /// Also brings item forward on canvas.
        /// </remarks>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void BringForwardItem(FigureDto figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true, refreshEvent: Figures.Refresh);

            _undoableCommandManager.Execute(command);

            Figures?.Refresh();
        }

        /// <summary>
        /// Sends item backward on canvas.
        /// </summary>
        /// <remarks>
        /// Also sends item backward on canvas.
        /// </remarks>
        /// <param name="figure">Target figure.</param>
        [RelayCommand]
        private void SendBackwardItem(FigureDto figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: false, refreshEvent: Figures.Refresh);

            _undoableCommandManager.Execute(command);

            Figures?.Refresh();
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<ObservableCollection<FigureDto>?>>(this, (r, m) =>
            {
                Figures = m?.NewValue;
            });

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<FigureDto?>>(this, (r, m) =>
            {
                SelectedFigure = m.NewValue;
            });
        }
    }
}
