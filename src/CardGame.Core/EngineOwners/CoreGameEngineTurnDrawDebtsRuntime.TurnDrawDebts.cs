#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTurnDrawDebtsRuntime
{
    internal bool TracksTurnDrawDebt => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawExtraAndArmTurnDamageUseDebt) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SelectTurnDamageUseDebtPayment);

    internal static bool IsTurnDebtDamageCard(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TurnDrawDebts.IsTurnDebtDamageCard(kind);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 270)]
    internal void ObserveTurnDrawDebtUse(IGameEvent payload)
    {
        if (!TracksTurnDrawDebt || payload is not CardUseDeclaredEvent declared || !IsTurnDebtDamageCard(declared.CardKind))
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == declared.ResolutionId);
        if (use.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0)
            return;
        if (use.Action is { } action)
        {
            if (action.Type != CardActionType.Use)
                return;
            if (action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind)
                throw new InvalidOperationException("The damage-card debt differs from its actual use actor or effective kind.");
        }
        else if (use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !_coreCurrentTurnUseKindsOperations.IsActualTurnLegacyVirtualUse(use))
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnDamageCardUsedEvent>().Any(e => e.CardUseFrameId == use.Id))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnDamageCardUsedEvent(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.SourceSeat, use.Id, use.CardKind));
    }

    internal SkillProgramStepOutcome DrawExtraAndArmTurnDebt(ProgramSkillFrame frame, string stateId)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.ExtraDrawReceipt is not null || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber <= 0 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not ProgramLifecycleTriggerWindowFrame parent || parent.Id != context.ParentFrameId || parent.Window != context.Window || parent.OwnerSeat != frame.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, parent.Candidates[parent.CandidateIndex]))
            throw new InvalidOperationException("Extra-draw debt requires its real current owner draw-phase candidate.");
        var requested = _corePublicFactionCountsOperations.GetPublicLivingFactionCount();
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, requested, null, null, SkillProgramCardSetVisibility.Private, new("program.extra-draw-debt.draw"));
        var after = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? before : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "program.extra-draw-debt.draw");
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { ExtraDrawReceipt = new(current.InstructionIndex - 1, parent.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, stateId, requested, actual, before, after) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramExtraDrawIssuedEvent(frame.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, stateId, requested, actual));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    internal ProgramExtraDrawIssuedEvent? CurrentTurnDrawDebt(int owner, string skill, string instance, string hash, string stateId)
    {
        if (owner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualTurnDamageCardUsedEvent>().Any(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == owner && e.ActorSeat == owner))
            return null;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTurnDrawDebtPaymentStartedEvent>().Any(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.StateId == stateId))
            return null;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramExtraDrawIssuedEvent>().LastOrDefault(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == owner && e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.GameplayHash == hash && e.StateId == stateId && e.ActualCount > 0);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 50)]
    internal bool CanRunTurnDrawDebtPayment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var payment = trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.SelectTurnDamageUseDebtPayment);
        return payment is null || context.Window == SkillProgramTriggerWindow.TurnEnding && CurrentTurnDrawDebt(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash, payment.StateId!)is not null;
    }

    internal SkillProgramStepOutcome SelectTurnDrawDebtPayment(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.TurnDrawDebtPayment is not null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[^2] is not TurnEndingBoundaryFrame ending || ending.Id != context.ParentFrameId || ending.OwnerSeat != frame.OwnerSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex].Candidate is not { } candidate || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate))
            throw new InvalidOperationException("Turn draw-debt payment lost its exact actual ending candidate.");
        var issued = CurrentTurnDrawDebt(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, effect.StateId!);
        if (issued is null)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, effect.ResultBind!, [], SkillProgramCardSetVisibility.Private, []);
            return SkillProgramStepOutcome.Continue;
        }

        // X is evaluated once here, before selecting and before any payment child.
        var x = _corePublicFactionCountsOperations.GetPublicLivingFactionCount();
        var count = Math.Min(x, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment2(frame.OwnerSeat).Count);
        var before = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsAccess._cardMovements[^1].Sequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { TurnDrawDebtPayment = new(frame.InstructionIndex - 1, effect.StateId!, effect.ResultBind!, issued.FrameId, x, count, before) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTurnDrawDebtPaymentStartedEvent(frame.Id, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, frame.OwnerSeat, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), effect.StateId!, issued.FrameId, x, count));
        return _coreProgramOwnedCardsOperations.SelectProgramOwnedCards(frame, frame.OwnerSeat, count, null, [CardZoneKind.Hand, CardZoneKind.Equipment], effect.ResultBind!, 0, 0, [], []);
    }

    internal SkillProgramEffect TurnDrawDebtOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (effect.Op != SkillProgramEffectOp.SelectTurnDamageUseDebtPayment)
            return effect;
        if (frame.TurnDrawDebtPayment is not { } receipt || receipt.InstructionIndex != frame.InstructionIndex - 1 || receipt.StateId != effect.StateId || receipt.ResultBind != effect.ResultBind)
            throw new InvalidOperationException("Debt selection lost its frozen ending amount.");
        return new(SkillProgramEffectOp.SelectOwnedCards, SkillProgramEffectTarget.Owner, receipt.RequiredPaymentCount, effect.Condition, zones: [CardZoneKind.Hand, CardZoneKind.Equipment], resultBind: effect.ResultBind);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 540)]
    internal bool IsExtraDrawDebtMovement(ProgramSkillFrame frame, SkillProgramEffect? paid, ProgramMovementContinuation movement) => paid?.Op == SkillProgramEffectOp.DrawExtraAndArmTurnDamageUseDebt && frame.ExtraDrawReceipt is { } receipt && receipt.InstructionIndex == frame.InstructionIndex - 1 && receipt.StateId == paid.StateId && receipt.TurnOwnerSeat == frame.OwnerSeat && movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    internal void AssertTurnDrawDebtReceipts(ProgramSkillFrame frame, IReadOnlyList<SkillProgramEffect> instructions)
    {
        if (frame.ExtraDrawReceipt is { } drawn)
        {
            if (drawn.InstructionIndex < 0 || drawn.InstructionIndex >= instructions.Count || instructions[drawn.InstructionIndex].Op != SkillProgramEffectOp.DrawExtraAndArmTurnDamageUseDebt || instructions[drawn.InstructionIndex].StateId != drawn.StateId || drawn.InstructionIndex != frame.InstructionIndex - 1 || drawn.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || drawn.TurnOwnerSeat != frame.OwnerSeat || drawn.RequestedCount < 0 || drawn.ActualCount < 0 || drawn.ActualCount > drawn.RequestedCount || drawn.MovementSequenceBefore < 0 || drawn.MovementSequenceAfter < drawn.MovementSequenceBefore || frame.WindowContext?.ParentFrameId != drawn.DrawWindowFrameId || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > drawn.MovementSequenceBefore && m.Sequence <= drawn.MovementSequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason.Value == "program.extra-draw-debt.draw") != drawn.ActualCount || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramExtraDrawIssuedEvent>().Any(e => e.FrameId == frame.Id && e.Source == new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) && e.ActualTurnNumber == drawn.ActualTurnNumber && e.TurnOwnerSeat == drawn.TurnOwnerSeat && e.GameplayHash == frame.GameplayHash && e.RequestedCount == drawn.RequestedCount && e.ActualCount == drawn.ActualCount && e.StateId == drawn.StateId))
                throw new InvalidOperationException("The extra-draw receipt differs from its real atomic draws or source.");
        }

        if (frame.TurnDrawDebtPayment is { } payment && (payment.InstructionIndex < 0 || payment.InstructionIndex >= instructions.Count || instructions[payment.InstructionIndex].Op != SkillProgramEffectOp.SelectTurnDamageUseDebtPayment || instructions[payment.InstructionIndex].StateId != payment.StateId || instructions[payment.InstructionIndex].ResultBind != payment.ResultBind || payment.MovementSequenceBefore < 0 || payment.RequiredPaymentCount < 0 || payment.RequiredPaymentCount > payment.EndingFactionCount || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTurnDrawDebtPaymentStartedEvent>().Any(e => e.FrameId == frame.Id && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.OwnerSeat == frame.OwnerSeat && e.ExtraDrawProgramFrameId == payment.ExtraDrawProgramFrameId && e.EndingFactionCount == payment.EndingFactionCount && e.RequiredPaymentCount == payment.RequiredPaymentCount && e.StateId == payment.StateId && e.Source == new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId)) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramExtraDrawIssuedEvent>().Any(e => e.FrameId == payment.ExtraDrawProgramFrameId && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnOwnerSeat == frame.OwnerSeat && e.StateId == payment.StateId && e.ActualCount > 0 && e.GameplayHash == frame.GameplayHash && e.Source.SkillId == frame.SkillId && e.Source.SkillInstanceId == frame.SkillInstanceId && e.Source.OwnerSeat == frame.OwnerSeat)))
            throw new InvalidOperationException("The ending debt lost its exact source, frozen amount or once-only payment.");
    }

    SkillProgramStepOutcome global::CardGame.Core.ITurnDrawDebtProgramHost.DrawExtraAndArmTurnDamageUseDebt(ProgramSkillFrame f, string stateId) => DrawExtraAndArmTurnDebt(f, stateId);
    SkillProgramStepOutcome global::CardGame.Core.ITurnDrawDebtProgramHost.SelectTurnDamageUseDebtPayment(ProgramSkillFrame f, SkillProgramEffect effect) => SelectTurnDrawDebtPayment(f, effect);
}
