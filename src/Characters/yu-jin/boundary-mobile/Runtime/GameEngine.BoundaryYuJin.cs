namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability] private sealed class BoundaryYuJinHost(GameEngine engine) : IBoundaryYuJinHost
    { public void BindRemainder(ProgramSkillFrame f, string bind) => engine.BindBoundaryYuJinRemainder(f, bind); }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 57600, SkillProgramEffectOp.BindBoundaryYuJinRemainder)]
    private bool CanOfferBoundaryYuJinJieyue(ProgramTriggerCandidate c) => c.SkillId == BoundaryYuJinComposition.Skill &&
        GetHand(_players[c.OwnerSeat]).Concat(GetEquipment(_players[c.OwnerSeat])).Any(card => !card.IsGeneralWeapon);

    private void BindBoundaryYuJinRemainder(ProgramSkillFrame supplied, string bind)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != BoundaryYuJinComposition.Skill || f.InstructionIndex != 7 || bind != BoundaryYuJinComposition.Remainder ||
            f.SelectedTargetSeats is not [var recipient] || recipient == f.OwnerSeat ||
            f.ChoiceBindings.SingleOrDefault(c => c.Name == BoundaryYuJinComposition.Answer) is not { OptionId: "retain", ChooserSeat: var chooser } || chooser != recipient ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || f.CardSetBindings.Any(b => b.Name == bind))
            throw new InvalidOperationException("Mobile Jieyue lost its original Ending frame and recipient's retained-card decision.");
        var hand = GetProgramCardSet(f, BoundaryYuJinComposition.Hand);
        var equipment = GetProgramCardSet(f, BoundaryYuJinComposition.Equipment);
        var actualHand = GetHand(_players[recipient]); var actualEquipment = GetEquipment(_players[recipient]);
        bool Retained(ProgramSkillCardSetBinding b, CardZoneKind zone, int count) => (b.SelectionActorSeat == recipient || count == 0 && b.SelectionActorSeat is null) && b.CardIds.Count == Math.Min(1, count) &&
            b.CardIds.Count == b.SourceLocations.Count && b.SourceLocations.All(l => l == new CardLocation(zone, recipient)) &&
            b.CardIds.All(id => _cardZones.GetLocation(id) == new CardLocation(zone, recipient));
        if (!Retained(hand, CardZoneKind.Hand, actualHand.Count) || !Retained(equipment, CardZoneKind.Equipment, actualEquipment.Count))
            throw new InvalidOperationException("Jieyue must retain the recipient's actual own hand/equipment entity in its original zone, or an empty binding for an absent zone.");
        var kept = hand.CardIds.Concat(equipment.CardIds).ToHashSet();
        var rest = actualHand.Concat(actualEquipment).Where(c => !kept.Contains(c.Id))
            .Where(c => !IsSelfHandCategoryDiscardForbidden(recipient, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard))
            .Where(c => !IsForeignEquipmentDiscardPrevented(recipient, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
        SetProgramCardSet(f.Id, bind, rest.Select(c => c.Id).ToArray(), SkillProgramCardSetVisibility.Private,
            rest.Select(c => _cardZones.GetLocation(c.Id)).ToArray(), selectionActorSeat: recipient);
    }
}
