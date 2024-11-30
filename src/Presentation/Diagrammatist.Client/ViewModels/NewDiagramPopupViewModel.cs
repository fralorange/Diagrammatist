using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Domain.Settings;

namespace DiagramApp.Client.ViewModels
{
    public partial class NewDiagramPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private DiagramSettings _settings = new();

        [RelayCommand]
        private void Ok()
        {
            WeakReferenceMessenger.Default.Send(Settings);
        }
    }
}
