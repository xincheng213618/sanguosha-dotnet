using System.Globalization;

namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
internal sealed class ShiTaiShiCiRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramOperationDraws draws, IShiTaiShiCiFacts facts,
    IShiTaiShiCiOperations operations) : IShiTzProgramHost
{
    private CardUseFrame? LifecycleCardUse(long id) => frames.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == id);
    private const string ShiTzHanzhan = ShiTaiShiCiComposition.HanzhanSkill,        ShiTzZhanlie = ShiTaiShiCiComposition.ZhanlieSkill,ShiTzZhenfeng = ShiTaiShiCiComposition.ZhenfengSkill;

    private static CardConversionSource ShiTzSource(ProgramSkillFrame f) =>
        new(f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private bool HasShiTzCapability => facts.HasActivationOperation(SkillProgramEffectOp.ResolveShiTzHanzhan);
    private int ShiTzLieCount() => facts.History<ShiTzLieGainedEvent>().Count();
    private int? ShiTzFixedX() => facts.History<ShiTzZhenfengFixedEvent>().LastOrDefault()?.Value;
    private int ShiTzHanzhanHandCap(ShiTaiShiCiPlayer player) => ShiTzFixedX() ?? player.MaxHp;
    private int ShiTzZhanlieRange() => ShiTzFixedX() ?? facts.AttackRange(facts.CurrentSeat);
    private ShiTzSlashIssuedEvent? ShiTzIssuedSlash(long resolutionId)
    {
        if (LifecycleCardUse(resolutionId)?.Action?.ConversionChain is not [var source] || source.SkillId != ShiTzZhanlie) return null;
        var frame = frames.OfType<ProgramSkillFrame>().LastOrDefault(f => f.SkillId == ShiTzZhanlie && f.ShiTaiShiCi is { Operation: "zhanlie" });
        return frame is null ? null : facts.History<ShiTzSlashIssuedEvent>().LastOrDefault(e =>
            e.Source.OwnerSeat == frame.OwnerSeat && e.Source.SkillInstanceId == frame.SkillInstanceId &&
            e.ActualTurn == facts.TurnNumber);
    }
    private bool ShiTzGateRequired(long resolutionId) =>
        ShiTzIssuedSlash(resolutionId) is { Options: var options } && (options & ShiTaiShiCiComposition.OptionRespondCost) != 0 &&
        !facts.History<ShiTzRespondGatePaidEvent>().Any(e => e.AttackResolutionId == resolutionId);

    public SkillProgramStepOutcome ResolveHanzhan(ProgramSkillFrame frame) => BeginShiTz(frame);
    public SkillProgramStepOutcome ResolveRangeRecord(ProgramSkillFrame frame) => BeginShiTz(frame);
    public SkillProgramStepOutcome ResolveZhanlie(ProgramSkillFrame frame) => BeginShiTz(frame);
    public SkillProgramStepOutcome ResolveRespondGate(ProgramSkillFrame frame) => BeginShiTz(frame);
    public SkillProgramStepOutcome ResolveZhenfeng(ProgramSkillFrame frame) => BeginShiTz(frame);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67600)]
    private bool CanOfferShiTz(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != ShiTzZhanlie) return true;
        if (trigger.Id == ShiTaiShiCiComposition.ZhanlieGateTrigger)
            return ShiTzGateAttack(context) is { } attack && ShiTzGateRequired(attack.ResolutionId);
        if (trigger.Id == ShiTaiShiCiComposition.ZhanlieEndingTrigger)
            return ShiTzLieCount() > 0;
        return true;
    }

    private ShiTaiShiCiAttack? ShiTzGateAttack(ProgramSkillWindowContext context)
    {
        if (facts.Winner != Winner.None || frames.OfType<ProgramCardTriggerWindowFrame>()
            .SingleOrDefault(item => item.Id == context.ParentFrameId) is not { AttackOwnerFrameId: { } ownerId })
            return null;
        var attack = facts.Attack(ownerId);
        return context is { Window: SkillProgramTriggerWindow.SlashBeforeResponse, CardUse: { } action } &&
            context.TargetSeat == attack.TargetSeat && LifecycleCardUse(attack.ResolutionId)?.Action?.ActionId == action.CardActionId &&
            RuntimeSkills.RuntimeRules_Core_CommonRules.IsSlashCard(attack.EffectiveCardKind ?? CardKind.Slash) && facts.ActiveAttackId == attack.ResolutionId ? attack : null;
    }

    private SkillProgramStepOutcome BeginShiTz(ProgramSkillFrame input)
    {
        var f = frames.ActiveProgram(input.Id);
        if (f.ShiTaiShiCi is not null) throw new InvalidOperationException("Shi Tai Shi Ci restarted a running operation.");
        var r = f.ActivationId == ShiTaiShiCiComposition.HanzhanActivation && f.TriggerId is null && f.SkillId == ShiTzHanzhan
            ? StartShiTzHanzhan(f)
            : f.ActivationId == ShiTaiShiCiComposition.ZhenfengActivation && f.TriggerId is null && f.SkillId == ShiTzZhenfeng
                ? StartShiTzZhenfeng(f)
                : f.SkillId == ShiTzZhanlie && f.TriggerId == ShiTaiShiCiComposition.ZhanlieRangeTrigger ? StartShiTzRange(f)
                : f.SkillId == ShiTzZhanlie && f.TriggerId == ShiTaiShiCiComposition.ZhanlieEndingTrigger ? StartShiTzZhanlie(f)
                : f.SkillId == ShiTzZhanlie && f.TriggerId == ShiTaiShiCiComposition.ZhanlieGateTrigger ? StartShiTzGate(f)
                : throw new InvalidOperationException("Shi Tai Shi Ci instruction has an incompatible owning skill.");
        frames.ReplaceTop(f = f with { ShiTaiShiCi = r });
        if (r.Stage == 30)
        {
            ContinueShiTz(f.Id);
            return SkillProgramStepOutcome.AwaitChild;
        }
        ContinueShiTz(f.Id);
        return r.Operation == "hanzhan" ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.AwaitChoice;
    }

    private ShiTaiShiCiReceipt StartShiTzHanzhan(ProgramSkillFrame f)
    {
        if (f.SelectedTargetSeats is not [var owner, var counterpart] || owner != f.OwnerSeat || owner == counterpart ||
            !facts.ValidSeat(counterpart))
            throw new InvalidOperationException("Hanzhan needs itself and one other living participant as ordered rally pair.");
        var r = new ShiTaiShiCiReceipt("hanzhan", facts.TurnNumber, CounterpartSeat: counterpart)
        { Source = ShiTzSource(f), GameplayHash = f.GameplayHash };
        events.AdvanceFact(new ShiTzHanzhanStartedEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber, counterpart));
        return r;
    }

    private ShiTaiShiCiReceipt StartShiTzZhenfeng(ProgramSkillFrame f) =>
        new("zhenfeng", facts.TurnNumber) { Source = ShiTzSource(f), GameplayHash = f.GameplayHash };

    private ShiTaiShiCiReceipt StartShiTzRange(ProgramSkillFrame f) =>
        new("range", facts.TurnNumber) { Source = ShiTzSource(f), GameplayHash = f.GameplayHash };

    private ShiTaiShiCiReceipt StartShiTzZhanlie(ProgramSkillFrame f)
    {
        if (ShiTzLieCount() <= 0) throw new InvalidOperationException("Zhanlie ending requires at least one accumulated lie marker.");
        return new("zhanlie", facts.TurnNumber) { Source = ShiTzSource(f), GameplayHash = f.GameplayHash };
    }

    private ShiTaiShiCiReceipt StartShiTzGate(ProgramSkillFrame f)
    {
        var attack = ShiTzGateAttack(f.WindowContext ??
            throw new InvalidOperationException("Zhanlie gate lost its native slash window.")) ??
            throw new InvalidOperationException("Zhanlie gate lost its current battle slash.");
        var target = attack.TargetSeat;
        if (facts.Hand(target).Count == 0)
        {
            // No hand card can buy response qualification; the Slash loses its effect on this target.
            events.AdvanceFact(new ShiTzRespondGatePaidEvent(f.Id, attack.ResolutionId, target, false));
            operations.MarkEffectIneffective(attack.ResolutionId, target);
            events.AdvanceFact(new CardEffectSkippedEvent(attack.ResolutionId, attack.CardUserSeat, target,
                attack.EffectiveCardKind ?? CardKind.Slash, CardEffectSkipReason.SkillNullified));
            return new("gate", facts.TurnNumber, Stage: 30, TargetSeat: target)
            { Source = ShiTzSource(f), GameplayHash = f.GameplayHash, AttackResolutionId = attack.ResolutionId };
        }
        return new("gate", facts.TurnNumber, TargetSeat: target)
        { Source = ShiTzSource(f), GameplayHash = f.GameplayHash, AttackResolutionId = attack.ResolutionId };
    }

    // ---- choice publication -------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiTzChoices(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!; var choices = new List<PromptChoice>();
        void Add(string branch, string label, int? target = null, IReadOnlyList<int>? cards = null) => choices.Add(new(
            new($"shi-tai-shi-ci.{f.Id}.{r.Stage}.{branch}.{target ?? -1}"), label, cards ?? [],
            target is { } seat ? [seat] : [], new Dictionary<string, string>
            {
                ["program-action"] = ShiTaiShiCiComposition.ZhanlieProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch,
            }));
        if (r.Operation == "zhenfeng" && r.Stage == 0)
        {
            var owner = facts.Player(f.OwnerSeat);
            Add("recover", "回复2点体力");
            Add("fix-hp", $"将“酣战”和“战烈”的X固定为当前体力值（{owner.Hp}）");
            Add("fix-lost", $"将“酣战”和“战烈”的X固定为已损失体力值（{owner.MaxHp - owner.Hp}）");
            Add("fix-alive", $"将“酣战”和“战烈”的X固定为存活角色数（{facts.LivingCount}）");
        }
        else if (r.Operation == "zhanlie" && r.Stage == 3)
        {
            var selected = r.TargetSeats!;
            var maximum = (r.Options & ShiTaiShiCiComposition.OptionExtraTarget) != 0 ? 2 : 1;
            foreach (var seat in facts.Targets(f.OwnerSeat, SkillProgramTargetKind.OtherLivingSlashable))
                Add(selected.Contains(seat) ? "unpick" : "pick",
                    (selected.Contains(seat) ? "取消选择 " : "选择 ") + facts.Player(seat).Name, seat);
            if (selected.Count > 0)
                Add("slash", $"视为使用无次数限制的【杀】（已选 {selected.Count}/{maximum} 名目标）");
        }
        else if (r.Operation == "gate" && r.Stage == 0)
        {
            foreach (var choice in facts.HandDiscardChoices(f.Id, r.TargetSeat, r.TargetSeat))
                choices.Add(choice with { Parameters = new Dictionary<string, string>(choice.Parameters)
                    { ["program-action"] = ShiTaiShiCiComposition.ZhanlieProgramAction, ["branch"] = "gate-pay" } });
        }
        return Array.AsReadOnly(choices.ToArray());
    }

    private IReadOnlyList<PromptChoice> ShiTzOptionChoices(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        var y = ShiTzLieCount() / 3;
        var chosen = ShiTzCountBits(r.Options);
        var choices = new List<PromptChoice>();
        void Add(string option, string label) => choices.Add(new(
            new($"shi-tai-shi-ci.{f.Id}.0.{option}"), label, [], [], new Dictionary<string, string>
            {
                ["program-action"] = ShiTaiShiCiComposition.ZhanlieProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = "option", ["option"] = option,
            }));
        Add("extra-target", "此杀目标数+1");
        Add("extra-damage", "此杀基础伤害+1");
        Add("respond-cost", "此杀需目标角色额外弃置一张手牌才能响应");
        Add("reward-draw", "此杀结算后你摸两张牌");
        var result = choices.Where(choice => (r.Options & ShiTzOptionFlag(choice.Parameters["option"])) == 0 && chosen < y).ToList();
        if (chosen > 0 || y == 0)
            result.Add(new(new($"shi-tai-shi-ci.{f.Id}.0.done"),
                y == 0 ? "移去全部“烈”，视为使用一张无次数限制的【杀】" : $"确认（已选 {chosen}/{y} 项）", [], [],
                new Dictionary<string, string>
                { ["program-action"] = ShiTaiShiCiComposition.ZhanlieProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "done" }));
        return Array.AsReadOnly(result.ToArray());
    }

    private static int ShiTzOptionFlag(string option) => option switch
    {
        "extra-target" => ShiTaiShiCiComposition.OptionExtraTarget,
        "extra-damage" => ShiTaiShiCiComposition.OptionExtraDamage,
        "respond-cost" => ShiTaiShiCiComposition.OptionRespondCost,
        _ => ShiTaiShiCiComposition.OptionRewardDraw,
    };

    private static int ShiTzCountBits(int value)
    {
        var count = 0;
        while (value != 0) { count += value & 1; value >>= 1; }
        return count;
    }

    private void PublishShiTz(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        var choices = r.Operation == "zhanlie" && r.Stage == 0 ? ShiTzOptionChoices(f) : ShiTzChoices(f);
        if (choices.Count == 0 || facts.Winner != Winner.None ||
            r.Operation != "gate" && !facts.Player(f.OwnerSeat).IsAlive ||
            r.Operation == "gate" && !facts.Player(r.TargetSeat).IsAlive)
        { FinishShiTzOperation(f); return; }
        var skill = facts.Skill(f.SkillId);
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var chooser = r.Operation == "gate" ? r.TargetSeat : f.OwnerSeat;
        prompts.Set(new(DecisionKind.ProgramTrigger, chooser,
            r.Operation switch
            {
                "zhenfeng" => "振锋：选择一项。",
                "zhanlie" => r.Stage == 0 ? $"战烈：至多选择 {ShiTzLieCount() / 3} 项（当前 {ShiTzLieCount()} 枚“烈”）。" : "战烈：选择此杀的目标。",
                _ => "战烈：弃置一张手牌以获得响应此杀的资格。",
            },
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = prompts.NextId(), IsPrivate = true, TargetSeat = chooser, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        }, facts.Player(chooser).IsHuman);
        operations.PublishState();
    }

    [ProgramAnswer(ShiTaiShiCiComposition.ZhanlieProgramAction)]
    private void AnswerShiTz(PromptChoice selected)
    {
        if (frames.Top is not ProgramSkillFrame { ShiTaiShiCi: { } r } f ||
            prompts.Current?.PlayerSeat != (r.Operation == "gate" ? r.TargetSeat : f.OwnerSeat) ||
            !(r.Operation == "zhanlie" && r.Stage == 0 ? ShiTzOptionChoices(f) : ShiTzChoices(f))
                .Any(choice => RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Tai Shi Ci choice lost its current frame, chooser or legal branch.");
        prompts.Clear();
        frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, branch)
        {
            case ("zhenfeng", "recover"):
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 9 } });
                ContinueShiTz(f.Id);
                return;
            case ("zhenfeng", _):
            {
                var kind = branch switch { "fix-hp" => "hp", "fix-lost" => "lost", _ => "alive" };
                var value = kind switch
                {
                    "hp" => facts.Player(f.OwnerSeat).Hp,
                    "lost" => facts.Player(f.OwnerSeat).MaxHp - facts.Player(f.OwnerSeat).Hp,
                    _ => facts.LivingCount,
                };
                events.AdvanceFact(new ShiTzZhenfengFixedEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber, kind, value));
                FinishShiTzOperation(frames.ActiveProgram(f.Id));
                return;
            }
            case ("zhanlie", "option"):
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Options = r.Options | ShiTzOptionFlag(selected.Parameters["option"]) } });
                PublishShiTz(frames.ActiveProgram(f.Id));
                return;
            case ("zhanlie", "done") when r.Stage == 0:
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 3, TargetSeats = Array.Empty<int>() } });
                PublishShiTz(frames.ActiveProgram(f.Id));
                return;
            case ("zhanlie", "pick") when r.Stage == 3:
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { TargetSeats = r.TargetSeats!.Append(selected.Targets.Single()).ToArray() } });
                PublishShiTz(frames.ActiveProgram(f.Id));
                return;
            case ("zhanlie", "unpick") when r.Stage == 3:
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { TargetSeats = r.TargetSeats!.Where(s => s != selected.Targets.Single()).ToArray() } });
                PublishShiTz(frames.ActiveProgram(f.Id));
                return;
            case ("zhanlie", "slash") when r.Stage == 3:
            {
                var targets = r.TargetSeats ?? throw new InvalidOperationException("Zhanlie slash lost its chosen targets.");
                events.AdvanceFact(new ShiTzSlashIssuedEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    targets.ToArray(), r.Options));
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 6, SlashCursor = 0 } });
                ContinueShiTz(f.Id);
                return;
            }
            case ("gate", "gate-pay"):
                events.AdvanceFact(new ShiTzRespondGatePaidEvent(f.Id, r.AttackResolutionId, r.TargetSeat, true));
                frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 18, Before = facts.MovementSequence },
                    PendingMovementContinuation = new(r.TargetSeat, 0, null) });
                operations.LogicalDiscard(f, r.TargetSeat, [selected.Cards.Single()], new(ShiTaiShiCiComposition.HanzhanGateMoveReason));
                return;
            default:
                throw new InvalidOperationException("Unknown Shi Tai Shi Ci choice branch.");
        }
    }

    // ---- continuations ------------------------------------------------------

    private void ContinueShiTz(long id)
    {
        if (prompts.Current is not null || frames.Top is not ProgramSkillFrame { ShiTaiShiCi: { } r } f || f.Id != id) return;
        if (r.Stage is 6 or 8 or 12 or 14 or 18)
        {
            if (DrainShiTz(f)) return;
            f = frames.ActiveProgram(id);
            frames.ReplaceTop(f = f with { PendingMovementContinuation = null, ShiTaiShiCi = f.ShiTaiShiCi! with { Draw = null } });
            r = f.ShiTaiShiCi!;
        }
        switch (r.Operation, r.Stage)
        {
            case ("hanzhan", 0) or ("hanzhan", 6) or ("hanzhan", 8):
                ContinueShiTzHanzhanRally(frames.ActiveProgram(id));
                return;
            case ("range", 0):
            {
                events.AdvanceFact(new ShiTzRangeRecordedEvent(facts.TurnNumber, facts.CurrentSeat, ShiTzZhanlieRange(), ShiTzFixedX()));
                FinishShiTzOperation(frames.ActiveProgram(id));
                return;
            }
            case ("zhenfeng", 0) or ("zhanlie", 0) or ("zhanlie", 3) or ("gate", 0):
                PublishShiTz(frames.ActiveProgram(id));
                return;
            case ("zhanlie", 6):
                ContinueShiTzZhanlieSlashes(frames.ActiveProgram(id));
                return;
            case ("zhanlie", 16):
                ContinueShiTzZhanlieSlashes(frames.ActiveProgram(id) with
                { ShiTaiShiCi = f.ShiTaiShiCi! with { SlashCursor = f.ShiTaiShiCi.SlashCursor + 1, Stage = 6 } });
                return;
            case ("zhanlie", 12) or ("zhanlie", 14):
                ContinueShiTzZhanlieReward(frames.ActiveProgram(id));
                return;
            case ("gate", 18) or ("gate", 30):
                FinishShiTzOperation(frames.ActiveProgram(id));
                return;
            case ("zhenfeng", 9):
                ContinueShiTzZhenfengRecovery(frames.ActiveProgram(id));
                return;
            case ("zhenfeng", 12):
                FinishShiTzOperation(frames.ActiveProgram(id));
                return;
            default:
                throw new InvalidOperationException("Shi Tai Shi Ci has an unknown continuation stage.");
        }
    }

    private void ContinueShiTzHanzhanRally(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        var participants = new[] { f.OwnerSeat, r.CounterpartSeat };
        for (var cursor = r.DrawCursor; cursor < participants.Length * 8; cursor++)
        {
            var seat = participants[cursor % participants.Length];
            if (!facts.Player(seat).IsAlive) continue;
            var missing = ShiTzHanzhanHandCap(facts.Player(seat)) - facts.Hand(seat).Count;
            if (missing <= 0) continue;
            frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { DrawCursor = cursor, Stage = 6 } });
            if (draws.BeginOperationProgramDraw(frames.ActiveProgram(f.Id), seat, Math.Min(3, missing),
                    new(ShiTaiShiCiComposition.HanzhanRallyDrawReason)) == SkillProgramStepOutcome.AwaitChild) return;
            ContinueShiTz(f.Id);
            return;
        }
        if (!facts.Player(f.OwnerSeat).IsAlive || !facts.Player(r.CounterpartSeat).IsAlive || f.OwnerSeat == r.CounterpartSeat)
        { FinishShiTzOperation(frames.ActiveProgram(f.Id)); return; }
        frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 9, DrawCursor = participants.Length * 8 } });
        FinishShiTzOperation(frames.ActiveProgram(f.Id));
    }

    private void ContinueShiTzZhanlieSlashes(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        var targets = r.TargetSeats!;
        while (r.SlashCursor < targets.Count && !facts.Player(targets[r.SlashCursor]).IsAlive) r = r with { SlashCursor = r.SlashCursor + 1 };
        if (r.SlashCursor >= targets.Count)
        {
            if ((r.Options & ShiTaiShiCiComposition.OptionRewardDraw) != 0 && facts.Player(f.OwnerSeat).IsAlive)
            { frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 12 } }); ContinueShiTz(f.Id); return; }
            FinishShiTzOperation(frames.ActiveProgram(f.Id)); return;
        }
        var targetSeat = targets[r.SlashCursor];
        frames.ReplaceTop(f = f with { SelectedTargetSeats = Array.AsReadOnly(new[] { targetSeat }),
            ShiTaiShiCi = r with { Stage = 16 } });
        if (operations.BeginVirtualCardUse(frames.ActiveProgram(f.Id), targetSeat, CardKind.Slash, false) == SkillProgramStepOutcome.AwaitChild) return;
        ContinueShiTz(f.Id);
    }

    private void ContinueShiTzZhanlieReward(ProgramSkillFrame f)
    {
        // The settlement draw is an ordinary completed draw; it needs no producer return.
        operations.DrawCards(f.Id, f.OwnerSeat, 2,
            new(ShiTaiShiCiComposition.ZhanlieRewardDrawReason));
        FinishShiTzOperation(frames.ActiveProgram(f.Id));
    }

    private void ContinueShiTzZhenfengRecovery(ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        if (!facts.Player(f.OwnerSeat).IsAlive || facts.Player(f.OwnerSeat).Hp >= facts.Player(f.OwnerSeat).MaxHp)
        { FinishShiTzOperation(frames.ActiveProgram(f.Id)); return; }
        // Recovery is issued once before its replacement/HP-change children.
        // BeginRecovery alone only creates a primitive frame; the shared host
        // applies the real clamped recovery and retains any queued invoice.
        frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { Stage = 12 },
            PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        operations.Recover(f.Id, f.OwnerSeat, f.OwnerSeat, 2);
        ContinueShiTz(f.Id);
    }

    private bool DrainShiTz(ProgramSkillFrame f) => operations.DrainChildWindows(f.Id);

    private void FinishShiTzOperation(ProgramSkillFrame f)
    {
        if (f.ShiTaiShiCi!.Operation is "hanzhan")
        {
            // The rally is the first of two instructions; advancing runs the virtual Duel.
            frames.ReplaceTop(frames.ActiveProgram(f.Id) with { ShiTaiShiCi = null });
            frames.AdvanceProgram(f.Id);
            return;
        }
        if (f.ShiTaiShiCi.Operation is "zhanlie" or "zhenfeng")
        { operations.Finish(f, true); return; }
        frames.ReplaceTop(frames.ActiveProgram(f.Id) with { ShiTaiShiCi = null });
        frames.AdvanceProgram(f.Id);
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShiTzHanzhan)]
    private void ReturnShiTzHanzhanDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        if (f.ShiTaiShiCi is not { Operation: "hanzhan", Stage: 6, Draw: null } r ||
            (outcome.RecipientSeat != f.OwnerSeat && outcome.RecipientSeat != r.CounterpartSeat) || outcome.RequestedCount is not (> 0 and <= 3))
            throw new InvalidOperationException("Hanzhan lost its original once-issued rally draw.");
        events.AdvanceFact(new ShiTzHanzhanRallyDrawnEvent(f.Id, outcome.RecipientSeat, outcome.RequestedCount,
            outcome.ActualDrawIds.Count + outcome.ReplacementGainedIds.Count));
        frames.ReplaceTop(f = f with { ShiTaiShiCi = r with { DrawCursor = r.DrawCursor + 1, Draw = outcome,
            Before = outcome.SequenceBefore, Stage = 8 }, PendingMovementContinuation = new(outcome.RecipientSeat, 0, null) });
        ContinueShiTz(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 67600)]
    private bool ResumeShiTz(long id)
    {
        if (frames.Top is not ProgramSkillFrame { ShiTaiShiCi: not null } f || f.Id != id) return false;
        ContinueShiTz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 67600)]
    private bool ReturnShiTzMovement(ProgramSkillFrame f) => f.ShiTaiShiCi is not null && ResumeShiTz(f.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 67600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 67600)]
    private bool ContinuePaidShiTz(ProgramSkillFrame f) => f.ShiTaiShiCi is { Stage: > 0 };

    internal int ZhanlieDamageBonus(long? programFrameId)
    {
        if (programFrameId is not { } frameId || frames.Find(frameId) is not ProgramSkillFrame { ShiTaiShiCi: { Operation: "zhanlie" } r })
            return 0;
        return (r.Options & ShiTaiShiCiComposition.OptionExtraDamage) != 0 ? 1 : 0;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 67600)]
    private bool ShiTzMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) =>
        f.ShiTaiShiCi is { } r && (r.Stage is 8 or 14 or 18 || r is { Operation: "zhenfeng", Stage: 12 }) &&
        p.CoverageResultBind is null && p.BeforeCount == 0 &&
        p.SubjectSeat == (r.Stage == 18 ? r.TargetSeat : f.OwnerSeat);

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiTzZhanlie, SkillProgramEffectOp.ResolveShiTzZhenfeng,
        SkillProgramEffectOp.ResolveShiTzRespondGate)]
    private PromptChoice ChooseShiTzAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShiTaiShiCi!;
        if (r.Operation == "zhenfeng")
        {
            var owner = facts.Player(f.OwnerSeat);
            return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "recover" && owner.Hp < owner.MaxHp) ??
                decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"].StartsWith("fix")) ?? decision.Choices[0];
        }
        if (r.Operation == "zhanlie")
        {
            if (r.Stage == 0)
            {
                var y = ShiTzLieCount() / 3;
                return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "option" && y > 0 &&
                    (ShiTzOptionFlag(choice.Parameters["option"]) & (ShiTaiShiCiComposition.OptionExtraDamage |
                        ShiTaiShiCiComposition.OptionRewardDraw)) != 0) ??
                    decision.Choices.Single(choice => choice.Parameters["branch"] == "done");
            }
            var maximum = (r.Options & ShiTaiShiCiComposition.OptionExtraTarget) != 0 ? 2 : 1;
            var pick = decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "pick" &&
                !facts.Allies(f.OwnerSeat, choice.Targets.Single()));
            return (r.TargetSeats?.Count ?? 0) < maximum && pick is not null ? pick :
                decision.Choices.Single(choice => choice.Parameters["branch"] == "slash");
        }
        return decision.Choices
            .OrderBy(choice => facts.KeepValue(r.TargetSeat, facts.Hand(r.TargetSeat).Single(card => card.Id == choice.Cards[0])))
            .ThenBy(choice => choice.Cards[0]).First();
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 67600)]
    private void ObserveShiTzDiscardedSlashes(IGameEvent payload)
    {
        if (!HasShiTzCapability || facts.TurnNumber <= 0 || facts.Winner != Winner.None) return;
        var range = facts.History<ShiTzRangeRecordedEvent>().LastOrDefault(e => e.TurnNumber == facts.TurnNumber)?.Range ?? 0;
        if (range <= 0) return;
        foreach (var movement in facts.Movements.Skip(facts.LieScanSequence).ToArray())
        {
            operations.SetLieScanSequence(movement.Sequence);
            if (movement.TurnNumber != facts.TurnNumber || movement.To != CardLocation.DiscardPile ||
                !ShiTaiShiCiComposition.IsSlashKind(movement.CardKind)) continue;
            var before = facts.History<ShiTzLieGainedEvent>().Count(e => e.TurnNumber == facts.TurnNumber);
            if (before >= range || before >= ShiTaiShiCiComposition.MaxLieCount ||
                facts.History<ShiTzLieGainedEvent>().Any(e => e.TurnNumber == facts.TurnNumber && e.CardId == movement.CardId)) continue;
            var total = before + 1;
            events.AdvanceFact(new ShiTzLieGainedEvent(facts.TurnNumber, facts.CurrentSeat, movement.CardId, total, total));
        }
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> PublicStates(int owner, string skillId)
    {
        if (!HasShiTzCapability || skillId != ShiTzZhanlie) return [];
        var lies = ShiTzLieCount();
        return Array.AsReadOnly(facts.ProgramInstances(owner).Where(i => i.SkillId == ShiTzZhanlie).SelectMany(i => new[]
        {
            new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "zhanlie-lies", ProgramPublicRuleStateKind.GameActivationProgress,
                ShiTaiShiCiComposition.MaxLieCount, lies, SkillUsageScope.Game),
            new ProgramPublicRuleStateSnapshot(i.SkillInstanceId, "zhanlie-y", ProgramPublicRuleStateKind.GameActivationProgress,
                2, lies / 3, SkillUsageScope.Game),
        }).ToArray());
    }
}
