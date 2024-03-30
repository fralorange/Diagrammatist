using DiagramApp.Domain.Toolbox;
using Newtonsoft.Json;

namespace DiagramApp.Application.AppServices.Services
{
    public class ToolboxService : IToolboxService
    {
        public async Task<List<ToolboxItem>> GetToolboxItemsAsync(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);

            var json = reader.ReadToEnd();
            var items = JsonConvert.DeserializeObject<List<ToolboxItem>>(json)
                ?? throw new JsonSerializationException(nameof(fileName));

            return items;
        }
    }
}
