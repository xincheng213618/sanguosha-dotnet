#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoMouOlRuntime
{
    internal bool ValidMouDongZhao(ProgramSkillFrame f)
    {
        if (f.MouDongZhao is not { } r || r.Source != MouDongZhaoSource(f) || r.GameplayHash != f.GameplayHash || !MouDongZhaoComposition.Handles(r.Operation) || f.InstructionIndex != 1 || r.Before < 0 || r.After < r.Before)
            return false;
        var signed = r.Operation switch
        {
            SkillProgramEffectOp.ResolveMouDongZhaoShunji => f.TriggerId == "damage-draw-and-gift" && f.ActivationId == f.TriggerId && f.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target } c && target == r.TargetSeat && c.ParentFrameId == r.WindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoShunjiAcceptedEvent>().Count(e => e == new MouDongZhaoShunjiAcceptedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.RoundNumber, r.TargetSeat)) == 1,
            SkillProgramEffectOp.ResolveMouDongZhaoYishi => f.TriggerId is null && f.ActivationId == "keep-suits-and-move" && r.Phase is { } phase && phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoYishiStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Phase == phase && e.HandCount == r.Hand.Count) == 1,
            _ => f.TriggerId == "follow-suit-loss" && f.ActivationId == f.TriggerId && f.WindowContext is { MovementBatch: { } batch } context && context.ParentFrameId == r.WindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoYishiLossDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.GrantFrameId == r.GrantFrameId && e.BatchId == batch.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash) == 1
        };
        return signed && (r.Draw is null || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoDrawReturnedEvent>().Count(e => e == new MouDongZhaoDrawReturnedEvent(f.Id, r.Draw.RequestedCount, r.Draw.ActualDrawIds.Count, r.Draw.ReplacementGainedIds.Count, r.Draw.SequenceBefore, r.Draw.SequenceAfter, r.Draw.AttemptFrameId)) == 1);
    }

    internal void MutateMouDongZhao(ProgramSkillFrame f, MouDongZhaoStage stage, Action mutation)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = f.MouDongZhao!with { Stage = stage, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        mutation();
        SealMouDongZhaoMovements(f.Id, before);
        ContinueMouDongZhao(f.Id);
    }

    internal void SealMouDongZhaoMovements(long frameId, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDongZhao = f.MouDongZhao!with { After = after } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after))
        {
            var fact = new MouDongZhaoMovementSignedEvent(frameId, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouDongZhao(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMouDongZhao(long frameId)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDongZhao: { } r } f || f.Id != frameId)
            return;
        if (!ValidMouDongZhao(f))
            throw new InvalidOperationException("OL Dong Zhao lost its original accepted source and native receipt.");
        if (r.Stage == MouDongZhaoStage.Drawing)
            return;
        if (r.Stage == MouDongZhaoStage.Damage)
        {
            // Native ProgramAttack returns here after its damage, Dying and
            // after-damage children; its original parent cursor is restored by
            // CompleteProgramAttack before this owning continuation runs.
            if (f.AttackAttempt is not null)
            {
                _coreNativeDamageCompletionOperations.CompleteDamageAttack(_kernelCoreGameEngineProgramAttackAttemptOperations.CreateProgramAttackHandle(f.Id));
                return;
            }

            if (f.AttackReturn is not null || !ExactMouShunjiWindow(f))
                throw new InvalidOperationException("Shunji lost its exact native damage return and original after-damage window.");
            CompleteMouDongZhao(f);
            return;
        }

        if (r.Stage is MouDongZhaoStage.DrawChildren or MouDongZhaoStage.GiftChildren or MouDongZhaoStage.DiscardChildren or MouDongZhaoStage.MoveChildren)
        {
            if (DrainMouDongZhao(frameId))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
            r = f.MouDongZhao!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            CompleteMouDongZhao(f);
            return;
        }

        if (r.Stage == MouDongZhaoStage.DrawChildren)
        {
            if (r.Operation == SkillProgramEffectOp.DrawMouDongZhaoYishiLoss || r.GiftCount == 0 || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            {
                CompleteMouDongZhao(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Stage = MouDongZhaoStage.ChoosingGift, Selected = [] } });
            PublishMouDongZhao(f);
            return;
        }

        if (r.Stage == MouDongZhaoStage.GiftChildren)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoNameSpentEvent>().Any(e => e.FrameId == f.Id))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Stage = MouDongZhaoStage.Damage } });
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, 1);
                return;
            }

            CompleteMouDongZhao(f);
            return;
        }

        if (r.Stage == MouDongZhaoStage.DiscardChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = r with { Stage = MouDongZhaoStage.ChoosingMove, Selected = [] } });
            PublishMouDongZhao(f);
            return;
        }

        if (r.Stage == MouDongZhaoStage.MoveChildren)
        {
            GrantMouYishi(f);
            CompleteMouDongZhao(f);
            return;
        }

        if (r.Stage == MouDongZhaoStage.Complete)
        {
            CompleteMouDongZhao(f);
            return;
        }

        PublishMouDongZhao(f);
    }

    internal void CompleteMouDongZhao(ProgramSkillFrame f)
    {
        if (f.MouDongZhao is { } r)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoCompletedEvent(f.Id, r.Operation));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouDongZhao = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 33500)]
    internal bool ReturnedMouDongZhaoBinding(ProgramSkillFrame f, bool completed)
    {
        if (completed && f.MouDongZhao is { Stage: MouDongZhaoStage.Damage } r && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhaoCompletedEvent>().Any(e => e.FrameId == f.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouDongZhaoCompletedEvent(f.Id, r.Operation));
        return false;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 33500)]
    internal bool ResumeMouDongZhao(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDongZhao: not null } f || f.Id != frameId)
            return false;
        ContinueMouDongZhao(frameId);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33500)]
    internal bool ReturnMouDongZhaoMovement(ProgramSkillFrame f) => f.MouDongZhao is not null && ResumeMouDongZhao(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33500)]
    internal bool ContinueAcceptedMouDongZhao(ProgramSkillFrame f) => ValidMouDongZhao(f) || f.MouDongZhao is null && f.InstructionIndex is 0 or 1 && f.SkillId == MouDongZhaoComposition.Yishi && f.TriggerId == "follow-suit-loss" && f.ActivationId == f.TriggerId && f.WindowContext is { } context && IsIssuedMouYishiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, context.OccurrenceIndex), context);
}
