#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRongMouMobileRuntime
{
    // This fact is source-independent: losing/reacquiring Juxiang does not erase
    // an already declared Barbarian Assault in the same actual turn.
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 30400)]
    internal void ObserveMouZhuRongAssault(CardUseDeclaredEvent e)
    {
        if (HasMouZhuRong && e.CardKind == CardKind.BarbarianAssault)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhuRongAssaultUsedEvent(MouZhuRongTurnOrdinal, e.SourceSeat, e.ResolutionId));
    }

    internal SkillProgramStepOutcome GiftMouZhuRongOutsideAssault(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MouZhuRongComposition.Juxiang || f.TriggerId != "outside-assault-gift" || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || f.SelectedTargetSeats is not [var recipient] || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouZhuRongAssaultGeneratedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Juxiang requires its current Ending and one living recipient.");
        // The fixed mode recipe, not the depleted stock, supplies printed faces.
        // A mode without this kind uses the ordinary game's three printed faces.
        var templates = CoreMatchParticipantSetupRuntime.CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.DeckId)).Where(c => c.Kind == CardKind.BarbarianAssault).Select(c => (c.Suit, c.Rank)).Distinct().ToArray();
        if (templates.Length == 0)
            templates = [(Suit.Spade, 7), (Suit.Spade, 13), (Suit.Club, 7)];
        var before = _randomOperations._random.State;
        var template = templates[_randomOperations._random.Next(templates.Length)];
        var card = new Card(checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1), CardKind.BarbarianAssault, template.Suit, template.Rank);
        _cardZonesOperations._cardZones.AddGeneratedCard(card);
        _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhuRongAssaultGeneratedEvent(f.Id, MouZhuRongSource(f), recipient, card.Id, before, _randomOperations._random.State));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Hand(recipient), new("skill-program.mou-mobile:juxiang.outside-gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(f.Id, f.OwnerSeat, recipient, [card.Id], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null), disclosure: CardMovementDisclosure.FaceDown);
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat);
    }
}
