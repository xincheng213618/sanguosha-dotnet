#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchParticipantSetupRuntime
{
    internal void AssignAutomaticGodFactions()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(_coreMatchSetupDefinitionsOperations.RequiresGodFactionSelection).OrderBy(player => player.Seat))
        {
            player.ChosenFactionId = _coreMatchSetupDefinitionsOperations.ChooseAutomaticGodFaction(player);
        }
    }

    internal Role GetTeamRole(string teamId)
    {
        var teamIndex = _coreMatchSetupDefinitionsOperations.GetModeTeamIds().Select((candidate, index) => (candidate, index)).Where(item => item.candidate == teamId).Select(item => item.index).FirstOrDefault(-1);
        return teamIndex switch
        {
            0 => Role.TeamA,
            1 => Role.TeamB,
            _ => throw new InvalidOperationException($"Mode '{(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id)}' does not define a supported team '{(teamId)}'.")};
    }

    internal int GetStartingSeat() => _modeRulesOperations._modeRules.Opening.GetStartingSeat(_coreModeRulesOperations.CaptureModeLife(), _randomOperations._random.Next, _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._modeInitialActorSeat);
    internal static IReadOnlyList<GeneralDefinition> CreateRuntimeGeneralPool(ContentRegistry contentRegistry, ContentModeDefinition mode) => MatchContentFactory.CreateGeneralPool(contentRegistry, mode);
    internal static ContentDeckRecipe ResolveDeckDefinition(ContentRegistry contentRegistry, string? deckId) => MatchContentFactory.ResolveDeck(contentRegistry, deckId);
    internal void SetupPlayers() => _coreModeSetupOperations.ApplyModeSetupPlan(_coreModeSetupOperations.ModeSetup.BuildPlayers());
    internal static IReadOnlyList<Card> CreateDeckFromRegistry(ContentRegistry contentRegistry, ContentDeckRecipe definition) => MatchContentFactory.CreateDeck(contentRegistry, definition);
    internal void SetupDeck()
    {
        var cards = CreateDeckFromRegistry(_contentRegistryOperations._contentRegistry, ResolveDeckDefinition(_contentRegistryOperations._contentRegistry, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.DeckId ?? _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.DeckId));
        cards = _coreModeSetupOperations.ModeSetup.PrepareDeck(cards);
        _cardZonesOperations._cardZones.LoadInitialDeck(cards);
        _corePhysicalCardPopulationInitialCardCountAccess._initialCardCount = _cardZonesOperations._cardZones.TotalCards;
        if (!_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.UseInteractiveSetup)
        {
            if (_coreNativeDamageHandlesOperations.UsesFormalWoodenOx && cards.Count(card => card.Kind == CardKind.WoodenOx) == 1)
            {
                _cardZonesOperations._cardZones.ShuffleKeepingSingleKindAtBottom(CardLocation.DrawPile, _randomOperations._random, CardKind.WoodenOx);
            }
            else
            {
                _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
            }
        }

        _coreNativeCoreInvariantsOperations.AssertCoreInvariants();
    }

    internal void DealInitialHands()
    {
        _coreModeSetupEquipmentOperations.PrepareModeSelectedEquipmentAndDeck();
        _modeRuntimeOperations._modeRuntime.BeforeInitialDeal();
        var counts = _coreMatchParticipantsPlayersAccess._players.ToDictionary(player => player.Seat, _coreInitialHandModifiersOperations.GetInitialHandSize);
        var rounds = counts.Values.DefaultIfEmpty(0).Max();
        for (var cardIndex = 0; cardIndex < rounds; cardIndex++)
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            {
                if (cardIndex >= counts[player.Seat])
                    continue;
                // The first seat of the first public team starts with one
                // fewer card in the 2v2 adapter, compensating for initiative.
                if (_modeRulesOperations._modeRules.ReducesFirstActorInitialHand && cardIndex == 0 && player.Seat == _coreActualTurnProgressCurrentSeatAccess._currentSeat)
                {
                    continue;
                }

                if (!_modeRuntimeOperations._modeRuntime.TryDealInitialCard(player.Seat, cardIndex))
                    _corePhysicalDrawFacadesOperations.DrawCards(player, 1, log: false, CardMoveReasons.InitialDeal);
            }
        }

        _modeRuntimeOperations._modeRuntime.AfterInitialDeal();
        _coreNativeCoreInvariantsOperations.AssertCoreInvariants();
    }

    internal void RunOneSetupStep()
    {
        if (_coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex >= _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionOrder.Count)
        {
            var factionPlayer = _coreMatchParticipantsPlayersAccess._players.Where(_coreMatchSetupDefinitionsOperations.RequiresGodFactionSelection).OrderBy(player => player.Seat).FirstOrDefault();
            if (factionPlayer is not null)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GodFactionSelectionRequestedEvent(factionPlayer.Seat, global::CardGame.Core.EngineOwners.CoreMainDefinitions.GodFactionChoices));
                if (factionPlayer.IsHuman)
                {
                    RequestHumanGodFactionSelection(factionPlayer);
                }
                else
                {
                    ApplyGodFactionSelection(factionPlayer, _coreMatchSetupDefinitionsOperations.ChooseAutomaticGodFaction(factionPlayer));
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                }

                return;
            }

            CompleteSetup();
            return;
        }

        var seat = _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionOrder[_coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex];
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        var offers = GetAvailableGeneralOffers(player);
        IReadOnlyList<GeneralDefinition> candidates = offers.Select(offer => offer.General).ToArray();
        if (candidates.Count == 0)
        {
            throw new InvalidOperationException($"Mode '{(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id)}' ran out of generals during setup.");
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GeneralSelectionRequestedEvent(player.Seat, candidates.Select(general => general.Id).ToArray()));
        if (player.IsHuman)
        {
            RequestHumanGeneralSelection(player, candidates, offers.ToDictionary(offer => offer.General.Id, offer => offer.CandidateId));
            return;
        }

        var view = _corePlayerViewProjectionOperations.CreateSnapshot(player.Seat);
        var(general, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[player.Seat].ChooseGeneral(view, view.Players.Single(snapshot => snapshot.Seat == player.Seat).Role ?? Role.Renegade, candidates, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.GeneralSelectionWeights, _contentRegistryOperations._contentRegistry);
        _coreNativeAiDecisionsOperations.AddGeneralThought(thought);
        ApplyGeneralSelection(player, general);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void RequestHumanGodFactionSelection(CharacterState player)
    {
        var choices = global::CardGame.Core.EngineOwners.CoreMainDefinitions.GodFactionChoices.Select(factionId => new PromptChoice(new ChoiceId($"setup.god-faction.{(factionId)}"), $"选择{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(factionId))}势力", [], [], new Dictionary<string, string> { ["action"] = "select-god-faction", ["faction-id"] = factionId })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.SelectFaction, player.Seat, $"{(player.General.Name)}为神势力武将。请选择本局归属的魏、蜀、吴、群势力；该选择会影响主公技等势力判定，但不会改写武将的印刷势力。", [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            IsPrivate = true
        };
        _coreMatchOutcomeStatusAccess._status = EngineStatus.AwaitingHumanFactionSelection;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ApplyGodFactionSelection(CharacterState player, string factionId)
    {
        if (!_coreMatchSetupDefinitionsOperations.RequiresGodFactionSelection(player))
        {
            throw new InvalidOperationException($"Seat {(player.Seat)} does not currently require a god-faction choice.");
        }

        if (!global::CardGame.Core.EngineOwners.CoreMainDefinitions.GodFactionChoices.Contains(factionId, StringComparer.Ordinal))
        {
            throw new InvalidOperationException($"Faction '{(factionId)}' is not a legal god-faction choice.");
        }

        player.ChosenFactionId = factionId;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        // Public logs must not reveal a hidden general or its private faction choice.
        if (player.GeneralRevealed)
        {
            _coreMatchLogOperations.AddLog("GodFactionSelected", $"{(player.Name)} 为神势力武将 {(player.General.Name)} 选择了{(global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(factionId))}势力。", player.Seat);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GodFactionSelectedEvent(player.Seat, factionId));
    }

    internal IReadOnlyList<(GeneralDefinition General, string CandidateId)> GetAvailableGeneralOffers(CharacterState player, bool fullRoster = false) => _coreModeSetupOperations.GetModeGeneralOffers(player, fullRoster);
    internal void RequestHumanGeneralSelection(CharacterState player, IReadOnlyList<GeneralDefinition> candidates, IReadOnlyDictionary<string, string> candidateIds, bool fullRoster = false)
    {
        var facts = new ModeGeneralSelectionFacts(player.Role, player.GeneralSelected, player.General.Name, player.General.BaseHp);
        var choices = candidates.Select(general => _coreModeRulesOperations.ModeSetupPresentation.GeneralChoice(facts, new(general.Id, general.Name, general.SkillSummary, general.FactionId, global::CardGame.Core.EngineOwners.CoreMatchPresentationRuntime.GetFactionName(general.FactionId ?? string.Empty), general.BaseHp, general.InitialHp, candidateIds[general.Id]), fullRoster)).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.SelectGeneral, player.Seat, _coreModeRulesOperations.ModeSetupPresentation.SelectionPrompt(facts), [], [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            ValidContentIds = candidates.Select(general => general.Id).ToArray()
        };
        _coreMatchOutcomeStatusAccess._status = EngineStatus.AwaitingHumanGeneralSelection;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ApplyGeneralSelection(CharacterState player, GeneralDefinition general)
    {
        if (!_coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._availableGenerals.Remove(general))
        {
            throw new InvalidOperationException($"General '{(general.Id)}' is not available for seat {(player.Seat)}.");
        }

        player.GeneralDraftCandidates = null;
        var retiredCandidates = _coreModeSetupOperations.ModeSetup.RemoveGeneralIdsAfterSelection(general.Id);
        if (retiredCandidates.Count != 0)
            _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._availableGenerals.RemoveAll(candidate => retiredCandidates.Contains(candidate.Id, StringComparer.Ordinal));
        if (_coreNativeDamageHandlesOperations.IsNationalWarMode)
        {
            if (string.IsNullOrWhiteSpace(general.FactionId) || !string.Equals(general.FactionId, player.NationalFactionId, StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"General '{(general.Id)}' does not belong to seat {(player.Seat)}'s assigned faction.");
            }

            if (!player.GeneralSelected)
            {
                player.General = general;
                player.GeneralSelected = true;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GeneralSelectedEvent(player.Seat, _coreIdentityConcealmentOperations.GeneralHasIdentityConcealment(general) ? "concealment:ol" : general.Id));
            }
            else if (!player.SecondaryGeneralSelected)
            {
                player.SecondaryGeneral = general;
                player.SecondaryGeneralSelected = true;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SecondaryGeneralSelectedEvent(player.Seat, general.Id));
            }
            else
            {
                throw new InvalidOperationException($"Seat {(player.Seat)} already selected both generals.");
            }
        }
        else if (_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.DualGenerals && player.GeneralSelected)
        {
            if (player.SecondaryGeneralSelected)
                throw new InvalidOperationException($"Seat {(player.Seat)} already selected both generals.");
            player.SecondaryGeneral = general;
            player.SecondaryGeneralSelected = true;
            player.SecondaryGeneralRevealed = player.Role == Role.Lord && !player.IdentityConcealed;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SecondaryGeneralSelectedEvent(player.Seat, general.Id));
        }
        else
        {
            player.General = general;
            player.GeneralSelected = true;
            _coreModeSetupOperations.ApplyModeSelectedGeneral(player, general);
            player.GeneralRevealed = !_coreNativeDamageHandlesOperations.IsTeamMode && player.Role == Role.Lord && !_coreIdentityConcealmentOperations.GeneralHasIdentityConcealment(general);
            _coreMatchLogOperations.AddLog("GeneralSelected", player.GeneralRevealed ? $"主公 {(player.Name)} 选择了 {(general.Name)}。" : $"座位 {(player.Seat + 1)} 完成私有选将。", player.Seat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GeneralSelectedEvent(player.Seat, _coreIdentityConcealmentOperations.GeneralHasIdentityConcealment(general) ? "concealment:ol" : general.Id));
        }

        _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex++;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
    }

    internal void CompleteSetup()
    {
        if (_coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._setupComplete)
        {
            return;
        }

        if (_coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionIndex != _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._selectionOrder.Count || _coreMatchParticipantsPlayersAccess._players.Any(player => !player.GeneralSelected || _coreNativeDamageHandlesOperations.IsDualGeneralMode && !player.SecondaryGeneralSelected || _coreMatchSetupDefinitionsOperations.RequiresGodFactionSelection(player)))
        {
            throw new InvalidOperationException("Setup cannot complete before every seat selects a general.");
        }

        _coreNationalSkillsOperations.InitializeDualGeneralHealth();
        _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
        DealInitialHands();
        if (_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.ConfirmOpeningHand && _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._options.HumanSeat >= 0)
        {
            _coreOpeningCustomizationOperations.RequestOpeningHandConfirmation();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        FinalizeSetup();
    }

    internal void FinalizeSetup()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _characterDaQiaoXiaoQiaoHappyGodOperations.BindHappyQiaoHealth(player);
        foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(player => !_coreNativeDamageHandlesOperations.IsNationalWarMode))
        {
            player.GeneralRevealed = true;
            player.SecondaryGeneralRevealed = player.SecondaryGeneralSelected;
            _coreIdentityConcealmentOperations.InitializeSelectedIdentityConcealment(player);
        }

        _coreConversionStatesOperations.InitializeStructuredConversionSkills();
        _coreMatchSetupAvailableGeneralsModeInitialActorSeatSelectionIndexSelectionOrderSetupCompleteAccess._setupComplete = true;
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _coreMatchLogOperations.AddLog("SetupCompleted", _coreModeRulesOperations.ModeSetupPresentation.SetupCompletedText(new(_coreMatchParticipantsPlayersAccess._players.OrderBy(player => player.Seat).Select(_coreMatchPresentationOperations.GetPublicGeneralName))), _coreActualTurnProgressCurrentSeatAccess._currentSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GameStartedEvent(_coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._playerCount, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SetupCompletedEvent(_coreMatchParticipantsPlayersAccess._players.Count, _coreMatchDefinitionModeDefinitionOptionsPlayerCountAccess._modeDefinition.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
