namespace CardGame.Core;

// The original turn store owns these lists, including their enumeration order
// and any existing snapshot-on-read behavior. Projection never copies the store.
internal interface IActivePlayerTurnEffectQueries : IProgramSelectionQuery
{
    IReadOnlyList<TurnCardDamageModifier> DamageModifiers { get; }
    IReadOnlyList<TurnRuleModifier> RuleModifiers { get; }
    IReadOnlyList<TurnCardTargetAdjustment> TargetAdjustments { get; }
    IReadOnlyList<TurnCardUseProhibition> Prohibitions { get; }
    IReadOnlyList<TurnCardActionProhibition> ActionProhibitions { get; }
    IReadOnlyList<TurnHandCardColorRestriction> HandColorRestrictions { get; }
    IReadOnlyList<TurnCardTargetRestriction> TargetRestrictions { get; }
    IReadOnlyList<TurnCardConversion> Conversions { get; }
    IReadOnlyList<TurnCardRecoveryModifier> RecoveryModifiers { get; }
    IReadOnlyList<TurnRedSlashPolicy> RedSlashPolicies { get; }
    IReadOnlyList<TurnSlashSuitAllowance> SlashSuitAllowances { get; }
    IReadOnlyList<TurnJudgedRankSlashPolicy> JudgedRankSlashPolicies { get; }
    IReadOnlyList<TurnTargetCardQuotaAllowance> TargetCardQuotaAllowances { get; }
    IReadOnlyList<TurnHandLimitExemptCardGrant> HandLimitExemptCards { get; }
}

internal sealed partial class TurnCardUseEffectStore : IActivePlayerTurnEffectQueries
{
    IReadOnlyList<TurnCardDamageModifier> IActivePlayerTurnEffectQueries.DamageModifiers => DamageModifiers;
    IReadOnlyList<TurnRuleModifier> IActivePlayerTurnEffectQueries.RuleModifiers => RuleModifiers;
    IReadOnlyList<TurnCardTargetAdjustment> IActivePlayerTurnEffectQueries.TargetAdjustments => TargetAdjustments;
    IReadOnlyList<TurnCardUseProhibition> IActivePlayerTurnEffectQueries.Prohibitions => Prohibitions;
    IReadOnlyList<TurnCardActionProhibition> IActivePlayerTurnEffectQueries.ActionProhibitions => ActionProhibitions;
    IReadOnlyList<TurnHandCardColorRestriction> IActivePlayerTurnEffectQueries.HandColorRestrictions => HandColorRestrictions;
    IReadOnlyList<TurnCardTargetRestriction> IActivePlayerTurnEffectQueries.TargetRestrictions => TargetRestrictions;
    IReadOnlyList<TurnCardConversion> IActivePlayerTurnEffectQueries.Conversions => Conversions;
    IReadOnlyList<TurnCardRecoveryModifier> IActivePlayerTurnEffectQueries.RecoveryModifiers => RecoveryModifiers;
    IReadOnlyList<TurnRedSlashPolicy> IActivePlayerTurnEffectQueries.RedSlashPolicies => RedSlashPolicies;
    IReadOnlyList<TurnSlashSuitAllowance> IActivePlayerTurnEffectQueries.SlashSuitAllowances => SlashSuitAllowances;
    IReadOnlyList<TurnJudgedRankSlashPolicy> IActivePlayerTurnEffectQueries.JudgedRankSlashPolicies => JudgedRankSlashPolicies;
    IReadOnlyList<TurnTargetCardQuotaAllowance> IActivePlayerTurnEffectQueries.TargetCardQuotaAllowances => TargetCardQuotaAllowances;
    IReadOnlyList<TurnHandLimitExemptCardGrant> IActivePlayerTurnEffectQueries.HandLimitExemptCards => HandLimitExemptCards;
}

// Only private legacy record fields used by this projection cross as scoped
// values. Their owning lists and dictionaries remain in their rule modules.
internal readonly record struct ActiveNextSlashReserveFact(int OwnerSeat, string SkillId, int TurnNumber, int TurnSeat, int Amount);
internal readonly record struct ActiveSkillSuppressionFact(int SourceSeat, int TargetSeat, string SkillId);
internal readonly record struct ActiveRedTargetFact(CardConversionSource Source, int Maximum);
internal readonly record struct ActiveDamageHandLimitFact(string SkillId, int TurnNumber, int DamageCount);

internal interface IActivePlayerEffectQueries
{
    bool UsesAbsoluteSeatNumbers { get; }
    int RelativeTableOrder(int from, int target);
    string? MoCaoCaoEquipmentDescription(int seat);
    bool MoCaoCaoHasHandPrivilege(int seat);
    IEnumerable<MouGongsunZanBaojingGrantedEvent> ActiveMouGszBaojing(int seat);
    string? MouHxYangweiPublicDescription(int seat);
    string? WuLuKangDixianPublicDescription(int seat);
    string? WuZhangFeiPublicCycleDescription(int seat, IReadOnlyList<GeneralSkillDefinition>? primary, IReadOnlyList<GeneralSkillDefinition>? secondary);
    int NextSlashReserveCount { get; }
    ActiveNextSlashReserveFact NextSlashReserveAt(int index);
    CardConversionSource? NextCardTargetAdjustmentOwner(int seat);
    bool HasLegacyNextCardTargetAdjustment(CharacterState player);
    IReadOnlyList<DirectedTurnCardPolicy> DirectedTurnCardPolicies { get; }
    bool HasRuntimeSkillInstance(int seat, string skillId, string skillInstanceId);
    int SkillSuppressionCount { get; }
    ActiveSkillSuppressionFact SkillSuppressionAt(int index);
    IReadOnlyList<ProgramPhaseColorRestriction> PhaseColorRestrictions { get; }
    IReadOnlyList<CurrentTurnDirectedHeartSlashBonus> HeartSlashBonuses { get; }
    IReadOnlyList<ActorHandLimitPenalty> ActorHandLimitPenalties { get; }
    IReadOnlyList<ProgramNextTurnRuleModifier> NextTurnRuleModifiers { get; }
    IReadOnlyList<OriginalTargetAdditionGrant> OriginalTargetAdditionGrants { get; }
    ActiveRedTargetFact? RedAdditionalTargetGrant(int seat);
    IReadOnlyList<TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat);
    IReadOnlyList<TurnHandLimitCardKindExemption>? GetTurnHandLimitCardKindExemptionsSnapshot(int seat);
    IReadOnlyList<FirstRoundGameUsageRefund>? GetFirstRoundGameUsageRefundsSnapshot(int seat);
    bool HasNextUnlimitedCard(CharacterState player);
    IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> CardPolicies(CharacterState player, SkillProgramCardPolicyKind kind);
    SkillBindingShard GetSkillBindingShard(CharacterState player);
    bool GetProgramBooleanState(int seat, string skillId, string instanceId, string stateId);
    bool LiveXionghuoSource(XionghuoGrant grant);
    int? JiezhenOwnerSeat(int seat);
    bool HasMouWujingAllowance(CharacterState player);
    bool HasFriendQihuiAllowance(CharacterState player);
    IReadOnlyList<GiftHandRetentionObligation> GiftHandRetentionObligations { get; }
    bool FixedDistanceDebtConsumed(FixedDistanceOneTurnGrant grant);
    bool ValidFixedDistanceOneGrant(FixedDistanceOneTurnGrant grant);
    ActiveDamageHandLimitFact? DamageHandLimit(int seat);
    HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state);
    PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state);
    IEnumerable<PhaseHandSeizureIssuedEvent> UnsettledPhaseHandSeizures();
    PhaseHandSeizureIssuedEvent? CurrentPhaseHandSeizureDebt(int owner, string skill, string instance, string hash, string binding);
    IReadOnlyList<MoSimaYiRecorded>? MoGuifuRecords(int seat);
}
