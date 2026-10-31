#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrepDiscardEndingPromisesRuntime
{
    internal bool PrepDiscardEndingConsumed(long id) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingConsumedEvent>().Any(e => e.Id == id);
    internal bool ValidPrepDiscardEndingPromise(PrepDiscardEndingPromise p)
    {
        if (p.Id <= 0 || p.Id != p.ProducerProgramFrameId || p.PrepWindowId <= 0 || p.Count < 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.TargetSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.ActualTurnOwnerSeat) || p.PaidSource.OwnerSeat != p.ActualTurnOwnerSeat || p.EndingSource.OwnerSeat != p.PaidSource.OwnerSeat || p.EndingSource.SkillId != p.PaidSource.SkillId || p.EndingSource.SkillInstanceId != p.PaidSource.SkillInstanceId || p.PaidGameplayHash != p.EndingGameplayHash || p.PaymentBefore < 0 || p.PaymentAfter <= p.PaymentBefore)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<PrepDiscardEndingIssuedEvent>().Count(e => e.Promise.Id == p.Id) != 1 || !history.OfType<PrepDiscardEndingIssuedEvent>().Any(e => e.Promise == p) || history.OfType<PrepDiscardBenefitChosenEvent>().Count(e => e.ProgramFrameId == p.Id && e.Deferred && e.TargetSeat == p.TargetSeat && e.Count == p.Count) != 1)
            return false;
        var paid = history.OfType<PrepDiscardPaidEvent>().Where(e => e.ProgramFrameId == p.Id && !e.OwnerCost).ToArray();
        var frozen = history.OfType<PrepDiscardTargetFrozenEvent>().Where(e => e.ProgramFrameId == p.Id).ToArray();
        if (paid.Length != 1 || frozen.Length != 1 || paid[0].Source != p.PaidSource || paid[0].GameplayHash != p.PaidGameplayHash || paid[0].ActualTurnNumber != p.ActualTurnNumber || paid[0].ActualTurnOwnerSeat != p.ActualTurnOwnerSeat || paid[0].PayerSeat != p.TargetSeat || paid[0].NonEquipmentCount != p.Count || paid[0].SequenceBefore != p.PaymentBefore || paid[0].SequenceAfter != p.PaymentAfter || frozen[0].Source != p.PaidSource || frozen[0].GameplayHash != p.PaidGameplayHash || frozen[0].ActualTurnNumber != p.ActualTurnNumber || frozen[0].ActualTurnOwnerSeat != p.ActualTurnOwnerSeat || frozen[0].PrepWindowId != p.PrepWindowId || frozen[0].TargetSeat != p.TargetSeat || frozen[0].RequestedCount != Math.Max(1, frozen[0].HandCount - frozen[0].Hp) || paid[0].ActualCount != frozen[0].RequiredCount)
            return false;
        var records = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.PaymentBefore && m.Sequence <= p.PaymentAfter && m.From.OwnerSeat == p.TargetSeat && m.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.preparation-discard.target.ResolvePrepDiscardOrEnding").ToArray();
        return records.Length == paid[0].ActualCount && records.Select(m => m.CardId).Distinct().Count() == records.Length && records.Count(m => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(m.CardKind)) == p.Count;
    }

    internal void IssuePrepDiscardEnding(ProgramSkillFrame supplied, string endingBinding)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var d = f.PrepDiscard!;
        if (!_corePrepDiscardReceiptsOperations.ValidPrepDiscard(f) || d.Deferred != true || d.TargetPayment is not { } paid || d.TargetSeat is not { } target || !_corePrepDiscardPaymentsOperations.PrepDiscardCanContinue(f) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
            throw new InvalidOperationException("The Ending promise requires an exact committed preparation payment.");
        var candidate = _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], SkillProgramTriggerWindow.TurnEnding).SingleOrDefault(c => c.SkillId == f.SkillId && c.BindingId == endingBinding && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(c)is { Optional: false, TurnOwnerScope: SkillProgramTurnOwnerScope.PaidPrepDiscardEnding } t && t.Effects is [{ Op: SkillProgramEffectOp.DrawPrepDiscardEnding }]);
        if (candidate is null)
            return;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingIssuedEvent>().Any(e => e.Promise.Id == f.Id))
            throw new InvalidOperationException("An original preparation payment cannot issue its promise twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepDiscardEndingIssuedEvent(new(f.Id, f.Id, d.PrepWindowId, d.Source, d.GameplayHash, new(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId), candidate.GameplayHash, d.ActualTurnNumber, d.ActualTurnOwnerSeat, target, paid.NonEquipmentCount, paid.SequenceBefore, paid.SequenceAfter)));
    }

    internal void ConsumePrepDiscardEnding(PrepDiscardEndingPromise p, bool applied)
    {
        if (!PrepDiscardEndingConsumed(p.Id))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepDiscardEndingConsumedEvent(p.Id, p.ActualTurnNumber, p.ActualTurnOwnerSeat, applied));
    }

    internal void ExpirePrepDiscardEndingPromises()
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolvePrepDiscardOrEnding))
            return;
        foreach (var p in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingIssuedEvent>().Select(e => e.Promise).Where(p => p.ActualTurnNumber < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && !PrepDiscardEndingConsumed(p.Id)).ToArray())
            ConsumePrepDiscardEnding(p, false);
    }

    internal void AppendPrepDiscardEndingItems(List<TurnEndingBoundaryItem> items)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolvePrepDiscardOrEnding))
            return;
        foreach (var p in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingIssuedEvent>().Select(e => e.Promise).Where(p => p.ActualTurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && p.ActualTurnOwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !PrepDiscardEndingConsumed(p.Id)).ToArray())
        {
            var candidate = _coreMatchParticipantsPlayersAccess._players[p.ActualTurnOwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[p.TargetSeat].IsAlive && ValidPrepDiscardEndingPromise(p) ? _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[p.ActualTurnOwnerSeat], SkillProgramTriggerWindow.TurnEnding).SingleOrDefault(c => c.SkillId == p.EndingSource.SkillId && c.BindingId == p.EndingSource.BindingId && c.SkillInstanceId == p.EndingSource.SkillInstanceId && c.GameplayHash == p.EndingGameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger2(c)is { Optional: false, TurnOwnerScope: SkillProgramTurnOwnerScope.PaidPrepDiscardEnding } t && t.Effects is [{ Op: SkillProgramEffectOp.DrawPrepDiscardEnding }]) : null;
            if (candidate is null)
            {
                ConsumePrepDiscardEnding(p, false);
                continue;
            }

            candidate = candidate with
            {
                OccurrenceIndex = items.Count
            };
            items.Add(new(TurnEndingBoundaryItemKind.Program, candidate.Priority, $"prep-discard-ending:{(p.Id)}", candidate, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[p.ActualTurnOwnerSeat])) { PrepDiscardPromise = p });
        }
    }

    internal bool MatchesPrepDiscardEnding(ProgramTriggerCandidate c, ProgramSkillWindowContext context, bool begun)
    {
        if (context.PrepDiscardPromise is not { } p || !ValidPrepDiscardEndingPromise(p) || context.Window != SkillProgramTriggerWindow.TurnEnding || context.SourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || context.TargetSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || p.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || p.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || p.EndingSource != new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) || p.EndingGameplayHash != c.GameplayHash || PrepDiscardEndingConsumed(p.Id) != begun || _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is not { } boundary || boundary.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || boundary.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || boundary.ItemIndex < 0 || boundary.ItemIndex >= boundary.Items.Count || boundary.Items[boundary.ItemIndex].Candidate is not { } original || original.OwnerSeat != c.OwnerSeat || original.SkillId != c.SkillId || original.BindingId != c.BindingId || original.SkillInstanceId != c.SkillInstanceId || original.GameplayHash != c.GameplayHash || original.OccurrenceIndex != c.OccurrenceIndex || context.OccurrenceIndex != c.OccurrenceIndex || boundary.Items[boundary.ItemIndex].PrepDiscardPromise != p)
            return false;
        return !begun || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingConsumedEvent>().Any(e => e.Id == p.Id && e.Applied && e.ActualTurnNumber == p.ActualTurnNumber && e.ActualTurnOwnerSeat == p.ActualTurnOwnerSeat);
    }

    internal bool MatchesPrepDiscardEnding(ProgramSkillFrame f) => f.WindowContext is { } context && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId)is { } boundary && boundary.ItemIndex >= 0 && boundary.ItemIndex < boundary.Items.Count && boundary.Items[boundary.ItemIndex].Candidate is { } c && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, c) && MatchesPrepDiscardEnding(c, context, true);
    internal void ConsumePrepDiscardEndingAtBegin(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.PrepDiscardPromise is not { } p)
            return;
        if (!MatchesPrepDiscardEnding(c, context, false))
            throw new InvalidOperationException("The preparation promise changed before Ending activation.");
        ConsumePrepDiscardEnding(p, true);
    }

    internal void ConsumeSkippedPrepDiscardEnding(TurnEndingBoundaryFrame frame)
    {
        if (frame.Items[frame.ItemIndex].PrepDiscardPromise is { } p)
            ConsumePrepDiscardEnding(p, false);
    }

    internal SkillProgramStepOutcome DrawPrepDiscardEnding(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!MatchesPrepDiscardEnding(f) || f.InstructionIndex != 1)
            throw new InvalidOperationException("A promised draw requires the exact current actual Ending item.");
        var p = f.WindowContext!.PrepDiscardPromise!;
        if (f.PrepDiscardEndingDraw is null)
        {
            // A positive reward that can no longer be requested is cancelled,
            // not recorded as an empty draw. The consumed original item remains.
            if (p.Count > 0 && (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[p.TargetSeat].IsAlive))
                return SkillProgramStepOutcome.Continue;
            var before = _corePrepDiscardPaymentsOperations.PrepDiscardSequence;
            if (p.Count > 0)
                _coreProgramLifecycleOperations.DrawProgramCards(f.Id, p.TargetSeat, p.Count, null, null, SkillProgramCardSetVisibility.Private, new("program.preparation-discard.ending-draw"));
            var after = _corePrepDiscardPaymentsOperations.PrepDiscardSequence;
            var actual = _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(p.TargetSeat) && m.Reason.Value == "program.preparation-discard.ending-draw");
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrepDiscardEndingDraw = new(p, before, after, actual), ReexecuteParticipantInstruction = true });
            if (p.Count > 0)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PrepDiscardEndingDrawnEvent(f.Id, p.Id, p.TargetSeat, p.Count, actual, before, after));
            if (after > before)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = new(p.TargetSeat, 0, null) });
                if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id))
                    return SkillProgramStepOutcome.AwaitChild;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { PendingMovementContinuation = null });
            }
        }

        if (!ValidPrepDiscardEndingDraw(f) || f.PendingMovementContinuation is not null)
            throw new InvalidOperationException("The promised draw returned before its exact real movement children.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrepDiscardEndingDraw = f.PrepDiscardEndingDraw!with { ChildrenCompleted = true }, ReexecuteParticipantInstruction = false });
        return SkillProgramStepOutcome.Continue;
    }

    internal bool ValidPrepDiscardEndingDraw(ProgramSkillFrame f)
    {
        if (f.PrepDiscardEndingDraw is not { } r || !MatchesPrepDiscardEnding(f) || r.Promise != f.WindowContext!.PrepDiscardPromise || r.SequenceBefore < 0 || r.SequenceAfter < r.SequenceBefore || r.SequenceAfter > _corePrepDiscardPaymentsOperations.PrepDiscardSequence || r.ActualCount < 0 || r.ActualCount > r.Promise.Count || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.Promise.TargetSeat) && m.Reason.Value == "program.preparation-discard.ending-draw") != r.ActualCount)
            return false;
        if (r.Promise.Count == 0)
            return r.ActualCount == 0 && r.SequenceBefore == r.SequenceAfter && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingDrawnEvent>().Any(e => e.ProgramFrameId == f.Id);
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardEndingDrawnEvent>().Count(e => e.ProgramFrameId == f.Id && e.PromiseId == r.Promise.Id && e.TargetSeat == r.Promise.TargetSeat && e.RequestedCount == r.Promise.Count && e.ActualCount == r.ActualCount && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) == 1;
    }
}
