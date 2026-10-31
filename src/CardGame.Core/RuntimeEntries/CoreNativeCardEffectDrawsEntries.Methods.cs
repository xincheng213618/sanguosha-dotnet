#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeCardEffectDrawsEntries
{
    internal void AssertNativeCardEffectDraw(CardUseFrame use) => _resolve().AssertNativeCardEffectDraw(use);
    internal void ContinueNativeDrawTwo(long id) => _resolve().ContinueNativeDrawTwo(id);
    internal bool IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsNativeCardEffectDrawMovement(use, batch);
    internal bool IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => _resolve().IsNativeCardEffectDrawMovementReturn(use, frame);
    internal bool TryBeginNativeDrawTwo(NullificationWindowFrame pending) => _resolve().TryBeginNativeDrawTwo(pending);
}
