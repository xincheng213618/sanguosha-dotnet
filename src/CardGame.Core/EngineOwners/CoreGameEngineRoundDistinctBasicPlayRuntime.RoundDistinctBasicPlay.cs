#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundDistinctBasicPlayRuntime
{
    internal const string RoundDistinctBasicPlayPrefix = "round-distinct-basic.";
    // The mature active-skill command already carries two cards. These are
    // conversion menus, not content activations or artificial ProgramSkillFrames.
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 20)]
    internal IEnumerable<LegalAction> BuildRoundDistinctBasicPlayActions(CharacterState owner)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.UsesRoundDistinctBasicUse || !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(owner))
            yield break;
        var selections = new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.ColdSlash,
            CardKind.Peach,
            CardKind.Alcohol
        }.SelectMany(kind => _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, kind, false)).Where(s => _corePlayCardHistoryOperations.ViewAsRule(s.Source)?.RoundDistinctBasicUse is not null).DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join('-', s.Cards.Select(c => c.Id).Order())));
        foreach (var selection in selections)
        {
            var kind = selection.OutputKind;
            var targets = RoundDistinctBasicPlayTargets(owner, selection);
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && targets.Count == 0 || kind == CardKind.Peach && targets.Count == 0 || kind == CardKind.Alcohol && !CanUseRoundDistinctBasicAlcohol(owner, selection.Cards))
                continue;
            var ids = Array.AsReadOnly(selection.Cards.Select(c => c.Id).Order().ToArray());
            var targetCount = kind == CardKind.Alcohol ? 0 : 1;
            yield return new LegalAction(LegalActionKind.UseProgramSkill, null, null, _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将【{(selection.Cards[0].DisplayName)}】和【{(selection.Cards[1].DisplayName)}】当【{(CardCatalog.Get(kind).DisplayName)}】使用"), kind, MinCardCount: 2, MaxCardCount: 2, MinTargetCount: targetCount, MaxTargetCount: targetCount)
            {
                ProgramSkillId = selection.Source.SkillId,
                ProgramActivationId = RoundDistinctBasicPlayPrefix + selection.Source.BindingId + "." + string.Join('-', ids),
                ConversionSource = selection.Source,
                SelectableCardIds = ids,
                SelectableTargetSeats = Array.AsReadOnly(targets.ToArray()),
                ProgramAiHint = new(0, kind == CardKind.Peach ? 1 : 0, 0, 0, 0, CoreNativeCardClassificationsRuntime.IsSlashCard(kind) ? 1 : 0, false, false)
                {
                    ValueAdjustment = kind == CardKind.Alcohol ? 10 : 0
                }
            };
        }
    }

    internal IReadOnlyList<int> RoundDistinctBasicPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        var kind = selection.OutputKind;
        var suit = _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, selection.Cards);
        var color = _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, selection.Cards);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreMatchParticipantsPlayersAccess._players.Where(target => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, target, kind, suit, effectiveColor: color, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray())).Select(target => target.Seat).Order().ToArray();
        if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(owner) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit))
            return[owner.Seat];
        return[];
    }

    internal bool CanUseRoundDistinctBasicAlcohol(CharacterState owner, IReadOnlyList<Card> cards) => (!owner.HasAlcoholEffect || _characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(owner, null)) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && (_characterDaQiaoXiaoQiaoHappyGodOperations.IsHappyQiaoShutuCardQuery(owner, null) || _characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(owner, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(owner.Seat, owner.Seat) || (_coreAdvancedPoliciesOperations.HasNextUnlimitedCard(owner) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(owner) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(owner)) || _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(owner, CardKind.Alcohol, cards.Select(c => c.Id).ToArray())) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Alcohol, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards), _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, cards)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, cards));
    internal CommandError? ValidateRoundDistinctBasicPlaySelection(CharacterState owner, LegalAction action, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, cards, targets)is { } error)
            return error;
        if (action.ConversionSource is not { } source || action.PlayedCardKind is not { } kind || _corePlayCardHistoryOperations.ViewAsRule(source)?.RoundDistinctBasicUse is null || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, cards, kind, false, source)is not { } selection)
            return new(CommandErrorCode.InvalidCard, "The pair conversion requires its exact currently legal same-color hand-like materials.");
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind == CardKind.Peach)
        {
            if (targets.Count != 1 || !RoundDistinctBasicPlayTargets(owner, selection).Contains(targets[0]))
                return new(CommandErrorCode.InvalidTarget, "The pair conversion target is no longer legal.");
        }
        else if (kind != CardKind.Alcohol || targets.Count != 0 || !CanUseRoundDistinctBasicAlcohol(owner, selection.Cards))
            return new(CommandErrorCode.InvalidTarget, "The pair Alcohol use is no longer legal.");
        return null;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 10)]
    internal bool TrySubmitRoundDistinctBasicPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(RoundDistinctBasicPlayPrefix, StringComparison.Ordinal) || _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(command.SkillId)?.Program?.ViewAs.Any(r => r.RoundDistinctBasicUse is not null) != true)
            return false;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = BuildRoundDistinctBasicPlayActions(owner).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (command.SkillOwnerSeat is not null || action is null)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The pair conversion menu is no longer available.");
            return true;
        }

        error = ValidateRoundDistinctBasicPlaySelection(owner, action, command.CardIds, command.TargetSeats);
        if (error is not null)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            TryExecuteRoundDistinctBasicPlay(owner, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 10)]
    internal bool TryExecuteRoundDistinctBasicPlay(CharacterState owner, LegalAction selected, IReadOnlyList<int> cards, IReadOnlyList<int> targets)
    {
        if (selected.ProgramActivationId?.StartsWith(RoundDistinctBasicPlayPrefix, StringComparison.Ordinal) != true || selected.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.RoundDistinctBasicUse is null)
            return false;
        var action = BuildRoundDistinctBasicPlayActions(owner).SingleOrDefault(a => a.ProgramSkillId == selected.ProgramSkillId && a.ProgramActivationId == selected.ProgramActivationId && a.ConversionSource == source) ?? throw new InvalidOperationException("The pair conversion menu lost its exact published source.");
        if (ValidateRoundDistinctBasicPlaySelection(owner, action, cards, targets)is { } error)
            throw new InvalidOperationException(error.Message);
        var kind = action.PlayedCardKind!.Value;
        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, cards, kind, false, source)!;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(owner, _coreMatchParticipantsPlayersAccess._players[targets[0]], selection.Cards[0], kind, owner.Seat, physicalCards: selection.Cards, conversionSource: source);
        else if (kind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, selection.Cards[0], "桃", CardKind.Peach, conversionSource: source, physicalCards: selection.Cards);
        else
        {
            var id = _coreNativeCardUseOperations.BeginCardUse(selection.Cards[0], owner.Seat, [], CardKind.Alcohol, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: source);
            foreach (var cost in selection.Cards)
                _corePhysicalMovementFacadesOperations.MoveCard(cost, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(owner, cost), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
            if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, owner.Seat))
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, owner);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(selection.Cards[0].Id, SimpleCardUseEffect.Alcohol));
        }

        return true;
    }

    internal bool TryResolveRoundDistinctBasicDyingResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        if (selection.OutputKind != CardKind.Alcohol || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.RoundDistinctBasicUse is null)
            return false;
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || dying.ResponderSeat != owner.Seat || dying.VictimSeat != owner.Seat || !owner.IsAlive || owner.Hp > 0 || _coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) || _coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, CardKind.Alcohol, CardActionType.Use))
            throw new InvalidOperationException("A pair Alcohol use requires its own actual dying response.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, owner, selection.Cards[0], "酒", CardKind.Alcohol, conversionSource: selection.Source, dyingResponse: new(dying.FrameId, owner.Seat, false, null, true, selection.Cards[0].Id), physicalCards: selection.Cards);
        return true;
    }

    internal bool TryFinishRoundDistinctBasicAlcoholCost(CardUseFrame use, Card card)
    {
        if (use.CardKind != CardKind.Alcohol || use.RoundDistinctBasicUses?.Any(r => !r.IsResponseUse && r.CardActionId == use.Action?.ActionId) != true)
            return false;
        _coreRoundDistinctBasicUseOperations.AssertRoundDistinctBasicUses(use);
        if (use.CardId != card.Id || use.PhysicalCardIds is not { Count: 2 } ids)
            throw new InvalidOperationException("A pair Alcohol lost its exact native representative and two materials.");
        foreach (var id in ids)
            if (_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).SingleOrDefault(c => c.Id == id)is { } cost)
                _corePhysicalMovementFacadesOperations.MoveCard(cost, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
        return true;
    }
}
