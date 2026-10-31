#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineHandGuidanceEntries
{
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.HandCardGuidance> BuildHumanHandGuidance(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> legalActions) => _resolve().BuildHumanHandGuidance(legalActions);
}
