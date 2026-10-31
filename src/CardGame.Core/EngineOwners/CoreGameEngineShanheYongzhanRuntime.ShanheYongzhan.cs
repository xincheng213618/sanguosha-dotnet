#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheYongzhanRuntime
{
    internal bool IsShanheYongzhanDyingExitIdentity(ProgramTriggerCandidate candidate, int victim) => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && candidate.OwnerSeat == victim && candidate.BindingId == ShanheYongzhanPrograms.TriggerId && ShanheYongzhanPrograms.DamageAmount(candidate.SkillId) > 0;
    internal bool IsShanheYongzhanDyingExitCandidate(ProgramTriggerCandidate candidate, int victim)
    {
        if (!IsShanheYongzhanDyingExitIdentity(candidate, victim))
            return false;
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
        return ShanheYongzhanPrograms.IsTriggerShape(candidate.SkillId, trigger.Id, trigger.Window, trigger.Subject, trigger.Optional, trigger.Condition, trigger.Effects);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 120, SkillProgramTriggerWindow.DyingExited)]
    internal bool IsShanheYongzhanDyingExitContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.DyingExited || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || !IsShanheYongzhanDyingExitIdentity(candidate, candidate.OwnerSeat) || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } receipt } parent || parent.OwnerSeat != candidate.OwnerSeat || receipt.VictimSeat != candidate.OwnerSeat || receipt.Round != _coreActualRoundProgressRoundNumberAccess._roundNumber || receipt.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || receipt.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate)
            return false;
        // The immutable trigger shape was admitted when this exact candidate
        // entered the window; continuations retain the normal hash guard.
        // Current eligibility is checked before issuing the binding. A paid
        // return keeps its original survived-Dying fact even if a later child
        // changes the holder's HP or life state.
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == parent.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == receipt.DyingParentFrameId && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(fact => fact.ResolutionId == receipt.DyingFrameId && fact.VictimSeat == receipt.VictimSeat && fact.Survived) == 1;
    }

    internal bool IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) => frame.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(parent => parent.Id == context.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) && IsShanheYongzhanDyingExitContext(parent.Candidates[parent.CandidateIndex], context);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 62174)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 62174)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 62174)]
    internal bool HasShanheYongzhanDyingExitDamageObserver(long id)
    {
        var root = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(IsShanheYongzhanDyingExitReturn);
        return root is not null && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == id && frame is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    }
}
