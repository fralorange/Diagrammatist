using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Figures;
using System.Collections.ObjectModel;

namespace DiagramApp.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for object tree (explorer) component.
    /// </summary>
    public sealed partial class ObjectTreeViewModel : ObservableRecipient
    {
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

        public ObjectTreeViewModel()
        {
            IsActive = true;
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
