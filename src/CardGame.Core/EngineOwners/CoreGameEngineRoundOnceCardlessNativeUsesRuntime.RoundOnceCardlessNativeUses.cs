#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundOnceCardlessNativeUsesRuntime
{
    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> RoundOnceCardlessRules(CharacterState actor, CardKind? output = null, bool forResponse = false)
    {
        foreach (var instance in _coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances)
            foreach (var rule in instance.Program.ViewAs)
                if (rule.RoundOnceCardlessBasic is not null && (output is null || rule.OutputKind == output) && rule.Condition.Evaluate(_coreNativeSkillContextQueriesOperations.CreateSkillContext(actor)) && _coreRoundOnceCardlessBasicOperations.CanUseRoundOnceCardlessBasic(actor, instance, rule, forResponse) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, rule.OutputKind, CardActionType.Use))
                    yield return (new(instance.SkillId, rule.Id, actor.Seat, instance.SkillInstanceId), rule);
    }

    internal IReadOnlyList<LegalAction> RoundOnceCardlessPlayActions(CharacterState actor)
    {
        if (!actor.IsAlive || (_coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || actor.Seat != _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(actor) && !(_characterKongRongMouOlOperations.Value.IsMouKrLiwenUseQuery(actor) || _characterMaTieAnniversaryOperations.IsAnniversaryMaTieSlashQuery(actor)) && !_characterLuSuMouOperations.Value.IsMouLuSuUseQuery(actor))
            return[];
        var result = new List<LegalAction>();
        foreach (var(source, rule)in RoundOnceCardlessRules(actor, CardKind.Slash))
        {
            var card = new Card(0, CardKind.Slash, Suit.None, 0);
            var targets = _coreNativeMultiTargetActionsOperations.GetFangtianOrderedSlashTargets(actor, card, source, effectiveKind: CardKind.Slash);
            foreach (var target in targets)
                result.Add(new(LegalActionKind.Slash, 0, target.Seat, _coreCardConversionsOperations.DescribeConversion(source, $"视为使用【杀】对 {(target.Name)} 使用"), CardKind.Slash) { ConversionSource = source });
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(result, actor, card, targets, card.DisplayName, CardKind.Slash, source, null);
        }

        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(result, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(result, actor);
        return Array.AsReadOnly(result.Select(action => action with { RoundOnceCardlessBasicUse = _corePlayCardHistoryOperations.ViewAsRule(action.ConversionSource!)!.RoundOnceCardlessBasic }).ToArray());
    }

    internal bool TrySubmitRoundOnceCardlessPlay(PlayCardCommand command, out CommandResult result)
    {
        result = null !;
        if (command.CardId != 0 || command.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.RoundOnceCardlessBasic is null)
            return false;
        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var action = RoundOnceCardlessPlayActions(actor).SingleOrDefault(a => a.ConversionSource == source && a.PlayedCardKind == command.PlayedCardKind && a.TargetCardId == command.TargetCardId && a.TargetSeats.SequenceEqual(command.TargetSeats ?? []));
        if (action is null || command.AdditionalConversionSources is { Count: > 0 })
        {
            result = _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "Cardless basic changed its exact published source, ordinary Slash or ordered native targets.");
            return true;
        }

        result = _kernelCoreMainOperations2.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreCardConversionsOperations.SelectUseConversion(action);
            ExecuteRoundOnceCardlessPlay(actor, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    internal bool TryExecuteRoundOnceCardlessPlay(CharacterState actor, LegalAction action)
    {
        if (action.CardId != 0 || action.ConversionSource is not { } source || _corePlayCardHistoryOperations.ViewAsRule(source)?.RoundOnceCardlessBasic is null)
            return false;
        if (!RoundOnceCardlessPlayActions(actor).Any(a => a.Kind == action.Kind && a.ConversionSource == source && a.PlayedCardKind == action.PlayedCardKind && a.TargetCardId == action.TargetCardId && a.TargetSeats.SequenceEqual(action.TargetSeats)))
            throw new InvalidOperationException("Cardless Slash lost its exact published action before payment.");
        ExecuteRoundOnceCardlessPlay(actor, action);
        return true;
    }

    internal void ExecuteRoundOnceCardlessPlay(CharacterState actor, LegalAction action)
    {
        var source = action.ConversionSource!;
        if (action.PlayedCardKind != CardKind.Slash)
            throw new InvalidOperationException("Cardless Play requires ordinary Slash.");
        if (_coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, action))
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = action.TargetSeats;
        _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var card = new Card(0, CardKind.Slash, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, action.TargetSeats, CardKind.Slash, physicalCardIds: [], conversionSource: source);
        IssueRoundOnceCardlessNativeUse(id, source);
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        if (targets.Count == 0)
            throw new InvalidOperationException("Cardless Slash requires its actual selected targets.");
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], CardKind.Slash));
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (!_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targets[0]));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void IssueRoundOnceCardlessNativeUse(long id, CardConversionSource source)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id) ?? throw new InvalidOperationException("Cardless native use has no actual owning frame.");
        var action = use.Action ?? throw new InvalidOperationException("Cardless native use has no allocated action.");
        var receipt = _coreRoundOnceCardlessBasicOperations.IssueRoundOnceCardlessBasicUse(use, action, source);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, use with { RoundOnceCardlessBasicUse = receipt });
    }

    internal bool IsRoundOnceCardlessBasicUse(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use?.RoundOnceCardlessBasicUse is not { } receipt || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.Action is not { } action)
            return false;
        var original = action;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            if (fire.OriginalAction.ActionId != receipt.CardActionId || fire.OriginalAction.Type != CardActionType.Use || fire.OriginalAction.EffectiveKind != receipt.EffectiveKind)
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            original = fire.OriginalAction;
        }

        if (!_coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(receipt, use, original))
        {
            var declared = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Where(e => e.Action.ActionId == receipt.CardActionId && e.Action.ActorSeat == receipt.ActorSeat && e.Action.EffectiveKind == receipt.EffectiveKind).Select(e => e.Action).FirstOrDefault();
            if (declared is null || !_coreRoundOnceCardlessBasicOperations.IsIssuedRoundOnceCardlessBasicUse(receipt, use, declared) || !_coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, receipt.ProviderSeat))
                return false;
        }

        return use.SourceSeat == action.ActorSeat && action.ProviderSeat == receipt.ProviderSeat && (use.CardKind == receipt.EffectiveKind || use.CurrentSlashFirePolicy is not null);
    }

    internal Card RoundOnceCardlessNativeRepresentation(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use is null || !IsRoundOnceCardlessBasicUse(id))
            throw new InvalidOperationException("Logical cardless Slash lacks its exact native accepted source and paid issuance.");
        return new(0, use.CardKind, Suit.None, 0);
    }

    internal IReadOnlyList<PromptChoice> RoundOnceCardlessForcedSlashChoices(CharacterState actor, CharacterState target, long originalId, bool qinglong)
    {
        if (qinglong ? _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not { } blade || blade.Attack.ResolutionId != originalId || blade.Attack.SourceSeat != actor.Seat || blade.Attack.TargetSeat != target.Seat || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, blade.Attack) : _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is not { } borrowed || borrowed.ResolutionId != originalId || borrowed.WeaponOwnerSeat != actor.Seat || borrowed.SlashTargetSeat != target.Seat || borrowed.ActiveAttack is not null || _coreNativeBorrowedSwordOperations.GetWeapon(actor)is null)
            return[];
        var result = new List<PromptChoice>();
        foreach (var(source, rule)in RoundOnceCardlessForcedSlashRules(actor, target, qinglong))
        {
            var kind = rule.OutputKind;
            var parameters = new Dictionary<string, string>
            {
                [qinglong ? "action" : "response"] = "round-once-cardless-forced-slash",
                ["output-kind"] = kind.ToString(),
                ["original-frame-id"] = originalId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ["forced-use"] = qinglong ? "qinglong" : "borrowed-sword"
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            result.Add(new(new ChoiceId($"round-cardless.forced-{(parameters["forced-use"])}.{(originalId)}.{(source.BindingId)}.{(source.SkillInstanceId)}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(source))}】，视为使用【{(CardCatalog.Get(kind).DisplayName)}】攻击 {(target.Name)}。", [], [target.Seat], parameters));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal IEnumerable<(CardConversionSource Source, SkillProgramViewAs Rule)> RoundOnceCardlessForcedSlashRules(CharacterState actor, CharacterState target, bool qinglong) => RoundOnceCardlessRules(actor).Where(r => CoreNativeCardClassificationsRuntime.IsSlashCard(r.Rule.OutputKind) && (qinglong ? _coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(actor, target, r.Rule.OutputKind, null, false, Suit.None, 0) : _coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(actor, target, r.Rule.OutputKind, Suit.None, false, null, 0, [])) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, target.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, r.Rule.OutputKind, Suit.None));
    internal bool TryResolveRoundOnceCardlessForcedSlash(PromptChoice selected, bool advance)
    {
        if (selected.Parameters.GetValueOrDefault("action") != "round-once-cardless-forced-slash" && selected.Parameters.GetValueOrDefault("response") != "round-once-cardless-forced-slash")
            return false;
        if (selected.Parameters.GetValueOrDefault("forced-use")is not ("borrowed-sword" or "qinglong"))
            return false;
        var qinglong = selected.Parameters["forced-use"] == "qinglong";
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } prompt || prompt.Kind != (qinglong ? DecisionKind.QinglongCrescentBlade : DecisionKind.RespondSlash) || !prompt.Choices.Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])) || selected.Cards.Count != 0 || selected.Targets is not [var targetSeat] || !long.TryParse(selected.Parameters.GetValueOrDefault("original-frame-id"), out var originalId) || !Enum.TryParse<CardKind>(selected.Parameters.GetValueOrDefault("output-kind"), out var kind))
            throw new InvalidOperationException("A zero forced Slash changed its exact published producer, target or name.");
        var actor = _coreMatchParticipantsPlayersAccess._players[prompt.PlayerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!RoundOnceCardlessForcedSlashChoices(actor, target, originalId, qinglong).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [selected])))
            throw new InvalidOperationException("A forced zero Slash lost its original legal same-target use and shared allowance.");
        var conversion = CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected);
        _coreCardConversionsOperations.CaptureSelectedResponseConversion(selected);
        if (qinglong)
        {
            var old = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!.Attack;
            _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade = null;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeCardUseFramesOperations.SetCardUseStep(old.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinglongCrescentBladeResolvedEvent(old.ResolutionId, actor.Seat, target.Seat, Used: true, SlashCardIds: [], EffectiveSlashKind: kind));
            _coreQinglongInsertionOperations.SuspendContinuationForQinglongFollowup(old.ResolutionId);
            BeginRoundOnceCardlessForcedSlash(actor, target, kind, conversion, null, false);
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
            BeginRoundOnceCardlessForcedSlash(actor, target, kind, conversion, original, true);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advance)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        return true;
    }

    internal void BeginRoundOnceCardlessForcedSlash(CharacterState actor, CharacterState target, CardKind kind, CardConversionSource source, NativeOwners.BorrowedSwordHandle? borrowed, bool countsTowardLimit)
    {
        _coreRoundOnceCardlessBasicOperations.ConsumeRoundOnceCardlessBasic(source, false);
        var card = new Card(0, kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [target.Seat], kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind), physicalCardIds: [], conversionSource: source);
        IssueRoundOnceCardlessNativeUse(id, source);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, target.Seat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, target, kind));
        if (borrowed is not null)
            borrowed.ActiveAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        if (countsTowardLimit && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source))
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, target.Seat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [target.Seat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }
}
