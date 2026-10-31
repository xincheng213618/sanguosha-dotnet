#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGuanduEquipmentRuntime
{
    internal string? GuanduEquipmentSkillGrantId(Card card) => _coreEquipmentContentOperations.EquipmentRules.GetSkillBinding(card.Kind)is { } binding && binding.SkillId.StartsWith("guandu:", StringComparison.Ordinal) ? binding.CreateGrantId(card.Id) : null;
    internal static bool IsGuanduThunderChariotDeparture(Card card, CardLocation from, CardLocation to) => RuntimeSkills.RuntimeRules_Core_GuanduEquipment.IsGuanduThunderChariotDeparture(card, from, to);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 44002)]
    internal void RecordGuanduChariotDestruction(IGameEvent payload)
    {
        if (payload is not CardMovedEvent { CardKind: CardKind.ThunderChariot, From: { Zone: CardZoneKind.Equipment, OwnerSeat: { } owner }, To: { Zone: CardZoneKind.OutsideGame } } moved)
            return;
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Last(m => m.CardId == moved.CardId);
        if (!_coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduChariotDestroyedEvent>().Any(e => e.MovementSequence == movement.Sequence))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduChariotDestroyedEvent(moved.CardId, movement.Sequence, owner));
    }

    internal bool IsGuanduDestroyedChariotMaterial(long useId, int cardId) => cardId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.Action is { } action && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && action.PhysicalCards.Any(material => material.CardId == cardId && material.CardKind == CardKind.ThunderChariot && material.From is { Zone: CardZoneKind.Equipment, OwnerSeat: { } owner } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<GuanduChariotDestroyedEvent>().Any(e => e.CardId == cardId && e.FormerOwnerSeat == owner && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.CardId == cardId && m.Sequence == e.MovementSequence && m.From == material.From && m.To == CardLocation.OutsideGame)));
    internal void ContinueGuanduPolu(ProgramSkillFrame f)
    {
        if (f.WindowContext!.Window == SkillProgramTriggerWindow.AfterDamageApplied)
        {
            _coreGuanduGeneralsOperations.DrawGuanduGeneral(f, f.OwnerSeat, 1, "finish");
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
        if (owner.IsAlive && !_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(c => c.Kind == CardKind.ThunderChariot) && owner.EquipmentSlotCapacity(EquipmentSlot.Weapon) > 0)
        {
            var id = _cardZonesOperations._cardZones.CreateDiagnostics().Select(c => c.CardId).DefaultIfEmpty().Max() + 1;
            var card = new Card(id, CardKind.ThunderChariot, Suit.Diamond, 9);
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GuanduChariotCreatedEvent(f.Id, f.OwnerSeat, card.Id));
            _coreAdvancedWeaponsOperations.PlaceAdvancedEquipment(card, f.OwnerSeat);
        }

        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
    }

    internal Card[] GuanduChariotDiscardable(int actor, int target) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).Where(c => _coreFieldEquipmentOperations.FieldEquipmentSlot(c)is EquipmentSlot.Armor or EquipmentSlot.DefensiveHorse).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(actor, c, CardLocation.Equipment(target), OwnedCardMoveIntent.Discard)).ToArray();
    internal void ContinueGuanduThunderChariot(ProgramSkillFrame f)
    {
        var target = f.WindowContext?.TargetSeat ?? -1;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || target == f.OwnerSeat)
            throw new InvalidOperationException("The chariot lost its actual other damaged target.");
        var cards = GuanduChariotDiscardable(f.OwnerSeat, target);
        f = _coreGuanduGeneralsOperations.GuanduStage(f, "finish");
        if (cards.Length != 0)
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(f.OwnerSeat, f.OwnerSeat, cards, CardLocation.Equipment(target), new("activity.guandu.chariot.discard"));
        _coreGuanduGeneralsOperations.RunGuanduGeneral(f.Id);
    }
}
