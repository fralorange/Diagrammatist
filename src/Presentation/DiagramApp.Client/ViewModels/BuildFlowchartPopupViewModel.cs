using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Services.Diagram.Flowchart;
using DiagramApp.Domain.Diagram.Flowchart;
using LocalizationResourceManager.Maui;

namespace DiagramApp.Client.ViewModels
{
    public partial class BuildFlowchartPopupViewModel : ObservableObject
    {
        private readonly FlowchartBuilder _flowchartBuilder = new();
        private readonly ILocalizationResourceManager _localizationResourceManager;

        public BuildFlowchartPopupViewModel(ILocalizationResourceManager localizationResourceManager) 
            => _localizationResourceManager = localizationResourceManager;

        [RelayCommand]
        private void AddObject(string name)
        {
            FlowchartType flowType = (FlowchartType)Enum.Parse(typeof(FlowchartType), name);
            var text = _localizationResourceManager[name];

            var flowComponent = new FlowchartComponent
            {
                FlowType = flowType,
                Text = text,
            };

            _flowchartBuilder.AddObject(flowComponent);
        }
    }
}
