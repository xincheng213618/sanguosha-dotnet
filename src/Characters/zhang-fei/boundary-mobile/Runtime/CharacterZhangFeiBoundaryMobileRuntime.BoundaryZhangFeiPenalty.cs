#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangFeiBoundaryMobileRuntime
{
    internal CardUseFrame? BoundaryZhangFeiPenaltyUse(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, TargetSeat: { } target } || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } window || window.Id != context.ParentFrameId || window.ParentFrameId != context.DamageFrameId)
            return null;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        return attack is global::CardGame.Core.NativeOwners.CardAttackHandle && !attack.IsChainPropagation && attack.DamageWasApplied && attack.TargetSeat == target && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { BoundaryLiyong: { } right, Action: { } action } use && BoundaryZhangFeiSourceMatches(right, candidate) && action.ActionId == right.CardActionId && action.EffectiveDesignatedTargetSeats.Contains(target) ? use : null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 58600)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryZhangFeiPenalty(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary)
    {
        if (window != SkillProgramTriggerWindow.AfterDamageApplied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack)
            return ordinary;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.BoundaryLiyong is not { } right || right.Reserve.Source.OwnerSeat != owner.Seat)
            return ordinary;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(BoundaryZhangFeiLiyong).Program!.Triggers.Single(item => item.Id == BoundaryZhangFeiPenalty);
        return Array.AsReadOnly(ordinary.Where(candidate => candidate.SkillId != BoundaryZhangFeiLiyong || candidate.BindingId != BoundaryZhangFeiPenalty).Append(new ProgramTriggerCandidate(owner.Seat, BoundaryZhangFeiLiyong, BoundaryZhangFeiPenalty, right.Reserve.Source.SkillInstanceId, right.Reserve.GameplayHash, trigger.Priority)).ToArray());
    }

    internal SkillProgramStepOutcome PayBoundaryZhangFeiLiyong(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiyongPenalty is { Paid: true })
        {
            if (!ValidBoundaryZhangFeiLiyongPenalty(frame))
                throw new InvalidOperationException("Liyong lost its already-paid original damage return.");
            return SkillProgramStepOutcome.Continue;
        }

        var context = frame.WindowContext!;
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(frame).Priority);
        var use = BoundaryZhangFeiPenaltyUse(candidate, context) ?? throw new InvalidOperationException("Liyong penalty requires its exact surviving direct target after real Slash damage.");
        if (frame.BoundaryLiyongPenalty is null)
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryLiyongPenalty = new(use.Id, use.Action!.ActionId, context.ParentFrameId, context.DamageFrameId!.Value, context.TargetSeat!.Value, frame.InstructionIndex) });
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(frame.OwnerSeat) && _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoProgramHealthChoice(frame, frame.OwnerSeat, HpChangeKind.Loss, 1))
            return SkillProgramStepOutcome.AwaitChild;
        return _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(frame.Id, frame.SkillId, frame.OwnerSeat, 1, (before, after, lost) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var receipt = current.BoundaryLiyongPenalty!;
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { BoundaryLiyongPenalty = receipt with { Paid = true, HpBefore = before, HpAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryLiyongPenaltyPaidEvent(frame.Id, receipt.CardUseFrameId, receipt.DamageWindowId, receipt.TargetSeat, frame.OwnerSeat, before, after, lost));
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(frame.OwnerSeat))
                _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(frame.Id, frame.OwnerSeat, HpChangeKind.Loss, before, after);
        });
    }

    internal bool ValidBoundaryZhangFeiLiyongPenalty(ProgramSkillFrame frame) => frame is { SkillId: BoundaryZhangFeiLiyong, TriggerId: BoundaryZhangFeiPenalty, InstructionIndex: 1, BoundaryLiyongPenalty: { } receipt } && frame.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied } context && receipt.InstructionIndex == frame.InstructionIndex && receipt.DamageWindowId == context.ParentFrameId && receipt.DamageFrameId == context.DamageFrameId && receipt.TargetSeat == context.TargetSeat && context.Amount > 0 && _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().SingleOrDefault(window => window.Id == receipt.DamageWindowId)is { } damage && damage.ParentFrameId == receipt.DamageFrameId && _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.CardUseFrameId)is { Action: { } action, BoundaryLiyong: { } right } && action.ActionId == receipt.CardActionId && right.Reserve.Source.SkillInstanceId == frame.SkillInstanceId && right.Reserve.GameplayHash == frame.GameplayHash && right.Reserve.Source.OwnerSeat == frame.OwnerSeat && (!receipt.Paid || receipt.HpBefore is { } before && receipt.HpAfter == before - 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiyongPenaltyPaidEvent>().Count(fact => fact.ProducerFrameId == frame.Id && fact.CardUseFrameId == receipt.CardUseFrameId && fact.DamageWindowId == receipt.DamageWindowId && fact.TargetSeat == receipt.TargetSeat && fact.OwnerSeat == frame.OwnerSeat && fact.HpBefore == before && fact.HpAfter == receipt.HpAfter && fact.ActualLost == 1) == 1);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 58600)]
    internal bool ContinueBoundaryZhangFeiPenalty(ProgramSkillFrame frame) => frame.BoundaryLiyongPenalty is not null && ValidBoundaryZhangFeiLiyongPenalty(frame);
    [ProgramContinuation(ProgramContinuationKind.Resume, 58600)]
    internal bool ResumeBoundaryZhangFeiPenalty(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryLiyongPenalty: { } receipt } frame || frame.Id != id)
            return false;
        if (!ValidBoundaryZhangFeiLiyongPenalty(frame))
            throw new InvalidOperationException("Liyong cannot resume a different damage or HP payment.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        if (!receipt.Paid)
        {
            if (PayBoundaryZhangFeiLiyong(frame) == SkillProgramStepOutcome.AwaitChild)
                return true;
            return ResumeBoundaryZhangFeiPenalty(id);
        }

        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 58600)]
    internal void AssertBoundaryZhangFeiPenalty(ProgramSkillFrame frame)
    {
        if (frame.BoundaryLiyongPenalty is not null && !ValidBoundaryZhangFeiLiyongPenalty(frame))
            throw new InvalidOperationException("Liyong penalty lost original Use, actual damage or once-paid HP proof.");
    }
}
