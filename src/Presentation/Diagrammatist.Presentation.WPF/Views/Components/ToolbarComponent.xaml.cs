using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class ToolbarComponent : UserControl
    {
        /// <summary>
        /// A class that represents toolbar component and derives from <see cref="UserControl"/>.
        /// </summary>
        /// <remarks>
        /// This module used to store all instruments that user can interact with.
        /// </remarks>
        public ToolbarComponent()
        {
            DataContext = App.Current.Services.GetService<ToolbarViewModel>();

            InitializeComponent();
        }
    }
}
