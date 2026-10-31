#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetHandVisibilityEntries
{
    internal bool HasProgramVisibleTargetHandDuringTrick(int source, CardKind kind) => _resolve().HasProgramVisibleTargetHandDuringTrick(source, kind);
    internal PendingDecision ProjectVisibleTargetHandDecision(PendingDecision decision, int viewerSeat) => _resolve().ProjectVisibleTargetHandDecision(decision, viewerSeat);
    internal bool RefreshVisibleTargetHandSelectionPrompt() => _resolve().RefreshVisibleTargetHandSelectionPrompt();
    internal IReadOnlyList<PromptChoice> VisibleTargetHandChoices(TargetCardSelectionFrame selection, IReadOnlyList<PromptChoice> choices, bool includeMetadata) => _resolve().VisibleTargetHandChoices(selection, choices, includeMetadata);
}
