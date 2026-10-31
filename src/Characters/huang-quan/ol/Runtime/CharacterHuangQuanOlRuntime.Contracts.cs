#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterHuangQuanOlCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanOlCharacterGuoYouZhiMouMobileOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouGyzExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICharacterHuangQuanOlCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanOlCoreDragonBoatActivitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDragonBoatActivity { get; }
}

internal interface ICharacterHuangQuanOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterHuangQuanOlCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterHuangQuanOlCoreProgramNativeDrawOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramStepOutcome BeginOperationProgramDraw(global::CardGame.Core.ProgramSkillFrame frame, int recipientSeat, int requestedCount, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterHuangQuanOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, int occurrenceIndex = 0);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterHuangQuanOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterHuangQuanOlCharacterHuangQuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuangQuanFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
}

internal interface ICharacterHuangQuanOlCoreCharacterStateProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCharacterStateProgramWindow(long? resumeFrameId = default, global::CardGame.Core.CharacterStateContinuation continuation = (global::CardGame.Core.CharacterStateContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICharacterHuangQuanOlCoreProgramCardUsePaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramCardUsePayments CardUsePayments { get; }
}

internal interface ICharacterHuangQuanOlCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasKeBiNengCapability { get; }

    bool IsKeBiNengPingduanBasicQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterHuangQuanOlCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterHuangQuanOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterHuangQuanOlCharacterBaiLingYunAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversaryBaiLingYunExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICharacterHuangQuanOlCharacterWeiYanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMobileShiZhuangshiQuota(int seat);
}

internal interface ICharacterHuangQuanOlCoreFinalizedCardTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> GetImplicitSelfCardUseTargets(global::CardGame.Core.CardKind kind, int sourceSeat, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterHuangQuanOlCoreGuanduGeneralNativeCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ValidGuanduYuanlveOffer(global::CardGame.Core.ProgramSkillFrame f);
    bool FinishGuanduYuanlveUse(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterHuangQuanOlCharacterFuRongOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFuRongUnlimitedUse(long id, int seat);
}

internal interface ICharacterHuangQuanOlCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterHuangQuanOlCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterHuangQuanOlCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterHuangQuanOlCoreGuanduCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? GuanduExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
}

internal interface ICharacterHuangQuanOlCharacterJiaXuMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJxExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICharacterHuangQuanOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterHuangQuanOlCoreProgramAlcoholQuotaOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasAdditionalAlcoholUse(global::CardGame.Core.CharacterState owner);
}

internal interface ICharacterHuangQuanOlCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.Card ApplyProgramUseAppearance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICharacterHuangQuanOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterHuangQuanOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterHuangQuanOlCoreNativeActionDispatchOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteAction(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillCardIds = default, global::System.Collections.Generic.IReadOnlyList<int>? activeSkillTargetSeats = default);
}

internal interface ICharacterHuangQuanOlCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICharacterHuangQuanOlCharacterYaDanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsQingyaDiscardQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICharacterHuangQuanOlCoreDragonBoatGeneralsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DragonBoatAllies(int owner, int other);
}

internal interface ICharacterHuangQuanOlCharacterMiHengLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedSiruiUse(long useId, int actorSeat);
}

internal interface ICharacterHuangQuanOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterHuangQuanOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterHuangQuanOlCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TableStepSeat(int from, int offset);
}

internal interface ICharacterHuangQuanOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICharacterHuangQuanOlCharacterHuBanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsHuiyunExactHandQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICharacterHuangQuanOlCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
}

internal interface ICharacterHuangQuanOlCoreNativeCardAppearanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit EffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card);
}

internal interface ICharacterHuangQuanOlCharacterGuoYuanMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsShiGyHandQuery(global::CardGame.Core.CharacterState actor, int? id);
}

internal interface ICharacterHuangQuanOlCoreJianjiDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsJianjiDeclarationChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child);
    bool IsJianjiDeclarationQuery(global::CardGame.Core.CharacterState actor, int? selectedId);
}

internal interface ICharacterHuangQuanOlCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICharacterHuangQuanOlCoreDragonBoatCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int>? DragonBoatExpectedNativeTargets(global::CardGame.Core.CharacterState actor, global::CardGame.Core.Card card, global::CardGame.Core.LegalAction action);
    bool IsDragonBoatRealgarWineQuotaUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterHuangQuanOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterHuangQuanOlCharacterJiangJiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouJjExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected);
}

internal interface ICharacterHuangQuanOlCoreRoundGainedSourceUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedRoundGainedBasicBonus(long useId, int actor);
}

internal interface ICharacterHuangQuanOlCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterHuangQuanOlCharacterSunHuanAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsAnniversarySunHuanExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedPhysicalCardId);
}

internal interface ICharacterHuangQuanOlCharacterGuoTuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeijieCapability { get; }

    bool IsWeijieOrdinaryAlcoholQuery(global::CardGame.Core.CharacterState actor);
}

internal interface ICharacterHuangQuanOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICharacterHuangQuanOlCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<int> ApplyTurnCardGroupTargetRestrictions(global::CardGame.Core.CharacterState source, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterHuangQuanOlCoreSkillOwnershipWindowsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginAdvancedSkillsChanged(long parentFrameId);
}

internal interface ICharacterHuangQuanOlCoreJianjiDeclarationsOperations2 : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HuangQuanDeclarationObserverSuffix(int rootIndex);
}

internal interface ICharacterHuangQuanOlCoreNativeLegalActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> BuildLegalActions(global::CardGame.Core.CharacterState actor, bool includeProgramActions = true, int? selectedPhysicalCardId = default);
}

internal interface ICharacterHuangQuanOlCoreProgramAttackAttemptOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt? CurrentDamageAttempt { get; }
}
