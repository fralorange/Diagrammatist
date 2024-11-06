using System.Diagnostics;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    public static class UrlHelper
    {
        public static void OpenUrl(string? url)
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
