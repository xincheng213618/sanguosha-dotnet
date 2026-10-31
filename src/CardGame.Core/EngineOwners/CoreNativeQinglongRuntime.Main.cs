#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeQinglongRuntime
{
    internal bool CanUseQinglongCrescentBladeTarget(CharacterState source, CharacterState target, CardKind? effectiveKind = null, bool? effectiveColor = null, bool allowAnyColor = true, Suit? physicalSuit = null, int? effectiveRank = null, IReadOnlyList<int>? physicalCardIds = null) => source.IsAlive && target.IsAlive && source.Seat != target.Seat && _coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.QinglongCrescentBlade) && (effectiveKind is { } kind ? !_coreCardUseModulesOperations.IsCardUseForbidden(source.Seat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(source.Seat, target.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(target) : CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(candidate => CanUseQinglongCrescentBladeTarget(source, target, candidate, effectiveColor, allowAnyColor, physicalSuit, effectiveRank, physicalCardIds)));
    internal IReadOnlyList<Card> GetQinglongCrescentBladeSlashCards(CharacterState source, CharacterState target) => CanUseQinglongCrescentBladeTarget(source, target) ? _coreActionQueriesOperations.GetSlashUseCards(source).Where(card => _coreProgramCardUseRulesOperations.GetSlashUseVariants2(source, _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(source, card)).Concat(_characterMiHengLeOperations.SiruiPhysicalSlashVariants(source, card).Concat(_characterGuanYuWumiaoOperations.JuewuPhysicalSlashVariants(source, card)).Concat(_characterCaoZhiLeOperations.FulePhysicalSlashVariants(source, card).Concat(_coreProgramCardLegalityOperations.CardLegality.NativeSlashVariants(new(source, card))))).DistinctBy(v => v.EffectiveKind).Any(variant => CanUseQinglongCrescentBladeTarget(source, target, variant.EffectiveKind, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, card)), allowAnyColor: false, physicalSuit: _coreNativeCardAppearanceOperations.EffectiveSuit(source, card), effectiveRank: _coreFinalTargetSlashOperations.SpecificSlashRank(source, card, variant.EffectiveKind), physicalCardIds: [card.Id]))).ToArray() : [];
    internal bool CanRequestQinglongCrescentBladeFactionSlash(NativeOwners.QinglongCrescentBladeHandle pending)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[pending.Attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.Attack.TargetSeat];
        return _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is null && !pending.FactionSlashAttempted && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(source, CardKind.Slash)is not null && CanUseQinglongCrescentBladeTarget(source, target) && _coreProgramFactionPoliciesOperations.GetFactionSlashCandidateSeats(source.Seat).Count > 0;
    }

    internal bool TryBeginQinglongCrescentBladeChoice(NativeOwners.CardAttackHandle attack)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var effectiveKind = CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack);
        if (!_coreNativeDamageHandlesOperations.UsesFormalQinglongCrescentBlade || effectiveKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash))
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not null)
        {
            throw new InvalidOperationException("The engine cannot open two Qinglong Crescent Blade choices at once.");
        }

        var pending = _kernelCoreMainOperations.CreateQinglongCrescentBladeHandle(attack);
        _coreQinglongInsertionOperations.SuspendFactionRequestForQinglongChoice(attack);
        var slashes = GetQinglongCrescentBladeSlashCards(source, target);
        if (slashes.Count == 0 && !CanRequestQinglongCrescentBladeFactionSlash(pending) && !_coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashRules(source, target, true).Any() && !_characterXiaoQiaoMouOlOperations.HasProspectiveMouXqQinglong(source, target, attack) && !_coreChainedStateBasicConversionsOperations.ChainedStateBasicForcedSlashRules(source, target, true).Any() && !_coreDrawFundedDistinctBasicOffersOperations.DrawFundedDistinctBasicForcedSlashRules(source, target, true).Any() && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(source, CardKind.Slash) && !_characterLiuFengBoundaryOperations.HasBoundaryLiuFengForcedSlash2(source, target, pending) && !_characterMiHengLeOperations.HasSiruiForcedSlash(source, target, pending) && !(_characterGuoTuOlOperations.HasWeijieForcedSlash2(source, target, pending) || (_characterLiuZhangOlOperations.HasZonghuForcedSlash2(source, target, pending) || _characterHuanShuJunOlOperations.HasHuanpeiForcedSlash2(source, target, pending))) && _characterMaXiuMaTieOlOperations.KenshangQinglongSlashChoices(pending).Count == 0 && _characterChengJiChengCuiOlOperations.TousuiQinglongSlashChoices(pending).Count == 0)
        {
            _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(attack.ResolutionId);
            return false;
        }

        _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = pending;
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        PublishQinglongCrescentBladeChoice(pending, includeFactionSlash: true);
        return true;
    }

    internal IReadOnlyList<PromptChoice> QinglongPhysicalSlashChoices(NativeOwners.QinglongCrescentBladeHandle pending, IReadOnlyList<Card> slashes)
    {
        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        return slashes.SelectMany(card => new[] { _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(source, card) }.Concat(_characterMiHengLeOperations.SiruiPhysicalSlashVariants(source, card).Concat(_characterGuanYuWumiaoOperations.JuewuPhysicalSlashVariants(source, card)).Concat(_characterCaoZhiLeOperations.FulePhysicalSlashVariants(source, card).Concat(_coreProgramCardLegalityOperations.CardLegality.NativeSlashVariants(new(source, card)))).Select(variant => variant.EffectiveKind)).Distinct().SelectMany(followupKind =>
        {
            var cardDescription = CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind) ? $"使用【{(CardCatalog.Get(card.Kind).DisplayName)}】" : $"将【{(card.DisplayName)}】当作【杀】使用";
            return _coreCardConversionsOperations.CreateConversionChoiceVariants(source, card, followupKind, forResponse: false, $"qinglong.slash.resolution-{(attack.ResolutionId)}.card-{(card.Id)}", $"{(cardDescription)}，继续攻击 {(target.Name)}。", [card.Id], [target.Seat], new Dictionary<string, string> { ["action"] = "qinglong-slash", ["response-card-kind"] = followupKind.ToString() });
        })).ToArray();
    }

    internal void PublishQinglongCrescentBladeChoice(NativeOwners.QinglongCrescentBladeHandle pending, bool includeFactionSlash)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack))
        {
            throw new InvalidOperationException("The Qinglong Crescent Blade choice has no current Slash continuation.");
        }

        var attack = pending.Attack;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var effectiveKind = CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack);
        var slashes = GetQinglongCrescentBladeSlashCards(source, target);
        var choices = QinglongPhysicalSlashChoices(pending, slashes).ToList();
        if (includeFactionSlash && CanRequestQinglongCrescentBladeFactionSlash(pending))
        {
            choices.Add(new PromptChoice(new ChoiceId($"qinglong.faction-slash.resolution-{(attack.ResolutionId)}"), $"发动【激将】，请求其他蜀势力角色为你对 {(target.Name)} 提供一张【杀】。", [], [target.Seat], new Dictionary<string, string> { ["action"] = "qinglong-jijiang", ["skill"] = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(source, CardKind.Slash)!.SkillId }));
        }

        choices.AddRange(_coreChainedStateBasicConversionsOperations.ChainedStateBasicForcedSlashChoices(source, target, attack.ResolutionId, true));
        choices.AddRange(_coreRoundOnceCardlessNativeUsesOperations.RoundOnceCardlessForcedSlashChoices(source, target, attack.ResolutionId, true));
        choices.AddRange(_characterMaXiuMaTieOlOperations.KenshangQinglongSlashChoices(pending));
        choices.AddRange(_characterChengJiChengCuiOlOperations.TousuiQinglongSlashChoices(pending));
        choices.AddRange(_coreTieredRoundZeroUsesOperations.TieredRoundZeroForcedSlashChoices(source, target, attack.ResolutionId, true));
        choices.Add(new PromptChoice(new ChoiceId($"qinglong.skip.resolution-{(attack.ResolutionId)}"), "不发动【青龙偃月刀】，此【杀】被【闪】抵消。", [], [], new Dictionary<string, string> { ["action"] = "qinglong-skip" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.QinglongCrescentBlade, source.Seat, $"{(target.Name)} 已用【闪】抵消你的【{(CardCatalog.Get(effectiveKind).DisplayName)}】，是否发动【青龙偃月刀】对其再使用一张【杀】？", slashes.Select(card => card.Id).ToArray(), [target.Seat], SourceSeat: source.Seat, IncomingCard: effectiveKind)
        {
            PromptId = source.IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveQinglongCrescentBladeChoice(PromptChoice selected)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade ?? throw new InvalidOperationException("There is no Qinglong Crescent Blade choice to resolve.");
        var attack = pending.Attack;
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QinglongCrescentBlade } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The Qinglong Crescent Blade choice is not the current Slash continuation.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (selected.Parameters.GetValueOrDefault("action") == "mou-xiao-qiao-forced-slash")
        {
            _characterXiaoQiaoMouOlOperations.BeginMouXqForcedSlash(selected);
            return;
        }

        if (_characterChengJiChengCuiOlOperations.TryBeginTousuiQinglongSlash(pending, selected))
            return;
        if (_characterMaXiuMaTieOlOperations.TryBeginKenshangQinglongSlash(pending, selected))
            return;
        if (_coreRoundOnceCardlessNativeUsesOperations.TryResolveRoundOnceCardlessForcedSlash(selected, false))
            return;
        if (_coreTieredRoundZeroUsesOperations.TryResolveTieredRoundZeroForcedSlash(selected, false))
            return;
        var action = selected.Parameters.GetValueOrDefault("action");
        var usesSlash = action == "qinglong-slash";
        var requestsFactionSlash = action == "qinglong-jijiang";
        Card? slash = null;
        CardKind? effectiveSlashKind = null;
        if (usesSlash)
        {
            if (!CanUseQinglongCrescentBladeTarget(source, target) || selected.Cards.Count != 1 || !selected.Targets.SequenceEqual([target.Seat]))
            {
                throw new InvalidOperationException("Qinglong Crescent Blade must retain its source, weapon and same target.");
            }

            slash = GetQinglongCrescentBladeSlashCards(source, target).SingleOrDefault(card => card.Id == selected.Cards[0]) ?? throw new InvalidOperationException("The selected Qinglong Crescent Blade Slash is no longer available.");
            effectiveSlashKind = _coreProgramCardLegalityOperations.CardLegality.SelectNativeSlashKind(new(source, slash, _characterCaoZhiLeOperations.FuleSelectedSlashKind(source, slash, _characterGuanYuWumiaoOperations.JuewuSelectedSlashKind(source, slash, _characterMiHengLeOperations.SiruiSelectedSlashKind(source, slash, _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(source, slash), CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var siruiSelectedSource) ? siruiSelectedSource : null), siruiSelectedSource), siruiSelectedSource), siruiSelectedSource));
            if (!CanUseQinglongCrescentBladeTarget(source, target, effectiveSlashKind, CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(source, slash)), false, _coreNativeCardAppearanceOperations.EffectiveSuit(source, slash), _coreFinalTargetSlashOperations.SpecificSlashRank(source, slash, effectiveSlashKind ?? CardKind.Slash), [slash.Id]))
                throw new InvalidOperationException("The selected Qinglong Crescent Blade Slash kind is prohibited for its target.");
        }
        else if (requestsFactionSlash)
        {
            if (selected.Cards.Count != 0 || !selected.Targets.SequenceEqual([target.Seat]) || !CanRequestQinglongCrescentBladeFactionSlash(pending))
            {
                throw new InvalidOperationException("Qinglong Crescent Blade FactionSlash is no longer available.");
            }

            _coreNativeFactionSlashOperations.BeginQinglongCrescentBladeFactionSlashRequest(pending);
            return;
        }
        else if (action != "qinglong-skip" || selected.Cards.Count != 0 || selected.Targets.Count != 0)
        {
            throw new InvalidOperationException("The Qinglong Crescent Blade choice is malformed.");
        }

        if (usesSlash)
        {
            _coreCardConversionsOperations.CaptureSelectedResponseConversion(selected);
            BeginQinglongCrescentBladeFollowup(pending, slash!, effectiveSlashKind!.Value, source.Seat, [slash!]);
            return;
        }

        _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, Used: false, SlashCardIds: [], EffectiveSlashKind: null));
        _coreMatchLogOperations.AddLog("EquipmentSkipped", $"{(source.Name)} 未发动【青龙偃月刀】，对 {(target.Name)} 的【杀】被【闪】抵消。", source.Seat, target.Seat);
        _coreQinglongInsertionOperations.RestoreFactionRequestAfterQinglongChoice(attack.ResolutionId);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void BeginQinglongCrescentBladeFollowup(NativeOwners.QinglongCrescentBladeHandle pending, Card slash, CardKind effectiveSlashKind, int physicalOwnerSeat, IReadOnlyList<Card> physicalCards, NativeOwners.FactionCardRequestHandle? jijiang = null, CardConversionSource? conversionSource = null, bool skipInvocation = false)
    {
        var attack = pending.Attack;
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack))
        {
            throw new InvalidOperationException("The Qinglong Crescent Blade follow-up is not current.");
        }

        if (!skipInvocation && _coreNativeEquipmentInvocationsOperations.TryBeginQinglongEquipmentInvocation(pending, slash, effectiveSlashKind, physicalOwnerSeat, physicalCards, jijiang, conversionSource))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, Used: true, Array.AsReadOnly(physicalCards.Select(card => card.Id).ToArray()), effectiveSlashKind));
        _coreMatchLogOperations.AddLog("EquipmentEffect", $"{(source.Name)} 发动【青龙偃月刀】，对 {(target.Name)} 再使用一张【{(CardCatalog.Get(effectiveSlashKind).DisplayName)}】。", source.Seat, target.Seat);
        _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(attack.ResolutionId, jijiang);
        _coreNativeSlashUseOperations.ResolveSlashCore(source, target, slash, effectiveSlashKind, physicalOwnerSeat, factionRequest: jijiang, physicalCards: physicalCards, countsTowardSlashLimit: false, conversionSource: conversionSource);
    }
}
