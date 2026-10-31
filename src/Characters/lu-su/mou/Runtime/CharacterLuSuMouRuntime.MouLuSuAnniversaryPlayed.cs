#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    internal IReadOnlyList<MouLuSuAnniversaryPlayedPlan> MouLuSuPlayedPlans(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Yin: true, Operation: SkillProgramEffectOp.ResolveMouLuSuMengmou } r || r.Index >= r.X || !_coreMatchParticipantsPlayersAccess._players[r.Other].IsAlive)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Other];
        var plans = new List<MouLuSuAnniversaryPlayedPlan>();
        foreach (var card in _coreNativeResponseMaterialQueriesOperations.GetResponseCards(actor, CardKind.Slash).Where(c => _coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(actor, c, CardKind.Slash)))
        {
            var kind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(actor, card, CardKind.Slash);
            var identity = _coreCardConversionsOperations.HasProgramCardIdentity(actor, card);
            if (!identity && CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, CardKind.Slash) && (!_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(actor, card) || _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(actor, card, kind, true)))
                plans.Add(new(kind, null, false) { Cards = [card.Id] });
            foreach (var source in identity ? _coreCardConversionsOperations.GetProgramCardIdentitySources(actor, card, kind, true) : _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, true))
                plans.Add(new(kind, source, false) { Cards = [card.Id] });
        }

        plans.AddRange(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, CardKind.Slash, true).Select(p => new MouLuSuAnniversaryPlayedPlan(p.OutputKind, p.Source, false) { Cards = p.Cards.Select(c => c.Id).ToArray() }));
        plans.AddRange(_coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Select(p => new MouLuSuAnniversaryPlayedPlan(CardKind.Slash, null, true) { Cards = p.Select(c => c.Id).ToArray() }));
        return Array.AsReadOnly(plans.ToArray());
    }

    internal void IssueMouLuSuPlayedDeclaration(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.PlayedDeclaration, Played: { Source: { } source, Cards: [var cardId] } plan, Response: null, ActionId: 0 } r || !MouLuSuPlayedPlans(f).Any(p => SameMouLuSuPlayedPlan(p, plan)) || _corePlayCardHistoryOperations.ViewAsRule(source)?.DeclarationValidation is null || _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(r.Other, cardId)is not null)
            throw new InvalidOperationException("Mengmou requires its original current declared red Basic choice.");
        if (!_coreCardDeclarationsOperations.TryBeginCardDeclaration(_coreMatchParticipantsPlayersAccess._players[r.Other], _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), plan.Kind, source, _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.MouLuSuAnniversaryPlayed, r.Other, target: f.OwnerSeat, finalKind: plan.Kind), []))
            throw new InvalidOperationException("Mengmou cannot bypass its required native declaration challenge.");
    }

    internal bool MatchesMouLuSuPlayedDeclaration(ProgramSkillFrame f, CardDeclarationFrame d)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.PlayedDeclaration, Played: { Source: { } source, Cards: [var cardId] } plan, Response: null, ActionId: 0 } r || d.OwnerSeat != r.Other || d.TurnNumber != r.Turn || d.Return is not { Purpose: CardDeclarationPurpose.MouLuSuAnniversaryPlayed, UseAction: null } ret || ret.ParentFrameId != f.Id || ret.ParentKind != f.Kind || ret.ParentStep != f.Step || ret.ActorSeat != r.Other || ret.TargetSeat != f.OwnerSeat || ret.FinalEffectiveKind != plan.Kind || d.TargetSeats.Count != 0 || d.Payment is not { Claimed: false } p || p.DeclarationId != d.Id || p.OwnerFrameId != d.Id || p.ProviderSeat != r.Other || p.ActorSeat != r.Other || p.Cost.CardId != cardId || p.Cost.From != CardLocation.Hand(r.Other) || p.Source != source || source.OwnerSeat != r.Other || _corePlayCardHistoryOperations.ViewAsRule(source)is not { DeclarationValidation: not null } rule || rule.OutputKind != p.DeclaredKind || !CoreNativeCardClassificationsRuntime.MatchesRequiredCard(p.DeclaredKind, plan.Kind))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardDeclarationCommittedEvent>().Count(e => e.DeclarationId == d.Id && e.OwnerSeat == r.Other && e.ActorSeat == r.Other && e.DeclaredKind == p.DeclaredKind && e.TargetSeats.Count == 0) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.CardId == cardId && m.From == p.Cost.From && m.To == CardLocation.Processing && m.Reason.Value == "conversion.declaration.pay") == 1;
    }

    internal bool ReturnMouLuSuPlayedDeclaration(CardDeclarationFrame d, bool succeeded)
    {
        if (d.Return.Purpose != CardDeclarationPurpose.MouLuSuAnniversaryPlayed)
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(x => x.Id == d.Id) || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != d.Return.ParentFrameId || d.Stage != CardDeclarationStage.Returning || !MatchesMouLuSuPlayedDeclaration(f, d) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuSuAnniversaryDeclarationReturnedEvent>().Any(e => e.DeclarationId == d.Id))
            throw new InvalidOperationException("Mengmou declaration returned to another or already completed native request.");
        if (succeeded)
        {
            if (d.Succeeded != true || _cardZonesOperations._cardZones.GetLocation(d.Payment.Cost.CardId) != CardLocation.Processing || f.AcceptedDeclarationPayment is { Claimed: false })
                throw new InvalidOperationException("Mengmou cannot forge a passed declaration or repay an outstanding declaration entity.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = f with { AcceptedDeclarationPayment = d.Payment with { OwnerFrameId = f.Id } });
        }

        MouLuSuAnniversary.DeclarationReturned(f, d.Id, succeeded);
        return true;
    }

    internal bool MouLuSuPlayedDeclarationEdge(int i)
    {
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not CardDeclarationFrame d || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == d.Return.ParentFrameId)is not ProgramSkillFrame f || !MatchesMouLuSuPlayedDeclaration(f, d))
            return false;
        if (_resolutionStackOperations._resolutionStack[i] is CardDeclarationChallengeFrame c)
            return d.Stage == CardDeclarationStage.Challenging && c.ParentFrameId == d.Id && c.SeatIndex >= 0 && c.SeatIndex < c.EligibleSeats.Count && c.EligibleSeats.Distinct().Count() == c.EligibleSeats.Count && c.EligibleSeats.All(s => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && s != d.OwnerSeat);
        if (_resolutionStackOperations._resolutionStack[i] is CardsMovedTriggerWindowFrame m)
            return d.Stage is CardDeclarationStage.Paying or CardDeclarationStage.Cleaning && d.ActiveChildFrameId == m.Id && m.ResumeDeclarationFrameId == d.Id && m.Batch.ParentFrameId == d.Id && m.Batch.Movements is [var move] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.CardId == d.Payment.Cost.CardId && (d.Stage == CardDeclarationStage.Paying ? move.From == d.Payment.Cost.From && move.To == CardLocation.Processing && move.Reason.Value == "conversion.declaration.pay" : move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason.Value is "conversion.declaration.failed" or "conversion.declaration.cancelled");
        return _resolutionStackOperations._resolutionStack[i] is ProgramLifecycleTriggerWindowFrame skills && d.Stage == CardDeclarationStage.Granting && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == d.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeCardDeclaration;
    }

    internal static bool SameMouLuSuPlayedPlan(MouLuSuAnniversaryPlayedPlan a, MouLuSuAnniversaryPlayedPlan b) => a.Kind == b.Kind && a.Source == b.Source && a.Zhangba == b.Zhangba && a.Cards.SequenceEqual(b.Cards);
    internal void IssueMouLuSuPlayed(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.PlayedAccepted, Played: { } plan, Response: null, ActionId: 0 } r || !MouLuSuPlayedPlans(f).Any(p => SameMouLuSuPlayedPlan(p, plan)))
            throw new InvalidOperationException("Mengmou lost its exact currently legal Played Slash.");
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Other];
        CardActionContext action;
        if (plan.Source is { } multiSource && plan.Cards.Count > 1)
        {
            var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, plan.Cards, plan.Kind, true, multiSource) ?? throw new InvalidOperationException("Mengmou lost its original multi-card response materials.");
            action = _coreCardConversionsOperations.MoveProgramMultiCardResponse(actor, selection, f.Id, f.OwnerSeat);
        }
        else
        {
            var cards = plan.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
            var costs = cards.Select(c => _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, c.Id)?.Cost ?? new CardActionCost(c.Id, c.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, c), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, c))).ToArray();
            var preparedSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(actor, cards, plan.Kind);
            var suit = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, cards[0].Id)?.FrozenSuit ?? preparedSuit;
            _characterZhangFeiWumiaoOperations.PrepareWuZhangFeiResponseRanks(actor, plan.Kind, cards, plan.Source);
            _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(actor, plan.Kind, cards);
            _characterGongsunZanMouMobileOperations.PrepareMouGszHuResponse(actor, cards, plan.Kind, plan.Source, null);
            if (cards is [var one])
                _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(actor, one, plan.Kind, plan.Source);
            if (plan.Source is { } source)
            {
                _coreLastZoneConversionsOperations.ValidateLastZoneConversionCommit(source, actor, cards, true);
                _coreOwnPublicPileConversionsOperations.ValidateOwnPublicPileConversionCommit(source, actor, cards, true, plan.Kind);
                _corePlayCardHistoryOperations.ConsumeProgramViewAsUsage([source]);
            }

            foreach (var card in cards)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(card.Id, actor.Seat, f.OwnerSeat, plan.Kind));
            }

            action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, null, CardActionType.Response, actor.Seat, actor.Seat, null, actor.Seat, f.OwnerSeat, plan.Kind, [], costs, plan.Source is { } conversion ? [conversion] : [], effectiveSuit: suit, effectiveIsRed: _coreActionDiscardColorsOperations.CaptureActionColor(costs, suit)));
            _characterZhangFeiWumiaoOperations.BindWuZhangFeiResponseRanks(action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
            if (plan.Source is { } converted)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(f.Id, converted.SkillId, converted.BindingId, actor.Seat, plan.Cards, plan.Kind, false, []));
        }

        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, plan.Kind);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuSuAnniversary = f.MouLuSuAnniversary!with { Response = action, ActionId = action.ActionId, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryPlayedEvent(f.Id, r.Index, action.ActionId, false));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseAccepted, [f.OwnerSeat], null))
            FinishMouLuSuPlayedAccepted(f);
    }
    internal bool IsMouLuSuPlayedWindow(CardActionContext action, SkillProgramTriggerWindow window) => window is SkillProgramTriggerWindow.CardResponseAccepted or SkillProgramTriggerWindow.CardResponseCompleted && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouLuSuAnniversary: { Response: { } accepted } r } f && MouLuSuAnniversary.Valid(f) && action.ActionId == accepted.ActionId && action.Type == CardActionType.Response && action.ActorSeat == r.Other && action.ProviderSeat == r.Other && action.RequesterSeat is null && action.TargetSeats.Count == 0 && CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && r.ActionId == action.ActionId && r.Stage == (window == SkillProgramTriggerWindow.CardResponseAccepted ? MouLuSuAnniversaryStage.PlayedAccepted : MouLuSuAnniversaryStage.PlayedCompleted);
    internal MouLuSuAnniversaryPlayedReturn? MouLuSuPlayedWindowReturn(CardActionContext action, SkillProgramTriggerWindow window) => IsMouLuSuPlayedWindow(action, window) ? new(_resolutionStackOperations._resolutionStack[^1].Id, action.ActionId, window) : null;
    internal bool TryReturnMouLuSuPlayedWindow(ProgramCardTriggerWindowFrame window)
    {
        if (window.MouLuSuAnniversaryPlayed is not { } returned)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuSuAnniversary: { } r } f || f.Id != returned.FrameId || window.ParentFrameId != f.Id || window.Action.ActionId != returned.ActionId || r.ActionId != returned.ActionId || !IsMouLuSuPlayedWindow(window.Action, returned.Window))
            throw new InvalidOperationException("Mengmou response observer returned to another native Played receipt.");
        if (returned.Window == SkillProgramTriggerWindow.CardResponseAccepted)
            FinishMouLuSuPlayedAccepted(f);
        else
            FinishMouLuSuPlayedCompleted(f);
        return true;
    }

    internal void FinishMouLuSuPlayedAccepted(ProgramSkillFrame f)
    {
        var r = f.MouLuSuAnniversary!;
        var action = r.Response!;
        if (!IsMouLuSuPlayedWindow(action, SkillProgramTriggerWindow.CardResponseAccepted))
            throw new InvalidOperationException("Mengmou must complete its exact accepted response before disposing it.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuSuAnniversary = r with { Stage = MouLuSuAnniversaryStage.PlayedCostChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var cost in action.PhysicalCards)
        {
            if (_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId) || _coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, cost.CardId) || _characterMiHengLeOperations.TryGetSiruiDestroyedResponseCost(action, cost.CardId, out _))
                continue;
            if (_cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing)
                throw new InvalidOperationException("Mengmou cannot silently dispose a different response entity.");
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuSuAnniversary = f.MouLuSuAnniversary!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        MouLuSuAnniversary.Continue(f.Id);
    }

    internal void BeginMouLuSuPlayedCompleted(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.PlayedCostChildren, Response: { } action } r || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Mengmou must drain its actual Played payment before the Completed window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuSuAnniversary = r with { Stage = MouLuSuAnniversaryStage.PlayedCompleted } });
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseCompleted, [f.OwnerSeat], null))
            FinishMouLuSuPlayedCompleted(f);
    }

    internal void FinishMouLuSuPlayedCompleted(ProgramSkillFrame f)
    {
        var r = f.MouLuSuAnniversary!;
        if (!IsMouLuSuPlayedWindow(r.Response!, SkillProgramTriggerWindow.CardResponseCompleted) || r.Played is not { } plan)
            throw new InvalidOperationException("Mengmou cannot repeat its native Played completion.");
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuSuAnniversary = f.MouLuSuAnniversary!with { Stage = MouLuSuAnniversaryStage.PlayedChildren, Index = r.Index + 1, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryPlayedEvent(f.Id, r.Index, r.ActionId, true));
        MouLuSuAnniversary.Continue(f.Id);
    }
}
