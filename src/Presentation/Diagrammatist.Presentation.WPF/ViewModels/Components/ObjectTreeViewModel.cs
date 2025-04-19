using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient
    {
        private readonly IFigureManipulationService _figureManipulationService;

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

        public ObjectTreeViewModel(IFigureManipulationService figureManipulationService)
        {
            _figureManipulationService = figureManipulationService;

            IsActive = true;
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="DeleteItem"]/*'/>
        [RelayCommand]
        private void DeleteItem(FigureModel figure)
        {
            if (Figures is null || Connections is null)
                return;

            _figureManipulationService.Delete(figure, Figures, Connections);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Copy"]/*'/>
        [RelayCommand]
        private void Copy()
        {
            if (SelectedFigure is not null && Figures is not null)
            {
                _figureManipulationService.Copy(SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Cut"]/*'/>
        [RelayCommand]
        private void Cut()
        {
            if (SelectedFigure is not null && Figures is not null)
            {
                _figureManipulationService.Cut(SelectedFigure, Figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Duplicate"]/*'/>
        [RelayCommand]
        private void Duplicate()
        {
            if (SelectedFigure is not null && Figures is not null)
            {
                _figureManipulationService.Duplicate(SelectedFigure, Figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="BringForwardItem"]/*'/>
        [RelayCommand]
        private void BringForwardItem(FigureModel figure)
        {
            _figureManipulationService.BringForward(figure);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="SendBackwardItem"]/*'/>
        [RelayCommand]
        private void SendBackwardItem(FigureModel figure)
        {
            _figureManipulationService.SendBackward(figure);
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
