#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    internal IReadOnlyList<MouXiaoQiaoOlUsePlan> MouXqNativePlans(CharacterState actor, CardKind kind, CardConversionSource source)
    {
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, kind, [], source)) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [], null))
            return[];
        var actions = new List<LegalAction>();
        var required = new Dictionary<(LegalActionKind, int? ), CardKind?>();
        if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
        {
            foreach (var p in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: false, conversionSource: source).Where(p => p.ActionKind != LegalActionKind.Recast))
            {
                actions.Add(new(p.ActionKind, 0, p.TargetSeats.Count == 1 ? p.TargetSeats[0] : null, p.Description, kind, p.TargetCardId, p.TargetSeats) { ConversionSource = source });
                required[(p.ActionKind, p.TargetCardId)] = p.RequiredCardKind;
            }
        }
        else if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var card = new Card(0, kind, Suit.None, 0);
            var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, physicalSuit: Suit.None, effectiveColor: null, effectiveRank: 0, physicalCardIds: [], conversionSource: source)).ToArray();
            actions.AddRange(targets.Select(t => new LegalAction(LegalActionKind.Slash, 0, t.Seat, $"绝颜：对{(t.Name)}使用【{(card.DisplayName)}】", kind) { ConversionSource = source }));
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, card, targets, card.DisplayName, kind, source, null);
        }
        else if (kind is CardKind.Peach or CardKind.Alcohol)
        {
            var legal = !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None);
            if (legal && kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor))
                actions.Add(new(LegalActionKind.Peach, 0, actor.Seat, "绝颜：视为对自己使用【桃】", kind) { ConversionSource = source });
            if (legal && kind == CardKind.Alcohol && !actor.HasAlcoholEffect && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, kind)))
                actions.Add(new(LegalActionKind.Alcohol, 0, actor.Seat, "绝颜：视为对自己使用【酒】", kind) { ConversionSource = source });
        }
        else if (kind is CardKind.Zong or CardKind.RealgarWine)
            _coreDragonBoatCardsOperations.AddDragonBoatPhysicalOrConvertedActions(actions, actor, new(0, kind, Suit.None, 0), kind, source);
        else if (kind == CardKind.Intoxication)
            _coreGuanduCardsOperations.AddGuanduPhysicalOrConvertedActions(actions, actor, new(0, kind, Suit.None, 0), kind, source);
        _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(actions, actor);
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
        return Array.AsReadOnly(_coreCardUseModulesOperations.FilterTurnCardUseRestrictions(actor, actions).Select(a => new MouXiaoQiaoOlUsePlan(kind, a.Kind, a.TargetCardId, required.GetValueOrDefault((a.Kind, a.TargetCardId)), a.Description, _coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, a)) { Targets = a.TargetSeats }).ToArray());
    }

    internal CardActionContext PrepareMouXqJueyanAction(CardActionContext action)
    {
        var prepared = MouXiaoQiaoOl.PrepareNativeAction(action);
        return ReferenceEquals(prepared, action) ? action : _coreProgramFactionActionRewardOperations.CaptureFactionAction(prepared);
    }

    internal bool IsMouXqJueyanUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && MouXiaoQiaoOl.IsIssuedUse(use);
    internal Card MouXqJueyanRepresentation(long id) => IsMouXqJueyanUse(id) ? new(0, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind, Suit.None, 0) : throw new InvalidOperationException("Jueyan's zero-material native use lost its actual origin.");
    internal bool SkipMouXqJueyanFinishedMovement(long id, Card c) => c.Id == 0 && IsMouXqJueyanUse(id) && c.Kind == _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind;
    internal Card ReadMouXqJueyanAppearance(long id, CardAppearanceReference a)
    {
        if (!IsMouXqJueyanUse(id))
            return _characterDiaoChanLeOperations.ReadDiouUseAppearance(id, a);
        var c = MouXqJueyanRepresentation(id);
        if (a.Id != 0 || a.Kind != c.Kind || a.Suit != Suit.None || a.Rank != 0)
            throw new InvalidOperationException("Jueyan cannot inherit appearance from its retained original entity.");
        return c;
    }

    internal void IssueMouXqNativeUse(ProgramSkillFrame f, MouXiaoQiaoOlUsePlan p)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var source = f.MouXiaoQiaoOl!.Source;
        var card = new Card(0, p.Kind, Suit.None, 0);
        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        if (p.Kind is CardKind.Zong or CardKind.RealgarWine)
        {
            _coreDragonBoatCardsOperations.ResolveDragonBoatCard(actor, card, new(p.ActionKind, 0, p.Targets.FirstOrDefault(), p.Description, p.Kind, TargetSeats: p.Targets) { ConversionSource = source });
            return;
        }

        if (p.Kind == CardKind.Intoxication)
        {
            _coreGuanduCardsOperations.ResolveGuanduCard(actor, card, new(p.ActionKind, 0, p.Targets.FirstOrDefault(), p.Description, p.Kind, TargetSeats: p.Targets) { ConversionSource = source });
            return;
        }

        if (p.Kind == CardKind.Peach)
        {
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(actor, actor, card, "桃", p.Kind, conversionSource: source, physicalCards: []);
            return;
        }

        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, p.Targets, p.Kind, physicalCardIds: [], conversionSource: source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouXiaoQiaoOlOrigin is null)
            throw new InvalidOperationException("Jueyan must bind its own exact material-free issuance.");
        if (p.Kind == CardKind.Alcohol)
        {
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(id, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], p.Kind), programSkillCardUseFrameId: f.Id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source))
                _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal bool ContinueMouXqJueyanSimpleUse(long id, ProgramSimpleCardContinuation c)
    {
        if (c.CardId != 0 || !IsMouXqJueyanUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = MouXqJueyanRepresentation(id);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, use.CardKind);
            return true;
        }

        if (c.Effect == SimpleCardUseEffect.Recovery && use.CardKind is CardKind.Peach or CardKind.Zong)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, id, use.CardKind, c.RecoveryAmount, c.RecoveryPolicySources ?? []);
        else if (c.Effect == SimpleCardUseEffect.Alcohol && use.CardKind is CardKind.Alcohol or CardKind.RealgarWine)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        else
            throw new InvalidOperationException("Jueyan changed its exact accepted native simple effect.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 34000)]
    internal bool FinishMouXqJueyanAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsMouXqJueyanUse(attack.ResolutionId) || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind is not (CardKind.Duel or CardKind.FireAttack))
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

    internal void SignMouXqNativeBatches(ProgramSkillFrame f, CardUseFrame use)
    {
        foreach (var b in _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat))
        {
            if (b.Movements.Count == 0 || b.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= f.MouXiaoQiaoOl!.Before || m.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) || b.NativeDrawInvocation is { } draw && !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw))
                throw new InvalidOperationException("Jueyan's native Use lost its actual movement and draw invoices.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiaoQiaoJueyanBatchSignedEvent(f.Id, use.Id, b.Id, b.ParentBatchId, b.Movements.Count));
            foreach (var m in b.Movements)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiaoQiaoJueyanMovementSignedEvent(f.Id, use.Id, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        }
    }

    internal bool IsMouXqNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.MouXiaoQiaoOl is { Stage: MouXiaoQiaoOlStage.NativeChildren, NativeReturned: true } r && r.Source.SkillId == MouXiaoQiaoOlRuntime.Jueyan && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanBatchSignedEvent>().Count(e => e == new MouXiaoQiaoJueyanBatchSignedEvent(f.Id, r.UseId, b.Id, b.ParentBatchId, b.Movements.Count)) == 1 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiaoQiaoJueyanMovementSignedEvent>().Count(e => e == new MouXiaoQiaoJueyanMovementSignedEvent(f.Id, r.UseId, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 700)]
    internal bool WindowSettlement_IsMouXqNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsMouXqNativeSettlementBatch(frame, batch);
}
