using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using System.Windows.Input;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class CanvasComponent : UserControl
    {
        public CanvasComponent()
        {
            DataContext = App.Current.Services.GetService<CanvasViewModel>();
            
            InitializeComponent();
        }

        private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is ListBox listBox && e.OriginalSource is Canvas)
            {
                listBox.UnselectAll();
                Keyboard.ClearFocus();
            }
        }
    }
}
