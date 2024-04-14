using DiagramApp.Domain.Canvas.Figures;
using DiagramApp.Domain.Toolbox;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DiagramApp.Application.AppServices.Services
{
    public partial class ToolboxService : IToolboxService
    {
        public async Task<List<ToolboxItem>> GetToolboxItemsAsync(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd();
            var jsonItems = JsonConvert.DeserializeObject<List<ToolboxJson>>(json)
                ?? throw new JsonSerializationException(nameof(fileName));

            var items = jsonItems
                .Select(jsonItem =>
                {
                    Figure figure = jsonItem.Category switch
                    {
                        not ToolboxCategory.Advanced => new PathFigure { Name = jsonItem.Name, PathData = jsonItem.Data },
                        ToolboxCategory.Advanced when PolylineRegex().IsMatch(jsonItem.Data) => new PolylineFigure
                        {
                            Name = jsonItem.Name,
                            Points = jsonItem.Data.Split(' ')
                                .Select(s => s.Split(','))
                                .Select(pt => new System.Drawing.Point(int.Parse(pt[0]), int.Parse(pt[1])))
                                .ToList()
                        },
                        _ => new TextFigure { Name = jsonItem.Name, Text = jsonItem.Data }
                    };

                    return new ToolboxItem { Category = jsonItem.Category, Figure = figure };
                }).ToList();
            return items!;
        }

        [GeneratedRegex(@"^\d+\,\d+( \d+\,\d+)*$")]
        private static partial Regex PolylineRegex();
    }
}
