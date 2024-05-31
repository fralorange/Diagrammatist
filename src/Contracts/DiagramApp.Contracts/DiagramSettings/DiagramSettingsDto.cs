namespace DiagramApp.Contracts.DiagramSettings
{
    public class DiagramSettingsDto
    {
        public required string FileName { get; set; }
        public required int Width { get; set; }
        public required int Height { get; set; }
        public required string Background { get; set; }
        public required string Type { get; set; }
    }
}
