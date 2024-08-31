﻿using DiagramApp.Presentation.WPF.ViewModels.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace DiagramApp.Presentation.WPF.Views.Components
{
    public partial class CanvasComponent : UserControl
    {
        public CanvasComponent()
        {
            DataContext = App.Current.Services.GetService<CanvasViewModel>();
            
            InitializeComponent();
        }
    }
}
