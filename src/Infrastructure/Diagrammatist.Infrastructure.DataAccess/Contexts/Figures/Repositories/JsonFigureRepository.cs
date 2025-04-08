using Diagrammatist.Application.AppServices.Figures.Repositories;
using Diagrammatist.Domain.Figures;
using Diagrammatist.Domain.Figures.Special.Flowchart;
using Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Serialization;
using Newtonsoft.Json;
using System.Drawing;
using System.Reflection;

namespace Diagrammatist.Infrastructure.DataAccess.Contexts.Figures.Repositories
{
    /// <summary>
    /// A implemented class from <see cref="IFigureRepository"/>. A repository that deserializes json data about figures in toolbox.
    /// </summary>
    public class JsonFigureRepository : IFigureRepository
    {
        private readonly string ResourceName = "FiguresData.json";

        /// <inheritdoc/>
        /// <remarks>
        /// This method used to get data from json file.
        /// </remarks>
        public Task<Dictionary<string, List<Figure>>> GetAsync()
        {
            var assembly = Assembly.Load("Diagrammatist.Infrastructure");

            using var stream = assembly.GetManifestResourceStream(ResourceName) 
                ?? throw new FileNotFoundException($"Embedded resource '{ResourceName}' not found.");

            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd(); 
            var jsonFigures = JsonConvert.DeserializeObject<List<JsonFigure>>(json);

            var dictionary = new Dictionary<string, List<Figure>>();

            foreach (var figure in jsonFigures!)
            {
                if (!dictionary.TryGetValue(figure.Category, out List<Figure>? value))
                {
                    value = [];
                    dictionary.Add(figure.Category, value);
                }

                Figure parsedFigure = figure.Data.Type switch
                {
                    "Path" => new ShapeFigure { Name = figure.Name, Data = SplitJsonPathData(figure.Data.Value) },
                    "Points" => new LineFigure { Name = figure.Name, Points = SplitJsonPointsData(figure.Data.Value) },
                    "Text" => new TextFigure { Name = figure.Name, Text = figure.Data.Value },
                    "Flowchart" => new FlowchartFigure { Name = figure.Name, Data = SplitJsonPathData(figure.Data.Value), Subtype = CastSubtypeToFlowchartSubtype(figure.Data.Subtype)},
                    _ => throw new NotSupportedException($"{nameof(figure.Data.Type)} : {figure.Data.Type}")
                };
                value.Add(parsedFigure);
            }

            return Task.FromResult(dictionary);
        }

        private List<string> SplitJsonPathData(string input)
        {
            var splittedInput = input.Split("M", StringSplitOptions.TrimEntries);

            return splittedInput.Skip(1).Select(s => "M" + s).ToList();
        }

        private List<PointF> SplitJsonPointsData(string input)
        {
            var splittedInput = input.Split(' ');

            return splittedInput
                .Select(s =>
                {
                    var coords = s.Split(',');
                    return new PointF
                    {
                        X = int.TryParse(coords[0], out int x) ? x : default,
                        Y = int.TryParse(coords[1], out int y) ? y : default,
                    };
                })
                .ToList();
        }

        private FlowchartSubtype CastSubtypeToFlowchartSubtype(string? value)
        {
            return (value is not null) ? (FlowchartSubtype)Enum.Parse(typeof(FlowchartSubtype), value) : FlowchartSubtype.Process;
        }
    }
}
