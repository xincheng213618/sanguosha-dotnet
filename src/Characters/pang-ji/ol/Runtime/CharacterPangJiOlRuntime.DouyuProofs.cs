#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    internal bool ValidDouyuContest(ResolutionFrame parent)
    {
        if (parent.DouyuContest is not { } r || r.ParentFrameId != parent.Id || r.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || r.SequenceBefore < 0 || r.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.SequenceAfter < 0 || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Paid && (!r.Ready || r.SequenceAfter < r.SequenceBefore) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.SourceSeat) || r.OriginalCards.Count is < 2 or > 4 || r.OriginalCards[0].Seat != r.SourceSeat || r.OriginalCards.Select(c => c.Seat).Distinct().Count() != r.OriginalCards.Count || r.OriginalCards.Select(c => c.CardId).Distinct().Count() != r.OriginalCards.Count || r.OriginalCards.Any(c => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(c.Seat) || c.OriginalRank is < 1 or > 13 || _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).Kind != c.PrintedKind) || r.OriginalCards is not System.Collections.IList { IsReadOnly: true } || r.Assignments is not System.Collections.IList { IsReadOnly: true } || r.Opportunities is not System.Collections.IList { IsReadOnly: true } || r.Swaps is not System.Collections.IList { IsReadOnly: true } || !r.Assignments.Select(a => a.Seat).SequenceEqual(r.OriginalCards.Select(c => c.Seat)) || r.Opportunities.Select(o => o.Candidate.OwnerSeat).Distinct().Count() != r.Opportunities.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuContestStartedEvent>().Count(e => e == new DouyuContestStartedEvent(parent.Id, r.Simultaneous, r.SourceSeat, r.OriginalCards.Count, r.ActualTurn, r.TurnOwner, r.SequenceBefore)) != 1)
            return false;
        if (parent is PindianFrame p)
        {
            if (r.Simultaneous || r.OriginalCards.Count != 2 || p.SourceSeat != r.SourceSeat || p.OpponentSeat != r.OriginalCards[1].Seat)
                return false;
            if (p.WuLuKangContest is not null)
            {
                if (r.OriginalCards.Any(c => !_characterLuKangWumiaoOperations.IsWuLuKangOriginalInput(p, c.Seat, c.CardId, c.PaymentFrom, c.Top, c.ReservationSequence)))
                    return false;
            }
            else if (p.SourceCardId != r.OriginalCards[0].CardId || r.OriginalCards[0].PaymentFrom != (p.OwnedPileSource?.Pile.Location ?? (p.SourceUsesDrawPileTop ? CardLocation.Processing : CardLocation.Hand(p.SourceSeat))) || p.SourceUsesDrawPileTop != r.OriginalCards[0].Top || r.OriginalCards[1].PaymentFrom != (r.OriginalCards[1].Top ? CardLocation.DrawPile : CardLocation.Hand(p.OpponentSeat!.Value)))
                return false;
        }
        else if (parent is ProgramSkillFrame { SimultaneousPindian: { } simultaneous } f)
        {
            if (!r.Simultaneous || r.SourceSeat != f.OwnerSeat || !r.OriginalCards.Skip(1).Select(c => c.Seat).SequenceEqual(simultaneous.Opponents))
                return false;
            var actualSource = simultaneous.RandomSelection?.Receipts.SingleOrDefault(paid => paid.TargetSeat == f.OwnerSeat)?.CardId ?? simultaneous.OriginalSourceCardId;
            if (f.WuLuKangContest is not null ? r.OriginalCards.Any(c => !_characterLuKangWumiaoOperations.IsWuLuKangOriginalInput(f, c.Seat, c.CardId, c.PaymentFrom, c.Top, c.ReservationSequence)) : actualSource != r.OriginalCards[0].CardId || r.OriginalCards.Any(c => c.PaymentFrom != (c.Top ? CardLocation.Processing : CardLocation.Hand(c.Seat)) || c.Top && !simultaneous.TopReceipts.Any(t => t.Seat == c.Seat && t.CardId == c.CardId && t.ReservationMovementSequence == c.ReservationSequence)))
                return false;
        }
        else
            return false;
        for (var i = 0; i < r.OriginalCards.Count; i++)
        {
            var original = r.OriginalCards[i];
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<DouyuOriginalCardCommittedEvent>().Count(e => e.ContestId == parent.Id && e.Index == i && e.Card == original) != 1 || !r.Paid && _cardZonesOperations._cardZones.GetLocation(original.CardId) != original.PaymentFrom)
                return false;
        }

        for (var i = 0; i < r.Opportunities.Count; i++)
        {
            var opportunity = r.Opportunities[i];
            var c = opportunity.Candidate;
            if (opportunity.Opponents is not System.Collections.IList { IsReadOnly: true } || c.OccurrenceIndex != 0 || c.BindingId != "pre-reveal" || !r.OriginalCards.Any(original => original.Seat == c.OwnerSeat) || !opportunity.Opponents.SequenceEqual(c.OwnerSeat == r.SourceSeat ? r.OriginalCards.Skip(1).Select(original => original.Seat) : [r.SourceSeat]) || _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program is not { } program || program.GameplayHash != c.GameplayHash || program.Triggers.SingleOrDefault(t => t.Id == c.BindingId)is not { Window: SkillProgramTriggerWindow.PindianBeforeReveal } || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuCandidateCommittedEvent>().Count(e => e.ContestId == parent.Id && e.Ordinal == i && e.Source == DouyuSource(c) && e.GameplayHash == c.GameplayHash) != 1)
                return false;
        }

        var assignments = r.OriginalCards.ToDictionary(c => c.Seat, c => c.CardId);
        if (r.Swaps.Select(s => s.Source.OwnerSeat).Distinct().Count() != r.Swaps.Count)
            return false;
        for (var i = 0; i < r.Swaps.Count; i++)
        {
            var swap = r.Swaps[i];
            if (swap.Ordinal != i || !r.Opportunities.Any(o => DouyuSource(o.Candidate) == swap.Source && o.Candidate.GameplayHash == swap.GameplayHash && o.Opponents.Contains(swap.OpponentSeat)) || assignments[swap.Source.OwnerSeat] != swap.OwnerCardBefore || assignments[swap.OpponentSeat] != swap.OpponentCardBefore || swap.OwnerCardAfter != swap.OpponentCardBefore || swap.OpponentCardAfter != swap.OwnerCardBefore || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuSwapCommittedEvent>().Count(e => e == new DouyuSwapCommittedEvent(parent.Id, swap.WindowId, swap.FrameId, i, swap.Source, swap.GameplayHash, swap.OpponentSeat, swap.OwnerCardBefore, swap.OpponentCardBefore, swap.OwnerCardAfter, swap.OpponentCardAfter)) != 1)
                return false;
            assignments[swap.Source.OwnerSeat] = swap.OwnerCardAfter;
            assignments[swap.OpponentSeat] = swap.OpponentCardAfter;
        }

        for (var i = 0; i < r.Assignments.Count; i++)
        {
            var a = r.Assignments[i];
            if (assignments[a.Seat] != a.CardId || a.Rank is < 1 or > 13 || r.Ready && _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuAssignmentFinalizedEvent>().Count(e => e == new DouyuAssignmentFinalizedEvent(parent.Id, i, a.Seat, a.CardId, a.Rank)) != 1)
                return false;
        }

        return !r.Paid || r.OriginalCards.All(c => ValidDouyuMaterialPayment(parent.Id, c));
    }

    internal bool ValidDouyuMaterialPayment(long contestId, DouyuOriginalCard card)
    {
        var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuNativePaymentEvent>().Where(e => e.ContestId == contestId && e.OriginalSeat == card.Seat).ToArray();
        if (facts is not [var paid] || paid.CardId != card.CardId || paid.PrintedKind != card.PrintedKind || paid.From != card.PaymentFrom || paid.AlreadyReserved != (card.PaymentFrom == CardLocation.Processing))
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == paid.MovementSequence && m.CardId == card.CardId && m.CardKind == card.PrintedKind && m.To == CardLocation.Processing && (paid.AlreadyReserved ? card.ReservationSequence == m.Sequence && m.From == CardLocation.DrawPile && (card.Top && m.Reason.Value is "pindian.reserve-top" or "skill.pindian.reserve-top" || m.Reason.Value == "skill-program.wu-lu-kang.shenduan.reserve" && _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == contestId)is { } parent && _characterLuKangWumiaoOperations.IsWuLuKangOriginalInput(parent, card.Seat, card.CardId, card.PaymentFrom, card.Top, card.ReservationSequence)) : m.From == card.PaymentFrom && m.Reason == CardMoveReasons.PindianReveal)) == 1;
    }

    internal bool IsDouyuPairAssignment(PindianFrame pair, PindianResult result) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == pair.Id)is PindianFrame { DouyuContest: { Paid: true, Simultaneous: false } r } active && ValidDouyuContest(active) && result.SourceSeat == r.SourceSeat && result.OpponentSeat == r.OriginalCards[1].Seat && result.SourceCardId == r.Assignments[0].CardId && result.OpponentCardId == r.Assignments[1].CardId && result.SourceRank is >= 1 and <= 13 && result.OpponentRank is >= 1 and <= 13;
    internal IReadOnlyList<PindianRevealedCard> GetDouyuOriginalSimultaneousCards(ProgramSkillFrame f) => f.DouyuContest is { Simultaneous: true } r ? Array.AsReadOnly(r.OriginalCards.Select(c => new PindianRevealedCard(c.Seat, c.CardId, c.OriginalRank, c.OriginalRank)).ToArray()) : f.SimultaneousPindian!.Cards;
    internal bool IsDouyuHistoricalPairAssignment(long contestId, PindianResult result, int originalSourceCardId)
    {
        var originals = _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuOriginalCardCommittedEvent>().Where(e => e.ContestId == contestId).OrderBy(e => e.Index).ToArray();
        if (originals is not [var first, var second] || first.Index != 0 || second.Index != 1 || first.Card.CardId != originalSourceCardId || first.Card.Seat != result.SourceSeat || second.Card.Seat != result.OpponentSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuContestStartedEvent>().Count(e => e.ContestId == contestId && !e.Simultaneous && e.SourceSeat == result.SourceSeat && e.ParticipantCount == 2) != 1 || originals.Any(e => !ValidDouyuMaterialPayment(contestId, e.Card)) || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuResultRecordedEvent>().Count(e => e.ContestId == contestId && e.ComparisonIndex == 0 && e.Result == result) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == contestId && e.Result == result) != 1)
            return false;
        var ids = originals.ToDictionary(e => e.Card.Seat, e => e.Card.CardId);
        var swaps = _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuSwapCommittedEvent>().Where(e => e.ContestId == contestId).OrderBy(e => e.Ordinal).ToArray();
        if (swaps.Select(s => s.Source.OwnerSeat).Distinct().Count() != swaps.Length)
            return false;
        for (var i = 0; i < swaps.Length; i++)
        {
            var swap = swaps[i];
            if (swap.Ordinal != i || !ids.ContainsKey(swap.Source.OwnerSeat) || !ids.ContainsKey(swap.OpponentSeat) || swap.Source.OwnerSeat == swap.OpponentSeat || ids[swap.Source.OwnerSeat] != swap.OwnerCardBefore || ids[swap.OpponentSeat] != swap.OpponentCardBefore || swap.OwnerCardAfter != swap.OpponentCardBefore || swap.OpponentCardAfter != swap.OwnerCardBefore || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuCandidateCommittedEvent>().Count(e => e.ContestId == contestId && e.Source == swap.Source && e.GameplayHash == swap.GameplayHash) != 1)
                return false;
            ids[swap.Source.OwnerSeat] = swap.OwnerCardAfter;
            ids[swap.OpponentSeat] = swap.OpponentCardAfter;
        }

        return ids[result.SourceSeat] == result.SourceCardId && ids[result.OpponentSeat] == result.OpponentCardId && _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuAssignmentFinalizedEvent>().Count(e => e.ContestId == contestId && e.Seat == result.SourceSeat && e.CardId == result.SourceCardId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuAssignmentFinalizedEvent>().Count(e => e.ContestId == contestId && e.Seat == result.OpponentSeat && e.CardId == result.OpponentCardId) == 1;
    }

    [ProgramIssuedTrigger(270)]
    internal bool IsIssuedDouyuCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Douyu is not { } dc || context.OwnerSeat != candidate.OwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not DouyuWindowFrame w || w.ParentFrameId != dc.ContestId || w.Stage != dc.Stage || w.ComparisonIndex != dc.ComparisonIndex || w.CandidateIndex < 0 || w.CandidateIndex >= w.Entries.Count || w.Entries[w.CandidateIndex].Candidate != candidate || w.Entries[w.CandidateIndex].SwapFrameId != dc.SwapFrameId || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not { DouyuContest: { } r } parent || !ValidDouyuContest(parent) || context.SourceSeat != r.SourceSeat || context.TargetSeat != candidate.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program?.GameplayHash != candidate.GameplayHash)
            return false;
        if (w.Stage == DouyuWindowStage.BeforeReveal)
            return context.Window == SkillProgramTriggerWindow.PindianBeforeReveal && candidate.BindingId == "pre-reveal" && dc.SwapFrameId is null && !r.Ready && !r.Paid && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && !r.Swaps.Any(s => s.Source.OwnerSeat == candidate.OwnerSeat) && r.Opportunities.Any(o => o.Candidate == candidate);
        if (context.Window != SkillProgramTriggerWindow.PindianResultDetermined || candidate.BindingId != "swapped-win" || !r.Paid || dc.SwapFrameId is not { } id || r.Swaps.SingleOrDefault(s => s.FrameId == id)is not { } swap || swap.Source.OwnerSeat != candidate.OwnerSeat || swap.Source.SkillId != candidate.SkillId || swap.Source.SkillInstanceId != candidate.SkillInstanceId || swap.GameplayHash != candidate.GameplayHash || candidate.OccurrenceIndex != swap.Ordinal || _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuRewardClosedEvent>().Any(e => e.ContestId == parent.Id && e.SwapFrameId == id))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<DouyuResultRecordedEvent>().Count(e => e.ContestId == parent.Id && e.ComparisonIndex == dc.ComparisonIndex && DouyuSwappedPair(r, swap, e.Result) && e.Result.WonBy(candidate.OwnerSeat)) == 1;
    }

    internal bool DouyuPindianChildRidesOn(ResolutionFrame child, ResolutionFrame beneath)
    {
        if (child is DouyuWindowFrame window)
            return window.ParentFrameId == beneath.Id && beneath.DouyuContest is not null && ValidDouyuContest(beneath);
        if (beneath is DouyuWindowFrame w && child is ProgramSkillFrame f && f.WindowContext is { Douyu: { } dc } c)
            return c.ParentFrameId == w.Id && dc.ContestId == w.ParentFrameId && dc.Stage == w.Stage && dc.ComparisonIndex == w.ComparisonIndex && w.CandidateIndex >= 0 && w.CandidateIndex < w.Entries.Count && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Entries[w.CandidateIndex].Candidate) && dc.SwapFrameId == w.Entries[w.CandidateIndex].SwapFrameId;
        return false;
    }

    internal bool IsDouyuPairSubtree(PindianFrame pair)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == pair.Id);
        if (index < 0 || pair.DouyuContest is null || !ValidDouyuContest(pair) || index + 1 >= _resolutionStackOperations._resolutionStack.Count || !DouyuPindianChildRidesOn(_resolutionStackOperations._resolutionStack[index + 1], pair))
            return false;
        return _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => this.DouyuPindianChildRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || this.DouyuObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    }

    internal void AssertDouyuPairSubtreeProcessing(PindianFrame pair)
    {
        if (!IsDouyuPairSubtree(pair))
            throw new InvalidOperationException("Douyu cannot authorize an unrelated native processing subtree.");
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == pair.Id);
        var r = pair.DouyuContest!;
        var parentCards = pair.ParentProcessingCardIds ?? [];
        var expected = new HashSet<int>(parentCards);
        if (expected.Count != parentCards.Count || parentCards.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing) || r.OriginalCards.Any(c => expected.Contains(c.CardId)))
            throw new InvalidOperationException("Douyu cannot release or reuse the suspended parent's genuine processing materials.");
        foreach (var id in _corePindianPoliciesOperations.CurrentPindianProcessingIds(pair))
            expected.Add(id);
        for (var at = index + 2; at < _resolutionStackOperations._resolutionStack.Count; at++)
        {
            if (_resolutionStackOperations._resolutionStack[at] is CardUseFrame { Action: { Type: CardActionType.Use } action } use)
            {
                if (action.ActorSeat != use.SourceSeat || action.EffectiveKind != use.CardKind || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Count(e => e.ResolutionId == use.Id && e.SourceSeat == use.SourceSeat && e.CardId == use.CardId) != 1)
                    throw new InvalidOperationException("A nested Douyu observer must retain its exact declared native Use.");
                foreach (var cost in action.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing))
                {
                    var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
                    if (moved is null || moved.Sequence <= r.SequenceBefore || moved.CardKind != cost.CardKind || moved.From != cost.From || moved.To != CardLocation.Processing || !(_coreCardSupplyCompletionOperations.IsCardSupplyCompletionEntry(action, use.Id, _resolutionStackOperations._resolutionStack[at - 1].Id, cost, moved) || _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && moved.Reason == CardMoveReasons.EquipmentUse || use.CardKind == CardKind.IronChain && moved.Reason == CardMoveReasons.IronChainUse) || !expected.Add(cost.CardId))
                        throw new InvalidOperationException("A nested Douyu native Use cannot claim an unpaid, stale or ancestor processing entity.");
                }
            }
            else if (_resolutionStackOperations._resolutionStack[at] is ProgramCardTriggerWindowFrame { ResponseCompletion: not null } response && _coreResponseCompletionOperations.ValidResponseCompletionWindow(response))
            {
                foreach (var cost in response.Action.PhysicalCards.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing))
                {
                    var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId);
                    if (moved is null || moved.Sequence <= r.SequenceBefore || moved.CardKind != cost.CardKind || moved.From != cost.From || moved.To != CardLocation.Processing || moved.Reason != CardMoveReasons.Respond || !expected.Add(cost.CardId))
                        throw new InvalidOperationException("A nested Douyu response lost its exact native payment entity.");
                }
            }
            // A completed ordinary Program Judgment binds only its actual final
            // result. While still active, judgments hold their card in Judgment,
            // not Processing; no arbitrary judgment-frame ID is admitted here.
            else if (_resolutionStackOperations._resolutionStack[at] is ProgramSkillFrame program)
            {
                foreach (var bound in program.CardSetBindings)
                    for (var cardIndex = 0; cardIndex < bound.CardIds.Count; cardIndex++)
                    {
                        var id = bound.CardIds[cardIndex];
                        if (_cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Processing || expected.Contains(id))
                            continue;
                        var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == id);
                        if (bound.SourceLocations.Count != bound.CardIds.Count || bound.SourceLocations[cardIndex] != CardLocation.Processing || moved is null || moved.Sequence <= r.SequenceBefore || moved.From.Zone != CardZoneKind.Judgment || moved.To != CardLocation.Processing || moved.Reason.Value != "skill-program.judgment.result" || _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentResolvedEvent>().Count(e => e.ParentResolutionId == program.Id && e.TargetSeat == moved.From.OwnerSeat && e.CardId == id && e.CardKind == moved.CardKind && _coreProgramEventHistoryOperations.ProgramEventHistory<JudgmentRequestedEvent>().Any(start => start.ResolutionId == e.ResolutionId && start.ParentResolutionId == program.Id && start.TargetSeat == e.TargetSeat && start.Reason == e.Reason)) != 1)
                            throw new InvalidOperationException("A nested Douyu judgment result lacks its exact final-card handoff and native parent.");
                        expected.Add(id);
                    }
            }
        }

        if (!_cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(c => c.Id).Order().SequenceEqual(expected.Order()))
            throw new InvalidOperationException("Douyu lost the precise union of its parent, contest and paid native child processing cards.");
    }

    internal void AssertDouyuWindows()
    {
        foreach (var parent in _resolutionStackOperations._resolutionStack.Where(f => f.DouyuContest is not null))
            if (!ValidDouyuContest(parent))
                throw new InvalidOperationException("Douyu lost its frozen original selections, native payment or unique assignment permutation.");
        foreach (var w in _resolutionStackOperations._resolutionStack.OfType<DouyuWindowFrame>())
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == w.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.ParentFrameId || !Enum.IsDefined(w.Stage) || w.CandidateIndex < 0 || w.CandidateIndex > w.Entries.Count || w.Entries is not System.Collections.IList { IsReadOnly: true } || !DouyuPindianChildRidesOn(w, _resolutionStackOperations._resolutionStack[index - 1]) || w.Stage == DouyuWindowStage.BeforeReveal && w.ComparisonIndex != -1 || w.Stage == DouyuWindowStage.WinDraw && w.ComparisonIndex < 0)
                throw new InvalidOperationException("Douyu lost its exact private or result native window cursor.");
        }
    }
}
