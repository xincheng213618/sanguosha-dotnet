namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheEmptySlotEquipmentCapability(GameEngine engine) : IShanheEmptySlotEquipmentProgramHost
    {
        public SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame) => engine.PlaceShanheEmptySlotEquipment(frame);
    }

    private Card[] ShanheEmptySlotEquipmentCandidates(int ownerSeat)
    {
        var owner = _players[ownerSeat];
        if (!owner.IsAlive) return [];
        return _cardZones.CardsAt(CardLocation.DrawPile)
            .Where(card => EquipmentRules.IsEquipment(card.Kind) &&
                GetEquipment(owner).Count(equipped => FieldEquipmentSlot(equipped) == FieldEquipmentSlot(card)) <
                    owner.EquipmentSlotCapacity(FieldEquipmentSlot(card)))
            .OrderBy(card => card.Id).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28080)]
    private bool CanOfferShanheEmptySlotEquipment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.PlaceRandomEmptySlotDeckEquipment) ||
        context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && candidate.OwnerSeat == _currentSeat &&
            ShanheEmptySlotEquipmentCandidates(candidate.OwnerSeat).Length > 0;

    private SkillProgramStepOutcome PlaceShanheEmptySlotEquipment(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } || frame.OwnerSeat != _currentSeat)
            throw new InvalidOperationException("Empty-slot equipment placement requires its owner's actual preparation frame.");
        var candidates = ShanheEmptySlotEquipmentCandidates(frame.OwnerSeat);
        if (candidates.Length == 0) return SkillProgramStepOutcome.Continue;
        var card = candidates[_random.Next(candidates.Length)];
        var slot = FieldEquipmentSlot(card);
        // Native placement deliberately has no Use declaration, processing card,
        // generated entity or replacement. The chosen slot still has capacity.
        MoveCard(card, CardLocation.DrawPile, CardLocation.Equipment(frame.OwnerSeat), CardMoveReasons.EquipmentEnter);
        AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, frame.OwnerSeat, slot, card.Id, card.Kind, null));
        AddLog("EquipmentChanged", $"{_players[frame.OwnerSeat].Name} 将牌堆中的【{card.DisplayName}】置入空装备栏。", frame.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }
}
