using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Framework.Extensions.ObservableCollection;
using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient
    {
        private readonly IUndoableCommandManager _undoableCommandManager;

        private ObservableCollection<FigureModel>? _figures;

        /// <summary>
        /// Gets collection of <see cref="FigureModel"/>
        /// </summary>
        /// <remarks>
        /// This property used to display figures in UI.
        /// </remarks>
        public ObservableCollection<FigureModel>? Figures
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
        private FigureModel? _selectedFigure;

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
        private void DeleteItem(FigureModel figure)
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
        private void BringForwardItem(FigureModel figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true);

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
        private void SendBackwardItem(FigureModel figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: false);

            _undoableCommandManager.Execute(command);

            Figures?.Refresh();
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<ObservableCollection<FigureModel>?>>(this, (r, m) =>
            {
                Figures = m?.NewValue;
            });

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<FigureModel?>>(this, (r, m) =>
            {
                SelectedFigure = m.NewValue;
            });
        }
    }
}
