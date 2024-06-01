using DiagramApp.Contracts.Canvas.Figures;
using DiagramApp.Contracts.DiagramSettings;

namespace DiagramApp.Contracts.Canvas
{
    public class CanvasDto
    {
        public required int ImaginaryWidth { get; set; }
        public required int ImaginaryHeight { get; set; }
        public required DiagramSettingsDto Settings { get; set; }
        public required double Zoom { get; set; }
        public required OffsetDto Offset { get; set; }
        public required string Controls { get; set; }
        public required bool IsGridVisible { get; set; }
        public required double GridSpacing { get; set; }
        public required double Rotation { get; set; }
        public List<FigureDto> Figures { get; set; } = [];
        public string FileLocation { get; set; } = string.Empty;
    }
}
