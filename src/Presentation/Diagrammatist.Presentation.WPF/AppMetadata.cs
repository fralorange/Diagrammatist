﻿using System.Reflection;

namespace Diagrammatist.Presentation.WPF
{
    /// <summary>
    /// A static class that represents app metadata.
    /// </summary>
    public class AppMetadata
    {
        /// <summary>
        /// Gets the name of the app pipe used for inter-process communication.
        /// </summary>
        public const string AppPipe = "DiagrammatistPipe";

        /// <summary>
        /// Gets app version.
        /// </summary>
        public static string Version => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
            .InformationalVersion.Split('+')[0] ?? string.Empty;

        /// <summary>
        /// Gets app title.
        /// </summary>
        public static string Title => Assembly
            .GetExecutingAssembly()
            .GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? string.Empty;
    }
}
