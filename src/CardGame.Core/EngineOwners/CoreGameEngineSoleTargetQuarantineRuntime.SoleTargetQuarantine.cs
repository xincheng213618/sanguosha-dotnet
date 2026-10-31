#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSoleTargetQuarantineRuntime
{
    internal bool IsValidSoleTargetProtectionPaidFact(SoleTargetProtectionReceipt r)
    {
        if (r.Cost is not { } cost || r.InstructionIndex != 1 || r.ProgramFrameId <= r.WindowFrameId || r.WindowFrameId <= r.CardUseFrameId || r.ActionId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Source.OwnerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ProviderSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ProtectedSeat) || r.Source.OwnerSeat == r.ActorSeat || r.Source.OwnerSeat == r.ProtectedSeat || r.OwnerHp <= r.ProtectedHp || r.ActualTurnNumber <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActualTurnOwnerSeat) || cost.From.OwnerSeat != r.Source.OwnerSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.CostSequenceAfter <= r.CostSequenceBefore || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || !_coreCompletedCategoryUseProofOperations.ActualHandGainMutationBinding(r.ProgramFrameId, r.Source.SkillId, r.Source.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.ProtectOtherSoleCardTarget) || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Any(e => e.FrameId == r.ProgramFrameId && e.SkillId == r.Source.SkillId && e.BindingId == r.Source.BindingId && e.SkillInstanceId == r.Source.SkillInstanceId && e.OwnerSeat == r.Source.OwnerSeat))
            return false;
        var destination = cost.Kind == CardKind.GeneralWeapon && cost.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.DiscardPile;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.TurnNumber == r.ActualTurnNumber && m.CardId == cost.CardId && m.CardKind == cost.Kind && m.From == cost.From && m.To == destination && m.Reason.Value == "skill-program.sole-target-protection.cost" && m.Sequence > r.CostSequenceBefore && m.Sequence <= r.CostSequenceAfter) == 1;
    }

    internal bool MatchesSoleTargetProtectionOriginalTargets(long id, IReadOnlyList<int> originalTargets) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { } action, SoleTargetProtectionOrigin: { } r } use && r.CardUseFrameId == id && r.ActionId == action.ActionId && r.ActorSeat == use.SourceSeat && r.ProviderSeat == action.ProviderSeat && r.EffectiveKind == use.CardKind && r.OriginalTargets.SequenceEqual(originalTargets) && r.Materials.All(m => action.PhysicalCards.Any(c => c.CardId == m.CardId && c.CardKind == m.Kind && c.From == m.From)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetProtectionPaidEvent>().Any(e => e.Receipt.CardUseFrameId == id && e.Receipt.ActionId == r.ActionId && e.Receipt.OriginalTargets.SequenceEqual(originalTargets) && e.Receipt.Materials.SequenceEqual(r.Materials) && IsValidSoleTargetProtectionPaidFact(e.Receipt) && _coreCompletedCategoryUseProofOperations.ActualHandGainMutationBinding(e.Receipt.ProgramFrameId, e.Receipt.Source.SkillId, e.Receipt.Source.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.ProtectOtherSoleCardTarget)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetReplacedEvent>().Any(e => IsSoleTargetReplacementFact(use, e, originalTargets)) && _coreCompletedCategoryUseProofOperations.HasExactAcceptedActualHandGainUse(use, action);
    internal bool HasExactSoleTargetQuarantine(CardUseFrame use)
    {
        if (use.SoleTargetQuarantine is not { } r || use.Action is not { } action || r.CardUseFrameId != use.Id || r.ActionId != action.ActionId || r.ActorSeat != use.SourceSeat || r.ActorSeat != action.ActorSeat || r.ProviderSeat != action.ProviderSeat || r.EffectiveKind != use.CardKind || r.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || !r.TargetSeats.SequenceEqual(use.TargetSeats) || !action.TargetSeats.SequenceEqual(use.TargetSeats) || use.SoleTargetProtectionSources?.Contains(r.Source) != true || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetProtectionPaidEvent>().Any(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == action.ActionId && e.Receipt.Source == r.Source && e.Receipt.GameplayHash == r.GameplayHash && IsValidSoleTargetProtectionPaidFact(e.Receipt)))
            return false;
        if (r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.Materials.Any(m => !action.PhysicalCards.Any(c => c.CardId == m.CardId && c.From == m.From && c.CardKind == m.Kind) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.CardId == m.CardId && move.From == CardLocation.Processing && move.To == CardLocation.OutsideGame && move.Reason.Value == "skill-program.sole-target-protection.quarantine" && move.Sequence > r.QuarantineSequenceBefore && move.Sequence <= r.QuarantineSequenceAfter)))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetQuarantineIssuedEvent>().Any(e => e.Receipt.CardUseFrameId == r.CardUseFrameId && e.Receipt.ActionId == r.ActionId && e.Receipt.Source == r.Source && e.Receipt.Materials.SequenceEqual(r.Materials) && e.Receipt.QuarantineSequenceBefore == r.QuarantineSequenceBefore && e.Receipt.QuarantineSequenceAfter == r.QuarantineSequenceAfter);
    }

    internal bool IsSoleTargetQuarantinedMaterial(long id, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { } use && HasExactSoleTargetQuarantine(use) && use.SoleTargetQuarantine!.Materials.Any(m => m.CardId == cardId) && (_cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame || IsGameEndedSoleTargetMaterial(use, cardId));
    internal bool IsGameEndedSoleTargetMaterial(CardUseFrame use, int cardId) => _coreMatchOutcomeWinnerAccess._winner != Winner.None && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.DiscardPile && use.SoleTargetQuarantine is { } r && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetQuarantineSettledEvent>().Any(e => e.GameEnded && e.CardUseFrameId == use.Id && e.ActionId == r.ActionId && e.ActorSeat == r.ActorSeat && e.Destination == CardLocation.DiscardPile && e.CardIds.Contains(cardId) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == cardId && m.Sequence > e.SequenceBefore && m.Sequence <= e.SequenceAfter && m.From == CardLocation.OutsideGame && m.To == CardLocation.DiscardPile && m.Reason.Value == "skill-program.sole-target-protection.game-end") == 1);
    internal bool TryCompleteSoleTargetQuarantinedUse(long id)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id);
        if (use?.SoleTargetQuarantine is null || use.Step == ResolutionFrameStep.Completed)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != id || !HasExactSoleTargetQuarantine(use) || use.SoleTargetQuarantine.Materials.Any(m => !IsSoleTargetQuarantinedMaterial(use.Id, m.CardId)))
            throw new InvalidOperationException("A quarantined use lost its exact paid material issuance or owning completion.");
        var attack = use.CardAttack is null ? null : _kernelCoreMainOperations.CreateCardAttackHandle(id);
        var completion = attack is null ? null : _coreCardAttackStateOperations.CaptureAttackCompletion(attack);
        var card = use.CardId > 0 ? _coreCardAttackStateOperations.GetAttackCard(use.CardId) : new Card(0, use.CardKind, Suit.None, 0);
        _coreNativeCardUseCompletionOperations.QueueCardUseFinishedWithoutPop(id, card, use.CardKind);
        if (_coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreCardActionsOperations.TryBeginProgramCardWindow(attack, use.Action!, SkillProgramTriggerWindow.CardUseCompleted, use.TargetSeats, attack is null ? ProgramCardContinuation.CompletedCard : ProgramCardContinuation.CompletedSlash))
            return true;
        _coreNativeCardUseCompletionOperations.PopFinishedCardUse(id);
        if (attack is not null)
            _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        if (completion is not null)
            _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
        return true;
    }

    internal SoleTargetQuarantineReceipt[] OutstandingSoleTargetQuarantines(int? turn = null, int? owner = null)
    {
        if (!_coreSoleTargetProtectionOperations.Value.HasSoleTargetProtectionCapability)
            return[];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var settled = history.OfType<SoleTargetQuarantineSettledEvent>().Select(e => (e.CardUseFrameId, e.ActionId)).ToHashSet();
        return history.OfType<SoleTargetQuarantineIssuedEvent>().Select(e => e.Receipt).Where(r => !settled.Contains((r.CardUseFrameId, r.ActionId)) && (turn is null || r.ActualTurnNumber == turn) && (owner is null || r.ActualTurnOwnerSeat == owner)).OrderBy(r => r.CardUseFrameId).ToArray();
    }

    internal void SettleSoleTargetQuarantine(SoleTargetQuarantineReceipt r, bool gameEnded)
    {
        if (r.Materials.Any(m => _cardZonesOperations._cardZones.GetLocation(m.CardId) != CardLocation.OutsideGame))
            throw new InvalidOperationException("A due quarantine lost one of its actual isolated materials.");
        var destination = !gameEnded && _coreMatchParticipantsPlayersAccess._players[r.ActorSeat].IsAlive ? CardLocation.Hand(r.ActorSeat) : CardLocation.DiscardPile;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (r.Materials.Count > 0)
            _corePhysicalMovementFacadesOperations.MoveCards(r.Materials.Select(m => _coreCardAttackStateOperations.GetAttackCard(m.CardId)).ToArray(), CardLocation.OutsideGame, destination, new(gameEnded ? "skill-program.sole-target-protection.game-end" : "skill-program.sole-target-protection.return"));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new SoleTargetQuarantineSettledEvent(r.CardUseFrameId, r.ActionId, r.ActorSeat, r.ActualTurnNumber, r.ActualTurnOwnerSeat, destination, Array.AsReadOnly(r.Materials.Select(m => m.CardId).ToArray()), before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, gameEnded));
    }

    internal void SettleGameEndedSoleTargetQuarantines()
    {
        foreach (var r in OutstandingSoleTargetQuarantines())
            SettleSoleTargetQuarantine(r, true);
    }

    internal bool ContinueSoleTargetQuarantineReturns(DeferredTurnEndFrame parent)
    {
        var cursor = parent.SoleTargetQuarantineReturns;
        var afterEnded = parent.AfterTurnEnded is not null;
        var resumedAfterEnded = afterEnded && _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDianjunActualTurnEnd) && _coreAfterTurnEndedProgramsOperations.IsActualAfterTurnEndedParent(parent) && parent.AfterTurnEnded!.CurrentChild is null && parent.AfterTurnEnded.ItemIndex >= 0 && parent.AfterTurnEnded.ItemIndex <= parent.AfterTurnEnded.Items.Count;
        if (resumedAfterEnded && (cursor is null || cursor.Index == cursor.Receipts.Count))
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault() != parent)
                throw new InvalidOperationException("Late quarantine return requires its exact resumed actual-end parent.");
            // The original cursor was frozen before Dianjun's end-owned Play.
            // Add only still-outstanding receipts from this same actual turn;
            // retain the consumed prefix and the original after-end window.
            var late = OutstandingSoleTargetQuarantines(parent.TurnNumber, parent.OwnerSeat);
            if (late.Length > 0)
            {
                var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
                if (!history.OfType<ZhangYiResolutionStartedEvent>().Any(start => start.ParentWindowId == parent.Id && start.ActualTurnNumber == parent.TurnNumber && start.Source.OwnerSeat == parent.OwnerSeat && history.OfType<ZhangYiDianjunCleanupIssuedEvent>().Any(cleaned => cleaned.FrameId == start.FrameId && cleaned.OwnerSeat == parent.OwnerSeat && cleaned.ActualTurnNumber == parent.TurnNumber)))
                    throw new InvalidOperationException("A late quarantine lacks its completed end-owned Play return.");
                if (cursor is not null && late.Any(r => cursor.Receipts.Any(old => old.CardUseFrameId == r.CardUseFrameId && old.ActionId == r.ActionId)))
                    throw new InvalidOperationException("A consumed quarantine cursor retained an unsettled receipt.");
                cursor = cursor is null ? new(Array.AsReadOnly(late)) : cursor with
                {
                    Receipts = Array.AsReadOnly(cursor.Receipts.Concat(late).ToArray())
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { SoleTargetQuarantineReturns = cursor });
            }
        }

        if (cursor is null)
            return false;
        if (cursor.MovementChildFrameId is not null || parent.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || parent.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || parent.Current is not null || afterEnded && !resumedAfterEnded || parent.Prelude is { Completed: false } || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.ActorSeat == parent.OwnerSeat && e.TurnNumber == parent.TurnNumber))
            throw new InvalidOperationException("Quarantine return requires its actual completed turn and drained owned movement child.");
        while (true)
        {
            while (_pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(b => b.AwaitingProgramFrameId is null && b.ParentFrameId == parent.Id))
            {
                var batch = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.AwaitingProgramFrameId is null && b.ParentFrameId == parent.Id).OrderBy(b => b.Id).First();
                _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(batch);
                var child = _coreDeferredTurnEndPreludesOperations.CreateCardsMovedProgramWindow(batch);
                if (child is null)
                    continue;
                cursor = cursor with
                {
                    MovementChildFrameId = child.Id,
                    MovementBatchId = batch.Id,
                    CurrentBatch = batch
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(parent with { SoleTargetQuarantineReturns = cursor });
                _coreRuntimeOperations.PushRuntimeFrame(child with { SoleTargetQuarantineEndReturn = parent.Id });
                _coreRuntimeOperations.AdvanceRuntimeTop<CardsMovedTriggerWindowFrame>();
                return true;
            }

            if (cursor.Index == cursor.Receipts.Count)
                return false;
            var r = cursor.Receipts[cursor.Index];
            if (r.ActualTurnNumber != parent.TurnNumber || r.ActualTurnOwnerSeat != parent.OwnerSeat || !OutstandingSoleTargetQuarantines(parent.TurnNumber, parent.OwnerSeat).Any(x => x.CardUseFrameId == r.CardUseFrameId && x.ActionId == r.ActionId))
                throw new InvalidOperationException("An actual-end cursor retained a premature or already settled use.");
            cursor = cursor with
            {
                Index = cursor.Index + 1
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { SoleTargetQuarantineReturns = cursor });
            SettleSoleTargetQuarantine(r, false);
        }
    }

    internal bool MatchesSoleTargetQuarantineReturn(DeferredTurnEndFrame parent, CardsMovedTriggerWindowFrame child) => parent.SoleTargetQuarantineReturns is { CurrentBatch: { } batch } cursor && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && cursor.Index > 0 && cursor.Index <= cursor.Receipts.Count && cursor.MovementChildFrameId == child.Id && cursor.MovementBatchId == child.Batch.Id && child.SoleTargetQuarantineEndReturn == parent.Id && child.ResumeProgramFrameId is null && child.Batch.ParentFrameId == parent.Id && child.Batch.AwaitingProgramFrameId is null && CoreGameEngineDeferredTurnEndPreludesRuntime.SameDeferredTurnEndBatch(batch, child.Batch);
    internal void ReturnSoleTargetQuarantineMovement(CardsMovedTriggerWindowFrame child)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeferredTurnEndFrame parent || !MatchesSoleTargetQuarantineReturn(parent, child) || child.CandidateIndex != child.Candidates.Count)
            throw new InvalidOperationException("A quarantine gain lost its exact actual-end return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { SoleTargetQuarantineReturns = parent.SoleTargetQuarantineReturns!with { MovementChildFrameId = null, MovementBatchId = null, CurrentBatch = null } });
        _coreDeferredHandAlignmentsOperations.ContinueDeferredTurnEnd();
    }

    internal void RefreshSoleProtectionObservers(CardUseFrame use)
    {
        var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Last(w => w.ParentFrameId == use.Id && CoreGameEngineCardActionsRuntime.GetCardActionWindow(w) == SkillProgramTriggerWindow.CardUseTargetsFinalized);
        var extra = _coreCardActionsOperations.CollectSharedCardActionCandidates(use.Action!, SkillProgramTriggerWindow.CardUseTargetsFinalized, _coreSoleTargetProtectionOperations.Value.SoleProtectionMainTargets(use), null, window.Continuation).Where(c => _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.Triggers.Single(t => t.Id == c.TriggerId).Effects.Any(e => e.Op == SkillProgramEffectOp.ProtectOtherSoleCardTarget) && use.SoleTargetProtectionSources?.Contains(new(c.SkillId, c.TriggerId, c.OwnerSeat, c.SkillInstanceId)) != true && !window.Candidates.Skip(window.CandidateIndex + 1).Any(old => old.OwnerSeat == c.OwnerSeat && old.SkillId == c.SkillId && old.TriggerId == c.TriggerId && old.SkillInstanceId == c.SkillInstanceId)).Select(c => c with { FrozenContext = c.FrozenContext!with { ParentFrameId = window.Id, CardUse = c.FrozenContext.CardUse!with { ParentCardUseFrameId = use.Id } } }).ToArray();
        if (extra.Length > 0)
            _coreRuntimeOperations.ReplaceRuntimeFrame(window.Id, window with { Candidates = Array.AsReadOnly(window.Candidates.Concat(extra).ToArray()) });
    }

    internal bool IsSoleTargetReplacementFact(CardUseFrame use, SoleTargetReplacedEvent fact, IReadOnlyList<int> before)
    {
        if (_coreProgramTargetWindowProtocolsOperations.TargetWindowProtocols.AcceptsReplacementFact(use, fact, before))
            return true;
        if (fact.CardUseFrameId != use.Id || fact.ActionId != use.Action?.ActionId || !fact.BeforeTargets.SequenceEqual(before) || fact.PreviousTargetSeat == fact.NewTargetSeat || fact.NewTargetSeat != fact.Source.OwnerSeat || use.SoleTargetProtectionSources?.Contains(fact.Source) != true)
            return false;
        var expected = before.Select((s, i) => s == fact.PreviousTargetSeat && (use.CardKind != CardKind.BorrowedSword || i % 2 == 0) ? fact.NewTargetSeat : s);
        if (!expected.SequenceEqual(fact.AfterTargets))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SoleTargetProtectionPaidEvent>().Count(e => e.Receipt.CardUseFrameId == use.Id && e.Receipt.ActionId == fact.ActionId && e.Receipt.Source == fact.Source && e.Receipt.ProtectedSeat == fact.PreviousTargetSeat && e.Receipt.OriginalTargets.SequenceEqual(before) && IsValidSoleTargetProtectionPaidFact(e.Receipt) && _coreCompletedCategoryUseProofOperations.ActualHandGainMutationBinding(e.Receipt.ProgramFrameId, fact.Source.SkillId, fact.Source.OwnerSeat, SkillProgramTriggerWindow.CardUseTargetsFinalized, SkillProgramEffectOp.ProtectOtherSoleCardTarget)) == 1;
    }
}
