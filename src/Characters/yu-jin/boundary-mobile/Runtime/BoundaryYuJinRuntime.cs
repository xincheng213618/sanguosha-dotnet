namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class BoundaryYuJinRuntime(IProgramRuntimeFrames frames, ICardZoneQueries cards,
    IOwnedCardMoveQueries moves, IProgramCardSetBindings bindings) : IBoundaryYuJinHost
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 57600, SkillProgramEffectOp.BindBoundaryYuJinRemainder)]
    private bool CanOfferBoundaryYuJinJieyue(ProgramTriggerCandidate candidate) =>
        candidate.SkillId == BoundaryYuJinComposition.Skill &&
        cards.CardsAt(CardLocation.Hand(candidate.OwnerSeat))
            .Concat(cards.CardsAt(CardLocation.Equipment(candidate.OwnerSeat))).Any(card => !card.IsGeneralWeapon);

    public void BindRemainder(ProgramSkillFrame supplied, string bind)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.SkillId != BoundaryYuJinComposition.Skill || frame.InstructionIndex != 7 || bind != BoundaryYuJinComposition.Remainder ||
            frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat ||
            frame.ChoiceBindings.SingleOrDefault(choice => choice.Name == BoundaryYuJinComposition.Answer) is not
                { OptionId: "retain", ChooserSeat: var chooser } || chooser != recipient ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || frame.CardSetBindings.Any(set => set.Name == bind))
            throw new InvalidOperationException("Mobile Jieyue lost its original Ending frame and recipient's retained-card decision.");
        var hand = ProgramBoundCardRuntime.GetProgramCardSet(frame, BoundaryYuJinComposition.Hand);
        var equipment = ProgramBoundCardRuntime.GetProgramCardSet(frame, BoundaryYuJinComposition.Equipment);
        var actualHand = cards.CardsAt(CardLocation.Hand(recipient));
        var actualEquipment = cards.CardsAt(CardLocation.Equipment(recipient));
        bool Retained(ProgramSkillCardSetBinding set, CardZoneKind zone, int count) =>
            (set.SelectionActorSeat == recipient || count == 0 && set.SelectionActorSeat is null) &&
            set.CardIds.Count == Math.Min(1, count) && set.CardIds.Count == set.SourceLocations.Count &&
            set.SourceLocations.All(location => location == new CardLocation(zone, recipient)) &&
            set.CardIds.All(id => cards.GetLocation(id) == new CardLocation(zone, recipient));
        if (!Retained(hand, CardZoneKind.Hand, actualHand.Count) || !Retained(equipment, CardZoneKind.Equipment, actualEquipment.Count))
            throw new InvalidOperationException("Jieyue must retain the recipient's actual own hand/equipment entity in its original zone, or an empty binding for an absent zone.");
        var kept = hand.CardIds.Concat(equipment.CardIds).ToHashSet();
        var rest = actualHand.Concat(actualEquipment).Where(card => !kept.Contains(card.Id))
            .Where(card => !moves.IsSelfHandCategoryDiscardForbidden(recipient, card, cards.GetLocation(card.Id), OwnedCardMoveIntent.Discard))
            .Where(card => !moves.IsForeignEquipmentDiscardPrevented(recipient, card, cards.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
        bindings.SetProgramCardSet(frame.Id, bind, rest.Select(card => card.Id).ToArray(),
            SkillProgramCardSetVisibility.Private, rest.Select(card => cards.GetLocation(card.Id)).ToArray(), selectionActorSeat: recipient);
    }
}
