﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Application.AppServices.Contexts.Canvas.Services;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Settings;
using DiagramApp.Presentation.WPF.Framework.Commands.Helpers.Undoable;
using DiagramApp.Presentation.WPF.Framework.Commands.Manager;
using DiagramApp.Presentation.WPF.Framework.Commands.UndoableCommand;
using DiagramApp.Presentation.WPF.Framework.Messages;
using DiagramApp.Presentation.WPF.ViewModels.Components.Consts.Flags;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace DiagramApp.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class TabsViewModel : ObservableRecipient
    {
        private readonly ICanvasManipulationService _canvasManipulationService;
        private readonly IUndoableCommandManager _undoableCommandManager;

        /// <summary>
        /// Gets or sets collection of <see cref="CanvasDto"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store canvases in tabs UI.
        /// </remarks>
        public ObservableCollection<CanvasDto> Canvases { get; } = [];

        /// <summary>
        /// Gets or sets <see cref="CanvasDto"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store and distribute selected <see cref="CanvasDto"/>.
        /// </remarks>
        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private CanvasDto? _selectedCanvas;

        public TabsViewModel(ICanvasManipulationService canvasManipulationService, IUndoableCommandManager undoableCommandManager)
        {
            _canvasManipulationService = canvasManipulationService;
            _undoableCommandManager = undoableCommandManager;

            IsActive = true;
        }

        /// <summary>
        /// Creates new <see cref="CanvasDto"/> and adds it to <see cref="Canvases"/>
        /// </summary>
        /// <param name="settings">Diagram settings.</param>
        private async Task CreateCanvas(DiagramSettingsDto settings)
        {
            var canvas = await _canvasManipulationService.CreateCanvasAsync(settings);

            Canvases.Add(canvas);
            SelectedCanvas = canvas;
        }

        /// <summary>
        /// Updates existing <see cref="CanvasDto"/> settings.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        /// <param name="newSettings">New settings.</param>
        private void UpdateCanvas(DiagramSettingsDto newSettings)
        {
            if (SelectedCanvas is not null)
            {
                var oldSettings = SelectedCanvas.Settings;
                var currentCanvas = SelectedCanvas;

                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () =>
                    {
                        _canvasManipulationService.UpdateCanvas(currentCanvas, newSettings);
                        Messenger.Send<RefreshCanvasMessage>(new(newSettings));
                    },
                    () =>
                    {
                        _canvasManipulationService.UpdateCanvas(currentCanvas, oldSettings);
                        Messenger.Send<RefreshCanvasMessage>(new(oldSettings));
                    }
                );

                _undoableCommandManager.Execute(command);
            }
        }

        /// <summary>
        /// Deletes existing <see cref="CanvasDto"/> from <see cref="Canvases"/>.
        /// </summary>
        /// <param name="target">Target canvas.</param>
        [RelayCommand]
        private void CloseCanvas(CanvasDto target)
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
