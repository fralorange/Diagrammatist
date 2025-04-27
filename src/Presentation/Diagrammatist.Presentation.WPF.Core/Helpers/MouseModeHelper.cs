namespace Diagrammatist.Presentation.WPF.Core.Helpers
{
    /// <summary>
    /// A class that helps with changing mouse modes.
    /// </summary>
    public static class MouseModeHelper
    {
        /// <summary>
        /// Gets enum mode from string.
        /// </summary>
        /// <typeparam name="T">Enum type.</typeparam>
        /// <param name="mode">String mode.</param>
        /// <returns></returns>
        public static T GetParsedMode<T>(string mode) where T : struct
        {
            return (T)Enum.Parse(typeof(T), mode);
        }
    }
}
