using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Toolbox;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels
{
    public class ToolboxViewModel : ObservableObject
    {
        // mock data
        public ObservableCollection<ToolboxItem> ToolboxItems { get; } = new()
        {
            new() {
                Name = "Прямоугольник",
                Category = "Фигуры",
                PathData = "M0,0 L100,0 L100,100 L0,100 Z"
            },
            new()
            {
                Name = "Круг",
                Category = "Фигуры",
                PathData = "M50,0 A50,50 0 0 1 100,50 A50,50 0 0 1 50,100 A50,50 0 0 1 0,50 A50,50 0 0 1 50,0 Z"
            },
            new()
            {
                Name = "Ромб",
                Category = "Фигуры",
                PathData = "M50,50 L100,0 L150,50 L100,100 Z"
            }
        };
    }
}
