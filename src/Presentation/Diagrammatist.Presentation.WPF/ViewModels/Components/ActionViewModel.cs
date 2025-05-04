using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for specific actions.
    /// </summary>
    public partial class ActionViewModel : ObservableRecipient
    {
        /// <summary>
        /// Gets action items.
        /// </summary>
        public ObservableCollection<ActionItemViewModel> Actions { get; } = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionViewModel"/> class.
        /// </summary>
        public ActionViewModel()
        {
            Actions.Add(new ActionItemViewModel(
                tooltip: LocalizationHelper.GetLocalizedValue<string>("Action.ActionResources", "Confirm"),
                command: ConfirmCommand,
                iconData: Geometry.Parse("M2,8 L6,12 L13,3")
            ));

            Actions.Add(new ActionItemViewModel(
                tooltip: LocalizationHelper.GetLocalizedValue<string>("Action.ActionResources", "Cancel"),
                command: CancelCommand,
                iconData: Geometry.Parse("M3,3 L12,12 M12,3 L3,12")
            ));

            IsActive = true;
        }

        /// <summary>
        /// Occurs when the user requests to end drawing a line.
        /// </summary>
        public event Func<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?>? RequestEndDrawing;

        /// <summary>
        /// Performs an early confirmation of the drawing operation.
        /// </summary>
        public void EarlyConfirm() => Confirm();

        /// <summary>
        /// Confirms the drawing operation and notifies the end of drawing.
        /// </summary>
        [RelayCommand]
        private void Confirm()
        {
            if (RequestEndDrawing?.Invoke() is { } data)
            {
                Messenger.Send(new LineDrawResultMessage(data));
            }
        }

        /// <summary>
        /// Cancels the drawing operation and notifies the end of drawing.
        /// </summary>
        [RelayCommand]
        private void Cancel()
        {
            RequestEndDrawing?.Invoke();
            Messenger.Send(new LineDrawResultMessage(null));
        }
    }
}
