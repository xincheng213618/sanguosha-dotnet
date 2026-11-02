using System.Globalization;
using StageNode = CardGame.Core.ProgramStageNode<CardGame.Core.MouCaoHongRuntime, CardGame.Core.MouCaoHongReceipt, CardGame.Core.MouCaoHongStage>;
using StagePlan = CardGame.Core.ProgramStagePlan<CardGame.Core.MouCaoHongRuntime, CardGame.Core.MouCaoHongReceipt, CardGame.Core.MouCaoHongStage>;
namespace CardGame.Core;
[ProgramRuntimeModule,ProgramCapability]
internal sealed class MouCaoHongRuntime(IMouCaoHongFacts facts, IMouCaoHongNative native, IProgramRuntimeFrames frames, IProgramRuntimeEvents events, IProgramRuntimePrompts prompts) : IMouCaoHongRuntime
{
    internal const string Action = "mou-cao-hong-anniversary",TakeReason = "skill-program.mou-cao-hong.obtain",GiveReason = "skill-program.mou:yingjia.return",DiscardReason = "skill-program.mou:xianju.ending-discard";
    private IMouCaoHongNative Native => native;
    private IMouCaoHongFacts Facts => facts;
    private IProgramRuntimeEvents Events => events;
    internal static CardConversionSource Source(ProgramSkillFrame f) => new(f.SkillId, f.TriggerId ?? f.ActivationId, f.OwnerSeat, f.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 35900, SkillProgramEffectOp.ResolveMouCaoHongYingjia, SkillProgramEffectOp.SettleMouCaoHongXianju)]
    private bool Qualifies(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => t.Effects.Single().Op == SkillProgramEffectOp.ResolveMouCaoHongYingjia ? facts.Use(c, w) is { Targets.Count: > 0 } : facts.Debt(c, w) is not null;
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = frames.ActiveProgram(supplied.Id);
        if (f.MouCaoHong is not null) { Continue(f.Id); return SkillProgramStepOutcome.AwaitChild; }
        if (f.InstructionIndex != 1 || facts.Program(f.SkillId)?.GameplayHash != f.GameplayHash || !facts.Parent(f)) throw new InvalidOperationException("Mou Cao Hong requires his exact native Use, actual Play activation or previously issued Play-ending cost.");
        var candidate = f.TriggerId is null ? null : new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, f.WindowContext!.OccurrenceIndex);
        var use = op == SkillProgramEffectOp.ResolveMouCaoHongYingjia ? facts.Use(candidate!, f.WindowContext!) : null;
        var debt = op == SkillProgramEffectOp.SettleMouCaoHongXianju ? facts.Debt(candidate!, f.WindowContext!) : null;
        if (op == SkillProgramEffectOp.ResolveMouCaoHongYingjia && use is not { Targets.Count: > 0 } || op == SkillProgramEffectOp.SettleMouCaoHongXianju && debt is null ||
            op == SkillProgramEffectOp.ResolveMouCaoHongXianju && !facts.ActualPlay(f.OwnerSeat)) throw new InvalidOperationException("Mou Cao Hong lost his actual target declaration or paid phase identity.");
        var stage = op == SkillProgramEffectOp.ResolveMouCaoHongYingjia ? MouCaoHongStage.ExchangeTarget : op == SkillProgramEffectOp.ResolveMouCaoHongXianju ? MouCaoHongStage.ObtainCard : MouCaoHongStage.DiscardCards;
        var r = new MouCaoHongReceipt(Source(f), f.GameplayHash, op, facts.Turn, facts.TurnOwner, facts.PhaseId, f.WindowContext?.ParentFrameId ?? 0, use?.ActionId ?? 0, use?.UseId ?? 0, debt?.FrameId ?? 0)
        { Stage = stage, Targets = op == SkillProgramEffectOp.ResolveMouCaoHongXianju ? facts.InRange(f.OwnerSeat) : use?.Targets ?? [], Requested = op == SkillProgramEffectOp.SettleMouCaoHongXianju ? facts.OutsideCount(f.OwnerSeat) : 0, Before = facts.Sequence, After = facts.Sequence };
        frames.ReplaceTop(f = f with { MouCaoHong = r }); events.AdvanceFact(new MouCaoHongStartedEvent(f.Id, r.Source, r.GameplayHash, op, r.Turn, r.TurnOwner, r.PhaseId, r.ParentId, r.ActionId, r.UseId, r.DebtId));
        if (op == SkillProgramEffectOp.ResolveMouCaoHongYingjia)
        {
            foreach (var target in r.Targets) events.AdvanceFact(new MouCaoHongDistanceIssuedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.TurnOwner, r.ActionId, target));
            if (!facts.AllDistanceOne(f.OwnerSeat)) { Complete(f); return SkillProgramStepOutcome.AwaitChild; }
        }
        else if (op == SkillProgramEffectOp.ResolveMouCaoHongXianju) events.AdvanceFact(new MouCaoHongXianjuIssuedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.TurnOwner, r.PhaseId));
        else events.AdvanceFact(new MouCaoHongDebtConsumedEvent(r.DebtId, f.Id, r.Requested));
        Continue(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }
    internal bool Valid(ProgramSkillFrame f) => f.MouCaoHong is { } r && f.InstructionIndex == 1 && r.Source == Source(f) && r.GameplayHash == f.GameplayHash && facts.Program(f.SkillId)?.GameplayHash == f.GameplayHash &&
        facts.Parent(f) && r.Turn == facts.Turn && r.TurnOwner == facts.TurnOwner && r.PhaseId == facts.PhaseId && Enum.IsDefined(r.Stage) && r.Cursor >= 0 && r.Cursor <= r.Targets.Count && r.Requested >= 0 && r.Actual >= 0 &&
        r.Before >= 0 && r.After >= r.Before && r.After <= facts.Sequence && r.Selected.Distinct().Count() == r.Selected.Count && r.Cards.Select(c => c.Id).Distinct().Count() == r.Cards.Count && r.Targets.Distinct().Count() == r.Targets.Count &&
        facts.History<MouCaoHongStartedEvent>().Count(e => e == new MouCaoHongStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.Turn, r.TurnOwner, r.PhaseId, r.ParentId, r.ActionId, r.UseId, r.DebtId)) == 1;
    private IReadOnlyList<MouCaoHongCard> Payable(ProgramSkillFrame f) => facts.Cards(f.OwnerSeat, f.OwnerSeat, f.MouCaoHong!.Stage == MouCaoHongStage.DiscardCards);
    private int Need(ProgramSkillFrame f) => Math.Min(f.MouCaoHong!.Requested, Payable(f).Count);
    private PromptChoice Choice(ProgramSkillFrame f, string branch, string text, int? card = null, int? target = null) => new(new ChoiceId($"mou-cao-hong-anniversary.{f.Id}.{f.MouCaoHong!.Stage}.{f.MouCaoHong.Cursor}.{branch}.{card}.{target}"), text,
        card is { } c ? [c] : [], target is { } s ? [s] : [], new Dictionary<string, string> { ["program-action"] = Action, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch });
    private IReadOnlyList<PromptChoice> Choices(ProgramSkillFrame f)
    {
        var r = f.MouCaoHong!;
        if (r.Stage == MouCaoHongStage.ExchangeTarget) return facts.ExchangeTargets(f.OwnerSeat).Select(s => Choice(f, "target", facts.Name(s), target: s)).Append(Choice(f, "decline", "不交换手牌")).ToArray();
        if (r.Stage == MouCaoHongStage.ObtainCard)
        {
            var cards = facts.Cards(f.OwnerSeat, r.Target, false); var result = new List<PromptChoice>();
            if (cards.Any(c => c.From.Zone == CardZoneKind.Hand)) result.Add(Choice(f, "hand", "获得其一张随机手牌"));
            result.AddRange(cards.Where(c => c.From.Zone == CardZoneKind.Equipment).Select(c => Choice(f, "card", "获得" + c.Label, c.Id))); return result;
        }
        if (r.Stage is MouCaoHongStage.ReturnCards or MouCaoHongStage.DiscardCards)
        {
            var n = Need(f); var result = Payable(f).Where(c => r.Selected.Contains(c.Id) || r.Selected.Count < n).Select(c => Choice(f, r.Selected.Contains(c.Id) ? "remove" : "add", (r.Selected.Contains(c.Id) ? "取消" : "选择") + c.Label, c.Id)).ToList();
            if (r.Selected.Count == n) result.Add(Choice(f, "done", "确认" + n + "张牌")); return result;
        }
        return [];
    }
    private void Publish(ProgramSkillFrame f)
    {
        var choices = Choices(f); var r = f.MouCaoHong!; frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        var text = r.Stage switch { MouCaoHongStage.ExchangeTarget => "迎驾：可以获得一名角色全部手牌，再交给其等量的牌。", MouCaoHongStage.ObtainCard => "险拒：选择获得其一张手牌或装备牌。", MouCaoHongStage.ReturnCards => "迎驾：从当前手牌和装备中选择" + Need(f) + "张交回。", _ => "险拒：弃置" + Need(f) + "张牌。" };
        prompts.Set(new PendingDecision(DecisionKind.ProgramTrigger, f.OwnerSeat, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), r.Target >= 0 ? r.Target : null)
        { PromptId = prompts.NextId(), IsPrivate = true, Choices = Array.AsReadOnly(choices.ToArray()) }, facts.Human(f.OwnerSeat)); native.Publish();
    }
    [ProgramAnswer(Action)]
    private void Answer(PromptChoice chosen)
    {
        if (frames.Top is not ProgramSkillFrame { MouCaoHong: { } r } f || !Valid(f) || prompts.Current?.PlayerSeat != f.OwnerSeat ||
            Choices(f).SingleOrDefault(c => c.Id == chosen.Id) is not { } canonical || !canonical.Cards.SequenceEqual(chosen.Cards) || !canonical.Targets.SequenceEqual(chosen.Targets) || canonical.Parameters.Count != chosen.Parameters.Count || canonical.Parameters.Any(p => chosen.Parameters.GetValueOrDefault(p.Key) != p.Value)) throw new InvalidOperationException("Mou Cao Hong requires the current private native material or exact target choice.");
        prompts.Clear(); frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect }); var branch = chosen.Parameters["branch"];
        if (branch == "decline") { Complete(f); return; }
        if (r.Stage == MouCaoHongStage.ExchangeTarget)
        {
            var target = chosen.Targets.Single(); var cards = facts.Hand(target); events.AdvanceFact(new MouCaoHongExchangeIssuedEvent(f.Id, f.OwnerSeat, r.Turn, target));
            StartMove(f, r with { Target = target, Stage = MouCaoHongStage.ExchangeTakeChildren, Cards = cards, Requested = cards.Count }); return;
        }
        if (r.Stage == MouCaoHongStage.ObtainCard)
        {
            var cards = facts.Cards(f.OwnerSeat, r.Target, false); var card = branch == "hand" ? native.RandomHand(f, cards.Where(c => c.From.Zone == CardZoneKind.Hand).ToArray()) : cards.Single(c => c.Id == chosen.Cards.Single());
            StartMove(f, r with { Stage = MouCaoHongStage.ObtainChildren, Cards = [card], Requested = 1 }); return;
        }
        if (branch != "done")
        {
            var id = chosen.Cards.Single(); frames.ReplaceTop(f with { MouCaoHong = r with { Selected = branch == "add" ? r.Selected.Append(id).ToArray() : r.Selected.Where(c => c != id).ToArray() } }); Continue(f.Id); return;
        }
        var paying = Payable(f).Where(c => r.Selected.Contains(c.Id)).ToArray();
        StartMove(f, r with { Stage = r.Stage == MouCaoHongStage.ReturnCards ? MouCaoHongStage.ReturnChildren : MouCaoHongStage.DiscardChildren, Cards = paying });
    }
    private void StartMove(ProgramSkillFrame f, MouCaoHongReceipt r)
    {
        f = f with { MouCaoHong = r with { Before = facts.Sequence, After = facts.Sequence, Actual = 0 }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) };
        ProgramStageRuntime.Issue(frames, f, paid => { if (r.Cards.Count > 0) native.Move(paid); });
        Continue(f.Id);
    }
    internal void Continue(long id) => ProgramStageRuntime.Run(id, frames, prompts, this, Stages.Plan);
    private ProgramStageResult ChooseObtainCard(ProgramSkillFrame f, MouCaoHongReceipt r)
    {
        if (r.Cursor >= r.Targets.Count) { Complete(f); return ProgramStageResult.Yield; }
        var target = r.Targets[r.Cursor]; frames.ReplaceTop(f = f with { MouCaoHong = r = r with { Target = target } });
        if (!facts.Alive(target) || facts.Cards(f.OwnerSeat, target, false).Count == 0)
        {
            frames.ReplaceTop(f with { MouCaoHong = r with { Cursor = r.Cursor + 1 } });
            return ProgramStageResult.Continue;
        }
        Publish(f); return ProgramStageResult.Yield;
    }
    private ProgramStageResult ChoosePayment(ProgramSkillFrame f, MouCaoHongReceipt r)
    {
        if (r.Stage == MouCaoHongStage.ReturnCards && !facts.Alive(r.Target)) { Complete(f); return ProgramStageResult.Yield; }
        var allowed = Payable(f).Select(c => c.Id).ToHashSet(); var selected = r.Selected.Where(allowed.Contains).Take(Need(f)).ToArray();
        if (!selected.SequenceEqual(r.Selected)) frames.ReplaceTop(f = f with { MouCaoHong = r = r with { Selected = selected } });
        if (Need(f) == 0)
            StartMove(f, r with { Stage = r.Stage == MouCaoHongStage.ReturnCards ? MouCaoHongStage.ReturnChildren : MouCaoHongStage.DiscardChildren, Cards = [] });
        else Publish(f);
        return ProgramStageResult.Yield;
    }
    private static class Stages
    {
        internal static readonly StagePlan Plan = new(
            read: static f => f.MouCaoHong,
            stage: static r => r.Stage,
            nodes:
            [
                StageNode.Prompt(MouCaoHongStage.ExchangeTarget, static (runtime, f, _) => runtime.Publish(f)),
                StageNode.Run(MouCaoHongStage.ObtainCard, static (runtime, f, r) => runtime.ChooseObtainCard(f, r)),
                StageNode.Run(MouCaoHongStage.ReturnCards, static (runtime, f, r) => runtime.ChoosePayment(f, r)),
                StageNode.Run(MouCaoHongStage.DiscardCards, static (runtime, f, r) => runtime.ChoosePayment(f, r)),
                StageNode.ChildrenTo(MouCaoHongStage.ExchangeTakeChildren, MouCaoHongStage.ReturnCards,
                    update: static r => r with { Requested = r.Actual, Selected = [], Cards = [] }, clearMovement: true),
                StageNode.ChildrenTo(MouCaoHongStage.ObtainChildren, MouCaoHongStage.ObtainCard,
                    update: static r => r with { Cursor = r.Cursor + 1, Cards = [], Selected = [] }, clearMovement: true),
                StageNode.ChildrenComplete(MouCaoHongStage.ReturnChildren, clearMovement: true),
                StageNode.ChildrenComplete(MouCaoHongStage.DiscardChildren, clearMovement: true)
            ],
            stopBeforeStage: static (runtime, f, r) =>
            {
                if (runtime.Facts.Finished || !runtime.Facts.Alive(f.OwnerSeat) && r.Stage is MouCaoHongStage.ExchangeTarget or MouCaoHongStage.ObtainCard or MouCaoHongStage.ReturnCards or MouCaoHongStage.DiscardCards)
                { runtime.Complete(f); return true; }
                return false;
            },
            withStage: static (f, r, next) => f with { MouCaoHong = r with { Stage = next } },
            drain: static (runtime, f, _) => runtime.Native.Drain(f),
            complete: static (runtime, f) => runtime.Complete(f),
            afterChildren: static (runtime, f, r) => runtime.Events.AdvanceFact(
                new MouCaoHongSegmentCompletedEvent(f.Id, r.Stage, r.Target, r.Requested, r.Actual)));
    }
    private void Complete(ProgramSkillFrame f) { events.AdvanceFact(new MouCaoHongCompletedEvent(f.Id)); frames.ReplaceTop(f = f with { MouCaoHong = null, PendingMovementContinuation = null }); native.Finish(f); }
    [ProgramContinuation(ProgramContinuationKind.Resume, 35900)] private bool Resume(long id) { if (frames.Top is not ProgramSkillFrame { MouCaoHong: not null } f || f.Id != id) return false; Continue(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 35900)] private bool Movement(ProgramSkillFrame f) => f.MouCaoHong is not null && Resume(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 35900)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 35900)] private bool Paid(ProgramSkillFrame f) => f.MouCaoHong is not null && Valid(f);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouCaoHongYingjia, SkillProgramEffectOp.ResolveMouCaoHongXianju, SkillProgramEffectOp.SettleMouCaoHongXianju)]
    private PromptChoice Ai(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouCaoHong!.Stage is MouCaoHongStage.ReturnCards or MouCaoHongStage.DiscardCards)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "done") ?? p.Choices.Where(c => c.Parameters["branch"] == "add").OrderBy(c => facts.Keep(f.OwnerSeat, c.Cards.Single())).First();
        return p.Choices.First();
    }
}
