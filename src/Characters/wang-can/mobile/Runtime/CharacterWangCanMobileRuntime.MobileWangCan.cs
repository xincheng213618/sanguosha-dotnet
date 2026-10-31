#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanMobileRuntime
{
    internal const string MobileWangCanShanxi = "mobile:shanxi-wang-can";
    internal const string MobileWangCanXiGift = "skill-program.mobile-wang-can.xi-gift";
    internal static CardConversionSource MobileWangCanSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal IEnumerable<int> MobileWangCanXiTargets(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner && p.Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) == 0).Select(p => p.Seat);
    SkillProgramStepOutcome global::CardGame.Core.IMobileWangCanHost.Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => BeginMobileWangCanXi(frame, op);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 81500, SkillProgramEffectOp.TransferMobileWangCanXi)]
    internal bool CanOfferMobileWangCanMark(ProgramTriggerLegalityContext g) => g.SkillId == MobileWangCanShanxi && MobileWangCanXiTargets(g.OwnerSeat).Any();
    [ProgramPipelineHook(ProgramPipelinePhase.CaptureHealthChange, 81500)]
    internal void CaptureMobileWangCanRecovery(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Recovery || change.Amount <= 0 || change.HpAfter <= 0 || !_coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive || _coreMatchParticipantsPlayersAccess._players[change.TargetSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) < 1)
            return;
        var mark = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiMovedEvent>().LastOrDefault();
        if (mark is null || mark.TargetSeat != change.TargetSeat || !_coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat], MobileWangCanShanxi, mark.Source.SkillInstanceId))
            return;
        foreach (var c in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[mark.Source.OwnerSeat], SkillProgramTriggerWindow.AfterHpRecovered).Where(c => c.SkillId == MobileWangCanShanxi && c.BindingId == "marked-recovery" && c.SkillInstanceId == mark.Source.SkillInstanceId && c.GameplayHash == mark.Hash))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWangCanXiQualifiedEvent(change.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, change.TargetSeat));
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IncludeHealthChangeCandidates, 81500)]
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMobileWangCanRecovery(HpChangeContext change, IReadOnlyList<ProgramTriggerCandidate> ordinary) => Array.AsReadOnly(ordinary.Where(c => c.SkillId != MobileWangCanShanxi).Concat(_coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiQualifiedEvent>().Where(e => e.ChangeId == change.Id).Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.Hash, 0))).ToArray());
    internal bool ExactMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => c.SkillId == MobileWangCanShanxi && c.BindingId == "marked-recovery" && c.OccurrenceIndex == 0 && w is { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery, HpAfter: > 0 } change } && w.OwnerSeat == c.OwnerSeat && w.ParentFrameId == change.Id && w.TargetSeat == change.TargetSeat && w.SourceSeat == change.SourceSeat && w.Amount == change.Amount && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[change.TargetSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[change.TargetSeat].Hp > 0 && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) && _contentRegistryOperations._contentRegistry.GetSkill(c.SkillId).Program!.GameplayHash == c.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiQualifiedEvent>().Count(e => e.ChangeId == change.Id && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.Hash == c.GameplayHash && e.TargetSeat == change.TargetSeat) == 1 && _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault()is { } hp && hp.Id == change.Id && hp.Change == change && hp.CandidateIndex >= 0 && hp.CandidateIndex < hp.Candidates.Count && hp.Candidates[hp.CandidateIndex] == c;
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 81500)]
    internal bool AdmitMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => ExactMobileWangCanRecovery(c, w);
    internal SkillProgramStepOutcome BeginMobileWangCanXi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileWangCanShanxi || f.MobileWangCanXi is not null || f.InstructionIndex != 1 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Shanxi lost its exact active issuing source.");
        var marking = op == SkillProgramEffectOp.TransferMobileWangCanXi;
        if (marking ? f.TriggerId != "move-xi" || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, OwnerSeat: var owner } || owner != f.OwnerSeat || !MobileWangCanXiTargets(owner).Any() : f.WindowContext is not { } w || !ExactMobileWangCanRecovery(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Shanxi lost its actual Play start or exact marked recovery event.");
        var r = new MobileWangCanXiReceipt(MobileWangCanSource(f), f.GameplayHash, marking ? 0 : f.WindowContext!.HpChange!.Id, marking ? -1 : f.WindowContext!.HpChange!.TargetSeat, marking ? MobileWangCanXiStage.Mark : MobileWangCanXiStage.Choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileWangCanXi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWangCanXiIssuedEvent(f.Id, r.Source, r.Hash, r.ChangeId, r.Subject));
        ContinueMobileWangCanXi(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> MobileWangCanXiCosts(ProgramSkillFrame f) => _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.MobileWangCanXi!.Subject, f.MobileWangCanXi.Subject, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Where(c => c.Cards.Count == 1).ToArray();
    internal IReadOnlyList<PromptChoice> MobileWangCanXiChoices(ProgramSkillFrame f)
    {
        var r = f.MobileWangCanXi!;
        var choices = new List<PromptChoice>();
        void Add(string id, string label, string branch, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => choices.Add(new(new($"mobile-wang-can.{(f.Id)}.{(id)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = "mobile-wang-can-xi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        if (r.Stage == MobileWangCanXiStage.Mark)
            foreach (var target in MobileWangCanXiTargets(f.OwnerSeat))
                Add("mark." + target, "将檄移给" + _coreMatchParticipantsPlayersAccess._players[target].Name, "mark", [], [target]);
        if (r.Stage == MobileWangCanXiStage.Choice)
        {
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && MobileWangCanXiCosts(f).Count >= 2)
                Add("gift", "交给王粲两张牌", "gift", [], []);
            Add("lose-hp", "失去1点体力", "lose-hp", [], []);
        }

        if (r.Stage == MobileWangCanXiStage.Cards)
        {
            foreach (var card in r.Cards)
                Add("unpick." + card, "取消已选牌", "unpick", [card], []);
            if (r.Cards.Count < 2)
                foreach (var card in MobileWangCanXiCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single())))
                    Add("card." + card.Cards.Single(), card.Label, "card", card.Cards, []);
            if (r.Cards.Count == 2 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                Add("pay", "交出所选两张牌", "pay", [], []);
        }

        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }

    internal void FinishMobileWangCanXi(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileWangCanXi = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal void ContinueMobileWangCanXi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileWangCanXi: { } r } f || f.Id != id)
            return;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || r.Subject >= 0 && !_coreMatchParticipantsPlayersAccess._players[r.Subject].IsAlive)
        {
            FinishMobileWangCanXi(f);
            return;
        }

        if (r.Stage is MobileWangCanXiStage.Mark or MobileWangCanXiStage.Choice or MobileWangCanXiStage.Cards)
        {
            var choices = MobileWangCanXiChoices(f);
            if (choices.Count == 0)
            {
                FinishMobileWangCanXi(f);
                return;
            }

            var chooser = r.Stage == MobileWangCanXiStage.Mark ? f.OwnerSeat : r.Subject;
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MobileWangCanXiStage.Mark ? "选择一名没有檄的其他角色。" : "善檄：交给王粲两张牌，或失去1点体力。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = r.Stage == MobileWangCanXiStage.Cards,
                Choices = choices,
                SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MobileWangCanXi!;
        if (r.Stage is MobileWangCanXiStage.GiftChildren or MobileWangCanXiStage.HealthChildren)
        {
            FinishMobileWangCanXi(f);
            return;
        }

        if (r.Stage != MobileWangCanXiStage.Health)
            throw new InvalidOperationException("Shanxi lost its paid native child cursor.");
        if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(r.Subject) && _characterDaQiaoXiaoQiaoHappyGodOperations.TryBeginHappyQiaoProgramHealthChoice(f, r.Subject, HpChangeKind.Loss, 1))
            return;
        var outcome = _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(id, f.SkillId, r.Subject, 1, (before, next, actual) =>
        {
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { MobileWangCanXi = current.MobileWangCanXi!with { Stage = MobileWangCanXiStage.HealthChildren, ActualCount = actual } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWangCanXiPaidEvent(id, r.Subject, false, actual, 0, 0));
            if (_characterDaQiaoXiaoQiaoHappyGodOperations.HasHappyQiaoHealth(r.Subject))
                _characterDaQiaoXiaoQiaoHappyGodOperations.CompleteHappyQiaoHealthPayment(id, r.Subject, HpChangeKind.Loss, before, next);
        });
        if (outcome == SkillProgramStepOutcome.Continue)
            ContinueMobileWangCanXi(id);
    }

    [ProgramAnswer("mobile-wang-can-xi")]
    internal void AnswerMobileWangCanXi(PromptChoice selected)
    {
        var f = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Shanxi answer lost its original frame.");
        var r = f.MobileWangCanXi!;
        if (selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], MobileWangCanXiChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Shanxi payment or target is no longer legal.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "mark":
                var target = selected.Targets.Single();
                var marker = PlayerMarkerKind.MobileWangCanXi;
                foreach (var p in _coreMatchParticipantsPlayersAccess._players)
                {
                    foreach (var source in p.MarkerSourceCounts.Where(x => x.Key.Marker == marker).ToArray())
                    {
                        p.MarkerSourceCounts.Remove(source.Key);
                        var remaining = p.Markers.GetValueOrDefault(marker) - source.Value;
                        if (remaining == 0)
                            p.Markers.Remove(marker);
                        else
                            p.Markers[marker] = remaining;
                        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, p.Seat, marker, -source.Value, remaining, source.Key.SkillOwnerSeat, "program.mobile-wang-can.xi-transfer"));
                    }
                }

                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, target, marker, 1);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWangCanXiMovedEvent(f.Id, r.Source, r.Hash, target));
                FinishMobileWangCanXi(f);
                return;
            case "gift":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.Cards } });
                break;
            case "lose-hp":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.Health } });
                break;
            case "card":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } });
                break;
            case "unpick":
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } });
                break;
            case "pay":
                var available = MobileWangCanXiCosts(f).SelectMany(c => c.Cards).ToHashSet();
                if (r.Cards.Count != 2 || r.Cards.Distinct().Count() != 2 || r.Cards.Any(c => !available.Contains(c)) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
                    throw new InvalidOperationException("Shanxi requires two current actual HE cards and a living recipient.");
                var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.GiftChildren, Before = before, After = before } });
                _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(f.OwnerSeat), new(MobileWangCanXiGift), (_, moves) =>
                {
                    var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                    var actual = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat));
                    _coreRuntimeOperations.ReplaceRuntimeTop(current with { MobileWangCanXi = current.MobileWangCanXi!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence, ActualCount = actual } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileWangCanXiPaidEvent(f.Id, r.Subject, true, actual, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
                });
                break;
            default:
                throw new InvalidOperationException("Unknown Shanxi branch.");
        }

        ContinueMobileWangCanXi(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 81500)]
    internal bool ResumeMobileWangCanXi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileWangCanXi: not null } f || f.Id != id)
            return false;
        ContinueMobileWangCanXi(id);
        return true;
    }

    [ProgramAiChoice(SkillProgramEffectOp.TransferMobileWangCanXi, SkillProgramEffectOp.ResolveMobileWangCanXiRecovery)]
    internal PromptChoice ChooseMobileWangCanXi(PendingDecision d, ProgramSkillFrame f) => d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? d.Choices.FirstOrDefault(c => c.Parameters["branch"] is "mark" or "gift" or "card") ?? d.Choices[0];
}
