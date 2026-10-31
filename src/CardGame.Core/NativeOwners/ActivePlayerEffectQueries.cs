#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct ActivePlayerEffectQueries(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreNativeDamageHandlesRuntime>> getCoreNativeDamageHandlesRuntime, Func<RuntimeEntries.CoreGameEngineTableOrderEntries> getCoreGameEngineTableOrderRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoCaoOlMagicRuntime>> getCharacterCaoCaoOlMagicRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterGongsunZanMouOlRuntime>> getCharacterGongsunZanMouOlRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHuaXiongMouOlRuntime>> getCharacterHuaXiongMouOlRuntime, Func<RuntimeEntries.CharacterLuKangWumiaoEntries> getCharacterLuKangWumiaoRuntime, Func<RuntimeEntries.CharacterZhangFeiWumiaoEntries> getCharacterZhangFeiWumiaoRuntime, EngineStates.CoreGameEngineNextSlashProgramsState gameEngineNextSlashProgramsState, EngineStates.CoreGameEngineNextCardTargetAdjustmentsState gameEngineNextCardTargetAdjustmentsState, Func<RuntimeEntries.CoreGameEngineNextCardTargetAdjustmentsEntries> getCoreGameEngineNextCardTargetAdjustmentsRuntime, EngineStates.CoreDirectedTurnCardPoliciesState directedTurnCardPoliciesState, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, EngineStates.CoreGameEngineStrategicProgramsState gameEngineStrategicProgramsState, EngineStates.CoreGameEngineProgramPhaseColorRestrictionsState gameEngineProgramPhaseColorRestrictionsState, EngineStates.CoreGameEngineCurrentTurnShownCardBenefitsState gameEngineCurrentTurnShownCardBenefitsState, EngineStates.CoreGameEngineActorHandLimitPenaltiesState gameEngineActorHandLimitPenaltiesState, EngineStates.CoreGameEngineDeferredBenefitsProgramsState gameEngineDeferredBenefitsProgramsState, EngineStates.CoreGameEngineOriginalTargetAdditionsState gameEngineOriginalTargetAdditionsState, EngineStates.CoreGameEngineRedAdditionalTargetsState gameEngineRedAdditionalTargetsState, Func<RuntimeEntries.CoreGameEngineTurnHandCategoryRestrictionsEntries> getCoreGameEngineTurnHandCategoryRestrictionsRuntime, Func<RuntimeEntries.CoreGameEngineTurnHandLimitKindExemptionsEntries> getCoreGameEngineTurnHandLimitKindExemptionsRuntime, Func<RuntimeEntries.CoreGameEngineBoundDiscardSlashBenefitsEntries> getCoreGameEngineBoundDiscardSlashBenefitsRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineAdvancedPoliciesRuntime>> getCoreGameEngineAdvancedPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentStateEntries> getCoreGameEngineProgramPersistentStateRuntime, Func<RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime, EngineStates.CharacterHuangChengYanOlState characterHuangChengYanOlState, Func<RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime, EngineStates.CoreGameEngineConvertingGiftsState gameEngineConvertingGiftsState, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineFixedDistanceOneDebtRuntime>> getCoreGameEngineFixedDistanceOneDebtRuntime, EngineStates.CoreGameEngineProgramPersistentCombatPoliciesState gameEngineProgramPersistentCombatPoliciesState, Func<RuntimeEntries.CoreGameEngineHalfHandPhaseDebtProgramsEntries> getCoreGameEngineHalfHandPhaseDebtProgramsRuntime, Func<RuntimeEntries.CoreGameEnginePhaseHandSeizureEntries> getCoreGameEnginePhaseHandSeizureRuntime, Func<RuntimeEntries.CharacterSimaYiOlMagicEntries> getCharacterSimaYiOlMagicRuntime) : IActivePlayerEffectQueries
{
    public bool UsesAbsoluteSeatNumbers => getCoreNativeDamageHandlesRuntime().Value.IsTeamMode || getCoreNativeDamageHandlesRuntime().Value.IsNationalWarMode;

    public int RelativeTableOrder(int from, int target) => getCoreGameEngineTableOrderRuntime().RelativeTableOrder(from, target);
    public string? MoCaoCaoEquipmentDescription(int seat) => getCharacterCaoCaoOlMagicRuntime().Value.MoCaoCaoEquipmentDescription(seat);
    public bool MoCaoCaoHasHandPrivilege(int seat) => getCharacterCaoCaoOlMagicRuntime().Value.MoCaoCaoHasHandPrivilege(seat);
    public IEnumerable<MouGongsunZanBaojingGrantedEvent> ActiveMouGszBaojing(int seat) => getCharacterGongsunZanMouOlRuntime().Value.ActiveMouGszBaojing(seat);
    public string? MouHxYangweiPublicDescription(int seat) => getCharacterHuaXiongMouOlRuntime().Value.MouHxYangweiPublicDescription(seat);
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
    public bool HasNextUnlimitedCard(CharacterState player) => getCoreGameEngineAdvancedPoliciesRuntime().Value.HasNextUnlimitedCard(player);
    public IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> CardPolicies(CharacterState player, SkillProgramCardPolicyKind kind) => getCoreGameEngineProgramCardPoliciesRuntime().CardPolicies(player, kind);
    public SkillBindingShard GetSkillBindingShard(CharacterState player) => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(player);
    public bool GetProgramBooleanState(int seat, string skillId, string instanceId, string stateId) => getCoreGameEngineProgramPersistentStateRuntime().GetProgramBooleanState2(seat, skillId, instanceId, stateId);
    public bool LiveXionghuoSource(XionghuoGrant grant) => getCharacterXuRongOlRuntime().LiveXionghuoSource(grant);
    public int? JiezhenOwnerSeat(int seat) => characterHuangChengYanOlState.ReferenceToJiezhenConversions.TryGetValue(seat, out var state) ? state.OwnerSeat : null;
    public bool HasMouWujingAllowance(CharacterState player) => getCharacterZhugeLiangMouRuntime().HasMouWujingAllowance(player);
    public bool HasFriendQihuiAllowance(CharacterState player) => getCoreGameEngineAdvancedPoliciesRuntime().Value.HasFriendQihuiAllowance(player);
    public IReadOnlyList<GiftHandRetentionObligation> GiftHandRetentionObligations => gameEngineConvertingGiftsState.ReferenceToGiftHandRetentionObligations;

    public bool FixedDistanceDebtConsumed(FixedDistanceOneTurnGrant grant) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.FixedDistanceDebtConsumed(grant);
    public bool ValidFixedDistanceOneGrant(FixedDistanceOneTurnGrant grant) => getCoreGameEngineFixedDistanceOneDebtRuntime().Value.ValidFixedDistanceOneGrant(grant);
    public ActiveDamageHandLimitFact? DamageHandLimit(int seat) => gameEngineProgramPersistentCombatPoliciesState.ReferenceToProgramPlayDamageHandLimits.TryGetValue(seat, out var state) ? new(state.SkillId, state.TurnNumber, state.DamageCount) : null;
    public HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().CurrentHalfHandSupport(owner, skill, instance, hash, state);
    public PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state) => getCoreGameEngineHalfHandPhaseDebtProgramsRuntime().CurrentPhaseHandDebt(owner, skill, instance, hash, state);
    public IEnumerable<PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures() => getCoreGameEnginePhaseHandSeizureRuntime().UnsettledPhaseHandSeizures();
    public PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding) => getCoreGameEnginePhaseHandSeizureRuntime().CurrentPhaseHandSeizureDebt(owner, skill, instance, hash, binding);
    public IReadOnlyList<MoSimaYiRecorded>? MoGuifuRecords(int seat) => getCharacterSimaYiOlMagicRuntime().HasMoSimaYi ? getCharacterSimaYiOlMagicRuntime().MoSimaYi.Records(seat) : null;
}
