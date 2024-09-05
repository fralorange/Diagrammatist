using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class ToolbarComponent : UserControl
    {
        public ToolbarComponent()
        {
            DataContext = App.Current.Services.GetService<ToolbarViewModel>();

            InitializeComponent();
        }
    }
}
