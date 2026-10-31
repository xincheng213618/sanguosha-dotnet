#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinPiOlRuntime
{
    internal const string ChijieClaimReason = "skill-program.chijie.claim", YinjuDrawReason = "skill-program.yinju.draw";
    internal bool XinPiSourceCurrent(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash;
    internal SkillProgramStepOutcome BeginXinPi(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XinPi is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("XinPi requires its unpaid exact first instruction.");
        var source = XinPiSource(f);
        if (e.Op == SkillProgramEffectOp.ArmYinjuActualTurnRelation)
        {
            if (!XinPiSourceCurrent(f) || f.TriggerId is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].YinjuRelation is not null)
                throw new InvalidOperationException("Yinju requires its one real own-Play limited activation.");
            var relation = new YinjuTurnRelation(source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, target, f.Id);
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].YinjuRelation = relation;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinjuRelationArmedEvent(relation));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.WindowContext is not { } w || f.TriggerId is not { } binding)
            throw new InvalidOperationException("XinPi trigger lost its native context.");
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, binding, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority);
        if (e.Op == SkillProgramEffectOp.InvalidateRemainingActualDamageCardEffects)
        {
            if (!XinPiSourceCurrent(f) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ChijieUsedActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || ChijieDamageUse(f.OwnerSeat, w, out var damage)is not { Action: { } action } use || IsXinPiUseIneffective(use))
                throw new InvalidOperationException("Chijie requires real already-applied other-source card damage and its shared turn allowance.");
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ChijieUsedActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChijieUsedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, "invalidate", use.Id, action.ActionId));
            var proof = new ChijieInvalidationProof(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, use.Id, action.ActionId, damage.DamageFrameId, f.OwnerSeat, damage.SourceSeat);
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { ChijieInvalidation = proof });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChijieUseInvalidatedEvent(proof));
            foreach (var seat in XinPiMainTargets(use))
                _coreProgramCardTargetEffectsOperations.MarkCardEffectIneffective(use.Id, seat);
            return SkillProgramStepOutcome.Continue;
        }

        if (e.Op == SkillProgramEffectOp.ClaimCompletedUndamagedIncomingCardEntities)
        {
            if (!XinPiSourceCurrent(f) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ChijieUsedActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !ChijieUndamagedIncoming(f.OwnerSeat, w, out var use))
                throw new InvalidOperationException("Chijie claim requires the entire actual incoming use without any native damage.");
            var ids = ChijieClaimable(use);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            var receipt = new XinPiReceipt(source, f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, w.ParentFrameId, XinPiStage.ClaimChildren, use.Id, use.Action!.ActionId, Before: before, After: before, Requested: ids.Count, Actual: ids.Count)
            {
                Entities = ids,
                From = ids.Select(_cardZonesOperations._cardZones.GetLocation).ToArray()
            };
            _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].ChijieUsedActualTurn = _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { XinPi = receipt, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChijieUsedEvent(f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, "claim", use.Id, use.Action.ActionId));
            var claim = new XinPiClaimProof(f.Id, use.Id, use.Action.ActionId, source, f.GameplayHash, before)
            {
                Entities = ids
            };
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { XinPiClaim = claim });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChijieClaimIssuedEvent(f.Id, use.Id, use.Action.ActionId, source, f.GameplayHash, before, ids.Count));
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(ChijieClaimReason), (batch, moves) =>
            {
                var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var r = active.XinPi!with
                {
                    After = moves[^1].Sequence,
                    BatchId = batch
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(active with { XinPi = r });
                _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { XinPiClaim = claim with { After = r.After, BatchId = batch } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ChijieClaimPaidEvent(f.Id, r.Source, use.Id, use.Action.ActionId, before, r.After, batch, ids.Count));
            });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (e.Op == SkillProgramEffectOp.PreventYinjuDamageAndRecover)
        {
            if (!ExactYinjuBeforeCandidate(candidate, w, out var relation, out var before))
                throw new InvalidOperationException("Yinju prevention lost its issued exact source/recipient attempt.");
            var target = _coreMatchParticipantsPlayersAccess._players[relation.TargetSeat];
            var requested = target.HasHpValue ? Math.Min(before.Amount, Math.Max(0, target.MaxHp - target.Hp)) : 0;
            var attempt = _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId ?? before.ParentFrameId;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { XinPi = new(source, f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, w.ParentFrameId, XinPiStage.RecoveryChildren, DamageAttemptId: attempt, TargetSeat: target.Seat, Requested: requested, Relation: relation) });
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinjuRecoveryPaidEvent(f.Id, relation, before.Id, attempt, f.OwnerSeat, target.Seat, before.Amount, requested, target.Hp));
            if (requested > 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, target.Seat, before.Amount, null, null);
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (!ExactYinjuUseCandidate(candidate, w, out var drawRelation, out var native))
            throw new InvalidOperationException("Yinju target draw lost its exact accepted actual native use/recipient.");
        var rDraw = new XinPiReceipt(source, f.GameplayHash, 1, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, w.ParentFrameId, XinPiStage.OwnerDrawChildren, native.Id, native.Action!.ActionId, TargetSeat: drawRelation.TargetSeat, Relation: drawRelation);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XinPi = rDraw });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YinjuUseDrawStartedEvent(f.Id, drawRelation, native.Id, native.Action.ActionId, w.ParentFrameId));
        PayYinjuDraw(f, f.OwnerSeat);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainXinPi(ProgramSkillFrame f)
    {
        var movement = f.PendingMovementContinuation is not null;
        var continuation = movement ? PostEventContinuation.AwaitedProgramMovement : PostEventContinuation.Program;
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, continuation) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, continuation) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    }

    internal void FinishXinPi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XinPi = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 25100)]
    internal bool ResumeXinPi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XinPi: { } r } f || f.Id != id)
            return false;
        if (DrainXinPi(f))
            return true;
        if (r.Stage == XinPiStage.OwnerDrawChildren && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && r.TargetSeat is { } target && _coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XinPi = r with { Stage = XinPiStage.RecipientDrawChildren }, PendingMovementContinuation = null });
            PayYinjuDraw(f, target);
            return true;
        }

        FinishXinPi(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 25100)]
    internal bool ReturnXinPiMovement(ProgramSkillFrame f) => f.XinPi is not null && ResumeXinPi(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 330)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 330)]
    internal bool CanContinueXinPi(ProgramSkillFrame f)
    {
        if (f.XinPi is null)
            return f.InstructionIndex == 0 && f.TriggerId is { } trigger && f.WindowContext is { } w && f.ActivationId == trigger && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && HasIssuedYinjuCandidate(new(f.OwnerSeat, f.SkillId, trigger, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), w);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IXinPiProgramHost.BeginXinPi(ProgramSkillFrame f, SkillProgramEffect e) => BeginXinPi(f, e);
}
