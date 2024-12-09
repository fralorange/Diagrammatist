using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Application.AppServices.Canvas.Services;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Helpers;
using Diagrammatist.Presentation.WPF.Framework.Commands.Undoable.Manager;
using Diagrammatist.Presentation.WPF.Framework.Messages;
using Diagrammatist.Presentation.WPF.Mappers.Canvas;
using Diagrammatist.Presentation.WPF.Models.Canvas;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Constants.Flags;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        private readonly ICanvasManipulationService _canvasManipulationService;
        private readonly IUndoableCommandManager _undoableCommandManager;

        /// <summary>
        /// Gets or sets collection of <see cref="CanvasModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store canvases in tabs UI.
        /// </remarks>
        public ObservableCollection<CanvasModel> Canvases { get; } = [];

        /// <summary>
        /// Gets or sets <see cref="CanvasModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store and distribute selected <see cref="CanvasModel"/>.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasModel? _selectedCanvas;

        public TabsViewModel(ICanvasManipulationService canvasManipulationService, IUndoableCommandManager undoableCommandManager)
        {
            _canvasManipulationService = canvasManipulationService;
            _undoableCommandManager = undoableCommandManager;

            IsActive = true;
        }

        /// <summary>
        /// Creates new <see cref="CanvasModel"/> and adds it to <see cref="Canvases"/>
        /// </summary>
        /// <param name="settings">Diagram settings.</param>
        private async Task CreateCanvas(SettingsModel settings)
        {
            var canvasDomain = await _canvasManipulationService.CreateCanvasAsync(settings.ToDomain());

            var canvas = canvasDomain.ToModel();

            Canvases.Add(canvas);
            SelectedCanvas = canvas;
        }

        /// <summary>
        /// Updates existing <see cref="CanvasModel"/> settings.
        /// </summary>
        /// <param name="newSettings">New settings.</param>
        private void UpdateCanvas(SettingsModel newSettings)
        {
            if (SelectedCanvas is not null)
            {
                var oldSettings = SelectedCanvas.Settings;
                var currentCanvas = SelectedCanvas;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        var settingsDomain = newSettings.ToDomain();

                        _canvasManipulationService.UpdateCanvasSettings(currentCanvas.ToDomain(), settingsDomain);

                        currentCanvas.Settings = settingsDomain.ToModel();
                    },
                    () =>
                    {
                        var settingsDomain = oldSettings.ToDomain();

                        _canvasManipulationService.UpdateCanvasSettings(currentCanvas.ToDomain(), settingsDomain);

                        currentCanvas.Settings = settingsDomain.ToModel();
                    }
                );

                _undoableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Deletes existing <see cref="CanvasModel"/> from <see cref="Canvases"/>.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        [RelayCommand]
        private void CloseCanvas(CanvasModel target)
        {
            Canvases.Remove(target);
        }

        private void CloseCanvases()
        {
            if (Canvases.Count > 0)
            {
                foreach (var canvas in Canvases.ToList())
                {
                    CloseCanvas(canvas);
                }
            }
        }

        /// <summary>
        /// Invokes when canvas changes. Updates <see cref="_undoableCommandManager"/> content.
        /// </summary>
        [RelayCommand]
        private void CanvasChanged()
        {
            _undoableCommandManager.UpdateContent(SelectedCanvas);
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<TabsViewModel, NewCanvasSettingsMessage>(this, (r, m) =>
            {
                Dispatcher.CurrentDispatcher.Invoke(async () =>
                {
                    await CreateCanvas(m.Value);
                });
            });

            Messenger.Register<TabsViewModel, UpdatedSettingsMessage>(this, (r, m) =>
            {
                UpdateCanvas(m.Value);
            });

            Messenger.Register<TabsViewModel, string>(this, (r, m) =>
            {
                switch (m)
                {
                    case MessengerFlags.CloseCanvas when SelectedCanvas is not null:
                        CloseCanvas(SelectedCanvas);
                        break;
                    case MessengerFlags.CloseCanvases:
                        CloseCanvases();
                        break;
                }
            });
        }
    }
}
