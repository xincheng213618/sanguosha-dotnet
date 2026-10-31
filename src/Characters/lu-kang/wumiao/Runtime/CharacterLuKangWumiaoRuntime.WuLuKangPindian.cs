#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal const string WuLuKangDiscardReason = "skill-program.wu-lu-kang.shenduan.discard", WuLuKangReserveReason = "skill-program.wu-lu-kang.shenduan.reserve", WuLuKangBottomReason = "skill-program.wu-lu-kang.shenduan.bottom", WuLuKangDrawReason = "skill-program.wu-lu-kang.shenduan.draw";
    internal bool HasWuLuKangCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuKangShenduan) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveWuLuKangKegou) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveWuLuKangDixian);

    internal static CardConversionSource WuLuKangSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangPindian.WuLuKangSource(f);
    internal static CardConversionSource WuLuKangSource(ProgramTriggerCandidate c) => RuntimeSkills.RuntimeRules_lu_kang_wumiao_WuLuKangPindian.WuLuKangSource(c);
    internal bool CanWuLuKangDiscardForPindian(CharacterState owner) => owner.IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.PindianDiscardForMaximumDeck).Any(binding => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner.Seat, card, CardLocation.Equipment(owner.Seat), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(owner.Seat, binding.Source.SkillId, binding.Source.SkillInstanceId, card)));
    internal bool HasWuLuKangPindianInput(CharacterState owner) => _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 || CanWuLuKangDiscardForPindian(owner);
    internal ResolutionFrame WuLuKangParent(long id) => _resolutionStackOperations._resolutionStack.Single(f => f.Id == id && (f is PindianFrame || f is ProgramSkillFrame { SimultaneousPindian: not null }));
    internal IReadOnlyList<ProgramTriggerCandidate> WuLuKangSources(int? onlySeat = null) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && (onlySeat is null || p.Seat == onlySeat)).SelectMany(p => _coreNationalSkillsOperations.GetSkillBindingShard(p).ProgramInstances.SelectMany(i => i.Program.Triggers.Where(t => t.Window == SkillProgramTriggerWindow.WuLuKangPindian && t.Effects is [{ Op: SkillProgramEffectOp.ResolveWuLuKangShenduan }]).Select(t => new ProgramTriggerCandidate(p.Seat, i.SkillId, t.Id, i.SkillInstanceId, i.Program.GameplayHash, t.Priority)))).OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ToArray());
    internal int WuLuKangTurnOrdinal => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Count();

    internal void CaptureWuLuKangContest(ResolutionFrame parent, bool group, int source, IReadOnlyList<WuLuKangPindianInput> inputs)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || parent.WuLuKangContest is not null || inputs.Count is < 2 or > 4 || inputs[0].Seat != source || inputs.Select(c => c.Seat).Distinct().Count() != inputs.Count)
            throw new InvalidOperationException("Shenduan requires the complete native private commitments.");
        var committed = inputs.ToArray();
        for (var i = 0; i < committed.Length; i++)
        {
            var input = committed[i];
            if (input.Top && input.From == CardLocation.DrawPile)
            {
                var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(input.CardId);
                var sequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence + 1;
                committed[i] = input with
                {
                    From = CardLocation.Processing,
                    ReservationSequence = sequence
                };
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new(WuLuKangReserveReason), emitCardMovedEvents: false, disclosure: CardMovementDisclosure.FaceDown);
            }
        }

        inputs = Array.AsReadOnly(committed);
        var r = new WuLuKangContestReceipt(parent.Id, group, source, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, WuLuKangTurnOrdinal)
        {
            Original = inputs,
            Assignments = inputs.Select(c => new WuLuKangPindianAssignment(c.Seat, c.CardId, c.Rank)).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangContest = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangContestStartedEvent(parent.Id, group, source, inputs.Count, r.ActualTurn, r.TurnOwner, r.TurnOrdinal));
        var entries = inputs.SelectMany(c => WuLuKangSources(c.Seat).Select(s => new WuLuKangPindianWindowEntry(s, c.Seat))).ToArray();
        BeginWuLuKangWindow(parent.Id, WuLuKangPindianStage.BeforeSourceReplacement, -1, entries, null);
    }

    internal bool TryBeginWuLuKangPairBeforeReveal(PindianFrame f, int opponentCardId, bool top)
    {
        if (f.WuLuKangContest is { } existing)
        {
            if (!existing.SourcesReady || existing.Simultaneous || existing.Original[1].CardId != opponentCardId || existing.Original[1].Top != top)
                throw new InvalidOperationException("Shenduan cannot replace its original pair continuation.");
            return false;
        }

        if (!HasWuLuKangCapability || f.OpponentSeat is not { } other)
            return false;
        var sourceFrom = f.OwnedPileSource?.Pile.Location ?? (f.SourceUsesDrawPileTop ? CardLocation.Processing : CardLocation.Hand(f.SourceSeat));
        WuLuKangPindianInput Input(int seat, int id, CardLocation from, bool isTop)
        {
            if (id <= 0)
                return new(seat, id, CardKind.Slash, from, 0, false, null);
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(id);
            var rank = _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholPindianRank(f, _coreMatchParticipantsPlayersAccess._players[seat], card, seat == f.SourceSeat, isTop) ?? _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[seat], card);
            return new(seat, id, card.Kind, from, rank, isTop, from == CardLocation.Processing ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == id && m.To == CardLocation.Processing).Sequence : null);
        }

        CaptureWuLuKangContest(f, false, f.SourceSeat, [Input(f.SourceSeat, f.SourceCardId ?? 0, sourceFrom, f.SourceUsesDrawPileTop), Input(other, opponentCardId, top ? CardLocation.DrawPile : CardLocation.Hand(other), top)]);
        return true;
    }

    internal bool TryBeginWuLuKangGroupBeforeReveal(ProgramSkillFrame f)
    {
        if (f.WuLuKangContest is { } existing)
        {
            if (!existing.SourcesReady || !existing.Simultaneous)
                throw new InvalidOperationException("Shenduan cannot reenter an unfinished group selection.");
            return false;
        }

        if (!HasWuLuKangCapability || f.SimultaneousPindian is not { Revealed: false } r)
            return false;
        CaptureWuLuKangContest(f, true, f.OwnerSeat, r.Cards.Select(c => new WuLuKangPindianInput(c.Seat, c.CardId, c.CardId <= 0 ? CardKind.Slash : _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).Kind, _coreSimultaneousPindianSelectionsOperations.SimultaneousOriginalLocation(r, c), c.OriginalRank, r.TopReceipts.Any(t => t.CardId == c.CardId), r.TopReceipts.SingleOrDefault(t => t.CardId == c.CardId)?.ReservationMovementSequence)).ToArray());
        return true;
    }

    internal void BeginWuLuKangWindow(long parent, WuLuKangPindianStage stage, int comparison, IReadOnlyList<WuLuKangPindianWindowEntry> entries, PindianResult? result)
    {
        var w = new WuLuKangPindianFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent, stage, comparison, entries, result);
        _coreRuntimeOperations.PushRuntimeFrame(w);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangWindowOpenedEvent(w.Id, parent, stage, comparison, entries.Count));
        for (var i = 0; i < entries.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangWindowCandidateEvent(w.Id, i, WuLuKangSource(entries[i].Candidate), entries[i].Candidate.GameplayHash, entries[i].ParticipantSeat));
        ContinueWuLuKangWindow(w.Id);
    }

    internal bool TryBeginWuLuKangPostComparison(ResolutionFrame parent, PindianResult result, int comparison)
    {
        if (!HasWuLuKangCapability)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangComparisonSettledEvent>().Any(e => e.ContestId == parent.Id && e.ComparisonIndex == comparison))
            return false;
        if (parent.WuLuKangContest is null)
            throw new InvalidOperationException("Shenduan result requires its original native selection receipt.");
        var r = parent.WuLuKangContest with
        {
            Revealed = true,
            Comparisons = parent.WuLuKangContest.Comparisons.Append(new WuLuKangSettledComparison(comparison, result)).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangContest = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangComparisonSettledEvent(parent.Id, comparison, result));
        BeginWuLuKangWindow(parent.Id, WuLuKangPindianStage.PostComparison, comparison, WuLuKangSources().Select(c => new WuLuKangPindianWindowEntry(c, c.OwnerSeat)).ToArray(), result);
        return true;
    }

    internal ProgramSkillWindowContext WuLuKangContext(WuLuKangPindianFrame w, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.WuLuKangPindian, w.Id, c.OwnerSeat, SourceSeat: WuLuKangParent(w.ParentFrameId).WuLuKangContest!.SourceSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]))
    {
        WuLuKangPindian = new(w.ParentFrameId, w.Stage, w.ComparisonIndex, w.Entries[w.CandidateIndex].ParticipantSeat)
    };
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 0, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(WuLuKangPindianFrame), 270)]
    internal void ContinueWuLuKangWindow(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is WuLuKangPindianFrame w && w.Id == id)
        {
            AssertWuLuKangWindows();
            ProgramTriggerWindowDriver.Run(_kernelCharacterLuKangWumiaoOperations.CreateWuLuKangWindowHost());
        }
    }

    internal void CloseWuLuKangWindow(WuLuKangPindianFrame w)
    {
        var parent = WuLuKangParent(w.ParentFrameId);
        var r = parent.WuLuKangContest!;
        if (w.Stage == WuLuKangPindianStage.BeforeSourceReplacement)
        {
            foreach (var input in r.Original.Where(c => c.From == CardLocation.Processing && r.Assignments.All(a => a.CardId != c.CardId)))
                if (_cardZonesOperations._cardZones.GetLocation(input.CardId) == CardLocation.Processing)
                    _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(input.CardId), CardLocation.Processing, CardLocation.DrawPile, new(WuLuKangReserveReason), emitCardMovedEvents: false, disclosure: CardMovementDisclosure.FaceDown);
            r = r with
            {
                SourcesReady = true,
                Assignments = r.Assignments.Select(a => a.CardId <= 0 ? a : a with { Rank = WuLuKangFinalInputRank(r, a) }).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent = parent with { WuLuKangContest = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangSourcesClosedEvent(parent.Id));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangWindowClosedEvent(w.Id, parent.Id, w.Stage, w.ComparisonIndex));
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.WuLuKangPindian);
        if (w.Stage == WuLuKangPindianStage.BeforeSourceReplacement && !r.Cancelled && (!WuLuKangInputsAvailable(parent) || r.Assignments.Any(a => a.CardId <= 0)))
        {
            MarkWuLuKangCancelled(parent.Id, "unpaid-source-window-exhausted");
            parent = WuLuKangParent(parent.Id);
            r = parent.WuLuKangContest!;
        }

        if (r.Cancelled)
        {
            if (WuLuKangHeldProcessing(parent).Count > 0)
            {
                var cleanup = w.Entries.FirstOrDefault() ?? throw new InvalidOperationException("A paid cancelled contest lost its original source candidate.");
                BeginWuLuKangWindow(parent.Id, WuLuKangPindianStage.GroupClose, -1, [cleanup], null);
                return;
            }

            CancelWuLuKangContest(parent);
            return;
        }

        if (w.Stage == WuLuKangPindianStage.BeforeSourceReplacement)
        {
            if (parent is PindianFrame pair)
            {
                var input = r.Original[1];
                _corePindianOperations.RevealPindian(pair, input.CardId, input.Top);
            }
            else
                _coreSimultaneousPindianSelectionsOperations.RevealSimultaneousCommittedGroup((ProgramSkillFrame)parent);
        }
        else if (parent is PindianFrame pair)
        {
            if (!_corePindianClaimsOperations.BeginPindianClaims(pair))
                _corePindianOperations.CompletePindian((PindianFrame)_resolutionStackOperations._resolutionStack[^1]);
        }
        else
            _coreRuntimeOperations.AdvanceRuntimeProgram(parent.Id);
    }

    [ProgramWindowReturn(SkillProgramTriggerWindow.WuLuKangPindian)]
    internal void ReturnWuLuKangBinding(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not WuLuKangPindianFrame w || f.WindowContext?.WuLuKangPindian is not { } c || f.WindowContext.ParentFrameId != w.Id || c.ContestId != w.ParentFrameId || c.Stage != w.Stage || c.ComparisonIndex != w.ComparisonIndex || w.CandidateIndex >= w.Entries.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Entries[w.CandidateIndex].Candidate))
            throw new InvalidOperationException("Shenduan returned to a different comparison or native participant.");
        _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
        ContinueWuLuKangWindow(w.Id);
    }

    internal bool IsIssuedWuLuKangCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.WuLuKangPindian is not { } c || context.Window != SkillProgramTriggerWindow.WuLuKangPindian || context.OwnerSeat != candidate.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not WuLuKangPindianFrame w || w.ParentFrameId != c.ContestId || w.Stage != c.Stage || w.ComparisonIndex != c.ComparisonIndex || w.CandidateIndex < 0 || w.CandidateIndex >= w.Entries.Count || w.Entries[w.CandidateIndex] != new WuLuKangPindianWindowEntry(candidate, c.ParticipantSeat) || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == c.ContestId)is not { WuLuKangContest: { } r } parent || !ValidWuLuKangContest(parent) || context.SourceSeat != r.SourceSeat || context.TargetSeat != candidate.OwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex || _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program is not { } p || p.GameplayHash != candidate.GameplayHash || !WuLuKangComposition.IsShenduan(p) || candidate.OccurrenceIndex != 0 || candidate.Priority != p.Triggers[0].Priority || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangWindowCandidateEvent>().Count(e => e == new WuLuKangWindowCandidateEvent(w.Id, w.CandidateIndex, WuLuKangSource(candidate), candidate.GameplayHash, c.ParticipantSeat)) != 1)
            return false;
        if (r.Cancelled && w.Stage != WuLuKangPindianStage.GroupClose)
            return false;
        if (w.Stage == WuLuKangPindianStage.GroupClose)
            return r.Cancelled && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangWindowClosedEvent>().Any(e => e.ContestId == parent.Id && e.Stage == WuLuKangPindianStage.BeforeSourceReplacement);
        return _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], candidate.SkillId, candidate.SkillInstanceId) && (w.Stage == WuLuKangPindianStage.PostComparison || c.ParticipantSeat == candidate.OwnerSeat && r.Original.Any(i => i.Seat == c.ParticipantSeat) && (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 0 || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(candidate.OwnerSeat, card, CardLocation.Equipment(candidate.OwnerSeat), OwnedCardMoveIntent.Discard) && !_coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, card))) && !_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangShenduanStartedEvent>().Any(e => e.ContestId == parent.Id && e.ParticipantSeat == c.ParticipantSeat && e.Stage == WuLuKangPindianStage.BeforeSourceReplacement));
    }

    internal WuLuKangPindianInput WuLuKangActualInput(ResolutionFrame parent, int seat, int id, CardLocation from, int rank, bool top)
    {
        if (parent.WuLuKangContest is not { SourcesReady: true } r)
            return new(seat, id, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind, from, rank, top, null);
        var a = r.Assignments.Single(i => i.Seat == seat);
        var replacement = r.Replacements.LastOrDefault(p => p.ParticipantSeat == seat && p.ReservedCardId == a.CardId);
        return replacement is null ? r.Original.Single(i => i.Seat == seat)with
        {
            Rank = a.Rank
        }

        : new(seat, a.CardId, _coreAdvancedLifecycleOperations.GetAdvancedCard(a.CardId).Kind, CardLocation.Processing, a.Rank, false, replacement.ReservationSequence);
    }

    internal int WuLuKangFinalInputRank(WuLuKangContestReceipt contest, WuLuKangPindianAssignment assignment)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[assignment.Seat];
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(assignment.CardId);
        var stockSource = contest.Original.Single(c => c.Seat == assignment.Seat).Top || contest.Replacements.Any(p => p.ParticipantSeat == assignment.Seat && p.ReservedCardId == assignment.CardId);
        return stockSource && card.Kind == CardKind.Alcohol && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(owner) ? 13 : _corePindianPoliciesOperations.EffectivePindianRank(owner, card);
    }

    internal DouyuOriginalCard WuLuKangDouyuInput(ResolutionFrame parent, DouyuOriginalCard original)
    {
        if (parent.WuLuKangContest is not { SourcesReady: true })
            return original;
        var input = WuLuKangActualInput(parent, original.Seat, original.CardId, original.PaymentFrom, original.OriginalRank, original.Top);
        return new(input.Seat, input.CardId, input.PrintedKind, input.From, input.Rank, input.Top, input.ReservationSequence);
    }

    internal bool IsWuLuKangOriginalInput(ResolutionFrame parent, int seat, int card, CardLocation from, bool top, long? sequence)
    {
        if (parent.WuLuKangContest is not { SourcesReady: true } r || !ValidWuLuKangContest(parent))
            return false;
        var a = r.Assignments.SingleOrDefault(i => i.Seat == seat);
        if (a is null || a.CardId != card)
            return false;
        var input = WuLuKangActualInput(parent, seat, card, from, a.Rank, top);
        return input.CardId == card && input.From == from && input.Top == top && input.ReservationSequence == sequence;
    }

    [ProgramViewContributor(ProgramViewContribution.ConcealedCardExclusion, 20)]
    internal bool IsWuLuKangPrivatePindianCard(int id) => _resolutionStackOperations._resolutionStack.Any(f => f.WuLuKangContest is { Revealed: false, Cancelled: false } r && (r.Replacements.Any(p => p.ReservedCardId == id) || r.Original.Any(p => p.CardId == id && p.From == CardLocation.Processing)));
    internal IReadOnlyList<int> WuLuKangHeldProcessing(ResolutionFrame parent) => parent.WuLuKangContest is not { } r ? [] : r.Original.Select(c => c.CardId).Concat(r.Replacements.Where(p => p.ReservedCardId is not null).Select(p => p.ReservedCardId!.Value)).Where(id => id > 0 && _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).Distinct().ToArray();
    internal void RevealWuLuKangInputs(ResolutionFrame parent)
    {
        if (parent.WuLuKangContest is not { Revealed: false } r)
            return;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangContest = r with { Revealed = true } });
        foreach (var a in r.Assignments)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangPindianInputRevealedEvent(parent.Id, a.Seat, a.CardId, a.Rank, r.Replacements.Any(p => p.ReservedCardId == a.CardId)));
    }

    internal void MarkWuLuKangCancelled(long contest, string reason)
    {
        var parent = WuLuKangParent(contest);
        var r = parent.WuLuKangContest!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangContest = r with { Cancelled = true, CancellationReason = reason } });
    }

    internal void CancelWuLuKangContest(ResolutionFrame parent)
    {
        var r = parent.WuLuKangContest!;
        if (!r.Cancelled || WuLuKangHeldProcessing(parent).Count != 0)
            throw new InvalidOperationException("Cancelled Pindian must finish its exact native material cleanup first.");
        var parentId = parent is PindianFrame pair ? pair.ParentFrameId : parent.Id;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangPindianCancelledEvent(parent.Id, parentId, r.Simultaneous, r.CancellationReason ?? "participant-unavailable"));
        if (parent is PindianFrame p)
        {
            _coreRuntimeOperations.PopResolutionFrame(p.Id, ResolutionFrameKind.Pindian);
            var owner = _coreProgramLifecycleOperations.GetActiveProgramFrame(p.ParentFrameId);
            if (owner.WuLuKangKegou is { } kegou)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(owner with { WuLuKangKegou = kegou with { PindianCancelled = true } });
                ContinueWuLuKangKegou(owner.Id);
            }
            else
            {
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(owner, "拼点参与者或尚未支付的拼点牌已不可用。");
            }
        }
        else
        {
            var group = (ProgramSkillFrame)parent;
            _coreRuntimeOperations.ReplaceRuntimeTop(group = group with { SimultaneousPindian = null, PendingMovementContinuation = null });
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(group, "同时拼点的参与者或尚未支付的拼点牌已不可用。");
        }
    }

    [ProgramIssuedTrigger(280, ProgramIssuedTriggerMatch.Decision)]
    internal bool? DecideIssuedTrigger280(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.WuLuKangPindian ? IsIssuedWuLuKangCandidate(candidate, context) : null;
}
