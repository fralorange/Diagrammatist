using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.Core.Services.Formatting.Gost
{
    /// <summary>  
    /// A class that implements the <see cref="IFormatter"/> interface   
    /// for formatting figures in a GOST 19.701-90-compliant manner.  
    /// </summary>  
    public class GostFormatter : IFormatter
    {
        /// <inheritdoc/>
        public string Name => "Gost";

        /// <inheritdoc/>
        public void Format(IEnumerable<FigureModel> figures, IFormattingContext formattingContext)
        {
            const double elementWidth = 80;
            const double elementHeight = 80;

            const double spacing = 37.8;

            if (figures is null || !figures.Any())
                return;

            var ordered = figures
                .Where(f => f is not LineFigureModel)
                .OrderBy(f => f.PosY)
                .ThenBy(f => f.PosX)
                .ToList();

            var rows = new List<List<FigureModel>>();
            List<FigureModel>? currentRow = null;
            var rowStartY = 0.0;

            foreach (var figure in ordered)
            {
                if (currentRow is null ||
                    Math.Abs(figure.PosY - rowStartY) > elementHeight / 2)
                {
                    currentRow = [];
                    rows.Add(currentRow);
                    rowStartY = figure.PosY;
                }
                currentRow.Add(figure);
            }

            var baseX = ordered.Min(f => f.PosX);
            var baseY = ordered.Min(f => f.PosY);

            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                var row = rows[rowIndex];
                for (int colIndex = 0; colIndex < row.Count; colIndex++)
                {
                    var fig = row[colIndex];

                    formattingContext.SetElementSize(fig, elementWidth, elementHeight);

                    var newX = baseX + colIndex * (elementWidth + spacing);
                    var newY = baseY + rowIndex * (elementHeight + spacing);

                    formattingContext.MoveElement(fig, new Point(newX, baseY + rowIndex * (elementHeight + spacing)));
                }
            }
        }
    }
}
