using Diagrammatist.Presentation.WPF.Core.Shared.Enums;

namespace Diagrammatist.Presentation.WPF.Core.Shared.Entities
{
    /// <summary>
    /// A record that represents a confirmation response.
    /// </summary>
    /// <param name="Result"></param>
    /// <param name="DoNotShowAgain"></param>
    public record ConfirmationResponse(ConfirmationResult Result, bool DoNotShowAgain);
}
