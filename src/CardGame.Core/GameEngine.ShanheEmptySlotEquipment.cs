namespace CardGame.Core;

public sealed record ShanheRandomEquipmentCreatedEvent(long ProgramFrameId, int OwnerSeat, string SkillId,
    int CardId, CardKind CardKind, Suit Suit, int Rank) : IGameEvent;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheEmptySlotEquipmentCapability(GameEngine engine) : IShanheEmptySlotEquipmentProgramHost
    {
        public SkillProgramStepOutcome PlaceRandomDeckEquipment(ProgramSkillFrame frame, bool armorOnly = false) =>
            engine.PlaceShanheEmptySlotEquipment(frame, armorOnly);
    }

    private Card[] ShanheEmptySlotEquipmentCandidates(int ownerSeat, bool armorOnly = false)
    {
        var owner = _players[ownerSeat];
        if (!owner.IsAlive || armorOnly && GetEquipment(owner).Any(card => FieldEquipmentSlot(card) == EquipmentSlot.Armor)) return [];
        // Shanhe excludes equipment before dealing and before recycling. Its
        // random equipment skills use the registered deck's printed templates,
        // creating a native entity directly in the field, never in DrawPile.
        var pool = _options.ShanheBattle is { StartingResourcesExpireOnUse: true }
            ? CreateDeckFromRegistry(_contentRegistry,
                ResolveDeckDefinition(_contentRegistry, _options.DeckId ?? _modeDefinition.DeckId))
            : _cardZones.CardsAt(CardLocation.DrawPile);
        return pool
            .Where(card => EquipmentRules.IsEquipment(card.Kind) && (!armorOnly || FieldEquipmentSlot(card) == EquipmentSlot.Armor) &&
                GetEquipment(owner).Count(equipped => FieldEquipmentSlot(equipped) == FieldEquipmentSlot(card)) <
                    owner.EquipmentSlotCapacity(FieldEquipmentSlot(card)))
            .OrderBy(card => card.Id).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 28080)]
    private bool CanOfferShanheEmptySlotEquipment(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        !trigger.Effects.Any(effect => effect.Op is SkillProgramEffectOp.PlaceRandomEmptySlotDeckEquipment or
            SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment) ||
        context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && candidate.OwnerSeat == _currentSeat &&
            ShanheEmptySlotEquipmentCandidates(candidate.OwnerSeat, trigger.Effects.Any(effect =>
                effect.Op == SkillProgramEffectOp.PlaceRandomMissingArmorDeckEquipment)).Length > 0;

    private SkillProgramStepOutcome PlaceShanheEmptySlotEquipment(ProgramSkillFrame supplied, bool armorOnly)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } || frame.OwnerSeat != _currentSeat)
            throw new InvalidOperationException("Empty-slot equipment placement requires its owner's actual preparation frame.");
        var candidates = ShanheEmptySlotEquipmentCandidates(frame.OwnerSeat, armorOnly);
        if (candidates.Length == 0) return SkillProgramStepOutcome.Continue;
        var card = candidates[_random.Next(candidates.Length)];
        var from = CardLocation.DrawPile;
        if (_options.ShanheBattle is { StartingResourcesExpireOnUse: true })
        {
            card = card with { Id = CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1 };
            _cardZones.AddGeneratedCard(card);
            _generatedPhysicalCardIds.Add(card.Id);
            from = CardLocation.OutsideGame;
            AdvanceEventRulesAndQueueFact(new ShanheRandomEquipmentCreatedEvent(frame.Id, frame.OwnerSeat, frame.SkillId,
                card.Id, card.Kind, card.Suit, card.Rank));
        }
        var slot = FieldEquipmentSlot(card);
        // Native placement has no Use declaration, processing or replacement.
        // Every entity enters through one real movement into remaining capacity.
        MoveCard(card, from, CardLocation.Equipment(frame.OwnerSeat), CardMoveReasons.EquipmentEnter);
        AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(frame.Id, frame.OwnerSeat, slot, card.Id, card.Kind, null));
        AddLog("EquipmentChanged", $"{_players[frame.OwnerSeat].Name} 将【{card.DisplayName}】置入空装备栏。", frame.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }
}
