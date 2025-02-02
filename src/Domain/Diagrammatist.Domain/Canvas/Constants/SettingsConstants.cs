﻿using System.Drawing;

namespace Diagrammatist.Domain.Canvas.Constants
{
    /// <summary>
    /// Diagram Settings constants.
    /// </summary>
    public static class SettingsConstants
    {
        /// <summary>
        /// Default value for diagram's file name.
        /// </summary>
        public const string DefaultFileName = "";
        /// <summary>
        /// Default value for diagram canvas width.
        /// </summary>
        public const int DefaultWidth = 512;
        /// <summary>
        /// Default value for diagram canvas height.
        /// </summary>
        public const int DefaultHeight = 512;
        /// <summary>
        /// Default value for diagram canvas background.
        /// </summary>
        public static readonly Color DefaultBackground = Color.DimGray;
        /// <summary>
        /// Default value for diagram's type.
        /// </summary>
        public const Diagrams DefaultType = Diagrams.Custom;
    }
}
