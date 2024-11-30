using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents a figures component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// Module that also can be called as 'Toolbox' that contains every item that can be placed on canvas.
    /// </remarks>
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
