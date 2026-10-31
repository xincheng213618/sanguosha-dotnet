#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreSameTypeActualUseAidCorePlayerViewProjectionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameSnapshot CreateSnapshot(int viewerSeat, bool revealAll = false);
}

internal interface ICoreSameTypeActualUseAidCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreSameTypeActualUseAidCoreDamageJudgmentSuitPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    long? LegacyDamageJudgmentVirtualProducer(global::CardGame.Core.CardUseFrame use, int target);
}

internal interface ICoreSameTypeActualUseAidCharacterDianWeiMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsMouDianWeiSlashUse(long id);
}

internal interface ICoreSameTypeActualUseAidResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreSameTypeActualUseAidCardZonesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.CardZoneStore _cardZones { get; }
}

internal interface ICoreSameTypeActualUseAidCoreProvenanceCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedProvenanceUseDistance(long? useId, int owner);
    bool HasProvenanceUseDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? physicalIds);
}

internal interface ICoreSameTypeActualUseAidCoreIssuedPlayUsePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedFirstPlayUseDistance(long? frameId);
}

internal interface ICoreSameTypeActualUseAidCoreNativeDamagePoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardTargetProhibited(global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind cardKind, global::CardGame.Core.Suit? suit = default, bool? effectiveIsRed = default);
    bool IsSlashProhibited(global::CardGame.Core.CharacterState target);
}

internal interface ICoreSameTypeActualUseAidCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
    void ReplaceRuntimeFrame(long expectedFrameId, global::CardGame.Core.ResolutionFrame next);
}

internal interface ICoreSameTypeActualUseAidCoreCurrentTurnShownCardBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasSlashUseDistanceBySuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardKind kind, global::CardGame.Core.Suit? suit);
}

internal interface ICoreSameTypeActualUseAidKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreSameTypeActualUseAidCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreSameTypeActualUseAidCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
}

internal interface ICoreSameTypeActualUseAidCoreRecipientContestReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesPindianWinnerUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PindianWinnerSlashReturn returned);
}

internal interface ICoreSameTypeActualUseAidCoreCardAttackStateOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Card GetAttackCard(int id);
}

internal interface ICoreSameTypeActualUseAidCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreSameTypeActualUseAidCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IgnoresProgramSlashDistance(global::CardGame.Core.CharacterState owner, global::CardGame.Core.CardConversionSource? source);
    bool IgnoresSpGuanYuWushengDistance(global::CardGame.Core.CharacterState player, global::CardGame.Core.Card card);
    bool IgnoresSlashUseDistance(global::CardGame.Core.CharacterState actor);
}

internal interface ICoreSameTypeActualUseAidCoreProgramCardPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> BuildOwnedCardPaymentChoices(long frameId, int chooserSeat, int cardOwnerSeat, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardZoneKind> zones, global::CardGame.Core.OwnedCardMoveIntent intent, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramCardCategory>? cardCategories = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardKind>? cardKinds = default, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit>? suits = default, global::System.Func<global::CardGame.Core.CardZoneKind, global::CardGame.Core.Card, bool>? canSelect = default, global::CardGame.Core.SkillProgramCardDestination destination = (global::CardGame.Core.SkillProgramCardDestination)1, int? destinationSeat = default, bool prohibitReplacingEquipment = false, int? correspondingZoneSeat = default);
}

internal interface ICoreSameTypeActualUseAidCoreForeignContestAidObserversOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AssertSameTypeAid(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreSameTypeActualUseAidCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreSameTypeActualUseAidCharacterHuangQuanMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>> NativeGiveCompletionRecords(long producer, int giver, int recipient, global::System.Collections.Generic.IReadOnlyList<int> ids, long before, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? original = default);
}

internal interface ICoreSameTypeActualUseAidCoreDeckEquipmentProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasBeneficiarySuitShield(int actor, int target, global::CardGame.Core.Suit? suit);
}

internal interface ICoreSameTypeActualUseAidCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetHand(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreSameTypeActualUseAidCoreRecoveryReplacementOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginQueuedRecoveryReplacement(long? parentFrameId, global::CardGame.Core.PostEventContinuation continuation, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreSameTypeActualUseAidCoreCardActionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, global::CardGame.Core.ProgramCardContinuation? continuation = default);
}

internal interface ICoreSameTypeActualUseAidCoreAlternatingSuitAndCompletedTopOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasPhaseSuitAllowance(int seat, global::CardGame.Core.Suit? suit);
}

internal interface ICoreSameTypeActualUseAidCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreSameTypeActualUseAidCoreFinalTargetSlashOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsWithinSpecificSlashRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? rank, long? existingUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default);
}

internal interface ICoreSameTypeActualUseAidCoreKuangfuReceiptsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedKuangfuDistance(long? id, int actor);
}

internal interface ICoreSameTypeActualUseAidCoreCurrentSlashFireOffersOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsCurrentSlashFireChangedUse(global::CardGame.Core.CardUseFrame use);
}

internal interface ICoreSameTypeActualUseAidCoreGrantedEntityPhasesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasIssuedGrantedPhaseEntityDistance(long? useId, int actor);
    bool HasGrantedPhaseEntityDistance(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int>? ids);
}

internal interface ICoreSameTypeActualUseAidCoreNationalSkillsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasRuntimeSkillInstance(global::CardGame.Core.CharacterState player, string skillId, string skillInstanceId);
}

internal interface ICoreSameTypeActualUseAidCoreCardMovementProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginCardsMovedProgramWindow(long? instructionFrameId = default, int? subjectSeat = default);
}

internal interface ICoreSameTypeActualUseAidCoreDirectedCardPolicyQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasCardDistanceExemption(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, long? cardUseFrameId = default, int? excludedEquipmentId = default, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default);
}

internal interface ICoreSameTypeActualUseAidCoreTurnRedSlashBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnRedSlashPolicyForColor(int actor, global::CardGame.Core.CardKind kind, bool? color);
}

internal interface ICoreSameTypeActualUseAidCoreActualUseTargetProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool MatchesActualUseTarget(global::CardGame.Core.ActualUseTargetIdentity identity);
}

internal interface ICoreSameTypeActualUseAidCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}

internal interface ICoreSameTypeActualUseAidCoreHpChangeProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryBeginHpChangedProgramWindow(long? resumeFrameId = default, global::CardGame.Core.PostEventContinuation continuation = (global::CardGame.Core.PostEventContinuation)0, int? cardId = default, global::CardGame.Core.CardKind? cardKind = default);
}

internal interface ICoreSameTypeActualUseAidCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreSameTypeActualUseAidCoreSkillProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void FinishProgramSkill(global::CardGame.Core.ProgramSkillFrame frame, bool completed);
}

internal interface ICoreSameTypeActualUseAidCoreProgramAtomicCardMovesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MoveProgramCardsFromMultipleSources(global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.CardMoveReason reason, global::System.Action<long, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardMovementRecord>>? afterPhysicalRecords = default, global::System.Func<global::CardGame.Core.Card, global::CardGame.Core.CardLocation, global::CardGame.Core.CardLocation>? destinationForCard = default, global::CardGame.Core.CardMovementDisclosure disclosure = (global::CardGame.Core.CardMovementDisclosure)0, global::System.Action<global::CardGame.Core.HappyWeiYanGainRequestSettlement>? afterRequestSettled = default, long? producerFrameId = default);
}

internal interface ICoreSameTypeActualUseAidCoreDirectedTurnCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsDirectedCardTargetProhibited(int actorSeat, int targetSeat, global::CardGame.Core.CardKind cardKind, long? cardUseFrameId = default);
}

internal interface ICoreSameTypeActualUseAidContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreSameTypeActualUseAidCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
    bool IsCardUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType actionType, bool ignoreIssuedPlayBan = false, long? existingUseFrameId = default);
}
