#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheGuanpingMoyanRuntime
{
    internal ProgramSkillFrame? GuanpingMoyanPaidLongyinRoot()
    {
        for (var index = 4; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame { SkillId: "shanhe:companion-moyan", TriggerId: "outside-loss-judgment" } moyan || moyan.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } frozen } context || context.OwnerSeat != moyan.OwnerSeat || context.Facts?.OwnerIsTurnPlayer != false || _resolutionStackOperations._resolutionStack[index - 1] is not CardsMovedTriggerWindowFrame moved || moved.Id != context.ParentFrameId || moved.Batch.Id != frozen.Id || !moved.Batch.Movements.SequenceEqual(frozen.Movements) || moved.CandidateIndex < 0 || moved.CandidateIndex >= moved.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(moyan, moved.Candidates[moved.CandidateIndex]) || _resolutionStackOperations._resolutionStack[index - 2] is not ProgramSkillFrame { SkillId: "classic:longyin", TriggerId: "committed-slash", InstructionIndex: 1, AttackAttempt: null } paid || paid.OwnerSeat != moyan.OwnerSeat || paid.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } use } paidContext || _resolutionStackOperations._resolutionStack[index - 3] is not ProgramCardTriggerWindowFrame window || window.Id != paidContext.ParentFrameId || window.ParentFrameId != use.ParentCardUseFrameId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(paid, CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || _resolutionStackOperations._resolutionStack[index - 4] is not CardUseFrame original || original.Id != window.ParentFrameId || original.Action?.ActionId != use.CardActionId || window.Action.ActionId != use.CardActionId || window.Action.Type != CardActionType.Use || !CoreNativeCardClassificationsRuntime.IsSlashCard(use.EffectiveKind) || window.Action.EffectiveKind != use.EffectiveKind || window.Action.ActorSeat != use.ActorSeat || _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId != original.Id)
                continue;
            var batch = moved.Batch;
            if (moved.ResumeProgramFrameId != paid.Id || batch.ParentFrameId != paid.Id || batch.AwaitingProgramFrameId is not null || batch.OriginSkillId != paid.SkillId || batch.OriginSkillInstanceId != paid.SkillInstanceId || batch.OriginOwnerSeat != paid.OwnerSeat || batch.Movements is not [var payment] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(payment) || payment.From.OwnerSeat != paid.OwnerSeat || payment.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || payment.To != _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(payment.CardId), payment.From, CardLocation.DiscardPile) || payment.Reason.Value != "skill-program.classic:longyin.SelectAndMoveOwnedCard" || batch.MovementTiming is not { Phase: TurnPhase.Play } timing || timing.ActualTurnOwnerSeat != use.ActorSeat || timing.PhaseActorSeat != use.ActorSeat || use.ActorSeat == paid.OwnerSeat || paidContext.Facts is not { CardActionActorIsCurrentTurn: true, CardActionPhaseIsPlay: true })
                continue;
            var valid = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying)))
                    break;
            }

            if (valid)
                return moyan;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 535)]
    internal bool IsGuanpingMoyanPaidLongyinDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && GuanpingMoyanPaidLongyinRoot()is { AttackAttempt: not null } moyan && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == moyan.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 575)]
    internal bool TryAdvanceGuanpingMoyanPaidLongyinSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || GuanpingMoyanPaidLongyinRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false);
    }
}
