#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterBeiMiHuOlCoreSameNameHandDiscardOrDamageObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SameNameHandObserverSuffix(int index);
}

internal interface ICharacterBeiMiHuOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICharacterBeiMiHuOlCharacterDongZhaoMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MobileShiDongZhaoExtraGrantedEvent? ShiDzPendingExtra(int beneficiary);
}

internal interface ICharacterBeiMiHuOlCharacterLuSuMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShiLsHandGrantsToExpire(int owner);
    void ExpireShiLsHandGrants(int upcomingOwnerSeat, long preludeId);
}

internal interface ICharacterBeiMiHuOlCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterBeiMiHuOlCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterBeiMiHuOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterBeiMiHuOlCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds);
}

internal interface ICharacterBeiMiHuOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterBeiMiHuOlCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterBeiMiHuOlCoreMouLuMengOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginRoundForTurn(global::CardGame.Core.CharacterState current);
}

internal interface ICharacterBeiMiHuOlCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasGrantedPhaseEntityDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? ids);
}

internal interface ICharacterBeiMiHuOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterBeiMiHuOlCharacterSimaHuiOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int SimaHuiOwnTurns(int owner);
}

internal interface ICharacterBeiMiHuOlKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICharacterBeiMiHuOlCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool CanBeDistanceFreeDesignatedTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
    global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICharacterBeiMiHuOlCoreFireTargetBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryDrainFireTargetMovement(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICharacterBeiMiHuOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
    void ChangeProgramMaximumHp(global::CardGame.Core.ProgramSkillFrame frame, int amount, string? sourceSkillId = default);
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTriggerFacts CaptureProgramTriggerFacts(global::CardGame.Core.CharacterState owner);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectEligibleProgramTriggerCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramTriggerFacts facts);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterBeiMiHuOlCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICharacterBeiMiHuOlCharacterGuoJiaMobileGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasShenGuoJiaAwakeningReceipt(global::CardGame.Core.ProgramSkillFrame frame);
    bool HasShenGuoJiaAwakeningCondition(int seat, string skill, string? binding = default);
}

internal interface ICharacterBeiMiHuOlCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterBeiMiHuOlCoreNativeRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int GetCombatDistance(int sourceSeat, int targetSeat);
}

internal interface ICharacterBeiMiHuOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICharacterBeiMiHuOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterBeiMiHuOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterBeiMiHuOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterBeiMiHuOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame? FindProgramObserverRoot(global::System.Func<global::CardGame.Core.ProgramSkillFrame, global::CardGame.Core.ResolutionFrame, bool> firstChild, global::System.Func<int, bool> suffix, long? requiredRootId = default);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterBeiMiHuOlCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void GrantProgramSkills(global::CardGame.Core.ProgramSkillFrame frame, global::System.Collections.Generic.IReadOnlyList<string> skillIds);
}

internal interface ICharacterBeiMiHuOlCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterBeiMiHuOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
    void AdvanceRuntimeFrame(long frameId);
    void PopResolutionFrame(long frameId, global::CardGame.Core.ResolutionFrameKind expectedKind);
    void ReturnRuntimeProgramMovement(long frameId);
}

internal interface ICharacterBeiMiHuOlCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICharacterBeiMiHuOlCoreStrategicProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetStrategicMarker(int seat, global::CardGame.Core.PlayerMarkerKind marker, int sourceSeat, int count, global::CardGame.Core.ProgramSkillFrame frame);
}

internal interface ICharacterBeiMiHuOlCharacterChenZhiMobileShiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.MobileShiChenZhiExtraGrantedEvent? ShiCzPendingExtra(int owner);
}

internal interface ICharacterBeiMiHuOlCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterBeiMiHuOlCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterBeiMiHuOlCoreGainGiftProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool GainGiftHasFinalGameEnd();
}

internal interface ICharacterBeiMiHuOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterBeiMiHuOlCoreCompletedCategoryUseProofOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ActualHandGainMutationBinding(long frameId, string skill, int owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramEffectOp operation);
}

internal interface ICharacterBeiMiHuOlCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICharacterBeiMiHuOlCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICharacterBeiMiHuOlCoreActualTurnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void BeginTurn();
    void BeginTurnAfterBeforeTurnPrelude(global::CardGame.Core.CharacterState current);
}

internal interface ICharacterBeiMiHuOlCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICharacterBeiMiHuOlCoreTableOrderOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int RelativeTableOrder(int from, int target);
}

internal interface ICharacterBeiMiHuOlCharacterBeiMiHuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool BeiMiHuFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child);
}
