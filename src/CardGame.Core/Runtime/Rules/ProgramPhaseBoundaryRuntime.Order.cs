namespace CardGame.Core;

internal sealed partial class ProgramPhaseBoundaryRuntime
{
    private IReadOnlyList<TurnEndingBoundaryItem> TurnEndingOrderMembers(TurnEndingBoundaryFrame frame)
    {
        var first = frame.Items[frame.ItemIndex].Candidate;
        if (first is null || BindingFacts.Trigger(first).OrderGroup is not { } group) return [];
        return frame.Items.Skip(frame.ItemIndex).TakeWhile(item =>
                item.Kind == TurnEndingBoundaryItemKind.Program && item.Candidate is { } candidate &&
                candidate.OwnerSeat == first.OwnerSeat && candidate.Priority == first.Priority &&
                BindingFacts.Trigger(candidate).OrderGroup == group)
            .Where(item => BindingFacts.CanRun(item.Candidate!, CreateTurnEndingProgramContext(frame, item.Candidate!, item.Facts)))
            .ToArray();
    }

    private bool TryExposeTurnEndingOrder(TurnEndingBoundaryFrame frame, ProgramTriggerCandidate first)
    {
        var members = TurnEndingOrderMembers(frame);
        if (members.Count < 2) return false;
        var group = BindingFacts.Trigger(first).OrderGroup!;
        var choices = members.Select(item =>
        {
            var candidate = item.Candidate!;
            return new PromptChoice(new ChoiceId($"turn-ending-order.{candidate.SkillId}.{candidate.BindingId}.{candidate.SkillInstanceId}"),
                Content.Skill(candidate.SkillId).Name, [], [], Parameters(candidate, "activate"));
        }).Append(new PromptChoice(new ChoiceId("turn-ending-order.skip." + group), "取消", [], [], Parameters(first, "skip"))).ToArray();
        var skill = Content.Skill(first.SkillId);
        Frames.ReplaceTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        Prompts.Set(new(DecisionKind.ProgramTrigger, first.OwnerSeat, "选择率先发动的技能", [], [], frame.OwnerSeat)
        {
            PromptId = Prompts.NextId(), IsPrivate = true, Choices = Array.AsReadOnly(choices),
            SkillPrompt = new(first.SkillId, skill.Name, "选择率先发动的技能", skill.Description)
        }, Participants.At(first.OwnerSeat).IsHuman);
        return true;

        Dictionary<string, string> Parameters(ProgramTriggerCandidate candidate, string action) => new()
        {
            ["program-action"] = action, ["order-group"] = group,
            ["skill-id"] = candidate.SkillId, ["binding-id"] = candidate.BindingId,
            ["skill-instance-id"] = candidate.SkillInstanceId
        };
    }

    internal bool TryResolveTurnEndingOrder(PromptChoice choice)
    {
        if (choice.Parameters.GetValueOrDefault("order-group") is not { } group) return false;
        if (Frames.Top is not TurnEndingBoundaryFrame frame)
            throw new InvalidOperationException("An ordered ending choice lost its owning frame.");
        var members = TurnEndingOrderMembers(frame);
        if (members.Count < 2 || BindingFacts.Trigger(members[0].Candidate!).OrderGroup != group ||
            choice.Cards.Count != 0 || choice.Targets.Count != 0)
            throw new InvalidOperationException("An ordered ending choice lost its eligible group.");
        var action = choice.Parameters.GetValueOrDefault("program-action");
        if (action == "skip")
        {
            Prompts.Clear();
            var items = frame.Items.ToArray();
            foreach (var item in members)
            {
                var candidate = item.Candidate!;
                Events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
                    candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.TurnEnding, false, false));
            }
            var retained = items.Take(frame.ItemIndex).Concat(items.Skip(frame.ItemIndex).Where(item => !members.Contains(item))).ToArray();
            Frames.ReplaceTop(frame with { Items = Array.AsReadOnly(retained), Step = ResolutionFrameStep.ResolvingEffect });
            Frames.AdvanceTop<TurnEndingBoundaryFrame>();
            return true;
        }
        if (action != "activate") throw new InvalidOperationException("Unsupported ordered ending answer.");
        var selected = members.SingleOrDefault(item => item.Candidate is { } candidate &&
            candidate.SkillId == choice.Parameters.GetValueOrDefault("skill-id") &&
            candidate.BindingId == choice.Parameters.GetValueOrDefault("binding-id") &&
            candidate.SkillInstanceId == choice.Parameters.GetValueOrDefault("skill-instance-id")) ??
            throw new InvalidOperationException("An ordered ending answer selected another binding.");
        Prompts.Clear();
        var reordered = frame.Items.ToArray();
        var selectedIndex = Array.IndexOf(reordered, selected, frame.ItemIndex);
        Array.Copy(reordered, frame.ItemIndex, reordered, frame.ItemIndex + 1, selectedIndex - frame.ItemIndex);
        reordered[frame.ItemIndex] = selected;
        Frames.ReplaceTop(frame with { Items = Array.AsReadOnly(reordered), Step = ResolutionFrameStep.ResolvingEffect });
        Bindings.Begin(selected.Candidate!, CreateTurnEndingProgramContext(frame, selected.Candidate!, selected.Facts));
        return true;
    }
}
