using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
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
            viewModel.RequestSaveAs += SaveAs;
            viewModel.OpenFailed += OpenFail;
            viewModel.CloseFailed += CloseFail;

            DataContext = viewModel;

            InitializeComponent();
        }

        private ConfirmationResult CloseFail()
        {
            return _alertService.RequestConfirmation(LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedCanvasMessage"),
                LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "UnsavedCanvasCaption"));
        }

        private void OpenFail()
        {
            _alertService.ShowError(LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "CanvasAlreadyOpenMessage"),
                LocalizationHelper.GetLocalizedValue<string>("Alert.AlertResources", "CanvasAlreadyOpenCaption"));
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

        private string SaveAs(string fileName)
        {
            SaveFileDialog saveFileDialog = new()
            {
                Filter = $"{App.Current.Resources["Filter"]}|*.{App.Current.Resources["Extension"]}",
                FileName = fileName,
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;
        }
    }
}
