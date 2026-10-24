namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryWurongGift = "skill-program.boundary-zhang-ni.wurong.gift";
    private static CardConversionSource BoundaryWurongSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramCapability]
    private sealed class BoundaryZhangNiHost(GameEngine engine) : IBoundaryZhangNiHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ConsumeBoundaryZhangNiDrawSkip ? engine.ConsumeBoundaryWurongSkip(f) : engine.BeginBoundaryWurong(f, e); }
    private SkillProgramStepOutcome BeginBoundaryWurong(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (e.StateId == "gifts") return BeginBoundaryWurongGift(f);
        if (f.BoundaryWurong is not null || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 ||
            f.ActivationId != BoundaryZhangNiComposition.Activation || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] ||
            target == f.OwnerSeat || !_players[target].IsAlive || CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Wurong requires one other living role in the original own actual Play activation.");
        var r = new BoundaryWurongReceipt(BoundaryWurongSource(f), f.GameplayHash, phase, target);
        ReplaceRuntimeTop(f = f with { BoundaryWurong = r });
        AdvanceEventRulesAndQueueFact(new BoundaryWurongStartedEvent(f.Id, r.Source, r.Hash, phase, target));
        PublishBoundaryWurong(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> BoundaryWurongChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!;
        PromptChoice Choice(string branch, string text, IReadOnlyList<int>? cards = null) => new(new($"boundary-wurong.{f.Id}.{r.Stage}.{branch}.{string.Join('-', cards ?? [])}"), text, cards ?? [], [],
            new Dictionary<string, string> { ["program-action"] = "boundary-wurong", ["branch"] = branch });
        if (r.Stage == BoundaryWurongStage.OwnerChoice) return [Choice("suppress", "镇压"), Choice("appease", "安抚")];
        if (r.Stage == BoundaryWurongStage.TargetChoice) return [Choice("resist", "反抗"), Choice("submit", "归顺")];
        var cards = GetHand(_players[r.Giver]).Concat(GetEquipment(_players[r.Giver])).ToArray(); var choices = new List<PromptChoice>();
        foreach (var card in cards.Where(c => r.Selected.Contains(c.Id))) choices.Add(Choice("unpick", "取消交出【" + card.DisplayName + "】", [card.Id]));
        if (r.Selected.Count < r.Required)
            foreach (var card in cards.Where(c => !r.Selected.Contains(c.Id))) choices.Add(Choice("card", "交出【" + card.DisplayName + "】", [card.Id]));
        if (r.Selected.Count == r.Required) choices.Add(Choice("pay", $"同时交给{_players[r.Recipient].Name}{r.Required}张牌"));
        return choices;
    }
    private void PublishBoundaryWurong(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!; var chooser = r.Stage == BoundaryWurongStage.OwnerChoice ? f.OwnerSeat : r.Stage == BoundaryWurongStage.TargetChoice ? r.Target : r.Giver;
        var choices = BoundaryWurongChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, chooser, r.Stage == BoundaryWurongStage.GiftChoice ? $"怃戎：选择{r.Required}张手牌或装备牌，确认后同时交出。" : "怃戎：秘密选择谋弈策略，双方提交后共同揭晓。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = chooser, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _pendingDecision = r.Stage == BoundaryWurongStage.GiftChoice
            ? NativeOwnedSelection(prompt, f, "wurong-gift", "branch", ["card"], ["pay"], r.Selected, r.Required - r.Selected.Count, r.Required - r.Selected.Count) : prompt;
        _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState();
    }
    [ProgramAnswer("boundary-wurong")]
    private void AnswerBoundaryWurong(PromptChoice choice)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Wurong lost its original private commitment frame.");
        AssertBoundaryWurong(f); var r = f.BoundaryWurong!;
        if (!BoundaryWurongChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets))) throw new InvalidOperationException("Wurong rejects a stale strategy or payment entity.");
        ClearPendingDecision(); var branch = choice.Parameters["branch"];
        if (r.Stage == BoundaryWurongStage.OwnerChoice)
        {
            ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Suppress = branch == "suppress", Stage = BoundaryWurongStage.TargetChoice } });
            // Only the existence of a commitment is public before reveal.
            AdvanceEventRulesAndQueueFact(new BoundaryWurongCommittedEvent(f.Id, f.OwnerSeat, true)); PublishBoundaryWurong(f); return;
        }
        if (r.Stage == BoundaryWurongStage.TargetChoice)
        {
            var resist = branch == "resist"; var outcome = (r.Suppress!.Value ? "suppress" : "appease") + (resist ? "-resist" : "-submit");
            ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Resist = resist, Outcome = outcome, Stage = BoundaryWurongStage.Revealed },
                ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(BoundaryZhangNiComposition.Result, outcome, f.OwnerSeat)).ToArray()) });
            AdvanceEventRulesAndQueueFact(new BoundaryWurongCommittedEvent(f.Id, r.Target, false));
            AdvanceEventRulesAndQueueFact(new BoundaryWurongRevealedEvent(f.Id, r.Suppress.Value, resist, outcome)); AdvanceRuntimeProgram(f.Id); return;
        }
        switch (branch)
        {
            case "card": ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Selected = [.. r.Selected, choice.Cards.Single()] } }); PublishBoundaryWurong(f); return;
            case "unpick": ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Selected = r.Selected.Where(id => id != choice.Cards.Single()).ToArray() } }); PublishBoundaryWurong(f); return;
            case "pay": PayBoundaryWurongGift(f); return;
            default: throw new InvalidOperationException("Unknown Wurong private commitment.");
        }
    }
    private SkillProgramStepOutcome BeginBoundaryWurongGift(ProgramSkillFrame f)
    {
        if (f.BoundaryWurong is not { Stage: BoundaryWurongStage.Revealed } r || f.InstructionIndex != 5) throw new InvalidOperationException("Wurong gifts require the original revealed consequence.");
        if (r.Resist == true || !_players[f.OwnerSeat].IsAlive || !_players[r.Target].IsAlive)
        { ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } }); return SkillProgramStepOutcome.Continue; }
        var giver = r.Suppress == true ? f.OwnerSeat : r.Target; var recipient = giver == f.OwnerSeat ? r.Target : f.OwnerSeat;
        var available = GetHand(_players[giver]).Count + GetEquipment(_players[giver]).Count;
        if (r.Suppress == false && available < 2)
        {
            AdvanceEventRulesAndQueueFact(new BoundaryWurongSkipIssuedEvent(f.Id, r.Source, r.Hash, r.Target));
            ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } }); return SkillProgramStepOutcome.Continue;
        }
        if (available == 0) { ReplaceRuntimeTop(f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete } }); return SkillProgramStepOutcome.Continue; }
        ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.GiftChoice, Giver = giver, Recipient = recipient, Required = Math.Min(2, available) } });
        PublishBoundaryWurong(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private void PayBoundaryWurongGift(ProgramSkillFrame f)
    {
        var r = f.BoundaryWurong!;
        if (r.Stage != BoundaryWurongStage.GiftChoice || r.Selected.Count != r.Required || r.Selected.Any(id => _cardZones.GetLocation(id) is not { OwnerSeat: { } holder, Zone: CardZoneKind.Hand or CardZoneKind.Equipment } || holder != r.Giver))
            throw new InvalidOperationException("Wurong must pay its complete current owned HE set once.");
        var before = _movementSequence;
        foreach (var p in _players) _observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        ReplaceRuntimeTop(f = f with { BoundaryWurong = r = r with { Stage = BoundaryWurongStage.GiftChildren, Before = before, After = before }, PendingMovementContinuation = new(r.Giver, 0, null) });
        foreach (var group in r.Selected.GroupBy(id => _cardZones.GetLocation(id)))
            MoveCards(group.Select(GetAdvancedCard).ToArray(), group.Key, CardLocation.Hand(r.Recipient), new(BoundaryWurongGift));
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { After = _movementSequence } });
        foreach (var movement in _cardMovements.Where(m => m.Sequence > before && m.Sequence <= _movementSequence && r.Selected.Contains(m.CardId) && m.From.OwnerSeat == r.Giver && m.To == CardLocation.Hand(r.Recipient) && m.Reason.Value == BoundaryWurongGift).ToArray())
            AdvanceEventRulesAndQueueFact(new BoundaryWurongGiftEntityEvent(f.Id, movement.CardId, movement.From, movement.To, movement.Sequence));
        AdvanceEventRulesAndQueueFact(new BoundaryWurongGiftPaidEvent(f.Id, r.Giver, r.Recipient, r.Selected.Count, before, _movementSequence)); ContinueBoundaryWurongGift(f.Id);
    }
    private void ContinueBoundaryWurongGift(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryWurong: { Stage: BoundaryWurongStage.GiftChildren } r } f || f.Id != id) return;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(f.Id) ||
            TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return;
        ReplaceRuntimeTop(f = f with { BoundaryWurong = r with { Stage = BoundaryWurongStage.Complete }, PendingMovementContinuation = null }); AdvanceRuntimeProgram(f.Id);
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 54200)]
    private bool ResumeBoundaryWurong(long id) { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryWurong.Stage: BoundaryWurongStage.GiftChildren } f || f.Id != id) return false; ContinueBoundaryWurongGift(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 54200)]
    private bool ReturnBoundaryWurongMovement(ProgramSkillFrame f) { if (f.BoundaryWurong?.Stage != BoundaryWurongStage.GiftChildren) return false; ContinueBoundaryWurongGift(f.Id); return true; }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54200)]
    private bool KeepBoundaryWurongPaid(ProgramSkillFrame f) => f.BoundaryWurong is { Stage: >= BoundaryWurongStage.Revealed } ||
        f.TriggerId == BoundaryZhangNiComposition.Skip && f.InstructionIndex is 0 or 1 && f.WindowContext is { } context &&
        AdmitBoundaryWurongSkip(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 1000, context.OccurrenceIndex), context);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryZhangNiWurong)]
    private static PromptChoice BoundaryWurongAi(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "pay") ?? p.Choices.First();
    private bool IsBoundaryWurongChoiceResult(ProgramSkillFrame f, ProgramChoiceResultBinding b) => f.SkillId == BoundaryZhangNiComposition.Skill && f.BoundaryWurong is { Stage: >= BoundaryWurongStage.Revealed, Outcome: { } outcome } r &&
        b == new ProgramChoiceResultBinding(BoundaryZhangNiComposition.Result, outcome, f.OwnerSeat) && ProgramEventHistory<BoundaryWurongRevealedEvent>().Where(e => e.FrameId == f.Id).ToArray() is [var revealed] &&
        revealed == new BoundaryWurongRevealedEvent(f.Id, r.Suppress!.Value, r.Resist!.Value, outcome);

    private BoundaryWurongSkipIssuedEvent[] PendingBoundaryWurongSkips(int target) => ProgramEventHistory<BoundaryWurongSkipIssuedEvent>().Where(e => e.Target == target &&
        _contentRegistry.Skills.GetValueOrDefault(e.Source.SkillId)?.Program?.GameplayHash == e.Hash && !ProgramEventHistory<BoundaryWurongSkipConsumedEvent>().Any(c => c.IssuerFrameId == e.FrameId)).ToArray();
    private bool HasPendingBoundaryWurongDrawSkip(int target) => PendingBoundaryWurongSkips(target).Length > 0;
    private static string BoundaryWurongSkipInstance(BoundaryWurongSkipIssuedEvent e) => "issued:boundary-wurong-skip:" + e.FrameId;
    [ProgramPipelineHook(ProgramPipelinePhase.IncludeTriggerCandidate, 54200)]
    private IReadOnlyList<ProgramTriggerCandidate> IncludeBoundaryWurongSkip(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> original)
    {
        if (window != SkillProgramTriggerWindow.DrawPhaseStarting || !_contentRegistry.Skills.ContainsKey(BoundaryZhangNiComposition.Skill)) return original;
        var rest = original.Where(c => c.SkillId != BoundaryZhangNiComposition.Skill || c.BindingId != BoundaryZhangNiComposition.Skip).ToArray(); var debt = PendingBoundaryWurongSkips(owner.Seat).FirstOrDefault();
        return debt is null ? rest : [.. rest, new ProgramTriggerCandidate(owner.Seat, BoundaryZhangNiComposition.Skill, BoundaryZhangNiComposition.Skip, BoundaryWurongSkipInstance(debt), debt.Hash, 1000)];
    }
    [ProgramPipelineHook(ProgramPipelinePhase.IssuedTriggerCandidate, 54200)]
    private bool AdmitBoundaryWurongSkip(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BoundaryZhangNiComposition.Skill || candidate.BindingId != BoundaryZhangNiComposition.Skip || context.Window != SkillProgramTriggerWindow.DrawPhaseStarting ||
            context.OwnerSeat != candidate.OwnerSeat || _resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId) is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting } parent ||
            parent.OwnerSeat != candidate.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate) return false;
        return ProgramEventHistory<BoundaryWurongSkipIssuedEvent>().Any(e => e.Target == candidate.OwnerSeat && e.Hash == candidate.GameplayHash && BoundaryWurongSkipInstance(e) == candidate.SkillInstanceId &&
            !ProgramEventHistory<BoundaryWurongSkipConsumedEvent>().Any(c => c.IssuerFrameId == e.FrameId && c.DrawWindowId != parent.Id));
    }
    private SkillProgramStepOutcome ConsumeBoundaryWurongSkip(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { } context || !AdmitBoundaryWurongSkip(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, 1000), context)) throw new InvalidOperationException("Wurong skip requires its issued next actual Draw window.");
        foreach (var debt in PendingBoundaryWurongSkips(f.OwnerSeat)) AdvanceEventRulesAndQueueFact(new BoundaryWurongSkipConsumedEvent(debt.FrameId, f.Id, context.ParentFrameId, f.OwnerSeat, _turnNumber));
        var parent = _resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Single(p => p.Id == context.ParentFrameId);
        if (parent.ActualPhaseSubstitution is null)
        {
            ReplaceRuntimeFrame(parent.Id, parent with { ActualPhaseSubstitution = new(parent.Id, f.Id, f.OwnerSeat, f.SkillId, f.SkillInstanceId, _turnNumber, SkillProgramTurnPhase.Draw) });
            AdvanceEventRulesAndQueueFact(new ProgramTurnPhaseSubstitutedEvent(f.Id, f.SkillId, GetProgramBindingId(f), f.SkillInstanceId, f.OwnerSeat, _turnNumber, context.Window, SkillProgramTurnPhase.Draw));
        }
        return SkipProgramTurnPhases(f, [SkillProgramTurnPhase.Draw]);
    }
}
