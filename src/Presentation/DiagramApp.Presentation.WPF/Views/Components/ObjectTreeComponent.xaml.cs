using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class ObjectTreeComponent : UserControl
    {
        public ObjectTreeComponent()
        {
            DataContext = App.Current.Services.GetService<ObjectTreeViewModel>();

            InitializeComponent();
        }
    }
}
