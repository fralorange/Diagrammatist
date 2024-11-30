using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represents properties component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// This module used to configure items placed on the canvas.
    /// </remarks>
    public partial class PropertiesComponent : UserControl
    {
        public PropertiesComponent()
        {
            DataContext = App.Current.Services.GetService<PropertiesViewModel>();

            InitializeComponent();
        }
    }
}
