using Diagrammatist.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Diagrammatist.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents action component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// Additional module of the app that displays some actions.
    /// </remarks>
    public partial class ActionComponent : UserControl
    {
        public ActionComponent()
        {
            var viewModel = App.Current.Services.GetRequiredService<ActionViewModel>();

            DataContext = viewModel;

            InitializeComponent();
        }
    }
}
