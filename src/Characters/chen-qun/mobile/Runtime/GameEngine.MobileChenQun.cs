namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool MobileDingpinCurrentTurn(int turn, int owner) => turn == _turnNumber && owner == _turnProgression.OwnerSeat;
    private bool MobileDingpinCardAllowed(int owner, Card card)
    {
        var category = GetProgramCardCategory(card.Kind);
        if (ProgramEventHistory<CurrentTurnCardUseKindsRecordedEvent>().Any(e => e.ActorSeat == owner &&
            MobileDingpinCurrentTurn(e.TurnNumber, e.TurnOwnerSeat) && (e.CardCategoryMask & (1 << (int)category)) != 0)) return false;
        var invocations = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>()
            .Where(e => MobileDingpinCurrentTurn(e.TurnNumber, e.ActualTurnOwnerSeat)).Select(e => e.InvocationId).ToHashSet();
        return !ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Any(e => invocations.Contains(e.InvocationId) &&
            e.From.OwnerSeat == owner && GetProgramCardCategory(GetAdvancedCard(e.CardId).Kind) == category);
    }
    private bool MobileDingpinTargetAllowed(int owner, int target) => IsValidPlayerSeat(target) &&
        _players[target] is { IsAlive: true, HasHpValue: true } player && player.Hp < player.MaxHp &&
        !ProgramEventHistory<MobileDingpinCompletedEvent>().Any(e => e.Owner == owner && e.Target == target &&
            e.Suit is Suit.Spade or Suit.Club && MobileDingpinCurrentTurn(e.Turn, e.TurnOwner));

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 77200, SkillProgramEffectOp.PrepareMobileChenQunDingpin)]
    private bool QualifyMobileDingpin(ProgramActivationLegalityContext context) => context.Owner.IsAlive &&
        _currentSeat == context.OwnerSeat && _phase == TurnPhase.Play;

    [ProgramCapability]
    private sealed class MobileChenQunHost(GameEngine engine) : IMobileChenQunHost, IMobileChenQunSelection
    {
        public bool CardAllowed(int owner, Card card) => engine.MobileDingpinCardAllowed(owner, card);
        public bool TargetAllowed(int owner, int target) => engine.MobileDingpinTargetAllowed(owner, target);
        public void Run(ProgramSkillFrame frame, SkillProgramEffectOp operation)
        { if (operation == SkillProgramEffectOp.PrepareMobileChenQunDingpin) engine.PrepareMobileDingpin(frame); else engine.CompleteMobileDingpin(frame); }
    }
    private void PrepareMobileDingpin(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 1 || frame.SelectedCardIds is not [var card] || frame.SelectedTargetSeats is not [var target] ||
            CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat ||
            _cardZones.GetLocation(card) != CardLocation.Hand(frame.OwnerSeat) || !MobileDingpinCardAllowed(frame.OwnerSeat, GetAdvancedCard(card)) ||
            !MobileDingpinTargetAllowed(frame.OwnerSeat, target) || ProgramEventHistory<MobileDingpinPreparedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Dingpin requires its original legal hand category and wounded actual-Play target.");
        AdvanceEventRulesAndQueueFact(new MobileDingpinPreparedEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, phase, target, card, _movementSequence));
    }
    private bool MobileDingpinPaid(ProgramSkillFrame frame)
    {
        if (frame.SkillId != "mobile:dingpin" || ProgramEventHistory<MobileDingpinPreparedEvent>().Where(e => e.FrameId == frame.Id).ToArray() is not [var prepared] ||
            prepared.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) || prepared.Hash != frame.GameplayHash ||
            !frame.SelectedCardIds.SequenceEqual([prepared.CardId]) || !frame.SelectedTargetSeats.SequenceEqual([prepared.Target]) ||
            frame.CardSetBindings.SingleOrDefault(b => b.Name == "dingpin-cost") is not { } binding ||
            !binding.CardIds.SequenceEqual([prepared.CardId]) || !binding.SourceLocations.SequenceEqual([CardLocation.Hand(frame.OwnerSeat)])) return false;
        var costs = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().Where(e => e.ParentFrameId == frame.Id &&
            e.InitiatorSeat == frame.OwnerSeat && e.ExecutorSeat == frame.OwnerSeat && e.ActualCount == 1 && e.SequenceBefore >= prepared.Before &&
            e.TurnNumber == prepared.Phase.TurnNumber && e.ActualTurnOwnerSeat == prepared.Phase.ActualTurnOwnerSeat &&
            e.Reason.Value == "skill-program.mobile:dingpin.MoveBoundCards").ToArray();
        if (costs is not [var cost]) return false;
        return ProgramEventHistory<NativeLogicalDiscardMaterialCompletedEvent>().Where(e => e.InvocationId == cost.InvocationId).ToArray() is [var material] &&
            material.CardId == prepared.CardId && material.From == CardLocation.Hand(frame.OwnerSeat) && material.CompletionSequence > prepared.Before;
    }
    private void CompleteMobileDingpin(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.InstructionIndex != 6 || !MobileDingpinPaid(frame) || frame.SelectedTargetSeats is not [var target] ||
            frame.JudgmentOutcomeBindings?.SingleOrDefault(o => o.Name == "dingpin-outcome") is not { InstructionIndex: 4 } outcome ||
            outcome.TargetSeat != target || outcome.Reason != "skill.mobile.dingpin" || !IsValidProgramJudgmentOutcome(frame, outcome) ||
            ProgramEventHistory<MobileDingpinCompletedEvent>().Any(e => e.FrameId == frame.Id))
            throw new InvalidOperationException("Dingpin completion needs the original actual discard and finalized native judgment.");
        var prepared = ProgramEventHistory<MobileDingpinPreparedEvent>().Single(e => e.FrameId == frame.Id);
        AdvanceEventRulesAndQueueFact(new MobileDingpinCompletedEvent(frame.Id, frame.OwnerSeat, target,
            prepared.Phase.TurnNumber, prepared.Phase.ActualTurnOwnerSeat, outcome.JudgmentFrameId, outcome.Suit));
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.PaidInitialSelection, 77200)]
    private bool KeepMobileDingpinInitialSelection(ProgramSkillFrame frame) => frame.TriggerId is null &&
        frame.WindowContext is null && frame.ActivationId == "judge-unused-category" &&
        frame.InstructionIndex is >= 3 and <= 8 && MobileDingpinPaid(frame);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 77200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 77200)]
    private bool KeepMobileDingpinPaid(ProgramSkillFrame frame) => MobileDingpinPaid(frame);
}
