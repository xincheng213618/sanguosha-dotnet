#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePreventionDrawObserversRuntime
{
    internal bool IsValidPreventionDrawReceipt(ProgramSkillFrame root, BeforeDamageProgramWindowFrame parent)
    {
        var participant = _coreParticipantPreventionOperations.Value.IsParticipantPreventionDraw(root);
        if (root.PreventionDrawReceipt is not { } receipt || root.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context || context.ParentFrameId != parent.Id || !participant && context.TargetSeat != root.OwnerSeat || context.Amount != parent.Amount || receipt.BeforeDamageFrameId != parent.Id || receipt.TargetSeat != parent.TargetSeat || !participant && parent.TargetSeat != root.OwnerSeat || participant && (parent.SourceSeat == parent.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(parent.SourceSeat) || root.OwnerSeat != parent.SourceSeat && root.OwnerSeat != parent.TargetSeat) || receipt.SourceSeat != parent.SourceSeat || receipt.PreventedAmount != parent.Amount || !parent.Prevented || parent.Amount <= 0 || receipt.InstructionIndex != root.InstructionIndex - 1 || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex].Candidate))
            return false;
        var instructions = ProgramInstructionResolver.Default.Resolve(root, _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program!).Instructions;
        if (receipt.InstructionIndex < 0 || receipt.InstructionIndex >= instructions.Count || instructions[receipt.InstructionIndex] is not { } effect || effect.Op != (participant ? SkillProgramEffectOp.PreventParticipantDamageDrawActualSource : SkillProgramEffectOp.PreventCurrentDamageAndDrawMultiple) || receipt.Multiplier != (participant ? 2 : effect.Amount) || receipt.ActualDrawCount < 0 || receipt.ActualDrawCount > (participant ? 2 : checked(parent.Amount * effect.Amount)) || receipt.MovementSequenceBefore < 0 || receipt.MovementSequenceAfter < receipt.MovementSequenceBefore)
            return false;
        var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.MovementSequenceBefore && m.Sequence <= receipt.MovementSequenceAfter).ToArray();
        bool IsActualDraw(CardMovementRecord m) => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(participant ? parent.SourceSeat : root.OwnerSeat) && m.Reason.Value == $"skill-program.{(root.SkillId)}.prevented-damage-draw";
        if (movements.Count(IsActualDraw) != receipt.ActualDrawCount || movements.Any(m => !IsActualDraw(m) && !(m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
            return false;
        return _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(item => item.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents).OfType<ProgramDamagePreventedEvent>().Any(fact => fact.FrameId == parent.Id && fact.SkillId == root.SkillId && fact.BindingId == root.TriggerId && fact.OwnerSeat == root.OwnerSeat && fact.SourceSeat == context.SourceSeat && fact.TargetSeat == parent.TargetSeat && fact.Amount == parent.Amount);
    }

    internal ProgramSkillFrame? PreventionDrawObserverRoot(long beforeDamageId)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame parent || parent.Id != beforeDamageId || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !IsValidPreventionDrawReceipt(root, parent) || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat || _resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame first || first.Batch.ParentFrameId != root.Id || first.Batch.AwaitingProgramFrameId is { } waiting && waiting != root.Id || first.Batch.OriginSkillId != root.SkillId || first.Batch.OriginSkillInstanceId != root.SkillInstanceId || first.Batch.OriginOwnerSeat != root.OwnerSeat || first.Batch.Movements.Count == 0 || first.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.Sequence <= root.PreventionDrawReceipt!.MovementSequenceBefore || m.Sequence > root.PreventionDrawReceipt.MovementSequenceAfter))
                continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                // This helper proves the entire exact program/Alcohol use and
                // its descendants. Do not re-check those already proved edges
                // as if the Alcohol use were a direct native Dying response.
                if (_resolutionStackOperations._resolutionStack[child - 1] is DyingFrame alcoholDying && _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child - 1, alcoholDying))
                    break;
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                {
                    aligned = false;
                    break;
                }
            }

            if (aligned)
                return root;
        }

        return null;
    }
    internal bool HasPreventionDrawDying(long beforeDamageId) => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { ResumesProgramSkill: true } dying && PreventionDrawObserverRoot(beforeDamageId)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 400)]
    internal bool IsPreventionDrawProgramDying() => _resolutionStackOperations._resolutionStack.OfType<BeforeDamageProgramWindowFrame>().Any(parent => HasPreventionDrawDying(parent.Id));
}
