using Diagrammatist.Presentation.WPF.Framework.BaseClasses.ObservableObject.Args;

namespace Diagrammatist.Presentation.WPF.Framework.BaseClasses.ObservableObject.Handler
{
    /// <summary>
    /// Handles event with <see cref="ExtendedPropertyChangedEventArgs"/>.
    /// </summary>
    /// <param name="sender">Sender instance.</param>
    /// <param name="e">Args.</param>
    public delegate void ExtendedPropertyChangedEventHandler(object? sender, ExtendedPropertyChangedEventArgs e);
}
