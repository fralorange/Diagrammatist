using DiagramApp.Contracts.Settings;

namespace DiagramApp.Presentation.WPF.Framework.Messages
{
    public class RefreshCanvasMessage : UpdatedSettingsMessage
    {
        public RefreshCanvasMessage(DiagramSettingsDto value) : base(value)
        {
        }
    }
}
