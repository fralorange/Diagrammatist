using Diagrammatist.Presentation.WPF.Core.Controls;

namespace Diagrammatist.Presentation.WPF.Views.Dialogs
{
    /// <summary>
    /// A class that represents 'about' dialog window and derives from <see cref="TitleBarWindow"/>.
    /// </summary>
    /// <remarks>
    /// This window shows information about the app.
    /// </remarks>
    public partial class AboutAppDialog : TitleBarWindow
    {
        public AboutAppDialog()
        {
            InitializeComponent();
        }
    }
}
