using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Services;
using DiagramApp.Domain.Toolbox;

namespace DiagramApp.Client.ViewModels
{
    public partial class ToolboxViewModel : ObservableObject
    {
        private readonly IToolboxService _toolboxService;

        public List<ToolboxItem>? ToolboxItems { get; private set; }
        public ToolboxCategory[] Categories => Enum.GetValues<ToolboxCategory>();

        [ObservableProperty]
        private List<ToolboxItem>? _filteredToolboxItems;

        [ObservableProperty]
        private ToolboxCategory _selectedCategory;

        public ToolboxViewModel(IToolboxService toolboxService) => _toolboxService = toolboxService;

        [RelayCommand]
        private async Task LoadToolboxAsync()
        {
            ToolboxItems = await _toolboxService.GetToolboxItemsAsync("toolboxData.json");
            CategoryChanged();
        }

        [RelayCommand]
        private void CategoryChanged()
        {
            FilteredToolboxItems = ToolboxItems!
                .Where(item => item.Category == SelectedCategory)
                .ToList();
        }
    }
}
