#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiFuRenOlRuntime
{
    internal bool HasOrdinaryCaiFuRenCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveOrdinaryCaiFuRenXianzhou) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveOrdinaryCaiFuRenQieting);

    internal static CardConversionSource OrdinaryCaiFuRenSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_cai_fu_ren_ol_OrdinaryCaiFuRenXianzhou.OrdinaryCaiFuRenSource(f);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 27595, SkillProgramEffectOp.ResolveOrdinaryCaiFuRenXianzhou)]
    internal bool CanActivateOrdinaryCaiFuRenXianzhou(ProgramActivationLegalityContext c) => c.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreEquipmentDonationProgramsOperations.HasPayableAllEquipmentDonation(c.OwnerSeat, c.Program.Id, "ordinary-xianzhou");
    internal bool IsIssuedOrdinaryCaiFuRenXianzhou(ProgramSkillFrame f)
    {
        if (f.SkillId != "ol:xianzhou" || f.ActivationId != "all-equipment-donation" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.EquipmentDonation is not { } r || r.InstructionIndex != 0 || r.Source != OrdinaryCaiFuRenSource(f) || r.GameplayHash != f.GameplayHash || r.UsageId != "ordinary-xianzhou" || r.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || f.SelectedTargetSeats is not [var recipient] || recipient != r.RecipientSeat || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || r.PaidCardIds.Count == 0 || r.PaidCardIds.Distinct().Count() != r.PaidCardIds.Count || r.ActualDeliveredCount < 0 || r.ActualDeliveredCount > r.PaidCardIds.Count || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program is not { } p || p.GameplayHash != f.GameplayHash || p.Activations is not [{ Id: "all-equipment-donation", Effects: [{ Op: SkillProgramEffectOp.ResolveOrdinaryCaiFuRenXianzhou, StateId: "ordinary-xianzhou" }] }] || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime.EquipmentDonationUsage(r.UsageId), SkillUsageScope.Game) != 1)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentDonationPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray()is not [var paid] || paid != new EquipmentDonationPaidEvent(f.Id, r.Source, r.GameplayHash, r.UsageId, r.TurnNumber, r.TurnOwnerSeat, recipient, r.PaidCardIds.Count, r.ActualDeliveredCount, r.SequenceBefore, r.SequenceAfter))
            return false;
        var entities = _coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentDonationEntityPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        return entities.Length == r.PaidCardIds.Count && entities.Select(e => e.CardId).SequenceEqual(r.PaidCardIds) && entities.Count(e => e.Delivered) == r.ActualDeliveredCount && entities.Select((e, i) => e.PaidIndex == i && e.Source == r.Source && e.RecipientSeat == recipient && e.MovementSequence > r.SequenceBefore && e.MovementSequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.MovementSequence && m.CardId == e.CardId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(m.CardKind) && m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == "program.all-equipment-donation.give") == 1 && e.Delivered == (_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.CardId == e.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == "program.all-equipment-donation.give") == 1)).All(v => v);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27595)]
    internal bool ReturnPaidOrdinaryXianzhouMovement(ProgramSkillFrame f)
    {
        if (f.SkillId != "ol:xianzhou" || f.EquipmentDonation is not { Stage: EquipmentDonationStage.PaidMovement })
            return false;
        if (!IsIssuedOrdinaryCaiFuRenXianzhou(f) || !_coreEquipmentDonationObserversOperations.IsValidAllEquipmentDonation(f) || f.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != f.OwnerSeat)
            throw new InvalidOperationException("Ordinary Xianzhou movement must return its exact already-paid donation.");
        // The generic movement return cancels dead holders. This paid operation
        // instead drains the remaining native payment children and lets the
        // recipient-owned benefit cursor decide whether a new effect is legal.
        if (_coreEquipmentDonationProgramsOperations.AwaitEquipmentDonationMovements(f.Id) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    SkillProgramStepOutcome global::CardGame.Core.IOrdinaryCaiFuRenHost.Xianzhou(ProgramSkillFrame f, string stateId) => _coreEquipmentDonationProgramsOperations.RunAllEquipmentDonation(f, stateId);
    SkillProgramStepOutcome global::CardGame.Core.IOrdinaryCaiFuRenHost.Qieting(ProgramSkillFrame f) => BeginOrdinaryQieting(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOrdinaryCaiFuRenXianzhou)]
    internal PromptChoice ChooseOrdinaryXianzhouAi(PendingDecision d, ProgramSkillFrame f) => _coreEquipmentDonationProgramsOperations.SelectAiEquipmentDonation(d, f);
}
