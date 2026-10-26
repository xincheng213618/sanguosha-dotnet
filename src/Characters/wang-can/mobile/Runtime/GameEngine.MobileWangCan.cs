using System.Globalization;
namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string MobileWangCanShanxi = "mobile:shanxi-wang-can";
    private const string MobileWangCanXiGift = "skill-program.mobile-wang-can.xi-gift";
    private static CardConversionSource MobileWangCanSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private IEnumerable<int> MobileWangCanXiTargets(int owner) => _players.Where(p => p.IsAlive && p.Seat != owner && p.Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) == 0).Select(p => p.Seat);
    [ProgramCapability]
    private sealed class MobileWangCanHost(GameEngine engine) : IMobileWangCanHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op) => engine.BeginMobileWangCanXi(frame, op); }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 81500, SkillProgramEffectOp.TransferMobileWangCanXi)]
    private bool CanOfferMobileWangCanMark(ProgramTriggerLegalityContext g) => g.SkillId == MobileWangCanShanxi && MobileWangCanXiTargets(g.OwnerSeat).Any();

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureHealthChange, 81500)]
    private void CaptureMobileWangCanRecovery(HpChangeContext change)
    {
        if (change.Kind != HpChangeKind.Recovery || change.Amount <= 0 || change.HpAfter <= 0 || !_players[change.TargetSeat].IsAlive ||
            _players[change.TargetSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) < 1) return;
        var mark = ProgramEventHistory<MobileWangCanXiMovedEvent>().LastOrDefault();
        if (mark is null || mark.TargetSeat != change.TargetSeat || !_players[mark.Source.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[mark.Source.OwnerSeat], MobileWangCanShanxi, mark.Source.SkillInstanceId)) return;
        foreach (var c in CollectProgramTriggerCandidates(_players[mark.Source.OwnerSeat], SkillProgramTriggerWindow.AfterHpRecovered)
            .Where(c => c.SkillId == MobileWangCanShanxi && c.BindingId == "marked-recovery" && c.SkillInstanceId == mark.Source.SkillInstanceId && c.GameplayHash == mark.Hash))
            AdvanceEventRulesAndQueueFact(new MobileWangCanXiQualifiedEvent(change.Id, new(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId), c.GameplayHash, change.TargetSeat));
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeHealthChangeCandidates, 81500)]
    private IReadOnlyList<ProgramTriggerCandidate> IncludeMobileWangCanRecovery(HpChangeContext change, IReadOnlyList<ProgramTriggerCandidate> ordinary) =>
        Array.AsReadOnly(ordinary.Where(c => c.SkillId != MobileWangCanShanxi).Concat(ProgramEventHistory<MobileWangCanXiQualifiedEvent>()
            .Where(e => e.ChangeId == change.Id).Select(e => new ProgramTriggerCandidate(e.Source.OwnerSeat, e.Source.SkillId, e.Source.BindingId, e.Source.SkillInstanceId, e.Hash, 0))).ToArray());
    private bool ExactMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) =>
        c.SkillId == MobileWangCanShanxi && c.BindingId == "marked-recovery" && c.OccurrenceIndex == 0 &&
        w is { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { Kind: HpChangeKind.Recovery, HpAfter: > 0 } change } &&
        w.OwnerSeat == c.OwnerSeat && w.ParentFrameId == change.Id && w.TargetSeat == change.TargetSeat && w.SourceSeat == change.SourceSeat && w.Amount == change.Amount &&
        _players[c.OwnerSeat].IsAlive && _players[change.TargetSeat].IsAlive && _players[change.TargetSeat].Hp > 0 &&
        HasRuntimeSkillInstance(_players[c.OwnerSeat], c.SkillId, c.SkillInstanceId) && _contentRegistry.GetSkill(c.SkillId).Program!.GameplayHash == c.GameplayHash &&
        ProgramEventHistory<MobileWangCanXiQualifiedEvent>().Count(e => e.ChangeId == change.Id && e.Source == new CardConversionSource(c.SkillId, c.BindingId, c.OwnerSeat, c.SkillInstanceId) && e.Hash == c.GameplayHash && e.TargetSeat == change.TargetSeat) == 1 &&
        _resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault() is { } hp && hp.Id == change.Id && hp.Change == change &&
        hp.CandidateIndex >= 0 && hp.CandidateIndex < hp.Candidates.Count && hp.Candidates[hp.CandidateIndex] == c;
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 81500)]
    private bool AdmitMobileWangCanRecovery(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => ExactMobileWangCanRecovery(c, w);

    private SkillProgramStepOutcome BeginMobileWangCanXi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != MobileWangCanShanxi || f.MobileWangCanXi is not null || f.InstructionIndex != 1 || !_players[f.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            throw new InvalidOperationException("Shanxi lost its exact active issuing source.");
        var marking = op == SkillProgramEffectOp.TransferMobileWangCanXi;
        if (marking ? f.TriggerId != "move-xi" || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, OwnerSeat: var owner } || owner != f.OwnerSeat || !MobileWangCanXiTargets(owner).Any() :
            f.WindowContext is not { } w || !ExactMobileWangCanRecovery(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 0), w))
            throw new InvalidOperationException("Shanxi lost its actual Play start or exact marked recovery event.");
        var r = new MobileWangCanXiReceipt(MobileWangCanSource(f), f.GameplayHash, marking ? 0 : f.WindowContext!.HpChange!.Id,
            marking ? -1 : f.WindowContext!.HpChange!.TargetSeat, marking ? MobileWangCanXiStage.Mark : MobileWangCanXiStage.Choice);
        ReplaceRuntimeTop(f = f with { MobileWangCanXi = r });
        AdvanceEventRulesAndQueueFact(new MobileWangCanXiIssuedEvent(f.Id, r.Source, r.Hash, r.ChangeId, r.Subject));
        ContinueMobileWangCanXi(f.Id); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> MobileWangCanXiCosts(ProgramSkillFrame f) => BuildOwnedCardPaymentChoices(f.Id, f.MobileWangCanXi!.Subject, f.MobileWangCanXi.Subject,
        [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer).Where(c => c.Cards.Count == 1).ToArray();
    private IReadOnlyList<PromptChoice> MobileWangCanXiChoices(ProgramSkillFrame f)
    {
        var r = f.MobileWangCanXi!; var choices = new List<PromptChoice>();
        void Add(string id, string label, string branch, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => choices.Add(new(new($"mobile-wang-can.{f.Id}.{id}"), label, cards, targets,
            new Dictionary<string, string> { ["program-action"] = "mobile-wang-can-xi", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch }));
        if (r.Stage == MobileWangCanXiStage.Mark)
            foreach (var target in MobileWangCanXiTargets(f.OwnerSeat)) Add("mark." + target, "将檄移给" + _players[target].Name, "mark", [], [target]);
        if (r.Stage == MobileWangCanXiStage.Choice)
        {
            if (_players[f.OwnerSeat].IsAlive && MobileWangCanXiCosts(f).Count >= 2) Add("gift", "交给王粲两张牌", "gift", [], []);
            Add("lose-hp", "失去1点体力", "lose-hp", [], []);
        }
        if (r.Stage == MobileWangCanXiStage.Cards)
        {
            foreach (var card in r.Cards) Add("unpick." + card, "取消已选牌", "unpick", [card], []);
            if (r.Cards.Count < 2) foreach (var card in MobileWangCanXiCosts(f).Where(c => !r.Cards.Contains(c.Cards.Single()))) Add("card." + card.Cards.Single(), card.Label, "card", card.Cards, []);
            if (r.Cards.Count == 2 && _players[f.OwnerSeat].IsAlive) Add("pay", "交出所选两张牌", "pay", [], []);
        }
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }
    private void FinishMobileWangCanXi(ProgramSkillFrame f)
    { ReplaceRuntimeTop(f with { MobileWangCanXi = null }); AdvanceRuntimeProgram(f.Id); }
    private void ContinueMobileWangCanXi(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileWangCanXi: { } r } f || f.Id != id) return;
        if (_winner != Winner.None || r.Subject >= 0 && !_players[r.Subject].IsAlive) { FinishMobileWangCanXi(f); return; }
        if (r.Stage is MobileWangCanXiStage.Mark or MobileWangCanXiStage.Choice or MobileWangCanXiStage.Cards)
        {
            var choices = MobileWangCanXiChoices(f); if (choices.Count == 0) { FinishMobileWangCanXi(f); return; }
            var chooser = r.Stage == MobileWangCanXiStage.Mark ? f.OwnerSeat : r.Subject; var skill = _contentRegistry.GetSkill(f.SkillId);
            _pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MobileWangCanXiStage.Mark ? "选择一名没有檄的其他角色。" : "善檄：交给王粲两张牌，或失去1点体力。",
                choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
            { PromptId = CreatePromptId(), IsPrivate = r.Stage == MobileWangCanXiStage.Cards, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
            _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState(); return;
        }
        if (TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) ||
            TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || TryBeginCardsMovedProgramWindow(id) || TryBeginAdvancedSkillsChanged(id)) return;
        f = GetActiveProgramFrame(id); r = f.MobileWangCanXi!;
        if (r.Stage is MobileWangCanXiStage.GiftChildren or MobileWangCanXiStage.HealthChildren) { FinishMobileWangCanXi(f); return; }
        if (r.Stage != MobileWangCanXiStage.Health) throw new InvalidOperationException("Shanxi lost its paid native child cursor.");
        if (HasHappyQiaoHealth(r.Subject) && TryBeginHappyQiaoProgramHealthChoice(f, r.Subject, HpChangeKind.Loss, 1)) return;
        var outcome = ApplyProgramHpLoss(id, f.SkillId, r.Subject, 1, (before, next, actual) =>
        {
            var current = GetActiveProgramFrame(id);
            ReplaceRuntimeTop(current with { MobileWangCanXi = current.MobileWangCanXi! with { Stage = MobileWangCanXiStage.HealthChildren, ActualCount = actual } });
            AdvanceEventRulesAndQueueFact(new MobileWangCanXiPaidEvent(id, r.Subject, false, actual, 0, 0));
            if (HasHappyQiaoHealth(r.Subject)) CompleteHappyQiaoHealthPayment(id, r.Subject, HpChangeKind.Loss, before, next);
        });
        if (outcome == SkillProgramStepOutcome.Continue) ContinueMobileWangCanXi(id);
    }
    [ProgramAnswer("mobile-wang-can-xi")]
    private void AnswerMobileWangCanXi(PromptChoice selected)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Shanxi answer lost its original frame."); var r = f.MobileWangCanXi!;
        if (selected.Parameters.GetValueOrDefault("frame-id") != f.Id.ToString(CultureInfo.InvariantCulture) ||
            !AssistedChoicesEqual([selected], MobileWangCanXiChoices(f).Where(c => c.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Shanxi payment or target is no longer legal.");
        ClearPendingDecision();
        switch (selected.Parameters["branch"])
        {
            case "mark":
                var target = selected.Targets.Single(); var marker = PlayerMarkerKind.MobileWangCanXi;
                foreach (var p in _players)
                {
                    foreach (var source in p.MarkerSourceCounts.Where(x => x.Key.Marker == marker).ToArray())
                    {
                        p.MarkerSourceCounts.Remove(source.Key); var remaining = p.Markers.GetValueOrDefault(marker) - source.Value;
                        if (remaining == 0) p.Markers.Remove(marker); else p.Markers[marker] = remaining;
                        AdvanceEventRulesAndQueueFact(new PlayerMarkerChangedEvent(f.Id, p.Seat, marker, -source.Value, remaining, source.Key.SkillOwnerSeat, "program.mobile-wang-can.xi-transfer"));
                    }
                }
                MutateParticipantMarker(f, target, marker, 1);
                AdvanceEventRulesAndQueueFact(new MobileWangCanXiMovedEvent(f.Id, r.Source, r.Hash, target)); FinishMobileWangCanXi(f); return;
            case "gift": ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.Cards } }); break;
            case "lose-hp": ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.Health } }); break;
            case "card": ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Cards = r.Cards.Append(selected.Cards.Single()).ToArray() } }); break;
            case "unpick": ReplaceRuntimeTop(f with { MobileWangCanXi = r with { Cards = r.Cards.Where(c => c != selected.Cards.Single()).ToArray() } }); break;
            case "pay":
                var available = MobileWangCanXiCosts(f).SelectMany(c => c.Cards).ToHashSet();
                if (r.Cards.Count != 2 || r.Cards.Distinct().Count() != 2 || r.Cards.Any(c => !available.Contains(c)) || !_players[f.OwnerSeat].IsAlive)
                    throw new InvalidOperationException("Shanxi requires two current actual HE cards and a living recipient.");
                var before = _movementSequence;
                ReplaceRuntimeTop(f = f with { MobileWangCanXi = r with { Stage = MobileWangCanXiStage.GiftChildren, Before = before, After = before } });
                MoveProgramCardsFromMultipleSources(r.Cards, CardLocation.Hand(f.OwnerSeat), new(MobileWangCanXiGift), (_, moves) =>
                {
                    var current = GetActiveProgramFrame(f.Id); var actual = moves.Count(m => m.To == CardLocation.Hand(f.OwnerSeat));
                    ReplaceRuntimeTop(current with { MobileWangCanXi = current.MobileWangCanXi! with { After = _movementSequence, ActualCount = actual } });
                    AdvanceEventRulesAndQueueFact(new MobileWangCanXiPaidEvent(f.Id, r.Subject, true, actual, before, _movementSequence));
                }); break;
            default: throw new InvalidOperationException("Unknown Shanxi branch.");
        }
        ContinueMobileWangCanXi(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 81500)]
    private bool ResumeMobileWangCanXi(long id)
    { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { MobileWangCanXi: not null } f || f.Id != id) return false; ContinueMobileWangCanXi(id); return true; }
    [ProgramAiChoice(SkillProgramEffectOp.TransferMobileWangCanXi, SkillProgramEffectOp.ResolveMobileWangCanXiRecovery)]
    private PromptChoice ChooseMobileWangCanXi(PendingDecision d, ProgramSkillFrame f) =>
        d.Choices.FirstOrDefault(c => c.Parameters["branch"] == "pay") ?? d.Choices.FirstOrDefault(c => c.Parameters["branch"] is "mark" or "gift" or "card") ?? d.Choices[0];
}
