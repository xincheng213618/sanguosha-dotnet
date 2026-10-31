#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeckProgramsRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 30, SkillProgramEffectOp.UseDeckSlashesThenShuffle)]
    internal bool AllowsUseDeckSlashesThenShuffleTrigger(ProgramTriggerLegalityContext gate)
    {
        var owner = gate.Owner;
        var features = gate.Features;
        var context = gate.Context;
        return !(features.HasOperation(SkillProgramEffectOp.UseDeckSlashesThenShuffle) && (context.TargetSeat is not { } deckTarget || _coreMatchParticipantsPlayersAccess._players[deckTarget].Gender != GeneralGender.Male || owner.Hp <= 0 || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) > owner.Hp * 10));
    }

    SkillProgramStepOutcome global::CardGame.Core.IDeckProgramHost.ExecuteDeckProgram(SkillProgramEffect effect, ProgramSkillFrame frame) => ExecuteDeckProgram(effect, frame);
    [ProgramActionSource(ProgramActionSourcePhase.ProgramTail, 10)]
    internal IEnumerable<LegalAction> BuildDeckEndExchangeActions(CharacterState provider)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != provider.Seat))
            foreach (var program in _coreSkillProgramsOperations.EnabledActivationPrograms(owner))
                foreach (var activation in program.Activations.Where(item => ProgramInstructionResolver.Default.Features(item).HasOperation(SkillProgramEffectOp.ExchangeOwnedCardThroughDeckEnd)))
                {
                    if (_coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses.GetValueOrDefault((provider.Seat, owner.Seat, program.Id, activation.Id)) >= 1)
                        continue;
                    var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(provider).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(provider)).Select(card => card.Id).ToArray();
                    if (cards.Length == 0)
                        continue;
                    yield return new(LegalActionKind.UseProgramSkill, null, owner.Seat, $"将一张手牌或装备牌交给 {(owner.Name)}，由其选择是否交换牌堆另一端的牌。", MinCardCount: 1, MaxCardCount: 1, MinTargetCount: 1, MaxTargetCount: 1)
                    {
                        ProgramSkillId = program.Id,
                        ProgramActivationId = activation.Id,
                        ProgramSkillOwnerSeat = owner.Seat,
                        SelectableCardIds = cards,
                        SelectableTargetSeats = [owner.Seat],
                        ProgramAiHint = new(1, 0, 0, 0, 0, 0, true, false)
                    };
                }
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Contribution, 10)]
    internal bool TryExecuteDeckEndContribution(CharacterState provider, CharacterState owner, LegalAction action, int cardId)
    {
        var program = _coreSkillProgramsOperations.GetEnabledSkillProgram(owner, action.ProgramSkillId!);
        var plan = ProgramInstructionResolver.Default.Find(program, ProgramInstructionSourceKind.Activation, action.ProgramActivationId);
        var activation = plan?.Activation;
        if (activation is null || !plan!.Features.HasOperation(SkillProgramEffectOp.ExchangeOwnedCardThroughDeckEnd))
            return false;
        var key = (provider.Seat, owner.Seat, program.Id, activation.Id);
        if (_coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses.GetValueOrDefault(key) >= 1 || provider.Seat == owner.Seat)
            throw new InvalidOperationException("Deck exchange contribution exhausted its own phase entry.");
        var source = _cardZonesOperations._cardZones.GetLocation(cardId);
        if (source.OwnerSeat != provider.Seat || source.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
            throw new InvalidOperationException("The contribution requires one real provider hand or equipment card.");
        _coreGameEngineSkillProgramsProgramContributionUsesAccess._programContributionUses[key] = 1;
        var frame = new ProgramSkillFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, owner.Seat, program.Id, activation.Id, program.GameplayHash, 0, [cardId], [])
        {
            SkillInstanceId = _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(owner, program.Id),
            DeckEndExchange = new(provider.Seat, cardId, source, "gift")
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillStartedEvent(frame.Id, owner.Seat, program.Id, activation.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    internal SkillProgramStepOutcome ExecuteDeckProgram(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        if (effect.Op == SkillProgramEffectOp.ExchangeOwnedCardThroughDeckEnd)
        {
            if (frame.DeckEndExchange is null)
            {
                var id = frame.SelectedCardIds.Single();
                var location = _cardZonesOperations._cardZones.GetLocation(id);
                if (location.OwnerSeat != frame.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                    throw new InvalidOperationException("A self deck exchange requires one owned HE entity.");
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DeckEndExchange = new(frame.OwnerSeat, id, location, "choice") });
            }

            if (frame.DeckEndExchange!.Stage == "gift")
            {
                var d = frame.DeckEndExchange;
                var card = _cardZonesOperations._cardZones.CardsAt(d.OriginalLocation).Single(c => c.Id == d.CardId);
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckEndExchange = d with { Stage = "choice" } });
                _corePhysicalMovementFacadesOperations.MoveCard(card, d.OriginalLocation, CardLocation.Hand(frame.OwnerSeat), new("program.deck-end.gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, d.OriginalLocation.OwnerSeat!.Value, frame.OwnerSeat, [d.CardId], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
                AwaitDeckProgramMovement(frame.Id);
                return SkillProgramStepOutcome.AwaitChild;
            }

            ResumeDeckEndExchange(frame);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        var target = frame.WindowContext?.TargetSeat ?? throw new InvalidOperationException("Deck Slash sequence requires its ending-turn target.");
        if (_coreMatchParticipantsPlayersAccess._players[target].Gender != GeneralGender.Male || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || target == frame.OwnerSeat || _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp <= 0 || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) > _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].Hp * 10)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { DeckSlashSequence = new(target, [], _coreMatchDefinitionPlayerCountAccess._playerCount) });
        ResumeDeckSlashSequence(frame, effect);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1000)]
    internal bool TryResumeDeckPrograms(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.InstructionIndex == 0 || frame.DeckEndExchange is null && frame.DeckSlashSequence is null)
            return false;
        if (!DeckProgramSourceValid(frame) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "牌堆操作的技能实例或拥有者已失效。");
            return true;
        }

        if (frame.DeckEndExchange is not null)
            ResumeDeckEndExchange(frame);
        else
            ResumeDeckSlashSequence(frame, ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect);
        return true;
    }

    internal bool DeckProgramSourceValid(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Any(program => program.Id == frame.SkillId);
    internal void ResumeDeckEndExchange(ProgramSkillFrame frame)
    {
        var d = frame.DeckEndExchange!;
        if (!DeckProgramSourceValid(frame) || !_coreMatchParticipantsPlayersAccess._players[d.ProviderSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "牌堆交换参与者或技能已失效。");
            return;
        }

        if (d.Stage == "choice")
        {
            var expected = d.ProviderSeat == frame.OwnerSeat ? d.OriginalLocation : CardLocation.Hand(frame.OwnerSeat);
            if (_cardZonesOperations._cardZones.GetLocation(d.CardId) != expected)
            {
                FinishDeckEndExchange(frame);
                return;
            }

            PublishDeckEndChoice(frame);
            return;
        }

        if (d.Stage != "draw" || d.PlacedOnTop is null)
            throw new InvalidOperationException("Deck exchange lost its ordered draw stage.");
        var seats = new[]
        {
            frame.OwnerSeat,
            d.ProviderSeat
        }.Distinct().ToArray();
        if (d.DrawIndex >= seats.Length)
        {
            FinishDeckEndExchange(frame);
            return;
        }

        var seat = seats[d.DrawIndex];
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckEndExchange = d with { DrawIndex = d.DrawIndex + 1 } });
        if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive && _corePhysicalDrawFacadesOperations.EnsureDrawPile())
        {
            var pile = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile);
            var card = d.PlacedOnTop.Value ? pile[0] : pile[^1];
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(seat), new("program.deck-end.draw"));
        }

        AwaitDeckProgramMovement(frame.Id);
    }

    internal void FinishDeckEndExchange(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckEndExchange = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal IReadOnlyList<PromptChoice> DeckEndChoices(ProgramSkillFrame frame)
    {
        var d = frame.DeckEndExchange!;
        PromptChoice Choice(string end, string label) => new(new($"deck-end.{(frame.Id)}.{(end)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = "deck-end-exchange", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["end"] = end });
        var choices = new List<PromptChoice>
        {
            Choice("top", "置于牌堆顶，从牌堆底摸牌"),
            Choice("bottom", "置于牌堆底，从牌堆顶摸牌")
        };
        if (d.ProviderSeat != frame.OwnerSeat)
            choices.Add(Choice("decline", "保留获得的牌，不交换牌堆牌"));
        return choices;
    }

    internal void PublishDeckEndChoice(ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var held = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(frame.DeckEndExchange!.CardId)).Single(card => card.Id == frame.DeckEndExchange.CardId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择将【" + held.DisplayName + " " + global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(held.Suit) + held.RankText + "】置于哪一端，再从另一端摸牌。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = DeckEndChoices(frame),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("deck-end-exchange")]
    internal void ResolveDeckEndChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing deck exchange frame.");
        var d = frame.DeckEndExchange ?? throw new InvalidOperationException("Missing deck exchange draft.");
        if (d.Stage != "choice" || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], DeckEndChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Deck exchange choice changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!DeckProgramSourceValid(frame) || !_coreMatchParticipantsPlayersAccess._players[d.ProviderSeat].IsAlive)
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "牌堆交换的参与者或技能已失效。");
            return;
        }

        var location = d.ProviderSeat == frame.OwnerSeat ? d.OriginalLocation : CardLocation.Hand(frame.OwnerSeat);
        if (_cardZonesOperations._cardZones.GetLocation(d.CardId) != location)
        {
            FinishDeckEndExchange(frame);
            return;
        }

        if (selected.Parameters["end"] == "decline")
        {
            FinishDeckEndExchange(frame);
            return;
        }

        var onTop = selected.Parameters["end"] == "top";
        var card = _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == d.CardId);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckEndExchange = d with { Stage = "draw", PlacedOnTop = onTop } });
        _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DrawPile, new("program.deck-end.place"));
        if (!onTop)
            _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom([card.Id]);
        AwaitDeckProgramMovement(frame.Id);
    }

    internal void AwaitDeckProgramMovement(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(id);
    }

    internal void ResumeDeckSlashSequence(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var d = frame.DeckSlashSequence!;
        if (d.Shuffled)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckSlashSequence = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        var target = _coreMatchParticipantsPlayersAccess._players[d.TargetSeat];
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        // Query again after every real child; new deck cards participate, previously
        // declared physical entities never do, even if a child returned them.
        var declared = d.DeclaredIds.ToHashSet();
        var next = target.IsAlive && owner.IsAlive && d.UsedCount < d.Limit && DeckProgramSourceValid(frame) && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().FirstOrDefault(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) && !declared.Contains(card.Id) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(owner, target, card, effectiveKind: card.Kind, ignoreDistance: effect.TargetRestriction == SkillProgramCardTargetRestriction.DistanceUnlimitedAgainstTarget)) : null;
        if (next is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckSlashSequence = d with { ActiveCardId = null, Shuffled = true } });
            ShuffleDeckSequence(frame);
            AwaitDeckProgramMovement(frame.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { DeckSlashSequence = d with { DeclaredIds = d.DeclaredIds.Append(next.Id).ToArray(), ActiveCardId = next.Id } });
        _coreNativeSlashUseOperations.ResolveSlashCore(owner, target, next, next.Kind, owner.Seat, countsTowardSlashLimit: false, programSkillCardUseFrameId: frame.Id, physicalSourceLocation: CardLocation.DrawPile);
    }

    internal void ShuffleDeckSequence(ProgramSkillFrame frame)
    {
        _corePhysicalMovementFacadesOperations.MoveAllCards(CardLocation.DiscardPile, CardLocation.DrawPile, CardMoveReasons.Reshuffle);
        _cardZonesOperations._cardZones.Shuffle(CardLocation.DrawPile, _randomOperations._random);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DeckSlashSequenceShuffledEvent(frame.Id, frame.OwnerSeat, frame.DeckSlashSequence!.UsedCount, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile)));
    }

    internal void CleanupDeckSlashSequence(ProgramSkillFrame frame)
    {
        if (frame.DeckSlashSequence is { Shuffled: false })
            ShuffleDeckSequence(frame);
    }

    internal void AssertDeckProgramDrafts(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.DeckEndExchange is { } d)
        {
            if (paused.Op != SkillProgramEffectOp.ExchangeOwnedCardThroughDeckEnd || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.ProviderSeat) || frame.SelectedCardIds is not [var id] || id != d.CardId || d.OriginalLocation.OwnerSeat != d.ProviderSeat || d.OriginalLocation.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || d.Stage is not ("choice" or "draw") || d.DrawIndex < 0 || d.DrawIndex > (d.ProviderSeat == frame.OwnerSeat ? 1 : 2) || (d.Stage == "choice" ? d.PlacedOnTop is not null || d.DrawIndex != 0 : d.PlacedOnTop is null))
                throw new InvalidOperationException("Deck exchange draft lost its frozen entity, owner or ordered stage.");
            if (ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && d.Stage == "choice" && frame.PendingMovementContinuation is null && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p || p.PlayerSeat != frame.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, DeckEndChoices(frame))))
                throw new InvalidOperationException("Deck exchange lost its private matching prompt.");
        }

        if (frame.DeckSlashSequence is { } s && (paused.Op != SkillProgramEffectOp.UseDeckSlashesThenShuffle || frame.WindowContext?.TargetSeat != s.TargetSeat || s.TargetSeat == frame.OwnerSeat || s.Limit != _coreMatchDefinitionPlayerCountAccess._playerCount || s.DeclaredIds.Distinct().Count() != s.DeclaredIds.Count || s.UsedCount > s.Limit || s.ActiveCardId is { } active && s.DeclaredIds.LastOrDefault() != active))
            throw new InvalidOperationException("Deck Slash sequence lost its physical membership or declaration cap.");
    }
}
