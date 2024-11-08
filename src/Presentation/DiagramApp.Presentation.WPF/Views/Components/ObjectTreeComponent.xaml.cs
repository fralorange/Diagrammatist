using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    /// <summary>
    /// A class that represens object tree component and derives from <see cref="UserControl"/>.
    /// </summary>
    /// <remarks>
    /// Module that also can be called as 'Explorer' that contains informatically every item that placed on the canvas.
    /// </remarks>
    public partial class ObjectTreeComponent : UserControl
    {
        public ObjectTreeComponent()
        {
            DataContext = App.Current.Services.GetService<ObjectTreeViewModel>();

            InitializeComponent();
        }
    }
}
