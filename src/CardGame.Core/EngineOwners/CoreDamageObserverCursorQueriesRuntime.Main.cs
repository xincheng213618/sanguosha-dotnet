#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreDamageObserverCursorQueriesRuntime
{
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 280, group: 10)]
    internal bool IsPendingDamageProgramDying()
    {
        if (_coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } trigger || _coreDyingCursorRuntimeOperations.ActiveDying is not { ResumesProgramSkill: true } dying || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.Id == dying.ParentFrameId)is not { } program || program.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, ParentFrameId: var parentFrameId } || parentFrameId != trigger.Id || _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().LastOrDefault(frame => frame.Id == dying.FrameId)is not { } dyingFrame || dyingFrame.ParentFrameId != program.Id)
            return false;
        return _resolutionStackOperations._resolutionStack.LastOrDefault()is DyingFrame topDying && topDying.Id == dying.FrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame responseProgram && responseProgram.WindowContext is { Window: SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse, ParentFrameId: var dyingParentId } && dyingParentId == dying.FrameId;
    }

    internal ResolutionFrame? DamageCursorEffectiveTop(bool includeNestedObservers = false) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.DamageCursorEffectiveTop(includeNestedObservers);
    internal static bool DamageObserverRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.DamageObserverRidesOn(ride, beneath);
    internal ProgramSkillFrame? TypedDamageProgramObserver(long windowId) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.TypedDamageProgramObserver(windowId);
    internal bool DamageProgramMatchesWindow(ProgramSkillFrame root, DamageTriggerWindowFrame window) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.DamageProgramMatchesWindow(root, window);
    internal bool TypedDamageObserverCandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.TypedDamageObserverCandidateMatches(frame, candidate);
    internal bool TypedDamagePostEventEdge(ResolutionFrame parent, ResolutionFrame child) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.TypedDamagePostEventEdge(parent, child);
    internal static bool ProgramMovementReturnMatches(ProgramSkillFrame producer, CardsMovedTriggerWindowFrame movement, bool allowFactionRequestCost = false) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement, allowFactionRequestCost);
    internal static bool ProgramHpReturnMatches(ProgramSkillFrame producer, HpChangedTriggerWindowFrame changed, bool allowFactionRequestCost = false) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramHpReturnMatches(producer, changed, allowFactionRequestCost);
    internal static bool HasAlternateProgramMovementReturn(CardsMovedTriggerWindowFrame movement, bool allowFactionRequestCost = false) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_CommonRules.HasAlternateProgramMovementReturn(movement, allowFactionRequestCost);
    internal DamageCursorProjection ResolveDamageCursor(DamageTriggerWindowFrame window) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.ResolveDamageCursor(window);
    internal bool HasMalformedDamagePostEventEdge(long windowId) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.HasMalformedDamagePostEventEdge(windowId);
    internal bool DamageFrameRidesOn(ResolutionFrame ride, ResolutionFrame beneath) => _coreDamageAdjacencyServicesOperations.DamageAdjacency.DamageFrameRidesOn(ride, beneath);
}
