using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Collections.ObjectModel;
using Diagrammatist.Presentation.WPF.Core.Services.Clipboard;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly IClipboardService<FigureModel> _clipboardManager;
        private readonly IConnectionService _connectionService;

        private ObservableCollection<FigureModel>? _figures;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="ViewModelFigures"]/*'/>
        /// <remarks>
        /// This property used to display figures in UI.
        /// </remarks>
        public ObservableCollection<FigureModel>? Figures
        {
            get => _figures;
            private set => SetProperty(ref _figures, value);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="ViewModelConnections"]/*'/>
        /// <remarks>
        /// This property used to provide logic for connections in UI.
        /// </remarks>e
        private ObservableCollection<ConnectionModel>? _connections;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ConnectionModel>? Connections
        {
            get => _connections;
            private set => SetProperty(ref _connections, value);
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

        public ObjectTreeViewModel(ITrackableCommandManager trackableCommandManager, IClipboardService<FigureModel> clipboardManager, IConnectionService connectionManager)
        {
            _trackableCommandManager = trackableCommandManager;
            _clipboardManager = clipboardManager;
            _connectionService = connectionManager;

            IsActive = true;
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="DeleteItem"]/*'/>
        [RelayCommand]
        private void DeleteItem(FigureModel figure)
        {
            if (Figures is null || Connections is null)
                return;

            var command = DeleteItemHelper.CreateDeleteItemCommand(Figures, figure, _connectionService, Connections);

            _trackableCommandManager.Execute(command);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Copy"]/*'/>
        [RelayCommand]
        private void Copy()
        {
            if (SelectedFigure is not null)
            {
                CopyHelper.Copy(_clipboardManager, SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Cut"]/*'/>
        [RelayCommand]
        private void Cut()
        {
            if (SelectedFigure is not null)
            {
                var command = CutHelper.CreateCutCommand(
                    _clipboardManager,
                    Figures!,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure);

                _trackableCommandManager.Execute(command);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Duplicate"]/*'/>
        [RelayCommand]
        private void Duplicate()
        {
            if (SelectedFigure is not null)
            {
                var command = DuplicateHelper.CreateDuplicateCommand(
                    Figures!,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure,
                    figure => figure.Clone());

                _trackableCommandManager.Execute(command);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="BringForwardItem"]/*'/>
        [RelayCommand]
        private void BringForwardItem(FigureModel figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: true);

            _trackableCommandManager.Execute(command);

            Figures?.Refresh();
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="SendBackwardItem"]/*'/>
        [RelayCommand]
        private void SendBackwardItem(FigureModel figure)
        {
            if (Figures is null)
                return;

            var command = ZIndexAdjustmentHelper.CreateZIndexAdjustmentCommand(figure, forward: false);

            _trackableCommandManager.Execute(command);

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

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<ObservableCollection<ConnectionModel>?>>(this, (r, m) =>
            {
                Connections = m?.NewValue;
            });

            Messenger.Register<ObjectTreeViewModel, PropertyChangedMessage<FigureModel?>>(this, (r, m) =>
            {
                SelectedFigure = m.NewValue;
            });
        }
    }
}
