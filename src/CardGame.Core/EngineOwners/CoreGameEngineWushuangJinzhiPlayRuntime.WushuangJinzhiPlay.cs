#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWushuangJinzhiPlayRuntime
{
    internal const string WushuangJinzhiPlayPrefix = "wushuang-jinzhi-native.";
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 61020)]
    internal IEnumerable<LegalAction> BuildWushuangJinzhiPlayActions(CharacterState owner)
    {
        if (!_contentRegistryOperations._contentRegistry.Skills.ContainsKey("wushuang:skill-3252") || !owner.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(owner))
            yield break;
        foreach (var selection in new[]
        {
            CardKind.Slash,
            CardKind.FireSlash,
            CardKind.ThunderSlash,
            CardKind.ColdSlash,
            CardKind.Peach,
            CardKind.Alcohol
        }.SelectMany(k => _coreWushuangJinzhiOperations.WushuangJinzhiSelections(owner, k, false)).DistinctBy(s => (s.Source, s.OutputKind, Cards: string.Join('-', s.Cards.Select(c => c.Id)))))
        {
            var kind = selection.OutputKind;
            var targets = WushuangJinzhiPlayTargets(owner, selection);
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && targets.Count == 0 || kind == CardKind.Peach && targets.Count == 0 || kind == CardKind.Alcohol && !WushuangJinzhiAlcoholAllowed(owner, selection.Cards))
                continue;
            var ids = Array.AsReadOnly(selection.Cards.Select(c => c.Id).Order().ToArray());
            var targetCount = kind == CardKind.Alcohol ? 0 : 1;
            yield return new(LegalActionKind.UseProgramSkill, null, null, _coreCardConversionsOperations.DescribeConversion(selection.Source, $"将{(ids.Count)}张牌当【{(CardCatalog.Get(kind).DisplayName)}】使用"), kind, MinCardCount: ids.Count, MaxCardCount: ids.Count, MinTargetCount: targetCount, MaxTargetCount: targetCount)
            {
                ProgramSkillId = selection.Source.SkillId,
                ProgramActivationId = WushuangJinzhiPlayPrefix + selection.Source.BindingId + "." + string.Join('-', ids),
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

    internal IReadOnlyList<int> WushuangJinzhiPlayTargets(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        var kind = selection.OutputKind;
        var suit = Suit.None;
        bool? color = null;
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(owner, p, kind, suit, effectiveColor: color, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: selection.Source)).Select(p => p.Seat).ToArray();
        return kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(owner) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, suit) ? [owner.Seat] : [];
    }

    internal bool WushuangJinzhiAlcoholAllowed(CharacterState owner, IReadOnlyList<Card> cards) => !owner.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(owner.Seat) && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(owner, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(owner.Seat, owner.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(owner) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(owner) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(owner) || _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol) || _coreRoundGainedSourceUsesOperations.HasRoundGainedBasicBonus(owner, CardKind.Alcohol, cards.Select(c => c.Id).ToArray())) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Alcohol, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(owner.Seat, owner.Seat, Suit.None);
    internal CommandError? ValidateWushuangJinzhiPlay(CharacterState owner, LegalAction action, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(action, ids, targets)is { } error)
            return error;
        if (action.ConversionSource is not { } source || action.PlayedCardKind is not { } kind || !_coreWushuangJinzhiOperations.IsWushuangJinzhiSource(source) || _coreWushuangJinzhiOperations.WushuangJinzhiSelections(owner, kind, false).SingleOrDefault(s => s.Source == source && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()))is not { } selection)
            return new(CommandErrorCode.InvalidCard, "锦织的真实同色材料、本轮次数或技能资格已变化。");
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || kind == CardKind.Peach)
        {
            if (targets.Count != 1 || !WushuangJinzhiPlayTargets(owner, selection).Contains(targets[0]))
                return new(CommandErrorCode.InvalidTarget, "锦织目标或原生杀次数不合法。");
        }
        else if (kind != CardKind.Alcohol || targets.Count != 0 || !WushuangJinzhiAlcoholAllowed(owner, selection.Cards))
            return new(CommandErrorCode.InvalidTarget, "锦织原生酒不合法。");
        return null;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 61020)]
    internal bool TrySubmitWushuangJinzhiPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(WushuangJinzhiPlayPrefix, StringComparison.Ordinal))
            return false;
        var error = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (error is not null)
        {
            result = _kernelCoreMainOperations.Reject(error.Code, error.Message);
            return true;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = BuildWushuangJinzhiPlayActions(actor).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (command.SkillOwnerSeat is not null || action is null)
        {
            result = _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "锦织真实使用菜单已失效。");
            return true;
        }

        if (ValidateWushuangJinzhiPlay(actor, action, command.CardIds, command.TargetSeats)is { } invalid)
        {
            result = _kernelCoreMainOperations.Reject(invalid.Code, invalid.Message);
            return true;
        }

        result = _kernelCoreMainOperations.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            TryExecuteWushuangJinzhiPlay(actor, action, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 61020)]
    internal bool TryExecuteWushuangJinzhiPlay(CharacterState actor, LegalAction selected, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (selected.ProgramActivationId?.StartsWith(WushuangJinzhiPlayPrefix, StringComparison.Ordinal) != true || selected.ConversionSource is not { } source || !_coreWushuangJinzhiOperations.IsWushuangJinzhiSource(source))
            return false;
        if (ValidateWushuangJinzhiPlay(actor, selected, ids, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var kind = selected.PlayedCardKind!.Value;
        var selection = _coreWushuangJinzhiOperations.WushuangJinzhiSelections(actor, kind, false).Single(s => s.Source == source && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()));
        var appearance = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, selection.Cards[0], source);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], appearance, kind, actor.Seat, physicalCards: selection.Cards, conversionSource: source);
        else if (kind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, appearance, "桃", kind, conversionSource: source, physicalCards: selection.Cards);
        else
        {
            var use = _coreNativeCardUseOperations.BeginCardUse(appearance, actor.Seat, [], kind, physicalCardIds: selection.Cards.Select(c => c.Id).ToArray(), conversionSource: source);
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(selection.Cards.Select(c => c.Id).ToArray(), CardLocation.Processing, CardMoveReasons.Use);
            if (!_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(use, actor.Seat))
                _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(use, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(use, new(selection.Cards[0].Id, SimpleCardUseEffect.Alcohol));
        }

        return true;
    }
}
