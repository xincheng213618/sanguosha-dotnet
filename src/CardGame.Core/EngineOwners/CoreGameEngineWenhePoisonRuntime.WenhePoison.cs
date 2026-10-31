#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenhePoisonRuntime
{
    internal bool TryBeginWenhePoisonFireAttackPayment(CardUseFrame use, Card revealed, Card cost, int sequenceBefore)
    {
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.WenheActivity is null || !_pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(batch => batch.ParentFrameId == use.Id && batch.WenhePoisonDepartures?.Any(d => d.CardId == cost.Id && d.MovementSequence > sequenceBefore) == true))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || use.WenhePoisonFireAttackPayment is not null || use.CardKind != CardKind.FireAttack || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Poison Fire Attack requires its exact newly paid discard boundary.");
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var batches = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(batch => batch.ParentFrameId == use.Id && batch.Movements.Count > 0 && batch.Movements.All(move => move.CardId == cost.Id && move.Sequence > sequenceBefore && move.Sequence <= after)).OrderBy(batch => batch.Id).Select(batch => batch.Id).ToArray();
        var payment = new WenhePoisonFireAttackPayment(cost.Id, CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(use), revealed.Id, revealed.Suit, sequenceBefore, after, batches);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { WenhePoisonFireAttackPayment = payment, Step = ResolutionFrameStep.ResolvingEffect });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenhePoisonFireAttackPaidEvent(use.Id, cost.Id, payment.TargetSeat, revealed.Id, revealed.Suit, sequenceBefore, after));
        ContinueWenhePoisonFireAttackPayment(use.Id);
        return true;
    }

    internal void ContinueWenhePoisonFireAttackPayment(long useId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.Id != useId || use.WenhePoisonFireAttackPayment is not { ActiveChildId: null } payment)
            throw new InvalidOperationException("Poison Fire Attack lost its frozen paid owner.");
        AssertWenhePoisonFireAttackPayment(use);
        while (payment.BatchCursor < payment.BatchIds.Count)
        {
            var batchId = payment.BatchIds[payment.BatchCursor];
            var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.SingleOrDefault(batch => batch.Id == batchId) ?? throw new InvalidOperationException("Poison Fire Attack lost an unpaid movement child.");
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(batch);
            var child = _coreDeferredTurnEndPreludesOperations.CreateCardsMovedProgramWindow(batch);
            if (child is null)
            {
                payment = payment with
                {
                    BatchCursor = payment.BatchCursor + 1
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(use = use with { WenhePoisonFireAttackPayment = payment });
                continue;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(use with { WenhePoisonFireAttackPayment = payment with { ActiveChildId = child.Id } });
            _coreRuntimeOperations.PushRuntimeFrame(child with { WenhePoisonFireAttackReturn = new(use.Id, payment.CostCardId, batch.Id) });
            _coreRuntimeOperations.AdvanceRuntimeTop<CardsMovedTriggerWindowFrame>();
            return;
        }

        var canDamage = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[use.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[payment.TargetSeat].IsAlive;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FireAttackResolvedEvent(use.Id, use.SourceSeat, payment.TargetSeat, payment.RevealedCardId, payment.RevealedSuit, payment.CostCardId, canDamage));
        var card = _coreNativeFireAttackOperations.GetFireAttackEffectCard(use);
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { FireAttackSelection = null, WenhePoisonFireAttackPayment = null });
        if (!canDamage)
        {
            _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(use.Id, card);
            _coreNativeCardUseCompletionOperations.FinishCardUse(use.Id, card, use.CardKind);
            return;
        }

        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(use.Id, use.SourceSeat, payment.TargetSeat, (_characterZhouYuMouOperations.IsMouZyZeroFireUse(use.Id) || _coreTieredRoundZeroReturnsOperations.IsTieredRoundZeroFireAttackUse(use.Id) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(use.Id) || _characterQinLangOlOperations.IsQinLangXianyingUse(use.Id)) ? null : card, damageAmount: 1, playedCardKind: CardKind.FireAttack, physicalCards: _coreNativeTrickUseOperations.GetCardUsePhysicalCards(use.Id));
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal bool ReturnWenhePoisonFireAttackMovement(CardsMovedTriggerWindowFrame child)
    {
        if (child.WenhePoisonFireAttackReturn is not { } returned)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame parent || parent.Id != returned.ParentFrameId || !IsWenhePoisonFireAttackMovement(parent, child))
            throw new InvalidOperationException("Poison Fire Attack movement returned to a different paid owner.");
        var payment = parent.WenhePoisonFireAttackPayment!;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { WenhePoisonFireAttackPayment = payment with { BatchCursor = payment.BatchCursor + 1, ActiveChildId = null } });
        ContinueWenhePoisonFireAttackPayment(parent.Id);
        return true;
    }

    internal bool IsWenhePoisonFireAttackMovement(CardUseFrame parent, CardsMovedTriggerWindowFrame child) => parent.WenhePoisonFireAttackPayment is { } payment && child.WenhePoisonFireAttackReturn is { } returned && returned.ParentFrameId == parent.Id && returned.CostCardId == payment.CostCardId && returned.BatchId == child.Batch.Id && child.Id == child.Batch.Id && payment.ActiveChildId == child.Id && payment.BatchCursor < payment.BatchIds.Count && payment.BatchIds[payment.BatchCursor] == child.Batch.Id && child.Batch.ParentFrameId == parent.Id && child.Batch.Movements.Count > 0 && child.Batch.Movements.All(move => move.CardId == payment.CostCardId && move.Sequence > payment.SequenceBefore && move.Sequence <= payment.SequenceAfter) && child.ResumeProgramFrameId is null;
    internal void AssertWenhePoisonFireAttackPayment(CardUseFrame use)
    {
        var p = use.WenhePoisonFireAttackPayment!;
        var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > p.SequenceBefore && move.Sequence <= p.SequenceAfter).ToArray();
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.WenheActivity is null || use.CardKind != CardKind.FireAttack || use.FireAttackSelection?.RevealedCardId != p.RevealedCardId || CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(use) != p.TargetSeat || p.BatchIds is not System.Collections.IList { IsReadOnly: true } || p.BatchIds.Count != 2 || p.BatchIds.Distinct().Count() != 2 || p.BatchCursor < 0 || p.BatchCursor > p.BatchIds.Count || p.ActiveChildId is not null && p.BatchCursor >= p.BatchIds.Count || moves.Length != 2 || moves.Any(move => move.CardId != p.CostCardId) || moves[0].From != CardLocation.Hand(use.SourceSeat) || moves[0].To != CardLocation.Processing || moves[0].Reason != CardMoveReasons.FireAttackDiscard || moves[0].Disclosure != CardMovementDisclosure.FaceUp || moves[1].From != CardLocation.Processing || moves[1].To != CardLocation.DiscardPile || moves[1].Reason != CardMoveReasons.FireAttackDiscardFinished || _coreCardAttackStateOperations.GetAttackCard(p.CostCardId).Kind != CardKind.Peach || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenhePoisonFireAttackPaidEvent>().Count(e => e == new WenhePoisonFireAttackPaidEvent(use.Id, p.CostCardId, p.TargetSeat, p.RevealedCardId, p.RevealedSuit, p.SequenceBefore, p.SequenceAfter)) != 1)
            throw new InvalidOperationException("Poison Fire Attack changed its exact paid discard or native effect continuation.");
    }

    internal int? CaptureWenhePoisonRound() => _coreActivityBattleRulesOperations.Value.WenheRules?.PoisonRound;
    internal bool IsWenhePhysicalPoison(CharacterState owner, Card card) => _coreActivityBattleRulesOperations.Value.WenheRules?.IsPhysicalPoison(card.Kind, card.Id, owner.IsAlive, card.Id > 0 && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Hand(owner.Seat)) ?? false;
    internal CardKind EffectiveConversionInputKind(CharacterState owner, Card card) => IsWenhePhysicalPoison(owner, card) ? CardKind.Poison : card.Kind;
    [ProgramPipelineHook(ProgramPipelinePhase.HandCardProjection, 20)]
    internal CardSnapshot ProjectWenhePoisonHandCard(CharacterState owner, Card card, CardSnapshot snapshot) => IsWenhePhysicalPoison(owner, card) ? snapshot with
    {
        IdentityKind = CardKind.Poison,
        IdentitySkillName = "宴安鸩毒"
    }

    : snapshot;
    internal CardMovementBatchContext CaptureWenhePoisonDepartures(CardMovementBatchContext batch, int? poisonRound)
    {
        if (poisonRound is not > 0 || _coreActivityBattleRulesOperations.Value.WenheRules is not { } rules)
            return batch;
        var departures = batch.Movements.Where(move => move.From.OwnerSeat is { } owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && rules.AllowsPoisonDeparture(new(_coreCardAttackStateOperations.GetAttackCard(move.CardId).Kind, move.Disclosure, move.From.Zone, owner, move.To.Zone, move.To.OwnerSeat, _coreMatchParticipantsPlayersAccess._players[owner].IsAlive))).OrderBy(move => move.Sequence).Select(move => new WenhePoisonDeparture(poisonRound.Value, move.Sequence, move.From.OwnerSeat!.Value, move.CardId)).ToArray();
        return departures.Length == 0 ? batch : batch with
        {
            WenhePoisonDepartures = departures
        };
    }

    internal bool HasWenhePoisonDepartures(CardMovementBatchContext batch) => batch.WenhePoisonDepartures is { Count: > 0 };
    internal bool IsWenhePoisonUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => HasWenhePoisonDepartures(batch) && batch.ParentFrameId == use.Id && batch.ParentBatchId is null && IsWenhePoisonUsePaymentOrigin(use, batch) && use.Action is { Type: CardActionType.Use } action && batch.Movements.Count == action.PhysicalCards.Count && batch.Movements.Count > 0 && batch.Movements.Select(move => move.CardId).Distinct().Count() == batch.Movements.Count && batch.Movements.All(move => move.Reason == CardMoveReasons.Use && move.Disclosure == CardMovementDisclosure.FaceUp && move.To == CardLocation.Processing && action.PhysicalCards.Count(cost => cost.CardId == move.CardId && cost.From == move.From && cost.CardKind == move.CardKind) == 1 && move.Sequence > 0 && move.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[move.Sequence - 1] == move);
    internal bool IsWenhePoisonUsePaymentOrigin(CardUseFrame use, CardMovementBatchContext batch)
    {
        if (batch.AwaitingProgramFrameId is null)
            return true;
        // A real rescue Use can sit above a suspended discard program. That
        // ancestor owns its original payment, not this newly issued rescue cost.
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == use.Id);
        if (use.DyingResponse is not { } response || useIndex < 1 || _resolutionStackOperations._resolutionStack[useIndex - 1] is not DyingFrame dying || response.ResolutionId != dying.Id || response.ResponderSeat != use.SourceSeat || dying.ResponderSeat != use.SourceSeat || !(use.CardKind == CardKind.Peach && response.UsedPeach || use.CardKind == CardKind.Alcohol && response.UsedAlcohol))
            return false;
        var programIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == batch.AwaitingProgramFrameId);
        return programIndex >= 0 && programIndex < useIndex - 1 && _resolutionStackOperations._resolutionStack[programIndex] is ProgramSkillFrame program && CoreGameEngineSkillProgramsRuntime.IsAwaitingProgramMovement(program) && batch.OriginOwnerSeat == program.OwnerSeat && batch.OriginSkillId == program.SkillId && batch.OriginSkillInstanceId == program.SkillInstanceId;
    }

    internal void CaptureWenhePoisonUsePayment(CardMovementBatchContext batch)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || use.PaymentMovementReceipt is not null || !IsWenhePoisonUsePaymentBatch(use, batch))
            return;
        _coreRuntimeOperations.ReplaceRuntimeTop(use with { PaymentMovementReceipt = new(use.Id, use.Action!.ActionId, batch.Movements[0].Sequence - 1, batch.Movements[^1].Sequence) { Batch = batch } });
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.LeadingTail, 20)]
    internal bool HasWenhePoisonUsePaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation) => continuation.Kind is RecoveryPaidCardUseKind.Trick or RecoveryPaidCardUseKind.Simple or RecoveryPaidCardUseKind.EquipmentUse && use.PaymentMovementReceipt is { } receipt && receipt.OwnerFrameId == use.Id && IsWenhePoisonUsePaymentBatch(use, receipt.Batch) && CoreGameEngineCardUsePaymentsRuntime.CardUsePaymentBatchMatchesReceipt(use, receipt.Batch);
    internal bool IsPendingWenhePoisonWindow(CardsMovedTriggerWindowFrame frame) => HasWenhePoisonDepartures(frame.Batch) && frame.CandidateIndex == 0 && (frame.WenhePoisonLoss is null || frame.WenhePoisonLoss is { Stage: not WenhePoisonLossStage.Completed } receipt && receipt.Cursor >= 0 && receipt.Cursor < frame.Batch.WenhePoisonDepartures!.Count);
    internal bool IsCurrentWenhePoisonDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id)is var index && index > 0 && IsWenhePoisonDyingChild(_resolutionStackOperations._resolutionStack[index - 1], dying);
    internal bool IsOriginalDyingSuspendedByWenhePoison(DyingFrame original)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == original.Id);
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count)
            return false;
        var response = _resolutionStackOperations._resolutionStack[index + 1];
        var actualResponse = response is CardUseFrame { DyingResponse: { } use } && use.ResolutionId == original.Id || response is ProgramSkillFrame { WindowContext: { } program } && program.ParentFrameId == original.Id && program.Window is SkillProgramTriggerWindow.DyingResponse or SkillProgramTriggerWindow.SelfDyingResponse || response is ProgramLifecycleTriggerWindowFrame { ResumeDyingFrameId: { } entry } && entry == original.Id;
        if (!actualResponse)
            return false;
        for (var at = index + 2; at < _resolutionStackOperations._resolutionStack.Count; at++)
        {
            if (_resolutionStackOperations._resolutionStack[at] is not CardsMovedTriggerWindowFrame poison || !IsPendingWenhePoisonWindow(poison) || poison.Batch.ParentFrameId is not { } parent)
                continue;
            var parentIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == parent);
            if (parentIndex > index && parentIndex < at)
                return true;
        }

        return false;
    }

    internal bool TryAdvanceWenhePoisonLoss(CardsMovedTriggerWindowFrame supplied)
    {
        if (!HasWenhePoisonDepartures(supplied.Batch))
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardsMovedTriggerWindowFrame frame || frame.Id != supplied.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("Poison requires its exact active movement window.");
        AssertWenhePoisonMovementWindow(frame);
        if (frame.WenhePoisonLoss is null)
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { WenhePoisonLoss = new(0, WenhePoisonLossStage.Ready) });
        while (frame.WenhePoisonLoss!.Cursor < frame.Batch.WenhePoisonDepartures!.Count)
        {
            var receipt = frame.WenhePoisonLoss;
            var departure = frame.Batch.WenhePoisonDepartures[receipt.Cursor];
            if (receipt.Stage == WenhePoisonLossStage.Dying)
                throw new InvalidOperationException("Poison cannot resume before its precise dying child returns.");
            if (receipt.Stage == WenhePoisonLossStage.Ready)
            {
                var owner = _coreMatchParticipantsPlayersAccess._players[departure.OwnerSeat];
                var before = owner.Hp;
                var after = owner.IsAlive ? checked(before - _coreActivityBattleRulesOperations.Value.WenheRules!.PoisonHpLoss) : before;
                _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { WenhePoisonLoss = receipt with { Stage = WenhePoisonLossStage.HealthChildren, HpBefore = before, HpAfter = after } });
                owner.Hp = after;
                _coreHpChangeProgramsOperations.RecordHpChange(frame.Id, null, owner.Seat, before, after, HpChangeKind.Loss);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenhePoisonLossAppliedEvent(frame.Id, frame.Batch.Id, departure.RoundNumber, departure.MovementSequence, owner.Seat, departure.CardId, before, after));
                if (owner.IsAlive && before > 0 && after == 0)
                {
                    BeginWenhePoisonDying(frame, departure);
                    return true;
                }
            }

            frame = (CardsMovedTriggerWindowFrame)_resolutionStackOperations._resolutionStack.Last();
            if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frame.Id, PostEventContinuation.WenhePoison))
                return true;
            receipt = frame.WenhePoisonLoss!;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WenhePoisonDepartureSettledEvent(frame.Id, frame.Batch.Id, departure.MovementSequence, departure.OwnerSeat, departure.CardId));
            var cursor = receipt.Cursor + 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { WenhePoisonLoss = new(cursor, cursor == frame.Batch.WenhePoisonDepartures!.Count ? WenhePoisonLossStage.Completed : WenhePoisonLossStage.Ready) });
        }

        return false;
    }

    internal void BeginWenhePoisonDying(CardsMovedTriggerWindowFrame frame, WenhePoisonDeparture departure)
    {
        var id = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WenhePoisonLoss = frame.WenhePoisonLoss!with { Stage = WenhePoisonLossStage.Dying, DyingFrameId = id } });
        _coreRuntimeOperations.PushRuntimeFrame(new DyingFrame(id, frame.Id, departure.OwnerSeat, null, Array.AsReadOnly(_coreNativeDyingOperations.BuildDyingResponderSeats(departure.OwnerSeat).ToArray()), 0, DyingContinuationKind.WenhePoison));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerDyingEvent(id, departure.OwnerSeat, null));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (!_coreProgramLifecycleOperations.TryBeginMandatorySelfDyingProgram(_coreDyingCursorRuntimeOperations.Value.ActiveDying!) && !_coreDyingEntryProgramsOperations.TryBeginDyingEntryProgramWindow(_coreDyingCursorRuntimeOperations.Value.ActiveDying!))
            _coreNativeDyingOperations.ExposeHumanDyingPrompt();
    }

    internal void ReturnWenhePoisonDying(GameEngine.DyingCompletionReceipt returned)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardsMovedTriggerWindowFrame frame || frame.Id != returned.ParentFrameId || returned.Continuation != DyingContinuationKind.WenhePoison || frame.WenhePoisonLoss is not { Stage: WenhePoisonLossStage.Dying } receipt || receipt.DyingFrameId != returned.FrameId || frame.Batch.WenhePoisonDepartures![receipt.Cursor].OwnerSeat != returned.VictimSeat)
            throw new InvalidOperationException("Poison dying returned to a different paid departure.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { WenhePoisonLoss = receipt with { Stage = WenhePoisonLossStage.HealthChildren, DyingFrameId = null } });
        _coreRuntimeOperations.AdvanceRuntimeFrame(frame.Id);
    }

    internal void AssertWenhePoisonMovementWindow(CardsMovedTriggerWindowFrame frame)
    {
        if (frame.Batch.WenhePoisonDepartures is not { } departures)
        {
            if (frame.WenhePoisonLoss is not null)
                throw new InvalidOperationException("Poison lost its departure list.");
            return;
        }

        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.WenheActivity is null || departures.Count == 0 || departures is not System.Collections.IList { IsReadOnly: true } || departures.Select(d => d.MovementSequence).Distinct().Count() != departures.Count || !departures.Select(d => d.MovementSequence).SequenceEqual(departures.Select(d => d.MovementSequence).Order()) || departures.Any(d => d.RoundNumber < 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.OwnerSeat) || frame.Batch.Movements.Count(move => move.Sequence == d.MovementSequence && move.CardId == d.CardId && _coreCardAttackStateOperations.GetAttackCard(move.CardId).Kind == CardKind.Peach && move.Disclosure == CardMovementDisclosure.FaceUp && move.From == CardLocation.Hand(d.OwnerSeat) && move.To != move.From) != 1 || !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenheRoundRuleChangedEvent>().Any(rule => rule.RoundNumber == d.RoundNumber && rule.Rule == WenheRuleKind.Poison)))
            throw new InvalidOperationException("Poison changed its frozen public physical departure proof.");
        if (frame.WenhePoisonLoss is not { } receipt)
            return;
        if (!Enum.IsDefined(receipt.Stage) || receipt.Cursor < 0 || receipt.Cursor > departures.Count || (receipt.Stage == WenhePoisonLossStage.Completed) != (receipt.Cursor == departures.Count) || (receipt.Stage == WenhePoisonLossStage.Dying) != (receipt.DyingFrameId is not null))
            throw new InvalidOperationException("Poison changed its serial departure cursor or native child.");
        var applied = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenhePoisonLossAppliedEvent>().Where(e => e.WindowFrameId == frame.Id).ToArray();
        var settled = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<WenhePoisonDepartureSettledEvent>().Where(e => e.WindowFrameId == frame.Id).ToArray();
        var loss = _coreActivityBattleRulesOperations.Value.WenheRules!.PoisonHpLoss;
        var activeIssued = receipt.Stage is WenhePoisonLossStage.HealthChildren or WenhePoisonLossStage.Dying ? 1 : 0;
        if (applied.Length != receipt.Cursor + activeIssued || settled.Length != receipt.Cursor || applied.Where((e, i) => e.BatchId != frame.Batch.Id || e.RoundNumber != departures[i].RoundNumber || e.MovementSequence != departures[i].MovementSequence || e.OwnerSeat != departures[i].OwnerSeat || e.CardId != departures[i].CardId || ((long)e.HpBefore - e.HpAfter < 0 || (long)e.HpBefore - e.HpAfter > loss)).Any() || settled.Where((e, i) => e.BatchId != frame.Batch.Id || e.MovementSequence != departures[i].MovementSequence || e.OwnerSeat != departures[i].OwnerSeat || e.CardId != departures[i].CardId).Any() || activeIssued != 0 && (applied[^1].HpBefore != receipt.HpBefore || applied[^1].HpAfter != receipt.HpAfter))
            throw new InvalidOperationException("Poison lost an exact once-issued HP loss or departure return.");
    }

    internal bool IsWenhePoisonHealthChild(ResolutionFrame? parent, HpChangedTriggerWindowFrame child) => parent is CardsMovedTriggerWindowFrame { WenhePoisonLoss: { Stage: WenhePoisonLossStage.HealthChildren } receipt } frame && frame.Id == child.ResumeFrameId && child.Continuation == PostEventContinuation.WenhePoison && child.Change.ParentFrameId == frame.Id && child.Change.TargetSeat == frame.Batch.WenhePoisonDepartures![receipt.Cursor].OwnerSeat && child.Change.Kind == HpChangeKind.Loss && child.Change.HpBefore == receipt.HpBefore && child.Change.HpAfter == receipt.HpAfter;
    internal bool IsWenhePoisonDyingChild(ResolutionFrame? parent, DyingFrame child) => parent is CardsMovedTriggerWindowFrame { WenhePoisonLoss: { Stage: WenhePoisonLossStage.Dying } receipt } frame && child.ParentFrameId == frame.Id && child.Id == receipt.DyingFrameId && child.KillerSeat is null && child.Continuation == DyingContinuationKind.WenhePoison && child.VictimSeat == frame.Batch.WenhePoisonDepartures![receipt.Cursor].OwnerSeat && receipt.HpAfter == 0;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 20)]
    internal bool HasIssuedWenhePoisonPaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => IsWenhePoisonUsePaymentBatch(use, batch) && CoreGameEngineCardUsePaymentsRuntime.CardUsePaymentBatchMatchesReceipt(use, batch);
    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 1200)]
    internal void CaptureMovementBatchStartWenhePoisonRound(CardMovementBatchBuilder batch) => batch.WenhePoisonRound = CaptureWenhePoisonRound();
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 300)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonDepartures(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => CaptureWenhePoisonDepartures(completed, batch.WenhePoisonRound);
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 2100)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureWenhePoisonUsePayment(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureWenhePoisonUsePayment(completed);
        return completed;
    }
}
