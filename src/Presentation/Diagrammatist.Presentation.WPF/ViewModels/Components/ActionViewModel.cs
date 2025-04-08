using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for specific actions.
    /// </summary>
    public partial class ActionViewModel : ObservableRecipient
    {
        /// <summary>
        /// Occurs when user requests end drawing.
        /// </summary>
        public event Func<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?>? RequestEndDrawing;

        public ActionViewModel()
        {
            IsActive = true;
        }

        /// <summary>
        /// Confirms action when logic requests it.
        /// </summary>
        public void EarlyConfirm()
        {
            Confirm();
        }

        /// <summary>
        /// Confirms action.
        /// </summary>
        [RelayCommand]
        private void Confirm()
        {
            if (RequestEndDrawing is not null && RequestEndDrawing.Invoke() is { } lineData)
            {
                Messenger.Send(new LineDrawResultMessage(lineData));
            }
        }

        /// <summary>
        /// Cancels action.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            RequestEndDrawing?.Invoke();

            Messenger.Send(new LineDrawResultMessage(null));
        }
    }
}
