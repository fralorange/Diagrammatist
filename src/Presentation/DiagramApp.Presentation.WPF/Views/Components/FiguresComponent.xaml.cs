using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class FiguresComponent : UserControl
    {
        public FiguresComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<FiguresViewModel>();
            DataContext = viewModel;

            Loaded += async (s, e) =>
            {
                await viewModel.LoadFiguresAsync();
            };

            InitializeComponent();
        }
    }
}
