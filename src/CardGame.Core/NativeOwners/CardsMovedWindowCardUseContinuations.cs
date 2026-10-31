#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardsMovedWindowCardUseContinuations(Func<RuntimeEntries.CoreGameEngineGuanduCardsEntries> getCoreGameEngineGuanduCardsRuntime, Func<RuntimeEntries.CharacterGuanYuWumiaoEntries> getCharacterGuanYuWumiaoRuntime, Func<RuntimeEntries.CoreGameEngineDragonBoatCardsEntries> getCoreGameEngineDragonBoatCardsRuntime, Func<RuntimeEntries.CoreGameEngineWenheCardsEntries> getCoreGameEngineWenheCardsRuntime, Func<RuntimeEntries.CoreGameEngineSoleIncomingRangeDrawEntries> getCoreGameEngineSoleIncomingRangeDrawRuntime, Func<RuntimeEntries.CoreNativeCardEffectDrawsEntries> getCoreNativeCardEffectDrawsRuntime, Func<RuntimeEntries.CoreGameEngineFriendDuelResponsesEntries> getCoreGameEngineFriendDuelResponsesRuntime, Func<RuntimeEntries.CoreRecoveryReplacementEntries> getCoreRecoveryReplacementRuntime, Func<RuntimeEntries.CharacterWangCanOlEntries> getCharacterWangCanOlRuntime, Func<RuntimeEntries.CharacterGuoSiOlEntries> getCharacterGuoSiOlRuntime, Func<RuntimeEntries.CoreGameEngineEndingHistoricalUsesEntries> getCoreGameEngineEndingHistoricalUsesRuntime, Func<RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries> getCoreGameEngineRoundPricedPileAlcoholRuntime, Func<RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries> getCoreGameEngineUnrespondableCounterspellsRuntime, Func<RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime, Func<RuntimeEntries.CoreGameEngineColorFireAttacksEntries> getCoreGameEngineColorFireAttacksRuntime) : ICardsMovedWindowCardUseContinuations
{
    public void ContinueGuanduCardEffect(long id) => getCoreGameEngineGuanduCardsRuntime().ContinueGuanduCardEffect(id);
    public void ContinueWumiaoFlood(long id) => getCharacterGuanYuWumiaoRuntime().ContinueWumiaoFlood(id);
    public void ContinueDragonBoatCardEffect(long id) => getCoreGameEngineDragonBoatCardsRuntime().ContinueDragonBoatCardEffect(id);
    public void ContinueWenheCardEffect(long id) => getCoreGameEngineWenheCardsRuntime().ContinueWenheCardEffect(id);
    public void ContinueSoleIncomingRangeDraw(long id) => getCoreGameEngineSoleIncomingRangeDrawRuntime().ContinueSoleIncomingRangeDraw(id);
    public void ContinueNativeDrawTwo(long id) => getCoreNativeCardEffectDrawsRuntime().ContinueNativeDrawTwo(id);
    public void ContinueFriendDuelResponse(long id) => getCoreGameEngineFriendDuelResponsesRuntime().ContinueFriendDuelResponse(id);
    public void ContinueRecoveryPaidCardUse(long useId) => getCoreRecoveryReplacementRuntime().ContinueRecoveryPaidCardUse(useId);
    public void ContinueDenglouNativePayment(long id) => getCharacterWangCanOlRuntime().ContinueDenglouNativePayment(id);
    public void ContinueGuoSiNativePayment(long id) => getCharacterGuoSiOlRuntime().ContinueGuoSiNativePayment(id);
    public void AssertGuoSiNativeUse(CardUseFrame use) => getCharacterGuoSiOlRuntime().AssertGuoSiNativeUse(use);
    public void ContinueHistoricalEndingPayment(long id) => getCoreGameEngineEndingHistoricalUsesRuntime().ContinueHistoricalEndingPayment(id);
    public void ContinueRoundPileAlcoholUse(long id) => getCoreGameEngineRoundPricedPileAlcoholRuntime().ContinueRoundPileAlcoholUse(id);
    public void ContinuePolicyCounterspellPayment(long windowId) => getCoreGameEngineUnrespondableCounterspellsRuntime().ContinuePolicyCounterspellPayment(windowId);
    public void ContinueMouZhitianFirePayment(long id) => getCharacterZhugeLiangMouRuntime().ContinueMouZhitianFirePayment(id);
    public void ContinueColorFireAttackPayment(long useId) => getCoreGameEngineColorFireAttacksRuntime().ContinueColorFireAttackPayment(useId);
}
