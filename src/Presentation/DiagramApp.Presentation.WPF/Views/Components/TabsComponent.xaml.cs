using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class TabsComponent : UserControl
    {
        public TabsComponent()
        {
            InitializeComponent();
            
            DataContext = App.Current.Services.GetService<TabsViewModel>();
        }
    }
}
