#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICharacterWoLongFengChuOlCoreProgramOrdinaryTrickUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> BuildProgramOrdinaryTrickUseOptions(global::CardGame.Core.CharacterState source, global::CardGame.Core.CardKind? outputKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool excludeOwner = false, bool enforceUsePermission = false, global::CardGame.Core.Suit? beneficiaryShieldSuit = default, bool? actualEffectiveColor = default, bool hasActualColor = false, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, bool includeNextActualUseAdjustment = false, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICharacterWoLongFengChuOlCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICharacterWoLongFengChuOlCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void PushRuntimeFrame(global::CardGame.Core.ResolutionFrame frame);
}

internal interface ICharacterWoLongFengChuOlCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterWoLongFengChuOlCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICharacterWoLongFengChuOlCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}

internal interface ICharacterWoLongFengChuOlCoreObserverSubtreesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasMovementObserverSuffix(int rootIndex);
    bool TryAdvanceObserverSubtree(bool advanceRecoveryReplacement, string? incompleteAttackError = default, bool routeUnreturnedAttackThroughRuntime = false);
}

internal interface ICharacterWoLongFengChuOlCoreProgramContinuationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation);
}

internal interface ICharacterWoLongFengChuOlCoreActionQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RuleQueryEvaluation EvaluateDistance(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICharacterWoLongFengChuOlContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICharacterWoLongFengChuOlCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICharacterWoLongFengChuOlCoreTieredRoundZeroResponsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ResolveTieredRoundZeroDodge(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source);
    void ResolveTieredRoundZeroCounterspell(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CardConversionSource source);
    void ResolveTieredRoundZeroResponse(global::CardGame.Core.PromptChoice choice, bool advance);
}

internal interface ICharacterWoLongFengChuOlCoreActiveNativeAttackQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.CardAttackHandle? ActiveCardAttack { get; set; }
}

internal interface ICharacterWoLongFengChuOlCoreTieredRoundZeroUsesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ExecuteTieredRoundZeroPlay(global::CardGame.Core.CharacterState actor, global::CardGame.Core.LegalAction action);
    bool TryResolveTieredRoundZeroForcedSlash(global::CardGame.Core.PromptChoice selected, bool advance);
}

internal interface ICharacterWoLongFengChuOlCorePlayCardHistoryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramViewAs? ViewAsRule(global::CardGame.Core.CardConversionSource source);
}

internal interface ICharacterWoLongFengChuOlCoreProgramPersistentStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void SetProgramBooleanState(int ownerSeat, string skillId, string instanceId, string stateId, bool value);
}

internal interface ICharacterWoLongFengChuOlCoreDyingCursorRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DyingFrame? ActiveDying { get; }
}

internal interface ICharacterWoLongFengChuOlCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICharacterWoLongFengChuOlCoreNativeDyingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CompleteDying(global::CardGame.Core.DyingFrame dying, bool survived);
}

internal interface ICharacterWoLongFengChuOlCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICharacterWoLongFengChuOlCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    void DrawProgramCards(long frameId, int targetSeat, int amount, global::CardGame.Core.SkillProgramNumberExpression? numberExpression, string? resultBind, global::CardGame.Core.SkillProgramCardSetVisibility visibility, global::CardGame.Core.CardMoveReason reason);
}

internal interface ICharacterWoLongFengChuOlCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterWoLongFengChuOlCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit);
}

internal interface ICharacterWoLongFengChuOlCoreRuleQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanSpendSlashUse(global::CardGame.Core.CharacterState player, global::CardGame.Core.CharacterState target, bool ignoresCount, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, global::CardGame.Core.Card? physicalCard = default, int? effectiveRank = default, int? excludedEquipmentId = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, bool ignoreMoQuanyu = false, global::CardGame.Core.Suit? effectiveSuit = default);
}

internal interface ICharacterWoLongFengChuOlCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ClearPendingDecision();
    global::CardGame.Core.PromptId CreatePromptId();
    void RequestHumanPlay();
}

internal interface ICharacterWoLongFengChuOlSkillRuntimeStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.SkillRuntimeStateStore _skillRuntimeState { get; }
}

internal interface ICharacterWoLongFengChuOlCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresProgramSlashLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
}

internal interface ICharacterWoLongFengChuOlCoreHandComparisonOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSelfCardTargetProhibition(int seat);
}

internal interface ICharacterWoLongFengChuOlCharacterKongRongMouOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCancelMouKrLiwenPayment(long id);
}

internal interface ICharacterWoLongFengChuOlResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICharacterWoLongFengChuOlCoreTreatedWoundedOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsRuleWounded(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterWoLongFengChuOlCharacterKeBiNengOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryCancelKeBiNengPingduanBasicPayment(long programId);
}

internal interface ICharacterWoLongFengChuOlCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardPolicy(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramCardPolicyKind kind, global::CardGame.Core.CardKind? effectiveKind = default);
}

internal interface ICharacterWoLongFengChuOlKernelCoreGameEngineAdvancedLifecycleHostOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.NativeOwners.ProgramSkillHost CreateProgramSkillHost();
}

internal interface ICharacterWoLongFengChuOlCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICharacterWoLongFengChuOlCoreAdvancedWeaponsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasWeaponAbility(global::CardGame.Core.CharacterState player, global::CardGame.Core.CardKind kind);
}

internal interface ICharacterWoLongFengChuOlCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICharacterWoLongFengChuOlCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds);
}

internal interface ICharacterWoLongFengChuOlCoreWenyiPeachSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ResumeWenyiEquipmentSlotPeach(global::CardGame.Core.EquipmentSlotNamedUseDraft draft);
}

internal interface ICharacterWoLongFengChuOlCoreProgramSkillChangesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CommitConfiguredConversionTier(global::CardGame.Core.CharacterState owner, string state, int tier);
}

internal interface ICharacterWoLongFengChuOlCoreProgramTieredRoundConversionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    int TieredRoundTier(int ownerSeat, string stateId);
}

internal interface ICharacterWoLongFengChuOlCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterWoLongFengChuOlCoreSayingTieredSuppliesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool SayingEquipmentSlotUseStillLegal(global::CardGame.Core.CharacterState actor, global::CardGame.Core.EquipmentSlotNamedUseDraft draft);
    void ResolveSayingTieredSlash(global::CardGame.Core.PromptChoice selected);
}

internal interface ICharacterWoLongFengChuOlCoreAdvancedLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool RestoreEquipmentSlot(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot);
    void SetEquipmentSlotCapacity(global::CardGame.Core.CharacterState player, global::CardGame.Core.EquipmentSlot slot, int capacity);
}

internal interface ICharacterWoLongFengChuOlCoreResolutionIdentityBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long _resolutionSequence { get; set; }
}

internal interface ICharacterWoLongFengChuOlCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool UsesFormalQinglongCrescentBlade { get; }
}

internal interface ICharacterWoLongFengChuOlCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillBindingShard GetSkillBindingShard(global::CardGame.Core.CharacterState player);
}

internal interface ICharacterWoLongFengChuOlKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICharacterWoLongFengChuOlCharacterSunYuanOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool FinishSayingSuppliedSlashFailure(global::CardGame.Core.ProgramSkillFrame supplied);
}

internal interface ICharacterWoLongFengChuOlCoreNativeTargetLegalityOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanUseSlashTarget(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card slashCard, global::CardGame.Core.CardConversionSource? conversionSource = default, global::CardGame.Core.CardKind effectiveKind = (global::CardGame.Core.CardKind)0, bool ignoreDistance = false, long? existingUseFrameId = default, bool noEffectiveRank = false, int? specificEffectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}
