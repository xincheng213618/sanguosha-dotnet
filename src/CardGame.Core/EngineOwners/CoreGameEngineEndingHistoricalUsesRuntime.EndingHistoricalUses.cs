#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEndingHistoricalUsesRuntime
{
    internal bool TracksOwnPlayHistory => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.UseOwnPlayHistoryAtEnding);

    internal void RecordOwnPlayEligibleUse(CardActionContext action)
    {
        if (!TracksOwnPlayHistory || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || action.Type != CardActionType.Use || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(action.EffectiveKind, [SkillProgramCardCategory.Basic, SkillProgramCardCategory.InstantTrick]) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OwnPlayEligibleUseRecordedEvent>().Any(e => e.CardActionId == action.ActionId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new OwnPlayEligibleUseRecordedEvent(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, action.ActorSeat, action.ActionId, action.EffectiveKind));
    }

    internal IReadOnlyList<EndingHistoricalUseSlot> OwnPlayHistory(int owner) => Array.AsReadOnly(_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<OwnPlayEligibleUseRecordedEvent>().Where(e => e.ActualTurn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == owner && e.ActorSeat == owner).Take(2).Select(e => new EndingHistoricalUseSlot(e.CardActionId, e.EffectiveKind, e.PhaseInstanceId)).ToArray());
    internal bool ExactHistoricalEnding(ProgramSkillFrame f) => f.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().LastOrDefault()is { } ending && c.ParentFrameId == ending.Id && ending.OwnerSeat == f.OwnerSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
    internal SkillProgramStepOutcome BeginEndingHistoricalUses(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var slots = OwnPlayHistory(f.OwnerSeat);
        if (f.InstructionIndex != 1 || f.EndingHistoricalUses is not null || !ExactHistoricalEnding(f))
            throw new InvalidOperationException("Historical uses require the original own Ending candidate.");
        if (slots.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { EndingHistoricalUses = new(1, CoreGameEngineDyingSuitsPeachProgramsRuntime.DyingSuitsSource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, f.WindowContext!.ParentFrameId, slots) });
        PublishHistoricalEndingChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<LegalAction> HistoricalEndingActions(ProgramSkillFrame f)
    {
        var r = f.EndingHistoricalUses!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var result = new List<LegalAction>();
        if (r.Complete || r.UseReturn is not null || r.SlotIndex >= r.Slots.Count || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !actor.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            return[];
        var kind = r.Slots[r.SlotIndex].EffectiveKind;
        if (kind is CardKind.Dodge or CardKind.Nullification || _coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use))
            return[];
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(actor, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, [c.Id])))
        {
            var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, card);
            var color = CoreGameEnginePublicPileColorsRuntime.SuitColor(suit);
            if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
            {
                var targets = _coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, kind, suit, color, card.Rank, [card.Id]) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, t.Seat, kind) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(t, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, t.Seat, suit)).ToArray();
                foreach (var target in targets)
                    result.Add(new(LegalActionKind.Slash, card.Id, target.Seat, $"将【{(card.DisplayName)}】当【{(CardCatalog.Get(kind).DisplayName)}】对 {(target.Name)} 使用", kind, TargetSeats: [target.Seat]));
                _coreNativeMultiTargetActionsOperations.AddFangtianHalberdSlashActions(result, actor, card, targets, CardCatalog.Get(kind).DisplayName, kind);
                _coreProgramCardUseRulesOperations.AddProgramTargetCountSlashActions(result, actor, card, targets, CardCatalog.Get(kind).DisplayName, kind);
            }
            else if (kind == CardKind.Peach && _coreTreatedWoundedOperations.IsRuleWounded(actor) && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit))
                result.Add(new(LegalActionKind.Peach, card.Id, null, $"将【{(card.DisplayName)}】当【桃】使用", kind, TargetSeats: [actor.Seat]));
            else if (kind == CardKind.Alcohol && !actor.HasAlcoholEffect && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(actor.Seat) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(actor, kind, suit, color) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actor.Seat, actor.Seat, suit))
                result.Add(new(LegalActionKind.Alcohol, card.Id, null, $"将【{(card.DisplayName)}】当【酒】使用", kind, TargetSeats: [actor.Seat]));
            else if (CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind))
                foreach (var option in _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: color, hasActualColor: true, physicalCardIds: [card.Id], includeNextActualUseAdjustment: true))
                    result.Add(new(option.ActionKind, card.Id, option.TargetSeats.Count == 1 ? option.TargetSeats[0] : null, $"将【{(card.DisplayName)}】{(option.Description)}", kind, option.TargetCardId, option.TargetSeats));
        }

        var basicActions = result.Where(a => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(a.PlayedCardKind!.Value, [SkillProgramCardCategory.Basic])).ToList();
        var basicCount = basicActions.Count;
        _coreNextActualUseTargetAdjustmentsOperations.AddNextActualUseAdjustmentActions(basicActions, actor);
        result.AddRange(basicActions.Skip(basicCount));
        return Array.AsReadOnly(result.ToArray());
    }

    internal IReadOnlyList<PromptChoice> HistoricalEndingChoices(ProgramSkillFrame f)
    {
        var actions = HistoricalEndingActions(f);
        var result = actions.Select((a, i) => new PromptChoice(new($"historical-ending.{(f.Id)}.{(i)}"), a.Description, [a.CardId!.Value], a.TargetSeats ?? (a.TargetSeat is { } t ? [t] : []), new Dictionary<string, string> { ["program-action"] = "historical-ending", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = i.ToString(CultureInfo.InvariantCulture) })).ToList();
        result.Add(new(new($"historical-ending.{(f.Id)}.skip"), "不使用，结束本次技能。", [], [], new Dictionary<string, string> { ["program-action"] = "historical-ending", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = "skip" }));
        return Array.AsReadOnly(result.ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.UseOwnPlayHistoryAtEnding)]
    internal PromptChoice SelectAiHistoricalEndingUse(PendingDecision decision, ProgramSkillFrame f)
    {
        var(index, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat].ChooseHistoricalEndingUse(_corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat), HistoricalEndingActions(f), ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        return decision.Choices.Single(c => c.Parameters["option"] == (index < 0 ? "skip" : index.ToString(CultureInfo.InvariantCulture)));
    }

    internal void PublishHistoricalEndingChoice(ProgramSkillFrame f)
    {
        if (HistoricalEndingActions(f).Count == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var choices = HistoricalEndingChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"请选择第 {(f.EndingHistoricalUses!.SlotIndex + 1)} 次真实用牌，或结束。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("historical-ending")]
    internal void ResolveHistoricalEndingChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.EndingHistoricalUses!;
        var expected = HistoricalEndingChoices(f).SingleOrDefault(c => c.Id == choice.Id);
        if (expected is null || !expected.Cards.SequenceEqual(choice.Cards) || !expected.Targets.SequenceEqual(choice.Targets))
            throw new InvalidOperationException("Historical Ending lost its exact private material and ordered target choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters.GetValueOrDefault("option") == "skip")
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return;
        }

        var index = int.Parse(choice.Parameters["option"], CultureInfo.InvariantCulture);
        var action = HistoricalEndingActions(f)[index];
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var physical = _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Single(c => c.Id == action.CardId);
        var kind = action.PlayedCardKind!.Value;
        var suit = _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(actor, physical);
        var option = CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(kind) ? _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(actor, kind, suit, enforceUsePermission: true, beneficiaryShieldSuit: suit, actualEffectiveColor: CoreGameEnginePublicPileColorsRuntime.SuitColor(suit), hasActualColor: true, physicalCardIds: [physical.Id], includeNextActualUseAdjustment: true).Single(o => o.ActionKind == action.Kind && o.TargetCardId == action.TargetCardId && o.TargetSeats.SequenceEqual(action.TargetSeats!)) : null;
        var adjustedBasic = action.ProgramActivationId == "next-actual-use-target-adjustment";
        var fangtian = CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && action.TargetSeats.Count > 1 && _coreNativeDamageHandlesOperations.Value.UsesFormalFangtianHalberd && _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Count == 1 && _coreAdvancedWeaponsOperations.HasWeaponAbility(actor, CardKind.FangtianHalberd);
        var programTargetCount = CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && action.TargetSeats.Count > 1 && _coreProgramCardUseRulesOperations.UsesProgramCardTargetCount(actor, physical, kind, action.TargetSeats.Count);
        if (option?.NextActualUseAdjusted == true)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        else if (adjustedBasic)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = action.TargetSeats;
        var id = _coreNativeCardUseOperations.BeginCardUse(physical, actor.Seat, action.TargetSeats ?? [], kind, physicalCardIds: [physical.Id], conversionSource: r.Source, designatedTargetSeats: kind == CardKind.BorrowedSword ? action.TargetSeats.Where((_, targetIndex) => targetIndex % 2 == 0).ToArray() : null);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        var ret = new EndingHistoricalUseReturn(f.Id, f.InstructionIndex, r.Source, f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, r.EndingFrameId, r.SlotIndex, r.Slots[r.SlotIndex].CardActionId, id, use.Action!.ActionId, physical.Id, kind);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { EndingHistoricalUses = r with { UseReturn = ret } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { EndingHistoricalUseReturn = ret, EndingHistoricalCostBefore = _coreUniqueHpPeerProgramsOperations.Value.RecipientContestSequence, EndingHistoricalActionKind = action.Kind, EndingHistoricalTargetCardId = action.TargetCardId, EndingHistoricalRequiredCardKind = option?.RequiredCardKind, EndingHistoricalUsesFangtian = fangtian, EndingHistoricalUsesProgramTargetCount = programTargetCount });
        _corePhysicalMovementFacadesOperations.MoveCard(physical, CardLocation.Hand(actor.Seat), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { EndingHistoricalCostAfter = _coreUniqueHpPeerProgramsOperations.Value.RecipientContestSequence });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndingHistoricalUseIssuedEvent(ret));
        ContinueHistoricalEndingPayment(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 420)]
    internal bool ResumeEndingHistoricalUses(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.EndingHistoricalUses is not { } r)
            return false;
        if (r.Complete)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return true;
        }

        if (r.UseReturn is not null)
            throw new InvalidOperationException("Historical Use must return once after its complete owning Use.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishHistoricalEndingChoice(f);
        return true;
    }

    [NativeFrameRoute(typeof(CardUseFrame), 160, typeof(NativeFrameRouteConditions), "HistoricalEndingPayment")]
    internal void ContinueHistoricalEndingPayment(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        _coreDyingSuitsAndHistoricalUseObserversOperations.AssertHistoricalEndingUse(use);
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.HistoricalEndingCardUse) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.HistoricalEndingCardUse) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(id, u => u with { EndingHistoricalCostDrained = true });
        var actor = _coreMatchParticipantsPlayersAccess._players[use.SourceSeat];
        var card = _coreCardAttackStateOperations.GetAttackCard(use.CardId);
        var kind = use.CardKind;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !actor.IsAlive)
        {
            _coreNativeCardUseCompletionOperations.FinishCardUse(id, card, kind);
            return;
        }

        if (kind == CardKind.Peach)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Recovery));
            return;
        }

        if (kind == CardKind.Alcohol)
        {
            _coreCardUseLifecycleOperations.BeginSimpleCardUse(id, new(card.Id, SimpleCardUseEffect.Alcohol));
            return;
        }

        if (CoreNativeCardClassificationsRuntime.IsSlashCard(kind))
        {
            var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
            var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(id, actor);
            var damage = (1 + actor.AlcoholDamageBonus) + nuzhan.DamageBonus;
            _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(id, actor);
            actor.HasAlcoholEffect = false;
            // Ending is outside Play: issue a genuine Use without consuming a
            // finite Play quota. The mature ordered owner drains every target.
            if (targets.Count > 1)
            {
                var pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id, actor.Seat, card, kind, false, damage, targets.ToArray(), use.EndingHistoricalUsesFangtian, false, use.EndingHistoricalUseReturn!.Source);
                _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
                if (use.EndingHistoricalUsesFangtian)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FangtianHalberdUsedEvent(id, actor.Seat, card.Id, kind, targets));
                if (use.EndingHistoricalUsesProgramTargetCount)
                {
                    var targetCountRule = _coreRuleQueriesOperations.EvaluateCardTargetCount(actor, kind, id);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardTargetCountAppliedEvent(id, actor.Seat, kind, targets, Array.AsReadOnly(targetCountRule.Value.Contributions.Select(c => c.SourceId).ToArray())));
                }

                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, kind, actor.Seat, targets[0]));
                foreach (var target in targets)
                    _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[target]);
                _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(pending);
                return;
            }

            var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actor.Seat, targets[0], card, damageAmount: damage, playedCardKind: kind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targets[0]], kind), programSkillCardUseFrameId: use.EndingHistoricalUseReturn!.ProgramFrameId);
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, kind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, kind, actor.Seat, targets[0]));
            _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id);
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
                _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
            return;
        }

        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, actor.Seat, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats, use.EndingHistoricalActionKind!.Value, use.EndingHistoricalTargetCardId, use.EndingHistoricalRequiredCardKind, kind);
    }

    [NativeUseReturn(290)]
    internal void ReturnHistoricalEndingUse(CardUseFrame use)
    {
        // This completed frame has already been popped. Its frozen Active bit
        // distinguishes a real attack completion from a later skipped trick tail.
        if (use.EndingHistoricalUseReturn is not { } ret || use.CardAttack is { Active: true })
            return;
        FinishHistoricalEndingReturn(ret);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 20)]
    internal bool ReturnRegisteredCompleteHistoricalEndingAttack(GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.EndingHistoricalUseReturn is null)
            return false;
        CompleteHistoricalEndingAttack(completion);
        return true;
    }

    internal void CompleteHistoricalEndingAttack(GameEngine.AttackCompletionReceipt completion) => FinishHistoricalEndingReturn(completion.EndingHistoricalUseReturn!);
    internal void FinishHistoricalEndingReturn(EndingHistoricalUseReturn ret)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(ret.ProgramFrameId);
        var r = f.EndingHistoricalUses!;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || r.UseReturn != ret || _coreCardUseLifecycleOperations.LifecycleCardUse(ret.CardUseFrameId)is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(e => e.ResolutionId == ret.CardUseFrameId) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<EndingHistoricalUseReturnedEvent>().Any(e => e.Return == ret))
            throw new InvalidOperationException("Historical Ending returned before its entire issued card use finished.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EndingHistoricalUseReturnedEvent(ret));
        // Let the mature attack/group/simple producer finish its own tail
        // before the next runtime step can publish or pay the next slot.
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { EndingHistoricalUses = r with { UseReturn = null, SlotIndex = r.SlotIndex + 1, Complete = r.SlotIndex + 1 >= r.Slots.Count } });
    }
}
