using CommunityToolkit.Mvvm.ComponentModel;
using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Collections.ObjectModel;

namespace Diagrammatist.Presentation.WPF.Models.Canvas
{
    /// <summary>
    /// A canvas model.
    /// </summary>
    public partial class CanvasModel : ObservableObject
    {
        /// <summary>
        /// Gets or sets width of invisible border.
        /// </summary>
        /// <remarks>
        /// This property used to prevent user from scrolling by X-axis in to the abyss...
        /// </remarks>
        [ObservableProperty]
        private int _imaginaryWidth;

        /// <summary>
        /// Gets or sets height of invisible border.
        /// </summary>
        /// <remarks>
        /// This property used to prevent user from scrolling by Y-axis in to the abyss...
        /// </remarks>
        [ObservableProperty]
        private int _imaginaryHeight;

        /// <summary>
        /// Gets or sets diagram settings.
        /// </summary>
        /// <remarks>
        /// This property used to configure canvas.
        /// </remarks>
        [ObservableProperty]
        private SettingsModel _settings;

        /// <summary>
        /// Gets or sets zoom parameter.
        /// </summary>
        /// <remarks>
        /// This property used to set canvas scale.
        /// </remarks>
        [ObservableProperty]
        private double _zoom;

        /// <summary>
        /// Gets or sets screen offset.
        /// </summary>
        /// <remarks>
        /// This property used to determine canvas position in window.
        /// </remarks>
        [ObservableProperty]
        private OffsetModel _offset;

        /// <summary>
        /// Gets or sets figure collection.
        /// </summary>
        /// <remarks>
        /// This property used to set drawing context for canvas.
        /// </remarks>
        public ObservableCollection<FigureModel> Figures { get; set; } = [];

        /// <summary>
        /// Gets or sets 'has changes' flag.
        /// </summary>
        /// <remarks>
        /// This property used to determine whether canvas has changes or not.
        /// </remarks>
        [ObservableProperty]
        private bool _hasChanges;
    }
}
