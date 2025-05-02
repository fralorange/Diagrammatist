using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Manipulation;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for toolbar component.
    /// </summary>
    public sealed partial class ToolbarViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly IFigureManipulationService _figureManipulationService;

        private CanvasModel? _currentCanvas;
        private CanvasModel? CurrentCanvas
        {
            get => _currentCanvas;
            set => SetProperty(ref _currentCanvas, value);
        }

        private FigureModel? _selectedFigure;
        private FigureModel? SelectedFigure
        {
            get => _selectedFigure;
            set => SetProperty(ref _selectedFigure, value, true);
        }

        /// <summary>
        /// Gets or sets 'has selected figure' flag.
        /// </summary>
        /// <remarks>
        /// This property used to enable or disable clipboard buttons (copy, cut and duplicate).
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(
            nameof(CopyCommand),
            nameof(CutCommand),
            nameof(DuplicateCommand))]
        private bool _hasSelectedFigure;
        /// <summary>
        /// Gets or sets 'has canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to enable or disable clipboard buttons (paste).
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(PasteCommand))]
        private bool _hasCanvas;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentMouseMode"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        [NotifyCanExecuteChangedFor(nameof(ChangeModeCommand))]
        private MouseMode _currentMouseMode;

        public ToolbarViewModel(ITrackableCommandManager trackableCommandManager, IFigureManipulationService figureManipulationService)
        {
            _trackableCommandManager = trackableCommandManager;
            _figureManipulationService = figureManipulationService;

            IsActive = true;
        }

        #region Commands Can Execute

        private bool CanvasCanExecute()
        {
            return HasCanvas;
        }

        private bool FigureCanExecute()
        {
            return HasSelectedFigure;
        }

        private bool LineModeCanExecute()
        {
            return CurrentMouseMode is not MouseMode.Line;
        }

        #endregion

        #region Commands

        /// <summary>
        /// Changes mouse mode.
        /// </summary>
        /// <param name="mode">A new mouse mode.</param>
        [RelayCommand(CanExecute = nameof(LineModeCanExecute))]
        private void ChangeMode(string mode)
        {
            CurrentMouseMode = MouseModeHelper.GetParsedMode<MouseMode>(mode);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Paste"]/*'/>
        [RelayCommand(CanExecute = nameof(CanvasCanExecute))]
        private void Paste()
        {
            if (CurrentCanvas?.Figures is { } figures)
            {
                _figureManipulationService.Paste(figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Copy"]/*'/>
        [RelayCommand(CanExecute = nameof(FigureCanExecute))]
        private void Copy()
        {
            if (SelectedFigure is not null)
            {
                _figureManipulationService.Copy(SelectedFigure);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Cut"]/*'/>
        [RelayCommand(CanExecute = nameof(FigureCanExecute))]
        private void Cut()
        {
            if (SelectedFigure is not null && CurrentCanvas?.Figures is { } figures)
            {
                _figureManipulationService.Cut(SelectedFigure, figures);
            }
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Duplicate"]/*'/>
        [RelayCommand(CanExecute = nameof(FigureCanExecute))]
        private void Duplicate()
        {
            if (SelectedFigure is not null && CurrentCanvas?.Figures is { } figures)
            {
                _figureManipulationService.Duplicate(SelectedFigure, figures);
            }
        }

        #endregion

        /// <summary>
        /// Changes parameters that depend on specific mouse mode.
        /// </summary>
        /// <param name="oldValue">Old mouse mode.</param>
        /// <param name="newValue">New mouse mode.</param>
        partial void OnCurrentMouseModeChanged(MouseMode oldValue, MouseMode newValue)
        {
            if (newValue is MouseMode.Line)
            {
                Messenger.Send(new Tuple<string, bool>(ActionFlags.IsBlocked, true));
            }
            else if (oldValue is MouseMode.Line)
            {
                Messenger.Send(new Tuple<string, bool>(ActionFlags.IsBlocked, false));
            }
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<ToolbarViewModel, PropertyChangedMessage<MouseMode>>(this, (r, m) =>
            {
                CurrentMouseMode = m.NewValue;
            });

            Messenger.Register<ToolbarViewModel, PropertyChangedMessage<CanvasModel?>>(this, (r, m) =>
            {
                CurrentCanvas = m.NewValue;
                HasCanvas = CurrentCanvas is not null;
            });

            Messenger.Register<ToolbarViewModel, PropertyChangedMessage<FigureModel?>>(this, (r, m) =>
            {
                SelectedFigure = m.NewValue;
                HasSelectedFigure = SelectedFigure is not null;
            });
        }
    }
}
