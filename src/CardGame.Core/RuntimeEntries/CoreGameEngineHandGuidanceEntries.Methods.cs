#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHandGuidanceEntries
{
    internal IReadOnlyList<HandCardGuidance> BuildHumanHandGuidance(IReadOnlyList<LegalAction> legalActions) => _resolve().BuildHumanHandGuidance(legalActions);
}
