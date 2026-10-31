#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowCardUseProofs(Func<RuntimeEntries.CoreProgramCardUsePaymentsEntries> getCoreProgramCardUsePaymentsRuntime, Func<RuntimeEntries.CoreGameEngineCardUsePaymentsEntries> getCoreGameEngineCardUsePaymentsRuntime, Func<RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries> getCoreGameEngineSoleIncomingRangeDrawRuntime, Func<RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime, Func<RuntimeEntries.CoreGameEngineGuanduCardsEntries> getCoreGameEngineGuanduCardsRuntime, Func<RuntimeEntries.CoreGameEngineDragonBoatCardsEntries> getCoreGameEngineDragonBoatCardsRuntime, Func<RuntimeEntries.CoreGameEngineWenheCardsEntries> getCoreGameEngineWenheCardsRuntime, Func<RuntimeEntries.CoreNativeCardEffectDrawsEntries> getCoreNativeCardEffectDrawsRuntime, Func<RuntimeEntries.CoreGameEngineFriendDuelResponsesEntries> getCoreGameEngineFriendDuelResponsesRuntime) : ICardsMovedWindowCardUseProofs
{
    public bool HasPaidCardUseMovementTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => getCoreProgramCardUsePaymentsRuntime().HasPaidCardUseMovementTail(use, continuation);
    public bool IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => getCoreGameEngineCardUsePaymentsRuntime().IsCardUsePaymentBatch(use, batch);
    public bool IsSoleIncomingRangeDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => getCoreGameEngineSoleIncomingRangeDrawRuntime().IsSoleIncomingRangeDrawMovement(use, batch);
    public bool IsWumiaoFloodMovement(CardUseFrame use, CardMovementBatchContext batch) => getCharacterGuanYuWumiaoRuntime().IsWumiaoFloodMovement(use, batch);
    public bool IsGuanduCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => getCoreGameEngineGuanduCardsRuntime().IsGuanduCardEffectMovement(use, batch);
    public bool IsDragonBoatCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => getCoreGameEngineDragonBoatCardsRuntime().IsDragonBoatCardEffectMovement(use, batch);
    public bool IsWenheCardEffectMovement(CardUseFrame use, CardMovementBatchContext batch) => getCoreGameEngineWenheCardsRuntime().IsWenheCardEffectMovement(use, batch);
    public bool IsNativeCardEffectDrawMovement(CardUseFrame use, CardMovementBatchContext batch) => getCoreNativeCardEffectDrawsRuntime().IsNativeCardEffectDrawMovement(use, batch);
    public bool IsGuanduCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => getCoreGameEngineGuanduCardsRuntime().IsGuanduCardEffectMovementReturn(use, frame);
    public bool IsWumiaoFloodMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => getCharacterGuanYuWumiaoRuntime().IsWumiaoFloodMovementReturn(use, frame);
    public bool IsDragonBoatCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => getCoreGameEngineDragonBoatCardsRuntime().IsDragonBoatCardEffectMovementReturn(use, frame);
    public bool IsWenheCardEffectMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => getCoreGameEngineWenheCardsRuntime().IsWenheCardEffectMovementReturn(use, movement);
    public bool IsSoleIncomingRangeDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame window) => getCoreGameEngineSoleIncomingRangeDrawRuntime().IsSoleIncomingRangeDrawMovementReturn(use, window);
    public bool IsNativeCardEffectDrawMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame frame) => getCoreNativeCardEffectDrawsRuntime().IsNativeCardEffectDrawMovementReturn(use, frame);
    public bool IsFriendDuelResponseMovementReturn(FriendDuelResponseFrame parent, CardsMovedTriggerWindowFrame movement) => getCoreGameEngineFriendDuelResponsesRuntime().IsFriendDuelResponseMovementReturn(parent, movement);
    public bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => getCoreGameEngineCardUsePaymentsRuntime().IsPaidCardUseMovementReturn(use, movement);
}
