#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseEntityExchangeRuntime
{
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedAfterCategory, 110)]
    internal bool ResponseEntityExchangeFinalizedAfterCategoryTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ExchangeRespondedCardEntities) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(query.Kind);
    internal IEnumerable<IGameEvent> ResponseExchangeHistory() => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents);
    internal bool HasResponseEntityExchangeObservers() => _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && (_coreNationalSkillsOperations.GetSkillBindingShard(p)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardResponseAccepted) ?? []).Any(b => b.Trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ExchangeRespondedCardEntities)));
    internal IReadOnlyList<ProgramResponseExchangeStateSnapshot>? GetResponseExchangeStateSnapshots()
    {
        var states = _coreMatchParticipantsPlayersAccess._players.SelectMany(p => (_coreNationalSkillsOperations.GetSkillBindingShard(p)?.GetInstanceTriggers(SkillProgramTriggerWindow.CardResponseAccepted) ?? []).SelectMany(b => b.Trigger.Effects.Where(e => e.Op == SkillProgramEffectOp.ExchangeRespondedCardEntities).Select(e => new ProgramResponseExchangeStateSnapshot(p.Seat, b.SkillId, b.SkillInstanceId, e.StateId!, ResponseExchangeUpgraded(p.Seat, b.SkillId, b.SkillInstanceId, e.StateId!))))).Distinct().ToArray();
        return states.Length == 0 ? null : Array.AsReadOnly(states);
    }

    internal IReadOnlyList<TurnProhibitedPhysicalCardsSnapshot>? GetResponseEntityRestrictionSnapshots()
    {
        var states = ResponseExchangeHistory().OfType<ProgramResponseEntityClaimedEvent>().Where(e => e.Restricted && e.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat).Select(e => new TurnProhibitedPhysicalCardsSnapshot(e.RecipientSeat, e.OwnerSeat, e.SkillId, e.SkillInstanceId, Array.AsReadOnly(e.CardIds.ToArray()), e.TurnNumber, e.TurnSeat)).ToArray();
        return states.Length == 0 ? null : Array.AsReadOnly(states);
    }

    internal bool ResponseExchangeUpgraded(int owner, string skill, string instance, string state) => ResponseExchangeHistory().OfType<ProgramResponseExchangeUpgradedEvent>().Any(e => e.OwnerSeat == owner && e.SkillId == skill && e.SkillInstanceId == instance && e.StateId == state);
    internal bool IsExchangedCardClaim(long actionId, int cardId) => ResponseExchangeHistory().OfType<ProgramResponseEntityClaimedEvent>().Any(e => e.ClaimedActionId == actionId && e.CardIds.Contains(cardId));
    internal bool IsExchangedUseCardClaim(long frameId, int cardId) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == frameId)?.Action is { } action && IsExchangedCardClaim(action.ActionId, cardId);
    internal bool IsResponseEntityRestricted(int seat, int id)
    {
        foreach (var claim in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramResponseEntityClaimedEvent>())
            if (claim.Restricted && claim.RecipientSeat == seat && claim.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && claim.TurnSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && claim.CardIds.Contains(id))
                return true;
        return false;
    }

    internal Card EntityAtCurrentLocation(int id) => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
    internal CardActionContext? ActualRespondedAction(CardActionContext response)
    {
        if (response.Type != CardActionType.Response)
            return null;
        // The old ParentActionId remains the outer trick for compatibility. In a
        // counterspell chain, the directly answered action is the prior counterspell.
        if (response.EffectiveKind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { ChainDepth: > 1 })
            return ResponseExchangeHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.ActionId != response.ActionId && a.Type == CardActionType.Response && a.EffectiveKind == CardKind.Nullification && a.ParentActionId == response.ParentActionId);
        if (response.ParentActionId is not { } parentId)
            return null;
        return ResponseExchangeHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.ActionId == parentId);
    }

    internal bool CanExchangeResponseEntities(int owner, string skill, string instance, SkillProgramEffect effect, CardActionContext response)
    {
        var original = ActualRespondedAction(response);
        if (original is null || original.ActorSeat != owner || response.ActorSeat == owner || original.Type != CardActionType.Use && original.EffectiveKind != CardKind.Nullification || response.PhysicalCards.Count == 0 || response.ConversionChain.Any(_coreWushuangJinzhiOperations.IsWushuangJinzhiSource) || response.PhysicalCards.Any(c => IsExchangedCardClaim(response.ActionId, c.CardId) || _cardZonesOperations._cardZones.GetLocation(c.CardId).Zone is not (CardZoneKind.Processing or CardZoneKind.DiscardPile)))
            return false;
        if (effect.ResponseExchangeMode == ProgramResponseExchangeMode.ClaimResponseOnly || ResponseExchangeUpgraded(owner, skill, instance, effect.StateId!))
            return true;
        return _coreMatchParticipantsPlayersAccess._players[response.ActorSeat].IsAlive && !original.ConversionChain.Any(_coreWushuangJinzhiOperations.IsWushuangJinzhiSource) && original.PhysicalCards.Count > 0 && original.PhysicalCards.All(c => !IsExchangedCardClaim(original.ActionId, c.CardId) && _cardZonesOperations._cardZones.GetLocation(c.CardId).Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 70)]
    internal bool CanOfferResponseExchange(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var effect = trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.ExchangeRespondedCardEntities);
        if (effect is null)
            return true;
        var response = ResponseExchangeHistory().OfType<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.ActionId == context.CardUse?.CardActionId);
        return response is not null && CanExchangeResponseEntities(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, effect, response);
    }

    internal SkillProgramStepOutcome BeginResponseEntityExchange(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == frame.WindowContext!.ParentFrameId);
        var response = window.Action;
        var original = ActualRespondedAction(response);
        if (original is null || !CanExchangeResponseEntities(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect, response))
            return SkillProgramStepOutcome.Continue;
        var originals = effect.ResponseExchangeMode == ProgramResponseExchangeMode.ClaimResponseOnly || ResponseExchangeUpgraded(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect.StateId!) || original.ConversionChain.Any(_coreWushuangJinzhiOperations.IsWushuangJinzhiSource) ? [] : original.PhysicalCards.Select(c => c.CardId).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ResponseEntityExchange = new(original.ActionId, response.ActionId, response.ActorSeat, originals, response.PhysicalCards.Select(c => c.CardId).ToArray(), effect.StateId!, 1) });
        ClaimResponseEntities(frame, original.ActionId, response.ActorSeat, originals, true);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ClaimResponseEntities(ProgramSkillFrame frame, long action, int recipient, IReadOnlyList<int> ids, bool restricted)
    {
        if (ids.Count == 0 || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            return;
        var draft = frame.ResponseEntityExchange!;
        if (ids.Any(id => IsExchangedCardClaim(action, id) || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Processing or CardZoneKind.DiscardPile)))
            throw new InvalidOperationException("A response entity exchange lost its exact public cost.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramResponseEntityClaimedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, draft.RespondedActionId, draft.ResponseActionId, action, recipient, ids.ToArray(), _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, restricted));
        foreach (var id in ids)
            _corePhysicalMovementFacadesOperations.MoveCard(EntityAtCurrentLocation(id), _cardZonesOperations._cardZones.GetLocation(id), CardLocation.Hand(recipient), new CardMoveReason("skill-program.response-entity-exchange"));
    }

    internal SkillProgramStepOutcome BeginPublicSuitEscalatingDiscard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var target = frame.SelectedTargetSeats.Single();
        if (target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var previous = ResponseExchangeHistory().OfType<ProgramEscalatingDiscardStartedEvent>().Count(e => e.OwnerSeat == frame.OwnerSeat && e.SkillId == frame.SkillId && e.StateId == effect.StateId);
        if (effect.PublicSuitDiscardMode is not null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicSuitDiscard = new(target, checked(previous + 1), effect.StateId!, effect.SkillIds.Single()) });
            PublishPublicSuitChoice(frame);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return StartPublicSuitDraw(frame, effect.Suits.Single(), previous, previous);
    }

    internal SkillProgramStepOutcome StartPublicSuitDraw(ProgramSkillFrame frame, Suit suit, int previous, int required)
    {
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var target = frame.SelectedTargetSeats.Single();
        var draw = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Sum(p => _corePhysicalOwnedCardQueriesOperations.GetEquipment(p).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(p)).Count(c => _coreNativeCardAppearanceOperations.EffectiveSuit(p, c) == suit));
        if (effect.PublicSuitDiscardMode is not null)
            draw = Math.Min(5, draw);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramEscalatingDiscardStartedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, effect.StateId!, previous, target, draw));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicSuitDiscard = new(target, required, effect.StateId!, effect.SkillIds.Single(), 1) { ChosenSuit = effect.PublicSuitDiscardMode is null ? null : suit, DrawCount = effect.PublicSuitDiscardMode is null ? null : draw } });
        _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[target], draw, true, new CardMoveReason("skill-program.public-suit-escalating-draw"));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PublishPublicSuitChoice(ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var choices = new[]
        {
            Suit.Spade,
            Suit.Heart,
            Suit.Club,
            Suit.Diamond
        }.Select(s => new PromptChoice(new ChoiceId($"public-suit-choice.{(frame.Id)}.{(s)}"), global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(s), [], [], new Dictionary<string, string> { ["program-action"] = "public-suit-choice", ["suit"] = s.ToString() })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, $"{(skill.Name)}：选择场上牌的花色。", [], [], frame.PublicSuitDiscard!.TargetSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("public-suit-choice")]
    internal void ResolvePublicSuitChoice(PromptChoice choice)
    {
        var frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var draft = frame.PublicSuitDiscard!;
        if (draft.Stage != 0 || !Enum.TryParse<Suit>(choice.Parameters.GetValueOrDefault("suit"), out var suit) || suit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond))
            throw new InvalidOperationException("The public-suit choice lost its original escalating discard.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        StartPublicSuitDraw(frame, suit, draft.Required - 1, draft.Required);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 990)]
    internal bool TryResumeResponseEntityExchange(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != id || frame.PendingMovementContinuation is not null)
            return false;
        if (frame.ResponseEntityExchange is { } exchange)
        {
            if (exchange.Stage == 1)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ResponseEntityExchange = exchange with { Stage = 2 } });
                if (_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive)
                    ClaimResponseEntities(frame, exchange.ResponseActionId, frame.OwnerSeat, exchange.ResponseCardIds.Where(cardId => !IsExchangedCardClaim(exchange.ResponseActionId, cardId) && _cardZonesOperations._cardZones.GetLocation(cardId).Zone is CardZoneKind.Processing or CardZoneKind.DiscardPile).ToArray(), false);
                if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
                    _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            }
            else
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ResponseEntityExchange = null });
                _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            }

            return true;
        }

        if (frame.PublicSuitDiscard is { } discard)
        {
            if (discard.Stage == 0)
            {
                PublishPublicSuitChoice(frame);
                return true;
            }

            if (discard.Stage == 1)
            {
                var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[discard.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[discard.TargetSeat])).ToArray();
                if (!_coreMatchParticipantsPlayersAccess._players[discard.TargetSeat].IsAlive || discard.Required == 0 || cards.Length == 0)
                {
                    FinishPublicSuitDiscard(frame);
                    return true;
                }

                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicSuitDiscard = discard with { Required = Math.Min(discard.Required, cards.Length), Stage = 2, SelectedIds = [] } });
                PublishPublicSuitDiscard(frame);
                return true;
            }

            if (discard.Stage == 2)
            {
                PublishPublicSuitDiscard(frame);
                return true;
            }

            FinishPublicSuitDiscard(frame);
            return true;
        }

        return false;
    }

    internal void FinishPublicSuitDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.PublicSuitDiscard!;
        var policy = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect.PublicSuitDiscardMode;
        if (draft.Exhausted)
        {
            var sourceIds = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == frame.SkillId && g.SkillInstanceId == frame.SkillInstanceId).Select(g => g.SourceId).ToHashSet();
            foreach (var grant in _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == draft.UpgradeSkillId && g.IsEnabled && sourceIds.Contains(g.SourceId)))
                if (policy is null || !ResponseExchangeUpgraded(frame.OwnerSeat, grant.SkillId, grant.SkillInstanceId, draft.StateId))
                    _coreProgramSkillChangesOperations.CommitProgramSkillChangeFact(frame, new ProgramResponseExchangeUpgradedEvent(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, grant.SkillId, grant.SkillInstanceId, draft.StateId));
            if (policy is null)
                foreach (var grant in _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.Grants.Where(g => g.SkillId == frame.SkillId && g.SkillInstanceId == frame.SkillInstanceId))
                    _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].SkillGrants.RemoveGrant(grant.GrantId);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicSuitDiscard = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal void PublishPublicSuitDiscard(ProgramSkillFrame frame)
    {
        var draft = frame.PublicSuitDiscard!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat])).Where(c => !draft.SelectedIds!.Contains(c.Id)).ToArray();
        var choices = cards.Select(c => new PromptChoice(new ChoiceId($"public-suit-discard.{(frame.Id)}.{(c.Id)}"), $"弃置【{(c.DisplayName)}】{(global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.GetSuitDisplayName(c.Suit))}{(c.RankText)}", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "public-suit-discard" })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, draft.TargetSeat, $"{(skill.Name)}：选择弃置 {(draft.Required)} 张手牌或装备牌。", cards.Select(c => c.Id).ToArray(), [], draft.TargetSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("public-suit-discard")]
    internal void ResolvePublicSuitDiscard(PromptChoice choice)
    {
        var frame = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        var draft = frame.PublicSuitDiscard!;
        var id = choice.Cards.Single();
        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat])).ToArray();
        if (!_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat].IsAlive || available.All(c => c.Id != id) || draft.SelectedIds!.Contains(id))
            throw new InvalidOperationException("The selected escalating discard is no longer owned.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var selected = draft.SelectedIds!.Append(id).ToArray();
        if (selected.Length < draft.Required)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PublicSuitDiscard = draft with { SelectedIds = selected } });
            PublishPublicSuitDiscard((ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1]);
            return;
        }

        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[draft.TargetSeat]).Select(c => c.Id).ToArray();
        var exhausted = effect.PublicSuitDiscardMode is null ? selected.Length > 0 && selected.Length == available.Length : hand.Length > 0 && hand.All(selected.Contains);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PublicSuitDiscard = draft with { Stage = 3, SelectedIds = selected, Exhausted = exhausted, PaidHandIds = effect.PublicSuitDiscardMode is null ? null : hand } });
        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(frame.OwnerSeat, draft.TargetSeat, selected, new("skill-program.public-suit-escalating-discard"), frame.Id);
        foreach (var cardId in selected)
            _corePhysicalMovementFacadesOperations.MoveCard(EntityAtCurrentLocation(cardId), _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.DiscardPile, new CardMoveReason("skill-program.public-suit-escalating-discard"));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue)
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool TryGetResponseExchangeMovementDyingCosts(ProgramCardTriggerWindowFrame? window, out int[] costs)
    {
        costs = [];
        if (window is not { Continuation: ProgramCardContinuation.NullificationResponse, Action.Type: CardActionType.Response, Action.EffectiveKind: CardKind.Nullification } || _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not { } pending || window.ParentFrameId != pending.Id || _coreDyingCursorRuntimeOperations.ActiveDying is not { Continuation: DyingContinuationKind.ProgramSkill, KillerSeat: null } dying)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        if (i < 0 || i + 4 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[i + 1] is not ProgramSkillFrame exchange || exchange.WindowContext?.ParentFrameId != window.Id || exchange.ResponseEntityExchange is not { } draft || draft.ResponseActionId != window.Action.ActionId || exchange.PendingMovementContinuation is not { } movementPending || movementPending.SubjectSeat != exchange.OwnerSeat || movementPending.CoverageResultBind is not null || _resolutionStackOperations._resolutionStack[i + 2] is not CardsMovedTriggerWindowFrame movement || movement.Batch.ParentFrameId != exchange.Id || movement.ResumeProgramFrameId is { } resume && resume != exchange.Id || movement.Batch.OriginSkillId != exchange.SkillId || movement.Batch.OriginSkillInstanceId != exchange.SkillInstanceId || movement.Batch.OriginOwnerSeat != exchange.OwnerSeat || movement.Batch.Movements.Count == 0 || movement.Batch.Movements.Any(m => m.Reason.Value != "skill-program.response-entity-exchange" || !draft.OriginalCardIds.Concat(draft.ResponseCardIds).Contains(m.CardId)) || _resolutionStackOperations._resolutionStack[i + 3] is not ProgramSkillFrame program || program.WindowContext is not { } context || context.ParentFrameId != movement.Id || context.MovementBatch?.Id != movement.Batch.Id || context.Window is not (SkillProgramTriggerWindow.CardsMoved or SkillProgramTriggerWindow.CardsGained) || program.Id != dying.ParentFrameId || _resolutionStackOperations._resolutionStack[i + 4] is not DyingFrame child || child.Id != dying.FrameId || child.ParentFrameId != program.Id || child.VictimSeat != dying.VictimSeat || child.KillerSeat != dying.KillerSeat || child.ResponderIndex != dying.ResponderIndex || !child.ResponderSeats.SequenceEqual(dying.ResponderSeats))
            return false;
        var paid = ProgramInstructionResolver.Default.Resolve(exchange, _contentRegistryOperations._contentRegistry.GetSkill(exchange.SkillId).Program!).GetPausedInstruction(exchange.InstructionIndex).Effect;
        var effect = ProgramInstructionResolver.Default.Resolve(program, _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program!).GetPausedInstruction(program.InstructionIndex).Effect;
        if (paid.Op != SkillProgramEffectOp.ExchangeRespondedCardEntities || effect.Op != SkillProgramEffectOp.LoseHp || global::CardGame.Core.EngineOwners.CoreGameEngineProgramChoicesRuntime.ResolveProgramEffectTarget(program, effect.Target) != dying.VictimSeat)
            return false;
        if (i + 5 < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[i + 5] is CardUseFrame rescue)
        {
            if (rescue.DyingResponse is not { } response || response.ResolutionId != dying.FrameId || response.ResponderSeat != dying.ResponderSeat || rescue.SourceSeat != response.ResponderSeat || rescue.Action is not { Type: CardActionType.Use } action || action.ActorSeat != rescue.SourceSeat || action.ProviderSeat != rescue.SourceSeat || !rescue.TargetSeats.SequenceEqual([dying.VictimSeat]) || rescue.CardKind is not (CardKind.Peach or CardKind.Alcohol) || action.EffectiveKind != rescue.CardKind || !action.EffectiveDesignatedTargetSeats.SequenceEqual(rescue.TargetSeats) || !action.PhysicalCards.Select(c => c.CardId).SequenceEqual(rescue.PhysicalCardIds ?? [rescue.CardId]) || (rescue.CardKind == CardKind.Peach ? !response.UsedPeach : !response.UsedAlcohol || rescue.SourceSeat != dying.VictimSeat))
                return false;
            costs = (rescue.PhysicalCardIds ?? [rescue.CardId]).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray();
        }

        return true;
    }

    internal void AssertResponseEntityExchange(ProgramSkillFrame frame, SkillProgramEffect paused)
    {
        if (frame.ResponseEntityExchange is { } exchange)
        {
            var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == frame.WindowContext?.ParentFrameId);
            var original = window is null ? null : ActualRespondedAction(window.Action);
            if (paused.Op != SkillProgramEffectOp.ExchangeRespondedCardEntities || exchange.StateId != paused.StateId || exchange.Stage is not (1 or 2) || window is null || original is null || original.ActionId != exchange.RespondedActionId || original.ActorSeat != frame.OwnerSeat || window.Action.ActionId != exchange.ResponseActionId || window.Action.ActorSeat != exchange.RecipientSeat || !window.Action.PhysicalCards.Select(c => c.CardId).SequenceEqual(exchange.ResponseCardIds) || exchange.OriginalCardIds.Count > 0 && (!original.PhysicalCards.Select(c => c.CardId).SequenceEqual(exchange.OriginalCardIds) || paused.ResponseExchangeMode is not null) || exchange.OriginalCardIds.Count == 0 && paused.ResponseExchangeMode is null && !ResponseExchangeUpgraded(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, exchange.StateId))
                throw new InvalidOperationException("An entity exchange lost its exact responded/response identity or upgrade source.");
        }

        if (frame.PublicSuitDiscard is { } discard)
        {
            if (paused.Op != SkillProgramEffectOp.DrawPublicSuitThenEscalatingDiscard || discard.StateId != paused.StateId || discard.UpgradeSkillId != paused.SkillIds.Single() || discard.TargetSeat == frame.OwnerSeat || !frame.SelectedTargetSeats.SequenceEqual([discard.TargetSeat]) || discard.Required < 0 || discard.Stage is < 0 or > 3 || discard.Stage == 0 && paused.PublicSuitDiscardMode is null || discard.SelectedIds is { } selected && (selected.Distinct().Count() != selected.Count || selected.Count > discard.Required) || discard.Exhausted && (discard.Stage != 3 || discard.SelectedIds is not { Count: > 0 }) || (paused.PublicSuitDiscardMode is null ? discard.ChosenSuit is not null || discard.DrawCount is not null || discard.PaidHandIds is not null : discard.Stage == 0 ? discard.ChosenSuit is not null || discard.DrawCount is not null || discard.Required < 1 : discard.ChosenSuit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond) || discard.DrawCount is null or < 0 or > 5 || discard.Stage == 3 && (discard.PaidHandIds is null || discard.PaidHandIds.Distinct().Count() != discard.PaidHandIds.Count || discard.Exhausted != (discard.PaidHandIds.Count > 0 && discard.PaidHandIds.All(discard.SelectedIds!.Contains)))))
                throw new InvalidOperationException("An escalating discard lost its real participant, source or paid set.");
            if (discard.Stage == 0 && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: false } suitPrompt || suitPrompt.PlayerSeat != frame.OwnerSeat || suitPrompt.Choices.Count != 4 || suitPrompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") != "public-suit-choice")))
                throw new InvalidOperationException("An escalating discard lost its public suit choice.");
            if (discard.Stage == 2 && ReferenceEquals(frame, _resolutionStackOperations._resolutionStack.LastOrDefault()) && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != discard.TargetSeat || prompt.Choices.Count == 0 || prompt.Choices.Any(c => c.Parameters.GetValueOrDefault("program-action") != "public-suit-discard")))
                throw new InvalidOperationException("An escalating discard lost its private payer choice.");
        }
    }

    SkillProgramStepOutcome global::CardGame.Core.IResponseExchangeProgramHost.ExchangeRespondedCardEntities(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginResponseEntityExchange(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IResponseExchangeProgramHost.DrawPublicSuitThenEscalatingDiscard(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginPublicSuitEscalatingDiscard(frame, effect);
}
