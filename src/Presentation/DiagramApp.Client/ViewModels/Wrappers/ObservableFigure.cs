﻿using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas.Figures;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservableFigure : ObservableObject
    {
        protected readonly Figure _figure;

        public ObservableFigure(Figure figure)
            => _figure = figure;

        [ObservableProperty]
        private bool _isSelected;
        //Maybe put everything in external class ObservableProperties to group it.
        [ObservableProperty]
        private double _translationX;
        
        [ObservableProperty]
        private double _translationY;

        [ObservableProperty]
        private double _rotation;

        public string Name
        {
            get => _figure.Name;
            set => SetProperty(_figure.Name, value, _figure, (f, n) =>  f.Name = n);
        }
    }
}
