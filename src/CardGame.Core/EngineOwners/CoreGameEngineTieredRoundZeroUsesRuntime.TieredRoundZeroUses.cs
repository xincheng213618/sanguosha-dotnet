#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTieredRoundZeroUsesRuntime
{
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> TieredRoundZeroRules(CharacterState owner, CardKind? output = null, bool responseUse = false, bool dyingUse = false)
    {
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(owner).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs.Where(r => r.InputCount == 0 && r.TieredRoundConversion is not null))
                if ((output is null || rule.OutputKind == output) && rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(owner)) && (dyingUse ? !rule.NoDying && (rule.ForResponse || rule.ForPlay && rule.UseOnly) : responseUse ? rule.ForResponse : rule.ForPlay) && _coreProgramTieredRoundConversionOperations.CanUseTieredRoundConversion(owner, instance, rule, responseUse, dyingUse) && !_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, rule.OutputKind, CardActionType.Use) && _characterZhouYuOlMouOperations.IsMouGuquCardlessAllowed(owner, rule.OutputKind, responseUse, dyingUse))
                    yield return (new(instance.SkillId, rule.Id, owner.Seat, instance.SkillInstanceId), rule, instance.Program.GameplayHash);
        foreach (var paid in _characterWoLongFengChuOlOperations.PaidEquipmentSlotNamedZeroRules(owner, output, responseUse, dyingUse).Where(p => _characterZhouYuOlMouOperations.IsMouGuquCardlessAllowed(owner, p.Rule.OutputKind, responseUse, dyingUse)))
            yield return paid;
    }

    internal IReadOnlyList<LegalAction> TieredRoundZeroPlayActions(CharacterState actor)
    {
        if (!actor.IsAlive || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) && !_characterLuSuMouOperations.IsMouLuSuUseQuery(actor))
            return[];
        var result = new List<LegalAction>();
        foreach (var(source, rule, _)in TieredRoundZeroRules(actor))
        {
            var kind = rule.OutputKind;
            var card = new Card(0, kind, Suit.None, 0);
            if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            {
                var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: kind);
                foreach (var target in targets)
                    result.Add(new(LegalActionKind.Slash, 0, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"视为使用【{(card.DisplayName)}】对 {(target.Name)} 使用"), kind) { ConversionSource = source });
                // The normal target-count policy can offer a real multi-target
                // action; it does not turn a zero-material use into a last-hand
                // Fangtian payment.
                _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(result, actor, card, targets, card.DisplayName, kind, source, null);
            }
            else if (kind == CardKind.Intoxication)
                _coreGuanduBasicConversionsOperations.AddGuanduZeroBasicActions(result, actor, source);
            else if (kind is CardKind.Zong or CardKind.RealgarWine)
                _coreDragonBoatBasicConversionsOperations.AddDragonBoatZeroBasicActions(result, actor, source, kind);
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
                result.Add(new(LegalActionKind.Peach, 0, null, _coreCardConversionsOperations.DescribeConversion(source, "视为使用【桃】"), kind) { ConversionSource = source });
            else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || (_coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor)) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, CardKind.Alcohol)) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
                result.Add(new(LegalActionKind.Alcohol, 0, null, _coreCardConversionsOperations.DescribeConversion(source, "视为使用【酒】"), kind) { ConversionSource = source });
            else if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) && kind != CardKind.Nullification)
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: false))
                    result.Add(new(option.ActionKind, 0, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, _coreCardConversionsOperations.DescribeConversion(source, option.Description), kind, option.TargetCardId, option.TargetSeats) { ConversionSource = source });
        }

        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(result, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(result, actor);
        return Array.AsReadOnly(result.Where(action => _characterWoLongFengChuOlOperations.IsEquipmentSlotNamedActionPayable(actor, action)).Select(action => action with { TieredRoundZeroUse = _corePlayCardHistoryOperations.ViewAsRule(action.ConversionSource!)!.TieredRoundConversion }).ToArray());
    }

    internal bool TrySubmitTieredRoundZeroPlay(PlayCardCommand command, out CommandResult result)
    {
        result = null !;
        if (command.CardId != 0 || command.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.TieredRoundConversion is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var targets = command.TargetSeats ?? [];
        var action = TieredRoundZeroPlayActions(actor).SingleOrDefault(a => a.ConversionSource == source && a.PlayedCardKind == command.PlayedCardKind && a.TargetCardId == command.TargetCardId && a.TargetSeats.SequenceEqual(targets));
        if (action is null || command.AdditionalConversionSources is { Count: > 0 })
        {
            result = _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "The zero-material use changed its published source, card name or exact legal targets.");
            return true;
        }

        result = _kernelCoreMainOperations2.Accept(() =>
        {
            if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedPlay(actor, action))
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                    _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
                return;
            }

            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreCardConversionsOperations.SelectUseConversion(action);
            ExecuteTieredRoundZeroPlay(actor, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    internal IReadOnlyList<PromptChoice> TieredRoundZeroForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong)
    {
        if (qinglong ? _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || blade.Attack.ResolutionId != originalId || blade.Attack.SourceSeat != actor.Seat || blade.Attack.TargetSeat != target.Seat || !global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack) : _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not { } borrowed || borrowed.ResolutionId != originalId || borrowed.WeaponOwnerSeat != actor.Seat || borrowed.SlashTargetSeat != target.Seat || borrowed.ActiveAttack is not null || _coreNativeBorrowedSwordOperations.GetWeapon(actor)is null)
            return[];
        var result = new List<PromptChoice>();
        foreach (var(source, rule, _)in TieredRoundZeroForcedSlashRules(actor, target, qinglong))
        {
            if (!_characterWoLongFengChuOlOperations.IsEquipmentSlotNamedForcedSlashPayable(actor, source, rule.OutputKind, target, qinglong))
                continue;
            var kind = rule.OutputKind;
            var parameters = new Dictionary<string, string>
            {
                [qinglong ? "action" : "response"] = "tiered-round-zero-forced-slash",
                ["output-kind"] = kind.ToString(),
                ["original-frame-id"] = originalId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["forced-use"] = qinglong ? "qinglong" : "borrowed-sword"
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            result.Add(new(new ChoiceId($"tiered-zero.forced-{(parameters["forced-use"])}.{(originalId)}.{(source.BindingId)}.{(source.SkillInstanceId)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为使用【{(CardCatalog.Get(kind).DisplayName)}】攻击 {(target.Name)}。", [], [target.Seat], parameters));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule, string Hash)> TieredRoundZeroForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => TieredRoundZeroRules(actor).Where(r => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(r.Rule.OutputKind) && (qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, r.Rule.OutputKind, null, false, Suit.None, 0) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, r.Rule.OutputKind, Suit.None, false, null, 0, [])) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, r.Rule.OutputKind, Suit.None));
    internal bool TryResolveTieredRoundZeroForcedSlash(PromptChoice selected, bool advance)
    {
        if (selected.Parameters.GetValueOrDefault("forced-use")is not ("borrowed-sword" or "qinglong"))
            return false;
        var qinglong = selected.Parameters["forced-use"] == "qinglong";
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.Kind != (qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash) || !prompt.Choices.Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || selected.Cards.Count != 0 || selected.Targets is not [var targetSeat] || !long.TryParse(selected.Parameters.GetValueOrDefault("original-frame-id"), out var originalId) || !Enum.TryParse<CardKind>(selected.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("A zero forced Slash changed its exact published producer, target or name.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!TieredRoundZeroForcedSlashChoices(actor, target, originalId, qinglong).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("A forced zero Slash lost its original legal same-target use and shared allowance.");
        var conversion = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        _coreCardConversionsOperations.CaptureSelectedResponseConversion(selected);
        if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedForcedSlash(actor, conversion, kind, selected, qinglong))
            return true;
        if (qinglong)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, actor.Seat, target.Seat, Used: true, SlashCardIds: [], EffectiveSlashKind: kind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
            BeginTieredRoundForcedZeroSlash(actor, target, kind, conversion, null, false);
        }
        else
        {
            var original = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!;
            _coreNativeResponseWindowsOperations.PopResponseWindow(original.ResolutionId);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeCardUseFramesOperations.SetCardUseStep(original.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            original.AwaitingSlashChoice = false;
            original.SlashCardId = 0;
            original.EffectiveSlashKind = kind;
            BeginTieredRoundForcedZeroSlash(actor, target, kind, conversion, original, true);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        return true;
    }

    internal void BeginTieredRoundForcedZeroSlash(CharacterState actor, CharacterState target, CardKind kind, CardConversionSource source, global::CardGame.Core.NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit)
    {
        var card = new Card(0, kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [target.Seat], kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), physicalCardIds: [], conversionSource: source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind));
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (countsTowardLimit && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool TryExecuteTieredRoundZeroPlay(CharacterState actor, LegalAction action)
    {
        if (action.CardId != 0 || action.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.TieredRoundConversion is null)
            return false;
        if (!TieredRoundZeroPlayActions(actor).Any(a => a.Kind == action.Kind && a.ConversionSource == source && a.PlayedCardKind == action.PlayedCardKind && a.TargetCardId == action.TargetCardId && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("A zero-material use lost its published legal action before acceptance.");
        ExecuteTieredRoundZeroPlay(actor, action);
        return true;
    }

    internal void ExecuteTieredRoundZeroPlay(CharacterState actor, LegalAction action)
    {
        if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedPlay(actor, action))
            return;
        var source = action.ConversionSource!;
        var kind = action.PlayedCardKind!.Value;
        var card = new Card(0, kind, Suit.None, 0);
        if (_coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, action))
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = action.TargetSeats;
        if (kind == CardKind.Intoxication)
        {
            _coreGuanduCardsOperations.ResolveGuanduCard(actor, card, action);
            return;
        }

        if (kind is CardKind.Zong or CardKind.RealgarWine)
        {
            _coreDragonBoatCardsOperations.ResolveDragonBoatCard(actor, card, action);
            return;
        }

        if (kind == CardKind.Peach)
        {
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, card, "桃", CardKind.Peach, conversionSource: source, physicalCards: []);
            return;
        }

        // Resolve the published option before BeginCardUse consumes any
        // one-use target adjustment or quota and freezes its accepted action.
        CardKind? requiredKind = null;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
        {
            var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true);
            if (action.ProgramActivationId == "next-card-target-adjustment")
            {
                // Legacy targets are already published by this exact producer;
                // native options supply effect metadata, not replacement targets.
                if (!TieredRoundZeroPlayActions(actor).Any(a => a.ProgramActivationId == action.ProgramActivationId && a.ProgramSkillId == action.ProgramSkillId && a.ConversionSource == source && a.PlayedCardKind == kind && a.Kind == action.Kind && a.TargetCardId == action.TargetCardId && a.TargetSeats.SequenceEqual(action.TargetSeats)))
                    throw new InvalidOperationException("A legacy-adjusted zero trick lost its exact published producer and ordered targets.");
                requiredKind = options.Where(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId).Select(o => o.RequiredCardKind).Distinct().Single();
            }
            else
                requiredKind = options.Single(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId && o.TargetSeats.SequenceEqual(action.TargetSeats)).RequiredCardKind;
        }

        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats, kind, physicalCardIds: [], conversionSource: source);
        if (kind == CardKind.Alcohol)
        {
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
            if (targets.Count == 0)
                throw new InvalidOperationException("A virtual Slash requires its real selected targets.");
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind));
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            if (!_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true)
                _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, targets[0]));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, action.Kind, action.TargetCardId, requiredKind, kind);
    }

    internal void IssueTieredRoundConversionUse(CardUseFrame use)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ConversionChain.FirstOrDefault(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.TieredRoundConversion is not null)is not { } source)
            return;
        var rule = _corePlayCardHistoryOperations.ViewAsRule(source)!;
        var policy = rule.TieredRoundConversion!;
        if (source.OwnerSeat != use.SourceSeat || action.ActorSeat != use.SourceSeat || action.ProviderSeat != use.SourceSeat || action.RequesterSeat is not null || action.ConversionChain.Count != 1 || action.EffectiveKind != rule.OutputKind || action.PhysicalCards.Count != rule.InputCount || source.SkillInstanceId is null || _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program is not { } program)
            throw new InvalidOperationException("A tiered use lost its actual actor/provider/material and conversion tuple.");
        var receipt = new TieredRoundConversionUseReceipt(source, program.GameplayHash, policy.StateId, policy.UsageId, _coreProgramTieredRoundConversionOperations.TieredRoundTier(use.SourceSeat, policy.StateId), _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.ActionId, use.Id, use.SourceSeat, use.CardKind, action.PhysicalCards.Count, use.CardKind is CardKind.Peach or CardKind.Alcohol && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && dying.ResponderSeat == use.SourceSeat && use.TargetSeats.SequenceEqual([dying.VictimSeat]) ? dying.FrameId : null);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { TieredRoundConversionUse = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TieredRoundConversionUseIssuedEvent(receipt));
    }

    internal bool IsIssuedTieredRoundUse(CardUseFrame use, bool zeroOnly = false)
    {
        if (use.TieredRoundConversionUse is not { } r || use.Action is not { Type: CardActionType.Use } || TieredRoundIssuedOriginalOutputAction(use, r)is not { } action)
            return false;
        // This comparison-only view retains the original accepted output for
        // the mature actor-replacement proof. It never changes the runtime use,
        // its current FireSlash effect or the issued material/quota receipt.
        var issuedView = use.CurrentSlashFirePolicy is null ? use : use with
        {
            CardKind = r.EffectiveKind,
            Action = action
        };
        if (r.OwnerFrameId != use.Id || r.CardActionId != action.ActionId || !TieredRoundIssuedActorMatches(issuedView, r) || action.ProviderSeat != r.ActorSeat || action.RequesterSeat is not null || action.EffectiveKind != r.EffectiveKind || r.MaterialCount != action.PhysicalCards.Count || !(use.PhysicalCardIds ?? []).SequenceEqual(action.PhysicalCards.Select(c => c.CardId)) || action.ConversionChain is not [var source] || source != r.Source || r.Source.OwnerSeat != r.ActorSeat || string.IsNullOrWhiteSpace(r.Source.SkillInstanceId) || string.IsNullOrWhiteSpace(r.Source.BindingId) || r.FrozenTier is < 0 or > 2 || r.MaterialCount != (r.FrozenTier == 2 ? 0 : 1) || zeroOnly && (r.FrozenTier != 2 || use.CardId != 0) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program is not { } definition || definition.GameplayHash != r.GameplayHash || _coreActivityOrdinaryTricksOperations.ActivityRuntimeProgram(definition).ViewAs.SingleOrDefault(v => v.Id == r.Source.BindingId)is not { TieredRoundConversion: { } policy } rule || policy.StateId != r.StateId || policy.UsageId != r.UsageId || rule.OutputKind != r.EffectiveKind || rule.InputCount != r.MaterialCount || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TieredRoundConversionUseIssuedEvent>().Count(e => e.Receipt == r) != 1)
            return false;
        // This is issued identity, not a second current-shard permission query.
        return _skillRuntimeStateOperations._skillRuntimeState.GetUsage(r.ActorSeat, r.Source.SkillId, global::CardGame.Core.EngineOwners.CoreProgramTieredRoundConversionRuntime.TieredRoundUsage(policy), r.FrozenTier == 0 ? SkillUsageScope.Phase : SkillUsageScope.Round) == 1;
    }

    internal CardActionContext? TieredRoundIssuedOriginalOutputAction(CardUseFrame use, TieredRoundConversionUseReceipt receipt)
    {
        if (use.Action is not { Type: CardActionType.Use } action)
            return null;
        if (use.CurrentSlashFirePolicy is not { } fire)
            return use.CardKind == receipt.EffectiveKind ? action : null;
        if (!global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(receipt.EffectiveKind) || fire.OriginalKind != receipt.EffectiveKind || fire.OriginalAction.ActionId != receipt.CardActionId || fire.OriginalAction.Type != CardActionType.Use || fire.OriginalAction.EffectiveKind != receipt.EffectiveKind)
            return null;
        // Only the real 5403 issued policy can append a fire conversion source.
        // Its mature assertion proves original/current actors and materials,
        // the exact source chain, current targets/attack and the changed fact.
        // No current source shard is queried after the accepted cost.
        _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
        return fire.OriginalAction;
    }

    internal bool IsTieredRoundZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedTieredRoundUse(use, true);
    internal bool TieredRoundIssuedActorMatches(CardUseFrame use, TieredRoundConversionUseReceipt receipt)
    {
        if (use.Action is not { } action || action.ProviderSeat != receipt.ActorSeat)
            return false;
        var changes = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id).ToArray();
        if (changes.Length == 0)
            return action.ActorSeat == receipt.ActorSeat && use.SourceSeat == receipt.ActorSeat;
        // The original material owner and quota remain issued; mutable actual
        // actor changes only along the mature exact replacement producer chain.
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == receipt.ActorSeat && e.CardId == use.CardId && e.CardKind == receipt.EffectiveKind) == 1 && _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, receipt.ActorSeat);
    }

    internal Card TieredRoundZeroRepresentation(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use is null || !IsIssuedTieredRoundUse(use, true))
            throw new InvalidOperationException("A logical card has no exact zero-material issuance.");
        return new(0, use.CardKind, Suit.None, 0);
    }

    internal bool ContinueTieredRoundZeroSimpleUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (continuation.CardId != 0 || !IsTieredRoundZeroUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var card = TieredRoundZeroRepresentation(id);
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, use.CardKind);
            return true;
        }

        if (continuation.Effect == SimpleCardUseEffect.Recovery && use.CardKind is CardKind.Peach or CardKind.Alcohol or CardKind.Zong)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, id, use.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (continuation.Effect == SimpleCardUseEffect.Alcohol && use.CardKind is CardKind.Alcohol or CardKind.RealgarWine)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        else
            throw new InvalidOperationException("A zero-material basic use changed its exact simple effect.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 110)]
    internal bool FinishTieredRoundZeroAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsTieredRoundZeroUse(attack.ResolutionId) || attack.EffectiveCardKind is not { } kind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind != CardKind.Duel && kind != CardKind.AgainstCurrent && !(kind == CardKind.FireAttack && _coreTieredRoundZeroReturnsOperations.MatchesTieredRoundZeroFireAttackAttack(attack)))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    internal void AssertTieredRoundConversionUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Where(u => u.TieredRoundConversionUse is not null))
            if (!IsIssuedTieredRoundUse(use) || use.TieredRoundConversionUse!.FrozenTier == 2 && use.CardId != 0)
                throw new InvalidOperationException("A tiered conversion lost its accepted material count, source tuple or issued allowance.");
    }
}
