#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesEntries
{
    internal bool ContinueDeferredTurnEndPrelude(DeferredTurnEndFrame parent) => _resolve().ContinueDeferredTurnEndPrelude(parent);
    internal CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(CardMovementBatchContext batch, long? resumeProgramFrameId = default) => _resolve().CreateCardsMovedProgramWindow(batch, resumeProgramFrameId);
    internal bool IsActualDeferredTurnEndPrelude(DeferredTurnEndFrame parent) => _resolve().IsActualDeferredTurnEndPrelude(parent);
    internal bool MatchesDeferredTurnEndPreludeChild(DeferredTurnEndFrame parent, CardsMovedTriggerWindowFrame child) => _resolve().MatchesDeferredTurnEndPreludeChild(parent, child);
    internal void ReturnDeferredTurnEndPrelude(CardsMovedTriggerWindowFrame child) => _resolve().ReturnDeferredTurnEndPrelude(child);
}
