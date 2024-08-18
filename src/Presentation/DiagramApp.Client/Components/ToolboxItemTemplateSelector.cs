﻿using DiagramApp.Domain.Figures;
using DiagramApp.Domain.Toolbox;

namespace DiagramApp.Client.Components
{
    public class ToolboxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? PathFigureTemplate { get; set; }
        public DataTemplate? PathTextFigureTemplate { get; set; }
        public DataTemplate? PolylineFigureTemplate { get; set; }
        public DataTemplate? TextFigureTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container)
        {
            if (item is ToolboxItem toolboxItem)
            {
                return toolboxItem.Figure switch
                {
                    ShapeFigure => PathFigureTemplate,
                    LineFigure => PolylineFigureTemplate,
                    TextFigure => TextFigureTemplate,
                    _ => null,
                };
            }
            return null;
        }
    }
}
