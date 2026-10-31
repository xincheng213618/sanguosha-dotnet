#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AdjacentDiscardAndRoundAlcoholCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries> getCoreGameEngineCurrentSlashFireOffersRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries> getCoreGameEngineAdjacentDiscardStorageRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUsePaymentsEntries> getCoreGameEngineCompletedUsePaymentsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundPricedPileAlcoholEntries> getCoreGameEngineRoundPricedPileAlcoholRuntime) : IAdjacentDiscardAndRoundAlcoholHost
{
    public SkillProgramStepOutcome OfferCurrentSlashFireAndExtraTarget(ProgramSkillFrame frame) => getCoreGameEngineCurrentSlashFireOffersRuntime().BeginCurrentSlashFireOffer(frame);
    public SkillProgramStepOutcome StoreAdjacentDiscardedSlash(ProgramSkillFrame frame, CardZoneKind zone) => getCoreGameEngineAdjacentDiscardStorageRuntime().BeginAdjacentDiscardStorage(frame, zone);
    public SkillProgramStepOutcome PayCompletedUseDiscardOrLoseHp(ProgramSkillFrame frame) => getCoreGameEngineCompletedUsePaymentsRuntime().BeginCompletedUsePayment(frame);
    public SkillProgramStepOutcome UseRoundPricedPileDyingAlcohol(ProgramSkillFrame frame, string stateId, CardZoneKind zone) => getCoreGameEngineRoundPricedPileAlcoholRuntime().BeginRoundPileAlcohol(frame, stateId, zone);
}
