using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class PropertiesComponent : UserControl
    {
        public PropertiesComponent()
        {
            DataContext = App.Current.Services.GetService<PropertiesViewModel>();

            InitializeComponent();
        }
    }
}
