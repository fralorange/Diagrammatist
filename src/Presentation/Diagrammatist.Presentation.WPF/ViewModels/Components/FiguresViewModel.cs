using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Core.Factories.Figures.Line;
using Diagrammatist.Presentation.WPF.Core.Foundation.Extensions;
using Diagrammatist.Presentation.WPF.Core.Helpers;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Messaging.RequestMessages;
using Diagrammatist.Presentation.WPF.Core.Models.Connection;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Magnetic;
using Diagrammatist.Presentation.WPF.Core.Models.Figures.Special.Container;
using Diagrammatist.Presentation.WPF.Core.Services.Connection;
using Diagrammatist.Presentation.WPF.Core.Services.Figure.Placement;
using Diagrammatist.Presentation.WPF.Core.Shared.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed partial class FiguresViewModel : ObservableRecipient
    {
        private readonly IFigureService _figureService;
        private readonly IConnectionService _connectionService;
        private readonly IFigurePlacementService _figurePlacementService;
        private readonly ILineFactory _lineFactory;

        /// <summary>
        /// Occurs when a requested user input confirmed.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user presses "checkmark" button and passes line figure data.
        /// </remarks>
        public event Action<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?>? LineInputConfirmed;
        /// <summary>
        /// Occurs when a requested user input cancelled.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user presses "cross" button.
        /// </remarks>
        public event Action? LineInputCancelled;

        /// <summary>
        /// Gets or sets <see cref="ObservableCollection{T}"/> of <see cref="FigureModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store figures that placed on canvas.
        /// </remarks>
        [ObservableProperty]
        private ObservableCollection<FigureModel>? _canvasFigures;

        /// <summary>
        /// Gets or sets <see cref="ObservableCollection{T}"/> of <see cref="ConnectionModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store connections that placed on canvas.
        /// </remarks>
        [ObservableProperty]
        private ObservableCollection<ConnectionModel>? _canvasConnections;

        /// <summary>
        /// Gets or sets dictionary with string keys and <see cref="List{T}"/> of <see cref="FigureModel"/> pairs.
        /// </summary>
        /// <remarks>
        /// This property used to store toolbox figures
        /// </remarks>
        [ObservableProperty]
        private Dictionary<string, List<FigureModel>>? _figures;

        /// <summary>
        /// Gets or sets key value pair of string to <see cref="List{T}"/> of <see cref="FigureModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to show filtered <see cref="_figures"/> by category.
        /// </remarks>
        [ObservableProperty]
        private KeyValuePair<string, List<FigureModel>> _selectedCategory;

        /// <summary>
        /// Gets or sets <see cref="FigureModel"/> selected figure.
        /// </summary>
        /// <remarks>
        /// This property used to get selected figure from toolbox.
        /// </remarks>
        [ObservableProperty]
        private FigureModel? _selectedFigure;

        private MouseMode _currentMouseMode;

        /// <include file='../../../docs/common/CommonXmlDocComments.xml' path='CommonXmlDocComments/Behaviors/Member[@name="CurrentMouseMode"]/*'/>
        public MouseMode CurrentMouseMode
        {
            get => _currentMouseMode;
            private set => SetProperty(ref _currentMouseMode, value, true);
        }

        /// <summary>
        /// Initializes a new figures view model.
        /// </summary>
        /// <param name="figureService">A figure service.</param>
        /// <param name="connectionManager">A connection manager.</param>
        /// <param name="figurePlacementService"></param>
        /// <param name="lineFactory"></param>
        public FiguresViewModel(IFigureService figureService,
                                IConnectionService connectionManager,
                                IFigurePlacementService figurePlacementService,
                                ILineFactory lineFactory)
        {
            _figureService = figureService;
            _connectionService = connectionManager;
            _figurePlacementService = figurePlacementService;
            _lineFactory = lineFactory;

            IsActive = true;
        }

        #region Figure Properties Handlers

        private void LoadFigureColors(FigureModel figure)
        {
            var bgColor = (System.Windows.Media.Color)App.Current.Resources["AppBackground"];
            var textColor = (System.Windows.Media.Color)App.Current.Resources["AppTextColor"];
            var themeColor = (System.Windows.Media.Color)App.Current.Resources["AppThemeColor"];

            switch (figure)
            {
                case LineFigureModel line:
                    line.BackgroundColor = themeColor;
                    break;
                case ContainerFigureModel containerFigure:
                    containerFigure.TextColor = textColor;
                    containerFigure.BackgroundColor = bgColor;
                    break;
                case TextFigureModel textFigure:
                    textFigure.TextColor = textColor;
                    textFigure.BackgroundColor = bgColor;
                    break;
                default:
                    figure.BackgroundColor = bgColor;
                    break;
            }
        }

        /// <summary>
        /// Loads figures in <see cref="Figures"/> externally and asynchronously.
        /// </summary>
        public async Task LoadFiguresAsync()
        {
            var figureDomains = await _figureService.GetAsync();

            var figureModels = figureDomains.ToDictionary(
                pair => pair.Key,
                pair => pair.Value.Select(figure =>
                {
                    var model = figure.ToModel();

                    LoadFigureColors(model);

                    return model;
                }).ToList()
            );

            Figures = figureModels;
        }

        private void UpdateFigureColors()
        {
            if (Figures is null)
                return;

            foreach (var category in Figures.Values)
            {
                foreach (var figure in category)
                {
                    LoadFigureColors(figure);
                }
            }
        }

        private T CloneAndRename<T>(T template) where T : FigureModel
        {
            var clone = (T)template.Clone();
            var translatedName = FigureNameHelper.GetTranslatedName(clone.Name);

            var existingNames = CanvasFigures?.Select(f => f.Name);
            clone.Name = FigureNameHelper.GetUniqueName(translatedName, existingNames?.ToList());

            return clone;
        }

        private void PlaceFigureInVisibleArea(FigureModel figure)
        {
            var visibleArea = Messenger.Send<VisibleAreaRequestMessage>(new());

            if (!visibleArea.Response.IsEmpty)
            {
                figure.PosX = visibleArea.Response.Left;
                figure.PosY = visibleArea.Response.Top;
            }
            else
            {
                Messenger.Send<ScrollToFigureMessage>(new(figure));
            }
        }

        #endregion

        #region Figure Creation Handlers

        /// <summary>
        /// Checks if the template is ready to be added to the canvas.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private bool IsReadyToAdd(FigureModel? template)
        {
            return template != null
                && CanvasFigures != null
                && CanvasConnections != null;
        }

        /// <summary>
        /// Handles the creation of a line figure based on the provided template.
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private async Task<LineFigureModel?> HandleLineFigureAsync(LineFigureModel template)
        {
            var prevMode = CurrentMouseMode;
            CurrentMouseMode = MouseMode.Line;

            var input = await WaitForUserInputAsync();

            CurrentMouseMode = prevMode;
            if (input == null) return null;

            var (points, start, end) = input.Value;
            var figure = _lineFactory.CreateLine(
                    template,
                    start,
                    end,
                    CanvasConnections!
                );

            var existingNames = CanvasFigures?.Select(f => f.Name);
            figure.Name = FigureNameHelper.GetUniqueName(FigureNameHelper.GetTranslatedName(figure.Name), existingNames);
            figure.Points = points.ToObservableCollection();

            AddConnectionIfNeeded(start, end, figure);
            return figure;
        }

        /// <summary>
        /// Adds a connection between two magnetic points if they are not null.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="line"></param>
        private void AddConnectionIfNeeded(
            MagneticPointModel? start,
            MagneticPointModel? end,
            LineFigureModel line)
        {
            if (start == null || end == null)
                return;

            var connection = new ConnectionModel
            {
                SourceMagneticPoint = start,
                DestinationMagneticPoint = end,
                Line = line
            };

            _connectionService.AddConnection(CanvasConnections!, connection);
        }

        /// <summary>
        /// Handles the creation of a shape figure based on the provided template.
        /// </summary>
        /// <param name="template"></param>
        private ShapeFigureModel HandleShapeFigure(ShapeFigureModel template)
        {
            var shape = CloneAndRename(template);
            shape!.UpdateMagneticPoints();

            return shape;
        }

        /// <summary>
        /// Handles the creation of a text figure based on the provided template.
        /// </summary>
        /// <param name="template"></param>
        private FigureModel HandleDefaultFigure(FigureModel template)
        {
            var figure = CloneAndRename(template);

            return figure;
        }

        #endregion

        /// <summary>
        /// Adds new figure to canvas component.
        /// </summary>
        [RelayCommand]
        private async Task AddFigureAsync()
        {
            var template = SelectedFigure;
            SelectedFigure = null;

            if (!IsReadyToAdd(template))
                return;

            var figure = template switch
            {
                LineFigureModel lineTemplate => await HandleLineFigureAsync(lineTemplate),
                ShapeFigureModel shapeTemplate => HandleShapeFigure(shapeTemplate),
                FigureModel defaultTemplate => HandleDefaultFigure(defaultTemplate),
                _ => throw new NotSupportedException($"Unsupported figure type: {template!.GetType()}")
            };

            if (figure is null)
                return;

            _figurePlacementService.AddFigureWithUndo(figure, CanvasFigures!, CanvasConnections!);

            PlaceFigureInVisibleArea(figure);
        }

        /// <summary>
        /// Waits for user input to confirm or cancel line drawing.
        /// </summary>
        /// <returns></returns>
        private Task<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?> WaitForUserInputAsync()
        {
            var tcs = new TaskCompletionSource<(List<Point> points, MagneticPointModel? start, MagneticPointModel? end)?>();

            void OnUserConfirmed((List<Point> points, MagneticPointModel? start, MagneticPointModel? end)? lineData)
            {
                tcs.TrySetResult(lineData);

                Cleanup();
            }

            void OnUserCancelled()
            {
                tcs.TrySetResult(null);

                Cleanup();
            }

            void Cleanup()
            {
                LineInputConfirmed -= OnUserConfirmed;
                LineInputCancelled -= OnUserCancelled;
            }

            LineInputConfirmed += OnUserConfirmed;
            LineInputCancelled += OnUserCancelled;

            return tcs.Task;
        }

        /// <inheritdoc/>
        protected override void OnActivated()
        {
            base.OnActivated();

            Messenger.Register<FiguresViewModel, ThemeChangedMessage>(this, (r, m) => UpdateFigureColors());

            Messenger.Register<FiguresViewModel, LineDrawResultMessage>(this, (r, m) =>
            {
                if (m.Value is null)
                {
                    LineInputCancelled?.Invoke();
                }
                else
                {
                    var points = m.Value.Value.points;

                    var mStart = m.Value.Value.start;
                    var mEnd = m.Value.Value.end;

                    LineInputConfirmed?.Invoke((points, mStart, mEnd));
                }
            });

            Messenger.Register<FiguresViewModel, PropertyChangedMessage<MouseMode>>(this, (r, m) =>
            {
                CurrentMouseMode = m.NewValue;
            });

            Messenger.Register<FiguresViewModel, PropertyChangedMessage<ObservableCollection<FigureModel>?>>(this, (r, m) =>
            {
                if (m.Sender.GetType() == typeof(CanvasViewModel) &&
                    m.PropertyName == nameof(CanvasViewModel.CurrentCanvas.Figures))
                {
                    CanvasFigures = m.NewValue;
                }
            });

            Messenger.Register<FiguresViewModel, PropertyChangedMessage<ObservableCollection<ConnectionModel>?>>(this, (r, m) =>
            {
                if (m.Sender.GetType() == typeof(CanvasViewModel) &&
                    m.PropertyName == nameof(CanvasViewModel.CurrentCanvas.Connections))
                {
                    CanvasConnections = m.NewValue;
                }
            });
        }
    }
}
