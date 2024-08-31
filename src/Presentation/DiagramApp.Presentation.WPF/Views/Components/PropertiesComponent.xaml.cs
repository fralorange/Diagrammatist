using DiagramApp.Presentation.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class PropertiesComponent : UserControl
    {
        public PropertiesComponent()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<PropertiesViewModel>();
        }
    }
}
