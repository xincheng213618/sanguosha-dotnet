#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouBaMobileShiRuntime
{
    internal const string ShiXbLibbing = ShiXiahouBaComposition.LibbingSkill, ShiXbPoxi = ShiXiahouBaComposition.PoxiSkill;
    internal static CardConversionSource ShiXbSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HasShiXbCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShiXbLibbing);

    internal (int Last, int Count1, int Count2, int Count3) ShiXbLibbingState(int ownerSeat)
    {
        var last = 0;
        var count1 = 0;
        var count2 = 0;
        var count3 = 0;
        foreach (var payload in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            switch (payload)
            {
                case ShiXbLibbingAdvancedEvent { Source.OwnerSeat: var seat } advanced when seat == ownerSeat:
                    last = advanced.Option;
                    switch (advanced.Option)
                    {
                        case 1:
                            count1++;
                            break;
                        case 2:
                            count2++;
                            break;
                        default:
                            count3++;
                            break;
                    }

                    break;
                case ShiXbLibbingResetEvent { Source.OwnerSeat: var owner }when owner == ownerSeat:
                    last = 0;
                    count1 = 0;
                    count2 = 0;
                    count3 = 0;
                    break;
            }
        }

        return (last, count1, count2, count3);
    }

    internal int ShiXbLibbingRangeBonus(int seat) => !HasShiXbCapability ? 0 : ShiXbLibbingState(seat).Count1;
    internal bool ShiXbLibbingIsDamageUse(ProgramCardUseContext use) => ShiXiahouBaComposition.IsDamageCardKind(use.EffectiveKind);
    internal bool ShiXbPoxiCannotRespond(int sourceSeat, int responderSeat, CardKind incomingKind, IReadOnlyList<int> targetSeats)
    {
        // The innermost incoming Use only: a nested unrelated Use cannot inherit the outer ban.
        if (_resolutionStackOperations._resolutionStack.LastOrDefault(x => x is CardUseFrame { ShiXiahouBaPoxi: not null })is not CardUseFrame { Action: { } action, ShiXiahouBaPoxi: { } poxi })
            return false;
        return CoreNativeCardClassificationsRuntime.IsSlashCard(incomingKind) && action.Type == CardActionType.Use && action.ActorSeat == sourceSeat && sourceSeat == poxi.Source.OwnerSeat && action.ActionId == poxi.ActionId && targetSeats.Contains(responderSeat) && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[responderSeat]).Count <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).Count;
    }

    SkillProgramStepOutcome IShiXiahouBaProgramHost.ResolveLibbing(ProgramSkillFrame frame) => BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbLibbing);
    SkillProgramStepOutcome IShiXiahouBaProgramHost.ResolvePoxi(ProgramSkillFrame frame) => BeginShiXb(frame, SkillProgramEffectOp.ResolveShiXbPoxi);
    internal SkillProgramStepOutcome BeginShiXb(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiXiahouBa is not null)
            throw new InvalidOperationException("Shi Xiahou Ba restarted a running operation.");
        var r = op == SkillProgramEffectOp.ResolveShiXbLibbing && f.SkillId == ShiXbLibbing && f.TriggerId == ShiXiahouBaComposition.LibbingTrigger ? StartShiXbLibbing(f) : op == SkillProgramEffectOp.ResolveShiXbPoxi && f.SkillId == ShiXbPoxi && f.TriggerId == ShiXiahouBaComposition.PoxiTrigger ? StartShiXbPoxi(f) : throw new InvalidOperationException("Shi Xiahou Ba instruction has an incompatible owning skill.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXiahouBa = r });
        if (r.Operation == "libbing")
        {
            ContinueShiXb(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishShiXb(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal ShiXiahouBaReceipt StartShiXbLibbing(ProgramSkillFrame f)
    {
        var use = f.WindowContext?.CardUse ?? throw new InvalidOperationException("Libbing lost its completed-use window.");
        return new("libbing", _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            Source = ShiXbSource(f),
            GameplayHash = f.GameplayHash,
            ActionId = use.CardActionId
        };
    }

    internal ShiXiahouBaReceipt StartShiXbPoxi(ProgramSkillFrame f)
    {
        var(batch, index) = f.WindowContext is { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } movementBatch, MovementIndex: { } movementIndex } && movementIndex >= 0 && movementIndex < movementBatch.Movements.Count ? (movementBatch, movementIndex) : throw new InvalidOperationException("Poxi lost its exact discarded-slash movement.");
        var move = batch.Movements[index];
        if (move.To != CardLocation.DiscardPile || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(move) || _cardZonesOperations._cardZones.GetLocation(move.CardId) != CardLocation.DiscardPile || !CoreNativeCardClassificationsRuntime.IsSlashCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(move.CardId).Kind))
            throw new InvalidOperationException("Poxi lost its discarded original Slash.");
        return new("poxi", _coreActualTurnProgressTurnNumberAccess._turnNumber)
        {
            Source = ShiXbSource(f),
            GameplayHash = f.GameplayHash,
            BatchId = batch.Id,
            CardId = move.CardId,
            DiscardMovementSequence = move.Sequence
        };
    }

    internal void ContinueShiXb(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiahouBa: { } r } f || f.Id != id)
            return;
        if (r.Stage is ShiXiahouBaComposition.PoxiStageIssuingUse or ShiXiahouBaComposition.PoxiStageNativeUse)
            return;
        switch (r.Operation, r.Stage)
        {
            case ("libbing", 0):
                ContinueShiXbLibbing(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("poxi", ShiXiahouBaComposition.PoxiStageChoice):
                PublishShiXb(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            case ("poxi", ShiXiahouBaComposition.PoxiStageReturnedUse):
                FinishShiXbOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return;
            default:
                throw new InvalidOperationException("Shi Xiahou Ba has an unknown continuation stage.");
        }
    }

    internal void ContinueShiXbLibbing(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        var use = f.WindowContext?.CardUse ?? throw new InvalidOperationException("Libbing lost its completed-use window.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishShiXbOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var(last, count1, count2, count3) = ShiXbLibbingState(f.OwnerSeat);
        if (ShiXbLibbingIsDamageUse(use))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXbLibbingResetEvent(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, use.CardActionId, use.EffectiveKind.ToString()));
            FinishShiXbOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var option = last switch
        {
            1 => 2,
            2 => 3,
            3 => 1,
            _ => 1
        };
        var(next1, next2, next3) = option switch
        {
            1 => (count1 + 1, count2, count3),
            2 => (count1, count2 + 1, count3),
            _ => (count1, count2, count3 + 1),
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXbLibbingAdvancedEvent(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, option, next1, next2, next3));
        if (option == 2)
        {
            // X counts the current execution too: the first option-2 draw is two cards.
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, next2 + 1, null, null, SkillProgramCardSetVisibility.Private, new(ShiXiahouBaComposition.LibbingDrawReason));
        }

        FinishShiXbOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal Card ShiXbPoxiCard(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        return _cardZonesOperations._cardZones.GetLocation(r.CardId) == CardLocation.DiscardPile ? _coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId) : throw new InvalidOperationException("Poxi lost its discarded original Slash.");
    }

    internal IReadOnlyList<int> ShiXbPoxiTargets(ProgramSkillFrame f)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var card = ShiXbPoxiCard(f);
        if (_coreCardUseModulesOperations.IsCardUseForbidden(owner.Seat, card.Kind, CardActionType.Use) || !_coreProgramTargetRulesOperations.TargetRules.AllowsNativeUseMaterial(new(owner.Seat, card.Kind, [card.Id])))
            return[];
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat && _coreNativeTargetLegalityOperations.CanUseSlashTarget(owner, player, card, ignoreDistance: true, physicalCardIds: [card.Id])).Select(player => player.Seat).ToArray();
    }

    internal IReadOnlyList<PromptChoice> ShiXbChoices(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        if (r.Operation != "poxi" || r.Stage != ShiXiahouBaComposition.PoxiStageChoice)
            return[];
        var card = ShiXbPoxiCard(f);
        return Array.AsReadOnly(ShiXbPoxiTargets(f).Select(seat => new PromptChoice(new($"shi-xiahou-ba.{(f.Id)}.{(r.Stage)}.use.{(seat)}"), $"使用弃置的【{(card.DisplayName)}】对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}（无距离限制）", [card.Id], [seat], new Dictionary<string, string> { ["program-action"] = ShiXiahouBaComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "use", ["target"] = seat.ToString(CultureInfo.InvariantCulture), })).ToArray());
    }

    internal void PublishShiXb(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        var choices = ShiXbChoices(f);
        if (choices.Count == 0)
        {
            FinishShiXbOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "迫袭：选择目标，改为使用弃置的【杀】。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiXiahouBaComposition.ProgramAction)]
    internal void AnswerShiXb(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiahouBa: { Operation: "poxi" } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiXbChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Xiahou Ba choice lost its current frame, chooser or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, ShiXiahouBa = r with { Stage = ShiXiahouBaComposition.PoxiStageIssuingUse, TargetSeat = int.Parse(selected.Parameters["target"], CultureInfo.InvariantCulture) } });
        IssueShiXbNativeUse(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void IssueShiXbNativeUse(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        var card = ShiXbPoxiCard(f);
        if (!ShiXbPoxiTargets(f).Contains(r.TargetSeat))
            throw new InvalidOperationException("Poxi cannot issue its use at a lost target.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[r.TargetSeat];
        _coreCardConversionsOperations.SelectUseConversion(new(LegalActionKind.Slash, card.Id, target.Seat, "使用迫袭弃置的原牌", card.Kind));
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, owner.Seat, [target.Seat], card.Kind, physicalCardIds: [card.Id]);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("Poxi lost its declared native use.");
        if (use.Action is not { Type: CardActionType.Use, PhysicalCards: [var cost], ConversionChain.Count: 0 } action || action.ActorSeat != f.OwnerSeat || cost.CardId != card.Id || cost.From != CardLocation.DiscardPile || cost.CardKind != card.Kind || use.CardKind != card.Kind || !use.TargetSeats.SequenceEqual([r.TargetSeat]) || use.PhysicalCardIds is not [var physicalId] || physicalId != card.Id)
            throw new InvalidOperationException("Poxi lost its original discarded-pile material on native declaration.");
        var origin = new ShiXiahouBaPoxiUse(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, card.Id, card.Kind, CardLocation.DiscardPile, r.TargetSeat, useId, action.ActionId);
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(p => p.Id == f.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f = parent with { ShiXiahouBa = r with { Stage = ShiXiahouBaComposition.PoxiStageNativeUse, UseId = useId, ActionId = action.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { ShiXiahouBaPoxi = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXbPoxiNativeIssuedEvent(f.Id, r.Source!, f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, useId, action.ActionId, card.Id, r.TargetSeat));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DiscardPile, CardLocation.Processing, new(ShiXiahouBaComposition.PoxiUseMoveReason));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, owner.Seat, r.TargetSeat, card, damageAmount: 1 + owner.AlcoholDamageBonus, playedCardKind: card.Kind, physicalCards: [card], ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(owner, target, card.Kind), programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(useId, owner);
        owner.HasAlcoholEffect = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, card.Kind, owner.Seat, r.TargetSeat, IgnoresArmor: attack.IgnoresArmor));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(owner, target);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(useId)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, [r.TargetSeat], ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void FinishShiXbOperation(ProgramSkillFrame f)
    {
        var r = f.ShiXiahouBa!;
        if (r.Operation == "poxi")
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiXiahouBa = r with { Stage = ShiXiahouBaComposition.PoxiStageFinished } });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    // ---- native use return ----------------------------------------------------
    [NativeUseValidation(68000)]
    internal void ValidateShiXbNativeUse(CardUseFrame use)
    {
        if (use.ShiXiahouBaPoxi is { } n && (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is null || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)?.ShiXiahouBaPoxi?.UseId != n.UseId))
            throw new InvalidOperationException("Poxi cannot retire a different or incomplete original Slash Use.");
    }

    [NativeUseReturn(68000)]
    internal bool ReturnShiXbNativeUse(CardUseFrame use)
    {
        if (use.ShiXiahouBaPoxi is not { } n)
            return false;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(n.FrameId);
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || f.ShiXiahouBa is not { UseId: var useId } || useId != use.Id || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiXbPoxiNativeReturnedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("Poxi requires one exact whole native use return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXbPoxiNativeReturnedEvent(n.FrameId, use.Id));
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { ShiXiahouBa = f.ShiXiahouBa!with { Stage = ShiXiahouBaComposition.PoxiStageReturnedUse } });
        if (use.CardAttack is not { Active: true })
            ContinueShiXb(f.Id);
        return true;
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 68000)]
    internal bool ResumeShiXbAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiahouBa: { Operation: "poxi", Stage: ShiXiahouBaComposition.PoxiStageReturnedUse } } f)
            return false;
        ContinueShiXb(f.Id);
        return true;
    }

    // ---- runtime hooks --------------------------------------------------------
    [ProgramContinuation(ProgramContinuationKind.Resume, 68000)]
    internal bool ResumeShiXb(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXiahouBa: not null } f || f.Id != id)
            return false;
        ContinueShiXb(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68000)]
    internal bool ReturnShiXbMovement(ProgramSkillFrame f) => f.ShiXiahouBa is not null && ResumeShiXb(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 68000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 68000)]
    internal bool ContinuePaidShiXb(ProgramSkillFrame f) => f.ShiXiahouBa is { Operation: "poxi", Stage: not ShiXiahouBaComposition.PoxiStageFinished };
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 68000)]
    internal int ShiXbLibbingDamageBonus(GameEngine.IDamageAttempt attack) => attack is NativeOwners.CardAttackHandle card && card.CardUserSeat is { } user && user >= 0 && attack.EffectiveCardKind is { } kind && ShiXiahouBaComposition.IsDamageCardKind(kind) ? ShiXbLibbingState(user).Count3 : 0;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 68000)]
    internal bool ShiXbNativeMaterialBatch(CardUseFrame use, CardMovementBatchContext batch) => use.ShiXiahouBaPoxi is { } n && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == n.FrameId)is { ShiXiahouBa: { UseId: var useId } } && useId == use.Id && batch.ParentFrameId == use.Id && batch.Movements.Any(m => m.CardId == n.CardId && m.From == CardLocation.DiscardPile && m.To == CardLocation.Processing && m.Reason.Value == ShiXiahouBaComposition.PoxiUseMoveReason);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 68000)]
    internal bool ShiXbExemptDebit(long useId, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { ShiXiahouBaPoxi: { } n } && n.Source.OwnerSeat == actor && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == n.FrameId && f.ShiXiahouBa is { UseId: var owned } && owned == useId);

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiXbPoxi)]
    internal PromptChoice ChooseShiXbAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        return decision.Choices.OrderBy(choice => CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(owner, _coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()])).ThenBy(choice => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()]).Count).First();
    }
}
