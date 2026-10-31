#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeMultiTargetActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> GetFangtianOrderedSlashTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card slash, global::CardGame.Core.CardConversionSource? conversionSource = default, bool ignoresSlashLimit = false, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDrawFundedDistinctBasicOffersCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetChanshuangSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterHuanShuJunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetWenyiPeachRequest(global::CardGame.Core.ProgramSkillFrame f, out int actor, out int target);
}

internal interface ICoreDrawFundedDistinctBasicOffersCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreTurnTargetCardQuotaAllowancesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTargetCardQuotaAllowance(int actor, int target);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterZhugeLiangMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouWujingAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterZhouYuOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGuquCardlessAllowed(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardKind kind, bool forResponse, bool dyingUse = false);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreAdvancedPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasNextUnlimitedCard(global::CardGame.Core.CharacterState owner);
    bool HasFriendQihuiAllowance(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.FactionDefenseHandle? ActiveFactionDefense { get; set; }

    global::CardGame.Core.NativeOwners.BorrowedSwordHandle? ActiveBorrowedSword { get; set; }

    global::CardGame.Core.NativeOwners.QinglongCrescentBladeHandle? ActiveQinglongCrescentBlade { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreProgramCardUseRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddProgramTargetCountSlashActions(global::System.Collections.Generic.ICollection<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card physicalCard, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CharacterState> legalTargets, string slashName, global::CardGame.Core.CardKind? playedCardKind, global::CardGame.Core.CardConversionSource? conversionSource = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource>? additionalConversionSources = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNextActualUseTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextActualUseAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CommandResult Reject(global::CardGame.Core.CommandErrorCode code, string message);
    global::CardGame.Core.CommandResult Accept(global::System.Action operation);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeSkillContextQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PlayerSkillContext CreateSkillContext(global::CardGame.Core.CharacterState player, bool includeHandLimit = false);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreDrawFundedDistinctBasicPaymentOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginDrawFundedDistinctBasic(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> targets, global::CardGame.Core.PendingDecision original, global::CardGame.Core.GameEngine.DrawFundedBasicNeed need);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterLuSuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouLuSuUseQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreProgramSlashRequestOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int[] GetNearestLivingCharacterSeats(int seat);
}

internal interface ICoreDrawFundedDistinctBasicOffersResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeQinglongOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseQinglongCrescentBladeTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, bool? effectiveColor = default, bool allowAnyColor = true, global::CardGame.Core.Suit? physicalSuit = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterMaTieAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryMaTieSlashQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNextCardTargetAdjustmentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AddNextCardTargetAdjustmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceRulesAndPublishState();
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool TryGetSayingSlashRequest(global::CardGame.Core.ProgramSkillFrame f, out int actorSeat, out global::System.Collections.Generic.IReadOnlyList<int> targetSeats);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouKrLiwenUseQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterLvKuangLvXiangOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQigongRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool QigongAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreProgramAllHandBonusOperationOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUsePeachToRescue(int responderSeat, int victimSeat);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterCaoChunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCaoChunInitialRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    string DescribeConversion(global::CardGame.Core.CardConversionSource? source, string description);
    string ProgramConversionName(global::CardGame.Core.CardConversionSource source);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersKernelCoreGameEngineRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceToHumanBoundary();
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterWuJingOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWuJingHejiRequest(global::CardGame.Core.ProgramSkillFrame f);
    bool WuJingHejiAdapterActor(global::CardGame.Core.ProgramSkillFrame f, int actor, int? target, int cursor, long? parent, long? request);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreWenyiPeachSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool WenyiPeachUseLegal(int actorSeat, int targetSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> materials, global::CardGame.Core.CardConversionSource? source = default);
}

internal interface ICoreDrawFundedDistinctBasicOffersCharacterPeiYuanShaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFulueRequest(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreDrawFundedDistinctBasicOffersCoreCommandSessionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long Revision { get; }
}
