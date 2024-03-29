using DiagramApp.Domain.Toolbox;

namespace DiagramApp.Application.AppServices.Services
{
    public interface IToolboxService
    {
        Task<List<ToolboxItem>> GetToolboxItemsAsync(string path);
    }
}
