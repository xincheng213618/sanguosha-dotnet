using System.Globalization;
namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
internal sealed class OlShenZhenJiRuntime(IProgramRuntimeFrames frames, IProgramEventHistoryQueries history, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramOperationDraws draws, IOlShenZhenJiFacts facts,
    IOlShenZhenJiOperations operations) : IOlShenZhenJiHost
{
    private const string OlShenfu = "ol:shenfu";
    private static CardConversionSource OlShenfuSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private int[] OlShenfuUsedTargets(int owner, int turn) => history.Read<OlShenfuTargetSelectedEvent>()
        .Where(e => e.Source.OwnerSeat == owner && e.ActualTurn == turn).Select(e => e.TargetSeat).Distinct().ToArray();
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => BeginOlShenfu(frame);
    private SkillProgramStepOutcome BeginOlShenfu(ProgramSkillFrame supplied)
    {
        var f = frames.ActiveProgram(supplied.Id);
        if (f.OlShenfu is not null || f.InstructionIndex != 1 || f.SkillId != OlShenfu || f.TriggerId is null ||
            f.WindowContext?.Window != SkillProgramTriggerWindow.TurnEnding || f.OwnerSeat != facts.CurrentSeat)
            throw new InvalidOperationException("Shenfu requires its original accepted own-turn ending.");
        var r = new OlShenfuReceipt(OlShenfuSource(f), f.GameplayHash, f.InstructionIndex, facts.TurnNumber, facts.Hand(f.OwnerSeat).Count % 2 != 0)
            { Targets = OlShenfuUsedTargets(f.OwnerSeat, facts.TurnNumber) };
        frames.ReplaceTop(f = f with { OlShenfu = r });
        events.AdvanceFact(new OlShenfuStartedEvent(f.Id, r.Source, r.ActualTurn, r.Odd));
        PublishOlShenfu(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private PromptChoice OlShenfuChoice(ProgramSkillFrame f, string option, string label, int? target = null) =>
        new(new($"ol-shenfu.{f.Id}.{f.OlShenfu!.Ordinal}.{option}"), label, [], target is { } seat ? [seat] : [],
            new Dictionary<string, string> { ["program-action"] = "ol-shenfu", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
    private IReadOnlyList<PromptChoice> OlShenfuChoices(ProgramSkillFrame f)
    {
        var r = f.OlShenfu!; var choices = new List<PromptChoice>();
        if (r.Stage == OlShenfuStage.HandCard)
        {
            choices.AddRange(facts.HandDiscardChoices(f.Id, f.OwnerSeat, r.TargetSeat)
                .Select(c => c with { Id = new($"ol-shenfu.{f.Id}.{r.Ordinal}.card.{c.Id.Value}"),
                    Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "ol-shenfu", ["option"] = "card" } }));
            return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
        }
        if (r.Stage != OlShenfuStage.Choosing) return [];
        foreach (var p in facts.Players.Where(p => p.IsAlive && !r.Targets.Contains(p.Seat) && (!r.Odd || p.Seat != f.OwnerSeat))
            .OrderBy(p => facts.RelativeOrder(f.OwnerSeat, p.Seat)))
        {
            if (r.Odd) choices.Add(OlShenfuChoice(f, "thunder:" + p.Seat, "对" + p.Name + "造成1点雷电伤害", p.Seat));
            else
            {
                choices.Add(OlShenfuChoice(f, "draw:" + p.Seat, "令" + p.Name + "摸一张牌", p.Seat));
                if (facts.HandDiscardChoices(f.Id, f.OwnerSeat, p.Seat).Count > 0)
                    choices.Add(OlShenfuChoice(f, "discard:" + p.Seat, "弃置" + p.Name + "的一张手牌", p.Seat));
            }
        }
        choices.Add(OlShenfuChoice(f, "finish", "结束神赋"));
        return Array.AsReadOnly(choices.Select(PromptChoiceProjection.Freeze).ToArray());
    }
    private bool OlShenfuSourceCurrent(ProgramSkillFrame f) => facts.Winner == Winner.None && facts.Player(f.OwnerSeat).IsAlive &&
        facts.HasSkillInstance(f.OwnerSeat, f.SkillId, f.SkillInstanceId);
    private void PublishOlShenfu(ProgramSkillFrame f)
    {
        if (!OlShenfuSourceCurrent(f)) { FinishOlShenfu(f); return; }
        var choices = OlShenfuChoices(f);
        if (choices.Count == 0 || choices.All(c => c.Parameters.GetValueOrDefault("option") == "finish")) { FinishOlShenfu(f); return; }
        var skill = facts.Skill(f.SkillId);
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        prompts.Set(new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.OlShenfu!.Stage == OlShenfuStage.HandCard ? "神赋：选择一张手牌；他人的暗手牌仅显示牌位。" : "神赋：选择未指定过的角色，或结束。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        { PromptId = prompts.NextId(), IsPrivate = true, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) }, facts.Player(f.OwnerSeat).IsHuman);
        operations.PublishState();
    }
    [ProgramAnswer("ol-shenfu")] private void AnswerOlShenfu(PromptChoice choice)
    {
        if (frames.Top is not ProgramSkillFrame { OlShenfu: { } r } f ||
            prompts.Current?.PlayerSeat != f.OwnerSeat || !OlShenfuSourceCurrent(f) ||
            !OlShenfuChoices(f).Any(c => RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shenfu requires its exact unpaid current target or opaque hand slot.");
prompts.Clear();
        frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var option = choice.Parameters["option"];
        if (option == "finish") { FinishOlShenfu(f); return; }
        if (r.Stage == OlShenfuStage.HandCard)
        {
            if (option != "card" || choice.Parameters.GetValueOrDefault("source-zone") != CardZoneKind.Hand.ToString() ||
                choice.Parameters.GetValueOrDefault("card-owner-seat") != r.TargetSeat.ToString(CultureInfo.InvariantCulture) ||
                !int.TryParse(choice.Parameters.GetValueOrDefault("slot-index"), out var slot))
                throw new InvalidOperationException("Shenfu lost its actual selected Hand slot.");
            var card = facts.Hand(r.TargetSeat)[slot]; var before = facts.MovementSequence;
            frames.ReplaceTop(f = f with { OlShenfu = r with { Stage = OlShenfuStage.Children, MovementBefore = before, MovementAfter = before },
                PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            operations.LogicalDiscard(f, f.OwnerSeat, [card.Id], new("skill.ol-shen-zhen-ji.shenfu.discard"));
            f = frames.ActiveProgram(f.Id);
            frames.ReplaceTop(f = f with { OlShenfu = f.OlShenfu! with { MovementAfter = facts.MovementSequence } });
            events.AdvanceFact(new OlShenfuDiscardedEvent(f.Id, r.Ordinal, r.TargetSeat, before, facts.MovementSequence));
            ContinueOlShenfu(f.Id); return;
        }
        var target = choice.Targets.Single(); var mode = option.Split(':')[0];
        r = r with { TargetSeat = target, Mode = mode, Ordinal = r.Ordinal + 1, Targets = r.Targets.Append(target).ToArray(),
            KilledByDamage = false, DrawOutcome = null, MovementBefore = 0, MovementAfter = 0,
            Stage = mode == "thunder" ? OlShenfuStage.Damaging : mode == "draw" ? OlShenfuStage.Drawing : OlShenfuStage.HandCard };
        frames.ReplaceTop(f = f with { OlShenfu = r });
        events.AdvanceFact(new OlShenfuTargetSelectedEvent(f.Id, r.Source, r.ActualTurn, r.Ordinal, target, mode));
        if (mode == "thunder") { operations.BeginDamage(f, target, 1, DamageNature.Thunder); return; }
        if (mode == "draw") { draws.BeginOperationProgramDraw(f, target, 1, new("skill.ol-shen-zhen-ji.shenfu.draw")); return; }
        PublishOlShenfu(f);
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 40525)] private void CaptureOlShenfuDeath(DyingResolvedEvent fact)
    {
        if (fact.Survived || frames.OfType<DyingFrame>().SingleOrDefault(d => d.Id == fact.ResolutionId) is not { Continuation: DyingContinuationKind.Damage } dying ||
            frames.OfType<DamageFrame>().SingleOrDefault(d => d.Id == dying.ParentFrameId) is not { } damage ||
            frames.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == damage.ParentFrameId) is not { OlShenfu: { Stage: OlShenfuStage.Damaging } r } f ||
            dying.VictimSeat != r.TargetSeat || damage.TargetSeat != r.TargetSeat || damage.SourceSeat != f.OwnerSeat ||
            f.AttackAttempt is not { IsChainPropagation: false } || r.KilledByDamage) return;
        frames.Replace(f.Id, f with { OlShenfu = r with { KilledByDamage = true } });
        events.AdvanceFact(new OlShenfuDeathConfirmedEvent(f.Id, r.Ordinal, r.TargetSeat, damage.Id, dying.Id));
    }
    internal bool DrainChildWindows(long id) => operations.DrainChildWindows(id);
    private void ContinueOlShenfu(long id)
    {
        if (prompts.Current is not null || frames.Top is not ProgramSkillFrame { OlShenfu: { } r } f || f.Id != id) return;
        if (r.Stage is OlShenfuStage.Choosing or OlShenfuStage.HandCard) { PublishOlShenfu(f); return; }
        if (r.Stage == OlShenfuStage.Drawing || f.AttackAttempt is not null) return;
        if (DrainChildWindows(id)) return;
        f = frames.ActiveProgram(id); r = f.OlShenfu!;
        var repeat = r.Odd ? r.KilledByDamage : facts.Player(r.TargetSeat).IsAlive && facts.Hand(r.TargetSeat).Count == facts.Player(r.TargetSeat).Hp;
        events.AdvanceFact(new OlShenfuFlowResolvedEvent(f.Id, r.Ordinal, r.TargetSeat, r.Mode!, repeat));
        if (!repeat || !OlShenfuSourceCurrent(f)) { FinishOlShenfu(f); return; }
        frames.ReplaceTop(f = f with { PendingMovementContinuation = null, OlShenfu = r with { Stage = OlShenfuStage.Choosing } });
        PublishOlShenfu(f);
    }
    private void FinishOlShenfu(ProgramSkillFrame f)
    { frames.ReplaceTop(f with { OlShenfu = null, PendingMovementContinuation = null }); frames.AdvanceProgram(f.Id); }
    [ProgramContinuation(ProgramContinuationKind.Resume, 40525)] private bool ResumeOlShenfu(long id)
    { if (frames.Top is not ProgramSkillFrame { OlShenfu: not null } f || f.Id != id) return false; ContinueOlShenfu(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 40525)] private bool ReturnOlShenfuMovement(ProgramSkillFrame f) => f.OlShenfu is not null && ResumeOlShenfu(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveOlShenZhenJiShenfu)] private void ReturnOlShenfuDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.OlShenfu is not { Stage: OlShenfuStage.Drawing, Mode: "draw" } r || outcome.RecipientSeat != r.TargetSeat || outcome.RequestedCount != 1 ||
            history.Read<OlShenfuDrawReturnedEvent>().Any(e => e.FrameId == f.Id && e.Ordinal == r.Ordinal))
            throw new InvalidOperationException("Shenfu requires its exact one-card native producer return.");
        frames.ReplaceTop(f with { OlShenfu = r with { Stage = OlShenfuStage.Children, DrawOutcome = outcome } });
        events.AdvanceFact(new OlShenfuDrawReturnedEvent(f.Id, r.Ordinal, r.TargetSeat, outcome.RequestedCount, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter));
        ResumeOlShenfu(f.Id);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40525)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40525)]
    private bool RetainIssuedOlShenfu(ProgramSkillFrame f) => f.OlShenfu is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveOlShenZhenJiShenfu)] private PromptChoice ChooseOlShenfu(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
}
