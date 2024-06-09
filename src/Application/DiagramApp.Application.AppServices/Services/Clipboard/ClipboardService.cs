using DiagramApp.Contracts.Canvas.Figures;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DiagramApp.Application.AppServices.Services.Clipboard
{
    public class ClipboardService : IClipboardService
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public string ToClipboardString(FigureDto figure)
        {
            return JsonConvert.SerializeObject(figure, _settings);
        }

        public FigureDto? ToObjectFromClipboard(string clipboard)
        {
            if (!JsonIsValid(clipboard))
                return null;
            return JsonConvert.DeserializeObject<FigureDto>(clipboard, _settings);
        }

        private bool JsonIsValid(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
