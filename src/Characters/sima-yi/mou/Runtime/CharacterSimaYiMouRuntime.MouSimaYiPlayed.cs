#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiMouRuntime
{
    internal bool IsMouSmPlayedActor(CharacterState actor) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouSimaYi: { Operation: SkillProgramEffectOp.ResolveMouSimaYiPingliao, Stage: MouSmStage.Respondent or MouSmStage.PlayedAccepted or MouSmStage.PlayedDeclaration } r } f && r.Actor == actor.Seat && MouSimaYi.Valid(f);
    internal NativePublicPileCardRequest? MouSmPublicPileRequest(CharacterState actor, CardKind kind) => IsMouSmPlayedActor(actor) && CardCatalog.Get(kind).CategoryName == "基本牌" ? new(_resolutionStackOperations._resolutionStack[^1].Id, _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PromptId, actor.Seat, actor.Seat, null, actor.Seat, kind, null, NativePublicPileDirection.Played)
    {
        RequestCursor = ((ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1]).MouSimaYi!.Cursor
    }

    : null;
    internal bool MouSmPlanIsRed(CharacterState actor, MouSmPlayedPlan plan)
    {
        var cards = plan.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
        if (cards.Length == 0)
            return false;
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, cards[0], plan.Source);
        var suit = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, cards[0].Id)?.FrozenSuit ?? _corePlayCardHistoryOperations.CaptureUsedCardSuit(actor.Seat, plan.Cards, appearance);
        var costs = cards.Select(c => _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, c.Id)?.Cost ?? new CardActionCost(c.Id, c.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, c), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, c))).ToArray();
        return _coreActionDiscardColorsOperations.CaptureActionColor(costs, suit) == true;
    }

    internal IReadOnlyList<MouSmPlayedPlan> MouSmPlayedPlans(ProgramSkillFrame f)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Operation: SkillProgramEffectOp.ResolveMouSimaYiPingliao } r || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || !_coreMatchParticipantsPlayersAccess._players[r.Actor].IsAlive)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        var plans = new List<MouSmPlayedPlan>();
        foreach (var required in new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.Dodge,
            CardKind.Peach,
            CardKind.Alcohol
        }

        )
        {
            foreach (var card in _coreNativeResponseMaterialQueriesOperations.GetResponseCards(actor, required).Where(c => _coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(actor, c, required)))
            {
                var kind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(actor, card, required);
                var identity = _coreCardConversionsOperations.HasProgramCardIdentity(actor, card);
                if (!identity && CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, required) && (!_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(actor, card) || _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(actor, card, kind, true)))
                    plans.Add(new(kind, null, false) { Cards = [card.Id] });
                foreach (var source in identity ? _coreCardConversionsOperations.GetProgramCardIdentitySources(actor, card, kind, true) : _coreActionQueriesOperations.GetProgramViewAsConversions(actor, card, kind, true))
                    plans.Add(new(kind, source, false) { Cards = [card.Id] });
            }

            plans.AddRange(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, required, true).Where(p => !_characterGuanHaiOlOperations.IsXieweiSelection(p)).Select(p => new MouSmPlayedPlan(p.OutputKind, p.Source, false) { Cards = p.Cards.Select(c => c.Id).ToArray() }));
        }

        plans.AddRange(_coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(actor).Select(p => new MouSmPlayedPlan(CardKind.Slash, null, true) { Cards = p.Select(c => c.Id).ToArray() }));
        return Array.AsReadOnly(plans.Where(p => MouSmPlanIsRed(actor, p)).DistinctBy(p => (p.Kind, p.Source, p.Zhangba, string.Join(",", p.Cards))).ToArray());
    }

    internal static bool SameMouSmPlayedPlan(MouSmPlayedPlan a, MouSmPlayedPlan b) => a.Kind == b.Kind && a.Source == b.Source && a.Zhangba == b.Zhangba && a.Cards.SequenceEqual(b.Cards);
    internal void IssueMouSmPlayed(ProgramSkillFrame f)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Stage: MouSmStage.PlayedAccepted, Played: { } plan, Response: null, ResponseActionId: 0 } r || !MouSmPlayedPlans(f).Any(p => SameMouSmPlayedPlan(p, plan)))
            throw new InvalidOperationException("Pingliao lost its exact currently legal Played Slash.");
        var actor = _coreMatchParticipantsPlayersAccess._players[r.Actor];
        CardActionContext action;
        if (plan.Source is { } multiSource && plan.Cards.Count > 1)
        {
            var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, plan.Cards, plan.Kind, true, multiSource) ?? throw new InvalidOperationException("Pingliao lost its original multi-card response materials.");
            action = _coreCardConversionsOperations.MoveProgramMultiCardResponse(actor, selection, f.Id, f.OwnerSeat);
        }
        else
        {
            var cards = plan.Cards.Select(_coreAdvancedLifecycleOperations.GetAdvancedCard).ToArray();
            var costs = cards.Select(c => _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, c.Id)?.Cost ?? new CardActionCost(c.Id, c.Kind, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, c), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actor.Seat, c))).ToArray();
            var preparedSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(actor, cards, plan.Kind);
            var suit = _coreCardDeclarationsOperations.UnclaimedDeclarationPayment(actor.Seat, cards[0].Id)?.FrozenSuit ?? preparedSuit ?? _corePlayCardHistoryOperations.CaptureUsedCardSuit(actor.Seat, plan.Cards, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, cards[0], plan.Source));
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
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSimaYi = f.MouSimaYi!with { Response = action, ResponseActionId = action.ActionId, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSmPlayedEvent(f.Id, r.Cursor, r.Actor, action.ActionId, false));
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseAccepted, [f.OwnerSeat], null))
            FinishMouSmPlayedAccepted(f);
    }

    internal static bool SameMouSmAction(CardActionContext a, CardActionContext b) => a.ActionId == b.ActionId && a.ParentActionId == b.ParentActionId && a.Type == b.Type && a.ActorSeat == b.ActorSeat && a.ProviderSeat == b.ProviderSeat && a.RequesterSeat == b.RequesterSeat && a.ResponderSeat == b.ResponderSeat && a.OpponentSeat == b.OpponentSeat && a.EffectiveKind == b.EffectiveKind && a.EffectiveSuit == b.EffectiveSuit && a.EffectiveRank == b.EffectiveRank && a.EffectiveIsRed == b.EffectiveIsRed && a.FactionOrigin == b.FactionOrigin && a.TargetSeats.SequenceEqual(b.TargetSeats) && (a.DesignatedTargetSeats is null ? b.DesignatedTargetSeats is null : b.DesignatedTargetSeats is { } t && a.DesignatedTargetSeats.SequenceEqual(t)) && a.PhysicalCards.SequenceEqual(b.PhysicalCards) && a.ConversionChain.SequenceEqual(b.ConversionChain);
    internal bool IsMouSmPlayedWindow(CardActionContext action, SkillProgramTriggerWindow window) => window is SkillProgramTriggerWindow.CardResponseAccepted or SkillProgramTriggerWindow.CardResponseCompleted && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MouSimaYi: { Response: { } accepted } r } f && MouSimaYi.Valid(f) && action.ActionId == accepted.ActionId && action.Type == CardActionType.Response && action.ActorSeat == r.Actor && action.ProviderSeat == r.Actor && action.RequesterSeat is null && action.TargetSeats.Count == 0 && CardCatalog.Get(action.EffectiveKind).CategoryName == "基本牌" && action.EffectiveIsRed == true && r.ResponseActionId == action.ActionId && r.Stage == (window == SkillProgramTriggerWindow.CardResponseAccepted ? MouSmStage.PlayedAccepted : MouSmStage.PlayedCompleted);
    internal MouSmPlayedReturn? MouSmPlayedWindowReturn(CardActionContext action, SkillProgramTriggerWindow window) => IsMouSmPlayedWindow(action, window) ? new(_resolutionStackOperations._resolutionStack[^1].Id, action.ActionId, window) : null;
    internal bool ValidMouSmPlayedWindow(ProgramCardTriggerWindowFrame window) => window.MouSimaYiPlayed is { } back && window.Continuation is null && window.CompletedResponseReturn is null && window.AttackOwnerFrameId is null && window.ParentFrameId == back.FrameId && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == back.FrameId)is ProgramSkillFrame { MouSimaYi: { Response: { } response, Played: { } plan } r } f && MouSimaYi.Valid(f) && r.ResponseActionId == back.ActionId && window.Action.ActionId == back.ActionId && SameMouSmAction(response, window.Action) && window.Action.Type == CardActionType.Response && window.Action.ActorSeat == r.Actor && window.Action.ProviderSeat == r.Actor && window.Action.RequesterSeat is null && window.Action.TargetSeats.Count == 0 && window.Action.PhysicalCards.Select(c => c.CardId).SequenceEqual(plan.Cards) && back.Window == (r.Stage == MouSmStage.PlayedAccepted ? SkillProgramTriggerWindow.CardResponseAccepted : r.Stage == MouSmStage.PlayedCompleted ? SkillProgramTriggerWindow.CardResponseCompleted : (SkillProgramTriggerWindow)(-1)) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouSmPlayedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.ActionId == back.ActionId && e.Actor == r.Actor && !e.Completed) == 1;
    internal bool TryReturnMouSmPlayedWindow(ProgramCardTriggerWindowFrame window)
    {
        if (window.MouSimaYiPlayed is not { } returned)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouSimaYi: { } r } f || f.Id != returned.FrameId || window.ParentFrameId != f.Id || window.Action.ActionId != returned.ActionId || r.ResponseActionId != returned.ActionId || !IsMouSmPlayedWindow(window.Action, returned.Window))
            throw new InvalidOperationException("Pingliao response observer returned to another native Played receipt.");
        if (returned.Window == SkillProgramTriggerWindow.CardResponseAccepted)
            FinishMouSmPlayedAccepted(f);
        else
            FinishMouSmPlayedCompleted(f);
        return true;
    }

    internal void FinishMouSmPlayedAccepted(ProgramSkillFrame f)
    {
        var r = f.MouSimaYi!;
        var action = r.Response!;
        if (!IsMouSmPlayedWindow(action, SkillProgramTriggerWindow.CardResponseAccepted))
            throw new InvalidOperationException("Pingliao must complete its exact accepted response before disposing it.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSimaYi = r with { Stage = MouSmStage.PlayedCostChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var cost in action.PhysicalCards)
        {
            if (_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId) || _coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, cost.CardId) || _characterMiHengLeOperations.TryGetSiruiDestroyedResponseCost(action, cost.CardId, out _))
                continue;
            if (_cardZonesOperations._cardZones.GetLocation(cost.CardId) != CardLocation.Processing)
                throw new InvalidOperationException("Pingliao cannot silently dispose a different response entity.");
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouSimaYi = f.MouSimaYi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        MouSimaYi.Continue(f.Id);
    }

    internal void BeginMouSmPlayedCompleted(ProgramSkillFrame f)
    {
        if (!MouSimaYi.Valid(f) || f.MouSimaYi is not { Stage: MouSmStage.PlayedCostChildren, Response: { } action } r || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Pingliao must drain its actual Played payment before the Completed window.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouSimaYi = r with { Stage = MouSmStage.PlayedCompleted } });
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseCompleted, [f.OwnerSeat], null))
            FinishMouSmPlayedCompleted(f);
    }

    internal void FinishMouSmPlayedCompleted(ProgramSkillFrame f)
    {
        var r = f.MouSimaYi!;
        if (!IsMouSmPlayedWindow(r.Response!, SkillProgramTriggerWindow.CardResponseCompleted))
            throw new InvalidOperationException("Pingliao cannot complete another Played action.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouSmPlayedEvent(f.Id, r.Cursor, r.Actor, r.ResponseActionId, true));
        MouSimaYi.PlayedCompleted(f);
    }
}
