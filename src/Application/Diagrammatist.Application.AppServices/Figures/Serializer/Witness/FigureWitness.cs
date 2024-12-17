using Diagrammatist.Domain.Figures;
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
    partial class FigureWitness;
}
