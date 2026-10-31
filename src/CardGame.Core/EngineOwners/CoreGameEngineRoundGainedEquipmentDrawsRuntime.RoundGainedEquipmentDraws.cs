#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundGainedEquipmentDrawsRuntime
{
    internal const string RoundGainedEquipmentDrawReason = "skill-program.round-gained.equipment-draw";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 650)]
    internal bool CanOfferRoundGainedEquipmentDraw(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawForRoundGainedEquipmentUse) || _coreRoundGainedSourceUsesOperations.ExactIssuedRoundGainedUseCandidate(candidate, context)is { } q && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedEquipmentDrawIssuedEvent>().Any(e => _coreRoundGainedSourceUsesOperations.SameRoundGainedQualification(e.Qualification, q));
    internal RoundGainedUseQualification? ExactRoundGainedEquipmentDrawParent(ProgramSkillFrame frame)
    {
        if (frame.InstructionIndex != 1 || frame.TriggerId is null || frame.ActivationId != frame.TriggerId || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted } context || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0)
            return null;
        var q = _coreRoundGainedSourceUsesOperations.ExactIssuedRoundGainedUseCandidate(new(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0), context, frame.RoundGainedEquipmentDrawReceipt is not null);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        return q is not null && index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == context.ParentFrameId && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == frame.Id && e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.SkillInstanceId == frame.SkillInstanceId && e.Window == context.Window) == 1 ? q : null;
    }

    internal SkillProgramStepOutcome BeginRoundGainedEquipmentDraw(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var q = ExactRoundGainedEquipmentDrawParent(frame) ?? throw new InvalidOperationException("A round-gained equipment draw requires its exact qualified committed Use.");
        if (frame.RoundGainedEquipmentDrawReceipt is not null || frame.PendingMovementContinuation is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RoundGainedEquipmentDrawIssuedEvent>().Any(e => _coreRoundGainedSourceUsesOperations.SameRoundGainedQualification(e.Qualification, q)) || _coreRoundGainedSourceUsesOperations.RoundGainedCurrentBoundary()is not { } boundary)
            throw new InvalidOperationException("A round-gained equipment Use cannot issue its draw twice or outside a real round.");
        var before = _coreRoundGainedSourceUsesOperations.RoundGainedMovementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RoundGainedEquipmentDrawReceipt = new(1, q, 0, before, before) { DrawRoundNumber = boundary.RoundNumber }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], 1, true, new(RoundGainedEquipmentDrawReason));
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = frame.RoundGainedEquipmentDrawReceipt!with
        {
            DrawActual = drawn.Count,
            DrawnCardIds = drawn,
            MovementSequenceAfter = _coreRoundGainedSourceUsesOperations.RoundGainedMovementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RoundGainedEquipmentDrawReceipt = receipt });
        // Exclusion is durable before a native gain observer can use, give away,
        // or return this actual drawn entity. It is keyed by policy, not instance.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundGainedEquipmentDrawIssuedEvent(frame.Id, new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), q, receipt.DrawActual, before, receipt.MovementSequenceAfter, receipt.DrawnCardIds) { DrawRoundNumber = receipt.DrawRoundNumber });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 200)]
    internal bool ResumeRoundGainedEquipmentDraw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.RoundGainedEquipmentDrawReceipt is null)
            return false;
        if (_corePublicPilePreparationOperations.PreparationGameEnded())
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frame.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(frame.Id))
            return true;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var receipt = frame.RoundGainedEquipmentDrawReceipt!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundGainedEquipmentDrawResolvedEvent(id, new(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId), receipt.Qualification, receipt.DrawActual));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { RoundGainedEquipmentDrawReceipt = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 220)]
    internal bool ReturnRoundGainedEquipmentDrawMovement(ProgramSkillFrame frame)
    {
        if (frame.RoundGainedEquipmentDrawReceipt is null || frame.PendingMovementContinuation is null)
            return false;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal bool ValidRoundGainedEquipmentDraw(ProgramSkillFrame frame)
    {
        if (frame.RoundGainedEquipmentDrawReceipt is not { } receipt || ExactRoundGainedEquipmentDrawParent(frame)is not { } q || !_coreRoundGainedSourceUsesOperations.SameRoundGainedQualification(q, receipt.Qualification) || receipt.InstructionIndex != frame.InstructionIndex || receipt.InstructionIndex != 1 || receipt.DrawActual is < 0 or > 1 || receipt.DrawRoundNumber <= 0 || receipt.MovementSequenceBefore < 0 || receipt.MovementSequenceAfter < receipt.MovementSequenceBefore || receipt.MovementSequenceAfter > _coreRoundGainedSourceUsesOperations.RoundGainedMovementSequence || receipt.DrawnCardIds.Count != receipt.DrawActual || receipt.DrawnCardIds.Distinct().Count() != receipt.DrawActual || frame.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != frame.OwnerSeat)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var source = new CardConversionSource(frame.SkillId, frame.TriggerId!, frame.OwnerSeat, frame.SkillInstanceId);
        if (history.OfType<RoundGainedEquipmentDrawIssuedEvent>().Where(e => _coreRoundGainedSourceUsesOperations.SameRoundGainedQualification(e.Qualification, q)).ToArray()is not [var issued] || issued.ProgramFrameId != frame.Id || issued.Source != source || issued.DrawActual != receipt.DrawActual || issued.DrawRoundNumber != receipt.DrawRoundNumber || issued.MovementSequenceBefore != receipt.MovementSequenceBefore || issued.MovementSequenceAfter != receipt.MovementSequenceAfter || !issued.DrawnCardIds.SequenceEqual(receipt.DrawnCardIds) || history.OfType<RoundGainedEquipmentDrawResolvedEvent>().Any(e => _coreRoundGainedSourceUsesOperations.SameRoundGainedQualification(e.Qualification, q)) || history.OfType<RoundGainedRoundBoundaryEvent>().Count(e => e.RoundNumber == receipt.DrawRoundNumber && e.MovementSequence <= receipt.MovementSequenceBefore) != 1)
            return false;
        var invoice = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.MovementSequenceBefore && m.Sequence <= receipt.MovementSequenceAfter && m.Reason.Value == RoundGainedEquipmentDrawReason).ToArray();
        return invoice.Select(m => m.CardId).SequenceEqual(receipt.DrawnCardIds) && invoice.All(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat)) && (receipt.DrawActual != 0 || receipt.MovementSequenceBefore == receipt.MovementSequenceAfter);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 130)]
    internal bool IsRoundGainedEquipmentDrawAwaitedMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => frame.PendingMovementContinuation == pending && effect?.Op == SkillProgramEffectOp.DrawForRoundGainedEquipmentUse && ValidRoundGainedEquipmentDraw(frame);
}
