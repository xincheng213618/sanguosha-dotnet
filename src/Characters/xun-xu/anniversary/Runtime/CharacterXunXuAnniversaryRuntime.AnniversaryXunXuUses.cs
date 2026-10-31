#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXunXuAnniversaryRuntime
{
    internal const string XunXuPlayPrefix = "anniversary-boqia.";
    internal bool IsAnniversaryXunXuBoqiaSource(CardConversionSource? s) => s is { SkillId: AnniversaryXunXuComposition.Boqia } && AnniversaryXunXuComposition.IsBoqiaRuleId(s.BindingId) && _corePlayCardHistoryOperations.ViewAsRule(s)is { InputCount: 3, ExtendedUse: true };
    internal bool XunXuAllowance(int owner, CardKind kind)
    {
        var uses = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXunXuUseEvent>().Where(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber).ToArray();
        if (uses.Any(e => e.Name == CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(kind)))
            return false;
        return uses.Length == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXunXuRearmedEvent>().Any(e => e.ActionId == uses[^1].ActionId && e.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber);
    }

    internal bool XunXuUseDirection(CharacterState owner, CardKind kind, bool response, bool dying) => !response || kind == CardKind.Nullification && _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null || kind == CardKind.Dodge && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, kind) || kind is CardKind.Peach or CardKind.Alcohol && dying && _coreDyingCursorRuntimeOperations.ActiveDying?.ResponderSeat == owner.Seat;
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 46100)]
    internal bool BanUnavailableXunXuConversion(ProgramViewAsLegalityQuery q) => q.Source.SkillId == AnniversaryXunXuComposition.Boqia && (!IsAnniversaryXunXuBoqiaSource(q.Source) || q.Source.OwnerSeat != q.Owner.Seat || q.CardIds.Count != 3 || q.CardIds.Distinct().Count() != 3 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(q.Owner, q.Source.SkillId, q.Source.SkillInstanceId) || !XunXuAllowance(q.Owner.Seat, q.Kind) || !XunXuUseDirection(q.Owner, q.Kind, q.ForResponse, q.DyingUse) || q.CardIds.Any(id => id <= 0 || _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != q.Owner.Seat || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)));
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 46100)]
    internal void RecordActualXunXuUse(IGameEvent payload)
    {
        CardActionContext action;
        if (payload is CardUseDeclaredEvent declared && _coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is { Action: { } a })
            action = a;
        else if (payload is CardActionAcceptedEvent accepted)
            action = accepted.Action;
        else
            return;
        var sources = action.ConversionChain.Where(s => s.SkillId == AnniversaryXunXuComposition.Boqia).ToArray();
        if (sources.Length == 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXunXuUseEvent>().Any(e => e.ActionId == action.ActionId))
            return;
        if (sources is not [var source] || source.OwnerSeat != action.ActorSeat || source.OwnerSeat != action.ProviderSeat || action.RequesterSeat is not null || !IsAnniversaryXunXuBoqiaSource(source) || action.PhysicalCards.Count != 3 || action.PhysicalCards.Select(c => c.CardId).Distinct().Count() != 3 || action.PhysicalCards.Any(c => c.From.OwnerSeat != source.OwnerSeat || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || action.EffectiveKind != _corePlayCardHistoryOperations.ViewAsRule(source)!.OutputKind || action.Type == CardActionType.Response && !XunXuUseDirection(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], action.EffectiveKind, true, _coreDyingCursorRuntimeOperations.ActiveDying?.ResponderSeat == source.OwnerSeat))
            throw new InvalidOperationException("Boqia requires a true native Use, its accepted three HE entities, owner and exact conversion source.");
        if (!XunXuAllowance(source.OwnerSeat, action.EffectiveKind))
            throw new InvalidOperationException("Boqia cannot reuse this actual-turn allowance or card name.");
        var cards = action.PhysicalCards.Select(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId)).ToArray();
        var reward = cards.Select(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], c)).Distinct().Count() == 3 || cards.Select(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreWenhePoisonOperations.EffectiveConversionInputKind(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], c))).Distinct().Count() == 3;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AnniversaryXunXuUseEvent(source, action.ActionId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, CoreGameEngineAlternativeViewAsCostsRuntime.ProgramBasicCardName(action.EffectiveKind), reward));
    }

    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 46100)]
    internal IEnumerable<LegalAction> XunXuPlayActions(CharacterState owner)
    {
        if (!owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat != owner.Seat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !_coreNationalSkillsOperations.HasRuntimeSkill(owner, AnniversaryXunXuComposition.Boqia))
            yield break;
        foreach (var kind in AnniversaryXunXuComposition.Kinds.Where(k => k is not (CardKind.Dodge or CardKind.Nullification)))
            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, kind, false).Where(s => IsAnniversaryXunXuBoqiaSource(s.Source)))
            {
                var ids = selection.Cards.Select(c => c.Id).Order().ToArray();
                foreach (var plan in XunXuPlayPlans(owner, selection))
                {
                    var key = XunXuPlayPrefix + selection.Source.BindingId + "." + string.Join('-', ids) + "." + string.Join('-', plan.Targets) + "." + (plan.Card?.ToString() ?? "none");
                    yield return new(LegalActionKind.UseProgramSkill, null, null, _coreCardConversionsOperations.DescribeConversion(selection.Source, "将三张牌当【" + CardCatalog.Get(kind).DisplayName + "】使用"), kind, MinCardCount: 3, MaxCardCount: 3, MinTargetCount: plan.Targets.Count, MaxTargetCount: plan.Targets.Count)
                    {
                        ProgramSkillId = selection.Source.SkillId,
                        ProgramActivationId = key,
                        ConversionSource = selection.Source,
                        SelectableCardIds = Array.AsReadOnly(ids),
                        SelectableTargetSeats = Array.AsReadOnly(plan.Targets.ToArray()),
                        TargetCardId = plan.Card,
                        ProgramAiHint = new(0, kind == CardKind.Peach ? 1 : 0, 0, 0, 0, CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? 1 : 0, false, false)
                    };
                }
            }
    }

    internal IReadOnlyList<GameEngine.XunXuUsePlan> XunXuPlayPlans(CharacterState owner, ProgramMultiCardViewAsSelection s)
    {
        var kind = s.OutputKind;
        var ids = s.Cards.Select(c => c.Id).ToArray();
        var suit = _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(owner, s);
        var color = _coreGuanHaiBridgesOperations.ProgramMultiCardColor(owner, s);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, p, kind, physicalSuit: suit, effectiveColor: color, physicalCardIds: ids, conversionSource: s.Source) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(owner.Seat, p.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, p.Seat, suit)).Select(p => new GameEngine.XunXuUsePlan(LegalActionKind.Slash, [p.Seat])).ToArray();
        if (kind == CardKind.Peach)
            return _coreTreatedWoundedOperations.IsRuleWounded(owner) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit) ? [new(LegalActionKind.Peach, [owner.Seat])] : [];
        if (kind == CardKind.Alcohol)
            return !owner.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(owner, null) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit) ? [new(LegalActionKind.Alcohol, [])] : [];
        return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(owner, kind, suit, enforceUsePermission: true, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: ids, conversionSource: s.Source).Select(p => new GameEngine.XunXuUsePlan(p.ActionKind, p.TargetSeats, p.TargetCardId, p.RequiredCardKind)).ToArray();
    }

    [ProgramPipelineHook(ProgramPipelinePhase.FilterPlayPhaseActions, 46100)]
    internal IReadOnlyList<LegalAction> FilterXunXuNativeDuplicates(CharacterState actor, IReadOnlyList<LegalAction> actions) => actions.Where(a => a.ConversionSource?.SkillId != AnniversaryXunXuComposition.Boqia || a.ProgramActivationId?.StartsWith(XunXuPlayPrefix, StringComparison.Ordinal) == true).ToArray();
    internal CommandError? ValidateXunXuPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, ids, targets)is { } invalid)
            return invalid;
        if (action.ConversionSource is not { } source || action.PlayedCardKind is not { } kind || !IsAnniversaryXunXuBoqiaSource(source) || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, ids, kind, false, source)is not { } selection)
            return new(CommandErrorCode.InvalidCard, "博洽的原生牌名、三张实际材料或本回合额度已变化。");
        return XunXuPlayPlans(owner, selection).Any(p => p.Targets.SequenceEqual(targets) && p.Card == action.TargetCardId) ? null : new(CommandErrorCode.InvalidTarget, "博洽仍须满足原生目标、距离、次数与禁止使用限制。");
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 46100)]
    internal bool SubmitXunXuPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(XunXuPlayPrefix, StringComparison.Ordinal))
            return false;
        var invalid = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (invalid is not null)
        {
            result = _kernelCoreMainOperations.Reject(invalid.Code, invalid.Message);
            return true;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = XunXuPlayActions(owner).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (command.SkillOwnerSeat is not null || action is null)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "博洽的实际使用菜单已失效。");
            return true;
        }

        if (ValidateXunXuPlay(owner, action, command.CardIds, command.TargetSeats)is { } error)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            ExecuteXunXuPlay(owner, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 46100)]
    internal bool ExecuteXunXuPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (action.ProgramActivationId?.StartsWith(XunXuPlayPrefix, StringComparison.Ordinal) != true || action.ConversionSource is not { } source || !IsAnniversaryXunXuBoqiaSource(source))
            return false;
        if (ValidateXunXuPlay(owner, action, ids, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var kind = action.PlayedCardKind!.Value;
        var s = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, ids, kind, false, source)!;
        var plan = XunXuPlayPlans(owner, s).Single(p => p.Card == action.TargetCardId && p.Targets.SequenceEqual(targets));
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(owner, _coreMatchParticipantsPlayersAccess._players[targets[0]], s.Cards[0], kind, owner.Seat, physicalCards: s.Cards, conversionSource: source);
        else if (kind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, s.Cards[0], "桃", kind, conversionSource: source, physicalCards: s.Cards);
        else
        {
            var use = _coreNativeCardUseOperations.BeginCardUse(s.Cards[0], owner.Seat, targets, kind, physicalCardIds: ids, conversionSource: source);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(use, source.SkillId, source.BindingId, owner.Seat, ids, kind, true, targets));
            if (kind == CardKind.Alcohol)
            {
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(use, owner);
                _coreCardUseLifecycleOperations.BeginSimpleCardUse(use, new(s.Cards[0].Id, SimpleCardUseEffect.Alcohol));
            }
            else
                _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(use, s.Cards[0], owner.Seat, targets, plan.Action, plan.Card, plan.Required, kind);
        }

        return true;
    }

    internal bool TryResolveAnniversaryXunXuDyingAlcohol(CharacterState owner, ProgramMultiCardViewAsSelection s)
    {
        if (s.OutputKind != CardKind.Alcohol || !IsAnniversaryXunXuBoqiaSource(s.Source))
            return false;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } d || d.VictimSeat != owner.Seat || d.ResponderSeat != owner.Seat || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, s.Cards.Select(c => c.Id).ToArray(), CardKind.Alcohol, true, s.Source)is null || _coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat))
            throw new InvalidOperationException("Boqia Alcohol requires its native current self-rescue and three real HE materials.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, s.Cards[0], "酒", CardKind.Alcohol, conversionSource: s.Source, dyingResponse: new(d.FrameId, owner.Seat, false, null, true, s.Cards[0].Id), physicalCards: s.Cards);
        return true;
    }

    internal bool TryFinishAnniversaryXunXuAlcoholCost(CardUseFrame use, Card card)
    {
        if (use.CardKind != CardKind.Alcohol || use.CardId != card.Id)
            return false;
        return TryFinishAnniversaryXunXuUseMaterials(use);
    }

    internal bool TryFinishAnniversaryXunXuUseMaterials(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ConversionChain.SingleOrDefault(s => s.SkillId == AnniversaryXunXuComposition.Boqia)is not { } source || !IsAnniversaryXunXuBoqiaSource(source))
            return false;
        if (use.PhysicalCardIds is not { Count: 3 } ids || !ids.SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || ids.Any(id => id <= 0) || action.ActorSeat != source.OwnerSeat || action.ProviderSeat != source.OwnerSeat || action.EffectiveKind != use.CardKind || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXunXuUseEvent>().Count(e => e.ActionId == action.ActionId && e.Source == source) != 1)
            throw new InvalidOperationException("Boqia lost its exact accepted three-material native Use before final payment cleanup.");
        var remaining = ids.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_characterXinPiOlOperations.IsXinPiPhysicalClaim(use.Id, id) && !_characterHuaManOlOperations.IsHuaManClaimedEntity(use.Id, id) && !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(use.Id, id) && !_characterXuRongOlOperations.IsXuRongCausalEntityClaim(use.Id, id)).ToArray();
        if (remaining.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(remaining, CardLocation.DiscardPile, CardMoveReasons.UseFinished, disclosure: CardMovementDisclosure.FaceUp);
        return true;
    }

    internal bool TryFinishAnniversaryXunXuAcceptedResponse(CardActionContext action)
    {
        var sources = action.ConversionChain.Where(s => s.SkillId == AnniversaryXunXuComposition.Boqia).ToArray();
        if (sources.Length == 0)
            return false;
        if (action.Type != CardActionType.Response || sources is not [var source] || !IsAnniversaryXunXuBoqiaSource(source) || action.PhysicalCards.Count != 3 || action.ActorSeat != source.OwnerSeat || action.ProviderSeat != source.OwnerSeat || action.RequesterSeat is not null)
            throw new InvalidOperationException("Boqia accepted-response cleanup must retain its native owner and exact three-entity source.");
        return TryFinishAnniversaryXunXuResponseMaterials(new(action.PhysicalCards.Select(c => _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId)).ToArray(), source, action.EffectiveKind));
    }

    internal bool TryFinishAnniversaryXunXuResponseMaterials(ProgramMultiCardViewAsSelection selection)
    {
        if (!IsAnniversaryXunXuBoqiaSource(selection.Source))
            return false;
        var ids = selection.Cards.Select(c => c.Id).ToArray();
        var action = _coreProgramEventHistoryOperations.ProgramEventHistory<CardActionAcceptedEvent>().Select(e => e.Action).LastOrDefault(a => a.Type == CardActionType.Response && a.ActorSeat == selection.Source.OwnerSeat && a.ProviderSeat == selection.Source.OwnerSeat && a.EffectiveKind == selection.OutputKind && a.ConversionChain.Contains(selection.Source) && a.PhysicalCards.Select(c => c.CardId).SequenceEqual(ids));
        if (ids.Length != 3 || ids.Any(id => id <= 0) || ids.Distinct().Count() != 3 || action is null || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryXunXuUseEvent>().Count(e => e.ActionId == action.ActionId && e.Source == selection.Source) != 1)
            throw new InvalidOperationException("Boqia response cleanup requires its once-accepted genuine three-material response Use.");
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == action.ParentActionId);
        var remaining = ids.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing && !_coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, id) && (parent is null || !_characterXinPiOlOperations.IsXinPiPhysicalClaim(parent.Id, id) && !_characterHuaManOlOperations.IsHuaManClaimedEntity(parent.Id, id) && !_coreCurrentUsePhysicalClaimsOperations.IsCurrentUsePhysicalCardClaim(parent.Id, id) && !_characterXuRongOlOperations.IsXuRongCausalEntityClaim(parent.Id, id))).ToArray();
        if (remaining.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(remaining, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished, disclosure: CardMovementDisclosure.FaceUp);
        return true;
    }
}
