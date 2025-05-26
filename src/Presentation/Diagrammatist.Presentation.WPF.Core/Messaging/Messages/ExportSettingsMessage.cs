using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Shared.Records;

namespace Diagrammatist.Presentation.WPF.Core.Messaging.Messages
{
    /// <summary>
    /// A message class that carries export settings for diagrams.
    /// </summary>
    public class ExportSettingsMessage : ValueChangedMessage<ExportSettings>
    {
        /// <inheritdoc/>
        public ExportSettingsMessage(ExportSettings value) : base(value)
        {
        }
    }
}
