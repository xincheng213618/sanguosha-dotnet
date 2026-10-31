#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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
        if (f.SkillId != MouZhuRongComposition.Juxiang || f.TriggerId != "outside-assault-gift" || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } || f.SelectedTargetSeats is not [var recipient] || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongAssaultGeneratedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Juxiang requires its current Ending and one living recipient.");
        // The fixed mode recipe, not the depleted stock, supplies printed faces.
        // A mode without this kind uses the ordinary game's three printed faces.
        var templates = global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, global::CardGame.Core.EngineOwners.CoreMatchParticipantSetupRuntime.ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.DeckId)).Where(c => c.Kind == CardKind.BarbarianAssault).Select(c => (c.Suit, c.Rank)).Distinct().ToArray();
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

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 30400)]
    internal bool IsMouZhuRongGiftMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation m) => e?.Op == SkillProgramEffectOp.GiftMouZhuRongOutsideAssault && f.InstructionIndex == 2 && f.SkillId == MouZhuRongComposition.Juxiang && m.SubjectSeat == f.OwnerSeat && m.CoverageResultBind is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRongAssaultGeneratedEvent>().SingleOrDefault(g => g.FrameId == f.Id)is { } generated && generated.Source == MouZhuRongSource(f) && f.SelectedTargetSeats.SequenceEqual([generated.RecipientSeat]) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.CardId == generated.CardId && move.From == CardLocation.OutsideGame && move.To == CardLocation.Hand(generated.RecipientSeat) && move.Reason.Value == "skill-program.mou-mobile:juxiang.outside-gift") == 1;
}
