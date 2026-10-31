#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLeJiuAnniversaryRuntime
{
    internal LeJiuCuijinGrant? LeJiuGrantFor(CardUseFrame use, ProgramTriggerCandidate c, bool includeBound = false) => (use.LeJiuCuijin ?? []).SingleOrDefault(g => g.Source.OwnerSeat == c.OwnerSeat && g.Source.SkillInstanceId == c.SkillInstanceId && g.Hash == c.GameplayHash && ValidLeJiuGrant(use, g) && (includeBound || !_coreProgramEventHistoryOperations.ProgramEventHistory<LeJiuCuijinRewardBoundEvent>().Any(e => e.GrantFrameId == g.FrameId)));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 44100)]
    internal void CollectLeJiuCompletedUses(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.CardUseCompleted)
            return;
        result.RemoveAll(c => c.SkillId == AnniversaryLeJiuComposition.Skill && c.TriggerId == AnniversaryLeJiuComposition.Reward);
        if (_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == action.ActionId)is not { Step: ResolutionFrameStep.Completed, CausedDamage: false, LeJiuCuijin: { } grants } use)
            return;
        foreach (var g in grants.Where(g => ValidLeJiuGrant(use, g) && !_coreProgramEventHistoryOperations.ProgramEventHistory<LeJiuCuijinRewardBoundEvent>().Any(e => e.GrantFrameId == g.FrameId)))
            result.Add(new(g.Source.OwnerSeat, action.ActorSeat, g.Source.SkillId, AnniversaryLeJiuComposition.Reward, g.Hash, g.Source.SkillInstanceId!, 0, _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, g.Source.OwnerSeat, action.ActorSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[g.Source.OwnerSeat], action)with { CardUseCausedDamage = false })));
    }

    internal bool LeJiuParent(ProgramSkillFrame f) => f.WindowContext is { } w && _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is { } parent && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex])) && _coreCardActionsOperations.CreateCardActionProgramContext(parent, parent.Candidates[parent.CandidateIndex]) == w && parent.ParentFrameId == w.CardUse?.ParentCardUseFrameId && parent.Action.ActionId == w.CardUse.CardActionId;
    internal bool IsIssuedLeJiuCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w, bool includeBound)
    {
        if (c.SkillId != AnniversaryLeJiuComposition.Skill || c.BindingId != AnniversaryLeJiuComposition.Reward || w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || LeJiuContextUse(w)is not { Step: ResolutionFrameStep.Completed, CausedDamage: false } use || LeJiuGrantFor(use, c, includeBound)is null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId)is not { } parent || parent.ParentFrameId != use.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(parent.Candidates[parent.CandidateIndex]) != c)
            return false;
        return _coreCardActionsOperations.CreateCardActionProgramContext(parent, parent.Candidates[parent.CandidateIndex]) == w;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 44100)]
    internal bool AdmitLeJiuIssuedCompletion(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => IsIssuedLeJiuCandidate(c, w, false);
    internal static ProgramTriggerCandidate LeJiuCandidate(ProgramSkillFrame f) => new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext?.OccurrenceIndex ?? 0);
    internal void BindLeJiuCuijin(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.InstructionIndex != 1 || f.LeJiuCuijin is not null || f.WindowContext is not { } w || !IsIssuedLeJiuCandidate(LeJiuCandidate(f), w, false) || LeJiuContextUse(w)is not { } use || LeJiuGrantFor(use, LeJiuCandidate(f))is not { } grant)
            throw new InvalidOperationException("Cuijin must bind its one paid original whole-use no-damage result before drawing or damaging the user.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LeJiuCuijin = new(LeJiuSource(f), f.GameplayHash, w.ParentFrameId, use.Id, use.Action!.ActionId, use.SourceSeat, grant.Turn, 0, true, grant) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LeJiuCuijinRewardBoundEvent(f.Id, grant.FrameId, use.Id, use.Action!.ActionId, f.OwnerSeat, use.SourceSeat));
    }

    internal bool ValidLeJiuFrame(ProgramSkillFrame f)
    {
        if (f.LeJiuCuijin is not { } r || r.Source != LeJiuSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || f.WindowContext is not { } w || w.ParentFrameId != r.ParentId || LeJiuContextUse(w)is not { } use || use.Id != r.UseId || use.Action!.ActionId != r.ActionId || use.SourceSeat != r.Actor || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !LeJiuParent(f))
            return false;
        if (r.Reward)
            return f.InstructionIndex is >= 1 and <= 4 && r.Grant is { } grant && IsIssuedLeJiuCandidate(LeJiuCandidate(f), w, true) && LeJiuGrantFor(use, LeJiuCandidate(f), true) == grant && _coreProgramEventHistoryOperations.ProgramEventHistory<LeJiuCuijinRewardBoundEvent>().Count(e => e == new LeJiuCuijinRewardBoundEvent(f.Id, grant.FrameId, use.Id, r.ActionId, f.OwnerSeat, r.Actor)) == 1;
        return f.TriggerId == AnniversaryLeJiuComposition.Pay && w.Window == SkillProgramTriggerWindow.CardUseCommitted && r.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber && r.Before >= 0 && r.Before <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && f.InstructionIndex is >= 1 and <= 4 && _coreProgramEventHistoryOperations.ProgramEventHistory<LeJiuCuijinQualifiedEvent>().Count(e => e == new LeJiuCuijinQualifiedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.UseId, r.ActionId, r.Actor, r.Turn, r.Before)) == 1 && (r.Grant is null || ValidLeJiuGrant(use, r.Grant));
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 44100)]
    internal void AssertLeJiuFrame(ProgramSkillFrame f)
    {
        if (f.LeJiuCuijin is not null && !ValidLeJiuFrame(f))
            throw new InvalidOperationException("Cuijin lost its original action, observer candidate, paid grant or whole-use return.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44100)]
    internal bool ContinuePaidLeJiu(ProgramSkillFrame f) => f.LeJiuCuijin is { } r ? ValidLeJiuFrame(f) && (r.Reward || r.Grant is not null || LeJiuPaidCost(f)is not null) : f.InstructionIndex is 0 or 1 && f.WindowContext is { } w && IsIssuedLeJiuCandidate(LeJiuCandidate(f), w, false);
}
