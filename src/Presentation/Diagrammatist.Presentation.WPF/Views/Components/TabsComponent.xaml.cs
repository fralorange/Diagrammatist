using Diagrammatist.Presentation.WPF.Core.Controls;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Models.Canvas;
using Diagrammatist.Presentation.WPF.Core.Services.Alert;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="TabsComponent"/> class.
        /// </summary>
        public TabsComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<TabsViewModel>();
            _alertService = App.Current.Services.GetRequiredService<IAlertService>();

            viewModel.RequestOpen += OpenFile;
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

        private string OpenFile()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = $"{App.Current.Resources["Filter"]}|*.{App.Current.Resources["Extension"]}",
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return string.Empty;
        }

        /// <summary>
        /// Opens a file with the specified path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public void OpenFile(string filePath)
        {
            if (DataContext is not TabsViewModel viewModel) return;

            viewModel.OpenDocument(filePath);
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

        #region UI Event handlers

        private void TabsListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && sender is ListBox listBox)
            {
                var point = e.GetPosition(listBox);
                var item = listBox.InputHitTest(point) as DependencyObject;

                var container = item?.GetVisualAncestor<ListBoxItem>();
                if (container != null && listBox.ItemContainerGenerator.ItemFromContainer(container) is CanvasModel data)
                {
                    DragDrop.DoDragDrop(container, data, DragDropEffects.Move);
                }
            }
        }

        private void TabsListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(typeof(CanvasModel)) ? DragDropEffects.Move : DragDropEffects.None;
            e.Handled = true;
        }

        private void TabsListBox_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(CanvasModel))) return;

            var droppedData = (CanvasModel)e.Data.GetData(typeof(CanvasModel))!;
            if (sender is not ListBox listBox) return;

            var point = e.GetPosition(listBox);
            var element = listBox.InputHitTest(point) as DependencyObject;
            var container = element?.GetVisualAncestor<ListBoxItem>();

            if (container == null) return;

            var targetData = (CanvasModel)listBox.ItemContainerGenerator.ItemFromContainer(container)!;
            if (droppedData == targetData) return;

            if (listBox.ItemsSource is ObservableCollection<CanvasModel> items)
            {
                int oldIndex = items.IndexOf(droppedData);
                int newIndex = items.IndexOf(targetData);

                if (oldIndex >= 0 && newIndex >= 0)
                {
                    items.Move(oldIndex, newIndex);
                    listBox.SelectedItem = droppedData;
                }
            }
        }
        #endregion
    }
}
