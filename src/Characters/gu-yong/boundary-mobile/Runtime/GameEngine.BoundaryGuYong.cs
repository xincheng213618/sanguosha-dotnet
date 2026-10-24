namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class BoundaryGuYongHost(GameEngine engine) : IBoundaryGuYongHost
    {
        public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect) =>
            effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing ? engine.FreezeBoundaryGuYongColors(frame, effect) : engine.PrepareBoundaryGuYongBingyi(frame, effect);
    }

    private static int BoundaryGuYongColor(Suit suit) => suit == Suit.None ? -1 : IsRedSuit(suit) ? 1 : 0;

    private SkillProgramStepOutcome FreezeBoundaryGuYongColors(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.SkillId != "boundary:shenxing-mobile" || frame.TriggerId is not null || frame.InstructionIndex != 1 ||
            frame.SelectedCardIds.Count != 2 || frame.SelectedTargetSeats.Count != 0 || frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind))
            throw new InvalidOperationException("Mobile Shenxing requires its original unpaid two-card activation.");
        var colors = frame.SelectedCardIds.Select(id =>
        {
            var location = _cardZones.GetLocation(id);
            if (location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                throw new InvalidOperationException("Mobile Shenxing lost an original owned hand/equipment payment entity.");
            var card = _cardZones.CardsAt(location).Single(card => card.Id == id);
            return BoundaryGuYongColor(GetProgramEffectiveSuit(_players[frame.OwnerSeat], card));
        }).Distinct().ToArray();
        var outcome = colors.Length > 1 ? "different" : "same";
        ReplaceRuntimeTop(frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, outcome, frame.OwnerSeat)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new BoundaryGuYongColorsFrozenEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash, effect.ResultBind!, outcome));
        return SkillProgramStepOutcome.Continue;
    }

    private bool IsBoundaryGuYongChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) =>
        frame.SkillId == "boundary:shenxing-mobile" && binding.ChooserSeat == frame.OwnerSeat && binding.OptionId is "same" or "different" &&
        ProgramEventHistory<BoundaryGuYongColorsFrozenEvent>().Count(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat &&
            fact.SkillInstanceId == frame.SkillInstanceId && fact.GameplayHash == frame.GameplayHash && fact.ResultBind == binding.Name && fact.Outcome == binding.OptionId) == 1 &&
        ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Take(frame.InstructionIndex)
            .Count(effect => effect.Op == SkillProgramEffectOp.PrepareBoundaryGuYongShenxing && effect.ResultBind == binding.Name) == 1;

    private SkillProgramStepOutcome PrepareBoundaryGuYongBingyi(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        var binding = GetProgramCardSet(frame, effect.SourceBind!);
        var hand = GetHand(_players[frame.OwnerSeat]);
        if (frame.SkillId != "boundary:bingyi-mobile" || frame.TriggerId != "ending-reveal-share" || frame.InstructionIndex != 2 ||
            frame.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || hand.Count == 0 ||
            !binding.CardIds.Order().SequenceEqual(hand.Select(card => card.Id).Order()))
            throw new InvalidOperationException("Mobile Bingyi requires its original complete nonempty hand at its own ending window.");
        var sameColor = hand.Select(card => BoundaryGuYongColor(GetProgramEffectiveSuit(_players[frame.OwnerSeat], card))).Distinct().Count() == 1;
        var sameType = hand.Select(card => GetProgramCardCategory(card.Kind)).Distinct().Count() == 1;
        RevealProgramBoundCards(frame, effect.SourceBind!);
        if (!sameColor && !sameType)
        { FinishProgramSkill(GetActiveProgramFrame(frame.Id), true); return SkillProgramStepOutcome.AwaitChild; }
        return SkillProgramStepOutcome.Continue;
    }
}
