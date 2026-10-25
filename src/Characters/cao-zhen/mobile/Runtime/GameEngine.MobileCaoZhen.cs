namespace CardGame.Core;

public sealed partial class GameEngine
{
    private static readonly CardMoveReason MobileCaoZhenSidiCost = new("skill.mobile-cao-zhen.sidi.cost");
    [ProgramCapability]
    private sealed class MobileCaoZhenSidiHost(GameEngine engine) : IMobileCaoZhenSidiHost
    { public SkillProgramStepOutcome Consume(ProgramSkillFrame f, string bind) => engine.ConsumeMobileCaoZhenSidi(f, bind); }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 74010)]
    private bool MobileCaoZhenDodgeQualification(ProgramTriggerLegalityContext gate) => gate.SkillId != "mobile:sidi" ||
        gate.Trigger.Id != "store-after-actual-dodge" || gate.Context.CardUse is { } action &&
        (action.ActorSeat == gate.OwnerSeat || _turnProgression.OwnerSeat == gate.OwnerSeat);

    private SkillProgramStepOutcome ConsumeMobileCaoZhenSidi(ProgramSkillFrame supplied, string bind)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "mobile:sidi" || frame.InstructionIndex != 2 || frame.WindowContext is not
            { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor, TargetSeat: { } target } context ||
            actor != target || actor != _currentSeat || actor == frame.OwnerSeat || _phase != TurnPhase.Play ||
            _resolutionStack.OfType<PlayPhaseStartingBoundaryFrame>().SingleOrDefault(p => p.Id == context.ParentFrameId) is not { } parent ||
            parent.OwnerSeat != actor || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count ||
            parent.Items[parent.ItemIndex].Candidate is not { } candidate || !MountObserverCandidateMatches(frame, candidate))
            throw new InvalidOperationException("Mobile Sidi must pay on its original other-actor actual Play starting candidate.");
        var cards = GetProgramCardSet(frame, bind);
        if (cards.CardIds is not [var card] || cards.SourceLocations is not [var from] || from != CardLocation.Authority(frame.OwnerSeat) ||
            _cardZones.GetLocation(card) != from || ProgramEventHistory<MobileCaoZhenSidiAppliedEvent>().Any(e => e.ProgramFrameId == frame.Id))
            throw new InvalidOperationException("Mobile Sidi requires one still-owned real Qian and a once-only phase grant.");
        var before = _movementSequence;
        _ = MoveProgramBoundCards(frame.Id, frame.OwnerSeat, bind, null, SkillProgramCardDestination.DiscardPile, null, MobileCaoZhenSidiCost);
        var paid = _cardMovements.Single(m => m.Sequence > before && m.CardId == card && m.From == from && m.To == CardLocation.DiscardPile && m.Reason == MobileCaoZhenSidiCost);
        AdvanceEventRulesAndQueueFact(new MobileCaoZhenSidiAppliedEvent(frame.Id,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            _turnNumber, _turnProgression.OwnerSeat, _cardUseDebitPhaseInstanceId, actor, card, paid.Sequence));
        return SkillProgramStepOutcome.Continue;
    }

    private IEnumerable<RuleQueryContribution> MobileCaoZhenSidiContributions(CharacterState player, SkillRuleQuery query) =>
        query != SkillRuleQuery.SlashLimit || _phase != TurnPhase.Play || _currentSeat != player.Seat ? [] :
        ProgramEventHistory<MobileCaoZhenSidiAppliedEvent>().Where(e => e.ActualTurnNumber == _turnNumber && e.ActualTurnOwnerSeat == _turnProgression.OwnerSeat &&
            e.PlayPhaseInstanceId == _cardUseDebitPhaseInstanceId && e.AffectedSeat == player.Seat)
            .Select(e => (RuleQueryContribution)new FiniteRuleQueryContribution($"mobile-sidi:{e.ProgramFrameId}", SkillRuleOperation.Add, -1));
}
