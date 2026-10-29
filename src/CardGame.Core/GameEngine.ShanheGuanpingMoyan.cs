namespace CardGame.Core;

public sealed partial class GameEngine
{
    // Longyin has already paid before its post-movement child begins. Keep the
    // original committed Slash and the exact physical discard as the authority
    // for suspending that attack; a CardsMoved context alone is insufficient.
    private ProgramSkillFrame? GuanpingMoyanPaidLongyinRoot()
    {
        for (var index = 4; index < _resolutionStack.Count; index++)
        {
            if (_resolutionStack[index] is not ProgramSkillFrame
                { SkillId: "shanhe:companion-moyan", TriggerId: "outside-loss-judgment" } moyan ||
                moyan.WindowContext is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { } frozen } context ||
                context.OwnerSeat != moyan.OwnerSeat || context.Facts?.OwnerIsTurnPlayer != false ||
                _resolutionStack[index - 1] is not CardsMovedTriggerWindowFrame moved ||
                moved.Id != context.ParentFrameId || moved.Batch.Id != frozen.Id ||
                !moved.Batch.Movements.SequenceEqual(frozen.Movements) ||
                moved.CandidateIndex < 0 || moved.CandidateIndex >= moved.Candidates.Count ||
                !MountObserverCandidateMatches(moyan, moved.Candidates[moved.CandidateIndex]) ||
                _resolutionStack[index - 2] is not ProgramSkillFrame
                    { SkillId: "classic:longyin", TriggerId: "committed-slash", InstructionIndex: 1, AttackAttempt: null } paid ||
                paid.OwnerSeat != moyan.OwnerSeat || paid.WindowContext is not
                    { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } use } paidContext ||
                _resolutionStack[index - 3] is not ProgramCardTriggerWindowFrame window ||
                window.Id != paidContext.ParentFrameId || window.ParentFrameId != use.ParentCardUseFrameId ||
                window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
                !MountObserverCandidateMatches(paid, ToSharedCandidate(window.Candidates[window.CandidateIndex])) ||
                _resolutionStack[index - 4] is not CardUseFrame original || original.Id != window.ParentFrameId ||
                original.Action?.ActionId != use.CardActionId || window.Action.ActionId != use.CardActionId ||
                window.Action.Type != CardActionType.Use || !IsSlashCard(use.EffectiveKind) ||
                window.Action.EffectiveKind != use.EffectiveKind || window.Action.ActorSeat != use.ActorSeat ||
                ActiveCardAttack?.ResolutionId != original.Id) continue;
            var batch = moved.Batch;
            if (moved.ResumeProgramFrameId != paid.Id || batch.ParentFrameId != paid.Id || batch.AwaitingProgramFrameId is not null ||
                batch.OriginSkillId != paid.SkillId || batch.OriginSkillInstanceId != paid.SkillInstanceId ||
                batch.OriginOwnerSeat != paid.OwnerSeat || batch.Movements is not [var payment] ||
                !_cardMovements.Contains(payment) || payment.From.OwnerSeat != paid.OwnerSeat ||
                payment.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) ||
                payment.To != NormalizeEphemeralCardDestination(GetAdvancedCard(payment.CardId), payment.From,
                    CardLocation.DiscardPile) || payment.Reason.Value != "skill-program.classic:longyin.SelectAndMoveOwnedCard" ||
                batch.MovementTiming is not { Phase: TurnPhase.Play } timing || timing.ActualTurnOwnerSeat != use.ActorSeat ||
                timing.PhaseActorSeat != use.ActorSeat || use.ActorSeat == paid.OwnerSeat ||
                paidContext.Facts is not { CardActionActorIsCurrentTurn: true, CardActionPhaseIsPlay: true }) continue;
            var valid = true;
            for (var child = index + 1; child < _resolutionStack.Count; child++)
            {
                if (!HalfHandPaidDamageObserverEdge(child)) { valid = false; break; }
                if (_resolutionStack[child] is DyingFrame dying &&
                    (IsPaidHandRepaymentRescueRide(child, dying) || IsPaidHandRepaymentProgramAlcoholRide(child, dying) ||
                     PaidObserverDamageVirtualAlcoholRide(child, dying) || PolicyCounterspellVirtualAlcoholRide(child, dying))) break;
            }
            if (valid) return moyan;
        }
        return null;
    }


    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 2005)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 105)]
    private bool HasGuanpingMoyanPaidLongyinDamageObserver(long window) =>
        _resolutionStack.Any(frame => frame.Id == window && frame is BeforeDamageProgramWindowFrame or DamageTriggerWindowFrame) &&
        GuanpingMoyanPaidLongyinRoot() is { AttackAttempt: not null, AttackReturn: not null };

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 455, group: 10)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 535)]
    private bool IsGuanpingMoyanPaidLongyinDying() => ActiveDying is { } dying &&
        GuanpingMoyanPaidLongyinRoot() is { AttackAttempt: not null } moyan &&
        _resolutionStack.FindIndex(frame => frame.Id == dying.FrameId) > _resolutionStack.FindIndex(frame => frame.Id == moyan.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 575)]
    private bool TryAdvanceGuanpingMoyanPaidLongyinSubtree()
    {
        if (_pendingDecision is not null || GuanpingMoyanPaidLongyinRoot() is null) return false;
        return TryAdvanceObserverSubtree(advanceRecoveryReplacement: false);
    }
}
