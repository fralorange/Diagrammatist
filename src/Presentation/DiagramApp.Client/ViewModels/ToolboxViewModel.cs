using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Services;
using DiagramApp.Domain.Toolbox;
using LocalizationResourceManager.Maui;

namespace DiagramApp.Client.ViewModels
{
    public partial class ToolboxViewModel : ObservableObject
    {
        public List<ToolboxCategory> Categories => Enum.GetValues<ToolboxCategory>()
            .Where(item => item != ToolboxCategory.Advanced)
            .ToList();
        
        private readonly IToolboxService _toolboxService;
        private readonly ILocalizationResourceManager _localizationResourceManager;

        public List<ToolboxItem>? ToolboxItems { get; private set; }

        [ObservableProperty]
        private List<ToolboxItem>? _filteredToolboxItems;

        [ObservableProperty]
        private List<ToolboxItem>? _advancedToolboxItems;

        [ObservableProperty]
        private ToolboxCategory _selectedCategory;

        public ToolboxViewModel(IToolboxService toolboxService, ILocalizationResourceManager localizationResourceManager)
        {
            _toolboxService = toolboxService;
            _localizationResourceManager = localizationResourceManager;
        }

        [RelayCommand]
        private async Task LoadToolboxAsync()
        {
            var twoletterISO = _localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName;

            ToolboxItems = await _toolboxService.GetToolboxItemsAsync($"toolboxData.{twoletterISO}.json");
            InitializeAdvancedToolbox();

            OnPropertyChanged(nameof(Categories));
            
            CategoryChange();
        }

        [RelayCommand]
        private void CategoryChange()
        {
            FilteredToolboxItems = ToolboxItems! // maybe contain all filteredtoolboxitems in array or smth (optimize idea) so there will be no need in filtering it over and over again
                .Where(item => item.Category == SelectedCategory)
                .ToList();
        }

        private void InitializeAdvancedToolbox()
        {
            AdvancedToolboxItems = ToolboxItems!
                .Where(item => item.Category == ToolboxCategory.Advanced)
                .ToList();
        }
    }
}
