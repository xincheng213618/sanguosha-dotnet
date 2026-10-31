#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouFeiBoundaryMobileRuntime
{
    SkillProgramStepOutcome IBoundaryZhouFeiHost.Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginBoundaryZf(frame, operation);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 54700)]
    internal bool OfferBoundaryZf(ProgramTriggerLegalityContext gate)
    {
        if (gate.SkillId == BoundaryZfLiangyin)
        {
            if (gate.Trigger.Id != "round-unique")
                return true;
            var targets = BoundaryZfRoundTargets(gate.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber);
            return targets.Seen is not null || targets.Unseen is not null;
        }

        if (gate.SkillId != BoundaryZfKongsheng || gate.Trigger.Id == "return-held")
            return true;
        return _coreTableSeatQueriesOperations.IsValidPlayerSeat(_coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.OwnerSeat) && _coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.OwnerSeat].IsAlive && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.OwnerSeat]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.OwnerSeat]).Count > 0) && (gate.Trigger.Id == "prepare-store" || gate.Context.HpChange is { Kind: HpChangeKind.Loss or HpChangeKind.Recovery, Amount: > 0 } change && change.TargetSeat == gate.OwnerSeat);
    }

    internal SkillProgramStepOutcome BeginBoundaryZf(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var context = frame.WindowContext ?? throw new InvalidOperationException("Mobile Zhou Fei requires a native trigger.");
        if (frame.BoundaryZhouFei is not null || frame.InstructionIndex != 1)
            throw new InvalidOperationException("Zhou Fei cannot repay a started instruction.");
        var receipt = new BoundaryZhouFeiReceipt(BoundaryZfSource(frame), frame.GameplayHash, operation, context.ParentFrameId, _coreActualRoundProgressRoundNumberAccess._roundNumber, _coreActualTurnProgressCurrentSeatTurnProgressionAccess._turnProgression.OwnerSeat, -1)
        {
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        if (operation == SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || context.Window is not (SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or SkillProgramTriggerWindow.AfterHealthChanged) || context.Window == SkillProgramTriggerWindow.AfterHealthChanged && context.HpChange is not { Kind: HpChangeKind.Loss or HpChangeKind.Recovery, Amount: > 0 })
                throw new InvalidOperationException("Kongsheng stores only at preparation, actual recovery or direct HP loss.");
            receipt = receipt with
            {
                Stage = BoundaryZhouFeiStage.Store,
                Location = new(CardZoneKind.PrivatePersistentPile, receipt.Holder, $"boundary-zhou-fei:{(frame.Id)}")
            };
        }
        else if (operation == SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng)
        {
            if (frame.SkillId != BoundaryZfKongsheng || context.Window != SkillProgramTriggerWindow.TurnEnding || context.TargetSeat is not { } holder)
                throw new InvalidOperationException("Kongsheng returns at the original holder's ending.");
            receipt = receipt with
            {
                Holder = holder,
                Stage = BoundaryZhouFeiStage.UseChoice,
                Deposits = BoundaryZfLiveDeposits(holder).Where(deposit => deposit.Source.OwnerSeat == frame.OwnerSeat && deposit.Source.SkillInstanceId == frame.SkillInstanceId).Select(deposit => deposit.FrameId).ToArray()
            };
        }
        else if (context.Window == SkillProgramTriggerWindow.RoundEnding)
            receipt = receipt with
            {
                Stage = BoundaryZhouFeiStage.RoundChoice
            };
        else
        {
            if (frame.SkillId != BoundaryZfLiangyin || context.MovementBatch is not { } batch)
                throw new InvalidOperationException("Liangyin requires its actual global general-pile movement batch.");
            var group = BoundaryZfMovementGroups(batch).Single(group => group.Encoded == context.OccurrenceIndex);
            receipt = receipt with
            {
                Stage = BoundaryZhouFeiStage.Target,
                Direction = group.Direction
            };
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiStartedEvent(frame.Id, receipt.Source, receipt.Hash, operation, receipt.WindowId, receipt.Round, receipt.Holder, receipt.Direction));
        ContinueBoundaryZf(frame.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice BoundaryZfChoice(ProgramSkillFrame frame, string branch, int value, string text, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"boundary-zhou-fei.{(frame.Id)}.{(branch)}.{(value)}"), text, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = BoundaryZfAction, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value.ToString(CultureInfo.InvariantCulture) });
    internal IReadOnlyList<PromptChoice> BoundaryZfOwnedChoices(ProgramSkillFrame frame, int chooser, int owner, string branch, OwnedCardMoveIntent intent) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(frame.Id, chooser, owner, [CardZoneKind.Hand, CardZoneKind.Equipment], intent).Select(choice => choice with { Parameters = new Dictionary<string, string>(choice.Parameters) { ["program-action"] = BoundaryZfAction, ["branch"] = branch } }).ToArray();
    internal IReadOnlyList<PromptChoice> BoundaryZfChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhouFei!;
        var result = new List<PromptChoice>();
        if (receipt.Stage == BoundaryZhouFeiStage.Store)
        {
            result.AddRange(BoundaryZfOwnedChoices(frame, frame.OwnerSeat, receipt.Holder, "toggle", OwnedCardMoveIntent.Transfer).Select(choice =>
            {
                var card = BoundaryZfOwnedChoiceCard(choice, receipt.Holder);
                return choice with
                {
                    Description = (receipt.Selected.Contains(card.Id) ? "已选：" : "") + choice.Description
                };
            }));
            result.Add(BoundaryZfChoice(frame, "confirm-store", 0, $"扣置已选的{(receipt.Selected.Count)}张牌"));
            return result;
        }

        if (receipt.Stage == BoundaryZhouFeiStage.Target)
            return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && (receipt.Direction == 0 || _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(player).Count > 0)).Select(player => BoundaryZfChoice(frame, "target", player.Seat, $"令 {(player.Name)} {((receipt.Direction == 0 ? "摸" : "弃置"))}一张牌", targets: [player.Seat])).ToArray();
        if (receipt.Stage == BoundaryZhouFeiStage.Discard)
            return BoundaryZfOwnedChoices(frame, receipt.Target, receipt.Target, "discard", OwnedCardMoveIntent.Discard);
        if (receipt.Stage == BoundaryZhouFeiStage.RoundChoice)
        {
            var targets = BoundaryZfRoundTargets(frame.OwnerSeat, receipt.Round);
            if (targets.Seen is { } seen && _coreMatchParticipantsPlayersAccess._players[seen].HasHpValue)
                result.Add(BoundaryZfChoice(frame, "recover", seen, $"令唯一目标 {(_coreMatchParticipantsPlayersAccess._players[seen].Name)} 回复1点体力", targets: [seen]));
            if (targets.Unseen is { } unseen && _coreMatchParticipantsPlayersAccess._players[unseen].HasHpValue)
                result.Add(BoundaryZfChoice(frame, "lose-hp", unseen, $"令唯一未被选择的 {(_coreMatchParticipantsPlayersAccess._players[unseen].Name)} 失去1点体力", targets: [unseen]));
            return result;
        }

        if (receipt.Stage == BoundaryZhouFeiStage.UseChoice && receipt.Cursor < receipt.Deposits.Count)
        {
            var actions = BoundaryZfNativeActions(frame);
            result.AddRange(actions.Select((action, index) => BoundaryZfChoice(frame, "use", index, action.Description, [action.CardId!.Value], action.TargetSeats)));
            result.Add(BoundaryZfChoice(frame, "return", 0, "不使用，归还本次扣置的其余牌"));
            return result;
        }

        return[];
    }

    internal Card BoundaryZfOwnedChoiceCard(PromptChoice choice, int owner)
    {
        var zone = Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]);
        var slot = int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture);
        return _cardZonesOperations._cardZones.CardsAt(new(zone, owner))[slot];
    }

    internal void PublishBoundaryZf(ProgramSkillFrame frame)
    {
        var choices = BoundaryZfChoices(frame);
        if (choices.Count == 0)
        {
            FinishBoundaryZf(frame);
            return;
        }

        var receipt = frame.BoundaryZhouFei!;
        var chooser = receipt.Stage == BoundaryZhouFeiStage.Discard ? receipt.Target : frame.OwnerSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, receipt.Stage == BoundaryZhouFeiStage.Store ? "箜声：选择当前回合角色任意张手牌或装备，确认后一次扣置。" : receipt.Stage == BoundaryZhouFeiStage.UseChoice ? "箜声：可使用本次扣置的一张牌，然后原持有者获得其余。" : skill.Description, choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = receipt.Stage == BoundaryZhouFeiStage.Store ? receipt.Holder : null,
            Choices = choices,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(BoundaryZfAction)]
    internal void AnswerBoundaryZf(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhouFei: { } receipt } frame || !ValidBoundaryZf(frame) || !BoundaryZfChoices(frame).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Zhou Fei's original source, opaque slot or native card use changed.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch == "toggle")
        {
            var card = BoundaryZfOwnedChoiceCard(selected, receipt.Holder);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Selected = receipt.Selected.Contains(card.Id) ? receipt.Selected.Where(id => id != card.Id).ToArray() : receipt.Selected.Append(card.Id).ToArray() } });
            PublishBoundaryZf(frame);
            return;
        }

        if (branch == "confirm-store")
        {
            PayBoundaryZfStore(frame);
            return;
        }

        if (branch == "target")
        {
            var target = selected.Targets.Single();
            receipt = receipt with
            {
                Target = target,
                Stage = receipt.Direction == 0 ? BoundaryZhouFeiStage.Drawing : BoundaryZhouFeiStage.Discard
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiTargetEvent(frame.Id, frame.OwnerSeat, receipt.Round, target, receipt.Direction == 0));
            if (receipt.Direction == 0)
            {
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, target, 1, new("skill.boundary-zhou-fei.liangyin.draw"));
                return;
            }

            PublishBoundaryZf(frame);
            return;
        }

        if (branch == "discard")
        {
            var card = BoundaryZfOwnedChoiceCard(selected, receipt.Target);
            var from = _cardZonesOperations._cardZones.GetLocation(card.Id);
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.DiscardChildren, Payment = [new(card.Id, card.Kind, from)], Before = before, After = before }, PendingMovementContinuation = new(receipt.Target, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(frame, receipt.Target, [card.Id], new(BoundaryZfDiscardReason));
            SealBoundaryZf(frame.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiPaidEvent(frame.Id, branch, receipt.Target, 1, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
            ContinueBoundaryZf(frame.Id);
            return;
        }

        if (branch is "recover" or "lose-hp")
        {
            var target = selected.Targets.Single();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Target = target, Direction = branch == "recover" ? 2 : 3, Stage = BoundaryZhouFeiStage.EffectChildren, Before = before, After = before }, PendingMovementContinuation = new(target, 0, null) });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiPaidEvent(frame.Id, branch, target, 1, before, before));
            if (branch == "recover")
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(frame.Id, frame.OwnerSeat, target, 1, null, null);
            else
            {
                var outcome = _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(frame.Id, frame.SkillId, target, 1);
                if (outcome != SkillProgramStepOutcome.Continue || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id)
                    return;
            }

            SealBoundaryZf(frame.Id);
            ContinueBoundaryZf(frame.Id);
            return;
        }

        if (branch == "use")
        {
            IssueBoundaryZfUse(frame, int.Parse(selected.Parameters["value"], CultureInfo.InvariantCulture));
            return;
        }

        if (branch == "return")
        {
            ReturnBoundaryZfDeposit(frame);
            return;
        }

        throw new InvalidOperationException("Unknown mobile Zhou Fei branch.");
    }

    internal void PayBoundaryZfStore(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhouFei!;
        if (receipt.Selected.Count == 0)
        {
            FinishBoundaryZf(frame);
            return;
        }

        var valid = BoundaryZfOwnedChoices(frame, frame.OwnerSeat, receipt.Holder, "toggle", OwnedCardMoveIntent.Transfer).Select(choice => BoundaryZfOwnedChoiceCard(choice, receipt.Holder).Id).ToHashSet();
        if (receipt.Selected.Any(id => !valid.Contains(id)))
            throw new InvalidOperationException("Kongsheng cannot debit stale or foreign-zone selected materials.");
        var payment = receipt.Selected.Select(id => new BoundaryZhouFeiMaterial(id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, _cardZonesOperations._cardZones.GetLocation(id))).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var location = receipt.Location!.Value;
        _cardZonesOperations._cardZones.EnsurePrivatePersistentPile(location);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.StoreChildren, Payment = payment, Before = before, After = before }, PendingMovementContinuation = new(receipt.Holder, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(receipt.Selected, location, new(BoundaryZfStoreReason), disclosure: CardMovementDisclosure.FaceDown);
        SealBoundaryZf(frame.Id);
        var count = _cardZonesOperations._cardZones.CardsAt(location).Count;
        if (count > 0)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiStoredEvent(frame.Id, receipt.Source, receipt.Hash, receipt.Holder, location, count, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryZf(frame.Id);
    }

    internal void ReturnBoundaryZfDeposit(ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhouFei!;
        var deposit = BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]);
        var cards = _cardZonesOperations._cardZones.CardsAt(deposit.Location).ToArray();
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var destination = _coreMatchParticipantsPlayersAccess._players[receipt.Holder].IsAlive ? CardLocation.Hand(receipt.Holder) : CardLocation.DiscardPile;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.ReturnChildren, Before = before, After = before, Location = destination, Payment = cards.Select(card => new BoundaryZhouFeiMaterial(card.Id, card.Kind, deposit.Location)).ToArray() }, PendingMovementContinuation = new(receipt.Holder, 0, null) });
        if (cards.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(card => card.Id).ToArray(), destination, new(BoundaryZfReturnReason), disclosure: _coreMatchParticipantsPlayersAccess._players[receipt.Holder].IsAlive ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
        SealBoundaryZf(frame.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiDepositReturnedEvent(frame.Id, deposit.FrameId, receipt.Holder, cards.Length, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        ContinueBoundaryZf(frame.Id);
    }

    internal void SealBoundaryZf(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BoundaryZhouFei = frame.BoundaryZhouFei!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal bool DrainBoundaryZf(ProgramSkillFrame frame) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frame.Id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueBoundaryZf(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryZhouFei: { } receipt } frame || frame.Id != id)
            return;
        if (!ValidBoundaryZf(frame))
            throw new InvalidOperationException("Mobile Zhou Fei lost its original frame or paid native continuation.");
        if (receipt.Stage is BoundaryZhouFeiStage.Drawing or BoundaryZhouFeiStage.IssuingUse or BoundaryZhouFeiStage.NativeUse)
            return;
        if (receipt.Stage is BoundaryZhouFeiStage.StoreChildren or BoundaryZhouFeiStage.DiscardChildren or BoundaryZhouFeiStage.DrawChildren or BoundaryZhouFeiStage.EffectChildren or BoundaryZhouFeiStage.ReturnChildren)
        {
            if (DrainBoundaryZf(frame))
                return;
            frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            receipt = frame.BoundaryZhouFei!;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null });
            if (receipt.Stage != BoundaryZhouFeiStage.ReturnChildren)
            {
                FinishBoundaryZf(frame);
                return;
            }

            receipt = receipt with
            {
                Cursor = receipt.Cursor + 1,
                Stage = BoundaryZhouFeiStage.UseChoice,
                Payment = [],
                Native = null
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt });
        }

        if (receipt.Stage == BoundaryZhouFeiStage.Used)
        {
            ReturnBoundaryZfDeposit(frame);
            return;
        }

        if (receipt.Stage == BoundaryZhouFeiStage.UseChoice)
        {
            while (receipt.Cursor < receipt.Deposits.Count && _cardZonesOperations._cardZones.CardsAt(BoundaryZfDeposit(receipt.Deposits[receipt.Cursor]).Location).Count == 0)
            {
                receipt = receipt with
                {
                    Cursor = receipt.Cursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt });
            }

            if (receipt.Cursor == receipt.Deposits.Count)
            {
                FinishBoundaryZf(frame);
                return;
            }

            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            {
                ReturnBoundaryZfDeposit(frame);
                return;
            }
        }

        PublishBoundaryZf(frame);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin)]
    internal void ReturnBoundaryZfDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.BoundaryZhouFei is not { Stage: BoundaryZhouFeiStage.Drawing, Draw: null } receipt || outcome.RecipientSeat != receipt.Target || outcome.RequestedCount != 1 || !ValidBoundaryZf(frame))
            throw new InvalidOperationException("Liangyin must return its exact one-card native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = receipt with { Stage = BoundaryZhouFeiStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(receipt.Target, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryZhouFeiDrawEvent(frame.Id, receipt.Target, outcome.AttemptFrameId, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryZf(frame.Id);
    }

    internal void FinishBoundaryZf(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { BoundaryZhouFei = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 54700)]
    internal IEnumerable<CardSnapshot> BoundaryZfPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(frame => frame.OwnerSeat == viewer && frame.BoundaryZhouFei is { Stage: BoundaryZhouFeiStage.UseChoice } receipt && receipt.Cursor < receipt.Deposits.Count).SelectMany(frame => _cardZonesOperations._cardZones.CardsAt(BoundaryZfDeposit(frame.BoundaryZhouFei!.Deposits[frame.BoundaryZhouFei.Cursor]).Location).Select(_coreSnapshotValueProjectionOperations.ToSnapshot));
    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 54700)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? BoundaryZfPileViews(IReadOnlyList<NamedAmbushPileSnapshot>? existing, int holder, int viewer, bool revealAll)
    {
        var deposits = BoundaryZfLiveDeposits(holder);
        if (deposits.Length == 0)
            return existing;
        var cards = deposits.SelectMany(deposit => _cardZonesOperations._cardZones.CardsAt(deposit.Location)).ToArray();
        return Array.AsReadOnly((existing ?? []).Append(new NamedAmbushPileSnapshot(BoundaryZfKongsheng, "箜声", cards.Length, revealAll || viewer == holder ? Array.AsReadOnly(cards.Select(card => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(card), card.Kind)).ToArray()) : null)).ToArray());
    }

    [ProgramAiChoice(SkillProgramEffectOp.StoreBoundaryZhouFeiKongsheng, SkillProgramEffectOp.ResolveBoundaryZhouFeiLiangyin, SkillProgramEffectOp.ReturnBoundaryZhouFeiKongsheng)]
    internal PromptChoice AiBoundaryZf(PendingDecision prompt, ProgramSkillFrame frame)
    {
        var receipt = frame.BoundaryZhouFei!;
        if (receipt.Stage == BoundaryZhouFeiStage.Store)
            return receipt.Selected.Count == 0 ? prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "toggle") ?? prompt.Choices.Last() : prompt.Choices.Last();
        if (receipt.Stage == BoundaryZhouFeiStage.Target)
            return prompt.Choices.FirstOrDefault(choice => choice.Targets.Contains(frame.OwnerSeat)) ?? prompt.Choices.First();
        return prompt.Choices.First();
    }
}
