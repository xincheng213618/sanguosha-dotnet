#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5QiangxiRuntime
{
    internal SkillProgramStepOutcome ResolveShanheS5Qiangxi(ProgramSkillFrame supplied, string stage)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (stage == "prepare")
        {
            if (f.InstructionIndex != 1 || f.ShanheS5Qiangxi is not null || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != f.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || _coreActualRoundProgressRoundNumberAccess._roundNumber < 1 || _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId <= 0)
                throw new InvalidOperationException("S5 Qiangxi lost its original real Play activation and other living target.");
            var prior = _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5QiangxiPaidEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillId == f.SkillId && e.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
            var weaponId = f.ActivationId == "discard-weapon" ? f.SelectedCardIds.Single() : (int? )null;
            var from = weaponId is { } id ? _cardZonesOperations._cardZones.GetLocation(id) : (CardLocation? )null;
            if (weaponId is { } weapon && (from!.Value.OwnerSeat != f.OwnerSeat || from.Value.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || _coreEquipmentContentOperations.EquipmentRules.Get(_coreCardAttackStateOperations.GetAttackCard(weapon).Kind).Slot != EquipmentSlot.Weapon))
                throw new InvalidOperationException("S5 Qiangxi requires its actual selected owner weapon.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Qiangxi = new(new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, target, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, checked(prior + 1), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Hp, weaponId, from) });
            return SkillProgramStepOutcome.Continue;
        }

        var r = f.ShanheS5Qiangxi ?? throw new InvalidOperationException("S5 Qiangxi lost its frozen activation.");
        if (f.InstructionIndex != 3 || r.Paid || !HasShanheS5QiangxiPayment(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheS5QiangxiPaidEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("S5 Qiangxi requires one completed real cost on its owning frame.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Qiangxi = r with { Paid = true } });
        var damage = checked(r.ActivationCount + 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5QiangxiPaidEvent(f.Id, r.Source, r.GameplayHash, r.TargetSeat, r.RoundNumber, r.TurnNumber, r.PhaseInstanceId, r.ActivationCount, damage));
        return _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive ? _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.TargetSeat, damage) : SkillProgramStepOutcome.Continue;
    }

    internal bool HasShanheS5QiangxiPayment(ProgramSkillFrame f)
    {
        if (f.ShanheS5Qiangxi is not { } r)
            return false;
        if (r.WeaponCardId is not { } id)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == f.OwnerSeat && e.Amount == 1 && e.RemainingHp == r.CostHpBefore - 1) == 1;
        if (f.SelectedCardPayment is not { InstructionIndex: 2, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true } paid || !paid.CardIds.SequenceEqual([id]) || f.SelectedCardPaymentResult is not { InstructionIndex: 2, Operation: SkillProgramEffectOp.DiscardSelected, Completed: true })
            return false;
        var invoices = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == f.Id && e.InitiatorSeat == f.OwnerSeat && e.ExecutorSeat == f.OwnerSeat && e.SubjectSeat == f.OwnerSeat && e.ActualCount == 1 && e.Reason.Value == $"skill-program.{(f.SkillId)}.DiscardSelected").ToArray();
        return invoices is [var invoice] && _coreProgramEventHistoryOperations.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Count(e => e.InvocationId == invoice.InvocationId && e.CardId == id && e.From == r.WeaponFrom && e.CompletionSequence > invoice.SequenceBefore && e.CompletionSequence <= invoice.SequenceAfter) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 59100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59100)]
    internal bool ContinuePaidShanheS5Qiangxi(ProgramSkillFrame f) => HasShanheS5QiangxiPayment(f);

    SkillProgramStepOutcome global::CardGame.Core.IShanheS5QiangxiHost.ResolveShanheS5Qiangxi(ProgramSkillFrame frame, string stage) => ResolveShanheS5Qiangxi(frame, stage);
}
