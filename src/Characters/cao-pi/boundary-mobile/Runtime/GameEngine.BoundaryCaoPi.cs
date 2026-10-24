namespace CardGame.Core;
public sealed partial class GameEngine
{
    private const string BoundaryFangzhuDiscard = "skill-program.boundary-cao-pi.fangzhu.discard", BoundaryFangzhuDraw = "skill-program.boundary-cao-pi.fangzhu.draw";
    private static CardConversionSource BoundaryCaoPiSource(ProgramSkillFrame f) => new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    [ProgramCapability]
    private sealed class BoundaryCaoPiHost(GameEngine engine) : IBoundaryCaoPiHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e) => e.Op == SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang ? engine.BeginBoundaryXingshang(f, e) : engine.BeginBoundaryFangzhu(f, e); }
    private DeathFrame? BoundaryCaoPiDeath(ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CharacterDied || _resolutionStack.OfType<ProgramKillTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is not { } window) return null;
        return _resolutionStack.OfType<DeathFrame>().SingleOrDefault(d => d.Id == window.DeathFrameId && d.VictimSeat != context.OwnerSeat);
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 54500)]
    private bool QualifyBoundaryCaoPi(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId == BoundaryCaoPiComposition.Xingshang)
            return BoundaryCaoPiDeath(context) is { } death && (IsRuleWounded(_players[c.OwnerSeat]) || QueryProgramDeathCleanupCards(death, [CardZoneKind.Hand, CardZoneKind.Equipment]).Length > 0);
        if (c.SkillId == BoundaryCaoPiComposition.Fangzhu) return _players[c.OwnerSeat].HasHpValue;
        if (c.SkillId != BoundaryCaoPiComposition.Songwei) return true;
        return context is { Window: SkillProgramTriggerWindow.JudgmentFinalized, Judgment: { } j } && j.SubjectSeat != c.OwnerSeat &&
            IsValidPlayerSeat(j.SubjectSeat) && _players[j.SubjectSeat].IsAlive && GetEffectiveFactionId(_players[j.SubjectSeat]) == "wei" && j.Suit is Suit.Spade or Suit.Club &&
            _resolutionStack.OfType<ProgramJudgmentTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is { } window && window.Judgment == j &&
            window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && window.Candidates[window.CandidateIndex] is { } candidate &&
            candidate.OwnerSeat == c.OwnerSeat && candidate.SkillId == c.SkillId && candidate.SkillInstanceId == c.SkillInstanceId && candidate.GameplayHash == c.GameplayHash;
    }
    private SkillProgramStepOutcome BeginBoundaryXingshang(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.BoundaryXingshang is not null || f.InstructionIndex != 1 || f.WindowContext is not { } context || BoundaryCaoPiDeath(context) is not { } death) throw new InvalidOperationException("Xingshang requires the original other role's completed native cleanup.");
        ReplaceRuntimeTop(f = f with { BoundaryXingshang = new(BoundaryCaoPiSource(f), f.GameplayHash, context.ParentFrameId, death.Id, death.VictimSeat, e.ResultBind!) });
        PublishBoundaryCaoPi(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private SkillProgramStepOutcome BeginBoundaryFangzhu(ProgramSkillFrame input, SkillProgramEffect e)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.BoundaryFangzhu is not null || f.InstructionIndex != 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } context ||
            f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_players[target].IsAlive || UnambiguousHp(_players[f.OwnerSeat]) is not { } hp)
            throw new InvalidOperationException("Fangzhu requires the injured owner's original damage frame and distinct actual recipient.");
        var x = Math.Max(0, _players[f.OwnerSeat].MaxHp - hp); var r = new BoundaryFangzhuReceipt(BoundaryCaoPiSource(f), f.GameplayHash, context.ParentFrameId, context.DamageFrameId, target, x, e.ResultBind!);
        ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r });
        AdvanceEventRulesAndQueueFact(new BoundaryFangzhuStartedEvent(f.Id, r.Source, r.Hash, r.DamageWindow, r.DamageId, target, x, e.ResultBind!));
        PublishBoundaryCaoPi(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private Card[] BoundaryFangzhuMaterials(int target) => GetHand(_players[target]).Concat(GetEquipment(_players[target]))
        .Where(c => !IsSelfHandCategoryDiscardForbidden(target, c, _cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    private IReadOnlyList<PromptChoice> BoundaryCaoPiChoices(ProgramSkillFrame f)
    {
        PromptChoice Choice(string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"boundary-cao-pi.{f.Id}.{branch}.{string.Join('-', cards ?? [])}"), label, cards ?? [], [],
            new Dictionary<string, string> { ["program-action"] = "boundary-cao-pi", ["branch"] = branch });
        var choices = new List<PromptChoice>();
        if (f.BoundaryXingshang is { } x)
        {
            var death = _resolutionStack.OfType<DeathFrame>().Single(d => d.Id == x.DeathId);
            if (QueryProgramDeathCleanupCards(death, [CardZoneKind.Hand, CardZoneKind.Equipment]).Length > 0) choices.Add(Choice("claim", "获得其所有手牌和装备牌"));
            if (IsRuleWounded(_players[f.OwnerSeat])) choices.Add(Choice("recover", "回复1点体力"));
            return choices;
        }
        var r = f.BoundaryFangzhu!; var cards = BoundaryFangzhuMaterials(r.Target);
        foreach (var card in cards.Where(c => r.Selected.Contains(c.Id))) choices.Add(Choice("unpick", "取消弃置【" + card.DisplayName + "】", [card.Id]));
        if (cards.Length >= r.X)
        {
            if (r.Selected.Count < r.X) foreach (var card in cards.Where(c => !r.Selected.Contains(c.Id))) choices.Add(Choice("card", "选择弃置【" + card.DisplayName + "】", [card.Id]));
            else choices.Add(Choice("discard", $"同时弃置{r.X}张牌并失去1点体力"));
        }
        choices.Add(Choice("draw", $"摸{r.X}张牌，然后翻面")); return choices;
    }
    private void PublishBoundaryCaoPi(ProgramSkillFrame f)
    {
        var chooser = f.BoundaryFangzhu?.Target ?? f.OwnerSeat; var choices = BoundaryCaoPiChoices(f); var skill = _contentRegistry.GetSkill(f.SkillId);
        var prompt = new PendingDecision(DecisionKind.ProgramTrigger, chooser, f.BoundaryFangzhu is { } r ? $"放逐：由你选择摸{r.X}张牌后翻面，或弃置等量手牌/装备并失去1点体力。" : "行殇：获得该角色所有手牌和装备，或回复1点体力。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = chooser, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) };
        _pendingDecision = f.BoundaryFangzhu is { } cost ? NativeOwnedSelection(prompt, f, "fangzhu-target", "branch", ["card"], ["discard"], cost.Selected, cost.X - cost.Selected.Count, cost.X - cost.Selected.Count) : prompt;
        _status = _players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running; AdvanceRulesAndPublishState();
    }
    [ProgramAnswer("boundary-cao-pi")]
    private void AnswerBoundaryCaoPi(PromptChoice choice)
    {
        var f = _resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Cao Pi choice lost its original trigger frame.");
        AssertBoundaryCaoPi(f);
        if (!BoundaryCaoPiChoices(f).Any(c => c.Id == choice.Id && c.Cards.SequenceEqual(choice.Cards) && c.Targets.SequenceEqual(choice.Targets))) throw new InvalidOperationException("Cao Pi rejects an unrelated strategy or stale payment entity.");
        var branch = choice.Parameters["branch"]; ClearPendingDecision();
        if (f.BoundaryXingshang is { } corpse)
        {
            ReplaceRuntimeTop(f = f with { BoundaryXingshang = null });
            CommitBoundaryCaoPiResult(f, corpse.ResultBind, branch, f.OwnerSeat, corpse.Victim, 0); AdvanceRuntimeProgram(f.Id); return;
        }
        var r = f.BoundaryFangzhu!;
        if (branch is "card" or "unpick")
        {
            var selected = branch == "card" ? r.Selected.Append(choice.Cards.Single()).ToArray() : r.Selected.Where(id => id != choice.Cards.Single()).ToArray();
            ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Selected = selected } }); PublishBoundaryCaoPi(f); return;
        }
        var before = _movementSequence;
        if (branch == "draw")
        {
            ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.DrawRequest, Outcome = "drawn", Before = before, After = before } });
            BeginOperationProgramDraw(f, r.Target, r.X, new(BoundaryFangzhuDraw)); return;
        }
        if (branch != "discard" || r.Selected.Count != r.X) throw new InvalidOperationException("Fangzhu must choose its full exact HE cost or its frozen-X draw.");
        foreach (var p in _players) _observedSkillGrantRevisions.TryAdd(p.Seat, p.SkillGrants.Revision);
        ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r = r with { Stage = BoundaryFangzhuStage.DiscardChildren, Outcome = "discarded", Before = before, After = before }, PendingMovementContinuation = new(r.Target, 0, null) });
        MoveProgramLogicalDiscard(f, r.Target, r.Selected, new(BoundaryFangzhuDiscard));
        var invoice = ProgramEventHistory<NativeLogicalDiscardInvocationRecordedEvent>().SingleOrDefault(e => e.ParentFrameId == f.Id && e.Reason.Value == BoundaryFangzhuDiscard && e.SequenceBefore == before);
        f = GetActiveProgramFrame(f.Id); ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { After = _movementSequence, DiscardInvocation = invoice?.InvocationId, Actual = invoice?.ActualCount ?? 0 } });
        AdvanceEventRulesAndQueueFact(new BoundaryFangzhuPaidEvent(f.Id, r.Target, r.X, invoice?.InvocationId, invoice?.ActualCount ?? 0, before, _movementSequence)); ContinueBoundaryFangzhu(f.Id);
    }
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu)]
    private void ReturnBoundaryFangzhuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.BoundaryFangzhu is not { Stage: BoundaryFangzhuStage.DrawRequest } r || outcome.RecipientSeat != r.Target || outcome.RequestedCount != r.X || outcome.ActualDrawIds.Count > r.X ||
            outcome.Replacement is not null && outcome.ActualDrawIds.Count != 0) throw new InvalidOperationException("Fangzhu draw lost its original frozen-X request and exact typed return.");
        ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.DrawChildren, Before = outcome.SequenceBefore, After = outcome.SequenceAfter,
            Actual = outcome.ActualDrawIds.Count, DrawAttempt = outcome.AttemptFrameId, DrawReplaced = outcome.Replacement is not null }, PendingMovementContinuation = new(r.Target, 0, null) });
        AdvanceEventRulesAndQueueFact(new BoundaryFangzhuDrawReturnedEvent(f.Id, r.Target, r.X, outcome.ActualDrawIds.Count, outcome.AttemptFrameId, outcome.Replacement is not null, outcome.SequenceBefore, outcome.SequenceAfter));
        ContinueBoundaryFangzhu(f.Id);
    }
    private void ContinueBoundaryFangzhu(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryFangzhu: { } r } f || f.Id != id || r.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren)) return;
        if (TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || TryBeginAdvancedSkillsChanged(f.Id) ||
            TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || TryBeginCardsMovedProgramWindow(f.Id)) return;
        ReplaceRuntimeTop(f = f with { BoundaryFangzhu = r with { Stage = BoundaryFangzhuStage.Complete }, PendingMovementContinuation = null });
        if (r.Outcome == "discarded" && r.Actual != r.X) { FinishProgramSkill(f, false); return; }
        CommitBoundaryCaoPiResult(f, r.ResultBind, r.Outcome!, r.Target, r.Target, r.X); AdvanceRuntimeProgram(f.Id);
    }
    private void CommitBoundaryCaoPiResult(ProgramSkillFrame input, string bind, string outcome, int chooser, int subject, int count)
    {
        var f = GetActiveProgramFrame(input.Id);
        ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(bind, outcome, chooser)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new BoundaryCaoPiChoiceResultEvent(f.Id, f.SkillId, bind, outcome, chooser, subject, count));
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 54500)]
    private bool ResumeBoundaryFangzhu(long id) { if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { BoundaryFangzhu.Stage: BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren } f || f.Id != id) return false; ContinueBoundaryFangzhu(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 54500)]
    private bool ReturnBoundaryFangzhuMovement(ProgramSkillFrame f) { if (f.BoundaryFangzhu?.Stage is not (BoundaryFangzhuStage.DiscardChildren or BoundaryFangzhuStage.DrawChildren)) return false; ContinueBoundaryFangzhu(f.Id); return true; }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 54500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 54500)]
    private bool KeepBoundaryFangzhuPaid(ProgramSkillFrame f) => f.BoundaryFangzhu is { Stage: not BoundaryFangzhuStage.Choice };
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu, SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang)]
    private static PromptChoice BoundaryCaoPiAi(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "discard") ??
        p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "card") ?? p.Choices.First();
    private bool IsBoundaryCaoPiChoiceResult(ProgramSkillFrame f, ProgramChoiceResultBinding b) => f.SkillId is BoundaryCaoPiComposition.Xingshang or BoundaryCaoPiComposition.Fangzhu &&
        ProgramEventHistory<BoundaryCaoPiChoiceResultEvent>().Where(e => e.FrameId == f.Id && e.Bind == b.Name).ToArray() is [var e] && e.SkillId == f.SkillId && b == new ProgramChoiceResultBinding(e.Bind, e.Outcome, e.Chooser) &&
        (f.SkillId == BoundaryCaoPiComposition.Xingshang ? b.Name == "xingshang-result" && b.OptionId is "claim" or "recover" && b.ChooserSeat == f.OwnerSeat && f.WindowContext is { } c && BoundaryCaoPiDeath(c)?.VictimSeat == e.Subject :
            f.BoundaryFangzhu is { Stage: BoundaryFangzhuStage.Complete } r && b.Name == r.ResultBind && b.OptionId == r.Outcome && b.ChooserSeat == r.Target && e.Subject == r.Target && e.Count == r.X);
}
