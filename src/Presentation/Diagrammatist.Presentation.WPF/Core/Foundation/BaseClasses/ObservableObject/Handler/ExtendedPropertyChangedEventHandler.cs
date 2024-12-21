using Diagrammatist.Presentation.WPF.Core.Foundation.BaseClasses.ObservableObject.Args;

namespace Diagrammatist.Presentation.WPF.Core.Foundation.BaseClasses.ObservableObject.Handler
{
    /// <summary>
    /// Handles event with <see cref="ExtendedPropertyChangedEventArgs"/>.
    /// </summary>
    /// <param name="sender">Sender instance.</param>
    /// <param name="e">Args.</param>
    public delegate void ExtendedPropertyChangedEventHandler(object? sender, ExtendedPropertyChangedEventArgs e);
}
