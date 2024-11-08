using System.Diagnostics;

namespace DiagramApp.Presentation.WPF.Framework.Commands.Helpers
{
    /// <summary>
    /// A class that helps with opening urls via commands.
    /// </summary>
    public static class UrlHelper
    {
        /// <summary>
        /// Opens url using <see cref="Process"/> class.
        /// </summary>
        /// <param name="url"></param>
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
