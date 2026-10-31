namespace CardGame.Core;

internal sealed partial class ProgramTargetSelectionRuntime
{
    internal SkillProgramStepOutcome SelectProgramTargets(
        long frameId,
        int ownerSeat,
        SkillProgramTargetKind targetKind,
        int minimumTargets,
        int maximumTargets,
        SkillProgramNumberExpression? numberExpression,
        SkillProgramTargetAiOrder aiOrder)
    {
        var frame = frames.ActiveProgram(frameId);
        var paused = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        if (paused is { Op: SkillProgramEffectOp.SelectTargets,
                NumberExpression: SkillProgramNumberExpression.BoundCardCount, SourceBind: not null })
            return protocols.SelectNamedBoundTargets(frame, paused);
        if (paused.ReplaceSelection == true)
        {
            frames.ReplaceTop(frame = frame with { SelectedTargetSeats = Array.AsReadOnly(Array.Empty<int>()) });
        }
        var targetSeats = GetProgramTargetSeats(ownerSeat, targetKind, frame.WindowContext);
        if (paused.AllTargets == true)
        {
            frames.ReplaceTop(frame with { SelectedTargetSeats = Array.AsReadOnly(targetSeats.ToArray()) });
            return SkillProgramStepOutcome.Continue;
        }
        if ((numberExpression is SkillProgramNumberExpression.CurrentHp or SkillProgramNumberExpression.OwnerLostHpAtLeastOne) && !Player(ownerSeat).HasHpValue)
        { protocols.Cancel(frame, "体力值未定义，无法计算技能目标数量。"); return SkillProgramStepOutcome.Continue; }
        var expressionMaximum = numberExpression switch
        {
            SkillProgramNumberExpression.EventMovedCardCount => frame.WindowContext?.Window == SkillProgramTriggerWindow.CardsMoved ? frame.WindowContext.Facts?.MovedCardCount ?? 0 : throw new InvalidOperationException("Moved-card target count requires its owning movement window."),
            SkillProgramNumberExpression.BoundCardCount => frame.CardSetBindings.Last().CardIds.Count,
            SkillProgramNumberExpression.CurrentHandCount => Hand(Player(ownerSeat)).Count,
            SkillProgramNumberExpression.CurrentHp => Math.Max(0, Player(ownerSeat).Hp),
            SkillProgramNumberExpression.OwnerLostHpAtLeastOne => Math.Max(1, Player(ownerSeat).MaxHp - Player(ownerSeat).Hp),
            SkillProgramNumberExpression.PlannedNormalDrawCount =>
                facts.PlannedNormalDrawCount(frame),
            _ => maximumTargets
        };
        var cappedMaximum = Math.Min(Math.Min(maximumTargets, expressionMaximum), targetSeats.Count);
        if (numberExpression == SkillProgramNumberExpression.BoundCardCount)
            minimumTargets = expressionMaximum;
        if (minimumTargets < resolvers.MinimumTargets(targetKind) || cappedMaximum < minimumTargets)
        {
            protocols.Cancel(frame, "没有足够的合法技能目标，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var selections = new List<IReadOnlyList<int>>();
        var current = new List<int>();
        AddSelections(0);

        void AddSelections(int next)
        {
            if (current.Count >= minimumTargets)
            {
                var seats = current.ToArray();
                seats = protocols.OrderSupportTargets(frame, seats);
                if (targetKind == SkillProgramTargetKind.EquipmentExchangePair &&
                    !facts.IsEquipmentExchangePair(ownerSeat, seats[0], seats[1])) return;
                if (targetKind is SkillProgramTargetKind.OtherLivingUnequalHandPair or
                    SkillProgramTargetKind.OtherLivingPair)
                {
                    if (targetKind == SkillProgramTargetKind.OtherLivingUnequalHandPair &&
                        Hand(Player(seats[0])).Count == Hand(Player(seats[1])).Count)
                        return;
                    Array.Sort(seats, (left, right) =>
                        Hand(Player(left)).Count.CompareTo(Hand(Player(right)).Count));
                }
                if (targetKind == SkillProgramTargetKind.LivingPairDistinct)
                {
                    // Ordered pair: the first pick is the card source, the second the
                    // destination, so both permutations are exposed as distinct options.
                    selections.Add(Array.AsReadOnly(seats));
                    selections.Add(Array.AsReadOnly(new[] { seats[1], seats[0] }));
                }
                else
                {
                    selections.Add(Array.AsReadOnly(seats));
                    if (targetKind == SkillProgramTargetKind.OtherLivingMale)
                        selections.Add(Array.AsReadOnly(new[] { seats[1], seats[0] }));
                }
            }
            if (current.Count == cappedMaximum) return;
            for (var index = next; index < targetSeats.Count; index++)
            {
                current.Add(targetSeats[index]);
                AddSelections(index + 1);
                current.RemoveAt(current.Count - 1);
            }
        }

        if (selections.Count == 0)
        {
            protocols.Cancel(frame, "没有符合目标关系的角色组合，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = selections.Select(selection =>
        {
            var seats = selection.ToArray();
            var names = string.Join("、", seats.Select(seat => Player(seat).Name));
            var id = string.Join("-", seats.Select(seat =>
                seat.ToString(System.Globalization.CultureInfo.InvariantCulture)));
            return new PromptChoice(
                new ChoiceId($"program-targets.frame-{frameId}.seats-{id}"),
                seats.Length == 0 ? resolvers.EmptySelectionLabel(targetKind) : $"选择 {names}。",
                [],
                seats,
                new Dictionary<string, string>
                {
                    ["program-action"] = "select-targets",
                    ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["target-kind"] = targetKind.ToString(),
                    ["minimum-targets"] = minimumTargets.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["maximum-targets"] = (numberExpression is SkillProgramNumberExpression.PlannedNormalDrawCount or SkillProgramNumberExpression.OwnerLostHpAtLeastOne ||
                        targetKind == SkillProgramTargetKind.CurrentCardUseTargets
                        ? cappedMaximum : maximumTargets).ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["target-ai-order"] = aiOrder.ToString()
                });
        }).ToArray();
        var presentation = content.Skill(frame.SkillId);
        var displayedMaximum = numberExpression is SkillProgramNumberExpression.PlannedNormalDrawCount or SkillProgramNumberExpression.OwnerLostHpAtLeastOne ||
            targetKind == SkillProgramTargetKind.CurrentCardUseTargets
            ? cappedMaximum : maximumTargets;
        var targetCountText = minimumTargets == displayedMaximum
            ? minimumTargets.ToString(System.Globalization.CultureInfo.InvariantCulture)
            : $"{minimumTargets} 至 {displayedMaximum}";
        protocols.Present(new PendingDecision(
            DecisionKind.ProgramTrigger,
            ownerSeat,
            $"【{presentation.Name}】请选择 {targetCountText} 名目标角色。",
            [],
            targetSeats,
            frame.WindowContext?.SourceSeat)
        {
            PromptId = prompts.NextId(),
            IsPrivate = true,
            TargetSeat = ownerSeat,
            SkillPrompt = new SkillPromptPresentation(
                frame.SkillId,
                presentation.Name,
                $"{presentation.Name} · 选择目标",
                presentation.Description),
            Choices = Array.AsReadOnly(choices)
        }, ownerSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome SelectProgramTarget(
        long frameId,
        int ownerSeat,
        SkillProgramTargetKind targetKind,
        IReadOnlyList<CardZoneKind> zones,
        PlayerMarkerKind? marker,
        ProgramParticipantReference? actorReference = null,
        bool skipIfNoTarget = false)
    {
        var frame = frames.ActiveProgram(frameId);
        var targetSeats = SelectionSeats(frame, targetKind, zones, marker, actorReference)
            .Where(seat => SelectionTargetEligible(frame, targetKind, zones, seat, marker,
                actorReference) && CanSelectPindianOpponent(frame, seat)).ToArray();
        if (targetSeats.Length == 0)
        {
            if (skipIfNoTarget)
            {
                frames.ReplaceTop(frame with { SelectedTargetSeats = [] });
                return SkillProgramStepOutcome.Continue;
            }
            protocols.Cancel(frame, "没有仍然合法的技能目标，技能结算已取消。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var choices = targetSeats.Select(targetSeat =>
            new PromptChoice(
                new ChoiceId($"program-target.frame-{frameId}.seat-{targetSeat}"),
                $"选择 {Player(targetSeat).Name}（座位 {targetSeat + 1}）。",
                [],
                [targetSeat],
                new Dictionary<string, string>
                {
                    ["program-action"] = "select-target",
                    ["frame-id"] = frameId.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    ["target-kind"] = targetKind.ToString(),
                    ["marker"] = marker?.ToString() ?? ""
                })).ToArray();
        var presentation = content.Skill(frame.SkillId);
        protocols.Present(new PendingDecision(
            DecisionKind.ProgramTrigger,
            ownerSeat,
            $"【{presentation.Name}】请选择一名目标角色。",
            [],
            targetSeats,
            frame.WindowContext?.SourceSeat)
        {
            PromptId = prompts.NextId(),
            IsPrivate = true,
            TargetSeat = ownerSeat,
            SkillPrompt = new SkillPromptPresentation(
                frame.SkillId,
                presentation.Name,
                $"{presentation.Name} · 选择目标",
                presentation.Description),
            Choices = Array.AsReadOnly(choices)
        }, ownerSeat);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveTargetChoice(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        if (effect is not
            {
                Op: SkillProgramEffectOp.SelectTarget,
                TargetKind: { } targetKind
            } || selected.Parameters.GetValueOrDefault("target-kind") != targetKind.ToString() ||
            selected.Parameters.GetValueOrDefault("marker") != (effect.Marker?.ToString() ?? "") ||
            selected.Targets.Count != 1 || selected.Cards.Count != 0)
            throw new InvalidOperationException("The selected target does not match the suspended instruction.");
        var targetSeat = selected.Targets.Single();
        if (!SelectionTargetEligible(frame, targetKind, effect.Zones, targetSeat,
            effect.Marker, effect.ActorReference) ||
            !CanSelectPindianOpponent(frame, targetSeat))
            throw new InvalidOperationException("The selected program target is no longer legal.");
        prompts.Clear();
        frames.ReplaceTop(frame with
        {
            SelectedTargetSeats = Array.AsReadOnly(new[] { targetSeat })
        });
        if (frame.WindowContext?.Judgment is { } judgment)
            events.AdvanceFact(new ProgramJudgmentTargetSelectedEvent(
                frame.WindowContext.ParentFrameId, judgment.JudgmentFrameId,
                frame.SkillId, frame.TriggerId!, frame.OwnerSeat, targetSeat));
        frames.AdvanceProgram(frame.Id);
        return;
    }

    internal void ResolveTargetsChoice(ProgramSkillFrame frame, SkillProgramEffect effect, PromptChoice selected)
    {
        if (effect.SourceBind is not null)
        {
            protocols.ResolveNamedBoundTargets(frame, effect, selected);
            return;
        }
        if (effect is not
            {
                Op: SkillProgramEffectOp.SelectTargets,
                TargetKind: { } targetKind,
                TargetAiOrder: { } targetAiOrder
            } || selected.Parameters.GetValueOrDefault("target-kind") != targetKind.ToString() ||
            selected.Parameters.GetValueOrDefault("target-ai-order") != targetAiOrder.ToString() ||
            selected.Cards.Count != 0 ||
            selected.Targets.Count < effect.MinimumTargets ||
            selected.Targets.Count > effect.MaximumTargets ||
            (effect.NumberExpression == SkillProgramNumberExpression.CurrentHandCount &&
             selected.Targets.Count > Hand(Player(frame.OwnerSeat)).Count) ||
            selected.Targets.Distinct().Count() != selected.Targets.Count)
            throw new InvalidOperationException("The selected targets do not match the suspended instruction.");
        var legalTargets = GetProgramTargetSeats(frame.OwnerSeat, targetKind, frame.WindowContext);
        if (selected.Targets.Any(targetSeat => !legalTargets.Contains(targetSeat)))
            throw new InvalidOperationException("A selected program target is no longer legal.");
        if (targetKind == SkillProgramTargetKind.EquipmentExchangePair &&
            (selected.Targets.Count != 2 || !facts.IsEquipmentExchangePair(frame.OwnerSeat,
                selected.Targets[0], selected.Targets[1])))
            throw new InvalidOperationException("The selected equipment-exchange pair is no longer legal.");
        if (targetKind == SkillProgramTargetKind.OtherLivingUnequalHandPair &&
            (selected.Targets.Count != 2 ||
             Hand(Player(selected.Targets[0])).Count >= Hand(Player(selected.Targets[1])).Count))
            throw new InvalidOperationException("The selected unequal-hand pair is no longer legal.");
        if (targetKind == SkillProgramTargetKind.OtherLivingPair &&
            (selected.Targets.Count != 2 ||
             Hand(Player(selected.Targets[0])).Count > Hand(Player(selected.Targets[1])).Count))
            throw new InvalidOperationException("The selected pair must be two players in ascending hand order.");
        if (targetKind == SkillProgramTargetKind.LivingPairDistinct &&
            (selected.Targets.Count != 2 ||
             selected.Targets[0] == selected.Targets[1]))
            throw new InvalidOperationException("The selected ordered pair must name two distinct players.");
        prompts.Clear();
        frames.ReplaceTop(frame with
        {
            SelectedTargetSeats = Array.AsReadOnly(selected.Targets.ToArray())
        });
        frames.AdvanceProgram(frame.Id);
        return;
    }
}
