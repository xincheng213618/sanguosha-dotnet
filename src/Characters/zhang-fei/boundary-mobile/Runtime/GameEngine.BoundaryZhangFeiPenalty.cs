namespace CardGame.Core;

public sealed partial class GameEngine
{
    private CardUseFrame? BoundaryZhangFeiPenaltyUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target } || !_players[target].IsAlive ||
            ActiveDamageTrigger is not { } window || window.Id != context.ParentFrameId || window.ParentFrameId != context.DamageFrameId) return null;
        var attack = GetDamageTriggerAttack(window);
        return attack is CardAttackHandle && !attack.IsChainPropagation && attack.DamageWasApplied && attack.TargetSeat == target &&
            LifecycleCardUse(attack.ResolutionId) is { BoundaryLiyong: { } right, Action: { } action } use && BoundaryZhangFeiSourceMatches(right, candidate) &&
            action.ActionId == right.CardActionId && action.EffectiveDesignatedTargetSeats.Contains(target) ? use : null;
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 58600)]
    private IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryZhangFeiPenalty(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (window != SkillProgramTriggerWindow.AfterDamageApplied || CurrentDamageAttempt is not { } attack) return ordinary;
        if (LifecycleCardUse(attack.ResolutionId)?.BoundaryLiyong is not { } right || right.Reserve.Source.OwnerSeat != owner.Seat) return ordinary;
        var trigger = _contentRegistry.GetSkill(BoundaryZhangFeiLiyong).Program!.Triggers.Single(item => item.Id == BoundaryZhangFeiPenalty);
        return Array.AsReadOnly(ordinary.Where(candidate => candidate.SkillId != BoundaryZhangFeiLiyong || candidate.BindingId != BoundaryZhangFeiPenalty)
            .Append(new ProgramTriggerCandidate(owner.Seat, BoundaryZhangFeiLiyong, BoundaryZhangFeiPenalty, right.Reserve.Source.SkillInstanceId, right.Reserve.GameplayHash, trigger.Priority)).ToArray());
    }
    private SkillProgramStepOutcome PayBoundaryZhangFeiLiyong(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiyongPenalty is { Paid: true })
        {
            if (!ValidBoundaryZhangFeiLiyongPenalty(frame)) throw new InvalidOperationException("Liyong lost its already-paid original damage return.");
            return SkillProgramStepOutcome.Continue;
        }
        var context = frame.WindowContext!;
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, GetProgramTrigger(frame).Priority);
        var use = BoundaryZhangFeiPenaltyUse(candidate, context) ?? throw new InvalidOperationException("Liyong penalty requires its exact surviving direct target after real Slash damage.");
        if (frame.BoundaryLiyongPenalty is null) ReplaceRuntimeTop(frame = frame with { BoundaryLiyongPenalty = new(use.Id, use.Action!.ActionId,
            context.ParentFrameId, context.DamageFrameId!.Value, context.TargetSeat!.Value, frame.InstructionIndex) });
        if (HasHappyQiaoHealth(frame.OwnerSeat) && TryBeginHappyQiaoProgramHealthChoice(frame, frame.OwnerSeat, HpChangeKind.Loss, 1)) return SkillProgramStepOutcome.AwaitChild;
        return ApplyProgramHpLoss(frame.Id, frame.SkillId, frame.OwnerSeat, 1, (before, after, lost) => {
            var current = GetActiveProgramFrame(frame.Id); var receipt = current.BoundaryLiyongPenalty!;
            ReplaceRuntimeTop(current with { BoundaryLiyongPenalty = receipt with { Paid = true, HpBefore = before, HpAfter = after } });
            AdvanceEventRulesAndQueueFact(new BoundaryLiyongPenaltyPaidEvent(frame.Id, receipt.CardUseFrameId, receipt.DamageWindowId, receipt.TargetSeat, frame.OwnerSeat, before, after, lost));
            if (HasHappyQiaoHealth(frame.OwnerSeat)) CompleteHappyQiaoHealthPayment(frame.Id, frame.OwnerSeat, HpChangeKind.Loss, before, after);
        });
    }
    private bool ValidBoundaryZhangFeiLiyongPenalty(ProgramSkillFrame frame) => frame is { SkillId: BoundaryZhangFeiLiyong, TriggerId: BoundaryZhangFeiPenalty, InstructionIndex: 1, BoundaryLiyongPenalty: { } receipt } &&
        frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied } context && receipt.InstructionIndex == frame.InstructionIndex &&
        receipt.DamageWindowId == context.ParentFrameId && receipt.DamageFrameId == context.DamageFrameId && receipt.TargetSeat == context.TargetSeat &&
        context.Amount > 0 && _resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(window => window.Id == receipt.DamageWindowId) is { } damage && damage.ParentFrameId == receipt.DamageFrameId &&
        LifecycleCardUse(receipt.CardUseFrameId) is { Action: { } action, BoundaryLiyong: { } right } && action.ActionId == receipt.CardActionId &&
        right.Reserve.Source.SkillInstanceId == frame.SkillInstanceId && right.Reserve.GameplayHash == frame.GameplayHash && right.Reserve.Source.OwnerSeat == frame.OwnerSeat &&
        (!receipt.Paid || receipt.HpBefore is { } before && receipt.HpAfter == before - 1 && ProgramEventHistory<BoundaryLiyongPenaltyPaidEvent>().Count(fact =>
            fact.ProducerFrameId == frame.Id && fact.CardUseFrameId == receipt.CardUseFrameId && fact.DamageWindowId == receipt.DamageWindowId && fact.TargetSeat == receipt.TargetSeat && fact.OwnerSeat == frame.OwnerSeat && fact.HpBefore == before && fact.HpAfter == receipt.HpAfter && fact.ActualLost == 1) == 1);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 58600)]
    private bool ContinueBoundaryZhangFeiPenalty(ProgramSkillFrame frame) => frame.BoundaryLiyongPenalty is not null && ValidBoundaryZhangFeiLiyongPenalty(frame);
    [ProgramContinuation(ProgramContinuationKind.Resume, 58600)]
    private bool ResumeBoundaryZhangFeiPenalty(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryLiyongPenalty: { } receipt } frame || frame.Id != id) return false;
        if (!ValidBoundaryZhangFeiLiyongPenalty(frame)) throw new InvalidOperationException("Liyong cannot resume a different damage or HP payment.");
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return true;
        if (!receipt.Paid)
        {
            if (PayBoundaryZhangFeiLiyong(frame) == SkillProgramStepOutcome.AwaitChild) return true;
            return ResumeBoundaryZhangFeiPenalty(id);
        }
        FinishProgramSkill(frame, true); return true;
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 58600)]
    private void AssertBoundaryZhangFeiPenalty(ProgramSkillFrame frame)
    { if (frame.BoundaryLiyongPenalty is not null && !ValidBoundaryZhangFeiLiyongPenalty(frame)) throw new InvalidOperationException("Liyong penalty lost original Use, actual damage or once-paid HP proof."); }
}
