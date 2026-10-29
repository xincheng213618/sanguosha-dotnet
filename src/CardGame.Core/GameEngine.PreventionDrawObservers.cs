namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool IsValidPreventionDrawReceipt(ProgramSkillFrame root, BeforeDamageProgramWindowFrame parent)
    {
        var participant = IsParticipantPreventionDraw(root);
        if (root.PreventionDrawReceipt is not { } receipt || root.WindowContext is not
            { Window: SkillProgramTriggerWindow.BeforeDamageApplied } context ||
            context.ParentFrameId != parent.Id || !participant && context.TargetSeat != root.OwnerSeat || context.Amount != parent.Amount ||
            receipt.BeforeDamageFrameId != parent.Id || receipt.TargetSeat != parent.TargetSeat || !participant && parent.TargetSeat != root.OwnerSeat ||
            participant && (parent.SourceSeat == parent.TargetSeat || !IsValidPlayerSeat(parent.SourceSeat) || root.OwnerSeat != parent.SourceSeat && root.OwnerSeat != parent.TargetSeat) ||
            receipt.SourceSeat != parent.SourceSeat || receipt.PreventedAmount != parent.Amount || !parent.Prevented ||
            parent.Amount <= 0 || receipt.InstructionIndex != root.InstructionIndex - 1 ||
            parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
            !MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex].Candidate)) return false;
        var instructions = ProgramInstructionResolver.Default.Resolve(root, _contentRegistry.GetSkill(root.SkillId).Program!).Instructions;
        if (receipt.InstructionIndex < 0 || receipt.InstructionIndex >= instructions.Count ||
            instructions[receipt.InstructionIndex] is not { } effect ||
            effect.Op != (participant ? SkillProgramEffectOp.PreventParticipantDamageDrawActualSource : SkillProgramEffectOp.PreventCurrentDamageAndDrawMultiple) ||
            receipt.Multiplier != (participant ? 2 : effect.Amount) || receipt.ActualDrawCount < 0 || receipt.ActualDrawCount > (participant ? 2 : checked(parent.Amount * effect.Amount)) ||
            receipt.MovementSequenceBefore < 0 || receipt.MovementSequenceAfter < receipt.MovementSequenceBefore) return false;
        var movements = _cardMovements.Where(m => m.Sequence > receipt.MovementSequenceBefore && m.Sequence <= receipt.MovementSequenceAfter).ToArray();
        bool IsActualDraw(CardMovementRecord m) => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(participant ? parent.SourceSeat : root.OwnerSeat) &&
            m.Reason.Value == $"skill-program.{root.SkillId}.prevented-damage-draw";
        if (movements.Count(IsActualDraw) != receipt.ActualDrawCount || movements.Any(m => !IsActualDraw(m) &&
            !(m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle))) return false;
        return _events.Select(item => item.Payload).Concat(_pendingEvents).OfType<ProgramDamagePreventedEvent>().Any(fact =>
            fact.FrameId == parent.Id && fact.SkillId == root.SkillId && fact.BindingId == root.TriggerId &&
            fact.OwnerSeat == root.OwnerSeat && fact.SourceSeat == context.SourceSeat && fact.TargetSeat == parent.TargetSeat && fact.Amount == parent.Amount);
    }

    private ProgramSkillFrame? PreventionDrawObserverRoot(long beforeDamageId)
    {
        for (var index = 1; index + 1 < _resolutionStack.Count; index++)
        {
            if (_resolutionStack[index - 1] is not BeforeDamageProgramWindowFrame parent || parent.Id != beforeDamageId ||
                _resolutionStack[index] is not ProgramSkillFrame root || !IsValidPreventionDrawReceipt(root, parent) ||
                root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat ||
                _resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame first ||
                first.Batch.ParentFrameId != root.Id || first.Batch.AwaitingProgramFrameId is { } waiting && waiting != root.Id ||
                first.Batch.OriginSkillId != root.SkillId || first.Batch.OriginSkillInstanceId != root.SkillInstanceId || first.Batch.OriginOwnerSeat != root.OwnerSeat ||
                first.Batch.Movements.Count == 0 || first.Batch.Movements.Any(m => !_cardMovements.Contains(m) ||
                    m.Sequence <= root.PreventionDrawReceipt!.MovementSequenceBefore || m.Sequence > root.PreventionDrawReceipt.MovementSequenceAfter)) continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStack.Count; child++)
            {
                // This helper proves the entire exact program/Alcohol use and
                // its descendants. Do not re-check those already proved edges
                // as if the Alcohol use were a direct native Dying response.
                if (_resolutionStack[child - 1] is DyingFrame alcoholDying &&
                    IsPaidHandRepaymentProgramAlcoholRide(child - 1, alcoholDying)) break;
                if (!ProgramChildResolutionEdge(child)) { aligned = false; break; }
            }
            if (aligned) return root;
        }
        return null;
    }


    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 30)]
    private bool HasPreventionDrawDying(long beforeDamageId) => ActiveDying is { ResumesProgramSkill: true } dying &&
        PreventionDrawObserverRoot(beforeDamageId) is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 400)]
    private bool IsPreventionDrawProgramDying() => _resolutionStack.OfType<BeforeDamageProgramWindowFrame>()
        .Any(parent => HasPreventionDrawDying(parent.Id));

    // An outer after-damage cursor is admitted only when this exact original
    // program attack return names it and the ancestry still contains that live
    // candidate. A different prevention/Dying chain cannot bless the cursor.
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 390, group: 10)]
    private bool IsPreventionDrawInsideDamageProgramDying()
    {
        if (ActiveDamageTrigger is not { } damage) return false;
        foreach (var before in _resolutionStack.OfType<BeforeDamageProgramWindowFrame>())
        {
            if (!HasPreventionDrawDying(before.Id)) continue;
            var attackId = before.ContinuationAttackResolutionId ?? before.ParentFrameId;
            var index = _resolutionStack.FindIndex(f => f.Id == attackId);
            if (index < 1 || _resolutionStack[index] is not ProgramSkillFrame { AttackAttempt: not null, AttackReturn: { } returned } attack ||
                returned.ParentDamageWindowFrameId != damage.Id || _resolutionStack[index - 1] is not DamageTriggerWindowFrame parent || parent.Id != damage.Id ||
                parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count ||
                !MountObserverCandidateMatches(attack, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()) ||
                attack.WindowContext?.ParentFrameId != parent.Id) continue;
            return true;
        }
        return false;
    }
}
