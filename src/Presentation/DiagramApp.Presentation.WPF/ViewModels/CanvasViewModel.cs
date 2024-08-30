using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DiagramApp.Contracts.Canvas;
using DiagramApp.Contracts.Figures;
using DiagramApp.Contracts.Settings;
using DiagramApp.Infrastructure.Extensions.ObservableCollection;
using DiagramApp.Presentation.WPF.ViewModels.Enums.Modes;
using System.Collections.ObjectModel;

namespace DiagramApp.Presentation.WPF.ViewModels
{
    /// <summary>
    /// A view model class for canvas component.
    /// </summary>
    public sealed partial class CanvasViewModel : ObservableRecipient
    {
        private CanvasDto? _currentCanvas;

        /// <summary>
        /// Gets or sets current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to store current canvas values.
        /// </remarks>
        public CanvasDto? CurrentCanvas
        {
            get => _currentCanvas;
            private set => SetProperty(ref _currentCanvas, value, true);
        }

        private MouseMode _currentMouseMode;

        /// <summary>
        /// Gets or sets current mouse mode.
        /// </summary>
        /// <remarks>
        /// This property used to configure current mouse mode.
        /// </remarks>
        public MouseMode CurrentMouseMode
        {
            get => _currentMouseMode;
            private set => SetProperty(ref _currentMouseMode, value);
        }

        /// <summary>
        /// Gets imaginary width from current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to configure ScrollViewer's X-Axis.
        /// </remarks>
        public int ImaginaryWidth
        {
            get => CurrentCanvas?.ImaginaryWidth ?? default;
            private set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.ImaginaryWidth, value, CurrentCanvas, (c, w) => c.ImaginaryWidth = w);
                }
            }
        }

        /// <summary>
        /// Gets imaginary height from current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to configure ScrollViewer's Y-Axis.
        /// </remarks>
        public int ImaginaryHeight
        {
            get => CurrentCanvas?.ImaginaryWidth ?? default;
            private set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.ImaginaryHeight, value, CurrentCanvas, (c, h) => c.ImaginaryHeight = h);
                }
            }
        }

        /// <summary>
        /// Gets diagram settings from current canvas.
        /// </summary>
        /// <remarks>
        /// This property used to configure UI Canvas.
        /// </remarks>
        public DiagramSettingsDto Settings
        {
            get => CurrentCanvas?.Settings!;
            private set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.Settings, value, CurrentCanvas, (c, s) => c.Settings = s);
                }
            }
        }

        /// <summary>
        /// Gets zoom parameter.
        /// </summary>
        /// <remarks>
        /// This property used to configure canvas scale.
        /// </remarks>
        public double Zoom
        {
            get => CurrentCanvas?.Zoom ?? default;
            private set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.Zoom, value, CurrentCanvas, (c, z) => c.Zoom = z);
                }
            }
        }

        /// <summary>
        /// Gets rotation parameter.
        /// </summary>
        /// <remarks>
        /// This property used to configure canvas rotation.
        /// </remarks>
        public double Rotation
        {
            get => CurrentCanvas?.Rotation ?? default;
            private set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.Rotation, value, CurrentCanvas, (c, r) => c.Rotation = r);
                }
            }
        }

        /// <summary>
        /// Gets or sets screen offset.
        /// </summary>
        /// <remarks>
        /// This property used to configure canvas position in window.
        /// </remarks>
        public ScreenOffsetDto ScreenOffset
        {
            get => CurrentCanvas?.ScreenOffset!;
            set
            {
                if (CurrentCanvas is not null)
                {
                    SetProperty(CurrentCanvas.ScreenOffset, value, CurrentCanvas, (c, so) => c.ScreenOffset = so);
                }
            }
        }

        /// <summary>
        /// Gets or sets collection of <see cref="FigureDto"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store figures that placed on the canvas.
        /// </remarks>
        public ObservableCollection<FigureDto>? Figures { get; set; }

        /// <summary>
        /// Gets or sets selected figure.
        /// </summary>
        /// <remarks>
        /// This property used to store selected figure that placed on the canvas.
        /// </remarks>
        [ObservableProperty]
        private FigureDto? _selectedFigure;

        public CanvasViewModel()
        {
            IsActive = true;
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            Messenger.Register<CanvasViewModel, PropertyChangedMessage<CanvasDto?>>(this, (r, m) =>
            {
                CurrentCanvas = m.NewValue;

                Figures = CurrentCanvas?.Figures is not null
                    ? new ObservableCollection<FigureDto>(CurrentCanvas.Figures)
                    : null;

                Figures?.LinkTo(CurrentCanvas!.Figures);
            });

            Messenger.Register<CanvasViewModel, PropertyChangedMessage<MouseMode>>(this, (r, m) =>
            {
                CurrentMouseMode = m.NewValue;
            });
        }
    }
}
