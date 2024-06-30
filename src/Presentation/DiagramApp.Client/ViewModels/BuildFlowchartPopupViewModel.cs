using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiagramApp.Application.AppServices.Services.Diagram;
using DiagramApp.Application.AppServices.Services.Diagram.Flowchart;
using DiagramApp.Domain.Diagram;
using DiagramApp.Domain.Diagram.Flowchart;
using LocalizationResourceManager.Maui;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels
{
    public partial class BuildFlowchartPopupViewModel : ObservableObject, IDiagramObserver
    {
        private readonly FlowchartBuilder _flowchartBuilder = new();
        private readonly ILocalizationResourceManager _localizationResourceManager;

        public ObservableCollection<FlowchartComponent> Components = [];
        public ObservableCollection<Connection> Connections = [];

        public BuildFlowchartPopupViewModel(ILocalizationResourceManager localizationResourceManager)
        {
            _localizationResourceManager = localizationResourceManager;
            _flowchartBuilder.Subscribe(this);
        }

        public void UpdateComponents(IReadOnlyList<Component> components)
        {
            Components.Clear();
            components.OfType<FlowchartComponent>().ToList().ForEach(Components.Add);
        }

        public void UpdateConnections(IReadOnlyList<Connection> connections)
        {
            Connections.Clear();
            connections.ToList().ForEach(Connections.Add);
        }

        [RelayCommand]
        private void AddObject(string name)
        {
            FlowchartType flowType = (FlowchartType)Enum.Parse(typeof(FlowchartType), name);
            var text = _localizationResourceManager[name];

            var flowComponent = new FlowchartComponent
            {
                Width = 80,
                Height = 50,
                FlowType = flowType,
                Text = text,
            };

            _flowchartBuilder.AddObject(flowComponent);
        }
    }
}
