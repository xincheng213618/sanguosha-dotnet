#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEndingPairSlashObserversRuntime
{
    internal bool IsEndingPairMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (e?.Op == SkillProgramEffectOp.DrawEndingPairThenBlockRoundIfUnequal && f.EndingPairDraw is { AwaitingMovement: true } && _coreEndingPairDrawsOperations.Value.ValidEndingPairDraw(f) || e?.Op == SkillProgramEffectOp.RecastSelectedPhysicalSlash && f.PlaySlashRecast is { AwaitingMovement: true } && _corePlaySlashRecastsOperations.Value.ValidPlaySlashRecast(f));
    [ProgramContinuation(ProgramContinuationKind.MovementTail, 70)]
    internal bool ReturnEndingPairMovement(ProgramSkillFrame f)
    {
        if (f.PendingMovementContinuation is not { } pending || f.EndingPairDraw is null && f.PlaySlashRecast is null)
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
        if (!IsEndingPairMovement(f, effect, pending))
            throw new InvalidOperationException("The actual Ending/recast movement lost its exact issued paid instruction.");
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    internal bool EndingPairPaidFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (child is not CardsMovedTriggerWindowFrame moved || moved.Batch.ParentFrameId != f.Id || moved.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0)
            return false;
        long before, after;
        CardLocation from, to;
        CardMoveReason reason;
        int? id = null;
        if (f.EndingPairDraw is { AwaitingMovement: true } pair && _coreEndingPairDrawsOperations.Value.ValidEndingPairDraw(f))
        {
            var draw = pair.Cursor == 0 ? pair.FirstDraw : pair.SecondDraw;
            if (draw is null || draw.ActualCount != 1)
                return false;
            before = draw.SequenceBefore;
            after = draw.SequenceAfter;
            from = CardLocation.DrawPile;
            to = CardLocation.Hand(draw.RecipientSeat);
            reason = new($"skill-program.{(f.SkillId)}.ending-pair.{(pair.Cursor)}");
        }
        else if (f.PlaySlashRecast is { AwaitingMovement: true } recast && _corePlaySlashRecastsOperations.Value.ValidPlaySlashRecast(f))
        {
            if (recast.DrawIssued)
            {
                before = recast.DrawSequenceBefore;
                after = recast.DrawSequenceAfter;
                from = CardLocation.DrawPile;
                to = CardLocation.Hand(f.OwnerSeat);
                reason = CardMoveReasons.RecastDraw;
            }
            else
            {
                before = recast.SequenceBefore;
                after = recast.SequenceAfter;
                from = CardLocation.Hand(f.OwnerSeat);
                to = CardLocation.DiscardPile;
                reason = CardMoveReasons.RecastDiscard;
                id = recast.CardId;
            }
        }
        else
            return false;
        return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after && m.From == from && m.To == to && m.Reason == reason && (id is null || m.CardId == id));
    }

    internal ProgramSkillFrame? EndingPairPaidObserverRoot()
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame f || !EndingPairPaidFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]))
                continue;
            var valid = true;
            for (var n = i + 1; n < _resolutionStackOperations._resolutionStack.Count; n++)
            {
                // Local union only after the original real cost/draw invoice and
                // first incoming movement have been proved. No observer presence gate.
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageDyingFaceEdge(n) && !_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(n) && !(_resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame p && _resolutionStackOperations._resolutionStack[n] is DyingFrame { Continuation: DyingContinuationKind.AttackHpLoss } loss && _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverAttackHpLossDyingMatches(p, loss)) && !(_resolutionStackOperations._resolutionStack[n - 1] is DyingFrame dying && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame response && _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageDyingProgramMatches(response, dying)))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[n] is DyingFrame d && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(n, d) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(n, d) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(n, d) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(n, d)))
                    break;
            }

            if (valid)
                return f;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 640)]
    internal bool IsEndingPairSlashProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && EndingPairPaidObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
