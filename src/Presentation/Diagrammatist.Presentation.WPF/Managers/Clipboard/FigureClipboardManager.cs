using Diagrammatist.Application.AppServices.Figures.Services;
using Diagrammatist.Presentation.WPF.Mappers.Figures;
using Diagrammatist.Presentation.WPF.Models.Figures;
using System.Windows;
using ClipboardInstance = System.Windows.Clipboard;

namespace Diagrammatist.Presentation.WPF.Managers.Clipboard
{
    /// <summary>
    /// A class that manages <see cref="ClipboardInstance"/> operations for <see cref="FigureModel"/>.
    /// </summary>
    public class FigureClipboardManager : IClipboardManager<FigureModel>
    {
        private const string FigureClipboardFormat = $"{nameof(Diagrammatist)}.Figure";

        private readonly IFigureSerializationService _figureSerializationService;

        public FigureClipboardManager(IFigureSerializationService figureSerializationService)
        {
            _figureSerializationService = figureSerializationService;
        }

        /// <inheritdoc/>
        /// <param name="obj">Target figure.</param>
        public void CopyToClipboard(FigureModel obj)
        {
            var dataObject = new DataObject();
            dataObject.SetData(FigureClipboardFormat, _figureSerializationService.Serialize(obj.ToDomain()));
            ClipboardInstance.SetDataObject(dataObject, true);
        }

        /// <inheritdoc/>
        /// <returns>a <see cref="FigureModel"/>.</returns>
        public FigureModel? PasteFromClipboard()
        {
            if (ClipboardInstance.ContainsData(FigureClipboardFormat) && ClipboardInstance.GetData(FigureClipboardFormat) is byte[] data)
            {
                return _figureSerializationService.Deserialize(data)?.ToModel();
            }
            return null;
        }
    }
}
