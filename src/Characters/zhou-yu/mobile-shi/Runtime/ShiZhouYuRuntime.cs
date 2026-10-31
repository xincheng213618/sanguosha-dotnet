using System.Globalization;

namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
internal sealed class ShiZhouYuRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IShiZhouYuFacts facts, IShiZhouYuOperations operations) : IShiZhouYuHost
{
    private const string ShiZhouYuChiyun = ShiZhouYuComposition.ChiyunSkill,ShiZhouYuYanhui = ShiZhouYuComposition.YanhuiSkill,        ShiZhouYuFentao = ShiZhouYuComposition.FentaoSkill,ShiZhouYuXiongzi = ShiZhouYuComposition.XiongziSkill;

    private CardConversionSource ShiZhouYuSource(ProgramSkillFrame f) => facts.Source(f);
    private bool HasShiZhouYuCapability => facts.HasTriggerOperation(SkillProgramEffectOp.ResolveShiZhouYuChiyun);

    // ---- ledger-backed facts ---------------------------------------------------

    private int ShiZhouYuKeptOption(int ownerSeat) =>
        !HasShiZhouYuCapability ? 0 :
        facts.History<ShiZhouYuXiongziActivatedEvent>().LastOrDefault(entry => entry.OwnerSeat == ownerSeat &&
            entry.KeptOption is 1 or 2)?.KeptOption ?? 0;

    private bool ShiZhouYuChiyunPhaseSeen(int ownerSeat) =>
        facts.History<ShiZhouYuChiyunPhaseSeenEvent>().Any(entry =>
            entry.TurnNumber == facts.TurnNumber && entry.Phase == facts.Phase && entry.OwnerSeat == ownerSeat);

    private int ShiZhouYuYanhuiShownPlayers(int turnNumber) =>
        facts.History<ShiZhouYuYanhuiShownEvent>().Where(entry => entry.TurnNumber == turnNumber)
            .Select(entry => entry.HolderSeat).Distinct().Count();

    private int[] ShiZhouYuYanhuiVictims(int turnNumber) =>
        facts.History<ShiZhouYuYanhuiShownEvent>().Where(entry => entry.TurnNumber == turnNumber && entry.Discarded)
            .Select(entry => entry.HolderSeat).Distinct().Order().ToArray();

    private bool ShiZhouYuCardShownBefore(int turnNumber, int cardId) =>
        facts.History<ShiZhouYuYanhuiShownEvent>().Any(entry => entry.TurnNumber == turnNumber && entry.CardId == cardId);

    private bool ShiZhouYuFentaoPendingRechain(int ownerSeat, int targetSeat) =>
        facts.History<ShiZhouYuFentaoChosenEvent>().Any(entry => entry.Option == 2 &&
            entry.Source.OwnerSeat == ownerSeat && entry.TargetSeat == targetSeat && entry.ActualTurn == facts.TurnNumber &&
            !facts.History<ShiZhouYuFentaoRechainedEvent>().Any(done =>
                done.Source.OwnerSeat == ownerSeat && done.TargetSeat == targetSeat && done.ActualTurn == facts.TurnNumber));

    // ---- trigger legality -------------------------------------------------------

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67800,
        SkillProgramEffectOp.ResolveShiZhouYuChiyun, SkillProgramEffectOp.ResolveShiZhouYuYanhuiShow,
        SkillProgramEffectOp.ResolveShiZhouYuYanhuiEnding, SkillProgramEffectOp.ResolveShiZhouYuFentao,
        SkillProgramEffectOp.ResolveShiZhouYuFentaoRechain, SkillProgramEffectOp.ResolveShiZhouYuXiongzi)]
    private bool CanOfferShiZhouYu(ProgramTriggerLegalityContext gate)
    {
        if (facts.Winner != Winner.None || facts.TurnNumber <= 0 || !facts.Player(gate.OwnerSeat).IsAlive) return false;
        if (gate.SkillId == ShiZhouYuChiyun)
            return !ShiZhouYuChiyunPhaseSeen(gate.OwnerSeat) && facts.Hand(gate.OwnerSeat).Count > 0 &&
                facts.Players.Any(player => player.IsAlive && player.Seat != gate.OwnerSeat);
        if (gate.SkillId == ShiZhouYuYanhui && gate.Trigger.Id == ShiZhouYuComposition.YanhuiTargetTrigger)
            return gate.Context is { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } use } &&
                use.EventTargetSeat is { } target && target != gate.OwnerSeat && facts.ValidSeat(target) &&
                facts.Player(target).IsAlive && facts.Hand(target).Count > 0;
        if (gate.SkillId == ShiZhouYuYanhui)
            return ShiZhouYuYanhuiShownPlayers(facts.TurnNumber) > 0;
        if (gate.SkillId == ShiZhouYuFentao && gate.Trigger.Id == ShiZhouYuComposition.FentaoFireTrigger)
            return gate.Context is { Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                    TargetSeat: { } damaged, Amount: > 0 } &&
                damaged != gate.OwnerSeat && facts.ValidSeat(damaged) && facts.Player(damaged).IsAlive &&
                facts.Player(damaged).IsChained && facts.Damage is { } attack &&
                attack.TargetSeat == damaged && attack.Nature == DamageNature.Fire;
        if (gate.SkillId == ShiZhouYuFentao)
            return gate.Context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } settled } &&
                facts.ValidSeat(settled) && facts.Player(settled).IsAlive && ShiZhouYuFentaoPendingRechain(gate.OwnerSeat, settled);
        if (gate.SkillId == ShiZhouYuXiongzi)
            return facts.CurrentSeat == gate.OwnerSeat && !facts.History<ShiZhouYuXiongziActivatedEvent>()
                .Any(entry => entry.OwnerSeat == gate.OwnerSeat);
        return true;
    }

    // 雄姿 mode: the three foundation skills may only trigger during the owner's own turn.
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 67801,
        SkillProgramEffectOp.ResolveShiZhouYuChiyun, SkillProgramEffectOp.ResolveShiZhouYuYanhuiShow,
        SkillProgramEffectOp.ResolveShiZhouYuYanhuiEnding, SkillProgramEffectOp.ResolveShiZhouYuFentao)]
    private bool CanOfferShiZhouYuUnderXiongzi(ProgramTriggerLegalityContext gate)
    {
        var kept = ShiZhouYuKeptOption(gate.OwnerSeat);
        return kept == 0 || facts.CurrentSeat == gate.OwnerSeat;
    }

    public SkillProgramStepOutcome ResolveChiyun(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuChiyun);
    public SkillProgramStepOutcome ResolveChiyunOption(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuChiyunOption);
    public SkillProgramStepOutcome ResolveYanhuiShow(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuYanhuiShow);
    public SkillProgramStepOutcome ResolveYanhuiEnding(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuYanhuiEnding);
    public SkillProgramStepOutcome ResolveFentao(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuFentao);
    public SkillProgramStepOutcome ResolveFentaoRechain(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuFentaoRechain);
    public SkillProgramStepOutcome ResolveXiongzi(ProgramSkillFrame frame) => BeginShiZhouYu(frame, SkillProgramEffectOp.ResolveShiZhouYuXiongzi);

    private SkillProgramStepOutcome BeginShiZhouYu(ProgramSkillFrame input, SkillProgramEffectOp op)
    {
        var f = frames.ActiveProgram(input.Id);
        if (f.ShiZhouYu is not null && op != SkillProgramEffectOp.ResolveShiZhouYuChiyunOption)
            throw new InvalidOperationException("Shi Zhou Yu restarted a running operation.");
        var r = op switch
        {
            SkillProgramEffectOp.ResolveShiZhouYuChiyun when f.SkillId == ShiZhouYuChiyun && f.TriggerId == ShiZhouYuComposition.ChiyunTrigger =>
                StartShiZhouYuChiyun(f),
            SkillProgramEffectOp.ResolveShiZhouYuChiyunOption when f.SkillId == ShiZhouYuChiyun &&
                f.ShiZhouYu is { Operation: "chiyun-option", Stage: ShiZhouYuComposition.ChiyunStageOption } => f.ShiZhouYu!,
            SkillProgramEffectOp.ResolveShiZhouYuYanhuiShow when f.SkillId == ShiZhouYuYanhui &&
                f.TriggerId == ShiZhouYuComposition.YanhuiTargetTrigger && f.ShiZhouYu is null => StartShiZhouYuYanhuiShow(f),
            SkillProgramEffectOp.ResolveShiZhouYuYanhuiEnding when f.SkillId == ShiZhouYuYanhui &&
                f.TriggerId == ShiZhouYuComposition.YanhuiEndingTrigger && f.ShiZhouYu is null => StartShiZhouYuEnding(f),
            SkillProgramEffectOp.ResolveShiZhouYuFentao when f.SkillId == ShiZhouYuFentao &&
                f.TriggerId == ShiZhouYuComposition.FentaoFireTrigger && f.ShiZhouYu is null => StartShiZhouYuFentao(f),
            SkillProgramEffectOp.ResolveShiZhouYuFentaoRechain when f.SkillId == ShiZhouYuFentao &&
                f.TriggerId == ShiZhouYuComposition.FentaoRechainTrigger && f.ShiZhouYu is null => StartShiZhouYuRechain(f),
            SkillProgramEffectOp.ResolveShiZhouYuXiongzi when f.SkillId == ShiZhouYuXiongzi &&
                f.TriggerId == ShiZhouYuComposition.XiongziTrigger && f.ShiZhouYu is null =>
                new ShiZhouYuReceipt("xiongzi", facts.TurnNumber) { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash },
            _ => throw new InvalidOperationException("Shi Zhou Yu instruction has an incompatible owning skill."),
        };
        frames.ReplaceTop(f = f with { ShiZhouYu = r });
        ContinueShiZhouYu(f.Id);
        return prompts.Current is not null ? SkillProgramStepOutcome.AwaitChoice : SkillProgramStepOutcome.AwaitChild;
    }

    private ShiZhouYuReceipt StartShiZhouYuChiyun(ProgramSkillFrame f)
    {
        _ = f.WindowContext ?? throw new InvalidOperationException("Chiyun lost its gained-cards window.");
        events.AdvanceFact(new ShiZhouYuChiyunPhaseSeenEvent(facts.TurnNumber, facts.Phase, f.OwnerSeat));
        return new("chiyun", facts.TurnNumber) { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash };
    }

    private ShiZhouYuReceipt StartShiZhouYuYanhuiShow(ProgramSkillFrame f)
    {
        var use = f.WindowContext?.CardUse ??
            throw new InvalidOperationException("Yanhui lost its finalized-targets window.");
        var target = use.EventTargetSeat ?? throw new InvalidOperationException("Yanhui lost its targeted character.");
        if (target == f.OwnerSeat || !facts.ValidSeat(target) || !facts.Player(target).IsAlive || facts.Hand(target).Count == 0)
            throw new InvalidOperationException("Yanhui show lost its living other target with hand cards.");
        return new("yanhui-show", facts.TurnNumber, TargetSeat: target, Stage: ShiZhouYuComposition.YanhuiShowStagePick)
        { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash };
    }

    private ShiZhouYuReceipt StartShiZhouYuEnding(ProgramSkillFrame f)
    {
        if (ShiZhouYuYanhuiShownPlayers(facts.TurnNumber) <= 0)
            throw new InvalidOperationException("Yanhui ending requires at least one shown character this turn.");
        return new("yanhui-ending", facts.TurnNumber) { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash };
    }

    private ShiZhouYuReceipt StartShiZhouYuFentao(ProgramSkillFrame f)
    {
        var attack = facts.Damage ?? throw new InvalidOperationException("Fentao lost its current fire damage.");
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } target, Amount: > 0 } ||
            target != attack.TargetSeat || attack.TargetSeat == f.OwnerSeat || !facts.Player(target).IsChained ||
            attack.Nature != DamageNature.Fire)
            throw new InvalidOperationException("Fentao lost its chained fire-damage window.");
        return new("fentao", facts.TurnNumber, TargetSeat: target)
        { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash, AttackResolutionId = attack.ResolutionId };
    }

    private ShiZhouYuReceipt StartShiZhouYuRechain(ProgramSkillFrame f)
    {
        if (f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target } ||
            !ShiZhouYuFentaoPendingRechain(f.OwnerSeat, target))
            throw new InvalidOperationException("Fentao re-chain lost its planned discard outcome.");
        return new("fentao-rechain", facts.TurnNumber, TargetSeat: target, Stage: ShiZhouYuComposition.FentaoStageFinished)
        { Source = ShiZhouYuSource(f), GameplayHash = f.GameplayHash };
    }

    // ---- continuations -----------------------------------------------------------

    private void ContinueShiZhouYu(long id)
    {
        if (prompts.Current is not null ||
            frames.Top is not ProgramSkillFrame { ShiZhouYu: { } r } f || f.Id != id) return;
        if (r.Stage is ShiZhouYuComposition.ChiyunStageGiveIssued or ShiZhouYuComposition.FentaoStageDiscardIssued or
            ShiZhouYuComposition.YanhuiResultDiscardIssued)
        {
            if (DrainShiZhouYu(f)) return;
            f = frames.ActiveProgram(id);
            frames.ReplaceTop(f = f with { PendingMovementContinuation = null });
            r = f.ShiZhouYu!;
        }
        switch (r.Operation, r.Stage)
        {
            case ("chiyun", ShiZhouYuComposition.ChiyunStageTarget):
            case ("chiyun", ShiZhouYuComposition.ChiyunStageCards):
            case ("chiyun-option", ShiZhouYuComposition.ChiyunStageOption):
            case ("yanhui-ending", ShiZhouYuComposition.YanhuiStageEndingChoice):
            case ("fentao", ShiZhouYuComposition.FentaoStageChoice):
            case ("fentao", ShiZhouYuComposition.FentaoStageDiscardPick):
            case ("xiongzi", ShiZhouYuComposition.XiongziStageChoice):
                PublishShiZhouYu(frames.ActiveProgram(id));
                return;
            case ("chiyun", ShiZhouYuComposition.ChiyunStageGiveIssued):
                // The give batch settled; hand over to the recipient's option instruction.
                frames.ReplaceTop(frames.ActiveProgram(id) with
                { ShiZhouYu = r with { Operation = "chiyun-option", Stage = ShiZhouYuComposition.ChiyunStageOption } });
                frames.AdvanceProgram(id);
                return;
            case ("yanhui-show", ShiZhouYuComposition.YanhuiShowStagePick):
                PublishShiZhouYu(frames.ActiveProgram(id));
                return;
            case ("yanhui-show", ShiZhouYuComposition.YanhuiResultDiscardIssued):
            case ("fentao", ShiZhouYuComposition.FentaoStageDiscardIssued):
                // The discard batch settled inside a suspended parent window; hand
                // the program back to the runtime so the parent resumes exactly once.
                frames.ReplaceTop(frames.ActiveProgram(id) with { ShiZhouYu = null });
                frames.AdvanceProgram(id);
                return;
            case ("chiyun", ShiZhouYuComposition.ChiyunStageResolveDamage):
            case ("chiyun-option", ShiZhouYuComposition.ChiyunStageResolveDamage):
            case ("yanhui-ending", ShiZhouYuComposition.YanhuiStageEndingDamage):
            case ("fentao", ShiZhouYuComposition.FentaoStageFinished):
            case ("xiongzi", ShiZhouYuComposition.XiongziStageFinished):
                // The issued damage child settled through CompleteProgramAttack,
                // which resumes this frame; the damage was the final effect.
                FinishShiZhouYuOperation(frames.ActiveProgram(id));
                return;
            case ("fentao-rechain", ShiZhouYuComposition.FentaoStageFinished):
                RunShiZhouYuFentaoRechain(frames.ActiveProgram(id));
                return;
            default:
                throw new InvalidOperationException("Shi Zhou Yu has an unknown continuation stage.");
        }
    }

    private bool DrainShiZhouYu(ProgramSkillFrame f) => operations.DrainChildWindows(f.Id);

    private void FinishShiZhouYuOperation(ProgramSkillFrame f) =>
        operations.Finish(frames.ActiveProgram(f.Id), true);

    // ---- choice publication -------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiZhouYuChoices(ProgramSkillFrame f)
    {
        var r = f.ShiZhouYu!; var choices = new List<PromptChoice>();
        void Add(string branch, string label, int? target = null, IReadOnlyList<int>? cards = null) => choices.Add(new(
            new($"shi-zhou-yu.{f.Id}.{r.Stage}.{branch}.{target ?? -1}.{choices.Count}"), label, cards ?? [],
            target is { } seat ? [seat] : [], new Dictionary<string, string>
            {
                ["program-action"] = ShiZhouYuComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch,
                ["target"] = target?.ToString(CultureInfo.InvariantCulture) ?? "",
            }));
        var kept = ShiZhouYuKeptOption(f.OwnerSeat);
        switch (r.Operation, r.Stage)
        {
            case ("chiyun", ShiZhouYuComposition.ChiyunStageTarget):
            {
                foreach (var seat in facts.Players.Where(player => player.IsAlive && player.Seat != f.OwnerSeat)
                    .Select(player => player.Seat))
                    Add("target", $"将手牌交给 {facts.Player(seat).Name}", seat);
                Add("cancel", "不发动“炽沄”");
                break;
            }
            case ("chiyun", ShiZhouYuComposition.ChiyunStageCards):
            {
                var picked = r.PickedCards;
                foreach (var card in facts.Hand(f.OwnerSeat))
                    Add(picked.Contains(card.Id) ? "unpick" : "pick",
                        (picked.Contains(card.Id) ? "取消选择 " : "选择 ") + $"【{card.DisplayName}】", null, [card.Id]);
                if (picked.Count > 0)
                    Add("confirm", $"交给 {facts.Player(r.TargetSeat).Name}（已选 {picked.Count} 张）", null, picked.ToArray());
                Add("back", "重新选择目标");
                break;
            }
            case ("yanhui-show", ShiZhouYuComposition.YanhuiShowStagePick):
            {
                foreach (var card in facts.Hand(r.TargetSeat))
                    Add("show", $"观看并展示【{card.DisplayName}】（{card.RankText}）", null, [card.Id]);
                Add("decline", "不展示“焰洄”的手牌");
                break;
            }
            case ("chiyun-option", ShiZhouYuComposition.ChiyunStageOption):
            {
                if (kept != 2)
                    Add("option1", "展示所有与这些牌颜色相同的手牌，然后其受到炽沄的1点火焰伤害", r.TargetSeat);
                if (kept != 1)
                    Add("option2", "令炽沄周瑜摸两张牌，然后你横置", r.TargetSeat);
                break;
            }
            case ("yanhui-ending", ShiZhouYuComposition.YanhuiStageEndingChoice):
            {
                if (kept != 2)
                    foreach (var seat in ShiZhouYuYanhuiVictims(facts.TurnNumber).Where(seat => facts.Player(seat).IsAlive))
                        Add("damage", $"对 {facts.Player(seat).Name} 造成1点火焰伤害", seat);
                var draw = ShiZhouYuYanhuiShownPlayers(facts.TurnNumber);
                if (kept != 1 && draw > 0)
                    Add("draw", $"摸{draw}张牌（本回合展示过牌的角色数）");
                break;
            }
            case ("fentao", ShiZhouYuComposition.FentaoStageChoice):
            {
                if (kept != 2)
                    Add("amplify", "令本次传导的伤害值+1", r.TargetSeat);
                if (kept != 1)
                {
                    var target = facts.Player(r.TargetSeat);
                    Add("discard", $"弃置一半的牌（向上取整 {(facts.Hand(target.Seat).Count + facts.Equipment(target.Seat).Count + 1) / 2} 张）", r.TargetSeat);
                }
                break;
            }
            case ("fentao", ShiZhouYuComposition.FentaoStageDiscardPick):
            {
                var picked = r.PickedCards;
                var target = facts.Player(r.TargetSeat);
                foreach (var card in facts.Hand(target.Seat).Concat(facts.Equipment(target.Seat)))
                    Add(picked.Contains(card.Id) ? "unpick" : "pick",
                        (picked.Contains(card.Id) ? "取消弃置 " : "弃置 ") + $"【{card.DisplayName}】", null, [card.Id]);
                var required = (facts.Hand(target.Seat).Count + facts.Equipment(target.Seat).Count + 1) / 2;
                if (picked.Count == required)
                    Add("confirm", $"确认弃置 {picked.Count} 张", null, picked.ToArray());
                break;
            }
            case ("xiongzi", ShiZhouYuComposition.XiongziStageChoice):
                Add("keep1", "仅保留“炽沄”“焰洄”“焚涛”的全部选项一，且只能于你的回合内发动");
                Add("keep2", "仅保留“炽沄”“焰洄”“焚涛”的全部选项二，且只能于你的回合内发动");
                Add("cancel", "不发动“雄姿”");
                break;
        }
        return Array.AsReadOnly(choices.ToArray());
    }

    private void PublishShiZhouYu(ProgramSkillFrame f)
    {
        var r = f.ShiZhouYu!;
        var choices = ShiZhouYuChoices(f);
        var chooser = r.Operation is "chiyun-option" or "fentao" ? r.TargetSeat : f.OwnerSeat;
        if (choices.Count == 0 || facts.Winner != Winner.None ||
            !facts.Player(f.OwnerSeat).IsAlive || !facts.Player(chooser).IsAlive)
        { FinishShiZhouYuOperation(frames.ActiveProgram(f.Id)); return; }
        var skill = facts.Skill(f.SkillId);
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        prompts.Set(new(DecisionKind.ProgramTrigger, chooser,
            r.Operation switch
            {
                "chiyun" => r.Stage == ShiZhouYuComposition.ChiyunStageTarget
                    ? "炽沄：选择一名其他角色，将任意张手牌交给其。"
                    : "炽沄：选择要交出的手牌。",
                "chiyun-option" => "炽沄：选择一项。",
                "yanhui-show" => "焰洄：观看目标的手牌并展示其中一张。",
                "yanhui-ending" => "焰洄：选择一项。",
                "fentao" => r.Stage == ShiZhouYuComposition.FentaoStageChoice
                    ? "焚涛：选择一项。"
                    : "焚涛：选择要弃置的牌（一半，向上取整）。",
                _ => "雄姿：选择一项。",
            },
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), chooser)
        {
            PromptId = prompts.NextId(), IsPrivate = true, TargetSeat = chooser, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        }, facts.Player(chooser).IsHuman);
        operations.PublishState();
    }

    [ProgramAnswer(ShiZhouYuComposition.ProgramAction)]
    private void AnswerShiZhouYu(PromptChoice selected)
    {
        if (frames.Top is not ProgramSkillFrame { ShiZhouYu: { } r } f ||
            prompts.Current is not { } pending ||
            pending.PlayerSeat != (r.Operation is "chiyun-option" or "fentao" ? r.TargetSeat : f.OwnerSeat) ||
            !ShiZhouYuChoices(frames.ActiveProgram(f.Id)).Any(choice => facts.ChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zhou Yu choice lost its current frame, chooser or legal branch.");
        prompts.Clear();
        var branch = selected.Parameters["branch"];
        switch (r.Operation, branch)
        {
            case ("chiyun", "target") when r.Stage == ShiZhouYuComposition.ChiyunStageTarget:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.ChiyunStageCards,
                    TargetSeat = selected.Targets.Single(), PickedCards = Array.Empty<int>() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun", "cancel") when r.Stage == ShiZhouYuComposition.ChiyunStageTarget:
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun", "pick") when r.Stage == ShiZhouYuComposition.ChiyunStageCards:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { PickedCards = r.PickedCards.Append(selected.Cards.Single()).ToArray() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun", "unpick") when r.Stage == ShiZhouYuComposition.ChiyunStageCards:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { PickedCards = r.PickedCards.Where(id => id != selected.Cards.Single()).ToArray() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun", "back") when r.Stage == ShiZhouYuComposition.ChiyunStageCards:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.ChiyunStageTarget,
                    TargetSeat = -1, PickedCards = Array.Empty<int>() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun", "confirm") when r.Stage == ShiZhouYuComposition.ChiyunStageCards:
            {
                var ids = r.PickedCards.ToArray();
                if (ids.Length == 0 || ids.Distinct().Count() != ids.Length ||
                    ids.Any(id => facts.Location(id) is not { Zone: CardZoneKind.Hand, OwnerSeat: var owner } || owner != f.OwnerSeat))
                    throw new InvalidOperationException("Chiyun can only give current own hand cards.");
                events.AdvanceFact(new ShiZhouYuChiyunGivenEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    r.TargetSeat, ids, ids.Select(id => facts.IsRedCard(f.OwnerSeat,
                        facts.CardsAt(CardLocation.Hand(f.OwnerSeat)).Single(card => card.Id == id))).ToArray()));
                frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect,
                    ShiZhouYu = r with { Stage = ShiZhouYuComposition.ChiyunStageGiveIssued, PickedCards = ids },
                    PendingMovementContinuation = new(r.TargetSeat, 0, null) });
                operations.MoveCards(ids, CardLocation.Hand(r.TargetSeat),
                    new(ShiZhouYuComposition.ChiyunGiveReason));
                return;
            }
            case ("yanhui-show", "show") when r.Stage == ShiZhouYuComposition.YanhuiShowStagePick:
            {
                var cardId = selected.Cards.Single();
                if (facts.Location(cardId) is not { Zone: CardZoneKind.Hand, OwnerSeat: { } holder } || holder != r.TargetSeat)
                    throw new InvalidOperationException("Yanhui can only show a current hand card of its target.");
                var discard = ShiZhouYuCardShownBefore(facts.TurnNumber, cardId);
                events.AdvanceFact(new ShiZhouYuYanhuiShownEvent(facts.TurnNumber, r.TargetSeat, cardId, discard));
                if (!discard)
                { FinishShiZhouYuOperation(frames.ActiveProgram(f.Id)); return; }
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.YanhuiResultDiscardIssued },
                    PendingMovementContinuation = new(r.TargetSeat, 0, null) });
                operations.MoveCards([cardId], CardLocation.DiscardPile,
                    new(ShiZhouYuComposition.YanhuiDiscardReason), CardMovementDisclosure.FaceUp);
                // A settled discard batch with no trigger candidates never returns
                // control on its own; drive the program now (zhang-yan pattern).
                ContinueShiZhouYu(f.Id);
                return;
            }
            case ("yanhui-show", "decline") when r.Stage == ShiZhouYuComposition.YanhuiShowStagePick:
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            case ("chiyun-option", "option1") when r.Stage == ShiZhouYuComposition.ChiyunStageOption:
            {
                var target = r.TargetSeat;
                var givenRed = facts.History<ShiZhouYuChiyunGivenEvent>().Last(entry =>
                    entry.FrameId == f.Id && entry.Source == r.Source).GivenRed;
                var revealed = facts.CardsAt(CardLocation.Hand(target))
                    .Where(card => givenRed.Contains(facts.IsRedCard(target, card)))
                    .Select(card => card.Id).ToArray();
                events.AdvanceFact(new ShiZhouYuChiyunOptionEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    target, 1, revealed));
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.ChiyunStageResolveDamage, Option = 1 } });
                if (revealed.Length > 0) operations.RevealHand(f.Id, target, "chiyun-reveal", revealed);
                operations.Damage(frames.ActiveProgram(f.Id), target, 1, DamageNature.Fire);
                return;
            }
            case ("chiyun-option", "option2") when r.Stage == ShiZhouYuComposition.ChiyunStageOption:
            {
                var target = r.TargetSeat;
                events.AdvanceFact(new ShiZhouYuChiyunOptionEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    target, 2, Array.Empty<int>()));
                operations.DrawCards(f.Id, f.OwnerSeat, 2,
                    new(ShiZhouYuComposition.ChiyunDrawReason));
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.ChiyunStageFinished, Option = 2 } });
                operations.SetChained(frames.ActiveProgram(f.Id), true, target);
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            }
            case ("yanhui-ending", "damage") when r.Stage == ShiZhouYuComposition.YanhuiStageEndingChoice:
            {
                var seat = selected.Targets.Single();
                events.AdvanceFact(new ShiZhouYuYanhuiEndingChosenEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    1, seat, 0));
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.YanhuiStageEndingDamage,
                    TargetSeat = seat, Option = 1 } });
                operations.Damage(frames.ActiveProgram(f.Id), seat, 1, DamageNature.Fire);
                return;
            }
            case ("yanhui-ending", "draw") when r.Stage == ShiZhouYuComposition.YanhuiStageEndingChoice:
            {
                var draw = ShiZhouYuYanhuiShownPlayers(facts.TurnNumber);
                events.AdvanceFact(new ShiZhouYuYanhuiEndingChosenEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    2, null, draw));
                operations.DrawCards(f.Id, f.OwnerSeat, draw,
                    new(ShiZhouYuComposition.YanhuiDrawReason));
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            }
            case ("fentao", "amplify") when r.Stage == ShiZhouYuComposition.FentaoStageChoice:
                events.AdvanceFact(new ShiZhouYuFentaoChosenEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    r.AttackResolutionId, r.TargetSeat, 1, Array.Empty<int>()));
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            case ("fentao", "discard") when r.Stage == ShiZhouYuComposition.FentaoStageChoice:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.FentaoStageDiscardPick,
                    PickedCards = Array.Empty<int>() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("fentao", "pick") when r.Stage == ShiZhouYuComposition.FentaoStageDiscardPick:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { PickedCards = r.PickedCards.Append(selected.Cards.Single()).ToArray() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("fentao", "unpick") when r.Stage == ShiZhouYuComposition.FentaoStageDiscardPick:
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { PickedCards = r.PickedCards.Where(id => id != selected.Cards.Single()).ToArray() } });
                PublishShiZhouYu(frames.ActiveProgram(f.Id));
                return;
            case ("fentao", "confirm") when r.Stage == ShiZhouYuComposition.FentaoStageDiscardPick:
            {
                var target = facts.Player(r.TargetSeat);
                var required = (facts.Hand(target.Seat).Count + facts.Equipment(target.Seat).Count + 1) / 2;
                var ids = r.PickedCards.ToArray();
                if (ids.Length != required || ids.Distinct().Count() != ids.Length ||
                    ids.Any(id => facts.Location(id).OwnerSeat != r.TargetSeat))
                    throw new InvalidOperationException("Fentao must discard exactly half of the current cards.");
                events.AdvanceFact(new ShiZhouYuFentaoChosenEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    r.AttackResolutionId, r.TargetSeat, 2, ids));
                frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect,
                    ShiZhouYu = r with { Stage = ShiZhouYuComposition.FentaoStageDiscardIssued },
                    PendingMovementContinuation = new(r.TargetSeat, 0, null) });
                operations.LogicalDiscard(frames.ActiveProgram(f.Id), r.TargetSeat, ids,
                    new(ShiZhouYuComposition.FentaoDiscardReason));
                // Same settled-batch rule: drive the program after the discard lands.
                ContinueShiZhouYu(f.Id);
                return;
            }
            case ("xiongzi", "keep1") or ("xiongzi", "keep2") when r.Stage == ShiZhouYuComposition.XiongziStageChoice:
            {
                var kept = branch == "keep1" ? 1 : 2;
                events.AdvanceFact(new ShiZhouYuXiongziActivatedEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber,
                    f.OwnerSeat, kept));
                operations.DrawCards(f.Id, f.OwnerSeat, 2,
                    new(ShiZhouYuComposition.XiongziDrawReason));
                frames.ReplaceTop(f = f with { ShiZhouYu = r with { Stage = ShiZhouYuComposition.XiongziStageFinished, Option = kept } });
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            }
            case ("xiongzi", "cancel"):
                FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
                return;
            default:
                throw new InvalidOperationException("Unknown Shi Zhou Yu choice branch.");
        }
    }

    // ---- instruction bodies without published choices ------------------------------

    private void RunShiZhouYuFentaoRechain(ProgramSkillFrame f)
    {
        var r = f.ShiZhouYu!;
        var target = r.TargetSeat;
        events.AdvanceFact(new ShiZhouYuFentaoRechainedEvent(f.Id, r.Source!, f.GameplayHash, facts.TurnNumber, target));
        if (facts.Player(target).IsAlive)
        {
            operations.SetChained(frames.ActiveProgram(f.Id), true, target);
            operations.Log("ChainDamage", $"焚涛令 {facts.Player(target).Name} 于伤害结算后再次横置。", f.OwnerSeat, target);
        }
        FinishShiZhouYuOperation(frames.ActiveProgram(f.Id));
    }

    // ---- damage amplification --------------------------------------------------------

    // 焚涛 option 1: every chain-propagated segment of the amplified fire damage carries one extra damage.
    internal int FentaoAmplification(long resolutionId, bool isChainPropagation) =>
        HasShiZhouYuCapability && isChainPropagation &&
        facts.History<ShiZhouYuFentaoChosenEvent>().Any(entry => entry.Option == 1 &&
            entry.AttackResolutionId == resolutionId) ? 1 : 0;

    // ---- runtime hooks -----------------------------------------------------------------

    [ProgramContinuation(ProgramContinuationKind.Resume, 67800)]
    private bool ResumeShiZhouYu(long id)
    {
        if (frames.Top is not ProgramSkillFrame { ShiZhouYu: not null } f || f.Id != id) return false;
        ContinueShiZhouYu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 67800)]
    private bool ReturnShiZhouYuMovement(ProgramSkillFrame f) => f.ShiZhouYu is not null && ResumeShiZhouYu(f.Id);

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 67800)]
    private bool ShiZhouYuMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) =>
        f.ShiZhouYu is { Stage: ShiZhouYuComposition.ChiyunStageGiveIssued or
            ShiZhouYuComposition.FentaoStageDiscardIssued or ShiZhouYuComposition.YanhuiResultDiscardIssued } r &&
        p.CoverageResultBind is null && p.BeforeCount == 0 && p.SubjectSeat == r.TargetSeat;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 67800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 67800)]
    private bool ContinuePaidShiZhouYu(ProgramSkillFrame f) => f.ShiZhouYu is not null;

    // ---- AI choices -----------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZhouYuChiyun, SkillProgramEffectOp.ResolveShiZhouYuChiyunOption,
        SkillProgramEffectOp.ResolveShiZhouYuYanhuiShow, SkillProgramEffectOp.ResolveShiZhouYuYanhuiEnding,
        SkillProgramEffectOp.ResolveShiZhouYuFentao, SkillProgramEffectOp.ResolveShiZhouYuXiongzi)]
    private PromptChoice ChooseShiZhouYuAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShiZhouYu!;
        if (r.Operation == "chiyun" && r.Stage == ShiZhouYuComposition.ChiyunStageTarget)
            return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "target" &&
                !facts.Allies(f.OwnerSeat, choice.Targets.Single())) ??
                decision.Choices.First();
        if (r.Operation == "chiyun" && r.Stage == ShiZhouYuComposition.ChiyunStageCards)
        {
            var confirm = decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "confirm");
            var pick = decision.Choices.Where(choice => choice.Parameters["branch"] == "pick")
                .OrderBy(choice => facts.KeepValue(f.OwnerSeat, facts.Hand(f.OwnerSeat).Single(card => card.Id == choice.Cards[0]))).ThenBy(choice => choice.Cards[0]).FirstOrDefault();
            return pick ?? confirm ?? decision.Choices.First(choice => choice.Parameters["branch"] == "back");
        }
        if (r.Operation == "chiyun-option")
            return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "option2") ??
                decision.Choices.First();
        if (r.Operation == "yanhui-show")
            return decision.Choices.Where(choice => choice.Parameters["branch"] == "show")
                .OrderBy(choice => facts.KeepValue(r.TargetSeat, facts.Hand(r.TargetSeat).Single(card => card.Id == choice.Cards[0]))).ThenBy(choice => choice.Cards[0]).FirstOrDefault() ??
                decision.Choices.First();
        if (r.Operation == "yanhui-ending")
            return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "damage" &&
                !facts.Allies(f.OwnerSeat, choice.Targets.Single())) ??
                decision.Choices.First();
        if (r.Operation == "fentao" && r.Stage == ShiZhouYuComposition.FentaoStageChoice)
            return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "amplify") ??
                decision.Choices.First();
        if (r.Operation == "fentao")
        {
            var confirm = decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "confirm");
            var pick = decision.Choices.Where(choice => choice.Parameters["branch"] == "pick")
                .OrderBy(choice => choice.Cards[0]).FirstOrDefault();
            return confirm ?? pick ?? decision.Choices.First();
        }
        return decision.Choices.FirstOrDefault(choice => choice.Parameters["branch"] == "keep1") ??
            decision.Choices.First();
    }
}
