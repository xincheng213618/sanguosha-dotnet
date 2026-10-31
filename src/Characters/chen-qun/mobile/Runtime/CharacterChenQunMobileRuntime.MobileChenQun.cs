#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChenQunMobileRuntime
{
    internal bool MobileDingpinCurrentTurn(int turn, int owner) => turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && owner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;
    internal bool MobileDingpinCardAllowed(int owner, Card card)
    {
        var category = CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind);
        if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.ActorSeat == owner && MobileDingpinCurrentTurn(e.TurnNumber, e.TurnOwnerSeat) && (e.CardCategoryMask & (1 << (int)category)) != 0))
            return false;
        var invocations = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => MobileDingpinCurrentTurn(e.TurnNumber, e.ActualTurnOwnerSeat)).Select(e => e.InvocationId).ToHashSet();
        return !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(e => invocations.Contains(e.InvocationId) && e.From.OwnerSeat == owner && CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind) == category);
    }

    internal bool MobileDingpinTargetAllowed(int owner, int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target] is { IsAlive: true, HasHpValue: true } player && player.Hp < player.MaxHp && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDingpinCompletedEvent>().Any(e => e.Owner == owner && e.Target == target && e.Suit is Suit.Spade or Suit.Club && MobileDingpinCurrentTurn(e.Turn, e.TurnOwner));
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 77200, SkillProgramEffectOp.PrepareMobileChenQunDingpin)]
    internal bool QualifyMobileDingpin(ProgramActivationLegalityContext context) => context.Owner.IsAlive && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat == context.OwnerSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play;
    void IMobileChenQunHost.Run(ProgramSkillFrame frame, SkillProgramEffectOp operation)
    {
        if (operation == SkillProgramEffectOp.PrepareMobileChenQunDingpin)
            PrepareMobileDingpin(frame);
        else
            CompleteMobileDingpin(frame);
    }

    bool IMobileChenQunSelection.CardAllowed(int owner, Card card) => MobileDingpinCardAllowed(owner, card);
    bool IMobileChenQunSelection.TargetAllowed(int owner, int target) => MobileDingpinTargetAllowed(owner, target);
    internal void PrepareMobileDingpin(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || frame.SelectedCardIds is not [var card] || frame.SelectedTargetSeats is not [var target] || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(card) != CardLocation.Hand(frame.OwnerSeat) || !MobileDingpinCardAllowed(frame.OwnerSeat, _coreAdvancedLifecycleOperations.GetAdvancedCard(card)) || !MobileDingpinTargetAllowed(frame.OwnerSeat, target) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDingpinPreparedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Dingpin requires its original legal hand category and wounded actual-Play target.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDingpinPreparedEvent(frame.Id, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, phase, target, card, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
    }

    internal bool MobileDingpinPaid(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "mobile:dingpin" || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDingpinPreparedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var prepared] || prepared.Source != new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || prepared.Hash != frame.GameplayHash || !frame.SelectedCardIds.SequenceEqual([prepared.CardId]) || !frame.SelectedTargetSeats.SequenceEqual([prepared.Target]) || frame.CardSetBindings.SingleOrDefault(b => b.Name == "dingpin-cost")is not { } binding || !binding.CardIds.SequenceEqual([prepared.CardId]) || !binding.SourceLocations.SequenceEqual([CardLocation.Hand(frame.OwnerSeat)]))
            return false;
        var costs = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == frame.Id && e.InitiatorSeat == frame.OwnerSeat && e.ExecutorSeat == frame.OwnerSeat && e.ActualCount == 1 && e.SequenceBefore >= prepared.Before && e.TurnNumber == prepared.Phase.TurnNumber && e.ActualTurnOwnerSeat == prepared.Phase.ActualTurnOwnerSeat && e.Reason.Value == "skill-program.mobile:dingpin.MoveBoundCards").ToArray();
        if (costs is not [var cost])
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == cost.InvocationId).ToArray()is [var material] && material.CardId == prepared.CardId && material.From == CardLocation.Hand(frame.OwnerSeat) && material.CompletionSequence > prepared.Before;
    }

    internal void CompleteMobileDingpin(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 6 || !MobileDingpinPaid(frame) || frame.SelectedTargetSeats is not [var target] || frame.JudgmentOutcomeBindings?.SingleOrDefault(o => o.Name == "dingpin-outcome")is not { InstructionIndex: 4 } outcome || outcome.TargetSeat != target || outcome.Reason != "skill.mobile.dingpin" || !_coreProgramJudgmentOutcomesOperations.IsValidProgramJudgmentOutcome(frame, outcome) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDingpinCompletedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Dingpin completion needs the original actual discard and finalized native judgment.");
        var prepared = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MobileDingpinPreparedEvent>().Single(e => e.FrameId == frame.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileDingpinCompletedEvent(frame.Id, frame.OwnerSeat, target, prepared.Phase.TurnNumber, prepared.Phase.ActualTurnOwnerSeat, outcome.JudgmentFrameId, outcome.Suit));
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 77200)]
    internal bool KeepMobileDingpinInitialSelection(ProgramSkillFrame frame) => frame.TriggerId is null && frame.WindowContext is null && frame.ActivationId == "judge-unused-category" && frame.InstructionIndex is >= 3 and <= 8 && MobileDingpinPaid(frame);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 77200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 77200)]
    internal bool KeepMobileDingpinPaid(ProgramSkillFrame frame) => MobileDingpinPaid(frame);
}
