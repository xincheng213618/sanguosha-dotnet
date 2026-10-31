#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5ZhangxiuRuntime
{
    internal const string ShanheS5XiongluanId = "shanhe:s5-xiongluan-14116";
    internal const string ShanheXiongluanJudgmentReason = "shanhe.s5.xiongluan.abolish-judgment";
    SkillProgramStepOutcome global::CardGame.Core.IShanheS5ZhangxiuHost.PayShanheS5Xiongluan(ProgramSkillFrame f) => PayShanheS5Xiongluan(f);
    void global::CardGame.Core.IShanheS5ZhangxiuHost.ResolveShanheLeizhenUse(ProgramSkillFrame f) => _coreShanheLeizhenOperations.ResolveShanheLeizhenUse(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 59401)]
    internal bool CanPayShanheS5Xiongluan(CharacterState owner, SkillProgramActivation activation) => !activation.Effects.Any(e => e.Op == SkillProgramEffectOp.PayShanheS5Xiongluan) || owner.IsAlive && Enum.GetValues<EquipmentSlot>().Any(slot => owner.EquipmentSlotCapacity(slot) > 0);
    internal SkillProgramStepOutcome PayShanheS5Xiongluan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.SkillId != ShanheS5XiongluanId || f.TriggerId is not null || f.InstructionIndex != 1 || f.ShanheS5Xiongluan is not null || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !owner.IsAlive || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            throw new InvalidOperationException("S5 Xiongluan needs one real other target in its original own Play activation.");
        var slots = Enum.GetValues<EquipmentSlot>().Where(slot => owner.EquipmentSlotCapacity(slot) > 0).Order().ToArray();
        if (slots.Length == 0)
            throw new InvalidOperationException("S5 Xiongluan requires a remaining actual equipment slot.");
        var beforeRandom = _randomOperations._random.State;
        var chosen = slots[_randomOperations._random.Next(slots.Length)];
        var afterRandom = _randomOperations._random.State;
        var receipt = new ShanheS5XiongluanPayment(new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, target, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, chosen, slots.Length, beforeRandom, afterRandom, owner.JudgmentAreaAbolished, _coreCardMovementJournalMovementSequenceAccess._movementSequence, _coreCardMovementJournalMovementSequenceAccess._movementSequence, false);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Xiongluan = receipt, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (!owner.JudgmentAreaAbolished)
            _coreProgramAbolishAreasOperationOperations.AbolishProgramOwnerAreas(f, [CardZoneKind.Judgment]);
        _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(owner, chosen, 0);
        _coreProgramLifecycleOperations.DiscardProgramOwnedZoneCards(f, [CardZoneKind.Judgment], new(ShanheXiongluanJudgmentReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        receipt = receipt with
        {
            Paid = true,
            MovementAfter = _coreCardMovementJournalMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheS5Xiongluan = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheS5XiongluanCostPaidEvent(f.Id, receipt.Source, receipt.GameplayHash, target, receipt.ActualTurnNumber, receipt.PhaseInstanceId, chosen, slots.Length, receipt.JudgmentWasAbolished, receipt.MovementBefore, receipt.MovementAfter));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 59401)]
    internal bool ResumeShanheS5Xiongluan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheS5Xiongluan: { Paid: true, ChildrenCompleted: false } r } f || f.Id != id)
            return false;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheS5Xiongluan = r with { ChildrenCompleted = true }, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 59401)]
    internal bool ReturnShanheS5XiongluanMovement(ProgramSkillFrame f) => f.ShanheS5Xiongluan is { ChildrenCompleted: false } && ResumeShanheS5Xiongluan(f.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 59401)]
    internal bool IsShanheS5XiongluanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (effect?.Op != SkillProgramEffectOp.PayShanheS5Xiongluan || f.ShanheS5Xiongluan is not { Paid: true, ChildrenCompleted: false } || pending.SubjectSeat != f.OwnerSeat || pending.BeforeCount != 0 || pending.CoverageResultBind is not null)
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 59401)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 59401)]
    internal bool ContinuePaidShanheS5Xiongluan(ProgramSkillFrame f)
    {
        if (f.ShanheS5Xiongluan is not { Paid: true })
            return false;
        return true;
    }
}
