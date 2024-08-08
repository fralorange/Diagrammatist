using DiagramSettingsEntity = DiagramApp.Domain.DiagramSettings.DiagramSettings;

namespace DiagramApp.Domain.Canvas
{
    public class Canvas
    {
        public int ImaginaryWidth { get; set; }
        public int ImaginaryHeight { get; set; }
        public required DiagramSettingsEntity Settings { get; set; }
        public double Zoom { get; set; } = 1;
        public double Rotation { get; set; } = 0;
        public Offset Offset { get; set; } = new();
        public ControlsType Controls { get; set; } = ControlsType.Select;
    }
}
