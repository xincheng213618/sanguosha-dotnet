#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ActivePlayerEffectQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoCaoOlMagicEntries> getCharacterCaoCaoOlMagicRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGongsunZanMouOlEntries> getCharacterGongsunZanMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouOlEntries> getCharacterHuaXiongMouOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime, global::CardGame.Core.EngineStates.CoreGameEngineNextSlashProgramsState gameEngineNextSlashProgramsState, global::CardGame.Core.EngineStates.CoreGameEngineNextCardTargetAdjustmentsState gameEngineNextCardTargetAdjustmentsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries> getCoreGameEngineNextCardTargetAdjustmentsRuntime, global::CardGame.Core.EngineStates.CoreDirectedTurnCardPoliciesState directedTurnCardPoliciesState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::CardGame.Core.EngineStates.CoreGameEngineCurrentTurnSkillSuppressionsState gameEngineCurrentTurnSkillSuppressionsState, global::CardGame.Core.EngineStates.CoreGameEngineAlcoholSlashSuppressionsState gameEngineAlcoholSlashSuppressionsState, global::CardGame.Core.EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, global::CardGame.Core.EngineStates.CoreGameEngineProgramPhaseColorRestrictionsState gameEngineProgramPhaseColorRestrictionsState, global::CardGame.Core.EngineStates.CoreGameEngineCurrentTurnShownCardBenefitsState gameEngineCurrentTurnShownCardBenefitsState, global::CardGame.Core.EngineStates.CoreGameEngineActorHandLimitPenaltiesState gameEngineActorHandLimitPenaltiesState, global::CardGame.Core.EngineStates.CoreGameEngineDeferredBenefitsProgramsState gameEngineDeferredBenefitsProgramsState, global::CardGame.Core.EngineStates.CoreGameEngineOriginalTargetAdditionsState gameEngineOriginalTargetAdditionsState, global::CardGame.Core.EngineStates.CoreGameEngineRedAdditionalTargetsState gameEngineRedAdditionalTargetsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnHandLimitKindExemptionsEntries> getCoreGameEngineTurnHandLimitKindExemptionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries> getCoreGameEngineBoundDiscardSlashBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedPoliciesEntries> getCoreGameEngineAdvancedPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime, global::CardGame.Core.EngineStates.CharacterHuangChengYanOlState characterHuangChengYanOlState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime, global::CardGame.Core.EngineStates.CoreGameEngineConvertingGiftsState gameEngineConvertingGiftsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries> getCoreGameEngineFixedDistanceOneDebtRuntime, global::CardGame.Core.EngineStates.CoreGameEngineProgramPersistentCombatPoliciesState gameEngineProgramPersistentCombatPoliciesState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries> getCoreGameEngineHalfHandPhaseDebtProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaYiOlMagicEntries> getCharacterSimaYiOlMagicRuntime) : IActivePlayerEffectQueries
{
    public bool UsesAbsoluteSeatNumbers => getCoreNativeDamageHandlesRuntime().IsTeamMode || getCoreNativeDamageHandlesRuntime().IsNationalWarMode;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
    public string? MoCaoCaoEquipmentDescription(int seat) => getCharacterCaoCaoOlMagicRuntime().MoCaoCaoEquipmentDescription(seat);
    public bool MoCaoCaoHasHandPrivilege(int seat) => getCharacterCaoCaoOlMagicRuntime().MoCaoCaoHasHandPrivilege(seat);
    public IEnumerable<MouGongsunZanBaojingGrantedEvent> ActiveMouGszBaojing(int seat) => getCharacterGongsunZanMouOlRuntime().ActiveMouGszBaojing(seat);
    public string? MouHxYangweiPublicDescription(int seat) => getCharacterHuaXiongMouOlRuntime().MouHxYangweiPublicDescription(seat);
    public string? WuLuKangDixianPublicDescription(int seat) => getCharacterLuKangWumiaoRuntime().WuLuKangDixianPublicDescription(seat);
    public string? WuZhangFeiPublicCycleDescription(int seat, IReadOnlyList<GeneralSkillDefinition>? primary, IReadOnlyList<GeneralSkillDefinition>? secondary) => getCharacterZhangFeiWumiaoRuntime().WuZhangFeiPublicCycleDescription(seat, primary, secondary);
    public int NextSlashReserveCount => gameEngineNextSlashProgramsState.ReferenceToNamedNextSlashReserves.Count;

    public ActiveNextSlashReserveFact NextSlashReserveAt(int index)
    {
        var item = gameEngineNextSlashProgramsState.ReferenceToNamedNextSlashReserves[index];
        return new(item.OwnerSeat, item.SkillId, item.TurnNumber, item.TurnSeat, item.Amount);
    }

    public CardConversionSource? NextCardTargetAdjustmentOwner(int seat) => gameEngineNextCardTargetAdjustmentsState.ReferenceToNextCardTargetAdjustmentOwners.GetValueOrDefault(seat);
    public bool HasLegacyNextCardTargetAdjustment(CharacterState player) => getCoreGameEngineNextCardTargetAdjustmentsRuntime().HasLegacyNextCardTargetAdjustment(player);
    public IReadOnlyList<DirectedTurnCardPolicy> DirectedTurnCardPolicies => directedTurnCardPoliciesState.ReferenceToDirectedTurnCardPolicies;

    public bool HasRuntimeSkillInstance(int seat, string skillId, string skillInstanceId) => getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(matchParticipantsState.ReferenceToPlayers[seat], skillId, skillInstanceId);
    public IReadOnlyList<CurrentTurnNonLockedSkillSuppression> CurrentTurnSkillSuppressions => gameEngineCurrentTurnSkillSuppressionsState.ReferenceToCurrentTurnSkillSuppressions;
    public IReadOnlyList<CurrentTurnOwnSkillSuppression> CurrentTurnOwnSkillSuppressions => gameEngineAlcoholSlashSuppressionsState.ReferenceToCurrentTurnOwnSkillSuppressions;
    public int SkillSuppressionCount => gameEngineStrategicProgramsState.ReferenceToProgramSuppressions.Count;

    public ActiveSkillSuppressionFact SkillSuppressionAt(int index)
    {
        var item = gameEngineStrategicProgramsState.ReferenceToProgramSuppressions[index];
        return new(item.SourceSeat, item.TargetSeat, item.SkillId);
    }

    public IReadOnlyList<ProgramPhaseColorRestriction> PhaseColorRestrictions => gameEngineProgramPhaseColorRestrictionsState.ReferenceToProgramPhaseColorRestrictions;
    public IReadOnlyList<CurrentTurnDirectedHeartSlashBonus> HeartSlashBonuses => gameEngineCurrentTurnShownCardBenefitsState.ReferenceToCurrentTurnHeartSlashBonuses;
    public IReadOnlyList<ActorHandLimitPenalty> ActorHandLimitPenalties => gameEngineActorHandLimitPenaltiesState.ReferenceToActorHandLimitPenalties;
    public IReadOnlyList<ProgramNextTurnRuleModifier> NextTurnRuleModifiers => gameEngineDeferredBenefitsProgramsState.ReferenceToProgramNextTurnRuleModifiers;
    public IReadOnlyList<OriginalTargetAdditionGrant> OriginalTargetAdditionGrants => gameEngineOriginalTargetAdditionsState.ReferenceToOriginalTargetAdditionGrants;

    public ActiveRedTargetFact? RedAdditionalTargetGrant(int seat) => gameEngineRedAdditionalTargetsState.ReferenceToRedAdditionalTargetGrants.TryGetValue(seat, out var grant) ? new(grant.Source, grant.Maximum) : null;
    public IReadOnlyList<TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat) => getCoreGameEngineTurnHandCategoryRestrictionsRuntime().GetTurnHandCategoryRestrictionSnapshot(seat);
    public IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int seat) => getCoreGameEngineTurnHandLimitKindExemptionsRuntime().GetTurnHandLimitCardKindExemptionsSnapshot(seat);
    public IReadOnlyList<FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int seat) => getCoreGameEngineBoundDiscardSlashBenefitsRuntime().GetFirstRoundGameUsageRefundsSnapshot(seat);
    public bool HasNextUnlimitedCard(CharacterState player) => getCoreGameEngineAdvancedPoliciesRuntime().HasNextUnlimitedCard(player);
    public IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> CardPolicies(CharacterState player, SkillProgramCardPolicyKind kind) => getCoreGameEngineProgramCardPoliciesRuntime().CardPolicies(player, kind);
    public SkillBindingShard GetSkillBindingShard(CharacterState player) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(player);
    public bool GetProgramBooleanState(int seat, string skillId, string instanceId, string stateId) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(seat, skillId, instanceId, stateId);
    public bool LiveXionghuoSource(XionghuoGrant grant) => getCharacterXuRongOlRuntime().LiveXionghuoSource(grant);
    public int? JiezhenOwnerSeat(int seat) => characterHuangChengYanOlState.ReferenceToJiezhenConversions.TryGetValue(seat, out var state) ? state.OwnerSeat : null;
    public bool HasMouWujingAllowance(CharacterState player) => getCharacterZhugeLiangMouRuntime().HasMouWujingAllowance(player);
    public bool HasFriendQihuiAllowance(CharacterState player) => getCoreGameEngineAdvancedPoliciesRuntime().HasFriendQihuiAllowance(player);
    public IReadOnlyList<GiftHandRetentionObligation> GiftHandRetentionObligations => gameEngineConvertingGiftsState.ReferenceToGiftHandRetentionObligations;

    public bool FixedDistanceDebtConsumed(FixedDistanceOneTurnGrant grant) => getCoreGameEngineFixedDistanceOneDebtRuntime().FixedDistanceDebtConsumed(grant);
    public bool ValidFixedDistanceOneGrant(FixedDistanceOneTurnGrant grant) => getCoreGameEngineFixedDistanceOneDebtRuntime().ValidFixedDistanceOneGrant(grant);
    public ActiveDamageHandLimitFact? DamageHandLimit(int seat) => gameEngineProgramPersistentCombatPoliciesState.ReferenceToProgramPlayDamageHandLimits.TryGetValue(seat, out var state) ? new(state.SkillId, state.TurnNumber, state.DamageCount) : null;
    public HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().CurrentHalfHandSupport(owner, skill, instance, hash, state);
    public PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().CurrentPhaseHandDebt(owner, skill, instance, hash, state);
    public IEnumerable<PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures() => getCoreGameEnginePhaseHandSeizureRuntime().UnsettledPhaseHandSeizures();
    public PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding) => getCoreGameEnginePhaseHandSeizureRuntime().CurrentPhaseHandSeizureDebt(owner, skill, instance, hash, binding);
    public IReadOnlyList<MoSimaYiRecorded>? MoGuifuRecords(int seat) => getCharacterSimaYiOlMagicRuntime().HasMoSimaYi ? getCharacterSimaYiOlMagicRuntime().MoSimaYi.Records(seat) : null;
}
