#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaWanOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26510)]
    internal void AssertMaWan(ProgramSkillFrame f)
    {
        if (f.MaWanHunjiang is not { } r)
            return;
        if (f.SkillId != "ol:hunjiang" || r.Source != MaWanSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || !Enum.IsDefined(r.Stage) || r.Participants.Distinct().Count() != r.Participants.Count || r.Votes.Count > r.Participants.Count || !r.Votes.Select(v => v.Seat).SequenceEqual(r.Participants.Take(r.Votes.Count)) || r.SelectedTargets.Distinct().Count() != r.SelectedTargets.Count || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !ContinuePaidMaWan(f))
            throw new InvalidOperationException("Hunjiang lost its exact source, phase, owning instruction, frozen participant order or private commitments.");
        if (r.Stage == MaWanHunjiangStage.AppendTargets)
        {
            var use = RequireMaWanUse(f);
            if (r.CardUseId != use.Id || r.ActionId != use.Action!.ActionId || r.Participants.Count != 0 || r.Votes.Count != 0 || r.SelectedTargets.Any(s => !use.MaWanHunjiang!.Grants.Any(g => g.TargetSeat == s && g.Source.SkillId == f.SkillId && g.Source.SkillInstanceId == f.SkillInstanceId)))
                throw new InvalidOperationException("Hunjiang's native Slash extension lost its exact issued target membership.");
            return;
        }

        if (f.TriggerId is not null || f.WindowContext is not null || r.CardUseId is not null || r.ActionId is not null || r.Participants.Count == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<MaWanHunjiangStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurnNumber == r.ActualTurnNumber && e.PhaseInstanceId == r.PhaseInstanceId && e.ParticipantCount == r.Participants.Count) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<MaWanHunjiangParticipantFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Participants))
            throw new InvalidOperationException("Hunjiang's independent choices lost their original paid active entry.");
        var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<MaWanHunjiangVoteRevealedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == MaWanHunjiangStage.PrivateVotes && revealed.Length != 0 || r.Stage == MaWanHunjiangStage.DrawChildren && (r.Votes.Count != r.Participants.Count || revealed.Length != r.Participants.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<MaWanHunjiangDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Count == r.DrawCount && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) != 1))
            throw new InvalidOperationException("Hunjiang cannot reveal partial votes or repeat the settled draw.");
    }

    internal bool MaWanHasMovement(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved) => f.MaWanHunjiang is { Stage: MaWanHunjiangStage.DrawChildren } r && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MaWanDrawReason && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    internal bool MaWanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MaWanHunjiang is not { Stage: MaWanHunjiangStage.DrawChildren })
            return false;
        AssertMaWan(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return MaWanHasMovement(f, moved);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool MaWanObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MaWanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterMaWanOlOperations.MaWanFirstChild(f, child), (i) => _characterMaWanOlOperations.MaWanObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 440)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 550)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 580)]
    internal bool HasMaWanDamageObserver(long id) => MaWanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 410)]
    internal bool IsMaWanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MaWanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26510)]
    internal bool AdvanceMaWanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaWanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
