using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsShanheYongzhanDyingExitIdentity(ProgramTriggerCandidate candidate, int victim) =>
        _modeDefinition.Id == ShanheBattleSetup.BattleModeId && candidate.OwnerSeat == victim &&
        candidate.BindingId == ShanheYongzhanPrograms.TriggerId && ShanheYongzhanPrograms.DamageAmount(candidate.SkillId) > 0;

    private bool IsShanheYongzhanDyingExitCandidate(ProgramTriggerCandidate candidate, int victim)
    {
        if (!IsShanheYongzhanDyingExitIdentity(candidate, victim)) return false;
        var trigger = GetProgramTrigger(candidate);
        return ShanheYongzhanPrograms.IsTriggerShape(candidate.SkillId, trigger.Id, trigger.Window,
            trigger.Subject, trigger.Optional, trigger.Condition, trigger.Effects);
    }

    private bool IsShanheYongzhanDyingExitContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.DyingExited || context.OwnerSeat != candidate.OwnerSeat ||
            context.SourceSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat ||
            !IsShanheYongzhanDyingExitIdentity(candidate, candidate.OwnerSeat) ||
            _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is not
                { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit,
                  YanPuDyingExit: { } receipt } parent || parent.OwnerSeat != candidate.OwnerSeat ||
            receipt.VictimSeat != candidate.OwnerSeat || receipt.Round != _roundNumber || receipt.Turn != _turnNumber ||
            receipt.TurnOwner != _currentSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            parent.Candidates[parent.CandidateIndex] != candidate) return false;
        // The immutable trigger shape was admitted when this exact candidate
        // entered the window; continuations retain the normal hash guard.
        // Current eligibility is checked before issuing the binding. A paid
        // return keeps its original survived-Dying fact even if a later child
        // changes the holder's HP or life state.
        var index = _resolutionStack.FindIndex(frame => frame.Id == parent.Id);
        return index > 0 && _resolutionStack[index - 1].Id == receipt.DyingParentFrameId &&
            ProgramEventHistory<DyingResolvedEvent>().Count(fact => fact.ResolutionId == receipt.DyingFrameId &&
                fact.VictimSeat == receipt.VictimSeat && fact.Survived) == 1;
    }

    private bool IsShanheYongzhanDyingExitReturn(ProgramSkillFrame frame) =>
        frame.WindowContext is { } context &&
        _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(parent => parent.Id == context.ParentFrameId) is { } parent &&
        parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count &&
        MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]) &&
        IsShanheYongzhanDyingExitContext(parent.Candidates[parent.CandidateIndex], context);


    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 62174)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 62174)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 62174)]
    private bool HasShanheYongzhanDyingExitDamageObserver(long id)
    {
        var root = _resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(IsShanheYongzhanDyingExitReturn);
        return root is not null && _resolutionStack.FindIndex(frame => frame.Id == id &&
            frame is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) >
            _resolutionStack.FindIndex(frame => frame.Id == root.Id);
    }
}
