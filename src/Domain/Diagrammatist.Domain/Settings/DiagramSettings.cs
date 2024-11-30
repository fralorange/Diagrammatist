﻿using Diagrammatist.Domain.Settings.Constants;
using System.Drawing;

namespace Diagrammatist.Domain.Settings
{
    /// <summary>
    /// Diagram settings.
    /// </summary>
    public class DiagramSettings
    {
        /// <summary>
        /// Gets or sets diagram file name.
        /// </summary>
        /// <remarks>
        /// This property used to set file name.
        /// </remarks>
        public string FileName { get; set; } = DiagramSettingsConstants.DefaultFileName;
        /// <summary>
        /// Gets or sets diagram canvas width.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas width.
        /// </remarks>
        public int Width { get; set; } = DiagramSettingsConstants.DefaultWidth;
        /// <summary>
        /// Gets or sets diagram canvas height.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram's canvas height.
        /// </remarks>
        public int Height { get; set; } = DiagramSettingsConstants.DefaultHeight;
        /// <summary>
        /// Gets or sets diagram canvas background color.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram background color.
        /// </remarks>
        public Color Background { get; set; } = DiagramSettingsConstants.DefaultBackground;
        /// <summary>
        /// Gets or sets diagram type.
        /// </summary>
        /// <remarks>
        /// This property used to store diagram type.
        /// </remarks>
        public DiagramType Type { get; set; } = DiagramSettingsConstants.DefaultType;
    }
}
