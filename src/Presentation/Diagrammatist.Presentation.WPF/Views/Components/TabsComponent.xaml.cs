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
        public TabsComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<TabsViewModel>();

            viewModel.RequestOpen += OpenCanvas;
            viewModel.OpenFailed += OpenFail;

            DataContext = viewModel;
            
            InitializeComponent();
        }

        private void OpenFail()
        {
            // TO-DO: move into IDialogService or come up with a better idea of showing warnings to user
            // it's still shouldn't be invoked from code-behind, i think.
            MessageBox.Show("A canvas with that name is already open.", "PLACEHOLDER MESSAGE BOX");
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
