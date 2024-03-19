using DiagramSettingsEntity = DiagramApp.Domain.DiagramSettings.DiagramSettings;

namespace DiagramApp.Domain.Canvas
{
    public class Canvas
    {
        public (int X, int Y) ImaginaryBorderTopLeft { get; private set; }
        public (int X, int Y) ImaginaryBorderBottomRight { get; private set; }
        public DiagramSettingsEntity Settings { get; private set; }
        public double Zoom { get; set; } = 1;
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public ControlsType Controls { get; set; } = ControlsType.Select;

        public Canvas(DiagramSettingsEntity Settings)
        {
            this.Settings = Settings;
            PositionX = Settings.Width / 2;
            PositionY = Settings.Height / 2;
        }

        public void UpdateImaginaryBorders()
        {
            int halfWidth = Settings.Width / 2;
            int halfHeight = Settings.Height / 2;

            ImaginaryBorderTopLeft = (PositionX - (int)(halfWidth * Zoom), PositionY - (int)(halfHeight * Zoom));
            ImaginaryBorderBottomRight = (PositionX + (int)(halfWidth * Zoom), PositionY + (int)(halfHeight * Zoom));
        }

        public void MoveCanvas(int deltaX, int deltaY)
        {
            int newPositionX = PositionX + deltaX;
            int newPositionY = PositionY + deltaY;

            if (newPositionX >= ImaginaryBorderTopLeft.X && newPositionX <= ImaginaryBorderBottomRight.Y)
            {
                PositionX = newPositionX;
            }

            if (newPositionY >= ImaginaryBorderTopLeft.Y && newPositionY <= ImaginaryBorderBottomRight.Y)
            {
                PositionY = newPositionY;
            }
        }

        public void ZoomIn(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            int newPositionX = PositionX;
            int newPositionY = PositionY;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                newPositionX = PositionX - (int)((mouseX.Value - PositionX) * zoomFactor);
                newPositionY = PositionY - (int)((mouseY.Value - PositionY) * zoomFactor);
            }

            PositionX = newPositionX;
            PositionY = newPositionY;
            Zoom = Math.Min(2, Zoom + zoomFactor);

            UpdateImaginaryBorders();
            EnsureCanvasWithinBorders();
        }

        public void ZoomOut(double zoomFactor, int? mouseX = null, int? mouseY = null)
        {
            int newPositionX = PositionX;
            int newPositionY = PositionY;

            if (mouseX.HasValue && mouseY.HasValue)
            {
                newPositionX = PositionX + (int)((mouseX.Value - PositionX) * zoomFactor);
                newPositionY = PositionY + (int)((mouseY.Value - PositionY) * zoomFactor);
            }

            PositionX = newPositionX;
            PositionY = newPositionY;
            Zoom = Math.Max(0.5f, Zoom - zoomFactor);

            UpdateImaginaryBorders();
            EnsureCanvasWithinBorders();
        }

        private void EnsureCanvasWithinBorders()
        {
            if (PositionX < ImaginaryBorderTopLeft.X || PositionX > ImaginaryBorderBottomRight.X)
            {
                PositionX = Math.Max(ImaginaryBorderTopLeft.X, Math.Min(PositionX, ImaginaryBorderBottomRight.X));
            }

            if (PositionY < ImaginaryBorderTopLeft.Y || PositionY > ImaginaryBorderBottomRight.Y)
            {
                PositionY = Math.Max(ImaginaryBorderTopLeft.Y, Math.Min(PositionY, ImaginaryBorderBottomRight.Y));
            }
        }
    }
}
