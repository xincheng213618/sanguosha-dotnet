#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardEffectDrawsEntries
{
    internal void AssertNativeCardEffectDraw(global::CardGame.Core.CardUseFrame use) => _resolve().AssertNativeCardEffectDraw(use);
    internal void ContinueNativeDrawTwo(long id) => _resolve().ContinueNativeDrawTwo(id);
    internal bool HasNativeCardEffectDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().HasNativeCardEffectDrawProcessing(processing);
    internal bool IsNativeCardEffectDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsNativeCardEffectDrawMovement(use, batch);
    internal bool IsNativeCardEffectDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame frame) => _resolve().IsNativeCardEffectDrawMovementReturn(use, frame);
    internal bool IsNativeCardEffectDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().IsNativeCardEffectDrawStateReturn(use, frame);
    internal bool TryBeginNativeDrawTwo(global::CardGame.Core.NullificationWindowFrame pending) => _resolve().TryBeginNativeDrawTwo(pending);
}
