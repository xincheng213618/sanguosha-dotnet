#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuChuAnniversaryGodRuntime
{
    internal const string ShenXuChuAction = "shen-xu-chu";
    internal static string ZhuangpoDiscardReason(ProgramSkillFrame f) => "skill-program." + f.SkillId + ".discard";
    internal static CardConversionSource ShenXuChuSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IShenXuChuHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginShenXuChu(f, op);
    internal ProgramCardTriggerWindowFrame? ZhuangpoParent(ProgramSkillWindowContext? w) => w?.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(p => p.Id == w.ParentFrameId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 38800, SkillProgramEffectOp.ResolveZhuangpoTarget)]
    internal bool CanResolveZhuangpo(ProgramTriggerLegalityContext g) => ZhuangpoParent(g.WindowContext)is { Action: { EffectiveKind: CardKind.Duel } a } && a.ActorSeat == g.OwnerSeat && a.ConversionChain.Contains(new(g.SkillId, ShenXuChuComposition.ViewAs, g.OwnerSeat, g.Candidate.SkillInstanceId)) && a.TargetSeats.Count > 0 && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoTargetIssuedEvent>().Any(e => e.ActionId == a.ActionId && e.Source.OwnerSeat == g.OwnerSeat && e.Source.SkillInstanceId == g.Candidate.SkillInstanceId);
    internal (int Damage, int[] Seats) ZhengqingMaximum()
    {
        var totals = new Dictionary<(int Turn, int Source), int>();
        var actualTurn = 0;
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory())
        {
            if (fact is RoundStartedEvent)
            {
                totals.Clear();
                actualTurn = 0;
            }
            else if (fact is TurnStartedEvent t)
                actualTurn = t.TurnNumber;
            else if (fact is DamageAppliedEvent { Amount: > 0, SourceLess: false } d && actualTurn > 0 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(d.SourceSeat))
            {
                var key = (actualTurn, d.SourceSeat);
                totals[key] = checked(totals.GetValueOrDefault(key) + d.Amount);
            }
        }

        var best = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => (p.Seat, Damage: totals.Where(t => t.Key.Source == p.Seat).Select(t => t.Value).DefaultIfEmpty().Max())).ToArray();
        var maximum = best.Select(p => p.Damage).DefaultIfEmpty().Max();
        return (maximum, maximum == 0 ? [] : best.Where(p => p.Damage == maximum).Select(p => p.Seat).OrderBy(s => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatAccess._currentSeat, s)).ToArray());
    }

    internal static int ZhengqingWinner(IReadOnlyList<int> seats, int owner, DeterministicRandom random) => seats.Contains(owner) ? owner : seats.Count == 0 ? -1 : seats.Count == 1 ? seats[0] : seats[random.Next(seats.Count)];
    internal SkillProgramStepOutcome BeginShenXuChu(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShenXuChu is not null || f.InstructionIndex != 1 || f.WindowContext is not { } w || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash)
            throw new InvalidOperationException("Shen Xu Chu requires his original unpaid trigger instruction.");
        var r = new ShenXuChuReceipt(ShenXuChuSource(f), f.GameplayHash, w.ParentFrameId, f.InstructionIndex, op, ShenXuChuStage.MarkChildren);
        if (op == SkillProgramEffectOp.SettleZhengqingRound)
        {
            if (!ShenXuChuComposition.IsZhengqing(f.SkillId) || w.Window != SkillProgramTriggerWindow.RoundEnding || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame boundary || boundary.RoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhengqingSettledEvent>().Any(e => e.Source == r.Source && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber))
                throw new InvalidOperationException("Zhengqing requires its once-only actual round ending.");
            var(maximum, seats) = ZhengqingMaximum();
            var before = _randomOperations._random.State;
            var winner = ZhengqingWinner(seats, f.OwnerSeat, _randomOperations._random);
            var draws = _characterXuChuHappyGodOperations.ShenXuChuRoundDraws(f, winner, maximum);
            r = r with
            {
                TargetSeat = winner,
                PaidMarks = maximum,
                Draws = draws,
                MaximumSeats = seats,
                RandomBefore = before,
                RandomAfter = _randomOperations._random.State
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhengqingSettledEvent(f.Id, r.Source, _coreActualRoundProgressRoundNumberAccess._roundNumber, winner, maximum));
            foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            {
                var old = p.Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner);
                if (old == 0)
                    continue;
                if (p.IsAlive)
                    _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, p.Seat, PlayerMarkerKind.QingBanner, -old);
                else
                {
                    foreach (var key in p.MarkerSourceCounts.Keys.Where(k => k.Marker == PlayerMarkerKind.QingBanner).ToArray())
                        p.MarkerSourceCounts.Remove(key);
                    p.Markers.Remove(PlayerMarkerKind.QingBanner);
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, p.Seat, PlayerMarkerKind.QingBanner, -old, 0, f.OwnerSeat, "skill-program." + f.SkillId + ".clear"));
                }
            }

            if (winner >= 0)
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, winner, PlayerMarkerKind.QingBanner, maximum);
        }
        else
        {
            if (op != SkillProgramEffectOp.ResolveZhuangpoTarget || !ShenXuChuComposition.IsZhuangpo(f.SkillId) || ZhuangpoParent(w)is not { } parent || _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is not { Action: { EffectiveKind: CardKind.Duel } action } use || action.ActorSeat != f.OwnerSeat || !action.ConversionChain.Contains(new(f.SkillId, ShenXuChuComposition.ViewAs, f.OwnerSeat, f.SkillInstanceId)))
                throw new InvalidOperationException("Zhuangpo requires the exact converted Duel's designation parent.");
            r = r with
            {
                UseId = use.Id,
                ActionId = action.ActionId,
                Targets = action.TargetSeats,
                TargetSeat = action.TargetSeats[0],
                TargetMarked = action.TargetSeats.Any(s => _coreMatchParticipantsPlayersAccess._players[s].Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner) > 0),
                Stage = ShenXuChuStage.MarkChoice
            };
            r = r with
            {
                Draws = global::CardGame.Core.EngineOwners.CharacterXuChuHappyGodRuntime.ShenXuChuTargetDraws(f, r.TargetMarked)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuangpoTargetIssuedEvent(f.Id, r.Source, r.GameplayHash, use.Id, action.ActionId, r.TargetMarked ? action.TargetSeats.First(s => _coreMatchParticipantsPlayersAccess._players[s].Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner) > 0) : r.TargetSeat, r.TargetMarked));
        }

        ContinueShenXuChu(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool DrainShenXuChu(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.Program);
    [ProgramContinuation(ProgramContinuationKind.Resume, 38800)]
    internal bool ResumeShenXuChu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenXuChu: not null } f || f.Id != id)
            return false;
        ContinueShenXuChu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 38800)]
    internal bool ReturnShenXuChuMovement(ProgramSkillFrame f) => f.ShenXuChu is not null && ResumeShenXuChu(f.Id);
    internal void ContinueShenXuChu(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { ShenXuChu: { } r } f && f.Id == id)
        {
            if (r.Stage == ShenXuChuStage.DrawAttempt)
                return;
            if (DrainShenXuChu(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.ShenXuChu!;
            if (_coreMatchOutcomeWinnerAccess._winner != Winner.None)
            {
                FinishShenXuChu(f);
                return;
            }

            if (r.Stage == ShenXuChuStage.MarkChoice)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner) == 0 || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
                {
                    NextZhuangpoTarget(f);
                    continue;
                }

                PublishShenXuChu(f);
                return;
            }

            if (r.Stage == ShenXuChuStage.DiscardChoice)
            {
                if (!_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive || ZhuangpoDiscardOptions(f).Count == 0)
                {
                    NextZhuangpoTarget(f);
                    continue;
                }

                PublishShenXuChu(f);
                return;
            }

            if (r.Stage == ShenXuChuStage.DiscardChildren)
            {
                NextZhuangpoTarget(f);
                continue;
            }

            if (r.Stage == ShenXuChuStage.DrawChildren)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r = r with { Stage = ShenXuChuStage.MarkChildren, DrawIndex = r.DrawIndex + 1 }, PendingMovementContinuation = null });
            if (r.Stage == ShenXuChuStage.MarkChildren)
            {
                while (r.DrawIndex < r.Draws.Count && (!_coreMatchParticipantsPlayersAccess._players[r.Draws[r.DrawIndex].Seat].IsAlive || r.Draws[r.DrawIndex].Count == 0))
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r = r with { DrawIndex = r.DrawIndex + 1 } });
                if (r.DrawIndex >= r.Draws.Count)
                {
                    FinishShenXuChu(f);
                    return;
                }

                var draw = r.Draws[r.DrawIndex];
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r with { Stage = ShenXuChuStage.DrawAttempt } });
                _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, draw.Seat, draw.Count, new("skill-program." + f.SkillId + ".draw"));
                return;
            }
        }
    }

    internal void NextZhuangpoTarget(ProgramSkillFrame f)
    {
        var r = f.ShenXuChu!;
        var next = r.TargetIndex + 1;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenXuChu = r with { Stage = next < r.Targets.Count ? ShenXuChuStage.MarkChoice : ShenXuChuStage.MarkChildren, TargetIndex = next, TargetSeat = next < r.Targets.Count ? r.Targets[next] : r.TargetSeat, PaidMarks = 0, RequiredDiscards = 0, Selected = [] }, PendingMovementContinuation = null });
    }

    internal void FinishShenXuChu(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenXuChu = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.SettleZhengqingRound)]
    internal void ReturnZhengqingDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnShenXuChuDraw(f, outcome);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveZhuangpoTarget)]
    internal void ReturnZhuangpoDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => ReturnShenXuChuDraw(f, outcome);
    internal void ReturnShenXuChuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShenXuChu ?? throw new InvalidOperationException("Shen Xu Chu draw lost its frame.");
        if (r.Stage != ShenXuChuStage.DrawAttempt || r.DrawIndex >= r.Draws.Count || r.Draws[r.DrawIndex] != new ShenXuChuDraw(outcome.RecipientSeat, outcome.RequestedCount) || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<ShenXuChuDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Index == r.DrawIndex))
            throw new InvalidOperationException("Shen Xu Chu requires the once-only exact native draw return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShenXuChu = r with { Stage = ShenXuChuStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShenXuChuDrawReturnedEvent(f.Id, r.DrawIndex, outcome.RecipientSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueShenXuChu(f.Id);
    }

    internal IReadOnlyList<PromptChoice> ZhuangpoDiscardOptions(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.ShenXuChu!.TargetSeat, f.ShenXuChu.TargetSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard, canSelect: (_, c) => !f.ShenXuChu.Selected.Contains(c.Id));
    internal IReadOnlyList<PromptChoice> ShenXuChuChoices(ProgramSkillFrame f)
    {
        var r = f.ShenXuChu!;
        PromptChoice Choice(string option, string text, IReadOnlyList<int> cards) => new(new($"shen-xu-chu.{(f.Id)}.{(r.TargetIndex)}.{(option)}"), text, cards, [], new Dictionary<string, string> { ["program-action"] = ShenXuChuAction, ["option"] = option });
        return r.Stage == ShenXuChuStage.MarkChoice ? Array.AsReadOnly(Enumerable.Range(0, checked(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner) + 1)).Select(n => Choice(n.ToString(CultureInfo.InvariantCulture), n == 0 ? "不移去擎" : $"移去 {(n)} 个擎，令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 弃置 {(n)} 张牌", [])).ToArray()) : Array.AsReadOnly(ZhuangpoDiscardOptions(f).Select(c => Choice("card-" + c.Cards.Single(), c.Label, c.Cards)).ToArray());
    }

    internal void PublishShenXuChu(ProgramSkillFrame f)
    {
        var r = f.ShenXuChu!;
        var chooser = r.Stage == ShenXuChuStage.MarkChoice ? f.OwnerSeat : r.TargetSeat;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        _coreParticipantHandProgramsOperations.PublishParticipantHandChoice(f, chooser, ShenXuChuChoices(f), r.Stage == ShenXuChuStage.MarkChoice ? "壮魄：选择移去的擎数量。" : $"壮魄：还需弃置 {(r.RequiredDiscards - r.Selected.Count)} 张牌。", r.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer(ShenXuChuAction)]
    internal void AnswerShenXuChu(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShenXuChu: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == ShenXuChuStage.MarkChoice ? f.OwnerSeat : r.TargetSeat) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], ShenXuChuChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Zhuangpo requires the current participant's exact private answer.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == ShenXuChuStage.MarkChoice)
        {
            var count = int.Parse(selected.Parameters["option"], CultureInfo.InvariantCulture);
            var before = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.QingBanner);
            if (count == 0)
            {
                NextZhuangpoTarget(f);
                ContinueShenXuChu(f.Id);
                return;
            }

            if (!_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) || count < 1 || count > before)
                throw new InvalidOperationException("Zhuangpo's unspent mark cost is no longer available.");
            var required = Math.Min(count, ZhuangpoDiscardOptions(f).Count);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r with { Stage = ShenXuChuStage.DiscardChoice, PaidMarks = count, RequiredDiscards = required } });
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.QingBanner, -count);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuangpoMarksPaidEvent(f.Id, f.OwnerSeat, r.TargetSeat, before, count, before - count));
            if (required == 0)
                NextZhuangpoTarget(f);
            ContinueShenXuChu(f.Id);
            return;
        }

        if (r.Stage != ShenXuChuStage.DiscardChoice)
            throw new InvalidOperationException("Zhuangpo cannot pay a completed discard twice.");
        var ids = r.Selected.Append(selected.Cards.Single()).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r = r with { Selected = ids } });
        if (ids.Length < r.RequiredDiscards)
        {
            PublishShenXuChu(f);
            return;
        }

        var start = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShenXuChu = r with { Stage = ShenXuChuStage.DiscardChildren, Before = start, After = start }, PendingMovementContinuation = new(r.TargetSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.TargetSeat, ids, new(ZhuangpoDiscardReason(f)), (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var end = records.Select(m => (long)m.Sequence).DefaultIfEmpty(start).Max();
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { ShenXuChu = active.ShenXuChu!with { After = end } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhuangpoDiscardPaidEvent(f.Id, r.TargetSeat, ids.Length, start, end));
        });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveZhuangpoTarget)]
    internal PromptChoice ChooseShenXuChu(PendingDecision p, ProgramSkillFrame f) => f.ShenXuChu!.Stage == ShenXuChuStage.MarkChoice ? p.Choices.First(c => c.Parameters["option"] == "0") : p.Choices[0];
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 38800)]
    internal int ZhuangpoDuelBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.EffectiveCardKind != CardKind.Duel || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } action } use)
            return 0;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuangpoTargetIssuedEvent>().Any(e => e.UseId == use.Id && e.ActionId == action.ActionId && e.TargetMarked && ShenXuChuComposition.IsZhuangpo(e.Source.SkillId) && e.Source.OwnerSeat == action.ActorSeat && action.ConversionChain.Contains(new(e.Source.SkillId, ShenXuChuComposition.ViewAs, e.Source.OwnerSeat, e.Source.SkillInstanceId))) ? 1 : 0;
    }
}
