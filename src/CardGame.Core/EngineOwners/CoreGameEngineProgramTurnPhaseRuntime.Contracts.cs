#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreProgramTurnPhaseCoreNativeSlashUseOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginSlashTargetResolution(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
    void ContinueSlashAfterResponsePrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreProgramTurnPhaseCharacterLiuZanMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksAlternateActualUseColor { get; }
}

internal interface ICoreProgramTurnPhaseCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseVirtualSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Suit? physicalSuit = default, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool ignoreDistance = false, global::CardGame.Core.CardConversionSource? conversionSource = default, bool actualUnconvertedPhysical = false, bool countsTowardLimit = true);
}

internal interface ICoreProgramTurnPhaseCoreFirstSlashDuelLegacyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginFirstSlashDuelLegacyWindow(global::CardGame.Core.NativeOwners.CardAttackHandle attack);
}

internal interface ICoreProgramTurnPhaseCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardArmorBypass(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramTurnPhaseCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKongShuCapability { get; }
}

internal interface ICoreProgramTurnPhaseCharacterLiYiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiYiCapability { get; }
}

internal interface ICoreProgramTurnPhaseCharacterLuBuHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasHappyLuBuWuqianCapability(global::CardGame.Core.CharacterState source);
}

internal interface ICoreProgramTurnPhaseCoreNativeLogicalDiscardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksNativeLogicalDiscards { get; }
}

internal interface ICoreProgramTurnPhaseCharacterHuangGaiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramTurnPhaseCoreNativeOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkSlashUsedOrPlayedDuringCurrentPlayPhase(int actorSeat, global::CardGame.Core.CardKind effectiveCardKind);
}

internal interface ICoreProgramTurnPhaseCoreActualPlayPhaseCardUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordActualPlayPhaseUse(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreProgramTurnPhaseCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TracksPlayCardHistory { get; }
}

internal interface ICoreProgramTurnPhaseCoreSlashTargetPenaltyWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetPenalties(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreProgramTurnPhaseCorePhaseStartControlPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseStartCardUseParticipant(int actor);
}

internal interface ICoreProgramTurnPhaseCharacterZhouQunOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasZhouQunCapability { get; }
}

internal interface ICoreProgramTurnPhaseCoreShanheS5ShensuOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShanheS5PhaseSkipCost(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramTurnPhase> phases, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent);
    bool IsShanheS5ShensuFrame(global::CardGame.Core.ProgramSkillFrame f);
    int ShanheS5ShensuBaseDamageBonus(global::CardGame.Core.ProgramSkillFrame active);
}

internal interface ICoreProgramTurnPhaseCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCommittedSlashFireCapability(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreProgramTurnPhaseCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICoreProgramTurnPhaseCoreAlcoholSlashSuppressionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureProgramAlcoholConsumption(long cardUseFrameId, global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramTurnPhaseCorePhaseNamePredictionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObservePhaseNamePredictionUse(global::CardGame.Core.CardActionContext action, bool qualifiedActualUse = false);
    void RecordPredictionNativeCardUse(long frameId, int actorSeat, global::CardGame.Core.CardKind kind);
}

internal interface ICoreProgramTurnPhaseCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreProgramTurnPhaseCoreProgramFactionActionRewardOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardActionContext CaptureFactionAction(global::CardGame.Core.CardActionContext action);
}

internal interface ICoreProgramTurnPhaseCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreProgramTurnPhaseCharacterLuMengMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordYingboCardUse(long resolutionId, int sourceSeat, global::CardGame.Core.CardKind cardKind);
}

internal interface ICoreProgramTurnPhaseCharacterSunHaoOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSunHaoCanshiCapability { get; }
}

internal interface ICoreProgramTurnPhaseKernelCoreMainOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle CreateCardAttackHandle(long resolutionId, int sourceSeat, int targetSeat, global::CardGame.Core.Card? card, int damageAmount = 1, global::CardGame.Core.CardKind? playedCardKind = default, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = default, global::CardGame.Core.DamageNature? damageNatureOverride = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? physicalCards = default, long? programJudgmentFrameId = default, long? programSkillFrameId = default, global::CardGame.Core.CardConversionSource? conversionSource = default, long? programSkillCardUseFrameId = default);
}

internal interface ICoreProgramTurnPhaseCoreAlternativeViewAsCostsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind);
}

internal interface ICoreProgramTurnPhaseCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryMarkProgramUseCommitted(long frameId);
}

internal interface ICoreProgramTurnPhaseCharacterHuangYueyingOlMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMouHuangYueyingBingcaiCapability { get; }
}

internal interface ICoreProgramTurnPhaseCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreProgramTurnPhaseCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramCardWindow(global::CardGame.Core.NativeOwners.CardAttackHandle? attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> opponents, global::CardGame.Core.ProgramCardContinuation? continuation, bool? cardUseCausedDamage = default, global::CardGame.Core.ProgramTrickContinuation? trickContinuation = default, global::CardGame.Core.ProgramSimpleCardContinuation? simpleContinuation = default, global::CardGame.Core.ProgramCompletedResponseReturn? completedResponseReturn = default, global::CardGame.Core.CaoChunHandUsePayment? caoChunPayment = default, bool caoChunDebtOnly = false);
}

internal interface ICoreProgramTurnPhaseCharacterLeJiuAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAnniversaryLeJiuUseObserver(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreProgramTurnPhaseCoreSoleTargetProtectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSoleTargetProtectionParticipant { get; }
}

internal interface ICoreProgramTurnPhaseKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreProgramTurnPhaseCoreSlashTargetBenefitWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginSlashTargetBenefits(global::CardGame.Core.NativeOwners.CardAttackHandle attack, bool legacy = false, bool afterActualTargets = false);
}

internal interface ICoreProgramTurnPhaseCharacterMaChengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMaChengCapability { get; }
}

internal interface ICoreProgramTurnPhaseCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreProgramTurnPhaseContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreProgramTurnPhaseCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICoreProgramTurnPhaseCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreProgramTurnPhaseCoreShanheXingtianOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShanheXingtianSeal(int seat);
}

internal interface ICoreProgramTurnPhaseCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginActualUseTargetPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.ActualUseTargetReturnKind kind);
}

internal interface ICoreProgramTurnPhaseCoreNativeCardContinuationCursorsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.DuelHandle? ActiveDuel { get; set; }
}

internal interface ICoreProgramTurnPhaseCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetSeatDistance(int sourceSeat, int targetSeat);
    int GetAttackRange(int sourceSeat);
}

internal interface ICoreProgramTurnPhaseCoreAlternativePhaseCostProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordAlternativePhaseSkip(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramTurnPhase> phases);
}

internal interface ICoreProgramTurnPhaseCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICoreProgramTurnPhaseCharacterCaiWenJiLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeifenPreUsePrivilege(global::CardGame.Core.CharacterState actor);
    bool HasLeCaiWenJiBeifenCapability { get; }
}

internal interface ICoreProgramTurnPhaseCharacterLiuYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasLiuYanCapability { get; }
}

internal interface ICoreProgramTurnPhaseResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreProgramTurnPhaseCharacterCuiYanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCuiYanCapability { get; }
}
