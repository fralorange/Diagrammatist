﻿using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents tabs component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// This module used to show all currently open canvases that user can interact with.
    /// </remarks>
    public partial class TabsComponent : UserControl
    {
        private readonly IAlertService _alertService;

        public TabsComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<TabsViewModel>();
            _alertService = App.Current.Services.GetRequiredService<IAlertService>();

            viewModel.RequestOpen += OpenCanvas;
            viewModel.OpenFailed += OpenFail;
            viewModel.CloseFailed += CloseFail;

            DataContext = viewModel;

            InitializeComponent();
        }

        private MessageBoxResult CloseFail()
        {
            return _alertService.RequestConfirmation(LocalizationHelpers.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedCanvasMessage"),
                LocalizationHelpers.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedCanvasCaption"));
        }

        private void OpenFail()
        {
            _alertService.ShowError(LocalizationHelpers.GetLocalizedValue<string>("Alert.AlertResources", "CanvasAlreadyOpenMessage"),
                LocalizationHelpers.GetLocalizedValue<string>("Alert.AlertResources", "CanvasAlreadyOpenCaption"));
        }

        private string OpenCanvas()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"{App.Current.Resources["Filter"]}|*.{App.Current.Resources["Extension"]}",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }
    }
}
