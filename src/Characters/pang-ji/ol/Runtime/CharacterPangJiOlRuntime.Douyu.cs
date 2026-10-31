#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{
    internal const string DouyuDrawReason = "skill-program.pang-ji.douyu.draw";
    internal bool HasDouyuCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw);

    internal static CardConversionSource DouyuSource(ProgramTriggerCandidate c) => RuntimeSkills.RuntimeRules_pang_ji_ol_Douyu.DouyuSource(c);
    internal static CardConversionSource DouyuSource(ProgramSkillFrame f) => RuntimeSkills.RuntimeRules_pang_ji_ol_Douyu.DouyuSource(f);
    internal ResolutionFrame DouyuParent(long id) => _resolutionStackOperations._resolutionStack.Single(f => f.Id == id && (f is PindianFrame || f is ProgramSkillFrame { SimultaneousPindian: not null }));
    internal IReadOnlyList<DouyuOpportunity> CollectDouyuOpportunities(int source, IReadOnlyList<int> participants)
    {
        var result = new List<DouyuOpportunity>();
        foreach (var seat in participants)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                continue;
            var candidate = _coreNationalSkillsOperations.GetSkillBindingShard(_coreMatchParticipantsPlayersAccess._players[seat]).ProgramInstances.SelectMany(i => i.Program.Triggers.Where(t => t.Id == "pre-reveal" && t.Window == SkillProgramTriggerWindow.PindianBeforeReveal && t.Effects is [{ Op: SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw }]).Select(t => new ProgramTriggerCandidate(seat, i.SkillId, t.Id, i.SkillInstanceId, i.Program.GameplayHash, t.Priority))).Where(c => _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[seat], c.SkillId, c.SkillInstanceId)).OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            if (candidate is not null)
                result.Add(new(candidate) { Opponents = seat == source ? participants.Where(s => s != source).ToArray() : [source] });
        }

        return Array.AsReadOnly(result.OrderBy(o => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, o.Candidate.OwnerSeat)).ThenByDescending(o => o.Candidate.Priority).ToArray());
    }

    internal void CaptureDouyuContest(ResolutionFrame parent, bool simultaneous, int source, IReadOnlyList<DouyuOriginalCard> original, IReadOnlyList<DouyuOpportunity> opportunities)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent.Id || parent.DouyuContest is not null || original.Count is < 2 or > 4 || original.Select(c => c.Seat).Distinct().Count() != original.Count || original.Select(c => c.CardId).Distinct().Count() != original.Count || original[0].Seat != source || original.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != c.PaymentFrom))
            throw new InvalidOperationException("Douyu requires the exact complete unrevealed native selection and original payment zones.");
        var receipt = new DouyuContestReceipt(parent.Id, simultaneous, source, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
        {
            OriginalCards = original,
            Assignments = original.Select(c => new DouyuAssignment(c.Seat, c.CardId, c.OriginalRank)).ToArray(),
            Opportunities = opportunities
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { DouyuContest = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuContestStartedEvent(parent.Id, simultaneous, source, original.Count, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        for (var i = 0; i < original.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuOriginalCardCommittedEvent(parent.Id, i, original[i]));
        for (var i = 0; i < opportunities.Count; i++)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuCandidateCommittedEvent(parent.Id, i, DouyuSource(opportunities[i].Candidate), opportunities[i].Candidate.GameplayHash));
        _coreRuntimeOperations.PushRuntimeFrame(new DouyuWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent.Id, DouyuWindowStage.BeforeReveal, -1, opportunities.Select(o => new DouyuWindowEntry(o.Candidate, null)).ToArray()));
        ContinueDouyuWindow(_resolutionStackOperations._resolutionStack[^1].Id);
    }

    internal bool TryBeginDouyuPairPreReveal(PindianFrame frame, int opponentCardId, bool opponentTop)
    {
        if (frame.DouyuContest is { } existing)
        {
            if (!existing.Ready || existing.Simultaneous || existing.ParentFrameId != frame.Id || existing.OriginalCards[1].CardId != _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, frame.OpponentSeat!.Value, opponentCardId, opponentTop ? CardLocation.DrawPile : CardLocation.Hand(frame.OpponentSeat.Value), 0, opponentTop).CardId || existing.OriginalCards[1].Top != _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, frame.OpponentSeat!.Value, opponentCardId, opponentTop ? CardLocation.DrawPile : CardLocation.Hand(frame.OpponentSeat.Value), 0, opponentTop).Top)
                throw new InvalidOperationException("Douyu cannot replace its original native pair return.");
            return false;
        }

        if (!HasDouyuCapability || frame.OpponentSeat is not { } opponent)
            return false;
        var opportunities = CollectDouyuOpportunities(frame.SourceSeat, [frame.SourceSeat, opponent]);
        if (opportunities.Count == 0)
            return false;
        _corePindianOwnedPileSourcesOperations.Value.AssertOwnedPilePindianSource(frame);
        if (frame.Result is not null || frame.PindianStep != PindianStep.ChooseOpponentCard || frame.SourceCardId is not { } sourceId || !_corePindianPoliciesOperations.HasCurrentPindianSourceCard(frame))
            throw new InvalidOperationException("Douyu requires the native unconsumed pair's two real selections.");
        var from = frame.OwnedPileSource?.Pile.Location ?? (frame.SourceUsesDrawPileTop ? CardLocation.Processing : CardLocation.Hand(frame.SourceSeat));
        var sourceInput = _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, frame.SourceSeat, sourceId, from, 0, frame.SourceUsesDrawPileTop);
        from = sourceInput.From;
        sourceId = sourceInput.CardId;
        var sourceCard = _cardZonesOperations._cardZones.CardsAt(from).Single(c => c.Id == sourceId);
        var opponentFrom = opponentTop ? CardLocation.DrawPile : CardLocation.Hand(opponent);
        var opponentInput = _characterLuKangWumiaoOperations.WuLuKangActualInput(frame, opponent, opponentCardId, opponentFrom, 0, opponentTop);
        opponentFrom = opponentInput.From;
        opponentCardId = opponentInput.CardId;
        var opponentCard = _cardZonesOperations._cardZones.CardsAt(opponentFrom).Single(c => c.Id == opponentCardId);
        if (opponentTop && frame.WuLuKangContest is null && _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last().Id != opponentCardId)
            throw new InvalidOperationException("Douyu cannot choose a different private opponent top.");
        long? reserved = frame.WuLuKangContest is not null ? sourceInput.ReservationSequence : frame.SourceUsesDrawPileTop ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == sourceId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason.Value == "pindian.reserve-top").Sequence : null;
        CaptureDouyuContest(frame, false, frame.SourceSeat, [new(frame.SourceSeat, sourceId, sourceCard.Kind, from, frame.WuLuKangContest is not null ? sourceInput.Rank : _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholPindianRank(frame, _coreMatchParticipantsPlayersAccess._players[frame.SourceSeat], sourceCard, true, frame.SourceUsesDrawPileTop) ?? _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[frame.SourceSeat], sourceCard), sourceInput.Top, reserved), new(opponent, opponentCardId, opponentCard.Kind, opponentFrom, frame.WuLuKangContest is not null ? opponentInput.Rank : _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholPindianRank(frame, _coreMatchParticipantsPlayersAccess._players[opponent], opponentCard, false, opponentTop) ?? _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[opponent], opponentCard), opponentInput.Top, opponentInput.ReservationSequence)], opportunities);
        return true;
    }

    internal bool TryBeginDouyuSimultaneousPreReveal(ProgramSkillFrame frame)
    {
        if (frame.DouyuContest is { } existing)
        {
            if (!existing.Ready || !existing.Simultaneous || existing.ParentFrameId != frame.Id)
                throw new InvalidOperationException("Douyu cannot replace its native common-group return.");
            return false;
        }

        if (!HasDouyuCapability || frame.SimultaneousPindian is not { Revealed: false } r)
            return false;
        var participants = new[]
        {
            frame.OwnerSeat
        }.Concat(r.Opponents).ToArray();
        var opportunities = CollectDouyuOpportunities(frame.OwnerSeat, participants);
        if (opportunities.Count == 0)
            return false;
        if (r.SelectionIndex != r.Opponents.Count || !r.Cards.Select(c => c.Seat).SequenceEqual(participants))
            throw new InvalidOperationException("Douyu cannot start before every real simultaneous participant has committed.");
        var originals = r.Cards.Select(c => new DouyuOriginalCard(c.Seat, c.CardId, c.CardId <= 0 ? CardKind.Slash : _coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId).Kind, _coreSimultaneousPindianSelectionsOperations.SimultaneousOriginalLocation(r, c), c.OriginalRank, r.TopReceipts.Any(t => t.CardId == c.CardId), r.TopReceipts.SingleOrDefault(t => t.CardId == c.CardId)?.ReservationMovementSequence)).Select(c => _characterLuKangWumiaoOperations.WuLuKangDouyuInput(frame, c)).ToArray();
        CaptureDouyuContest(frame, true, frame.OwnerSeat, originals, opportunities);
        return true;
    }

    internal int DouyuAssignedRank(int seat, int cardId)
    {
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var actor = _coreMatchParticipantsPlayersAccess._players[seat];
        return card.Kind == CardKind.Alcohol && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(actor) ? 13 : _corePindianPoliciesOperations.EffectivePindianRank(actor, card);
    }

    internal void FinalizeDouyuAssignments(DouyuWindowFrame window)
    {
        var parent = DouyuParent(window.ParentFrameId);
        var r = parent.DouyuContest!;
        if (r.Ready || r.Paid || window.Stage != DouyuWindowStage.BeforeReveal || window.CandidateIndex != window.Entries.Count)
            throw new InvalidOperationException("Douyu must finish its unique private opportunity queue before native reveal.");
        r = r with
        {
            Ready = true,
            Assignments = r.Assignments.Select(a => a with { Rank = DouyuAssignedRank(a.Seat, a.CardId) }).ToArray()
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { DouyuContest = r });
        for (var i = 0; i < r.Assignments.Count; i++)
        {
            var a = r.Assignments[i];
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuAssignmentFinalizedEvent(parent.Id, i, a.Seat, a.CardId, a.Rank));
        }
    }

    internal void CommitDouyuNativePayment(ResolutionFrame parent)
    {
        var r = parent.DouyuContest!;
        if (!r.Ready || r.Paid)
            throw new InvalidOperationException("Douyu native materials can be committed only once after the private queue.");
        foreach (var original in r.OriginalCards)
        {
            if (_cardZonesOperations._cardZones.GetLocation(original.CardId) != CardLocation.Processing)
                throw new InvalidOperationException("Douyu cannot invent an unpaid native Pindian entity.");
            var reserved = original.PaymentFrom == CardLocation.Processing;
            var movement = reserved ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence == original.ReservationSequence && m.CardId == original.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing) : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > r.SequenceBefore && m.CardId == original.CardId && m.CardKind == original.PrintedKind && m.From == original.PaymentFrom && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.PindianReveal);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuNativePaymentEvent(parent.Id, original.Seat, original.CardId, original.PrintedKind, original.PaymentFrom, movement.Sequence, reserved));
        }

        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { DouyuContest = r with { Paid = true, SequenceAfter = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
    }

    internal PindianResult ApplyDouyuPairResult(PindianFrame frame, PindianResult native)
    {
        if (frame.DouyuContest is null)
            return native;
        var r = DouyuParent(frame.Id).DouyuContest!;
        if (r.Simultaneous || native.SourceSeat != r.SourceSeat || native.SourceCardId != r.OriginalCards[0].CardId || native.OpponentSeat != r.OriginalCards[1].Seat || native.OpponentCardId != r.OriginalCards[1].CardId)
            throw new InvalidOperationException("Douyu must consume the original pair's native payment rather than another contest.");
        CommitDouyuNativePayment(DouyuParent(frame.Id));
        var source = r.Assignments[0];
        var opponent = r.Assignments[1];
        return native with
        {
            SourceCardId = source.CardId,
            OpponentCardId = opponent.CardId,
            SourceRank = source.Rank,
            OpponentRank = opponent.Rank
        };
    }

    internal IReadOnlyList<PindianRevealedCard> ApplyDouyuSimultaneousCards(ProgramSkillFrame frame, IReadOnlyList<PindianRevealedCard> original)
    {
        if (frame.DouyuContest is null)
            return original;
        var r = DouyuParent(frame.Id).DouyuContest!;
        if (!r.Simultaneous || !original.Select(c => (c.Seat, c.CardId, c.OriginalRank)).SequenceEqual(r.OriginalCards.Select(c => (c.Seat, c.CardId, c.OriginalRank))))
            throw new InvalidOperationException("Douyu cannot rebind another simultaneous group's material ledger.");
        CommitDouyuNativePayment(DouyuParent(frame.Id));
        return Array.AsReadOnly(r.Assignments.Select(a => new PindianRevealedCard(a.Seat, a.CardId, a.Rank, a.Rank)).ToArray());
    }

    internal ProgramSkillWindowContext DouyuContext(DouyuWindowFrame w, ProgramTriggerCandidate c) => new(w.Stage == DouyuWindowStage.BeforeReveal ? SkillProgramTriggerWindow.PindianBeforeReveal : SkillProgramTriggerWindow.PindianResultDetermined, w.Id, c.OwnerSeat, SourceSeat: DouyuParent(w.ParentFrameId).DouyuContest!.SourceSeat, TargetSeat: c.OwnerSeat, OccurrenceIndex: c.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]))
    {
        Douyu = new(w.ParentFrameId, w.Stage, w.ComparisonIndex, w.Entries[w.CandidateIndex].SwapFrameId)
    };
    internal void SkipDouyuCandidate(DouyuWindowFrame window, ProgramTriggerCandidate candidate)
    {
        if (window.CandidateIndex >= window.Entries.Count || window.Entries[window.CandidateIndex].Candidate != candidate)
            throw new InvalidOperationException("Douyu cannot skip a different private or issued result candidate.");
        if (window.Stage == DouyuWindowStage.WinDraw)
        {
            var swap = window.Entries[window.CandidateIndex].SwapFrameId ?? throw new InvalidOperationException("A Douyu win candidate lost its original swap.");
            if (_coreProgramEventHistoryOperations.Value.ProgramEventHistory<DouyuRewardClosedEvent>().Any(e => e.ContestId == window.ParentFrameId && e.SwapFrameId == swap))
                throw new InvalidOperationException("Douyu cannot close the same issued win twice.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuRewardClosedEvent(window.ParentFrameId, window.ComparisonIndex, swap, false));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(window with { CandidateIndex = window.CandidateIndex + 1 });
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 10, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(DouyuWindowFrame), 280)]
    internal void ContinueDouyuWindow(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DouyuWindowFrame w || w.Id != id)
            return;
        AssertDouyuWindows();
        ProgramTriggerWindowDriver.Run(_kernelCharacterPangJiOlOperations.CreateDouyuTriggerWindowHost());
    }

    internal void CloseDouyuWindow(DouyuWindowFrame w)
    {
        AssertDouyuWindows();
        if (w.Stage == DouyuWindowStage.BeforeReveal)
            FinalizeDouyuAssignments(w);
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.DouyuWindow);
        var parent = DouyuParent(w.ParentFrameId);
        if (w.Stage == DouyuWindowStage.BeforeReveal)
        {
            if (parent is PindianFrame pair)
            {
                var original = pair.DouyuContest!.OriginalCards[1];
                _corePindianOperations.RevealPindian(pair, original.CardId, original.Top);
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

    [ProgramWindowReturn(SkillProgramTriggerWindow.PindianBeforeReveal, SkillProgramTriggerWindow.PindianResultDetermined)]
    internal void ReturnDouyuBinding(ProgramSkillFrame frame, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DouyuWindowFrame w || frame.WindowContext is not { Douyu: { } c } context || w.Id != context.ParentFrameId || w.ParentFrameId != c.ContestId || w.CandidateIndex >= w.Entries.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, w.Entries[w.CandidateIndex].Candidate))
            throw new InvalidOperationException("Douyu lost its exact native private/result window return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
        ContinueDouyuWindow(w.Id);
    }

    SkillProgramStepOutcome IDouyuHost.Resolve(ProgramSkillFrame frame) => ResolveDouyu(frame);
    internal SkillProgramStepOutcome ResolveDouyu(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.Douyu is not null || f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { Douyu: { } context } c || !IsIssuedDouyuCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c))
            throw new InvalidOperationException("Douyu requires its exact native committed-selection or issued swapped-win binding.");
        var r = DouyuParent(context.ContestId).DouyuContest!;
        var issued = context.SwapFrameId is { } swapId ? r.Swaps.Single(s => s.FrameId == swapId).Source : DouyuSource(f);
        var receipt = new DouyuReceipt(DouyuSource(f), issued, f.GameplayHash, context.ContestId, c.ParentFrameId, context.ComparisonIndex, context.SwapFrameId, context.Stage == DouyuWindowStage.BeforeReveal ? DouyuStage.OpponentChoice : DouyuStage.DrawWaiting);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Douyu = receipt });
        if (context.Stage == DouyuWindowStage.BeforeReveal)
        {
            PublishDouyu(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        return BeginDouyuWinDraw(f);
    }

    internal IReadOnlyList<PromptChoice> DouyuChoices(ProgramSkillFrame f)
    {
        if (f.Douyu is not { Stage: DouyuStage.OpponentChoice } r)
            return[];
        var contest = DouyuParent(r.ContestId).DouyuContest!;
        var opportunity = contest.Opportunities.Single(o => o.Candidate.OwnerSeat == f.OwnerSeat && o.Candidate.SkillInstanceId == f.SkillInstanceId);
        return Array.AsReadOnly(opportunity.Opponents.Where(s => _coreMatchParticipantsPlayersAccess._players[s].IsAlive).Select(seat => PromptChoiceProjection.Freeze(new PromptChoice(new($"douyu.{(f.Id)}.swap.{(seat)}"), "与" + _coreMatchParticipantsPlayersAccess._players[seat].Name + "交换尚未亮出的拼点牌", [], [seat], new Dictionary<string, string> { ["program-action"] = "douyu", ["branch"] = "swap", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture) }))).ToArray());
    }

    internal void PublishDouyu(ProgramSkillFrame f)
    {
        var choices = DouyuChoices(f);
        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f) || choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { Douyu = null });
            _coreSkillProgramsOperations.FinishProgramSkill(f, false);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "斗誉：选择真实拼点对手；双方拼点牌尚未公开。", [], choices.SelectMany(c => c.Targets).ToArray(), f.OwnerSeat)
        {
            IsPrivate = true,
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("douyu")]
    internal void AnswerDouyu(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Douyu: { Stage: DouyuStage.OpponentChoice } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !DouyuChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(f))
            throw new InvalidOperationException("Douyu requires its actual owner's private published opponent choice.");
        var parent = DouyuParent(r.ContestId);
        var contest = parent.DouyuContest!;
        var opponent = choice.Targets.Single();
        var ownerCard = contest.Assignments.Single(a => a.Seat == f.OwnerSeat);
        var otherCard = contest.Assignments.Single(a => a.Seat == opponent);
        var swap = new DouyuSwap(f.Id, r.WindowId, contest.Swaps.Count, r.Source, r.GameplayHash, opponent, ownerCard.CardId, otherCard.CardId, otherCard.CardId, ownerCard.CardId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { DouyuContest = contest with { Swaps = contest.Swaps.Append(swap).ToArray(), Assignments = contest.Assignments.Select(a => a.Seat == f.OwnerSeat ? a with { CardId = otherCard.CardId } : a.Seat == opponent ? a with { CardId = ownerCard.CardId } : a).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DouyuSwapCommittedEvent(parent.Id, r.WindowId, f.Id, swap.Ordinal, r.Source, r.GameplayHash, opponent, swap.OwnerCardBefore, swap.OpponentCardBefore, swap.OwnerCardAfter, swap.OpponentCardAfter));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, Douyu = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveDouyuPreRevealSwapAndWinDraw)]
    internal PromptChoice ChooseDouyuAi(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
