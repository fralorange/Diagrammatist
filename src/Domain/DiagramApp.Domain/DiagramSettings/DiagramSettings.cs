namespace DiagramApp.Domain.DiagramSettings
{
    public class DiagramSettings
    {
        public string FileName { get; set; } = string.Empty;
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 512;
        public BackgroundType Background { get; set; } = BackgroundType.White;
        public DiagramType Type { get; set; } = DiagramType.Custom;
    }
}
