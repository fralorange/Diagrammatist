using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Special.Container;
using Diagrammatist.Domain.Figures.Special.Flowchart;
using PolyType;

namespace Diagrammatist.Application.AppServices.Figures.Serializer.Witness
{
    /// <summary>
    /// A figure witness partial class.
    /// </summary>
    [GenerateShape<Figure>]
    [GenerateShape<LineFigure>]
    [GenerateShape<ShapeFigure>]
    [GenerateShape<TextFigure>]
    [GenerateShape<ContainerFigure>]
    [GenerateShape<FlowchartFigure>]
    [GenerateShape<FlowLineFigure>]
    public partial class FigureWitness;
}
