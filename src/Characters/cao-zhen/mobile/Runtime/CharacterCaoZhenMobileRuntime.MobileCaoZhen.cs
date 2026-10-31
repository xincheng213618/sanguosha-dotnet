#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhenMobileRuntime
{
    SkillProgramStepOutcome IMobileCaoZhenSidiHost.Consume(ProgramSkillFrame f, string bind) => ConsumeMobileCaoZhenSidi(f, bind);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 74010)]
    internal bool MobileCaoZhenDodgeQualification(ProgramTriggerLegalityContext gate) => gate.SkillId != "mobile:sidi" || gate.Trigger.Id != "store-after-actual-dodge" || gate.Context.CardUse is { } action && (action.ActorSeat == gate.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat == gate.OwnerSeat);
    internal SkillProgramStepOutcome ConsumeMobileCaoZhenSidi(ProgramSkillFrame supplied, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "mobile:sidi" || frame.InstructionIndex != 2 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor, TargetSeat: { } target } context || actor != target || actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || actor == frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _resolutionStackOperations._resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId)is not { } parent || parent.OwnerSeat != actor || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate))
            throw new InvalidOperationException("Mobile Sidi must pay on its original other-actor actual Play starting candidate.");
        var cards = _coreProgramLifecycleOperations.GetProgramCardSet(frame, bind);
        if (cards.CardIds is not [var card] || cards.SourceLocations is not [var from] || from != CardLocation.Authority(frame.OwnerSeat) || _cardZonesOperations._cardZones.GetLocation(card) != from || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileCaoZhenSidiAppliedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Mobile Sidi requires one still-owned real Qian and a once-only phase grant.");
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _ = _coreProgramLifecycleOperations.MoveProgramBoundCards(frame.Id, frame.OwnerSeat, bind, null, SkillProgramCardDestination.DiscardPile, null, CharacterCaoZhenMobileDefinitions.MobileCaoZhenSidiCost);
        var paid = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card && m.From == from && m.To == CardLocation.DiscardPile && m.Reason == CharacterCaoZhenMobileDefinitions.MobileCaoZhenSidiCost);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileCaoZhenSidiAppliedEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, actor, card, paid.Sequence));
        return SkillProgramStepOutcome.Continue;
    }

    internal IEnumerable<RuleQueryContribution> MobileCaoZhenSidiContributions(CharacterState player, SkillRuleQuery query) => query != SkillRuleQuery.SlashLimit || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != player.Seat ? [] : _coreProgramEventHistoryOperations.ProgramEventHistory<MobileCaoZhenSidiAppliedEvent>().Where(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.PlayPhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId && e.AffectedSeat == player.Seat).Select(e => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-sidi:{(e.ProgramFrameId)}", SkillRuleOperation.Add, -1));
}
