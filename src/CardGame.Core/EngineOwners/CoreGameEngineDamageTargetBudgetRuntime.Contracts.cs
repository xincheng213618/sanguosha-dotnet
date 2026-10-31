#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreDamageTargetBudgetCoreAttributedMarkerEventsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MutateParticipantMarker(global::CardGame.Core.ProgramSkillFrame frame, int seat, global::CardGame.Core.PlayerMarkerKind marker, int delta, bool activeOnly = false);
}

internal interface ICoreDamageTargetBudgetResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreDamageTargetBudgetCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreDamageTargetBudgetCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreDamageTargetBudgetCoreNativeDamageHandlesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.GameEngine.IDamageAttempt GetDamageTriggerAttack(global::CardGame.Core.DamageTriggerWindowFrame frame);
}

internal interface ICoreDamageTargetBudgetContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreDamageTargetBudgetCoreProgramMarkerPaymentsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void PayProgramMarkerCost(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramMarkerCost? cost, string skillId, string bindingId);
}

internal interface ICoreDamageTargetBudgetCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreDamageTargetBudgetCoreTableSeatQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsValidPlayerSeat(int seat);
}

internal interface ICoreDamageTargetBudgetCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateLifecycleCardUse(long frameId, global::System.Func<global::CardGame.Core.CardUseFrame, global::CardGame.Core.CardUseFrame> update);
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreDamageTargetBudgetCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreDamageTargetBudgetCoreCurrentUseTargetCandidateRefreshOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RefreshCurrentUseAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> addedMainTargets);
}

internal interface ICoreDamageTargetBudgetCoreUniqueLeaderTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeUniqueLeaderTrickTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreDamageTargetBudgetCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreDamageTargetBudgetCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetDesignatedExtraTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreDamageTargetBudgetKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
    void AdvanceRulesAndPublishState();
}

internal interface ICoreDamageTargetBudgetCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreDamageTargetBudgetCoreRecipientCategorySlashTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeRecipientCategorySlashTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreDamageTargetBudgetCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
    global::CardGame.Core.SkillProgramTrigger GetProgramTrigger(global::CardGame.Core.ProgramSkillFrame frame);
}
