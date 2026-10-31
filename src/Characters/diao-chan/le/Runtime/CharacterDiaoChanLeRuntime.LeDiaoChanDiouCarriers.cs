#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanLeRuntime
{
    // Registration only; the query still belongs to legacy GameEngine.LeDiaoChanDiou.Carriers.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 200)]
    internal void ProjectDiouCarrierCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(IsDiouCarrier(cardId) ? "低讴·虚拟" : null);
    }

    internal bool IsDiouCarrier(int id) => id > 0 && HasDiouCapability && _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierCreatedEvent>().Where(e => e.CardId == id).ToArray()is [var fact] && fact.Source.SkillId == DiouSkill && fact.Source.BindingId == DiouBinding && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.Source.OwnerSeat) && _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program?.GameplayHash == fact.GameplayHash && (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(fact.Kind) || CoreNativeCardClassificationsRuntime.IsDelayedCard(fact.Kind)) && _coreAdvancedLifecycleOperations.GetAdvancedCard(id)is { Suit: Suit.None, Rank: 0 } card && card.Kind == fact.Kind;
    internal CardLocation NormalizeDiouCarrierDestination(Card card, CardLocation from, CardLocation to)
    {
        if (!IsDiouCarrier(card.Id) || from == to || to == CardLocation.OutsideGame)
            return to;
        if (from == CardLocation.OutsideGame)
        {
            // Exactly the original issued Use can bring a fresh carrier into
            // Processing. Destruction is final, even if a later ability searches
            // all registered entities rather than only the draw pile.
            if (to == CardLocation.Processing && !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == card.Id) && _resolutionStackOperations._resolutionStack.LastOrDefault()is CardUseFrame { DiouOrigin: { } origin } use && origin.CarrierId == card.Id && origin.Kind == card.Kind && IsIssuedDiouUse(use))
                return to;
            return CardLocation.OutsideGame;
        }

        // A continuing field effect can change its field holder without
        // inventing a Hand/deck entity. This also preserves native equipment
        // exchange and Wooden Ox grain-transfer semantics.
        if (from.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment)
            return to.Zone == from.Zone ? to : CardLocation.OutsideGame;
        return to.Zone is CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.Processing ? to : CardLocation.OutsideGame;
    }

    internal void RecordDiouCarrierDestruction(CardMovementRecord movement)
    {
        if (movement.To != CardLocation.OutsideGame || !IsDiouCarrier(movement.CardId))
            return;
        var original = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierCreatedEvent>().Single(e => e.CardId == movement.CardId);
        if (movement.From == CardLocation.OutsideGame || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierDestroyedEvent>().Any(e => e.CardId == movement.CardId))
            throw new InvalidOperationException("A Diou carrier cannot be destroyed twice or re-enter after destruction.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouCarrierDestroyedEvent(original.FrameId, original.Source, original.GameplayHash, movement.CardId, movement.CardKind, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? 0, movement.Sequence, movement.From, movement.Reason));
    }

    internal bool IsDiouCarrierDestroyedMaterial(CardMovementRecord movement) => movement.To == CardLocation.OutsideGame && IsDiouCarrier(movement.CardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierDestroyedEvent>().Where(e => e.CardId == movement.CardId).ToArray()is [var fact] && fact.MovementSequence == movement.Sequence && fact.From == movement.From && fact.Kind == movement.CardKind && fact.Reason == movement.Reason && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierCreatedEvent>().Any(created => created.CardId == fact.CardId && created.FrameId == fact.OriginalProgramFrameId && created.Source == fact.Source && created.GameplayHash == fact.GameplayHash && created.Kind == fact.Kind);
    internal bool IsDiouCarrierDestroyedMaterial(int cardId, CardLocation from, CardLocation to) => to == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierDestroyedEvent>().Where(e => e.CardId == cardId && e.From == from).ToArray()is [var fact] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == fact.MovementSequence)is { } movement && IsDiouCarrierDestroyedMaterial(movement);
    internal bool IsDiouCarrierDestroyedMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } && action.PhysicalCards.SingleOrDefault(c => c.CardId == cardId)is { } material && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DiouCarrierDestroyedEvent>().Where(e => e.CardId == cardId).ToArray()is [var fact] && fact.ParentFrameId == useId && fact.From == material.From && fact.Kind == material.CardKind && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == fact.MovementSequence)is { } movement && IsDiouCarrierDestroyedMaterial(movement);
    internal void RetireUnplacedDiouCarrier(long useId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { DiouOrigin: { CarrierId: > 0 } origin } use || !IsIssuedDiouUse(use))
            return;
        if (_cardZonesOperations._cardZones.GetLocation(origin.CarrierId) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(origin.CarrierId), CardLocation.Processing, CardLocation.OutsideGame, new("skill-program.le-diao-chan.diou.carrier-retired"));
    }
}
