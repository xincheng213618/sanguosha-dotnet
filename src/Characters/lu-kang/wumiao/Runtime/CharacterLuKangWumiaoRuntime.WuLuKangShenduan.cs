#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    SkillProgramStepOutcome IWuLuKangHost.Shenduan(ProgramSkillFrame f) => BeginWuLuKangShenduan(f);
    SkillProgramStepOutcome IWuLuKangHost.Kegou(ProgramSkillFrame f) => BeginWuLuKangKegou(f);
    internal SkillProgramStepOutcome BeginWuLuKangShenduan(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.WuLuKangShenduan is not null || f.TriggerId is null || f.InstructionIndex != 1 || f.WindowContext is not { WuLuKangPindian: { } c } context || !IsIssuedWuLuKangCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context))
            throw new InvalidOperationException("Shenduan can start only in its exact private-source or settled-comparison window.");
        var parent = WuLuKangParent(c.ContestId);
        var w = (WuLuKangPindianFrame)_resolutionStackOperations._resolutionStack[^2];
        var recipients = w.Result is { } p ? new[]
        {
            (p.SourceSeat, p.SourceRank),
            (p.OpponentSeat, p.OpponentRank)
        }.Where(i => i.Item2 == 13).Select(i => i.Item1).Distinct().ToArray() : [];
        var r = new WuLuKangShenduanReceipt(WuLuKangSource(f), f.GameplayHash, context.ParentFrameId, c.ContestId, c.Stage, c.ComparisonIndex, c.ParticipantSeat)
        {
            Stage = c.Stage == WuLuKangPindianStage.BeforeSourceReplacement ? WuLuKangShenduanStage.DiscardChoice : WuLuKangShenduanStage.BottomChildren,
            Recipients = recipients,
            Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence,
            After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangShenduanStartedEvent(f.Id, r.WindowId, r.ContestId, r.Source, r.GameplayHash, r.WindowStage, r.ComparisonIndex, r.ParticipantSeat));
        if (c.Stage == WuLuKangPindianStage.GroupClose)
        {
            CancelWuLuKangFromShenduan(f, parent.WuLuKangContest!.CancellationReason ?? "participant-unavailable");
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (r.Stage == WuLuKangShenduanStage.DiscardChoice)
        {
            if (parent.WuLuKangContest!.Cancelled)
            {
                FinishWuLuKangShenduan(f);
                return SkillProgramStepOutcome.AwaitChild;
            }

            var choices = WuLuKangShenduanChoices(f);
            if (choices.Count == 0)
            {
                CancelWuLuKangFromShenduan(f, "unpaid-source-unavailable");
                return SkillProgramStepOutcome.AwaitChild;
            }

            PublishWuLuKang(f, f.OwnerSeat, choices, "审断：弃置一张手牌或装备牌，以牌堆中点数最大的牌替代本次拼点牌。");
            return SkillProgramStepOutcome.AwaitChoice;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        if (w.Result is { } result && result.SourceRank != result.OpponentRank)
        {
            var winner = result.SourceWon ? result.SourceSeat : result.OpponentSeat;
            var id = result.CardOf(winner);
            if (_cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing)
            {
                var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DrawPile, new(WuLuKangBottomReason), m =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var contest = WuLuKangParent(r.ContestId);
                    var receipt = contest.WuLuKangContest!;
                    _coreRuntimeOperations.ReplaceRuntimeFrame(contest.Id, contest with { WuLuKangContest = receipt with { BottomedCardIds = receipt.BottomedCardIds.Append(id).ToArray() } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangWinnerBottomedEvent(f.Id, r.ContestId, r.ComparisonIndex, winner, id, m.Sequence));
                });
                _cardZonesOperations._cardZones.PlaceDrawPileCardsAtBottom([id]);
                SealWuLuKangMovements(f.Id, before);
            }
        }

        ContinueWuLuKangShenduan(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void PublishWuLuKang(ProgramSkillFrame f, int chooser, IReadOnlyList<PromptChoice> choices, string text)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = chooser,
            Choices = Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal IReadOnlyList<PromptChoice> WuLuKangShenduanChoices(ProgramSkillFrame f)
    {
        var r = f.WuLuKangShenduan!;
        var parent = WuLuKangParent(r.ContestId);
        var assigned = parent.WuLuKangContest!.Assignments.Single(a => a.Seat == r.ParticipantSeat);
        Dictionary<string, string> P(string branch) => new()
        {
            ["program-action"] = "wu-lu-kang-shenduan",
            ["branch"] = branch,
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture)
        };
        var choices = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "wu-lu-kang-shenduan", ["branch"] = "discard", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["zone"] = c.Parameters["source-zone"].ToLowerInvariant() } }).ToList();
        choices.Add(new(new($"wu-lu-kang.{(f.Id)}.decline"), assigned.CardId > 0 ? "不替换拼点牌" : "不弃牌并取消本次拼点", [], [], P("decline")));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal CardLocation WuLuKangAssignedLocation(ResolutionFrame parent, int seat)
    {
        var r = parent.WuLuKangContest!;
        var a = r.Assignments.Single(i => i.Seat == seat);
        return r.Replacements.Any(p => p.ParticipantSeat == seat && p.ReservedCardId == a.CardId) ? CardLocation.Processing : r.Original.Single(i => i.Seat == seat).From;
    }

    [ProgramAnswer("wu-lu-kang-shenduan")]
    internal void AnswerWuLuKangShenduan(PromptChoice selected)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(_resolutionStackOperations._resolutionStack[^1].Id);
        var r = f.WuLuKangShenduan!;
        if (!ValidWuLuKangShenduan(f) || r.Stage != WuLuKangShenduanStage.DiscardChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], WuLuKangShenduanChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Shenduan requires its original participant's actual HE discard slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters["branch"] == "decline")
        {
            var contest = WuLuKangParent(r.ContestId);
            if (contest.WuLuKangContest!.Assignments.Single(a => a.Seat == r.ParticipantSeat).CardId <= 0)
                CancelWuLuKangFromShenduan(f, "discard-backed-source-declined");
            else
                FinishWuLuKangShenduan(f);
            return;
        }

        var from = new CardLocation(Enum.Parse<CardZoneKind>(selected.Parameters["source-zone"]), f.OwnerSeat);
        var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(selected.Parameters["slot-index"], CultureInfo.InvariantCulture)];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.DiscardChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new(WuLuKangDiscardReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence > before && m.CardId == card.Id && m.From == from && m.Reason.Value == WuLuKangDiscardReason);
        var payment = new WuLuKangShenduanPayment(f.Id, r.Source, r.GameplayHash, r.ParticipantSeat, card.Id, card.Kind, from, movement.To, movement.Sequence, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = f.WuLuKangShenduan!with { Payment = payment } });
        var parent = WuLuKangParent(r.ContestId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(parent.Id, parent with { WuLuKangContest = parent.WuLuKangContest!with { Replacements = parent.WuLuKangContest.Replacements.Append(payment).ToArray() } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangShenduanDiscardPaidEvent(f.Id, r.ContestId, r.ParticipantSeat, card.Id, card.Kind, from, movement.To, movement.Sequence));
        SealWuLuKangMovements(f.Id, before);
        ContinueWuLuKangShenduan(f.Id);
    }

    internal bool WuLuKangInputsAvailable(ResolutionFrame parent, int? replacing = null) => parent.WuLuKangContest!.Assignments.All(a => _coreMatchParticipantsPlayersAccess._players[a.Seat].IsAlive && (a.Seat == replacing || a.CardId > 0 && _cardZonesOperations._cardZones.GetLocation(a.CardId) == WuLuKangAssignedLocation(parent, a.Seat) || a.CardId <= 0 && !parent.WuLuKangContest.SourcesReady && !parent.WuLuKangContest.Replacements.Any(p => p.ParticipantSeat == a.Seat) && CanWuLuKangDiscardForPindian(_coreMatchParticipantsPlayersAccess._players[a.Seat])));
    internal void ContinueWuLuKangShenduan(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangShenduan: { } r } f || f.Id != id)
            return;
        if (r.Stage is WuLuKangShenduanStage.DiscardChoice or WuLuKangShenduanStage.Drawing)
            return;
        if (DrainWuLuKangChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.WuLuKangShenduan!;
        var parent = WuLuKangParent(r.ContestId);
        if (r.Stage == WuLuKangShenduanStage.CancelChildren)
        {
            FinishWuLuKangShenduan(f);
            return;
        }

        if (r.Stage == WuLuKangShenduanStage.DiscardChildren)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !WuLuKangInputsAvailable(parent, r.ParticipantSeat))
            {
                CancelWuLuKangFromShenduan(f, "participant-or-unpaid-input-unavailable");
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.RefillChildren, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _corePhysicalDrawFacadesOperations.EnsureDrawPile();
            SealWuLuKangMovements(id, before);
            ContinueWuLuKangShenduan(id);
            return;
        }

        if (r.Stage == WuLuKangShenduanStage.RefillChildren)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !WuLuKangInputsAvailable(parent, r.ParticipantSeat) || _cardZonesOperations._cardZones.Count(CardLocation.DrawPile) == 0)
            {
                CancelWuLuKangFromShenduan(f, "maximum-stock-unavailable");
                return;
            }

            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Reverse().OrderByDescending(c => c.Rank).First();
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.ReserveChildren, Before = before, After = before } });
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, new(WuLuKangReserveReason), m =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                var paid = current.WuLuKangShenduan!.Payment!with
                {
                    ReservedCardId = m.CardId,
                    ReservedPrintedRank = card.Rank,
                    ReservationSequence = m.Sequence,
                    After = m.Sequence
                };
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { WuLuKangShenduan = current.WuLuKangShenduan with { Payment = paid, After = m.Sequence } });
                var contest = WuLuKangParent(r.ContestId);
                var receipt = contest.WuLuKangContest!;
                var effective = card.Kind == CardKind.Alcohol && _coreAlcoholIdentityPoliciesOperations.HasAlcoholKingIdentityPolicy(_coreMatchParticipantsPlayersAccess._players[r.ParticipantSeat]) ? 13 : _corePindianPoliciesOperations.EffectivePindianRank(_coreMatchParticipantsPlayersAccess._players[r.ParticipantSeat], card);
                _coreRuntimeOperations.ReplaceRuntimeFrame(contest.Id, contest with { WuLuKangContest = receipt with { Replacements = receipt.Replacements.Select(p => p.FrameId == id ? paid : p).ToArray(), Assignments = receipt.Assignments.Select(a => a.Seat == r.ParticipantSeat ? new WuLuKangPindianAssignment(a.Seat, card.Id, effective) : a).ToArray() } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangMaximumReservedEvent(id, r.ContestId, r.ParticipantSeat));
            }, emitCardMovedEvents: false, disclosure: CardMovementDisclosure.FaceDown);
            SealWuLuKangMovements(id, before, privateReserve: true);
            ContinueWuLuKangShenduan(id);
            return;
        }

        if (r.Stage == WuLuKangShenduanStage.ReserveChildren)
        {
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !WuLuKangInputsAvailable(parent))
            {
                CancelWuLuKangFromShenduan(f, "reserved-input-unavailable");
                return;
            }

            FinishWuLuKangShenduan(f);
            return;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishWuLuKangShenduan(f);
            return;
        }

        if (r.Stage == WuLuKangShenduanStage.DrawChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r = r with { Stage = WuLuKangShenduanStage.BottomChildren, RecipientIndex = r.RecipientIndex + 1, Draw = null, SelectedMinimumCardId = null, SelectedMinimumRank = null }, PendingMovementContinuation = null });
        }

        while (r.RecipientIndex < r.Recipients.Count && !_coreMatchParticipantsPlayersAccess._players[r.Recipients[r.RecipientIndex]].IsAlive)
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r = r with { RecipientIndex = r.RecipientIndex + 1 } });
        if (r.RecipientIndex == r.Recipients.Count)
        {
            FinishWuLuKangShenduan(f);
            return;
        }

        var recipient = r.Recipients[r.RecipientIndex];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.Drawing }, PendingMovementContinuation = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangKDrawIssuedEvent(id, r.ContestId, r.ComparisonIndex, r.Source, recipient));
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, recipient, 1, new(WuLuKangDrawReason));
    }

    internal Card WuLuKangMinimumDrawMaterial(CharacterState player, CardMoveReason reason, IReadOnlyList<Card> stock, Card ordinary)
    {
        if (reason.Value != WuLuKangDrawReason || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuLuKangShenduan: { Stage: WuLuKangShenduanStage.Drawing } r, NativeDraw: { Operation: SkillProgramEffectOp.ResolveWuLuKangShenduan, RequestedCount: 1, Replacement: null } } f)
            return ordinary;
        if (!ValidWuLuKangShenduan(f) || r.Recipients[r.RecipientIndex] != player.Seat || r.SelectedMinimumCardId is not null || stock.Count == 0)
            throw new InvalidOperationException("Shenduan minimum material requires its one exact unreplaced native draw request.");
        var card = stock.Reverse().OrderBy(c => c.Rank).First();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuLuKangShenduan = r with { SelectedMinimumCardId = card.Id, SelectedMinimumRank = card.Rank } });
        return card;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWuLuKangShenduan)]
    internal void ReturnWuLuKangMinimumDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.WuLuKangShenduan!;
        if (!ValidWuLuKangShenduan(f) || r.Stage != WuLuKangShenduanStage.Drawing || outcome.RequestedCount != 1 || outcome.RecipientSeat != r.Recipients[r.RecipientIndex] || outcome.ActualDrawIds.Count > 1 || outcome.ActualDrawIds.Count == 1 && outcome.ActualDrawIds[0] != r.SelectedMinimumCardId)
            throw new InvalidOperationException("Shenduan cannot replace its native minimum material or draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.DrawChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangKDrawReturnedEvent(f.Id, outcome.RecipientSeat, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count, outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter));
        SealWuLuKangMovements(f.Id, outcome.SequenceBefore);
        ContinueWuLuKangShenduan(f.Id);
    }

    internal void CancelWuLuKangFromShenduan(ProgramSkillFrame f, string reason)
    {
        var r = f.WuLuKangShenduan!;
        MarkWuLuKangCancelled(r.ContestId, reason);
        var parent = WuLuKangParent(r.ContestId);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = r with { Stage = WuLuKangShenduanStage.CancelChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        foreach (var id in WuLuKangHeldProcessing(parent))
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.PindianFinish);
        SealWuLuKangMovements(f.Id, before);
        ContinueWuLuKangShenduan(f.Id);
    }

    internal void FinishWuLuKangShenduan(ProgramSkillFrame f)
    {
        var r = f.WuLuKangShenduan!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WuLuKangShenduanCompletedEvent(f.Id, r.WindowId, r.ContestId, r.ComparisonIndex));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WuLuKangShenduan = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveWuLuKangShenduan)]
    internal PromptChoice AiWuLuKangShenduan(PendingDecision pending, ProgramSkillFrame f) => pending.Choices.Where(c => c.Parameters["branch"] == "discard").OrderBy(c => c.Cards.Count == 1 ? CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[pending.PlayerSeat]) : 0).ThenBy(c => c.Id.Value, StringComparer.Ordinal).FirstOrDefault() ?? pending.Choices[0];
}
