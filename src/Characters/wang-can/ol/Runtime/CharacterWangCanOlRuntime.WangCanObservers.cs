#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanOlRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 500)]
    internal bool CanOfferWangCan(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w)
    {
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveSanwenActualGainDuplicates))
            return _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && w.Sanwen is { } q && q.OwnerSeat == c.OwnerSeat && w.MovementBatch is { } batch && ValidSanwenProof(q, batch) && SanwenPayable(q).Count > 0;
        if (t.Effects.Any(e => e.Op == SkillProgramEffectOp.ViewDenglouTopAndUseBasics))
            return c.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]).Count == 0;
        return true;
    }

    internal bool WangCanSegmentMatches(ProgramSkillFrame f, WangCanReceipt r) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanSegmentPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Stage == r.Stage && e.ActualTurn == r.ActualTurn && e.Before == r.Before && e.After == r.After && e.ActualCount == r.ActualCount) == 1;
    internal void RequireIssuedWangCan(ProgramSkillFrame f)
    {
        if (f.WangCan is not { } r)
            return;
        if (r.Source != WangCanSource(f) || r.GameplayHash != f.GameplayHash || r.Instruction != f.InstructionIndex || r.WindowId != f.WindowContext?.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.Before > r.After || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.WindowId == r.WindowId && e.ActualTurn == r.ActualTurn && e.Operation == _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op) != 1 || r.Entities.Distinct().Count() != r.Entities.Count || r.Remaining.Distinct().Count() != r.Remaining.Count)
            throw new InvalidOperationException("Wang Can lost its exact original source, window, instruction or physical receipt.");
        if (r.Sanwen is { } q && (f.WindowContext?.Sanwen != q || f.WindowContext.MovementBatch is not { } batch || !ValidSanwenProof(q, batch)))
            throw new InvalidOperationException("Sanwen lacks its original native gain batch and pre-gain witnesses.");
        if (r.Stage is WangCanStage.SanwenDiscard or WangCanStage.SanwenDraw or WangCanStage.QiaiGift or WangCanStage.DenglouViewChildren or WangCanStage.DenglouGain or WangCanStage.DenglouDiscard or WangCanStage.DenglouCancel)
        {
            if (!WangCanSegmentMatches(f, r) || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null })
                throw new InvalidOperationException("Wang Can cannot borrow another instruction's paid native movement.");
            var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == WangCanReason(r.Stage)).ToArray();
            if (moves.Length != r.ActualCount || moves.Select(m => m.CardId).Distinct().Count() != moves.Length || r.Stage != WangCanStage.SanwenDraw && moves.Any(m => !r.Entities.Contains(m.CardId)))
                throw new InvalidOperationException("Wang Can native payment differs from its exact entity and movement invoice.");
            if (r.Stage == WangCanStage.SanwenDiscard && (r.Sanwen is not { } gain || moves.Any(m => m.From != CardLocation.Hand(f.OwnerSeat) || m.To != CardLocation.DiscardPile || !gain.Incoming.Any(i => i.CardId == m.CardId))))
                throw new InvalidOperationException("Sanwen may discard only its actual matching newly gained hand entities.");
            if (r.Stage == WangCanStage.SanwenDraw && (moves.Any(m => m.From != CardLocation.DrawPile || m.To != CardLocation.Hand(f.OwnerSeat)) || r.ActualCount > r.Cursor || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanSegmentPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Stage == WangCanStage.SanwenDiscard && e.ActualCount * 2 == r.Cursor) != 1))
                throw new InvalidOperationException("Sanwen draw request must derive from exactly twice its own actual discard count.");
            if (r.Stage == WangCanStage.QiaiGift && (r.Cursor < 0 || r.Cursor >= r.Seats.Count || moves.Any(m => m.From.OwnerSeat != r.Seats[r.Cursor] || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || m.To != CardLocation.Hand(f.OwnerSeat)) || r.ActualCount != 1))
                throw new InvalidOperationException("Qiai payment lost its exact living payer's chosen HE entity.");
            if (r.Stage == WangCanStage.DenglouGain && moves.Any(m => m.From != r.Lease || m.To != CardLocation.Hand(f.OwnerSeat) || CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind) == SkillProgramCardCategory.Basic))
                throw new InvalidOperationException("Denglou first obtains only the actual nonbasic leased entities.");
            if (r.Stage is WangCanStage.DenglouDiscard or WangCanStage.DenglouCancel && moves.Any(m => m.From != r.Lease || m.To != CardLocation.DiscardPile))
                throw new InvalidOperationException("Denglou disposal cannot move another zone's cards.");
        }

        if (r.Lease is { } lease && (lease != WangCanLease(f.OwnerSeat) || r.Remaining.Any(id => !r.Entities.Contains(id))))
            throw new InvalidOperationException("Denglou cannot claim another reserve entity.");
        if (r.Lease is { } originalLease)
        {
            var top = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.LeaseBefore && m.Sequence <= r.LeaseAfter && m.Reason.Value == WangCanReason(WangCanStage.DenglouViewChildren)).ToArray();
            if (r.Entities.Count > 4 || r.LeaseBefore > r.LeaseAfter || r.LeaseAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !top.Select(m => m.CardId).SequenceEqual(r.Entities) || top.Any(m => m.From != CardLocation.DrawPile || m.To != originalLease) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanSegmentPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Stage == WangCanStage.DenglouViewChildren && e.Before == r.LeaseBefore && e.After == r.LeaseAfter && e.ActualCount == r.Entities.Count) != 1)
                throw new InvalidOperationException("Denglou lost its once-paid original exact private top lease.");
        }

        if (r.DyingFrameId is { } dyingId && _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)?.ResumeDyingFrameId != dyingId)
            throw new InvalidOperationException("Qiai lost its original accepted dying window.");
        if (r.Stage == WangCanStage.Completed && (r.Native is not { } returned || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanNativeReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == returned.UseId && e.ActionId == returned.ActionId) != 1 || _coreCardUseLifecycleOperations.LifecycleCardUse(returned.UseId)is not null))
            throw new InvalidOperationException("Denglou terminal continuation requires its unique actual native return.");
        if (r.Stage == WangCanStage.DenglouNative && (r.Native is not { } n || n.ProgramFrameId != f.Id || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<WangCanNativeIssuedEvent>().Count(e => e.FrameId == f.Id && e.UseId == n.UseId && e.ActionId == n.ActionId && e.Source == r.Source && e.GameplayHash == r.GameplayHash) != 1))
            throw new InvalidOperationException("Denglou native use is not uniquely issued.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 280)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 280)]
    internal bool CanContinueIssuedWangCan(ProgramSkillFrame f)
    {
        if (f.WangCan is not { } r)
            return false;
        RequireIssuedWangCan(f);
        return true;
    }
    internal bool WangCanFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.WangCan is not { } r || !CanContinueIssuedWangCan(root))
            return false;
        if (child is CardUseFrame use)
            return DenglouNativeRidesOn(root, use);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == WangCanReason(r.Stage));
        return false;
    }

    internal ProgramSkillFrame? WangCanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.WangCanFirstChild(r, c), (i) => this.WangCanObserverSuffix(i));
    internal bool WangCanObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || this.DenglouNativePaymentEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i, d) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i, d)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 350)]
    internal bool TryAdvanceWangCanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WangCanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
