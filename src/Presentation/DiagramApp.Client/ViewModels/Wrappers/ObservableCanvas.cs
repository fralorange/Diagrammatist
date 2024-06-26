﻿using CommunityToolkit.Mvvm.ComponentModel;
using DiagramApp.Domain.Canvas;
using DiagramApp.Domain.DiagramSettings;
using System.Collections.ObjectModel;

namespace DiagramApp.Client.ViewModels.Wrappers
{
    public partial class ObservableCanvas : ObservableObject
    {
        private readonly Canvas _canvas;

        private readonly Stack<Action> _undoCommands = [];
        private readonly Stack<Action> _redoCommands = [];

        [ObservableProperty]
        private bool _canUndo;

        [ObservableProperty]
        private bool _canRedo;

        public ObservableCanvas(Canvas canvas)
        {
            _canvas = canvas;
            Zoom = _canvas.Zoom;
            Offset = new ObservableOffset(_canvas.Offset);
        }

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBlocked))]
        private bool _isBlocked;
        public bool IsNotBlocked => !IsBlocked;
        public event EventHandler<object>? BlockedResourcesReceived;

        [ObservableProperty]
        private double _rotation = 0;

        public ObservableCollection<ObservableFigure> Figures { get; } = [];
        //change to collection l8r if multiple selection needed
        [ObservableProperty]
        private ObservableFigure? _selectedFigure = null;

        public int ImaginaryWidth
        {
            get => _canvas.ImaginaryWidth;
            set => SetProperty(_canvas.ImaginaryWidth, value, _canvas, (c, iw) => c.ImaginaryWidth = iw);
        }

        public int ImaginaryHeight
        {
            get => _canvas.ImaginaryHeight;
            set => SetProperty(_canvas.ImaginaryHeight, value, _canvas, (c, ih) => c.ImaginaryHeight = ih);
        }

        public double Zoom
        {
            get => _canvas.Zoom;
            set => SetProperty(_canvas.Zoom, value, _canvas, (c, z) => c.Zoom = z);
        }

        public ControlsType Controls
        {
            get => _canvas.Controls;
            set => SetProperty(_canvas.Controls, value, _canvas, (c, ctrls) => c.Controls = ctrls);
        }

        public ObservableOffset Offset { get; }

        public DiagramSettings Settings
        {
            get => _canvas.Settings;
            set => SetProperty(_canvas.Settings, value, _canvas, (c, sett) => c.Settings = sett);
        }

        internal virtual void OnBlockedResourcesReceived(object obj) => BlockedResourcesReceived?.Invoke(this, obj);
        public async Task<TResult> BlockAsync<TResult>()
        {
            var tcs = new TaskCompletionSource<TResult>();

            EventHandler<object>? handler = null;
            handler = (sender, result) =>
            {
                IsBlocked = false;
                BlockedResourcesReceived -= handler;
                tcs.SetResult((TResult)result);
            };
            BlockedResourcesReceived += handler;

            IsBlocked = true;
            ChangeControls("Select");
            DeselectFigure();

            var result = await tcs.Task;
            return result;
        }
        private void UpdateUndoRedoState()
        {
            CanUndo = _undoCommands.Count != 0;
            CanRedo = _redoCommands.Count != 0;
        }
        public void Undo()
        {
            if (CanUndo)
            {
                _undoCommands.Pop().Invoke();
                UpdateUndoRedoState();
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _redoCommands.Pop().Invoke();
                UpdateUndoRedoState();
            }
        }

        public void ClearRedoCommands() => _redoCommands.Clear();

        public void AddUndoCommand(Action command)
        {
            _undoCommands.Push(command);
            UpdateUndoRedoState();
        }

        public void AddRedoCommand(Action command)
        {
            _redoCommands.Push(command);
            UpdateUndoRedoState();
        }

        public void ZoomIn(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomIn(zoomFactor, mouseX, mouseY);
            ZoomChanged();
        }

        public void ZoomOut(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            _canvas.ZoomOut(zoomFactor, mouseX, mouseY);
            ZoomChanged();
        }

        public void ZoomReset()
        {
            _canvas.Zoom = 1;
            ZoomChanged();
        }

        public void MoveCanvas(double deltaX, double deltaY)
        {
            _canvas.MoveCanvas(deltaX, deltaY);
            OnPropertyChanged(nameof(Offset));
        }

        public void ChangeControls(string controlName)
        {
            _canvas.ChangeControls(controlName);
            OnPropertyChanged(nameof(Controls));
        }

        public void UpdateSettings(DiagramSettings settings)
        {
            _canvas.UpdateSettings(settings);
            OnPropertyChanged(nameof(Settings));
            ZoomChanged();
        }

        public void SelectFigure(ObservableFigure figure)
        {
            if (SelectedFigure is not null)
            {
                SelectedFigure.IsSelected = false;
            }

            SelectedFigure = figure;
            SelectedFigure.IsSelected = true;
        }

        public void DeselectFigure()
        {
            if (SelectedFigure is not null)
            {
                SelectedFigure!.IsSelected = false;
                SelectedFigure = null;
            }
        }

        private void ZoomChanged()
        {
            OnPropertyChanged(nameof(Zoom));
            OnPropertyChanged(nameof(Offset));
            OnPropertyChanged(nameof(ImaginaryWidth));
            OnPropertyChanged(nameof(ImaginaryHeight));
        }
    }
}
