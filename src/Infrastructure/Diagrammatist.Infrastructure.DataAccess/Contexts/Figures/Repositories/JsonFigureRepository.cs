using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Domain.Figures;
using Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Serialization;
using Newtonsoft.Json;
using System.Drawing;

namespace Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Repositories
{
    /// <summary>
    /// A implemented class from <see cref="IFigureRepository"/>. A repository that deserializes json data about figures in toolbox.
    /// </summary>
    public class JsonFigureRepository : IFigureRepository
    {
        private readonly string FilePath = "Contexts/Figures/Data/FiguresData.json";

        /// <inheritdoc/>
        /// <remarks>
        /// This method used to get data from json file.
        /// </remarks>
        public Task<Dictionary<string, List<Figure>>> GetAsync()
        {
            var json = File.ReadAllText(FilePath);
            var jsonFigures = JsonConvert.DeserializeObject<List<JsonFigure>>(json);

            var dictionary = new Dictionary<string, List<Figure>>();

            foreach (var figure in jsonFigures!)
            {
                if (!dictionary.ContainsKey(figure.Category))
                {
                    dictionary.Add(figure.Category, new List<Figure>());
                }

                Figure parsedFigure = figure.Data.Type switch
                {
                    "Path" => new ShapeFigure { Name = figure.Name, Data = SplitJsonPathData(figure.Data.Value) },
                    "Points" => new LineFigure { Name = figure.Name, Points = SplitJsonPointsData(figure.Data.Value) },
                    "Text" => new TextFigure { Name = figure.Name, Text = figure.Data.Value },
                    _ => throw new NotSupportedException($"{nameof(figure.Data.Type)} : {figure.Data.Type}")
                };

                dictionary[figure.Category].Add(parsedFigure);
            }

            return Task.FromResult(dictionary);
        }

        private List<string> SplitJsonPathData(string input)
        {
            var splittedInput = input.Split("M", StringSplitOptions.TrimEntries);

            return splittedInput.Skip(1).Select(s => "M" + s).ToList();
        }

        private List<Point> SplitJsonPointsData(string input)
        {
            var splittedInput = input.Split(' ');

            return splittedInput
                .Select(s =>
                {
                    var coords = s.Split(',');
                    return new Point
                    {
                        X = int.TryParse(coords[0], out int x) ? x : default,
                        Y = int.TryParse(coords[1], out int y) ? y : default,
                    };
                })
                .ToList();
        }
    }
}
