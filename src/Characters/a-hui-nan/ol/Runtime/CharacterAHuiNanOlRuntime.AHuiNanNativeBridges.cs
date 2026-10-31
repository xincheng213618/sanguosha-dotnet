#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterAHuiNanOlRuntime
{
    internal IReadOnlyList<IReadOnlyList<int>> AHuiNanUseTargets(ProgramSkillFrame f)
    {
        var r = f.AHuiNanJueman!;
        if (r.Stage != AHuiNanJuemanStage.UseChoice || r.History.Count != 3)
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var kind = r.History[2].Kind;
        if (!actor.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return[];
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseSlashTarget(actor, p, new(0, kind, Suit.None, 0), effectiveKind: kind, noEffectiveRank: true, physicalCardIds: []) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, kind, [p.Seat]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(p, kind, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, p.Seat, Suit.None)).Select(p => (IReadOnlyList<int>)Array.AsReadOnly(new[] { p.Seat })).ToArray());
        if (kind is CardKind.Peach or CardKind.Alcohol && (kind != CardKind.Peach || _coreTreatedWoundedOperations.IsRuleWounded(actor)) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None, null) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind))
            return Array.AsReadOnly(new IReadOnlyList<int>[] { Array.AsReadOnly(new[] { actor.Seat }) });
        return[];
    }

    internal bool IsAHuiNanJuemanSlashQuery(CharacterState actor) => HasAHuiNanCapability && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AHuiNanJueman: { Stage: AHuiNanJuemanStage.UseChoice } r } f && f.OwnerSeat == actor.Seat && r.History.Count == 3 && CoreNativeCardClassificationsRuntime.IsSlashCard(r.History[2].Kind) && ValidAHuiNan(f);
    internal bool MatchesAHuiNanJueman(ProgramSkillFrame f, AHuiNanJuemanUseOrigin o) => f.AHuiNanJueman is { Stage: AHuiNanJuemanStage.NativeUse, Use: { } expected } r && o == expected && o.ProgramFrameId == f.Id && o.Source == r.Source && o.GameplayHash == r.GameplayHash && o.ActualTurn == r.ActualTurn && o.ActorSeat == f.OwnerSeat && r.History.Count == 3 && r.History[2].Kind == o.CardKind && ValidAHuiNan(f);
    internal void BeginAHuiNanJuemanUse(ProgramSkillFrame f, AHuiNanJuemanUseOrigin o, IReadOnlyList<int> targets)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || !MatchesAHuiNanJueman(f, o) || o.CardUseFrameId != _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1 || string.Join(',', targets) != o.TargetSignature)
            throw new InvalidOperationException("Jueman lost its committed third basic-card receipt.");
        var actor = _coreMatchParticipantsPlayersAccess._players[o.ActorSeat];
        var card = new Card(0, o.CardKind, Suit.None, 0);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, o.CardKind, physicalCardIds: [], aHuiNanJueman: o);
        if (id != o.CardUseFrameId)
            throw new InvalidOperationException("Jueman lost its reserved native use identity.");
        if (o.CardKind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Recovery, 1));
            return;
        }

        if (o.CardKind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (!CoreNativeCardClassificationsRuntime.IsSlashCard(o.CardKind))
            throw new InvalidOperationException("Jueman cannot actively use an otherwise unplayable basic card.");
        var current = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, current.TargetSeats[0], null, 1 + actor.AlcoholDamageBonus, o.CardKind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[current.TargetSeats[0]], o.CardKind));
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, o.CardKind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, o.CardKind, actor.Seat, current.TargetSeats[0]));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, current.Action!, SkillProgramTriggerWindow.CardUseCommitted, current.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal bool IsAHuiNanJuemanUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, PhysicalCardIds.Count: 0, AHuiNanJueman: { } o, Action: { Type: CardActionType.Use } current } use)
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
        return i > 0 && _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MatchesAHuiNanJueman(f, o);
    }

    internal bool HasIssuedAHuiNanJuemanUse(long id) => IsAHuiNanJuemanUse(id);
    internal bool MatchesAHuiNanJuemanUseAction(long id, CardActionContext a, int cardId) => cardId == 0 && IsAHuiNanJuemanUse(id) && _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action is { } issued && a.ActionId == issued.ActionId && a.Type == CardActionType.Use && a.EffectiveKind == issued.EffectiveKind && a.ActorSeat == issued.ActorSeat && a.ProviderSeat == issued.ProviderSeat && a.PhysicalCards.Count == 0 && a.ConversionChain.SequenceEqual(issued.ConversionChain) && a.TargetSeats.SequenceEqual(issued.TargetSeats) && a.EffectiveSuit == Suit.None && a.EffectiveRank == 0 && a.EffectiveIsRed == false;
    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 130)]
    internal bool FinishAHuiNanJuemanAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (!IsAHuiNanJuemanUse(attack.ResolutionId) || attack.Card is not null || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
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

    internal bool ContinueAHuiNanJuemanSimpleUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (!IsAHuiNanJuemanUse(id))
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
            throw new InvalidOperationException("Jueman changed its native zero-material basic effect.");
        return true;
    }

    [NativeUseValidation(27200)]
    internal void ValidateAHuiNanUse(CardUseFrame use)
    {
        if (use.AHuiNanJueman is not null && !IsAHuiNanJuemanUse(use.Id))
            throw new InvalidOperationException("Jueman lost its exact printed-third-basic issuance.");
    }

    [NativeUseReturn(27200)]
    internal void ReturnAHuiNanUse(CardUseFrame use)
    {
        if (use.AHuiNanJueman is not { } o)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || !MatchesAHuiNanJueman(f, o))
            throw new InvalidOperationException("Jueman must return once to its exact original actual-ending frame.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AHuiNanJuemanUseReturnedEvent(o));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { AHuiNanJueman = f.AHuiNanJueman!with { Stage = AHuiNanJuemanStage.NativeChildren, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (use.CardAttack is not { Active: true })
            ContinueAHuiNan(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 27200)]
    internal bool ReturnAHuiNanAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AHuiNanJueman.Stage: AHuiNanJuemanStage.NativeChildren } f)
            return false;
        ContinueAHuiNan(f.Id);
        return true;
    }

    internal void ExpireAHuiNanJuemanTurnEndWine()
    {
        if (!HasAHuiNanCapability)
            return;
        var ids = _coreProgramEventHistoryOperations.ProgramEventHistory<AHuiNanJuemanUseIssuedEvent>().Where(e => e.Origin.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Origin.CardKind == CardKind.Alcohol).Select(e => e.Origin.CardUseFrameId).ToHashSet();
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        foreach (var group in facts.OfType<AlcoholAppliedEvent>().Where(e => ids.Contains(e.ResolutionId)).GroupBy(e => e.SourceSeat))
        {
            if (facts.OfType<AlcoholAppliedEvent>().LastOrDefault(e => e.SourceSeat == group.Key) == group.Last() && _coreMatchParticipantsPlayersAccess._players[group.Key].HasAlcoholEffect)
            {
                _coreMatchParticipantsPlayersAccess._players[group.Key].HasAlcoholEffect = false;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AlcoholExpiredEvent(group.Key));
            }
        }
    }
}
