#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreTargetedOrdinaryTrickCoreNativePromptBoundaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PromptId CreatePromptId();
    void ClearPendingDecision();
}

internal interface ICoreTargetedOrdinaryTrickCorePendingDecisionPolicyPipelineOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.PendingDecision? _pendingDecision { get; set; }
}

internal interface ICoreTargetedOrdinaryTrickResolutionStackOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.FrameStore _resolutionStack { get; }
}

internal interface ICoreTargetedOrdinaryTrickCoreUniqueLeaderTrickTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool CanBeUniqueLeaderTrickTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
}

internal interface ICoreTargetedOrdinaryTrickCoreTargetedTrickHandHpAiOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    double TargetedTrickPublicChoiceScore(int owner, global::CardGame.Core.CardUseFrame use, global::CardGame.Core.PromptChoice choice);
}

internal interface ICoreTargetedOrdinaryTrickCoreProgramCardUseRolesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void UpdateProgramRoleCardUse(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CardActionContext action);
}

internal interface ICoreTargetedOrdinaryTrickCoreUnequalHandHpEndingOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardConversionSource TargetedTrickHandHpSource(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreTargetedOrdinaryTrickCoreNativeBorrowedSwordOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default);
}

internal interface ICoreTargetedOrdinaryTrickCoreCardUseModulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool HasTurnCardTargetRestriction(int actorSeat, global::CardGame.Core.SkillProgramCardTargetRestriction restriction, int? targetSeat = default);
}

internal interface ICoreTargetedOrdinaryTrickTurnCardUseEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.TurnCardUseEffectStore _turnCardUseEffects { get; }
}

internal interface ICoreTargetedOrdinaryTrickCorePrivateOfferReturnsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.SkillProgramEffect? GetPausedPrivateOfferEffect(global::CardGame.Core.ProgramSkillFrame f);
}

internal interface ICoreTargetedOrdinaryTrickCoreProgramCardTargetEffectsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void MarkCardEffectIneffective(long resolutionId, int targetSeat);
}

internal interface ICoreTargetedOrdinaryTrickCoreAlternatingBenefitsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IEnumerable<global::CardGame.Core.IGameEvent> CompleteProgramEventHistory();
}

internal interface ICoreTargetedOrdinaryTrickContentRegistryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ContentRegistry _contentRegistry { get; }
}

internal interface ICoreTargetedOrdinaryTrickCoreRuntimeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ReplaceRuntimeTop(global::CardGame.Core.ResolutionFrame next);
    void AdvanceRuntimeProgram(long frameId);
}

internal interface ICoreTargetedOrdinaryTrickCoreDesignatedExtraTargetsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool TryGetDesignatedExtraTargetUse(int owner, global::CardGame.Core.ProgramSkillWindowContext context, out global::CardGame.Core.CardUseFrame use, out global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    bool CanBeDistanceFreeDesignatedTarget(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.CharacterState target);
    bool DesignatedExtraTargetCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context, global::CardGame.Core.ProgramCardTriggerWindowFrame window);
    global::CardGame.Core.CardActionContext CloneDesignatedExtraTargetAction(global::CardGame.Core.CardActionContext action, global::System.Collections.Generic.IReadOnlyList<int> targets);
}

internal interface ICoreTargetedOrdinaryTrickCoreCardUseLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.CardUseFrame? LifecycleCardUse(long frameId);
}

internal interface ICoreTargetedOrdinaryTrickCoreCompletedCategoryUseProofOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool ActualHandGainMutationBinding(long frameId, string skill, int owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::CardGame.Core.SkillProgramEffectOp operation);
}

internal interface ICoreTargetedOrdinaryTrickKernelCoreGameEngineRuleProgressionOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void AdvanceEventRulesAndQueueFact(global::CardGame.Core.IGameEvent payload);
}

internal interface ICoreTargetedOrdinaryTrickCoreCurrentUseTargetCandidateRefreshOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RefreshCurrentUseAddedTargetCandidates(global::CardGame.Core.CardUseFrame use, global::System.Collections.Generic.IReadOnlyList<int> addedMainTargets);
}

internal interface ICoreTargetedOrdinaryTrickCoreProgramLifecycleOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramSkillFrame GetActiveProgramFrame(long frameId);
}
