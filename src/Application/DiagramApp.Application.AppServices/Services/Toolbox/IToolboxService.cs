using DiagramApp.Domain.Toolbox;

namespace DiagramApp.Application.AppServices.Services.Toolbox
{
    public interface IToolboxService
    {
        Task<List<ToolboxItem>> GetToolboxItemsAsync(string path);
    }
}
