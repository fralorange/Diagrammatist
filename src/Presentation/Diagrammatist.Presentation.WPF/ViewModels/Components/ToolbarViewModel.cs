using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.General;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Managers.Clipboard;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for toolbar component.
    /// </summary>
    public sealed partial class ToolbarViewModel : ObservableRecipient
    {
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly IClipboardManager<FigureModel> _clipboardManager;

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
        private bool _hasSelectedFigure;
        /// <summary>
        /// Gets or sets 'has canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to enable or disable clipboard buttons (paste).
        /// </remarks>
        [ObservableProperty]
        private bool _hasCanvas;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentMouseMode"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private MouseMode _currentMouseMode;

        public ToolbarViewModel(ITrackableCommandManager trackableCommandManager, IClipboardManager<FigureModel> clipboardManager)
        {
            _trackableCommandManager = trackableCommandManager;
            _clipboardManager = clipboardManager;

            IsActive = true;
        }

        /// <summary>
        /// Changes mouse mode.
        /// </summary>
        /// <param name="mode">A new mouse mode.</param>
        [RelayCommand]
        private void ChangeMode(string mode)
        {
            CurrentMouseMode = (MouseMode)Enum.Parse(typeof(MouseMode), mode);
        }

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="Paste"]/*'/>
        [RelayCommand]
        private void Paste()
        {
            if (CurrentCanvas is not null && _clipboardManager.PasteFromClipboard() is { } pastedFigure)
            {
                var command = PasteHelper.CreatePasteCommand(
                    CurrentCanvas.Figures,
                    pastedFigure,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure);

                _trackableCommandManager.Execute(command);
            }
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
                    CurrentCanvas!.Figures,
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
                    CurrentCanvas!.Figures,
                    () => SelectedFigure,
                    figure => SelectedFigure = figure,
                    figure => figure.Clone());

                _trackableCommandManager.Execute(command);
            }
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

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
