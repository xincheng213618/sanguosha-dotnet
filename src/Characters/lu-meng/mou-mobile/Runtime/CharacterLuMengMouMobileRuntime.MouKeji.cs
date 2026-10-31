#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouMobileRuntime
{
    internal bool MouLuMengOwnDying(int owner) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _resolutionStackOperations._resolutionStack.OfType<DyingFrame>().Any(f => f.VictimSeat == owner && f.Step != ResolutionFrameStep.Completed);
    internal bool IsMouLuMengPeachForbidden(int actor, CardKind kind, CardActionType type) => kind == CardKind.Peach && (type == CardActionType.Use || type == CardActionType.Response && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(_coreMatchParticipantsPlayersAccess._players[actor], kind)) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[actor], MouKeji) && !MouLuMengOwnDying(actor);
    internal bool IsMouLuMengPeachRescueAllowed(int actor) => !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[actor], MouKeji) || MouLuMengOwnDying(actor);
    internal int MouLuMengArmorHandLimit(CharacterState owner) => _coreNationalSkillsOperations.HasRuntimeSkill(owner, MouKeji) ? owner.Armor : 0;
    internal int MouKejiPhaseUses(int owner) => _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner, MouKeji, "discard-hand")) + _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner, MouKeji, "lose-hp"));
    [ProgramPhaseUsedProjection(10)]
    internal int MouLuMengProjectedPhaseUses(int owner, string skill, int ordinary) => skill == MouKeji && MouLuMengAwakened(owner) ? MouKejiPhaseUses(owner) : ordinary;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 29200, SkillProgramEffectOp.ResolveMouLuMengKeji)]
    internal bool CanActivateMouKeji(ProgramActivationLegalityContext c) => c.SkillId == MouKeji && MouLuMengComposition.IsKeji(c.Program) && c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == c.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == c.OwnerSeat && (!MouLuMengAwakened(c.OwnerSeat) || MouKejiPhaseUses(c.OwnerSeat) < 1);
    internal bool MouKejiRoot(ProgramSkillFrame f) => f.SkillId == MouKeji && f.TriggerId is null && f.WindowContext is null && f.ActivationId is "discard-hand" or "lose-hp" && f.SelectedTargetSeats.Count == 0 && (f.ActivationId == "discard-hand" ? f.SelectedCardIds is [> 0] : f.SelectedCardIds.Count == 0) && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is { } p && p.GameplayHash == f.GameplayHash && MouLuMengComposition.IsKeji(p);
    internal bool ValidMouKeji(ProgramSkillFrame f) => MouKejiRoot(f) && f.MouLuMengKeji is { } r && r.Source == MouLuMengSource(f) && r.GameplayHash == f.GameplayHash && r.Phase.ActorSeat == f.OwnerSeat && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && r.SelectedCardId == (f.SelectedCardIds.Count == 1 ? f.SelectedCardIds[0] : (int? )null) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengKejiReservedEvent>().Count(e => e == new MouLuMengKejiReservedEvent(f.Id, r.Source, r.GameplayHash, r.Phase)) == 1 && (!r.CostReturned || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuMengKejiCostReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var cost] && cost.Branch == f.ActivationId && cost.Paid == r.Paid);
    internal bool MouKejiCostWasPaid(ProgramSkillFrame f)
    {
        if (!ValidMouKeji(f))
            return false;
        var r = f.MouLuMengKeji!;
        if (f.ActivationId == "lose-hp")
            return _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1) == 1;
        return f.SelectedCardPayment is { InstructionIndex: 2, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true } payment && payment.RecipientSeat == f.OwnerSeat && payment.CardIds.SequenceEqual(f.SelectedCardIds) && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Count(e => e.ParentFrameId == f.Id && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat && e.SubjectSeat == f.OwnerSeat && e.ActualCount == 1 && e.HandCount == 1 && e.SequenceBefore >= r.MovementBefore && e.TurnNumber == r.Phase.TurnNumber && e.Reason.Value == $"skill-program.{(f.SkillId)}.{(SkillProgramEffectOp.DiscardSelected)}") == 1;
    }

    internal SkillProgramStepOutcome ResolveMouKeji(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MouKejiRoot(f))
            throw new InvalidOperationException("Keji needs its exact complete native Hand-discard or HP-loss activation.");
        if (f.InstructionIndex == 1)
        {
            var phase = MouLuMengPhase();
            if (f.MouLuMengKeji is not null || phase.Kind != ActualDiscardRecoveryPhaseKind.Play || phase.ActorSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || MouLuMengAwakened(f.OwnerSeat) && MouKejiPhaseUses(f.OwnerSeat) != 1 || f.SelectedCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(f.OwnerSeat)))
                throw new InvalidOperationException("Keji cannot reuse its phase allowance or replace its selected owned Hand material.");
            var r = new MouLuMengKejiReceipt(MouLuMengSource(f), f.GameplayHash, phase, f.SelectedCardIds.Count == 1 ? f.SelectedCardIds[0] : null, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuMengKeji = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengKejiReservedEvent(f.Id, r.Source, r.GameplayHash, phase));
            return SkillProgramStepOutcome.Continue;
        }

        if (f.InstructionIndex != 3 || !ValidMouKeji(f) || f.MouLuMengKeji is not { CostReturned: false } receipt || f.SelectedCardPayment is not null && f.SelectedCardPaymentResult is not { Completed: true })
            throw new InvalidOperationException("Keji's armor must follow the returned actual native cost and its children.");
        var paid = MouKejiCostWasPaid(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuMengKeji = receipt with { CostReturned = true, Paid = paid } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuMengKejiCostReturnedEvent(f.Id, f.ActivationId, paid));
        if (paid)
            return SkillProgramStepOutcome.Continue;
        _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(f, "克己未实际支付本次弃牌或体力流失，不获得护甲。");
        return SkillProgramStepOutcome.AwaitChild;
    }

    // The original selected-card movement owns payment and its returned child
    // cursor. Losing the source after that payment cannot cancel the armor tail.
    [ProgramContinuation(ProgramContinuationKind.Movement, 29200)]
    internal bool ReturnMouKejiDiscard(ProgramSkillFrame f)
    {
        if (f.MouLuMengKeji is null || f.SelectedCardPaymentResult is not null)
            return false;
        if (!ValidMouKeji(f) || f.ActivationId != "discard-hand" || f.InstructionIndex != 2 || f.SelectedCardPayment is not { Operation: SkillProgramEffectOp.DiscardSelected, InstructionIndex: 2, MovementCommitted: true, ActiveChildFrameId: null } payment || f.PendingMovementContinuation is not null || !payment.CardIds.SequenceEqual(f.SelectedCardIds))
            throw new InvalidOperationException("Keji cannot return an uncommitted or unrelated native discard payment.");
        var alive = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SelectedCardPaymentResult = new(payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, alive) });
        if (!alive)
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), "克己的实际弃牌已经完成，阵亡角色不再获得护甲。");
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }
}
