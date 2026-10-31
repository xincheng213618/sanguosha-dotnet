#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraDrawDebtObserversRuntime
{
    internal ProgramSkillFrame? ExtraDrawDebtObserverRoot()
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index - 1] is not ProgramLifecycleTriggerWindowFrame parent || parent.Window != SkillProgramTriggerWindow.DrawPhaseStarting || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || root.ExtraDrawReceipt is not { } paid || root.InstructionIndex != 1 || paid.InstructionIndex != 0 || paid.DrawWindowFrameId != parent.Id || paid.ActualTurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || paid.TurnOwnerSeat != root.OwnerSeat || parent.OwnerSeat != root.OwnerSeat || root.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || context.ParentFrameId != parent.Id || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, parent.Candidates[parent.CandidateIndex]) || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat)
                continue;
            var program = _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program;
            if (program is null || program.GameplayHash != root.GameplayHash || ProgramInstructionResolver.Default.Resolve(root, program).Instructions is not [{ Op: SkillProgramEffectOp.DrawExtraAndArmTurnDamageUseDebt } op] || op.StateId != paid.StateId)
                continue;
            _coreTurnDrawDebtsOperations.AssertTurnDrawDebtReceipts(root, [op]);
            if (_resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame moved || moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0 || moved.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.Sequence <= paid.MovementSequenceBefore || m.Sequence > paid.MovementSequenceAfter || m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(root.OwnerSeat) || m.Reason.Value != "program.extra-draw-debt.draw"))
                continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying))
                    break;
            }

            if (aligned)
                return root;
        }

        return null;
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 440)]
    internal bool IsExtraDrawDebtProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { ResumesProgramSkill: true } dying && ExtraDrawDebtObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
