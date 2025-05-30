﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Document.Services;
using Diagrammatist.Domain.Figures;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Managers.Command;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Services.Formatting;
using Diagrammatist.Presentation.WPF.Core.Services.Settings;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.Simulator.Models.Context;
using Diagrammatist.Presentation.WPF.Simulator.ViewModels;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using Diagrammatist.Presentation.WPF.ViewModels.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Configuration;

namespace Diagrammatist.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for main window.
    /// </summary>
    /// <remarks>
    /// This view model used to process menu and system commands.
    /// <para>
    /// Also used as mediator between menu options and view models.
    /// </para>
    /// </remarks>
    public sealed partial class MainViewModel : ObservableRecipient
    {
        private readonly IDialogService _dialogService;
        private readonly ITrackableCommandManager _trackableCommandManager;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Occurs when a request is made to close the current canvas.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user initiates a close action from menu button.
        /// </remarks>
        public event Action? OnRequestClose;

        #region MenuFlags
        /// <inheritdoc cref="ITrackableCommandManager.HasGlobalChanges"/>
        public bool HasGlobalChangesFlag => _trackableCommandManager.HasGlobalChanges;
        /// <inheritdoc cref="ITrackableCommandManager.HasChanges"/>
        public bool HasChangesFlag => _trackableCommandManager.HasChanges;
        /// <inheritdoc cref="IUndoableCommandManager.CanUndo"/>
        public bool HasUndoFlag => _trackableCommandManager.CanUndo;
        /// <inheritdoc cref="IUndoableCommandManager.CanRedo"/>
        public bool HasRedoFlag => _trackableCommandManager.CanRedo;
        /// <summary>
        /// Gets or sets 'has canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether app has a canvas on the screen right now or not.
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(
            nameof(MenuCloseCommand),
            nameof(MenuCloseAllCommand),
            nameof(MenuSaveAsCommand),
            nameof(MenuExportCommand),
            nameof(MenuZoomInCommand),
            nameof(MenuZoomOutCommand),
            nameof(MenuZoomResetCommand),
            nameof(MenuEnableGridCommand),
            nameof(MenuSimulatorCommand),
            nameof(MenuChangeSizeCommand),
            nameof(MenuChangeBackgroundCommand),
            nameof(MenuChangeTypeCommand),
            nameof(MenuAdaptToThemeCommand),
            nameof(MenuFormatCommand))]
        private bool _hasCanvasFlag;
        /// <summary>
        /// Gets or sets 'has custom canvas' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether app current canvas is custom or not.
        /// </remarks>
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(MenuSimulatorCommand))]
        private bool _hasCustomCanvasFlag;
        /// <summary>
        /// Gets 'has grid flag' from client-prefs (be default: True)
        /// </summary>
        /// <remarks>
        /// This property used to show checkmarks in front of grid menu item, for visual sake..
        /// </remarks>
        [ObservableProperty]
        private bool _hasGridFlag = Properties.Settings.Default.GridVisible;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsBlocked"]/*'/>
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBlocked))]
        [NotifyCanExecuteChangedFor(
            nameof(MenuNewCommand),
            nameof(MenuOpenCommand),
            nameof(MenuCloseCommand),
            nameof(MenuCloseAllCommand),
            nameof(MenuSaveCommand),
            nameof(MenuSaveAsCommand),
            nameof(MenuSaveAllCommand),
            nameof(MenuExportCommand),
            nameof(MenuExitCommand),
            nameof(MenuUndoCommand),
            nameof(MenuRedoCommand),
            nameof(MenuZoomInCommand),
            nameof(MenuZoomOutCommand),
            nameof(MenuZoomResetCommand),
            nameof(MenuSimulatorCommand),
            nameof(MenuChangeSizeCommand),
            nameof(MenuChangeBackgroundCommand),
            nameof(MenuChangeTypeCommand),
            nameof(MenuAdaptToThemeCommand),
            nameof(MenuFormatCommand))]
        private bool _isBlocked;
        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="IsNotBlocked"]/*'/>
        public bool IsNotBlocked => !IsBlocked;
        #endregion

        /// <summary>
        /// Gets formatters collection.
        /// </summary>
        public ObservableCollection<IFormatter>? Formatters { get; private set; }

        /// <summary>
        /// Gets formatter names collection.
        /// </summary>
        public ObservableCollection<string>? FormatterNames { get; private set; }

        /// <summary>
        /// Initializes main view model.
        /// </summary>
        /// <remarks>
        /// This view model acts as adviser between all components..
        /// </remarks>
        /// <param name="alertService"></param>
        /// <param name="dialogService"></param>
        /// <param name="trackableCommandManager"></param>
        public MainViewModel(IDialogService dialogService,
                             ITrackableCommandManager trackableCommandManager,
                             IServiceProvider serviceProvider)
        {
            _dialogService = dialogService;
            _trackableCommandManager = trackableCommandManager;
            _serviceProvider = serviceProvider;

            ConfigureEvents();
            ConfigureFormatters();

            IsActive = true;
        }

        private void ConfigureEvents()
        {
            _trackableCommandManager.StateChanged += OnStateChanged;
            _trackableCommandManager.OperationPerformed += OnOperationPerformed;

            Properties.Settings.Default.SettingChanging += OnSettingsChanged;
        }

        private void ConfigureFormatters()
        {
            Formatters = new ObservableCollection<IFormatter>(_serviceProvider.GetServices<IFormatter>());
            FormatterNames = new ObservableCollection<string>(Formatters.Select(f => f.Name));
        }

        #region Menu

        private bool MenuCanExecute()
        {
            return IsNotBlocked;
        }

        private bool CanvasCanExecute()
        {
            return HasCanvasFlag;
        }

        private bool MenuWithCanvasCanExecute()
        {
            return MenuCanExecute() && CanvasCanExecute();
        }

        private bool MenuWithNotCustomCanvasCanExecute()
        {
            return MenuCanExecute() && CanvasCanExecute() && !HasCustomCanvasFlag;
        }

        private bool MenuWithCanvasChangesCanExecute()
        {
            return MenuCanExecute() && HasChangesFlag;
        }

        private bool MenuWithGlobalChangesCanExecute()
        {
            return MenuCanExecute() && HasGlobalChangesFlag;
        }

        private bool MenuWithUndoCanExecute()
        {
            return MenuCanExecute() && HasUndoFlag;
        }

        private bool MenuWithRedoCanExecute()
        {
            return MenuCanExecute() && HasRedoFlag;
        }

        #region File

        /// <summary>
        /// Creates a new canvas through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuNew()
        {
            var dialogViewModel = new AddCanvasDialogViewModel();

            var success = _dialogService.ShowDialog(this, dialogViewModel);
            if (success == true)
            {
                var settings = dialogViewModel.Settings;

                Messenger.Send<NewCanvasSettingsMessage>(new(settings!));
            }
        }

        /// <summary>
        /// Opens an existing canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuOpen()
        {
            Messenger.Send(CommandFlags.Open);
        }

        /// <summary>
        /// Closes current canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuClose()
        {
            Messenger.Send(CommandFlags.CloseCanvas);
        }

        /// <summary>
        /// Closes all canvases from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuCloseAll()
        {
            Messenger.Send(CommandFlags.CloseCanvases);
        }

        /// <summary>
        /// Saves current canvas as new file from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuSaveAs()
        {
            Messenger.Send(CommandFlags.SaveAs);
        }

        /// <summary>
        /// Saves all canvases from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithGlobalChangesCanExecute))]
        private void MenuSaveAll()
        {
            SaveAll();
        }

        /// <summary>
        /// Saves all canvases from indirect call.
        /// </summary>
        /// <returns>Saving operation result.</returns>
        public bool SaveAll()
        {
            return Messenger.Send(new SaveAllRequestMessage());
        }

        /// <summary>
        /// Saves current canvas from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasChangesCanExecute))]
        private void MenuSave()
        {
            Messenger.Send(new SaveRequestMessage());
        }

        /// <summary>
        /// Exports current canvas as bitmap file from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuExport()
        {
            var dialogViewModel = new ExportDiagramDialogViewModel();

            if (_dialogService.ShowDialog(this, dialogViewModel) == true && dialogViewModel.ExportSettings is { } settings)
            {
                Messenger.Send<ExportSettingsMessage>(new(settings));
            }
        }

        /// <summary>
        /// Exits from program from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuCanExecute))]
        private void MenuExit()
        {
            if (OnRequestClose is not null)
            {
                OnRequestClose();
            }
        }

        #endregion
        #region Edit

        /// <summary>
        /// Cancels user last actions from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithUndoCanExecute))]
        private void MenuUndo()
        {
            Messenger.Send(CommandFlags.Undo);
        }

        /// <summary>
        /// Repeats user last actions if they were undid from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithRedoCanExecute))]
        private void MenuRedo()
        {
            Messenger.Send(CommandFlags.Redo);
        }
        #endregion
        #region View

        /// <summary>
        /// Zooms in to dynamic center from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomIn()
        {
            Messenger.Send(CommandFlags.ZoomIn);
        }

        /// <summary>
        /// Zoms out from dynamic center from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomOut()
        {
            Messenger.Send(CommandFlags.ZoomOut);
        }

        /// <summary>
        /// Resets current zoom to default from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuZoomReset()
        {
            Messenger.Send(CommandFlags.ZoomReset);
        }

        /// <summary>
        /// Enables or disables grid visual from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanvasCanExecute))]
        private void MenuEnableGrid()
        {
            Messenger.Send(CommandFlags.EnableGrid);
        }

        #endregion
        #region Diagram

        /// <summary>
        /// Changes size of the current selected canvas through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuChangeSize()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var dialogViewModel = new ChangeCanvasSizeDialogViewModel(currentCanvas.Settings.Width, currentCanvas.Settings.Height);

            if (_dialogService.ShowDialog(this, dialogViewModel) == true && dialogViewModel.Size is { } size)
            {
                Messenger.Send<UpdatedSizeMessage>(new(size));
            }
        }

        /// <summary>
        /// Changes background of the current selected canvas through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuChangeBackground()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var dialogViewModel = new ChangeCanvasBackgroundDialogViewModel(currentCanvas.Settings.Background);

            if (_dialogService.ShowDialog(this, dialogViewModel) == true && dialogViewModel.Color is { } background)
            {
                Messenger.Send<UpdatedBackgroundMessage>(new(background));
            }
        }

        /// <summary>
        /// Changes diagram type of the current diagram through dialog window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuChangeType()
        {
            var currentCanvas = Messenger.Send<CurrentCanvasRequestMessage>().Response;

            if (currentCanvas is null)
                return;

            var alertService = _serviceProvider.GetRequiredService<IAlertService>();
            var userSettingsService = _serviceProvider.GetRequiredService<IUserSettingsService>();
            var dialogViewModel = new ChangeDiagramTypeDialogViewModel(alertService, userSettingsService, currentCanvas.Settings.Type);

            if (_dialogService.ShowDialog(this, dialogViewModel) == true
                && dialogViewModel.SelectedType is { } diagram
                && diagram != currentCanvas.Settings.Type)
            {
                Messenger.Send<UpdatedTypeMessage>(new(diagram));
            }
        }

        /// <summary>
        /// Adapts current canvas to current theme from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuAdaptToTheme()
        {
            if (Messenger.Send<CurrentCanvasRequestMessage>().Response is not { } canvas)
                return;

            var alertService = _serviceProvider.GetRequiredService<IAlertService>();
            ThemeAdaptHelper.AdaptTheme(canvas, alertService, _trackableCommandManager);
        }

        #endregion
        #region Tools
        /// <summary>
        /// Opens dialog 'simulator' window from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithNotCustomCanvasCanExecute))]
        private void MenuSimulator()
        {
            var key = "Simulation";
            var doc = Messenger.Send<CurrentDocumentRequestMessage>().Response;

            if (doc is null) return;

            var payload = doc.GetPayloadData<SimulationContext>(key);

            var documentSerializationService = _serviceProvider.GetRequiredService<IDocumentSerializationService>();
            var alertService = _serviceProvider.GetRequiredService<IAlertService>();

            var terminated = false;

            var dialogViewModel = new SimulatorWindowViewModel(
                _dialogService, alertService, documentSerializationService, payload,
                onTerminate: () => terminated = true);

            dialogViewModel.RequestApply += () =>
            {
                if (dialogViewModel.NewContext is null)
                    return;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => doc.SetPayload(key, dialogViewModel.NewContext),
                    () => doc.SetPayload(key, payload));

                _trackableCommandManager.Execute(command);
            };

            if (!terminated && _dialogService.ShowDialog(this, dialogViewModel) == true &&
                dialogViewModel.NewContext is { } context)
            {
                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => doc.SetPayload(key, context),
                    () => doc.SetPayload(key, payload));

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Formats current canvas using selected formatter from menu button.
        /// </summary>
        [RelayCommand(CanExecute = nameof(MenuWithCanvasCanExecute))]
        private void MenuFormat(object param)
        {
            if (param is string formatterName &&
                Formatters?.FirstOrDefault(f => f.Name == formatterName) is IFormatter formatter &&
                Messenger.Send<CurrentCanvasRequestMessage>().Response is { } canvas)
            {
                var context = new FormattingContext();
                // Temporary solution.
                // TODO: Upgrade ExtendedCanvas so it can properly update position and size of children and notify about it.
                // TODO: Add support for automatic line bending.
                var alertService = _serviceProvider.GetRequiredService<IAlertService>();
                var connectionService = _serviceProvider.GetRequiredService<IConnectionService>();

                var localizedCaption = LocalizationHelper
                    .GetLocalizedValue<string>("MainResources", "FormatCaption"); 

                var localizedMessage = LocalizationHelper
                    .GetLocalizedValue<string>("MainResources", "FormatMessage");

                var result = alertService.RequestYesNoDecision(localizedMessage, localizedCaption);

                if (result is ConfirmationResult.No)
                {
                    return;
                }

                // This is Horrible! I'll re-do it later during development 1.1.0 version.
                Dictionary<Guid, (double PosX, double PosY, double? Width, double? Height, double? FontSize)>? snapshot = null;
                List<LineFigureModel>? deletedLines = null;
                List<ConnectionModel>? deletedConnections = null; 

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        snapshot = canvas.Figures.ToDictionary(
                            f => f.Id,
                            f =>
                            {
                                var shape = f as ShapeFigureModel;
                                var text = f as TextFigureModel;
                                return (f.PosX, f.PosY, shape?.Width, shape?.Height, text?.FontSize);
                            }
                        );

                        deletedLines = canvas.Figures.OfType<LineFigureModel>().ToList();

                        deletedConnections = new List<ConnectionModel>();
                        var connections = canvas.Connections;
                        foreach (var line in deletedLines)
                        {
                            var connection = connectionService.GetConnection(connections, line);
                            if (connection != null)
                            {
                                deletedConnections.Add(connection);
                                connectionService.RemoveConnection(connections, connection); 
                            }
                        }

                        canvas.Figures.RemoveAll(f => f is LineFigureModel);

                        formatter.Format(canvas.Figures, context);
                    },
                    () =>
                    {
                        foreach (var kvp in snapshot!)
                        {
                            var figure = canvas.Figures.FirstOrDefault(f => f.Id == kvp.Key);
                            if (figure != null)
                            {
                                figure.PosX = kvp.Value.PosX;
                                figure.PosY = kvp.Value.PosY;

                                if (figure is ShapeFigureModel shape)
                                {
                                    shape.Width = kvp.Value.Width ?? shape.Width;
                                    shape.Height = kvp.Value.Height ?? shape.Height;
                                }

                                if (figure is TextFigureModel text)
                                {
                                    text.FontSize = kvp.Value.FontSize ?? text.FontSize;
                                }
                            }
                        }

                        foreach (var line in deletedLines!)
                        {
                            if (!canvas.Figures.Any(f => f.Id == line.Id))
                            {
                                canvas.Figures.Add(line);
                            }
                        }

                        var connections = canvas.Connections;
                        foreach (var connection in deletedConnections!)
                        {
                            if (!connections.Any(c => c.Line.Id == connection.Line.Id))
                            {
                                connectionService.AddConnection(connections, connection);
                            }
                        }
                    }
                );

                _trackableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Opens dialog 'preferences' window from menu button.
        /// </summary>
        [RelayCommand]
        private void MenuPreferences()
        {
            var userSettingsService = _serviceProvider.GetRequiredService<IUserSettingsService>();
            var alertService = _serviceProvider.GetRequiredService<IAlertService>();

            var dialogViewModel = new SettingsDialogViewModel(userSettingsService, alertService, _trackableCommandManager);

            _dialogService.ShowDialog(this, dialogViewModel);
        }
        #endregion
        #region Help

        /// <summary>
        /// Opens app wiki from menu button. 
        /// </summary>
        [RelayCommand]
        private void MenuHelp()
        {
            var helpUrl = App.Current.Resources["HelpUrl"] as string;

            UrlHelper.OpenUrl(helpUrl);
        }

        /// <summary>
        /// Opens dialog 'about' window from menu button.
        /// </summary>
        [RelayCommand]
        private void MenuAbout()
        {
            var dialogViewModel = new AboutAppDialogViewModel();

            _dialogService.ShowDialog(this, dialogViewModel);
        }

        #endregion

        #endregion

        private void OnStateChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HasGlobalChangesFlag));
            OnPropertyChanged(nameof(HasChangesFlag));

            MenuSaveAllCommand.NotifyCanExecuteChanged();
            MenuSaveCommand.NotifyCanExecuteChanged();
        }

        private void OnOperationPerformed(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(HasUndoFlag));
            OnPropertyChanged(nameof(HasRedoFlag));

            MenuUndoCommand.NotifyCanExecuteChanged();
            MenuRedoCommand.NotifyCanExecuteChanged();
        }

        private void OnSettingsChanged(object? sender, SettingChangingEventArgs e)
        {
            if (e.SettingName == nameof(Properties.Settings.Default.GridVisible) && e.NewValue is bool gridVisible)
            {
                HasGridFlag = gridVisible;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // Configures menuflag - bool values.
            Messenger.Register<MainViewModel, Tuple<string, bool>>(this, (r, m) =>
            {
                switch (m.Item1)
                {
                    case ActionFlags.HasCanvas:
                        HasCanvasFlag = m.Item2;
                        break;
                    case ActionFlags.HasCustomCanvas:
                        HasCustomCanvasFlag = m.Item2;
                        break;
                    case ActionFlags.IsBlocked:
                        IsBlocked = m.Item2;
                        break;
                }
            });
        }
    }
}