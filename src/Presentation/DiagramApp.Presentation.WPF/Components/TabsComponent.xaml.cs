using DiagramApp.Presentation.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Components
{
    public partial class TabsComponent : UserControl
    {
        public TabsComponent()
        {
            DataContext = App.Current.Services.GetService<TabsViewModel>();
            
            InitializeComponent();
        }
    }
}
