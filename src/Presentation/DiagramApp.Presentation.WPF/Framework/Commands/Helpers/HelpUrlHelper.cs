using System.Diagnostics;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    public static class HelpUrlHelper
    {
        public static void OpenHelpUrl(string? url)
        {
            if (url is null)
                return;

            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true,
            });
        }
    }
}
