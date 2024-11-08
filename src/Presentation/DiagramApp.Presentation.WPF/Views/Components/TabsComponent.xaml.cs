using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents tabs component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// This module used to show all currently open canvases that user can interact with.
    /// </remarks>
    public partial class TabsComponent : UserControl
    {
        public TabsComponent()
        {
            InitializeComponent();
            
            DataContext = App.Current.Services.GetService<TabsViewModel>();
        }
    }
}
