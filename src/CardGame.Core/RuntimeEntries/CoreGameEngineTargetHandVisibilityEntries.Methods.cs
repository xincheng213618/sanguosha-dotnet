#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTargetHandVisibilityEntries
{
    internal bool HasProgramVisibleTargetHandDuringTrick(int source, global::CardGame.Core.CardKind kind) => _resolve().HasProgramVisibleTargetHandDuringTrick(source, kind);
    internal global::CardGame.Core.PendingDecision ProjectVisibleTargetHandDecision(global::CardGame.Core.PendingDecision decision, int viewerSeat) => _resolve().ProjectVisibleTargetHandDecision(decision, viewerSeat);
    internal bool RefreshVisibleTargetHandSelectionPrompt() => _resolve().RefreshVisibleTargetHandSelectionPrompt();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> VisibleTargetHandChoices(global::CardGame.Core.TargetCardSelectionFrame selection, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices, bool includeMetadata) => _resolve().VisibleTargetHandChoices(selection, choices, includeMetadata);
}
