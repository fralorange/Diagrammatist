using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Core.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Core.Models.Figures;
using System.Windows;
using ClipboardInstance = System.Windows.Clipboard;

namespace Diagrammatist.Presentation.WPF.Core.Services.Clipboard.Figure
{
    /// <summary>
    /// A class that manages <see cref="ClipboardInstance"/> operations for <see cref="FigureModel"/>.
    /// </summary>
    public class FigureClipboardService : IClipboardService<FigureModel>
    {
        private const string FigureClipboardFormat = $"{nameof(Diagrammatist)}.Figure";

        private readonly IFigureSerializationService _figureSerializationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FigureClipboardService"/> class.
        /// </summary>
        /// <param name="figureSerializationService"></param>
        public FigureClipboardService(IFigureSerializationService figureSerializationService)
        {
            _figureSerializationService = figureSerializationService;
        }

        /// <inheritdoc/>
        /// <param name="obj">Target figure.</param>
        public void CopyToClipboard(FigureModel obj, string key = "")
        {
            var format = $"{FigureClipboardFormat}{key}";

            var dataObject = new DataObject();
            dataObject.SetData(format, _figureSerializationService.Serialize(obj.ToDomain()));
            ClipboardInstance.SetDataObject(dataObject, true);
        }

        /// <inheritdoc/>
        /// <returns>a <see cref="FigureModel"/>.</returns>
        public FigureModel? GetFromClipboard(string key = "")
        {
            var format = $"{FigureClipboardFormat}{key}";

            if (ClipboardInstance.ContainsData(format) && ClipboardInstance.GetData(format) is byte[] data)
            {
                return _figureSerializationService.Deserialize(data)?.ToModel();
            }
            return null;
        }
    }
}
