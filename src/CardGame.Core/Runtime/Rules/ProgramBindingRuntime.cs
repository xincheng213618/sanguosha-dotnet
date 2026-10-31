namespace CardGame.Core;

/// <summary>Starts one admitted binding and owns its private activation choices.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramBindingRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramBindingFacts facts, IProgramBindingProtocols native,
    IProgramActivationWindows windows) : IProgramBindingActions
{
    public void Begin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (!facts.CanRun(candidate, context))
            throw new InvalidOperationException("The configured program binding is no longer eligible.");
        native.ConsumeEarnedActualEnding(candidate, context);
        native.ConsumePrepDiscardEnding(candidate, context);
        native.ConsumeFixedDistanceDebt(candidate, context);
        if (!facts.Trigger(candidate).Effects.Any(e => e.Op is SkillProgramEffectOp.DiscardSameCategoryForExtraEffect or SkillProgramEffectOp.PayTwoForChosenAssaultAndApplyTargetOutcomes or SkillProgramEffectOp.DyingRecipientChooseReplaceOrGrantThenRecover or SkillProgramEffectOp.GuessTargetCategoriesAndArmTurnDamage))
            native.ConsumeUsage(candidate);
        native.PayMarkerCost(candidate.OwnerSeat, facts.Trigger(candidate).MarkerCost,
            candidate.SkillId, candidate.BindingId);
        prompts.Clear();
        if (frames.Top is ProgramCardTriggerWindowFrame cardAction && cardAction.Id == context.ParentFrameId)
            frames.ReplaceTop(cardAction with { Activated = true });
        var frame = new ProgramSkillFrame(frames.NextId(), candidate.OwnerSeat, candidate.SkillId,
            candidate.BindingId, candidate.GameplayHash, 0, [], [])
        {
            SkillInstanceId = candidate.SkillInstanceId,
            TriggerId = candidate.BindingId,
            WindowContext = context,
            DyingAlcoholPermission = native.CaptureDyingAlcoholPermission(candidate, context)
        };
        if (ProgramInvocations.Find(frames.Find(context.ParentFrameId), candidate, context) is { } authorization)
            frame = frame with { InvocationAuthorization = authorization with { ProgramFrameId = frame.Id } };
        frame = native.CaptureHpConditionalSource(frame);
        frame = native.CaptureEquipmentSuitSource(frame);
        native.AttachAfterTurnEndedChild(frame, candidate, context);
        frames.Push(frame);
        events.AdvanceFact(new ProgramBindingStartedEvent(frame.Id, frame.SkillId, candidate.BindingId,
            frame.SkillInstanceId, frame.OwnerSeat, context.Window));
        native.LogStarted(frame.OwnerSeat, facts.Skill(frame.SkillId).Name);
        frames.AdvanceProgram(frame.Id);
    }

    public void Expose(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        native.RecordPublicPileColorOffer(candidate, context);
        var skill = facts.Skill(candidate.SkillId);
        var triggerFacts = context.Facts;
        var typeDrawCount = context.Window == SkillProgramTriggerWindow.PlayEnding &&
            facts.Trigger(candidate).Effects is [{ Op: SkillProgramEffectOp.Draw,
                NumberExpression: SkillProgramNumberExpression.CurrentTurnUsedCardCategoryCount }]
            ? triggerFacts?.CurrentTurnUsedCardCategoryCount : null;
        var actualPlayUses = context.Window == SkillProgramTriggerWindow.PlayEnding &&
            facts.Trigger(candidate).Effects.Any(effect => effect.Op == SkillProgramEffectOp.PrepareMobileGuoHuaiJingce)
            ? facts.ActualPlayPhaseUseCount(candidate.OwnerSeat) : (int?)null;
        var prompt = typeDrawCount is { } count
            ? $"出牌阶段结束：你本回合使用过的牌有 {count} 种类型。是否发动【{skill.Name}】摸 {count} 张牌？"
            : actualPlayUses is { } phaseUses && triggerFacts is not null
            ? $"出牌阶段结束：本出牌阶段已使用 {phaseUses} 张牌，当前体力为 {triggerFacts.CurrentHp}。是否发动【{skill.Name}】？"
            : context.Window == SkillProgramTriggerWindow.PlayEnding &&
                context.SourceSeat == candidate.OwnerSeat && triggerFacts is not null
            ? $"出牌阶段结束：本回合已使用 {triggerFacts.CardsUsedThisTurn} 张牌，当前体力为 {triggerFacts.CurrentHp}。是否发动【{skill.Name}】？"
            : facts.Trigger(candidate).Effects is [{ Op: SkillProgramEffectOp.OfferRiceAfterDamagePoint }]
            ? $"是否获得 {facts.Name(candidate.OwnerSeat)} 的一张米？"
            : context.OptionalChooserSeat is not null
            ? $"是否令 {facts.Name(candidate.OwnerSeat)} 发动【{skill.Name}】摸一张牌？" : $"是否发动【{skill.Name}】？";
        Dictionary<string, string> Parameters(string action)
        {
            var parameters = ActivationParameters(candidate, action);
            if (actualPlayUses is { } used)
                parameters["cards-used-this-play-phase"] = used.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (triggerFacts is not null)
            {
                parameters["cards-used-this-turn"] = triggerFacts.CardsUsedThisTurn.ToString(System.Globalization.CultureInfo.InvariantCulture);
                parameters["current-hp"] = triggerFacts.CurrentHp.ToString(System.Globalization.CultureInfo.InvariantCulture);
                parameters["classic-identity-mode"] = triggerFacts.IsClassicIdentityMode ? "true" : "false";
            }
            return parameters;
        }
        prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger,
            context.OptionalChooserSeat ?? candidate.OwnerSeat, prompt, [], [],
            context.OptionalChooserSeat is not null ? candidate.OwnerSeat : context.SourceSeat)
        {
            PromptId = prompts.NextId(),
            IsPrivate = true,
            TargetSeat = context.OptionalChooserSeat ?? context.TargetSeat ?? candidate.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(candidate.SkillId, skill.Name,
                $"{skill.Name} · 是否发动", skill.Description),
            Choices =
            [
                new PromptChoice(new ChoiceId($"program-trigger.activate.{candidate.SkillId}.{candidate.BindingId}.{candidate.SkillInstanceId}"),
                    skill.ProgramPresentation?.TriggerLabels.GetValueOrDefault(facts.Trigger(candidate).Id) is { } label
                        ? label : $"发动【{skill.Name}】。", [], [], Parameters("activate")),
                new PromptChoice(new ChoiceId($"program-trigger.skip.{candidate.SkillId}.{candidate.BindingId}.{candidate.SkillInstanceId}"),
                    $"不发动【{skill.Name}】。", [], [], Parameters("skip"))
            ]
        }, facts.IsHuman(context.OptionalChooserSeat ?? candidate.OwnerSeat));
    }

    public void ExposeGroup(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first,
        ProgramSkillWindowContext context, string choiceGroup)
    {
        var skill = facts.Skill(first.SkillId);
        Dictionary<string, string> Parameters(ProgramTriggerCandidate candidate, string action)
        {
            var parameters = ActivationParameters(candidate, action);
            parameters["choice-group"] = choiceGroup;
            return parameters;
        }
        var choices = members.Select(candidate => new PromptChoice(
            new ChoiceId($"program-trigger.activate.{candidate.SkillId}.{candidate.BindingId}.{candidate.SkillInstanceId}"),
            facts.Trigger(candidate).ChoiceLabel ?? throw new InvalidOperationException(
                "A configured program choice branch lost its presentation label."),
            [], [], Parameters(candidate, "activate"))).ToList();
        if (facts.Trigger(first).Optional)
            choices.Add(new PromptChoice(new ChoiceId($"program-trigger.skip-group.{first.SkillId}.{choiceGroup}.{first.SkillInstanceId}"),
                $"不发动【{skill.Name}】。", [], [], Parameters(first, "skip")));
        prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, first.OwnerSeat,
            $"请选择【{skill.Name}】的发动方式。", [], [], context.SourceSeat)
        {
            PromptId = prompts.NextId(),
            IsPrivate = true,
            TargetSeat = context.TargetSeat ?? first.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(first.SkillId, skill.Name,
                $"{skill.Name} · 选择方式", skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray())
        }, facts.IsHuman(first.OwnerSeat));
    }

    public IReadOnlyList<ProgramTriggerCandidate> ChoiceGroup(IEnumerable<ProgramTriggerCandidate> candidates,
        ProgramTriggerCandidate first, string group)
    {
        var members = candidates.TakeWhile(candidate => candidate.OwnerSeat == first.OwnerSeat &&
            candidate.SkillId == first.SkillId && candidate.SkillInstanceId == first.SkillInstanceId &&
            candidate.Priority == first.Priority && facts.Trigger(candidate).ChoiceGroup == group).ToArray();
        if (members.Length == 0)
            throw new InvalidOperationException("A configured program choice group lost every eligible branch.");
        return members;
    }

    internal void ResolveActivation(PromptChoice selected)
    {
        if (windows.TryResolveTurnEndingOrder(selected)) return;
        var action = selected.Parameters.GetValueOrDefault("program-action");
        var (candidate, context) = windows.PendingCandidate();
        if (windows.TryResolveGroup(candidate, context, selected, action)) return;
        if (selected.Parameters.GetValueOrDefault("skill-id") != candidate.SkillId ||
            selected.Parameters.GetValueOrDefault("binding-id") != candidate.BindingId ||
            selected.Parameters.GetValueOrDefault("skill-instance-id") != candidate.SkillInstanceId)
            throw new InvalidOperationException("The program activation identity changed.");
        prompts.Clear();
        if (action == "activate")
        {
            if (!facts.CanRun(candidate, context)) { windows.CompleteSkipped(candidate); return; }
            Begin(candidate, context);
            return;
        }
        if (action != "skip") throw new InvalidOperationException("Unsupported program trigger choice.");
        windows.CompleteSkipped(candidate);
    }

    private static Dictionary<string, string> ActivationParameters(ProgramTriggerCandidate candidate, string action) => new()
    {
        ["program-action"] = action,
        ["skill-id"] = candidate.SkillId,
        ["binding-id"] = candidate.BindingId,
        ["skill-instance-id"] = candidate.SkillInstanceId
    };
}

internal interface IProgramBindingActions : IProgramRuntimeOperations
{
    void Begin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void Expose(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context);
    void ExposeGroup(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first,
        ProgramSkillWindowContext context, string choiceGroup);
    IReadOnlyList<ProgramTriggerCandidate> ChoiceGroup(IEnumerable<ProgramTriggerCandidate> candidates,
        ProgramTriggerCandidate first, string group);
}
