#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQinLangOlRuntime
{
    internal IReadOnlyList<GameEngine.QinLangUseOption> QinLangUseOptions(ProgramSkillFrame f, QinLangXianyingGrantedEvent grant)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var kind = grant.Name;
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !QinLangImitable(kind) || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return[];
        var result = new List<GameEngine.QinLangUseOption>();
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, p, new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: []) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, kind, [p.Seat]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, Suit.None)))
                result.Add(new(LegalActionKind.Slash, [target.Seat], null, CardKind.Dodge, $"视为对{(target.Name)}使用【杀】"));
        }
        else if (kind is CardKind.Peach or CardKind.Alcohol)
        {
            if ((kind != CardKind.Peach || _coreTreatedWoundedOperations.IsRuleWounded(actor)) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None, null) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind))
                result.Add(new(kind == CardKind.Peach ? LegalActionKind.Peach : LegalActionKind.Alcohol, [actor.Seat], null, null, $"视为使用【{(CardCatalog.Get(kind).DisplayName)}】"));
        }
        else
        {
            IReadOnlyList<GameEngine.ProgramOrdinaryTrickUseOption> options;
            if (kind == CardKind.FiveGrains)
            {
                var targets = Enumerable.Range(0, _coreMatchParticipantsPlayersAccess._players.Count).Select(i => _coreTableOrderOperations.TableStepSeat(actor.Seat, i)).Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive && (!_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor.Seat, SkillProgramCardTargetRestriction.SelfOnly) || s == actor.Seat) && !_coreProgramCardPoliciesOperations.HasCardPolicy(_coreMatchParticipantsPlayersAccess._players[s], SkillProgramCardPolicyKind.ExcludeGlobalTarget, kind) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, s, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[s], kind, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, s, Suit.None)).ToArray();
                options = targets.Length == 0 || _coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, kind, targets) || _coreIssuedPlayUsePoliciesOperations.HasIssuedPlayPhaseUseBan(actor.Seat) || _coreNextActualUseTargetAdjustmentsOperations.IsNextActualUseTrickForbidden(actor.Seat, kind) ? [] : _coreNextActualUseTargetAdjustmentsOperations.NextActualUseOrdinaryTrickOptions(actor, Array.AsReadOnly(new[] { new GameEngine.ProgramOrdinaryTrickUseOption(new("xianying.five-grains"), kind, LegalActionKind.FiveGrains, Array.AsReadOnly(targets), null, null, "视为使用【五谷丰登】") }), Suit.None, null, Suit.None, false);
            }
            else
                options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true);
            foreach (var option in options)
                result.Add(new(option.ActionKind, option.TargetSeats.Count == 0 && kind == CardKind.DrawTwo ? [actor.Seat] : option.TargetSeats, option.TargetCardId, option.RequiredCardKind, option.Description));
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal bool IsQinLangXianyingSlashQuery(CharacterState actor) => HasQinLangCapability && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault()is { QinLangXianying: { Stage: QinLangXianyingStage.EndChoice } r } f && f.OwnerSeat == actor.Seat && r.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Grants.Any(g => !QinLangGrantConsumed(g.FrameId)) && ValidQinLang(f);
    internal void BeginQinLangXianyingUse(ProgramSkillFrame f, QinLangXianyingUseOrigin origin, IReadOnlyList<int> targets)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !MatchesQinLangXianying(f, origin) || origin.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 || string.Join(',', targets) != origin.TargetSignature)
            throw new InvalidOperationException("Xianying lost its exact committed grant and selected native use.");
        var kind = origin.CardKind;
        var actor = _coreMatchParticipantsPlayersAccess._players[origin.ActorSeat];
        var card = new Card(0, kind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, kind, physicalCardIds: [], qinLangXianying: origin);
        if (id != origin.CardUseFrameId)
            throw new InvalidOperationException("Xianying lost its reserved native use identity.");
        if (kind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Recovery, 1));
            return;
        }

        if (kind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var currentTargets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, currentTargets[0], null, 1 + actor.AlcoholDamageBonus, kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[currentTargets[0]], kind));
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, kind, actor.Seat, currentTargets[0]));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, currentTargets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, origin.ActionKind, origin.TargetCardId, origin.RequiredCardKind, kind);
    }

    internal bool MatchesQinLangXianying(ProgramSkillFrame f, QinLangXianyingUseOrigin o) => f.QinLangXianying is { Stage: QinLangXianyingStage.NativeUse, Use: { } expected } r && o == expected && o.ProgramFrameId == f.Id && o.Source == r.Source && o.GameplayHash == r.GameplayHash && o.ActualTurn == r.ActualTurn && o.ActorSeat == f.OwnerSeat && r.Grants.Any(g => g.FrameId == o.GrantFrameId && g.Name == o.CardKind) && ValidQinLang(f);
    internal bool IsQinLangXianyingUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, PhysicalCardIds.Count: 0, QinLangXianying: { } o, Action: { Type: CardActionType.Use } current } use)
            return false;
        var a = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            if (!CoreNativeCardClassificationsRuntime.IsSlashCard(o.CardKind) || fire.OriginalKind != o.CardKind || fire.OriginalAction.ActionId != current.ActionId)
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            a = fire.OriginalAction;
        }
        else if (use.CardKind != o.CardKind)
            return false;
        if (o.CardUseFrameId != id || a.EffectiveKind != o.CardKind || a.PhysicalCards.Count != 0 || a.ConversionChain.Count != 0 || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed != false || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || a.ResponderSeat is not null || a.OpponentSeat is not null || !current.TargetSeats.SequenceEqual(use.TargetSeats) || !_coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.ActorSeat, o.ActorSeat))
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MatchesQinLangXianying(f, o);
    }

    internal bool HasIssuedQinLangXianyingUse(long id) => IsQinLangXianyingUse(id);
    internal bool MatchesQinLangXianyingUseAction(long id, CardActionContext a, int cardId) => cardId == 0 && IsQinLangXianyingUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action is { } issued && a.ActionId == issued.ActionId && a.Type == CardActionType.Use && a.EffectiveKind == issued.EffectiveKind && a.ActorSeat == issued.ActorSeat && a.ProviderSeat == issued.ProviderSeat && a.PhysicalCards.Count == 0 && a.ConversionChain.SequenceEqual(issued.ConversionChain) && a.TargetSeats.SequenceEqual(issued.TargetSeats) && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.EffectiveIsRed == false;
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 140)]
    internal bool FinishQinLangXianyingAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (!IsQinLangXianyingUse(attack.ResolutionId) || attack.Card is not null || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind is not (CardKind.Duel or CardKind.FireAttack))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)!;
        _coreNativeCardUseFramesOperations.SetCardUseStep(use.Id, ResolutionFrameStep.Completed);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseFinishedEvent(use.Id, 0, use.CardKind));
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, ProgramCardContinuation.CompletedSlash, cardUseCausedDamage: attack.CardUseCausedDamage))
            paused = true;
        else
        {
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(use.Id);
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        }

        return true;
    }

    internal bool ContinueQinLangXianyingSimpleUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (!IsQinLangXianyingUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = new Card(0, use.CardKind, Suit.None, 0);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (continuation.Effect == SimpleCardUseEffect.Recovery && use.CardKind == CardKind.Peach)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, id, use.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (continuation.Effect == SimpleCardUseEffect.Alcohol && use.CardKind == CardKind.Alcohol)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        else
            throw new InvalidOperationException("Xianying changed its exact zero-material simple card effect.");
        return true;
    }

    [NativeUseValidation(27170)]
    internal void ValidateQinLangUse(CardUseFrame use)
    {
        if (use.QinLangXianying is not null && !IsQinLangXianyingUse(use.Id))
            throw new InvalidOperationException("Xianying lost its genuine zero-material issuance and exact owning grant.");
    }

    [NativeUseReturn(27170)]
    internal void ReturnQinLangUse(CardUseFrame use)
    {
        if (use.QinLangXianying is not { } origin)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !MatchesQinLangXianying(f, origin))
            throw new InvalidOperationException("Xianying requires its exact completed native use before returning once.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new QinLangXianyingUseReturnedEvent(origin));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { QinLangXianying = f.QinLangXianying!with { Stage = QinLangXianyingStage.NativeChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (use.CardAttack is not { Active: true })
            ContinueQinLang(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 27170)]
    internal bool ReturnQinLangAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { QinLangXianying.Stage: QinLangXianyingStage.NativeChildren } f)
            return false;
        ContinueQinLang(f.Id);
        return true;
    }

    internal void ExpireQinLangXianyingTurnEndWine()
    {
        if (!HasQinLangCapability)
            return;
        var ids = _coreProgramEventHistoryOperations.ProgramEventHistory<QinLangXianyingUseIssuedEvent>().Where(e => e.Origin.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.Origin.CardKind == CardKind.Alcohol).Select(e => e.Origin.CardUseFrameId).ToHashSet();
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        foreach (var entry in facts.OfType<AlcoholAppliedEvent>().Where(e => ids.Contains(e.ResolutionId)).GroupBy(e => e.SourceSeat))
        {
            var last = entry.Last();
            var lastAny = facts.OfType<AlcoholAppliedEvent>().LastOrDefault(e => e.SourceSeat == entry.Key);
            if (lastAny == last && _coreMatchParticipantsPlayersAccess._players[entry.Key].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[entry.Key].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(entry.Key));
            }
        }
    }

    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 70)]
    internal bool QinLangXianyingSlashQuotaBeforeLimit(ProgramSlashQuotaQuery query) => IsQinLangXianyingSlashQuery(query.Actor);
}
