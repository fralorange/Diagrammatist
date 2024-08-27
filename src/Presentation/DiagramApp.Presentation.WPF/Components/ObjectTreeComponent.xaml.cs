using DiagramApp.Presentation.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Components
{
    public partial class ObjectTreeComponent : UserControl
    {
        public ObjectTreeComponent()
        {
            InitializeComponent();

            DataContext = App.Current.Services.GetService<ObjectTreeViewModel>();
        }
    }
}
