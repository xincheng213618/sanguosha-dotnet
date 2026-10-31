#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuBaOlRuntime
{
    internal const string TongduGiftReason = "skill-program.liu-ba.tongdu.gift", TongduReturnReason = "skill-program.liu-ba.tongdu.return", LiuBaExchangeReason = "skill-program.liu-ba.zhubi.exchange";
    internal bool HasLiuBaCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.RecastZhubiAndMarkActualDraw) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.IssueTongduHandGift);

    internal static CardConversionSource LiuBaSource(ProgramSkillFrame f) => global::CardGame.Core.RuntimeSkills.RuntimeRules_liu_ba_ol_LiuBa.LiuBaSource(f);
    SkillProgramStepOutcome global::CardGame.Core.ILiuBaHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginLiuBa(f, op);
    int global::CardGame.Core.ILiuBaSelectionQuery.MaximumHp(int ownerSeat) => _coreMatchParticipantsPlayersAccess._players[ownerSeat].MaxHp;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27390)]
    internal bool CanOfferLiuBa(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context)
    {
        if (t.Effects is not [{ } e] || !LiuBaComposition.Handles(e.Op))
            return true;
        return e.Op switch
        {
            SkillProgramEffectOp.IssueTongduHandGift => context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && c.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0),
            SkillProgramEffectOp.ReturnTongduOriginalCardAtPlayEnd => IsIssuedTongduCandidate(c, context),
            SkillProgramEffectOp.ExchangeZhubiCoinsAtEnding => IsIssuedLiuBaCoinEndingCandidate(c, context),
            _ => false
        };
    }

    internal LiuBaEntity[] LiuBaOwnedEntities(int seat) => new[]
    {
        CardLocation.Hand(seat),
        CardLocation.Equipment(seat),
        CardLocation.Judgment(seat),
        CardLocation.WoodenOxGrain(seat)
    }.SelectMany(l => _cardZonesOperations._cardZones.CardsAt(l).Where(c => !(l.Zone == CardZoneKind.Equipment && c.IsGeneralWeapon) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, c, l, OwnedCardMoveIntent.Transfer)).Select(c => new LiuBaEntity(c.Id, c.Kind, l))).OrderBy(c => c.CardId).ToArray();
    internal SkillProgramStepOutcome BeginLiuBa(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = f.WindowContext;
        if (f.LiuBa is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            throw new InvalidOperationException("Liu Ba requires the original living native binding.");
        var issued = op is SkillProgramEffectOp.ReturnTongduOriginalCardAtPlayEnd or SkillProgramEffectOp.ExchangeZhubiCoinsAtEnding;
        if (op == SkillProgramEffectOp.RecastZhubiAndMarkActualDraw)
        {
            if (f.TriggerId is not null || f.ActivationId != "recast-coin" || context is not null || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((f.OwnerSeat, f.SkillId, ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation!.UsageGroup)) > _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp)
                throw new InvalidOperationException("Zhubi lost its current maximum-HP quota and real Play.");
        }
        else if (context is null || !CanOfferLiuBa(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), _coreProgramLifecycleOperations.GetProgramTrigger(f), context))
            throw new InvalidOperationException("Liu Ba lost the exact native preparation, PlayEnding debt or holder Ending entitlement.");
        if (!issued && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Liu Ba cannot start an unpaid operation after source loss.");
        var stage = op switch
        {
            SkillProgramEffectOp.IssueTongduHandGift => LiuBaStage.GiftTarget,
            SkillProgramEffectOp.RecastZhubiAndMarkActualDraw => LiuBaStage.RecastTarget,
            SkillProgramEffectOp.ReturnTongduOriginalCardAtPlayEnd => LiuBaStage.ReturnChildren,
            _ => LiuBaStage.Coins
        };
        var r = new LiuBaReceipt(LiuBaSource(f), f.GameplayHash, op, context?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, stage)
        {
            Recipient = f.OwnerSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.ParentId, r.ActualTurn, r.TurnOwner, r.PhaseInstance));
        if (stage == LiuBaStage.ReturnChildren)
        {
            PayTongduReturn(f);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (stage == LiuBaStage.Coins)
        {
            var viewed = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(5).Select(c => c.Id).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { ViewedBottom = viewed } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaEndingViewedEvent(f.Id, r.ParentId, f.OwnerSeat, r.ActualTurn, viewed.Length));
        }

        PublishLiuBa(f);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal int LiuBaChooser(ProgramSkillFrame f) => f.LiuBa!.Stage is LiuBaStage.GiftCard or LiuBaStage.RecastCard ? f.LiuBa.Recipient : f.OwnerSeat;
    internal PromptChoice LiuBaChoice(ProgramSkillFrame f, string branch, string label, int? card = null, int? seat = null) => new(new($"liu-ba.{(f.Id)}.{(branch)}.{(card ?? seat ?? -1)}"), label, card is { } id ? [id] : [], seat is { } s ? [s] : [], new Dictionary<string, string> { { "program-action", "liu-ba" }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> LiuBaChoices(ProgramSkillFrame f)
    {
        var r = f.LiuBa!;
        var list = new List<PromptChoice>();
        if (r.Stage is LiuBaStage.GiftTarget or LiuBaStage.RecastTarget)
        {
            foreach (var p in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (r.Stage == LiuBaStage.GiftTarget ? _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0 : LiuBaRecastEntities(p.Seat).Length > 0)))
                list.Add(LiuBaChoice(f, "target", p.Name, seat: p.Seat));
        }
        else if (r.Stage == LiuBaStage.GiftCard)
        {
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Recipient]).OrderBy(c => c.Id))
                list.Add(LiuBaChoice(f, "card", card.DisplayName, card.Id));
        }
        else if (r.Stage == LiuBaStage.RecastCard)
        {
            foreach (var card in LiuBaRecastEntities(r.Recipient))
                list.Add(LiuBaChoice(f, "card", _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId).DisplayName, card.CardId));
        }
        else if (r.Stage == LiuBaStage.Coins)
        {
            if (r.Coins.Count < r.ViewedBottom.Count)
                foreach (var coin in LiuBaCoins(f.OwnerSeat).Where(c => !r.Coins.Contains(c.CardId)))
                    list.Add(LiuBaChoice(f, "coin", $"选择币【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(coin.CardId).DisplayName)}】", coin.CardId));
            list.Add(LiuBaChoice(f, "done", r.Coins.Count == 0 ? "不交换" : "确认这些币"));
        }
        else if (r.Stage == LiuBaStage.Bottom)
            foreach (var id in r.ViewedBottom.Where(id => !r.SelectedBottom.Contains(id)))
                list.Add(LiuBaChoice(f, "bottom", $"换取底牌【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】", id));
        return Array.AsReadOnly(list.ToArray());
    }

    internal void PublishLiuBa(ProgramSkillFrame f)
    {
        var choices = LiuBaChoices(f);
        if (choices.Count == 0)
        {
            FinishLiuBa(f);
            return;
        }

        var who = LiuBaChooser(f);
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, who, s.Name + "：选择本次原实体。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = who,
            Choices = choices,
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[who].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("liu-ba")]
    internal void AnswerLiuBa(PromptChoice supplied)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuBa: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != LiuBaChooser(f) || !LiuBaChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [supplied])))
            throw new InvalidOperationException("Liu Ba requires the exact current participant's private answer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || (r.Op is SkillProgramEffectOp.IssueTongduHandGift or SkillProgramEffectOp.RecastZhubiAndMarkActualDraw) && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            FinishLiuBa(f);
            return;
        }

        var branch = supplied.Parameters["branch"];
        if (branch == "target")
        {
            var seat = supplied.Targets.Single();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { Recipient = seat, Stage = r.Stage == LiuBaStage.GiftTarget ? LiuBaStage.GiftCard : LiuBaStage.RecastCard } });
            PublishLiuBa(f);
            return;
        }

        if (branch == "card")
        {
            var id = supplied.Cards.Single();
            var from = _cardZonesOperations._cardZones.GetLocation(id);
            var entity = new LiuBaEntity(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, from);
            if (r.Stage == LiuBaStage.GiftCard)
                PayTongduGift(f, entity);
            else
                PayLiuBaRecast(f, entity);
            return;
        }

        if (!r.ViewedBottom.SequenceEqual(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Take(r.ViewedBottom.Count).Select(c => c.Id)))
            throw new InvalidOperationException("The frozen Liu Ba bottom view is no longer current.");
        if (branch == "coin")
        {
            var id = supplied.Cards.Single();
            var material = LiuBaCoins(f.OwnerSeat).Single(c => c.CardId == id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { Coins = r.Coins.Append(id).ToArray(), Entities = r.Entities.Append(material).ToArray() } });
            PublishLiuBa(f);
            return;
        }

        if (branch == "done")
        {
            if (r.Coins.Count == 0)
            {
                FinishLiuBa(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { Stage = LiuBaStage.Bottom } });
            PublishLiuBa(f);
            return;
        }

        if (branch == "bottom")
        {
            var selected = r.SelectedBottom.Append(supplied.Cards.Single()).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { SelectedBottom = selected } });
            if (selected.Length == r.Coins.Count)
                PayLiuBaExchange(f);
            else
                PublishLiuBa(f);
            return;
        }

        throw new InvalidOperationException("Unsupported Liu Ba private selection.");
    }

    internal void PrepareLiuBaMovement(ref ProgramSkillFrame f, LiuBaStage stage, IReadOnlyList<LiuBaEntity> entities)
    {
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        var r = f.LiuBa!;
        var grain = entities.Where(e => e.Kind == CardKind.WoodenOx && e.From.Zone == CardZoneKind.Equipment).SelectMany(e => _cardZonesOperations._cardZones.CardsAt(CardLocation.WoodenOxGrain(e.From.OwnerSeat!.Value))).Select(c => c.Id).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = r with { Paid = true, Stage = stage, Entities = entities, Grain = grain, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Step = r.Step + 1, BatchId = null, ActualCount = 0 }, PendingMovementContinuation = new(r.Recipient, 0, null) });
    }

    internal void RecordLiuBaMovement(ProgramSkillFrame f, long batch, IReadOnlyList<CardMovementRecord> records)
    {
        var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = current.LiuBa!;
        _coreRuntimeOperations.ReplaceRuntimeTop(current with { LiuBa = r with { BatchId = batch } });
        foreach (var m in records)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaEntityMovedEvent(f.Id, r.Step, batch, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
    }

    internal void CompleteLiuBaMovement(ProgramSkillFrame f, int count)
    {
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var r = f.LiuBa!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { LiuBa = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, ActualCount = count } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaMovementIssuedEvent(f.Id, r.Step, r.Stage, r.Before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, count));
        ContinueLiuBa(f.Id);
    }

    internal bool DrainLiuBa(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueLiuBa(long id)
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { LiuBa: { } r } f && f.Id == id)
        {
            if (!r.Paid)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishLiuBa(f);
                return;
            }

            if (DrainLiuBa(f))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.LiuBa!;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (r.Stage == LiuBaStage.RecastChildren)
            {
                PrepareLiuBaMovement(ref f, LiuBaStage.DrawChildren, []);
                var drawn = _coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.Recipient], 1, true, CardMoveReasons.RecastDraw) : [];
                f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var draw = f.LiuBa!;
                foreach (var card in drawn)
                {
                    var m = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > draw.Before && m.CardId == card && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Recipient) && m.Reason == CardMoveReasons.RecastDraw);
                    if (!HasLiuBaCoinEntity(card))
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaCoinMintedEvent(f.Id, draw.Source, draw.GameplayHash, r.Recipient, card, m.Sequence));
                }

                var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<LiuBaEntityMovedEvent>().Single(e => e.FrameId == id && e.Step == 1);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(r.Recipient, paid.CardId, paid.Kind, drawn.Count));
                f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), drawn.Count);
                CompleteLiuBaMovement(f, drawn.Count);
                return;
            }

            FinishLiuBa(f);
            return;
        }
    }

    internal void FinishLiuBa(ProgramSkillFrame f)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new LiuBaCompletedEvent(f.Id, f.LiuBa!.Op, f.LiuBa.Step));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { LiuBa = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27390)]
    internal bool ResumeLiuBa(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { LiuBa: not null } f || f.Id != id)
            return false;
        ContinueLiuBa(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 27390)]
    internal bool ReturnLiuBaMovement(ProgramSkillFrame f)
    {
        if (f.LiuBa is null)
            return false;
        ContinueLiuBa(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27390)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27390)]
    internal bool ContinuePaidLiuBa(ProgramSkillFrame f)
    {
        if (f.LiuBa is { Paid: true })
        {
            return true;
        }

        if (f.TriggerId is null || f.WindowContext is not { } c || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Any(e => e.FrameId == f.Id && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId))
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        return IsIssuedTongduCandidate(candidate, c) || IsIssuedLiuBaCoinEndingCandidate(candidate, c);
    }

    [ProgramAiChoice(SkillProgramEffectOp.IssueTongduHandGift, SkillProgramEffectOp.RecastZhubiAndMarkActualDraw, SkillProgramEffectOp.ExchangeZhubiCoinsAtEnding)]
    internal PromptChoice ChooseLiuBa(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.LiuBa!;
        if (r.Stage == LiuBaStage.Coins)
        {
            var coin = p.Choices.Where(c => c.Parameters["branch"] == "coin").OrderBy(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue).FirstOrDefault();
            var best = r.ViewedBottom.Select(id => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind).HandKeepValue).OrderDescending().Skip(r.Coins.Count).FirstOrDefault();
            return coin is not null && best > CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(coin.Cards.Single()).Kind).HandKeepValue ? coin : p.Choices.First(c => c.Parameters["branch"] == "done");
        }

        if (r.Stage == LiuBaStage.Bottom)
            return p.Choices.OrderByDescending(c => CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards.Single()).Kind).HandKeepValue).ThenBy(c => c.Cards.Single()).First();
        return p.Choices.OrderByDescending(c => c.Targets is [var seat] ? _coreAiBrainsAiBrainsAccess._aiBrains[f.OwnerSeat].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(f.OwnerSeat), seat, new SkillProgramAiHint(1, 0, 0, 0, 0, 0, false, false)) : c.Cards is [var id] ? -CardCatalog.Get(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind).HandKeepValue : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).First();
    }
}
