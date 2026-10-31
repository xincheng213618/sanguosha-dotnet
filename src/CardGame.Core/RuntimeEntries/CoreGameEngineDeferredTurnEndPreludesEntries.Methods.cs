#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesEntries
{
    internal bool ContinueDeferredTurnEndPrelude(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().ContinueDeferredTurnEndPrelude(parent);
    internal global::CardGame.Core.CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(global::CardGame.Core.CardMovementBatchContext batch, long? resumeProgramFrameId = default) => _resolve().CreateCardsMovedProgramWindow(batch, resumeProgramFrameId);
    internal bool HasDeferredTurnEndPreludeBoundary(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().HasDeferredTurnEndPreludeBoundary(parent);
    internal bool IsActualDeferredTurnEndPrelude(global::CardGame.Core.DeferredTurnEndFrame parent) => _resolve().IsActualDeferredTurnEndPrelude(parent);
    internal bool MatchesDeferredTurnEndPreludeChild(global::CardGame.Core.DeferredTurnEndFrame parent, global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().MatchesDeferredTurnEndPreludeChild(parent, child);
    internal void ReturnDeferredTurnEndPrelude(global::CardGame.Core.CardsMovedTriggerWindowFrame child) => _resolve().ReturnDeferredTurnEndPrelude(child);
}
