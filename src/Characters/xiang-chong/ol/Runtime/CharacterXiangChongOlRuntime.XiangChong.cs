#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiangChongOlRuntime
{
    internal const string XiangGift = "skill-program.xiang-chong.guying.gift", XiangDebt = "skill-program.xiang-chong.guying.debt", XiangReturn = "skill-program.xiang-chong.guying.return", MuzhenGift = "skill-program.xiang-chong.muzhen.gift", MuzhenClaim = "skill-program.xiang-chong.muzhen.claim";
    internal static CardConversionSource XiangSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_xiang_chong_ol_XiangChong.XiangSource(f);
    internal int XiangDebtCount(int owner)
    {
        var through = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiangChongDebtConsumedEvent>().Where(e => e.OwnerSeat == owner).Select(e => e.ThroughFrameId).DefaultIfEmpty(0).Max();
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiangChongGuyingInvokedEvent>().Count(e => e.OwnerSeat == owner && e.FrameId > through);
    }

    internal CardMovementRecord? XiangLoss(int owner, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardsMoved || context.MovementBatch is not { } b || b.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || b.MovementTiming is not { } t || t.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || t.ActualTurnOwnerSeat == owner || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiangChongGuyingInvokedEvent>().Any(e => e.OwnerSeat == owner && e.ActualTurn == b.TurnNumber))
            return null;
        var lost = b.Movements.Where(m => m.From.OwnerSeat == owner && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.From != m.To && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)).ToArray();
        return lost is [var single] ? single : null;
    }

    internal IReadOnlyList<XiangChongCard> XiangOwned(int owner, ProgramSkillFrame? f = null, bool discard = false) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner)).Where(c => !c.IsGeneralWeapon && (f is null || !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner, f.SkillId, f.SkillInstanceId, c)) && (!discard || !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard))).Select(c => new XiangChongCard(c.Id, c.Kind, _cardZonesOperations._cardZones.GetLocation(c.Id))).ToArray());
    internal long XiangPlayToken(int owner) => _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is { Kind: ActualDiscardRecoveryPhaseKind.Play, ActorSeat: var actor } p && actor == owner ? p.Token : 0;
    SkillProgramStepOutcome IXiangChongHost.Resolve(ProgramSkillFrame f, SkillProgramEffect effect) => BeginXiangChong(f, effect);
    internal SkillProgramStepOutcome BeginXiangChong(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.XiangChong is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0)
            throw new InvalidOperationException("XiangChong requires its unchanged zero-input native instruction.");
        var branch = e.Op == SkillProgramEffectOp.ResolveGuyingAtomicLossChoice ? "loss" : e.Op == SkillProgramEffectOp.SettleGuyingPreparationDebt ? "debt" : e.StateId!;
        var r = new XiangChongReceipt(XiangSource(f), f.GameplayHash, f.InstructionIndex, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, branch, branch == "loss" ? XiangChongStage.Choice : branch == "debt" ? XiangChongStage.Cards : XiangChongStage.Targets);
        if (branch == "loss")
        {
            var m = f.WindowContext is { } c ? XiangLoss(f.OwnerSeat, c) : null;
            r = r with
            {
                LossBatchId = f.WindowContext!.MovementBatch!.Id,
                LossSequence = m.Sequence,
                Lost = new(m.CardId, m.CardKind, m.From),
                LossDestination = m.To,
                Paid = true
            };
        }
        else if (branch == "debt")
        {
            if (f.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || f.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
                throw new InvalidOperationException("Guying debt belongs to its own real preparation.");
            var count = XiangDebtCount(f.OwnerSeat);
            var cards = XiangOwned(f.OwnerSeat, f, true);
            r = r with
            {
                Required = Math.Min(count, cards.Count),
                Cards = cards,
                Paid = true
            };
        }
        else
        {
            var token = XiangPlayToken(f.OwnerSeat);
            if (f.TriggerId is not null || token <= 0 || MuzhenUsed(f.OwnerSeat, branch, token) || !_coreMatchParticipantsPlayersAccess._players.Any(t => MuzhenTarget(f, f.OwnerSeat, t.Seat, branch)))
                throw new InvalidOperationException("Muzhen requires its unused own actual Play branch and legal true payment.");
            r = r with
            {
                PhaseToken = token,
                Required = branch == "gift" ? 2 : 1
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongStartedEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, branch, r.PhaseToken, r.Required, r.LossBatchId, r.LossSequence));
        if (branch == "loss")
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongGuyingInvokedEvent(f.Id, f.OwnerSeat, r.ActualTurn, r.LossBatchId, r.LossSequence, r.Lost!.CardId, r.Source));
        if (branch == "debt")
        {
            var count = XiangDebtCount(f.OwnerSeat);
            var through = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<XiangChongGuyingInvokedEvent>().Where(x => x.OwnerSeat == f.OwnerSeat).Select(x => x.FrameId).DefaultIfEmpty(0).Max();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongDebtConsumedEvent(f.Id, f.OwnerSeat, count, through));
            if (r.Required == 0)
            {
                FinishXiang(f, "no-discardable-cards");
                return SkillProgramStepOutcome.AwaitChild;
            }
        }

        if (branch == "loss" && XiangChoices(f).Count == 0)
        {
            FinishXiang(f, "neither-compensation-executable");
            return SkillProgramStepOutcome.AwaitChild;
        }

        PublishXiang(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool XiangLostStillAvailable(ProgramSkillFrame f) => f.XiangChong is { Lost: { } lost, LossDestination: { } to } r && _cardZonesOperations._cardZones.GetLocation(lost.CardId) == to && to != CardLocation.OutsideGame && to != CardLocation.Hand(f.OwnerSeat) && to != CardLocation.Equipment(f.OwnerSeat) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == lost.CardId).Sequence == r.LossSequence;
    internal static Dictionary<string, string> XiangArgs(string branch) => RuntimeSkills.RuntimeRules_xiang_chong_ol_XiangChong.XiangArgs(branch);
    internal IReadOnlyList<PromptChoice> XiangChoices(ProgramSkillFrame f)
    {
        var r = f.XiangChong!;
        var choices = new List<PromptChoice>();
        void Add(string branch, string label, int[]? cards = null, int[]? targets = null, Dictionary<string, string>? extra = null)
        {
            var args = XiangArgs(branch);
            if (extra is not null)
                foreach (var(k, v)in extra)
                    args[k] = v;
            choices.Add(new(new($"xiang.{(f.Id)}.{(branch)}.{(choices.Count)}"), label, cards ?? [], targets ?? [], args));
        }

        if (r.Stage == XiangChongStage.Choice)
        {
            if (_coreMatchParticipantsPlayersAccess._players[r.TurnOwner].IsAlive && XiangOwned(r.TurnOwner).Count > 0)
                Add("give", "随机交给向宠一张牌");
            if (XiangLostStillAvailable(f) && (!_coreEquipmentContentOperations.EquipmentRules.IsEquipment(r.Lost!.Kind) || CanIssueXiangChongGuyingEquipment(f, r.Lost.CardId, r.LossDestination!.Value)))
                Add("return", _coreEquipmentContentOperations.EquipmentRules.IsEquipment(r.Lost!.Kind) ? "令向宠使用原装备牌" : "令向宠获得原失去的牌");
        }
        else if (r.Stage == XiangChongStage.Targets)
        {
            foreach (var t in _coreMatchParticipantsPlayersAccess._players.Where(t => MuzhenTarget(f, f.OwnerSeat, t.Seat, r.Branch)))
                Add("target", "选择 " + t.Name, targets: [t.Seat]);
        }
        else if (r.Stage == XiangChongStage.Cards)
        {
            if (r.Selected.Count < r.Required)
                foreach (var c in r.Cards.Where(c => !r.Selected.Any(x => x.CardId == c.CardId)))
                    Add("card", "选择【" + CardCatalog.Get(c.Kind).DisplayName + "】", [c.CardId]);
            if (r.Selected.Count == r.Required)
                Add("pay", r.Branch == "debt" ? "弃置所选牌" : "支付睦阵原始牌");
        }
        else if (r.Stage == XiangChongStage.ClaimChoice)
        {
            var zone = r.Branch == "gift" ? CardZoneKind.Equipment : CardZoneKind.Hand;
            foreach (var raw in _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.Recipient, [zone], OwnedCardMoveIntent.Obtain))
            {
                var a = raw.Parameters.ToDictionary(x => x.Key, x => x.Value);
                a["program-action"] = "xiang-chong";
                a["branch"] = "claim";
                choices.Add(new(new($"xiang.{(f.Id)}.claim.{(choices.Count)}"), raw.Label, raw.Cards, raw.Targets, a));
            }
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishXiang(ProgramSkillFrame f)
    {
        var r = f.XiangChong!;
        var choices = XiangChoices(f);
        if (choices.Count == 0)
        {
            FinishXiang(f, "no-current-legal-choice");
            return;
        }

        var actor = r.Stage == XiangChongStage.Choice ? r.TurnOwner : f.OwnerSeat;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, actor, "向宠：选择当前实际结算项。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = actor,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[actor].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("xiang-chong")]
    internal void AnswerXiang(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiangChong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == XiangChongStage.Choice ? r.TurnOwner : f.OwnerSeat) || !XiangChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("XiangChong requires its exact current chooser and frozen private card frontier.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (branch == "target")
        {
            var target = choice.Targets.Single();
            var cards = XiangOwned(f.OwnerSeat, f).Where(c => r.Branch == "gift" || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && _coreEquipmentPlacementAndHpPairOperations.CanPlaceOwnedEquipment(f.OwnerSeat, target, _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId), c.From)).ToArray();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Recipient = target, Cards = cards, Stage = XiangChongStage.Cards } });
            PublishXiang(f);
            return;
        }

        if (branch == "card")
        {
            var c = r.Cards.Single(c => c.CardId == choice.Cards.Single());
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Selected = r.Selected.Append(c).ToArray() } });
            PublishXiang(f);
            return;
        }

        if (branch == "give")
        {
            var cards = XiangOwned(r.TurnOwner);
            var c = cards[_randomOperations._random.Next(cards.Count)];
            PayXiang(f, [c], CardLocation.Hand(f.OwnerSeat), XiangGift, "compensation");
            ContinueXiang(f.Id);
            return;
        }

        if (branch == "return")
        {
            if (_coreEquipmentContentOperations.EquipmentRules.IsEquipment(r.Lost!.Kind))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Stage = XiangChongStage.EquipmentReady } });
                ContinueXiang(f.Id);
                return;
            }

            PayXiang(f, [r.Lost with { From = r.LossDestination!.Value }], CardLocation.Hand(f.OwnerSeat), XiangReturn, "return");
            ContinueXiang(f.Id);
            return;
        }

        if (branch == "claim")
        {
            var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
            var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
            var from = new CardLocation(zone, r.Recipient);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
            PayXiang(f, [new(card.Id, card.Kind, from)], CardLocation.Hand(f.OwnerSeat), MuzhenClaim, "claim");
            ContinueXiang(f.Id);
            return;
        }

        if (branch != "pay" || r.Selected.Count != r.Required || r.Selected.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != c.From))
            throw new InvalidOperationException("XiangChong cannot pay another count or substituted original material.");
        if (r.Branch == "place")
            PlaceMuzhen(f);
        else
            PayXiang(f, r.Selected, r.Branch == "debt" ? CardLocation.DiscardPile : CardLocation.Hand(r.Recipient), r.Branch == "debt" ? XiangDebt : MuzhenGift, "payment");
        ContinueXiang(f.Id);
    }

    internal void PayXiang(ProgramSkillFrame f, IReadOnlyList<XiangChongCard> cards, CardLocation to, string reason, string segment)
    {
        var r = f.XiangChong!;
        var first = (r.Branch is "gift" or "place") && segment == "payment";
        var grain = cards.Where(c => c.Kind == CardKind.WoodenOx && c.From is { Zone: CardZoneKind.Equipment, OwnerSeat: not null }).SelectMany(c => _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[c.From.OwnerSeat!.Value]).Select(x => new XiangChongCard(x.Id, x.Kind, CardLocation.WoodenOxGrain(c.From.OwnerSeat.Value)))).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Paid = true, Stage = segment == "claim" ? XiangChongStage.ClaimChildren : XiangChongStage.PaymentChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Payments = cards, Auxiliaries = grain, Replaced = null } });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(c => c.CardId).ToArray(), to, new(reason), (_, movements) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { XiangChong = current.XiangChong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, BatchId = _activeCardMovementBatchIdsOperations._activeCardMovementBatchIds.Peek() } });
            foreach (var m in movements)
            {
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEntityMovedEvent(f.Id, segment, m.CardId, m.CardKind, m.From, m.To, m.Sequence));
                if (segment == "return" && m.From == CardLocation.Processing && f.WindowContext?.MovementBatch is { ParentFrameId: { } useId } lossBatch && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { Type: CardActionType.Use } action } use && action.PhysicalCards.Any(c => c.CardId == m.CardId && c.From == r.Lost!.From && c.CardKind == m.CardKind) && lossBatch.Movements.Any(x => x.CardId == m.CardId && x.Sequence == r.LossSequence && x.To == CardLocation.Processing))
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongUseMaterialClaimedEvent(use.Id, action.ActionId, f.Id, r.Source, r.GameplayHash, m.CardId, m.CardKind, f.OwnerSeat, r.LossSequence, m.Sequence));
            }
        });
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = f.XiangChong!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        if (first)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongBranchPaidEvent(f.Id, f.OwnerSeat, r.Recipient, r.Branch, r.PhaseToken, f.XiangChong!.Before, f.XiangChong.After));
    }

    internal bool DrainXiang(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    internal void ContinueXiang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiangChong: { } r } f || f.Id != id)
            return;
        if (r.Stage is XiangChongStage.PaymentChildren or XiangChongStage.ClaimChildren or XiangChongStage.NativeChildren)
        {
            if (DrainXiang(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.XiangChong!;
            if (r.Stage == XiangChongStage.PaymentChildren && (r.Branch is "gift" or "place") && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Recipient].IsAlive)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Stage = XiangChongStage.ClaimChoice } });
                PublishXiang(f);
                return;
            }

            FinishXiang(f, "paid-children-finished");
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
        {
            FinishXiang(f, "owner-dead-or-game-ended");
            return;
        }

        if (r.Stage == XiangChongStage.EquipmentReady)
        {
            if (!XiangLostStillAvailable(f) || !CanIssueXiangChongGuyingEquipment(f, r.Lost!.CardId, r.LossDestination!.Value))
            {
                FinishXiang(f, "original-equipment-no-longer-executable");
                return;
            }

            var origin = new XiangChongEquipmentOrigin(f.Id, _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1, r.Lost!.CardId, r.Lost.Kind, f.OwnerSeat, r.LossDestination!.Value);
            var slot = _coreEquipmentContentOperations.EquipmentRules.Get(origin.CardKind).Slot;
            var eq = _corePhysicalOwnedCardQueriesOperations.GetEquipment2(f.OwnerSeat).Where(c => _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot).ToArray();
            var old = eq.Length >= _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].EquipmentSlotCapacity(slot) ? eq[0] : null;
            var grain = old?.Kind == CardKind.WoodenOx ? _corePhysicalOwnedCardQueriesOperations.GetWoodenOxGrain(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(c => new XiangChongCard(c.Id, c.Kind, CardLocation.WoodenOxGrain(f.OwnerSeat))).ToArray() : Array.Empty<XiangChongCard>();
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = r with { Stage = XiangChongStage.EquipmentUse, Equipment = origin, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, Replaced = old is null ? null : new(old.Id, old.Kind, CardLocation.Equipment(f.OwnerSeat)), Auxiliaries = grain } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEquipmentIssuedEvent(origin));
            BeginXiangChongGuyingEquipment(f, origin);
            return;
        }

        if (r.Stage != XiangChongStage.EquipmentUse && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
            PublishXiang(f);
    }

    internal bool MatchesXiangChongEquipment(ProgramSkillFrame f, XiangChongEquipmentOrigin origin) => f.XiangChong is { Stage: XiangChongStage.EquipmentUse, Equipment: { } stored } r && stored == origin && origin.ProgramFrameId == f.Id && origin.ActorSeat == f.OwnerSeat && r.Lost?.CardId == origin.CardId && r.Lost.Kind == origin.CardKind && r.LossDestination == origin.From && ValidXiang(f);
    internal void FinishXiangChongEquipment(ProgramSkillFrame f, bool issued)
    {
        if (f.XiangChong?.Equipment is not { } o || !MatchesXiangChongEquipment(f, o))
            throw new InvalidOperationException("Guying equipment lost its original native return.");
        if (issued)
            CaptureXiangChongEquipmentMaterialEntry(f);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEquipmentReturnedEvent(o, issued));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { XiangChong = f.XiangChong with { Stage = XiangChongStage.NativeChildren, NativeCompleted = true, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        ContinueXiang(f.Id);
    }

    [NativeUseReturn(27140)]
    internal void ReturnXiangEquipment(CardUseFrame use)
    {
        if (use.XiangChongGuyingEquipment is { } o)
        {
            CaptureXiangChongEquipmentResult(use);
            FinishXiangChongEquipment(_coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId), true);
        }
    }

    internal void FinishXiang(ProgramSkillFrame f, string reason)
    {
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongCompletedEvent(f.Id, f.XiangChong!.Branch, f.XiangChong.Paid, reason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiangChong = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27140)]
    internal bool ResumeXiang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiangChong: not null } f || f.Id != id)
            return false;
        ContinueXiang(id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27140)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27140)]
    internal bool ContinuePaidXiang(ProgramSkillFrame f) => f.XiangChong is { Paid: true } && ValidXiang(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveGuyingAtomicLossChoice, SkillProgramEffectOp.SettleGuyingPreparationDebt, SkillProgramEffectOp.ResolveMuzhenExchange)]
    internal PromptChoice AiXiang(PendingDecision p, ProgramSkillFrame f)
    {
        var r = f.XiangChong!;
        if (r.Stage == XiangChongStage.Choice)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "give") ?? p.Choices[0];
        if (r.Stage == XiangChongStage.Cards)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? p.Choices.Where(c => c.Cards.Count == 1).OrderBy(c => CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
        return p.Choices[0];
    }
}
