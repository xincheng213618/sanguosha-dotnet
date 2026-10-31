#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanLeRuntime
{
    internal IReadOnlyList<DiouUsePlan> DiouNativePlans(CharacterState actor, CardKind kind, CardConversionSource source)
    {
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(actor.Seat, kind, [], source)) || _characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actor.Seat, [], null))
            return[];
        var result = new List<DiouUsePlan>();
        void Add(LegalActionKind action, IReadOnlyList<int> targets, string text, int? card = null, CardKind? required = null, bool adjusted = false) => result.Add(new(kind, action, card, required, text, adjusted) { Targets = targets });
        if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
        {
            var options = _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: false, conversionSource: source).Where(p => p.ActionKind != LegalActionKind.Recast).ToArray();
            var ordinary = options.Select(p => new LegalAction(p.ActionKind, 0, p.TargetSeats.Count == 1 ? p.TargetSeats[0] : null, p.Description, kind, p.TargetCardId, p.TargetSeats) { ConversionSource = source }).ToList();
            // Use the same genuine zero-material adjustment producers as
            // native actions. Both existing grant types must publish their
            // legal ordered targets before the choice is accepted and paid.
            _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(ordinary, actor);
            _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(ordinary, actor);
            foreach (var a in ordinary)
                Add(a.Kind, a.TargetSeats, a.Description, a.TargetCardId, options.FirstOrDefault()?.RequiredCardKind, _coreRedAdditionalTargetsOperations.IsTargetAdjustmentAction(actor, a));
            return Array.AsReadOnly(result.ToArray());
        }

        var neutral = new Card(0, kind, Suit.None, 0);
        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var actions = new List<LegalAction>();
            var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, physicalCardIds: [], conversionSource: source)).ToArray();
            actions.AddRange(targets.Select(t => new LegalAction(LegalActionKind.Slash, 0, t.Seat, $"视为对{(t.Name)}使用【{(neutral.DisplayName)}】", kind) { ConversionSource = source }));
            _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(actions, actor, neutral, targets, neutral.DisplayName, kind, source, null);
            _coreNextCardTargetAdjustmentsOperations.AddNextCardTargetAdjustmentActions(actions, actor);
            _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(actions, actor);
            foreach (var a in actions)
                Add(a.Kind, a.TargetSeats, a.Description, adjusted: a.ProgramActivationId is not null);
        }
        else if (kind is CardKind.Peach or CardKind.Alcohol)
        {
            var selfLegal = !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None);
            if (selfLegal && kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor))
                Add(LegalActionKind.Peach, [actor.Seat], "视为对自己使用【桃】");
            if (selfLegal && kind == CardKind.Alcohol && !actor.HasAlcoholEffect && (_characterHuangQuanOlOperations.CanUseJianjiOrdinaryAlcohol(actor, null) || _coreTurnTargetCardQuotaAllowancesOperations.HasTargetCardQuotaAllowance(actor.Seat, actor.Seat) || _coreAdvancedPoliciesOperations.HasNextUnlimitedCard(actor) || _characterZhugeLiangMouOperations.HasMouWujingAllowance(actor) || _coreAdvancedPoliciesOperations.HasFriendQihuiAllowance(actor) || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.UnlimitedAlcoholUse, kind)))
                Add(LegalActionKind.Alcohol, [actor.Seat], "视为对自己使用【酒】");
        }
        else if (kind is CardKind.Zong or CardKind.RealgarWine)
        {
            var actions = new List<LegalAction>();
            _coreDragonBoatCardsOperations.AddDragonBoatPhysicalOrConvertedActions(actions, actor, neutral, kind, source);
            foreach (var a in actions)
                Add(a.Kind, a.TargetSeats.Count > 0 ? a.TargetSeats : [actor.Seat], a.Description);
        }
        else if (kind == CardKind.Intoxication)
        {
            var actions = new List<LegalAction>();
            _coreGuanduCardsOperations.AddGuanduPhysicalOrConvertedActions(actions, actor, neutral, kind, source);
            foreach (var a in actions)
                Add(a.Kind, a.TargetSeats, a.Description);
        }
        else if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) && kind != CardKind.GeneralWeapon)
        {
            if (actor.EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(kind).Slot) > 0 && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, Suit.None) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, actor.Seat, kind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, Suit.None))
                Add(LegalActionKind.Equip, [actor.Seat], $"视为使用并装备【{(neutral.DisplayName)}】");
        }
        else if (CoreNativeCardClassificationsRuntime.IsDelayedCard(kind))
        {
            foreach (var t in _coreMatchParticipantsPlayersAccess._players.Where(t => t.IsAlive && !t.JudgmentAreaAbolished && !_coreActualTurnsOperations.HasJudgmentEffectiveCard(t, kind) && !_coreProgramCardPoliciesOperations.HasCardPolicy(t, SkillProgramCardPolicyKind.ProhibitDelayedTrickTarget) && (kind == CardKind.Lightning ? t.Seat == actor.Seat : t.Seat != actor.Seat) && (kind != CardKind.SupplyShortage || _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.IgnoreUseDistance, kind) || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(actor, t, kind) || _coreNativeRuleQueriesOperations.GetCombatDistance(actor.Seat, t.Seat) <= _coreRuleQueriesOperations.GetCardUseDistanceLimit(actor, kind)) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, t.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, Suit.None) && !_coreShenwuEquipmentOperations.IsShenwuRobeTargetProhibited(actor.Seat, t.Seat, kind) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t.Seat, Suit.None)))
                if (_coreCardUseModulesOperations.ApplyTurnCardGroupTargetRestrictions(actor, [t.Seat]).Contains(t.Seat))
                    Add(CoreGameEngineDelayedPlacementTargetProgramsRuntime.DelayedPlacementActionKind(kind), [t.Seat], $"视为对{(t.Name)}使用【{(neutral.DisplayName)}】");
        }

        return Array.AsReadOnly(result.ToArray());
    }

    internal void DispatchDiou(ProgramSkillFrame f, DiouUsePlan p)
    {
        var r = f.Diou!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!r.Paid || r.Stage != DiouStage.UseChoice || p.Kind != r.OutputKind || !DiouNativePlans(actor, r.OutputKind, DiouSource(f)).Any(o => SameDiouPlan(o, p)))
            throw new InvalidOperationException("Diou cannot issue another card name or bypass current native legality.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Diou = r with { Stage = DiouStage.NativeIssuing, Plan = p } });
        r = f.Diou!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouPlanAcceptedEvent(f.Id, p.Kind, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Adjusted, p.Targets.Count));
        for (var i = 0; i < p.Targets.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouTargetAcceptedEvent(f.Id, i, p.Targets[i]));
        Card card;
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(p.Kind) || CoreNativeCardClassificationsRuntime.IsDelayedCard(p.Kind))
        {
            var id = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1);
            card = new(id, p.Kind, Suit.None, 0);
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Diou = r with { CarrierId = id } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouCarrierCreatedEvent(f.Id, DiouSource(f), f.GameplayHash, id, p.Kind));
        }
        else
            card = new(0, p.Kind, Suit.None, 0);
        if (p.Adjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = p.Targets;
        var source = DiouSource(f);
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

        var useId = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, p.Targets, p.Kind, physicalCardIds: [], conversionSource: source);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.DiouOrigin is null)
            throw new InvalidOperationException("Diou must bind its distinct zero-material issuance before native effects.");
        if (card.Id > 0)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.OutsideGame, CardLocation.Processing, new("skill-program.le-diao-chan.diou.carrier"));
        if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(p.Kind))
        {
            if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(actor, card, useId))
                _coreNativeEquipmentUseOperations.CompleteEquipmentUse(actor, card, useId);
            return;
        }

        if (p.Kind == CardKind.Alcohol)
        {
            _characterHuangGaiMouMobileOperations.RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(0, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(p.Kind))
        {
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.TargetSeats;
            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targets[0], card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: p.Kind, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], p.Kind), programSkillCardUseFrameId: f.Id);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.UnlimitedUse != true && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, source))
                _coreCardUseDebitsOperations.RecordSlashUseDebit(useId, actor.Seat);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, p.Kind);
            _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, actor);
            actor.HasAlcoholEffect = false;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, p.Kind, actor.Seat, targets[0], IgnoresArmor: attack.IgnoresArmor));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.TargetSeats, p.ActionKind, p.TargetCardId, p.RequiredKind, p.Kind);
    }

    internal static bool SameDiouPlan(DiouUsePlan a, DiouUsePlan b) => RuntimeSkills.RuntimeRules_diao_chan_le_LeDiaoChanDiou_Native.SameDiouPlan(a, b);
    internal CardActionContext PrepareDiouAction(CardActionContext action)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Diou: { Stage: DiouStage.NativeIssuing } r } f)
            return action;
        if (!r.Paid || r.Plan is not { } plan || !DiouRootMatches(f) || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.RequesterSeat is not null || action.PhysicalCards.Count != 0 || action.EffectiveKind != plan.Kind || !action.ConversionChain.SequenceEqual([DiouSource(f)]))
            throw new InvalidOperationException("Diou cannot normalize another producer's action or replace any genuine material appearance.");
        // Generic CaptureCardUseAction has no rank witness for an empty
        // material set and may leave suit/rank unspecified. Like the other
        // actual zero-material producers, Diou explicitly signs its neutral
        // appearance before policies, accepted facts and native issuance.
        return _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat, action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, action.TargetSeats, action.PhysicalCards, action.ConversionChain, action.DesignatedTargetSeats, effectiveSuit: Suit.None, effectiveRank: 0, effectiveIsRed: null));
    }

    internal void BindDiouNativeUse(CardUseFrame use)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { Diou: { Stage: DiouStage.NativeIssuing } r } f)
            return;
        if (!DiouRootMatches(f) || !r.Paid || r.Plan is not { } plan || use.DiouOrigin is not null || use.Action is not { Type: CardActionType.Use } a || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.RequesterSeat is not null || a.PhysicalCards.Count != 0 || use.PhysicalCardIds is not { Count: 0 } || a.EffectiveKind != r.OutputKind || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || !a.ConversionChain.SequenceEqual([DiouSource(f)]) || use.CardId != r.CarrierId || !a.TargetSeats.SequenceEqual(use.TargetSeats) || !a.TargetSeats.SequenceEqual(_coreFinalizedCardTargetsOperations.GetImplicitSelfCardUseTargets(plan.Kind, f.OwnerSeat, plan.Targets)))
            throw new InvalidOperationException("Diou lost its exact separate zero-material native Use and retained-Hand source.");
        var origin = new DiouNativeOrigin(f.Id, DiouSource(f), f.GameplayHash, use.Id, a.ActionId, f.OwnerSeat, r.OutputKind, r.CarrierId);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { DiouOrigin = origin });
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Diou = r with { Stage = DiouStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouNativeIssuedEvent(f.Id, use.Id, a.ActionId, f.OwnerSeat, r.OutputKind, r.CarrierId));
    }

    internal bool IsIssuedDiouUse(CardUseFrame use)
    {
        if (use.DiouOrigin is not { } o || use.Action is not { Type: CardActionType.Use } a || o.UseId != use.Id || o.ActionId != a.ActionId || o.Kind != use.CardKind || a.EffectiveKind != use.CardKind || use.SourceSeat != a.ActorSeat || use.CardId != o.CarrierId || a.ProviderSeat != o.ActorSeat || a.RequesterSeat is not null || a.PhysicalCards.Count != 0 || use.PhysicalCardIds is not { Count: 0 } || a.EffectiveSuit != Suit.None || a.EffectiveRank != 0 || a.EffectiveIsRed is not null || !a.ConversionChain.SequenceEqual([o.Source]) || !_characterZhouYuOlMouOperations.MouGuquActorLineageMatches(use.Id, a.ActionId, o.ActorSeat, a.ActorSeat, a.ProviderSeat, a.Type) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.ProgramFrameId)is not ProgramSkillFrame { Diou: { } r } f || !DiouRootMatches(f) || r.UseId != use.Id || r.ActionId != a.ActionId || r.CarrierId != o.CarrierId || o.Source != DiouSource(f) || o.GameplayHash != f.GameplayHash)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<DiouNativeIssuedEvent>().Count(e => e == new DiouNativeIssuedEvent(f.Id, use.Id, a.ActionId, o.ActorSeat, o.Kind, o.CarrierId)) == 1 && (o.CarrierId == 0 || IsDiouCarrier(o.CarrierId));
    }

    internal bool IsDiouUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && IsIssuedDiouUse(use);
    internal bool IsDiouZeroUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { CardId: 0 } use && IsIssuedDiouUse(use);
    internal Card DiouRepresentation(long id) => IsDiouZeroUse(id) ? new(0, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind, Suit.None, 0) : throw new InvalidOperationException("Diou lost its zero-material native card representation.");
    internal Card ReadDiouUseAppearance(long id, CardAppearanceReference appearance)
    {
        if (!IsDiouZeroUse(id))
            return _coreDrawFundedDistinctBasicReturnsOperations.ReadDrawFundedDistinctBasicUseAppearance(id, appearance);
        if (appearance.Id != 0 || appearance.Kind != _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind || appearance.Suit != Suit.None || appearance.Rank != 0)
            throw new InvalidOperationException("Diou's native continuation changed its exact neutral zero-material appearance.");
        return DiouRepresentation(id);
    }

    internal bool SkipDiouFinishedMovement(long id, Card c) => c.Id == 0 && IsDiouZeroUse(id) && c.Kind == _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.CardKind;
    internal bool ContinueDiouSimpleUse(long id, ProgramSimpleCardContinuation continuation)
    {
        if (continuation.CardId != 0 || !IsDiouUse(id))
            return false;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = DiouRepresentation(id);
        _coreNativeCardUseFramesOperations.SetCardUseStep(id, ResolutionFrameStep.ResolvingEffect);
        if (!actor.IsAlive || _coreMatchOutcomeWinnerAccess._winner != Winner.None)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, use.CardKind);
            return true;
        }

        if (continuation.Effect == SimpleCardUseEffect.Recovery && use.CardKind is CardKind.Peach or CardKind.Zong)
            _coreNativeRecoveryCardsOperations.CompleteRecoveryCardUse(actor, _coreMatchParticipantsPlayersAccess._players[use.TargetSeats[use.TargetIndex]], card, id, use.CardKind, continuation.RecoveryAmount, continuation.RecoveryPolicySources ?? []);
        else if (continuation.Effect == SimpleCardUseEffect.Alcohol && use.CardKind is CardKind.Alcohol or CardKind.RealgarWine)
            _coreNativeTrickUseOperations.CompleteAlcoholUse(actor, card, id);
        else
            throw new InvalidOperationException("Diou's zero-material simple effect changed its accepted native card name.");
        return true;
    }

    [ProgramDamageHook(ProgramDamagePhase.IssuedAttackCompletion, 30)]
    internal bool FinishDiouAttack(NativeOwners.CardAttackHandle attack, out bool paused)
    {
        paused = false;
        if (attack.Card is not null || !IsDiouUse(attack.ResolutionId) || attack.EffectiveCardKind is not { } kind || !CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && kind is not (CardKind.Duel or CardKind.FireAttack))
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

    [NativeUseValidation(29042)]
    internal void ValidateDiouNativeUse(CardUseFrame use)
    {
        if (use.DiouOrigin is not null && (!IsIssuedDiouUse(use) || use.Step != ResolutionFrameStep.Completed || use.PendingRecoveryAttempts is { Count: > 0 } || use.RecoveryPaidContinuation is not null || use.DiouOrigin.CarrierId > 0 && _cardZonesOperations._cardZones.GetLocation(use.DiouOrigin.CarrierId) == CardLocation.Processing))
            throw new InvalidOperationException("Diou cannot return before its genuine whole native Use and carrier placement or retirement.");
    }

    [NativeUseReturn(29042)]
    internal void ReturnDiouNativeUse(CardUseFrame use)
    {
        if (use.DiouOrigin is not { } o)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Diou: { } r } f || f.Id != o.ProgramFrameId || r.UseId != use.Id || r.ActionId != o.ActionId || r.Returned || _coreProgramEventHistoryOperations.ProgramEventHistory<DiouNativeReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Diou can return only once to its original paid native parent.");
        var batches = SignDiouNativeBatches(f, use);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Diou = r with { Stage = DiouStage.NativeChildren, Returned = true, NativeBatchIds = batches }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouNativeReturnedEvent(f.Id, use.Id, o.ActionId));
    }

    internal IReadOnlyList<long> SignDiouNativeBatches(ProgramSkillFrame f, CardUseFrame use)
    {
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.ParentFrameId == use.Id && b.AwaitingProgramFrameId is null && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat).ToArray();
        foreach (var b in batches)
        {
            if (b.Movements.Count == 0 || b.Movements.Any(m => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= f.Diou!.ShowSequence || m.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) || b.NativeDrawInvocation is { } draw && !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw))
                throw new InvalidOperationException("Diou's retired native Use lost its exact original movement and draw invoices.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouNativeBatchSignedEvent(f.Id, use.Id, b.Id, b.ParentBatchId, b.Movements.Count));
            foreach (var m in b.Movements)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiouNativeMovementSignedEvent(f.Id, use.Id, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
        }

        return Array.AsReadOnly(batches.Select(b => b.Id).ToArray());
    }

    internal bool IsDiouNativeSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.Diou is { Stage: DiouStage.NativeChildren, Returned: true } r && DiouRootMatches(f) && b.ParentFrameId == r.UseId && b.AwaitingProgramFrameId is null && b.Movements.Count > 0 && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.OriginOwnerSeat == f.OwnerSeat && r.NativeBatchIds.Contains(b.Id) && _coreProgramEventHistoryOperations.ProgramEventHistory<DiouNativeBatchSignedEvent>().Count(e => e == new DiouNativeBatchSignedEvent(f.Id, r.UseId, b.Id, b.ParentBatchId, b.Movements.Count)) == 1 && b.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<DiouNativeMovementSignedEvent>().Count(e => e == new DiouNativeMovementSignedEvent(f.Id, r.UseId, b.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 800)]
    internal bool WindowSettlement_IsDiouNativeSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsDiouNativeSettlementBatch(frame, batch);
}
