#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreResolutionIdentityBoundaryEntries
{
    internal long _resolutionSequence { get => _resolve()._resolutionSequence; set => _resolve()._resolutionSequence = value; }
}
