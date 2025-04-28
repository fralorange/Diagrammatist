using Diagrammatist.Presentation.WPF.Core.Controls;

namespace Diagrammatist.Presentation.WPF.Views.Dialogs
{
    /// <summary>
    /// A class that represents 'change canvas size' dialog windows and derives from <see cref="TitleBarWindow"/>.
    /// </summary>
    /// <remarks>
    /// This class used to change canvas size through changing width and height of the current canvas.
    /// </remarks>
    public partial class ChangeCanvasSizeDialog : TitleBarWindow
    {
        public ChangeCanvasSizeDialog()
        {
            InitializeComponent();
        }
    }
}
