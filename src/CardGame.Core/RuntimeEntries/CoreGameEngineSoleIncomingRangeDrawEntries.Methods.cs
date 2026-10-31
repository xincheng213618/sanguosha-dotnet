#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingRangeDrawEntries
{
    internal void AssertSoleIncomingEffectRounds(global::CardGame.Core.CardUseFrame use) => _resolve().AssertSoleIncomingEffectRounds(use);
    internal void ContinueSoleIncomingRangeDraw(long id) => _resolve().ContinueSoleIncomingRangeDraw(id);
    internal bool FinishSoleIncomingAttackRangeDraw(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishSoleIncomingAttackRangeDraw(attack, out paused);
    internal bool HasSoleIncomingRangeDrawProcessing(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().HasSoleIncomingRangeDrawProcessing(processing);
    internal bool IsSoleIncomingRangeDrawMovement(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsSoleIncomingRangeDrawMovement(use, batch);
    internal bool IsSoleIncomingRangeDrawMovementReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardsMovedTriggerWindowFrame window) => _resolve().IsSoleIncomingRangeDrawMovementReturn(use, window);
    internal bool IsSoleIncomingRangeDrawStateReturn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().IsSoleIncomingRangeDrawStateReturn(use, window);
    internal bool TryBeginSoleIncomingAttackRangeDraw(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginSoleIncomingAttackRangeDraw(attack);
    internal bool TryBeginSoleIncomingRangeDraw(long id) => _resolve().TryBeginSoleIncomingRangeDraw(id);
}
