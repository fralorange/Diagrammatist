using DiagramApp.Presentation.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Components
{
    public partial class CanvasComponent : UserControl
    {
        public CanvasComponent()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<CanvasViewModel>();
        }
    }
}
