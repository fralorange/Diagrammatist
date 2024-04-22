using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using DiagramApp.Domain.DiagramSettings;

namespace DiagramApp.Client.ViewModels
{
    public partial class ChangeDiagramSizePopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private DiagramSettings? _settings;

        [RelayCommand]
        private void Ok()
        {
            WeakReferenceMessenger.Default.Send(Settings!);
        }
    }
}
