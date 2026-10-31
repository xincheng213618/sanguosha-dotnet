#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawEntries
{
    internal void AssertSoleIncomingEffectRounds(CardUseFrame use) => _resolve().AssertSoleIncomingEffectRounds(use);
    internal void ContinueSoleIncomingRangeDraw(long id) => _resolve().ContinueSoleIncomingRangeDraw(id);
    internal bool FinishSoleIncomingAttackRangeDraw(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishSoleIncomingAttackRangeDraw(attack, out paused);
    internal bool IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => _resolve().IsSoleIncomingRangeDrawMovement(use, batch);
    internal bool IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => _resolve().IsSoleIncomingRangeDrawMovementReturn(use, window);
    internal bool TryBeginSoleIncomingAttackRangeDraw(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginSoleIncomingAttackRangeDraw(attack);
    internal bool TryBeginSoleIncomingRangeDraw(long id) => _resolve().TryBeginSoleIncomingRangeDraw(id);
}
