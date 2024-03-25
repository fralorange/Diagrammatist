using DiagramSettingsEntity = DiagramApp.Domain.DiagramSettings.DiagramSettings;

namespace DiagramApp.Domain.Canvas
{
    public class Canvas
    {
        private readonly (int X, int Y) _center = (0, 0);
        public int ImaginaryWidth { get; set; }
        public int ImaginaryHeight { get; set; }
        public DiagramSettingsEntity Settings { get; private set; }
        public double Zoom { get; set; } = 1;
        public Offset Offset { get; set; } = new();
        public ControlsType Controls { get; set; } = ControlsType.Select;

        public Canvas(DiagramSettingsEntity Settings)
        {
            this.Settings = Settings;

            UpdateImaginaryBorders();
        }

        public void MoveCanvas(double newX, double newY)
        {
            Offset.X = newX;
            Offset.Y = newY;
        }

        public void ZoomIn(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            //double newPositionX = OffsetX;
            //double newPositionY = OffsetY;

            //if (mouseX.HasValue && mouseY.HasValue)
            //{
            //    newPositionX = OffsetX - (int)((mouseX.Value - OffsetX) * zoomFactor);
            //    newPositionY = OffsetY - (int)((mouseY.Value - OffsetY) * zoomFactor);
            //}

            //OffsetX = newPositionX;
            //OffsetY = newPositionY;
            Zoom = Math.Min(2, Zoom + zoomFactor);

            UpdateImaginaryBorders();
            EnsureCanvasWithinBorders();
        }

        public void ZoomOut(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            //double newPositionX = OffsetX;
            //double newPositionY = OffsetY;

            //if (mouseX.HasValue && mouseY.HasValue)
            //{
            //    newPositionX = OffsetX + (int)((mouseX.Value - OffsetX) * zoomFactor);
            //    newPositionY = OffsetY + (int)((mouseY.Value - OffsetY) * zoomFactor);
            //}

            //OffsetX = newPositionX;
            //OffsetY = newPositionY;
            Zoom = Math.Max(0.5f, Zoom - zoomFactor);

            UpdateImaginaryBorders();
            EnsureCanvasWithinBorders();
        }

        private void UpdateImaginaryBorders()
        {
            //int borderWidth = (int)(Settings.Width * 4 * Zoom);
            //int borderHeight = (int)(Settings.Height * 4 * Zoom);
            int borderWidth = Settings.Width * 3;
            int borderHeight = Settings.Height * 3;

            int borderCenterX = _center.X;
            int borderCenterY = _center.Y;

            ImaginaryWidth = Math.Abs(borderCenterX - borderWidth / 2) + Math.Abs(borderCenterX + borderWidth / 2);
            ImaginaryHeight = Math.Abs(borderCenterY - borderHeight / 2) + Math.Abs(borderCenterY + borderHeight / 2);
        }

        private void EnsureCanvasWithinBorders()
        {
            if (Offset.X < 0 || Offset.X > ImaginaryWidth)
            {
                Offset.X = Math.Max(0, Math.Min(Offset.X, ImaginaryWidth));
            }

            if (Offset.Y < 0 || Offset.Y > ImaginaryHeight)
            {
                Offset.Y = Math.Max(0, Math.Min(Offset.Y, ImaginaryHeight));
            }
        }
    }
}
