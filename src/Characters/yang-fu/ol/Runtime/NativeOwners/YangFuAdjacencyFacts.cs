#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class YangFuAdjacencyFacts(Func<RuntimeEntries.CoreGameEngineCardConversionsEntries> getCoreGameEngineCardConversionsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineJudgedRankSlashPoliciesEntries> getCoreGameEngineJudgedRankSlashPoliciesRuntime, Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, Func<RuntimeEntries.CoreGameEngineRoundGainedSourceUsesEntries> getCoreGameEngineRoundGainedSourceUsesRuntime, Func<RuntimeEntries.CoreGameEngineCurrentSlashFireOffersEntries> getCoreGameEngineCurrentSlashFireOffersRuntime, Func<RuntimeEntries.CharacterLiuPanOlEntries> getCharacterLiuPanOlRuntime, Func<RuntimeEntries.CharacterQianZhaoOlEntries> getCharacterQianZhaoOlRuntime, Func<RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries> getCoreGameEngineDesignatedExtraTargetsRuntime, Func<RuntimeEntries.CoreGameEngineRecipientCategorySlashTargetsEntries> getCoreGameEngineRecipientCategorySlashTargetsRuntime, Func<RuntimeEntries.CharacterZhangYanOlEntries> getCharacterZhangYanOlRuntime, Func<RuntimeEntries.CoreDyingCursorRuntimeEntries> getCoreDyingCursorRuntimeRuntime, Func<RuntimeEntries.CoreGameEngineObserverSubtreesEntries> getCoreGameEngineObserverSubtreesRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.CharacterYanPuOlEntries> getCharacterYanPuOlRuntime, Func<RuntimeEntries.CoreGameEngineDeckEquipmentProgramsEntries> getCoreGameEngineDeckEquipmentProgramsRuntime, Func<RuntimeEntries.CoreGameEngineRecoveryReplacementDamageRidesEntries> getCoreGameEngineRecoveryReplacementDamageRidesRuntime) : IYangFuAdjacencyFacts
{
    public bool IgnoresSlashLimit(int owner, CardConversionSource source) => getCoreGameEngineCardConversionsRuntime().IgnoresProgramSlashLimit(matchParticipantsState.ReferenceToPlayers[owner], source);
    public bool HasIssuedJudgedRankSlashQuota(long useId, int actor) => getCoreGameEngineJudgedRankSlashPoliciesRuntime().HasIssuedJudgedRankSlashQuota(useId, actor);
    public bool HasIssuedWuLuKangDixianUse(long useId, int actor) => getCharacterLuKangWumiaoRuntime().HasIssuedWuLuKangDixianUse(useId, actor);
    public bool HasIssuedRoundGainedBasicBonus(long useId, int actor) => getCoreGameEngineRoundGainedSourceUsesRuntime().HasIssuedRoundGainedBasicBonus(useId, actor);
    public void AssertCurrentSlashFirePolicy(CardUseFrame use) => getCoreGameEngineCurrentSlashFireOffersRuntime().AssertCurrentSlashFirePolicy(use);
    public bool HasPijingTargetTail(CardUseFrame use) => getCharacterLiuPanOlRuntime().HasPijingTargetTail(use);
    public bool HasWeifuTargetTail(CardUseFrame use) => getCharacterQianZhaoOlRuntime().HasWeifuTargetTail(use);
    public bool HasDesignatedExtraTargetTail(CardUseFrame use) => getCoreGameEngineDesignatedExtraTargetsRuntime().HasDesignatedExtraTargetTail(use);
    public bool HasRecipientCategorySlashTargetTail(CardUseFrame use) => getCoreGameEngineRecipientCategorySlashTargetsRuntime().HasRecipientCategorySlashTargetTail(use);
    public bool HasZhangYanLangdaoTargetTail(CardUseFrame use) => getCharacterZhangYanOlRuntime().HasZhangYanLangdaoTargetTail(use);
    public DyingFrame? ActiveDying => getCoreDyingCursorRuntimeRuntime().ActiveDying;

    public bool MovementObserverEdge(long childId) => getCoreGameEngineObserverSubtreesRuntime().IsMovementObserverEdge(getFrameStore().FindIndex(frame => frame.Id == childId));
    public bool DyingExitEdge(long childId) => getCharacterYanPuOlRuntime().IsYanPuDyingExitEdge(getFrameStore().FindIndex(frame => frame.Id == childId));
    public bool RandomEquipmentRide(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineDeckEquipmentProgramsRuntime().RandomEquipmentFrameRidesOn(child, parent);
    public bool RecoveryReplacementRide(ResolutionFrame child, ResolutionFrame parent) => getCoreGameEngineRecoveryReplacementDamageRidesRuntime().RecoveryReplacementFrameRidesOn(child, parent);
    public bool SuspendedOrPaidRescueSuffix(DyingFrame dying) => getCoreGameEngineObserverSubtreesRuntime().IsSuspendedOrPaidRescueObserverSuffix(getFrameStore().FindIndex(frame => frame.Id == dying.Id), dying);
}
