using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class FiguresComponent : UserControl
    {
        public FiguresComponent()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<FiguresViewModel>();
        }
    }
}
