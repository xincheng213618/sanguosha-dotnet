#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativePublicPileOriginalCardsRuntime
{
    internal NativePublicPileCardRequest NativePublicPileUseRequest(int actor, int provider, CardKind? kind = null, long? parent = null) => new(parent, _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId, actor, provider, actor == provider ? null : actor, null, kind, null, NativePublicPileDirection.Use);
    internal NativePublicPileCardRequest NativePublicPileResponseRequest(CharacterState provider, CardKind kind)
    {
        if (_characterSimaYiMouOperations.MouSmPublicPileRequest(provider, kind)is { } mouSmRequest)
            return mouSmRequest;
        long? ownerId = null;
        var actor = provider.Seat;
        int? requester = null;
        ProgramCardContinuation? continuation = null;
        int? cursor = null, epoch = null;
        var direction = NativePublicPileDirection.Played;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == provider.Seat && kind is CardKind.Peach or CardKind.Alcohol)
        {
            ownerId = dying.FrameId;
            cursor = dying.ResponderIndex;
            direction = NativePublicPileDirection.Use;
        }
        else if (kind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } counterspell)
        {
            ownerId = counterspell.Id;
            cursor = counterspell.CandidateIndex;
            epoch = counterspell.ChainDepth;
            direction = NativePublicPileDirection.Use;
            continuation = ProgramCardContinuation.NullificationResponse;
        }
        else if (kind == CardKind.Dodge && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense && defense.CurrentCandidateSeat == provider.Seat)
        {
            ownerId = defense.OwnerFrameId;
            cursor = defense.CandidateIndex;
            actor = defense.OwnerSeat;
            requester = actor;
            continuation = ProgramCardContinuation.FactionDefenseDodge;
        }
        else if (kind == CardKind.Slash && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true } supply && supply.CurrentCandidateSeat == provider.Seat)
        {
            ownerId = supply.OwnerFrameId;
            cursor = supply.CandidateIndex;
            actor = supply.OwnerSeat;
            requester = actor;
        }
        else if (kind == CardKind.Slash && _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } borrowed && borrowed.WeaponOwnerSeat == provider.Seat)
        {
            ownerId = borrowed.ResolutionId;
            direction = NativePublicPileDirection.Use;
        }
        else if (kind == CardKind.Slash && _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is { } followup && followup.Attack.SourceSeat == provider.Seat)
        {
            ownerId = followup.OwnerFrameId;
            direction = NativePublicPileDirection.Use;
        }
        else if (kind == CardKind.Slash && _coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && duel.ResponderSeat == provider.Seat)
        {
            ownerId = duel.ResolutionId;
            cursor = duel.SuccessfulSlashResponses;
            continuation = ProgramCardContinuation.DuelSlash;
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, CurrentAttack: { } groupAttack } group && groupAttack.TargetSeat == provider.Seat)
        {
            ownerId = group.ResolutionId;
            cursor = group.TargetIndex;
            epoch = groupAttack.SuccessfulDodgeResponses;
            continuation = ProgramCardContinuation.GroupResponse;
        }
        else if (kind == CardKind.Dodge && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } attack && attack.TargetSeat == provider.Seat)
        {
            ownerId = attack.ResolutionId;
            cursor = attack.SuccessfulDodgeResponses;
            direction = NativePublicPileDirection.Use;
            continuation = ProgramCardContinuation.Dodge;
        }

        return new(ownerId, _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId, actor, provider.Seat, requester, actor, kind, continuation, direction)
        {
            RequestCursor = cursor,
            RequestEpoch = epoch
        };
    }

    internal IReadOnlyList<NativePublicPileCardSelection> GetNativePublicPileOriginalCards(NativePublicPileCardRequest request)
    {
        if ((!_characterLiFengOlOperations.HasLiFengCapability && !_characterGongsunZanMouMobileOperations.HasMouGszMobile) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(request.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(request.ProviderSeat) || !_coreMatchParticipantsPlayersAccess._players[request.ActorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[request.ProviderSeat].IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
            return[];
        if (_resolutionStackOperations._resolutionStack.Any(f => f.NativePublicPilePayment is { SequenceAfter: 0 } pending && pending.Request.ProviderSeat == request.ProviderSeat && pending.Request != request))
            return[];
        return _characterLiFengOlOperations.ShuliangOriginalGrainChoices(request).Concat(_characterGongsunZanMouMobileOperations.MouGszHuOriginalChoices(request)).Where(selection => selection.Request == request && selection.AccessSource.OwnerSeat == request.ProviderSeat && selection.Pile.OwnerSeat == request.ProviderSeat && Enum.IsDefined(selection.Card.Kind) && _cardZonesOperations._cardZones.GetLocation(selection.Card.Id) == selection.Pile.Location && _corePublicPileSourcesOperations.PublicPileCards(selection.Pile).Any(c => c == selection.Card) && (request.RequiredKind is null || CoreNativeCardClassificationsRuntime.MatchesRequiredCard(selection.Card.Kind, request.RequiredKind.Value)) && (_characterLiFengOlOperations.IsShuliangOriginalGrainChoice(request, selection) || _characterGongsunZanMouMobileOperations.IsMouGszHuOriginalChoice(request, selection))).DistinctBy(selection => selection.Card.Id).ToArray();
    }

    internal NativePublicPileCardSelection RequireNativePublicPileOriginalCard(NativePublicPileCardRequest request, NativePublicPileCardSelection selection) => GetNativePublicPileOriginalCards(request).SingleOrDefault(s => s == selection) ?? throw new InvalidOperationException("An original public-pile card lost its exact live source, request or shared quota.");
    internal IReadOnlyList<Card> NativePublicPileResponseCards(CharacterState provider, CardKind kind)
    {
        var request = NativePublicPileResponseRequest(provider, kind);
        return request.ParentFrameId is null ? [] : GetNativePublicPileOriginalCards(request).Select(s => s.Card).ToArray();
    }

    internal IReadOnlyList<Card> NativePublicPileSlashUseCards(CharacterState provider)
    {
        var response = NativePublicPileResponseRequest(provider, CardKind.Slash);
        var request = response.ParentFrameId is null ? NativePublicPileUseRequest(provider.Seat, provider.Seat, CardKind.Slash, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id) : response;
        return GetNativePublicPileOriginalCards(request).Select(s => s.Card).ToArray();
    }

    internal IReadOnlyList<Card> NativePublicPileOriginalUseCards(CharacterState provider) => GetNativePublicPileOriginalCards(NativePublicPileUseRequest(provider.Seat, provider.Seat, parent: _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id)).Select(s => s.Card).ToArray();
    internal void PrepareNativePublicPileProvidedSlash(NativeOwners.FactionCardRequestHandle request, CharacterState provider, IReadOnlyList<Card> cards, CardConversionSource? conversion)
    {
        if (conversion is not null || cards is not [var card] || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone != CardZoneKind.PublicPersistentPile)
            return;
        var frozen = new NativePublicPileCardRequest(request.OwnerFrameId, _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId, request.OwnerSeat, provider.Seat, request.OwnerSeat, request.OwnerSeat, CardKind.Slash, null, NativePublicPileDirection.Played)
        {
            RequestCursor = request.CandidateIndex
        };
        var choice = GetNativePublicPileOriginalCards(frozen).SingleOrDefault(s => s.Card == card) ?? throw new InvalidOperationException("A supplied original grain lost its exact provider and native principal.");
        PrepareNativePublicPilePayment(_resolutionStackOperations._resolutionStack.Single(f => f.Id == request.OwnerFrameId), choice);
    }

    internal Card? FindNativePublicPileOriginalCard(CharacterState provider, int id)
    {
        var prepared = _resolutionStackOperations._resolutionStack.LastOrDefault(f => f.NativePublicPilePayment is { SequenceAfter: 0 } receipt && receipt.Request.ProviderSeat == provider.Seat && receipt.Card.Id == id)?.NativePublicPilePayment;
        if (prepared is not null && _cardZonesOperations._cardZones.GetLocation(id) == prepared.Pile.Location)
            return prepared.Card;
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != provider.Seat)
            return null;
        return GetNativePublicPileOriginalCards(NativePublicPileUseRequest(provider.Seat, provider.Seat)).SingleOrDefault(s => s.Card.Id == id)?.Card;
    }

    internal IReadOnlyList<LegalAction> NativePublicPileOriginalPlayActions(CharacterState actor, int? selectedId)
    {
        var selections = GetNativePublicPileOriginalCards(NativePublicPileUseRequest(actor.Seat, actor.Seat));
        return selections.Where(s => selectedId is null || s.Card.Id == selectedId).SelectMany(s => _coreNativeLegalActionsOperations.BuildLegalActionsCore(actor, includeProgramActions: false, selectedPhysicalCardId: s.Card.Id, nativeOriginalCard: s.Card).Where(a => a.CardId == s.Card.Id && a.ConversionSource is null && (a.AdditionalConversionSources is null || a.AdditionalConversionSources.Count == 0) && a.Kind is not (LegalActionKind.Recast or LegalActionKind.EndPlay or LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill or LegalActionKind.RevealGeneral) && a.MaxCardCount <= 1 && (a.PlayedCardKind ?? s.Card.Kind)is var kind && (kind == s.Card.Kind || s.Card.Kind == CardKind.Slash && kind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(actor))).Select(a => a with { NativePublicPileSource = s.AccessSource, Description = $"【{(_contentRegistryOperations._contentRegistry.GetSkill(s.AccessSource.SkillId).Name)}】{(a.Description)}" })).ToArray();
    }

    internal bool IsNativePublicPileOriginalChoice(CharacterState owner, Card card, CardKind kind, bool response)
    {
        if (!response && _characterHuangZuMobileShiOperations.IsShiHzOriginalUseChoice(owner, card, kind))
            return true;
        if ((!_characterLiFengOlOperations.HasLiFengCapability && !_characterGongsunZanMouMobileOperations.HasMouGszMobile) || card.Id <= 0 || _cardZonesOperations._cardZones.GetLocation(card.Id)is not { Zone: CardZoneKind.PublicPersistentPile } location || location.OwnerSeat != owner.Seat)
            return false;
        var prepared = _resolutionStackOperations._resolutionStack.LastOrDefault(f => f.NativePublicPilePayment is { SequenceAfter: 0 } p && p.Request.ProviderSeat == owner.Seat && p.Card.Id == card.Id)?.NativePublicPilePayment;
        if (prepared is not null)
            return prepared.Card == card && _cardZonesOperations._cardZones.GetLocation(card.Id) == prepared.Pile.Location && (card.Kind == kind || CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) && kind == CardKind.Slash || card.Kind == CardKind.Slash && kind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[prepared.Request.ActorSeat]));
        var request = response ? NativePublicPileResponseRequest(owner, kind) : NativePublicPileUseRequest(owner.Seat, owner.Seat);
        return (CoreNativeCardClassificationsRuntime.MatchesRequiredCard(card.Kind, kind) || card.Kind == CardKind.Slash && kind == CardKind.FireSlash && _coreNativeTargetLegalityOperations.HasZhuqueFan(owner)) && GetNativePublicPileOriginalCards(request).Any(s => s.Card == card);
    }

    internal void PrepareNativePublicPilePayment(ResolutionFrame parent, NativePublicPileCardSelection selection)
    {
        selection = RequireNativePublicPileOriginalCard(selection.Request, selection);
        if (parent.NativePublicPilePayment is { } previous && (previous.SequenceAfter == 0 || previous.ActionId is null))
            throw new InvalidOperationException("A native public-pile request still owns its previous payment.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { NativePublicPilePayment = new(parent.Id, selection, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
    }

    internal void PrepareNativePublicPileResponsePayment(CharacterState provider, Card card, CardKind kind, CardConversionSource? conversion)
    {
        if (_characterGongsunZanMouMobileOperations.PrepareMouGszHuResponse(provider, [card], kind, conversion))
            return;
        if (conversion is not null || card.Id <= 0 || _cardZonesOperations._cardZones.GetLocation(card.Id).Zone != CardZoneKind.PublicPersistentPile || _resolutionStackOperations._resolutionStack.Any(f => f.NativePublicPilePayment is { SequenceAfter: 0 } p && p.Request.ProviderSeat == provider.Seat && p.Card.Id == card.Id))
            return;
        var request = NativePublicPileResponseRequest(provider, kind);
        var selection = GetNativePublicPileOriginalCards(request).SingleOrDefault(s => s.Card == card) ?? throw new InvalidOperationException("An original public-pile response is no longer authorized.");
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == request.ParentFrameId) ?? throw new InvalidOperationException("An original public-pile response lost its native mother.");
        PrepareNativePublicPilePayment(parent, selection);
    }

    internal void PrepareNativePublicPileUse(CardUseFrame use)
    {
        _characterGongsunZanMouMobileOperations.PrepareMouGszHuUse(use);
        if (_characterHuangZuMobileShiOperations.IsShiHzOriginalUseParent(use))
            return;
        if (use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost], ConversionChain.Count: 0 } action || cost.From.Zone != CardZoneKind.PublicPersistentPile)
            return;
        if (_characterPangTongFriendOperations.IsFriendYangmingOriginalUseParent(use))
            return;
        var previous = _resolutionStackOperations._resolutionStack.Count > 1 ? _resolutionStackOperations._resolutionStack[^2] : null;
        if (previous?.NativePublicPilePayment is { SequenceAfter: 0 } selected && selected.Card.Id == cost.CardId && selected.Card.Kind == cost.CardKind && selected.Pile.Location == cost.From && selected.Request.ActorSeat == action.ActorSeat && selected.Request.ProviderSeat == action.ProviderSeat)
        {
            _coreRuntimeOperations.ReplaceRuntimeFrame(previous.Id, previous with { NativePublicPilePayment = null });
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { NativePublicPilePayment = selected with { OwnerFrameId = use.Id, ActionId = action.ActionId } });
            return;
        }

        var request = use.CardKind is CardKind.Peach or CardKind.Alcohol && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == action.ActorSeat && action.ActorSeat == action.ProviderSeat ? NativePublicPileResponseRequest(_coreMatchParticipantsPlayersAccess._players[action.ProviderSeat], use.CardKind) : NativePublicPileUseRequest(action.ActorSeat, action.ProviderSeat, parent: use.Id);
        var choice = GetNativePublicPileOriginalCards(request).SingleOrDefault(s => s.Card.Id == cost.CardId && s.Card.Kind == cost.CardKind && s.Pile.Location == cost.From) ?? throw new InvalidOperationException("An original public-pile use has no exact native source authorization.");
        PrepareNativePublicPilePayment(use, choice);
        var current = _resolutionStackOperations._resolutionStack.Single(f => f.Id == use.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, current with { NativePublicPilePayment = current.NativePublicPilePayment!with { ActionId = action.ActionId } });
    }

    internal bool TryNativePublicPilePaymentLocation(CharacterState provider, Card card, out CardLocation from)
    {
        var payment = _resolutionStackOperations._resolutionStack.LastOrDefault(f => f.NativePublicPilePayment is { SequenceAfter: 0 } p && p.Request.ProviderSeat == provider.Seat && p.Card.Id == card.Id)?.NativePublicPilePayment;
        from = payment?.Pile.Location ?? default;
        return payment is not null && payment.Card == card && payment.OwnerFrameId > 0 && payment.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && payment.TurnOwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && _cardZonesOperations._cardZones.GetLocation(card.Id) == from;
    }

    internal void CaptureNativePublicPilePaymentBatch(CardMovementBatchContext batch)
    {
        if (batch.ParentFrameId is not { } id || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id)is not { NativePublicPilePayment: { SequenceAfter: 0 } receipt } parent)
            return;
        var entries = batch.Movements.Where(m => m.CardId == receipt.Card.Id && m.CardKind == receipt.Card.Kind && m.From == receipt.Pile.Location && m.To == CardLocation.Processing && m.Sequence > receipt.SequenceBefore && (m.Reason == CardMoveReasons.Use || m.Reason == CardMoveReasons.EquipmentUse || m.Reason == CardMoveReasons.IronChainUse || m.Reason == CardMoveReasons.Respond || m.Reason == CardMoveReasons.Nullification)).ToArray();
        if (entries.Length == 0)
            return;
        if (entries is not [var entered] || entered.Sequence <= 0 || entered.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[(int)entered.Sequence - 1] != entered || receipt.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || receipt.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat)
            throw new InvalidOperationException("An original public-pile payment lost its one real native entry.");
        var paid = receipt with
        {
            SequenceAfter = entered.Sequence
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, parent with { NativePublicPilePayment = paid });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativePublicPileOriginalPaidEvent(id, entered.CardId, entered.CardKind, entered.From, entered.Sequence, entered.Reason, paid.Source, paid.Request.ActorSeat, paid.Request.ProviderSeat, paid.Request.RequesterSeat, paid.Request.ResponderSeat, paid.Request.Direction, paid.ActualTurn, paid.TurnOwnerSeat, paid.Request.RequestCursor, paid.Request.RequestEpoch));
        if (paid.Source.SkillId == "ol:shuliang")
            _characterLiFengOlOperations.RecordShuliangGrainPayment(paid);
        else if (!_characterGongsunZanMouMobileOperations.IsMouGszHuOriginalPayment(paid))
            throw new InvalidOperationException("An original public-pile payment lost its independently authorized source.");
        if (paid.ActionId is { } actionId)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativePublicPileOriginalBoundEvent(id, actionId, entered.CardId, entered.Sequence));
    }

    internal void BindNativePublicPilePayment(ResolutionFrame parent, CardActionContext action)
    {
        if (parent.NativePublicPilePayment is not { SequenceAfter: > 0, ActionId: null } receipt || action.PhysicalCards is not [var cost] || cost.CardId != receipt.Card.Id || cost.CardKind != receipt.Card.Kind || cost.From != receipt.Pile.Location || action.ProviderSeat != receipt.Request.ProviderSeat || action.ActorSeat != receipt.Request.ActorSeat || action.RequesterSeat != receipt.Request.RequesterSeat || action.ConversionChain.Count != 0)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { NativePublicPilePayment = receipt with { ActionId = action.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NativePublicPileOriginalBoundEvent(parent.Id, action.ActionId, cost.CardId, receipt.SequenceAfter));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 27590)]
    internal void BindAcceptedNativePublicPileOriginal(IGameEvent payload)
    {
        if (payload is not CardActionAcceptedEvent { Action: { } action })
            return;
        foreach (var parent in _resolutionStackOperations._resolutionStack.ToArray())
            BindNativePublicPilePayment(parent, action);
    }

    internal bool IsNativePublicPileOriginalActionCost(CardActionContext action, CardActionCost cost) => action.PhysicalCards is [var only] && only == cost && action.ConversionChain.Count == 0 && cost.From.Zone == CardZoneKind.PublicPersistentPile && cost.From.OwnerSeat == action.ProviderSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<NativePublicPileOriginalBoundEvent>().Where(e => e.ActionId == action.ActionId && e.CardId == cost.CardId).Any(bound => _coreProgramEventHistoryOperations.ProgramEventHistory<NativePublicPileOriginalPaidEvent>().Any(paid => paid.ParentFrameId == bound.ParentFrameId && paid.CardId == bound.CardId && paid.EntrySequence == bound.EntrySequence && paid.From == cost.From && paid.CardKind == cost.CardKind && paid.ActorSeat == action.ActorSeat && paid.ProviderSeat == action.ProviderSeat && paid.RequesterSeat == action.RequesterSeat));
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 27590)]
    internal void AssertNativePublicPileOriginalPayments()
    {
        foreach (var parent in _resolutionStackOperations._resolutionStack)
        {
            if (parent.NativePublicPilePayment is not { } paid)
                continue;
            if (paid.OwnerFrameId != parent.Id || paid.Source.OwnerSeat != paid.Request.ProviderSeat || paid.Pile.OwnerSeat != paid.Request.ProviderSeat || paid.Selection.Request != paid.Request || !Enum.IsDefined(paid.Request.Direction) || paid.SequenceBefore < 0 || paid.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || paid.SequenceAfter < 0 || paid.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || paid.SequenceAfter > 0 && paid.SequenceAfter <= paid.SequenceBefore || paid.Request.RequestCursor is < 0 || paid.Request.RequestEpoch is < 0)
                throw new InvalidOperationException("An original public-pile payment lost its owning frame and immutable native request.");
            if (paid.SequenceAfter == 0)
                continue;
            var records = _coreProgramEventHistoryOperations.ProgramEventHistory<NativePublicPileOriginalPaidEvent>().Where(e => e.ParentFrameId == parent.Id && e.EntrySequence == paid.SequenceAfter).ToArray();
            if (records is not [var fact] || fact.Source != paid.Source || fact.CardId != paid.Card.Id || fact.CardKind != paid.Card.Kind || fact.From != paid.Pile.Location || fact.ActorSeat != paid.Request.ActorSeat || fact.ProviderSeat != paid.Request.ProviderSeat || fact.RequesterSeat != paid.Request.RequesterSeat || fact.ResponderSeat != paid.Request.ResponderSeat || fact.Direction != paid.Request.Direction || fact.ActualTurn != paid.ActualTurn || fact.TurnOwnerSeat != paid.TurnOwnerSeat || fact.RequestCursor != paid.Request.RequestCursor || fact.RequestEpoch != paid.Request.RequestEpoch || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == fact.EntrySequence && m.CardId == fact.CardId && m.CardKind == fact.CardKind && m.From == fact.From && m.To == CardLocation.Processing && m.Reason == fact.Reason))
                throw new InvalidOperationException("An original public-pile payment lost its one actual native entry.");
            if (paid.ActionId is { } actionId && _coreProgramEventHistoryOperations.ProgramEventHistory<NativePublicPileOriginalBoundEvent>().Count(e => e.ParentFrameId == parent.Id && e.ActionId == actionId && e.CardId == paid.Card.Id && e.EntrySequence == paid.SequenceAfter) != 1)
                throw new InvalidOperationException("An original public-pile payment lost its exact accepted native action.");
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 3800)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureNativePublicPilePaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureNativePublicPilePaymentBatch(completed);
        return completed;
    }
}
