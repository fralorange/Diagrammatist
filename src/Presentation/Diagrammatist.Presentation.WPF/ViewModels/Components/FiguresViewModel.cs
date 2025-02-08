using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Core.Commands.Helpers.Undoable;
using Diagrammatist.Presentation.WPF.Core.Commands.Managers;
using Diagrammatist.Presentation.WPF.Core.Foundation.Base.ObservableObject.Args;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Messaging.Messages;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using Diagrammatist.Presentation.WPF.ViewModels.Components.Enums.Modes;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Diagrammatist.Presentation.WPF.ViewModels.Components
{
    /// <summary>
    /// A view model class for figures (toolbox) component.
    /// </summary>
    public sealed partial class FiguresViewModel : ObservableRecipient
    {
        private readonly IFigureService _figureService;
        private readonly ITrackableCommandManager _trackableCommandManager;

        /// <summary>
        /// Occurs when a requested user input confirmed.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user presses "checkmark" button and passes line figure data.
        /// </remarks>
        public event Action<(List<Point> points, Point point)?>? LineInputConfirmed;
        /// <summary>
        /// Occurs when a requested user input cancelled.
        /// </summary>
        /// <remarks>
        /// This event is triggered when user presses "cross" button.
        /// </remarks>
        public event Action? LineInputCancelled;

        /// <summary>
        /// Flag that determines whether figure changes in the moment or not.
        /// </summary>
        private bool _figureChanges;

        /// <summary>
        /// Gets or sets <see cref="ObservableCollection{T}"/> of <see cref="FigureModel"/>.
        /// </summary>
        /// <remarks>
        /// This property used to store figures that placed on canvas.
        /// </remarks>
        [ObservableProperty]
        private ObservableCollection<FigureModel>? _canvasFigures;

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
        /// <param name="trackableCommandManager">A command manager.</param>
        public FiguresViewModel(IFigureService figureService, ITrackableCommandManager trackableCommandManager)
        {
            _figureService = figureService;
            _trackableCommandManager = trackableCommandManager;

            IsActive = true;
        }

        private void LoadFigureColors(FigureModel figure)
        {
            var bgColor = (System.Windows.Media.Color)App.Current.Resources["AppBackground"];
            var textColor = (System.Windows.Media.Color)App.Current.Resources["AppTextColor"];
            var themeColor = (System.Windows.Media.Color)App.Current.Resources["AppThemeColor"];

            switch (figure)
            {
                case LineFigureModel line:
                    line.BackgroundColor = Color.FromArgb(themeColor.A, themeColor.R, themeColor.G, themeColor.B);
                    break;
                case TextFigureModel textFigure:
                    textFigure.TextColor = Color.FromArgb(textColor.A, textColor.R, textColor.G, textColor.B);
                    textFigure.BackgroundColor = Color.FromArgb(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
                    break;
                default:
                    figure.BackgroundColor = Color.FromArgb(bgColor.A, bgColor.R, bgColor.G, bgColor.B);
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

            var updateBgColor = (System.Windows.Media.Color)App.Current.Resources["AppBackground"];
            var updateThemeColor = (System.Windows.Media.Color)App.Current.Resources["AppThemeColor"];

            foreach (var category in Figures.Values)
            {
                foreach (var figure in category)
                {
                    LoadFigureColors(figure);
                }
            }
        }

        /// <summary>
        /// Adds new figure to canvas component.
        /// </summary>
        [RelayCommand]
        private async Task AddFigureAsync()
        {
            var figureTemplate = SelectedFigure;
            SelectedFigure = null;

            if (CanvasFigures is null || figureTemplate is null)
            {
                return;
            }

            if (figureTemplate is LineFigureModel)
            {
                var previousMouseMode = CurrentMouseMode;

                CurrentMouseMode = MouseMode.Line;

                var inputTask = WaitForUserInputAsync();

                var result = await inputTask;

                CurrentMouseMode = previousMouseMode;

                if (result is null)
                {
                    return;
                }

                var figure = (figureTemplate.Clone() as LineFigureModel)!;

                figure.PosX = result.Value.point.X;
                figure.PosY = result.Value.point.Y;
                figure.Points = result.Value.points;

                AddTrackedFigure(figure);
            }
            else
            {
                var figure = figureTemplate.Clone();

                AddTrackedFigure(figure);
            }
        }

        private void AddTrackedFigure(FigureModel figure)
        {
            // Track changes in figure properties.
            figure.ExtendedPropertyChanged += OnPropertyChanged;

            // Track addition to canvas.
            var command = CommonUndoableHelper.CreateUndoableCommand(
                () => CanvasFigures!.Add(figure),
                () => CanvasFigures!.Remove(figure)
            );

            _trackableCommandManager.Execute(command);
        }

        private void OnPropertyChanged(object? sender, ExtendedPropertyChangedEventArgs e)
        {
            if (sender is FigureModel figure && e.PropertyName is not null && !_figureChanges)
            {
                var command = CommonUndoableHelper.CreateUndoableCommand(
                    () => SetFigureProperty(figure, e.PropertyName, e.NewValue),
                    () => SetFigureProperty(figure, e.PropertyName, e.OldValue)
                );

                _trackableCommandManager.Execute(command);
            }
        }

        private void SetFigureProperty(FigureModel figure, string propertyName, object? value)
        {
            _figureChanges = true;
            try
            {
                var property = figure.GetType().GetProperty(propertyName);

                property?.SetValue(figure, value);
            }
            finally
            {
                _figureChanges = false;
            }
        }

        private Task<(List<Point> points, Point point)?> WaitForUserInputAsync()
        {
            var tcs = new TaskCompletionSource<(List<Point> points, Point point)?>();

            void OnUserConfirmed((List<Point> points, Point point)? lineData)
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
                    var position = m.Value.Value.point;

                    var positionX = Convert.ToInt32(position.X);
                    var positionY = Convert.ToInt32(position.Y);

                    var drawingPosition = new Point(positionX, positionY);

                    LineInputConfirmed?.Invoke((points.Select(point =>
                    {
                        int x = Convert.ToInt32(point.X);
                        int y = Convert.ToInt32(point.Y);

                        return new Point(x, y);
                    }).ToList(), drawingPosition));
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
        }
    }
}
