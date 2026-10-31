namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
internal sealed class YangFuRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IYangFuFacts facts, IYangFuTargetFacts targets,
    IYangFuAdjacencyFacts adjacency, IYangFuOperations operations) : IYangFuHost
{
    private const string BingzhongDrawReason = "skill-program.yang-fu.bingzhong.draw",SuyiEquipmentBind = "suyi-equipment";
    private static CardConversionSource YangFuSource(ProgramSkillFrame f) =>
        global::CardGame.Core.RuntimeSkills.RuntimeRules_yang_fu_ol_YangFu.YangFuSource(f);
    private YangFuReceipt NewYangFuReceipt(ProgramSkillFrame f, SkillProgramEffectOp op, YangFuStage stage, int target, int x,
        int? ordinal = null, CardUseFrame? use = null) => new(YangFuSource(f), f.GameplayHash, op, facts.TurnNumber, f.WindowContext?.ParentFrameId ?? 0,
            target, x, ordinal, use?.Id, use?.Action?.ActionId, stage, use?.TargetSeats ?? [], [], facts.MovementSequence, facts.MovementSequence);
    private void StartYangFu(ProgramSkillFrame f, YangFuReceipt r)
    {
        if (f.YangFu is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0) throw new InvalidOperationException("Yang Fu cannot replace an issued owning receipt.");
        frames.ReplaceTop(f with { YangFu = r });
        events.AdvanceFact(new YangFuStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActualTurn, r.ParentId, r.TargetSeat, r.X, r.Ordinal, r.UseId, r.ActionId, r.OriginalTargets.Count));
        for (var i = 0; i < r.OriginalTargets.Count; i++) events.AdvanceFact(new BingzhongOriginalTargetEvent(f.Id, i, r.OriginalTargets[i]));
    }
    private bool DrainYangFuChildren(long id) => operations.BeginCharacterStateChildren(id) ||
        operations.BeginHpChildren(id) || operations.BeginMovementChildren(id) || operations.BeginSkillChildren(id);
    [ProgramContinuation(ProgramContinuationKind.Resume, 27310)]
    private bool ResumeYangFu(long id)
    {
        if (frames.Top is not ProgramSkillFrame { YangFu: { } r } f || f.Id != id) return false;
        if (r.Stage is YangFuStage.Targets or YangFuStage.EquipmentUse or YangFuStage.SlashUse) return true;
        if (DrainYangFuChildren(id)) return true;
        f = frames.ActiveProgram(id); r = f.YangFu!;
        if (r.Stage != YangFuStage.EquipmentChildren || facts.Winner != Winner.None || !facts.Player(f.OwnerSeat).IsAlive || !facts.Player(r.TargetSeat).IsAlive ||
            !targets.CanUseVirtualSlash(f.OwnerSeat, r.TargetSeat) || targets.DirectedTargetProhibited(f.OwnerSeat, r.TargetSeat, CardKind.Slash) ||
            targets.TargetProhibited(r.TargetSeat, CardKind.Slash, Suit.None, null)) { FinishYangFu(f); return true; }
        frames.ReplaceTop(f = f with { YangFu = r with { Stage = YangFuStage.SlashUse, NativeOrigin = null, NativeCompleted = false, Before = facts.MovementSequence, After = facts.MovementSequence }, PendingMovementContinuation = null });
        _ = operations.UseVirtualSlash(f, r.TargetSeat); return true;
    }
    private void FinishYangFu(ProgramSkillFrame f)
    {
        events.AdvanceFact(new YangFuCompletedEvent(f.Id));
        frames.ReplaceTop(f = f with { YangFu = null, PendingMovementContinuation = null }); operations.Finish(f, true);
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 27310)]
    private bool ReturnYangFuMovement(ProgramSkillFrame f) => f.YangFu is not null && ResumeYangFu(f.Id);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => op switch
    {
        SkillProgramEffectOp.ResolveBingzhongExtraTargets => BeginBingzhongTargets(f),
        SkillProgramEffectOp.ResolveBingzhongAfterDamageDraw => BeginBingzhongDraw(f),
        SkillProgramEffectOp.ResolveSuyiEquipmentThenSlash => BeginSuyi(f),
        _ => throw new InvalidOperationException("Unknown Yang Fu native operation.")
    };

    private int BingzhongCurrentX() => checked(1 + facts.Players.Count(p => p.IsAlive && facts.HandCount(p.Seat) == 0));
    private SkillProgramStepOutcome BeginBingzhongDraw(ProgramSkillFrame supplied)
    {
        var f = frames.ActiveProgram(supplied.Id);
        if (f.SkillId != "ol:bingzhong" || f.TriggerId != "actual-damage-dynamic-x" || f.SelectedTargetSeats.Count != 0 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0, DamageFrameId: not null } c || c.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Bingzhong requires its true whole positive damage occurrence.");
        var r = NewYangFuReceipt(f, SkillProgramEffectOp.ResolveBingzhongAfterDamageDraw, YangFuStage.DrawChildren, f.OwnerSeat, BingzhongCurrentX());
        StartYangFu(f, r); f = frames.ActiveProgram(f.Id);
        frames.ReplaceTop(f with { PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var count = operations.DrawCards(f.OwnerSeat, r.X, new(BingzhongDrawReason));
        f = frames.ActiveProgram(f.Id); frames.ReplaceTop(f with { YangFu = f.YangFu! with { After = facts.MovementSequence, DrawCount = count } });
        events.AdvanceFact(new BingzhongDrawPaidEvent(f.Id, r.X, count, r.Before, facts.MovementSequence));
        frames.AdvanceProgram(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }

    private SkillProgramStepOutcome BeginSuyi(ProgramSkillFrame supplied)
    {
        var f = frames.ActiveProgram(supplied.Id);
        if (f.SkillId != "ol:suyi" || f.TriggerId is not null || f.WindowContext is not null || f.OwnerSeat != facts.CurrentSeat || facts.Phase != TurnPhase.Play ||
            f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !facts.Player(target).IsAlive || !facts.Player(f.OwnerSeat).IsAlive)
            throw new InvalidOperationException("Suyi requires its actual one-other-target native Play activation.");
        StartYangFu(f, NewYangFuReceipt(f, SkillProgramEffectOp.ResolveSuyiEquipmentThenSlash, YangFuStage.EquipmentUse, target, 0));
        f = frames.ActiveProgram(f.Id);
        var outcome = operations.UseDeckEquipment(f, target, SuyiEquipmentBind);
        if (outcome == SkillProgramStepOutcome.Continue)
        {
            f = frames.ActiveProgram(f.Id); frames.ReplaceTop(f with { YangFu = f.YangFu! with { Stage = YangFuStage.EquipmentChildren }, PendingMovementContinuation = new(target, 0, null) });
            events.AdvanceFact(new SuyiNoEquipmentEvent(f.Id, target)); frames.AdvanceProgram(f.Id);
        }
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 130)]
    private bool BingzhongNormalizeImplicitDrawTwoTargetWindow(ProgramTargetCardWindowQuery query) =>
        facts.HasTriggerOperation(SkillProgramEffectOp.ResolveBingzhongExtraTargets);


    private bool TryGetBingzhongUse(int owner, ProgramSkillWindowContext context, out CardUseFrame use, out ProgramCardTriggerWindowFrame window)
    {
        if (targets.TryGetDesignatedExtraTargetUse(owner, context, out use, out window)) return true;
        use = null!; window = null!;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity } ||
            !facts.ValidSeat(owner) || !facts.Player(owner).IsAlive || facts.Winner != Winner.None ||
            frames.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId) is not { Continuation: ProgramCardContinuation.Slash, CompletedResponseReturn: null } w ||
            facts.CardUse(identity.ParentCardUseFrameId) is not { Action: { Type: CardActionType.Use } a } u || u.SourceSeat != owner || a.ActorSeat != owner ||
            !RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(u.CardKind) || a.ActionId != identity.CardActionId || w.ParentFrameId != u.Id || w.Action.ActionId != a.ActionId || w.Action.ActorSeat != owner ||
            !w.Action.TargetSeats.SequenceEqual(u.TargetSeats) || !a.TargetSeats.SequenceEqual(u.TargetSeats) || u.DyingResponse is not null ||
            a.ResponderSeat is not null || a.RequesterSeat is not null || a.OpponentSeat is not null || a.PhysicalCards.Count == 0 ||
            !a.PhysicalCards.Select(c => c.CardId).SequenceEqual(u.PhysicalCardIds ?? [u.CardId]) ||
            !a.PhysicalCards.Any(c => targets.IsKenshangRemovedMaterial(u.Id, c.CardId) || targets.IsWuJingHejiRemovedMaterial(u.Id, c.CardId) || targets.IsSelectedForeignCardSlashRemovedMaterial(u.Id, c.CardId) || targets.IsZhangYanSujiRemovedMaterial(u.Id, c.CardId)) ||
            a.PhysicalCards.Any(c => facts.Location(c.CardId) != CardLocation.Processing && !targets.IsKenshangRemovedMaterial(u.Id, c.CardId) && !targets.IsWuJingHejiRemovedMaterial(u.Id, c.CardId) &&
                !targets.IsSelectedForeignCardSlashRemovedMaterial(u.Id, c.CardId) && !targets.IsZhangYanSujiRemovedMaterial(u.Id, c.CardId) && !targets.IsCurrentUsePhysicalCardClaim(u.Id, c.CardId) && !targets.IsExchangedCardClaim(a.ActionId, c.CardId))) return false;
        if (frames.ParentOf(w.Id)?.Id != u.Id) return false;
        use = u; window = w; return true;
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27310)]
    private bool QualifyBingzhongTargets(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveBingzhongExtraTargets)) return true;
        return candidate.SkillId == "ol:bingzhong" && TryGetBingzhongUse(candidate.OwnerSeat, context, out var use, out var window) &&
            targets.DesignatedExtraTargetCandidateMatches(candidate, context, window) && use.BingzhongTail is null &&
            targets.CapturedWholeActualTurnActorUseOrdinal(use.Action!) is { } ordinal && ordinal <= BingzhongCurrentX();
    }
    private CardUseFrame RequireBingzhongUse(ProgramSkillFrame f, out ProgramCardTriggerWindowFrame window)
    {
        if (f.SkillId != "ol:bingzhong" || f.TriggerId != "first-dynamic-x-targets" || f.SelectedTargetSeats.Count != 0 || f.WindowContext is not { } context ||
            !TryGetBingzhongUse(f.OwnerSeat, context, out var use, out window) ||
            !targets.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0), context, window))
            throw new InvalidOperationException("Bingzhong lost its exact finalized actual actor use.");
        if (frames.ParentOf(f.Id)?.Id != window.Id || frames.ParentOf(window.Id)?.Id != use.Id)
            throw new InvalidOperationException("Bingzhong requires its immediate native target window.");
        return use;
    }
    internal IReadOnlyList<IReadOnlyList<int>> BingzhongLegalTargets(CardUseFrame use)
    {
        var action = use.Action!; var actor = facts.Player(action.ActorSeat); var physical = action.PhysicalCards.Select(c => c.CardId).ToArray();
        var result = new List<IReadOnlyList<int>>();
        foreach (var target in facts.Players.OrderBy(p => p.Seat))
        {
            if (use.CardKind == CardKind.UnexpectedAssault)
            {
                if (!target.IsAlive || target.Seat == actor.Seat || use.TargetSeats.Contains(target.Seat) || facts.HandCount(target.Seat) == 0 ||
                    targets.DirectedTargetProhibited(actor.Seat, target.Seat, use.CardKind) || targets.TargetProhibited(target.Seat, use.CardKind, action.EffectiveSuit ?? Suit.None, RuntimeSkills.RuntimeRules_Core_DyingWindowSkillPolicies.ActualTargetPolicyColor(action)) ||
                    targets.BeneficiarySuitShield(actor.Seat, target.Seat, action.EffectiveSuit) || targets.SelfTargetForbidden(actor.Seat, use.CardKind, [target.Seat])) continue;
            }
            else if (!targets.CanBeDistanceFreeDesignatedTarget(use, target.Seat)) continue;
            if (RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(use.CardKind) && !targets.CanUsePhysicalSlash(actor.Seat, target.Seat, new(-1, use.CardKind, action.EffectiveSuit ?? Suit.None, action.EffectiveRank ?? 0),
                action.ConversionChain.FirstOrDefault(), use.CardKind, use.Id, action.EffectiveRank, physical)) continue;
            if (use.CardKind == CardKind.Snatch && targets.CombatDistance(actor.Seat, target.Seat) != 1 &&
                !targets.CardDistanceExemption(actor.Seat, target.Seat, CardKind.Snatch, use.Id) && !targets.CardPolicy(actor.Seat, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.Snatch) &&
                !targets.HasIssuedFuRongDistance(use.Id, actor.Seat) && !targets.HasIssuedProvenanceUseDistance(use.Id, actor.Seat) && !targets.HasIssuedGrantedPhaseEntityDistance(use.Id, actor.Seat) &&
                !targets.HasPhaseSuitAllowance(actor.Seat, action.EffectiveSuit) && !targets.FirstRedAvailable(actor.Seat, action.EffectiveSuit)) continue;
            if (use.CardKind == CardKind.BorrowedSword)
                foreach (var victim in facts.Players.Where(v => targets.BorrowedSwordTarget(target.Seat, v.Seat)).OrderBy(v => v.Seat)) result.Add(Array.AsReadOnly(new[] { target.Seat, victim.Seat }));
            else result.Add(Array.AsReadOnly(new[] { target.Seat }));
        }
        return Array.AsReadOnly(result.ToArray());
    }
    private SkillProgramStepOutcome BeginBingzhongTargets(ProgramSkillFrame supplied)
    {
        var f = frames.ActiveProgram(supplied.Id); var use = RequireBingzhongUse(f, out _);
        var ordinal = targets.CapturedWholeActualTurnActorUseOrdinal(use.Action!) ?? throw new InvalidOperationException("Bingzhong requires its frozen actual-use ordinal.");
        var x = BingzhongCurrentX();
        StartYangFu(f, NewYangFuReceipt(f, SkillProgramEffectOp.ResolveBingzhongExtraTargets, YangFuStage.Targets, f.OwnerSeat, x, ordinal, use));
        f = frames.ActiveProgram(f.Id);
        if (ordinal > x || BingzhongChoices(f).Count == 1) CompleteBingzhongTargets(f);
        else PublishBingzhongChoices(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> BingzhongChoices(ProgramSkillFrame f)
    {
        var r = f.YangFu!; var use = RequireBingzhongUse(f, out _); var result = new List<PromptChoice>();
        if (r.AddedTargets.Count / (use.CardKind == CardKind.BorrowedSword ? 2 : 1) < r.X)
        {
            var targets = r.OriginalTargets.Concat(r.AddedTargets).ToArray();
            foreach (var seats in BingzhongLegalTargets(use with { TargetSeats = targets, Action = CloneTargetAction(use.Action!, targets) }))
                result.Add(new(new($"bingzhong.{f.Id}.{string.Join('.', seats)}"), "追加目标：" + string.Join("、", seats.Select(s => facts.Player(s).Name)), [], seats,
                    new Dictionary<string, string> { ["program-action"] = "bingzhong-target", ["branch"] = "target" }));
        }
        result.Add(new(new($"bingzhong.{f.Id}.finish"), "完成额外目标选择", [], [], new Dictionary<string, string> { ["program-action"] = "bingzhong-target", ["branch"] = "finish" }));
        return Array.AsReadOnly(result.Select(PromptChoiceProjection.Freeze).ToArray());
    }
    private void PublishBingzhongChoices(ProgramSkillFrame f)
    {
        var choices = BingzhongChoices(f); var skill = facts.Skill(f.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"屏忠：额外指定至多{f.YangFu!.X}个目标。", [], choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        { PromptId = prompts.NextId(), IsPrivate = false, TargetSeat = f.OwnerSeat, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) }, facts.Player(f.OwnerSeat).IsHuman);
        operations.PublishState();
    }
    [ProgramAnswer("bingzhong-target")]
    private void AnswerBingzhongTargets(PromptChoice selected)
    {
        var f = frames.Top as ProgramSkillFrame ?? throw new InvalidOperationException("Bingzhong lost its public choice owner.");
var r = f.YangFu!; var choices = BingzhongChoices(f);
        var choice = choices.SingleOrDefault(c => c.Id == selected.Id);
        if (r.Stage != YangFuStage.Targets || choice is null || !facts.ChoicesEqual([choice], [selected])) throw new InvalidOperationException("Bingzhong cannot change its normal-range frozen target choice.");
        prompts.Clear();
        if (choice.Targets.Count == 0) { CompleteBingzhongTargets(f); return; }
        for (var i = 0; i < choice.Targets.Count; i++) events.AdvanceFact(new BingzhongTargetSelectedEvent(f.Id, r.AddedTargets.Count + i, choice.Targets[i]));
        frames.ReplaceTop(f = f with { YangFu = r with { AddedTargets = r.AddedTargets.Concat(choice.Targets).ToArray() } });
        if (BingzhongChoices(f).Count == 1) CompleteBingzhongTargets(f); else PublishBingzhongChoices(f);
    }
    private void CompleteBingzhongTargets(ProgramSkillFrame f)
    {
        var r = f.YangFu!; var use = RequireBingzhongUse(f, out var window); var old = use.Action!;
        var targets = r.OriginalTargets.Concat(r.AddedTargets).ToArray(); var action = CloneTargetAction(old, targets);
        var simple = use.AdjustedSimpleContinuation;
        if (r.AddedTargets.Count > 0 && window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
        {
            var continuation = window.SimpleContinuation ?? throw new InvalidOperationException("Bingzhong lost its original basic-card continuation.");
            if (continuation.Effect is not (SimpleCardUseEffect.Recovery or SimpleCardUseEffect.Alcohol)) throw new InvalidOperationException("Bingzhong cannot invent a basic-card effect.");
            simple ??= continuation with { RecoveryPolicySources = continuation.RecoveryPolicySources is { } sources ? Array.AsReadOnly(sources.ToArray()) : null };
        }
        var tail = new BingzhongTargetTail(f.Id, use.Id, action.ActionId, r.Source, r.GameplayHash, r.X, r.Ordinal!.Value, old.ActorSeat, old.ProviderSeat, r.OriginalTargets, r.AddedTargets);
        operations.UpdateRoleUse(use with { TargetSeats = targets, Action = action, BingzhongTail = tail, AdjustedSimpleContinuation = simple,
            TargetsAdjusted = use.TargetsAdjusted || r.AddedTargets.Count > 0, Enhancements = r.AddedTargets.Count == 0 ? use.Enhancements : use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
        events.AdvanceFact(new BingzhongTargetsAppliedEvent(f.Id, use.Id, action.ActionId, r.Source, r.GameplayHash, r.X, r.Ordinal.Value, old.ActorSeat, old.ProviderSeat, r.OriginalTargets.Count, r.AddedTargets.Count));
        FinishYangFu(frames.ActiveProgram(f.Id));
    }
    private bool YangFuActorChain(CardUseFrame use, int original, int provider, long actionId)
    {
        if (use.Action is not { Type: CardActionType.Use } action || action.ActionId != actionId || action.ProviderSeat != provider) return false;
        var actor = original;
        foreach (var e in facts.History<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == use.Id))
        {
            if (e.PreviousActorSeat != actor || e.OwnerSeat != actor || e.ProviderSeat != provider || !facts.ValidSeat(e.ActorSeat) ||
                !facts.History<ProgramBindingStartedEvent>().Any(b => b.FrameId == e.FrameId && b.SkillId == e.SkillId && b.OwnerSeat == actor && b.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized)) return false;
            actor = e.ActorSeat;
        }
        return actor == action.ActorSeat && actor == use.SourceSeat;
    }
    internal bool HasBingzhongTargetTail(CardUseFrame use) => use.Action is { } a && HasBingzhongTargetTail(use, a);
    internal bool HasBingzhongTargetTail(CardUseFrame use, CardActionContext action)
    {
        if (use.BingzhongTail is not { AddedTargets.Count: > 0 } r || action.ActionId != r.ActionId || r.UseId != use.Id || r.Source.SkillId != "ol:bingzhong" || r.Source.BindingId != "first-dynamic-x-targets" ||
            r.X < 1 || r.Ordinal < 1 || r.Ordinal > r.X || r.AddedTargets.Count / (use.CardKind == CardKind.BorrowedSword ? 2 : 1) > r.X ||
            !action.TargetSeats.SequenceEqual(r.OriginalTargets.Concat(r.AddedTargets)) || r.Source.OwnerSeat != r.OriginalActor ||
            facts.History<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id) is not { } declaration ||
            !YangFuActorChain(use, declaration.SourceSeat, r.ProviderSeat, r.ActionId) ||
            facts.Skill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash ||
            facts.History<BingzhongTargetsAppliedEvent>().Count(e => e == new BingzhongTargetsAppliedEvent(r.FrameId, r.UseId, r.ActionId, r.Source, r.GameplayHash, r.X, r.Ordinal, r.OriginalActor, r.ProviderSeat, r.OriginalTargets.Count, r.AddedTargets.Count)) != 1 ||
            !facts.History<BingzhongOriginalTargetEvent>().Where(e => e.FrameId == r.FrameId).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.OriginalTargets) ||
            !facts.History<BingzhongTargetSelectedEvent>().Where(e => e.FrameId == r.FrameId).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.AddedTargets)) return false;
        var actual = r.OriginalTargets.Concat(r.AddedTargets).ToArray();
        foreach (var e in facts.History<BingzhongRedirectedEvent>().Where(e => e.UseId == use.Id))
        {
            if (!RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(use.CardKind) || e.ActionId != r.ActionId || e.TargetIndex < 0 || e.TargetIndex >= actual.Length || actual[e.TargetIndex] != e.PreviousSeat || e.Source.OwnerSeat != e.PreviousSeat ||
                facts.Skill(e.Source.SkillId).Program is not { } p || p.GameplayHash != e.GameplayHash ||
                ProgramInstructionResolver.Default.Find(p, ProgramInstructionSourceKind.Trigger, e.Source.BindingId) is not { } plan || plan.Trigger?.Window != SkillProgramTriggerWindow.SlashTargetRedirecting ||
                e.InstructionIndex < 1 || e.InstructionIndex > plan.Instructions.Count || plan.GetPausedInstruction(e.InstructionIndex).Effect.Op != SkillProgramEffectOp.RedirectCurrentAttack ||
                facts.History<ProgramBindingStartedEvent>().Count(b => b.FrameId == e.FrameId && b.OwnerSeat == e.Source.OwnerSeat && b.SkillId == e.Source.SkillId && b.SkillInstanceId == e.Source.SkillInstanceId && b.BindingId == e.Source.BindingId) != 1) return false;
            actual[e.TargetIndex] = e.Seat;
        }
        return actual.SequenceEqual(use.TargetSeats);
    }
    internal void RecordBingzhongRedirect(ProgramSkillFrame f, long useId, int previousSeat, int nextSeat)
    {
        if (facts.CardUse(useId) is not { BingzhongTail: { AddedTargets.Count: > 0 }, Action: { } a } use) return;
        if (facts.PausedEffect(f)?.Op != SkillProgramEffectOp.RedirectCurrentAttack || f.OwnerSeat != previousSeat ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } c } context || c.ParentCardUseFrameId != useId || c.CardActionId != a.ActionId ||
            context.TargetSeat != previousSeat || use.TargetIndex < 0 || use.TargetIndex >= use.TargetSeats.Count || use.TargetSeats[use.TargetIndex] != nextSeat)
            throw new InvalidOperationException("Bingzhong cannot acknowledge an unrelated native redirect.");
        events.AdvanceFact(new BingzhongRedirectedEvent(f.Id, use.Id, a.ActionId, YangFuSource(f), f.GameplayHash, f.InstructionIndex, use.TargetIndex, previousSeat, nextSeat));
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBingzhongExtraTargets)]
    private PromptChoice ChooseAiBingzhong(PendingDecision decision, ProgramSkillFrame f)
    {
        var use = RequireBingzhongUse(f, out _); bool helpful = use.CardKind is CardKind.Peach or CardKind.Alcohol or CardKind.DrawTwo or CardKind.PeachGarden or CardKind.FiveGrains;
        return decision.Choices.FirstOrDefault(c => c.Targets.Count > 0 && (use.CardKind == CardKind.BorrowedSword ? !facts.Allies(f.OwnerSeat, c.Targets[1]) :
            (helpful || use.CardKind == CardKind.IronChain && facts.Player(c.Targets[0]).IsChained) == facts.Allies(f.OwnerSeat, c.Targets[0]))) ?? decision.Choices.Single(c => c.Targets.Count == 0);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27310)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27310)]
    private bool ContinuePaidYangFu(ProgramSkillFrame f)
    { if (f.YangFu is not { } r || r.Stage == YangFuStage.Targets) return false; return true; }
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 300)]
    private bool IsYangFuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.YangFu is not { } r || e?.Op != r.Op || p != f.PendingMovementContinuation ||
            r.Stage is not (YangFuStage.DrawChildren or YangFuStage.EquipmentChildren or YangFuStage.SlashChildren) &&
                !(r.Stage == YangFuStage.EquipmentUse && r.NativeOrigin is { } origin && facts.CardUse(origin.UseId) is { } use && IsIssuedSuyiUse(use))) return false;
return true;
    }
    private bool YangFuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YangFu is not { } r) return false;         if (child is CardUseFrame use) return r.Stage is YangFuStage.EquipmentUse or YangFuStage.SlashUse && use.SuyiOrigin == r.NativeOrigin && IsIssuedSuyiUse(use);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills) return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is CardsMovedTriggerWindowFrame moved && r.Stage is YangFuStage.DrawChildren or YangFuStage.EquipmentChildren or YangFuStage.SlashChildren &&
            RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => facts.Movements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }
    private ProgramSkillFrame? YangFuObserverRoot() => frames.OfType<ProgramSkillFrame>()
        .FirstOrDefault(root => frames.ChildOf(root.Id) is { } child && YangFuFirstChild(root, child) && HasYangFuObserverSuffix(child));

    private bool HasYangFuObserverSuffix(ResolutionFrame first)
    {
        for (var child = frames.ChildOf(first.Id); child is not null; child = frames.ChildOf(child.Id))
        {
            if (!adjacency.MovementObserverEdge(child.Id) && !adjacency.DyingExitEdge(child.Id) &&
                !adjacency.RandomEquipmentRide(child, frames.ParentOf(child.Id)!) &&
                !adjacency.RecoveryReplacementRide(child, frames.ParentOf(child.Id)!)) return false;
            if (child is DyingFrame dying && frames.ChildOf(child.Id) is not null && adjacency.SuspendedOrPaidRescueSuffix(dying)) return true;
        }
        return true;
    }
    private bool IsAbove<T>(long id, long rootId) where T : ResolutionFrame
    {
        if (frames.Find(id) is not T child) return false;
        for (var parent = frames.ParentOf(child.Id); parent is not null; parent = frames.ParentOf(parent.Id))
            if (parent.Id == rootId) return true;
        return false;
    }
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 770)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 880)]
    private bool HasYangFuDamageObserver(long id) => YangFuObserverRoot() is { } root && IsAbove<DamageTriggerWindowFrame>(id, root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 910)]
    private bool HasYangFuBeforeDamageObserver(long id) => YangFuObserverRoot() is { } root && IsAbove<BeforeDamageProgramWindowFrame>(id, root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27310)]
    private bool AdvanceYangFuObserver() => prompts.Current is null && YangFuObserverRoot() is not null && operations.AdvanceObserverSubtree();

    internal void BindSuyiEquipmentUse(CardUseFrame use)
    {
        if (frames.ParentOf(use.Id) is not ProgramSkillFrame { YangFu: { Op: SkillProgramEffectOp.ResolveSuyiEquipmentThenSlash, Stage: YangFuStage.EquipmentUse } r } f) return;
        if (r.NativeOrigin is not null || use.Action is not { Type: CardActionType.Use } action || use.SourceSeat != r.TargetSeat || action.ActorSeat != r.TargetSeat || action.ProviderSeat != r.TargetSeat ||
            !facts.IsEquipment(use.CardKind) || use.PhysicalCardIds is not [var id] || id != use.CardId || action.PhysicalCards is not [var cost] || cost.CardId != id || cost.From != CardLocation.DrawPile ||
            f.CardSetBindings.SingleOrDefault(b => b.Name == SuyiEquipmentBind) is not { CardIds: [var bound] } || bound != id ||
            !action.ConversionChain.SequenceEqual([r.Source]) || f.PendingMovementContinuation?.SubjectSeat != r.TargetSeat)
            throw new InvalidOperationException("Suyi must issue the exact real stock equipment for its selected foreign actor.");
        IssueSuyiNative(f, use, action);
    }
    internal void IssueSuyiSlashUse(long id, ProgramSkillFrame supplied, CardActionContext action)
    {
        if (supplied.YangFu is not { Op: SkillProgramEffectOp.ResolveSuyiEquipmentThenSlash, Stage: YangFuStage.SlashUse }) return;
        if (frames.ParentOf(id) is not ProgramSkillFrame { YangFu: { } r } f || f != supplied || facts.CardUse(id) is not { } use ||
            r.NativeOrigin is not null || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat ||
            action.EffectiveKind != CardKind.Slash || !action.TargetSeats.SequenceEqual([r.TargetSeat]) || action.PhysicalCards.Count != 0 ||
            use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || !action.ConversionChain.SequenceEqual([r.Source]))
            throw new InvalidOperationException("Suyi cannot replace its exact zero-entity ordinary Slash invoice.");
        IssueSuyiNative(f, use, action);
        if (facts.Phase == TurnPhase.Play && f.OwnerSeat == facts.CurrentSeat && !adjacency.IgnoresSlashLimit(f.OwnerSeat, r.Source) &&
            !(facts.CardUse(id)?.UnlimitedUse == true) && !adjacency.HasIssuedJudgedRankSlashQuota(id, f.OwnerSeat) && !adjacency.HasIssuedWuLuKangDixianUse(id, f.OwnerSeat) && !adjacency.HasIssuedRoundGainedBasicBonus(id, f.OwnerSeat))
        {
            operations.RecordSlashDebit(id, f.OwnerSeat);
            if (facts.HasUseDebit(action.ActionId)) events.AdvanceFact(new SuyiSlashQuotaPaidEvent(f.Id, id, f.OwnerSeat));
        }
    }
    private void IssueSuyiNative(ProgramSkillFrame f, CardUseFrame use, CardActionContext action)
    {
        var r = f.YangFu!; var origin = new YangFuNativeOrigin(f.Id, r.Source, r.GameplayHash, r.ActualTurn, use.Id, action.ActionId, action.ActorSeat, action.ProviderSeat, use.CardId, use.CardKind, facts.MovementSequence);
        frames.Replace(f.Id, f with { YangFu = r with { NativeOrigin = origin, Before = facts.MovementSequence } });
        operations.SetNativeOrigin(use.Id, origin); events.AdvanceFact(new SuyiNativeIssuedEvent(origin));
    }
    private bool IsIssuedSuyiUse(CardUseFrame use)
    {
        if (use.SuyiOrigin is not { } o || use.Action is not { } current) return false;
        var issued = current;
        if (use.CurrentSlashFirePolicy is { } fire)
        {
            if (o.Kind != CardKind.Slash || fire.OriginalKind != o.Kind || fire.OriginalAction.ActionId != o.ActionId) return false;
            adjacency.AssertCurrentSlashFirePolicy(use); issued = fire.OriginalAction;
        }
        else if (o.Kind != use.CardKind) return false;
        if (o.UseId != use.Id || o.ActionId != issued.ActionId || issued.EffectiveKind != o.Kind || o.CardId != use.CardId ||
            !YangFuActorChain(use, o.ActorSeat, o.ProviderSeat, o.ActionId) || facts.History<SuyiNativeIssuedEvent>().Count(e => e.Origin == o) != 1) return false;
        if (frames.ParentOf(use.Id) is not ProgramSkillFrame { YangFu: { Op: SkillProgramEffectOp.ResolveSuyiEquipmentThenSlash, NativeCompleted: false } r } f ||
            f.Id != o.ProgramFrameId || r.NativeOrigin != o || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.ActualTurn != o.ActualTurn ||
            !issued.ConversionChain.SequenceEqual([r.Source])) return false;
        return r.Stage == YangFuStage.EquipmentUse ? o.ActorSeat == r.TargetSeat && facts.IsEquipment(o.Kind) && use.PhysicalCardIds is [var id] && id == o.CardId &&
            issued.PhysicalCards is [var cost] && cost.CardId == id && cost.From == CardLocation.DrawPile :
            r.Stage == YangFuStage.SlashUse && o.ActorSeat == f.OwnerSeat && o.CardId == 0 && o.Kind == CardKind.Slash && use.PhysicalCardIds is { Count: 0 } && issued.PhysicalCards.Count == 0;
    }
    internal bool IsSuyiSlashUse(long id) => facts.CardUse(id) is { SuyiOrigin.Kind: CardKind.Slash } use && IsIssuedSuyiUse(use);
    internal bool TryGetSuyiVirtualPrimaryReturn(CardUseFrame use, ProgramSkillFrame parent, out IReadOnlyList<int> targets)
    {
        targets = [];
        if (use.SuyiOrigin is not { Kind: CardKind.Slash, CardId: 0 } origin || !IsSuyiSlashUse(use.Id) || origin.ProgramFrameId != parent.Id ||
            parent.YangFu is not { Stage: YangFuStage.SlashUse, NativeCompleted: false } r || r.NativeOrigin != origin ||
            parent.SelectedTargetSeats is not [var selected] || selected != r.TargetSeat ||
            use.CardAttack is not { CardId: null, PhysicalCardIds.Count: 0 } attack || attack.ProgramSkillCardUseFrameId != parent.Id ||
            !((HasBingzhongTargetTail(use) || adjacency.HasPijingTargetTail(use)) || adjacency.HasWeifuTargetTail(use) || adjacency.HasDesignatedExtraTargetTail(use) || adjacency.HasRecipientCategorySlashTargetTail(use) || adjacency.HasZhangYanLangdaoTargetTail(use)) || use.TargetSeats.Count == 0 ||
            facts.History<TargetsConfirmedEvent>().SingleOrDefault(e => e.ResolutionId == use.Id) is not { TargetSeats: [var original] } || original != r.TargetSeat ||
            facts.History<CardUseDeclaredEvent>().SingleOrDefault(e => e.ResolutionId == use.Id) is not { CardId: 0, CardKind: CardKind.Slash } declared || declared.SourceSeat != parent.OwnerSeat)
            return false;
        targets = Array.AsReadOnly(new[] { use.TargetSeats[0] }); return true;
    }
    internal bool IsSuyiEquipmentRide(ResolutionFrame ride, ResolutionFrame beneath) => ride is CardUseFrame { SuyiOrigin: { } o } use &&
        beneath is ProgramSkillFrame { YangFu.Stage: YangFuStage.EquipmentUse } f && o.ProgramFrameId == f.Id && facts.IsEquipment(o.Kind) && IsIssuedSuyiUse(use);
    [NativeUseValidation(27310)] private void ValidateSuyiNative(CardUseFrame use)
    { if (use.SuyiOrigin is not null && !IsIssuedSuyiUse(use)) throw new InvalidOperationException("Suyi lost its exact issued native use and original provider."); }
    [NativeUseReturn(27310)] private void ReturnSuyiNative(CardUseFrame use)
    {
        if (use.SuyiOrigin is not { } origin) return;
        var f = frames.ActiveProgram(origin.ProgramFrameId); var r = f.YangFu;
        if (r is null || r.NativeOrigin != origin || r.NativeCompleted || use.Step != ResolutionFrameStep.Completed || facts.CardUse(use.Id) is not null ||
            facts.History<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1 || origin.CardId != 0 &&
            facts.Movements.Count(m => m.Sequence > origin.Before && m.CardId == origin.CardId && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.EquipmentUse) != 1)
            throw new InvalidOperationException("Suyi lost its whole once-paid native return.");
        frames.ReplaceTop(f with { YangFu = r with { NativeCompleted = true, After = facts.MovementSequence } });
        if (use.CardAttack is { Active: true }) return; SettleSuyiNative(origin);
    }
    internal bool ReturnSuyiAttack(YangFuAttackCompletion completion)
    {
        if (frames.Top is not ProgramSkillFrame { YangFu: { Stage: YangFuStage.SlashUse, NativeCompleted: true, NativeOrigin: { } o } } || o.UseId != completion.ResolutionId) return false;
        if (completion.ProgramFrameId is { } parent && parent != o.ProgramFrameId) throw new InvalidOperationException("Suyi's real Slash returned to another owner.");
        SettleSuyiNative(o); return true;
    }
    private void SettleSuyiNative(YangFuNativeOrigin origin)
    {
        var f = frames.ActiveProgram(origin.ProgramFrameId); var r = f.YangFu!;
        if (!r.NativeCompleted || r.NativeOrigin != origin || facts.CardUse(origin.UseId) is not null || facts.History<SuyiNativeReturnedEvent>().Any(e => e.Origin == origin))
            throw new InvalidOperationException("Suyi cannot return or pay the same native child twice.");
        frames.ReplaceTop(f with { YangFu = r with { Stage = r.Stage == YangFuStage.EquipmentUse ? YangFuStage.EquipmentChildren : YangFuStage.SlashChildren }, PendingMovementContinuation = new(r.Stage == YangFuStage.EquipmentUse ? r.TargetSeat : f.OwnerSeat, 0, null) });
        events.AdvanceFact(new SuyiNativeReturnedEvent(origin));
        // The native child is fully paid and returned. A decided game will not
        // run another engine step to finish this exact ancestor.
        if (facts.Winner != Winner.None) frames.AdvanceProgram(origin.ProgramFrameId);
    }
    private static CardActionContext CloneTargetAction(CardActionContext action, IReadOnlyList<int> targets) =>
        new(action.ActionId, action.ParentActionId, action.Type, action.ActorSeat, action.ProviderSeat,
            action.RequesterSeat, action.ResponderSeat, action.OpponentSeat, action.EffectiveKind, targets,
            action.PhysicalCards, action.ConversionChain,
            action.EffectiveKind == CardKind.BorrowedSword ? targets.Where((_, index) => index % 2 == 0).ToArray() : targets,
            action.EffectiveSuit, action.EffectiveRank, action.EffectiveIsRed, action.FactionOrigin);
}
