using System.Globalization;
namespace CardGame.Core;
[ProgramRuntimeModule,ProgramCapability]
internal sealed class MobileSimaYiRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramOperationDraws draws, IMobileSimaYiFacts facts,
    IMobileSimaYiOperations operations) : IMobileSimaYiHost
{
    private const string MobileSimaJilue = "mobile-god:jilue",MobileSimaLianpo = "mobile-god:lianpo";
    private static IReadOnlyList<string> MobileSimaJilueSkills => MobileSimaYiComposition.LearningSkills;
    private static CardConversionSource MobileSimaSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private int MobileSimaAcquisitionOrdinal(int owner) => facts.History<SkillsAcquiredEvent>().Count(e => e.PlayerSeat == owner && e.SkillIds.Contains(MobileSimaJilue));
    private bool MobileSimaNeedsInitialSkill(int owner, string instance) => !facts.History<MobileSimaJilueInitializedEvent>().Any(e => e.OwnerSeat == owner &&
        e.SkillInstanceId == instance && e.AcquisitionOrdinal == MobileSimaAcquisitionOrdinal(owner));
    private string[] MobileSimaMissingSkills(int owner) => MobileSimaJilueSkills.Where(id => !facts.Grants(owner).Any(g => g.SkillId == id)).ToArray();
    private int MobileSimaLearningCost(int owner) => Math.Max(2, facts.History<MobileSimaJilueChoiceEvent>().Count(e => e.Source.OwnerSeat == owner &&
        e.Source.SkillId == MobileSimaJilue && e.Choice.StartsWith("learn:", StringComparison.Ordinal)) + 1);
    private bool MobileSimaCanExtraTurn(int owner) => !facts.History<MobileSimaJilueChoiceEvent>().Any(e => e.Source.OwnerSeat == owner && e.Source.SkillId == MobileSimaLianpo &&
        e.Choice == "extra-turn" && e.ActualTurn == facts.TurnNumber);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40425)]
    private bool AllowsMobileSimaTrigger(ProgramTriggerLegalityContext g)
    {
        if (g.Features.HasOperation(SkillProgramEffectOp.InitializeMobileShenSimaYiJilue))
            return MobileSimaNeedsInitialSkill(g.OwnerSeat, g.Candidate.SkillInstanceId) &&
                (g.Trigger.Window != SkillProgramTriggerWindow.SkillsChanged || MobileSimaAcquisitionOrdinal(g.OwnerSeat) > 0);
        return !g.Features.HasOperation(SkillProgramEffectOp.ResolveMobileShenSimaYiLianpo) || MobileSimaCanExtraTurn(g.OwnerSeat) ||
            facts.HasSkill(g.OwnerSeat, MobileSimaJilue) && MobileSimaMissingSkills(g.OwnerSeat).Length > 0;
    }
    public SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginMobileSimaYi(frame, operation);
    private SkillProgramStepOutcome BeginMobileSimaYi(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = frames.ActiveProgram(supplied.Id);
        if (f.MobileSimaYi is not null || f.InstructionIndex != 1 || f.TriggerId is null)
            throw new InvalidOperationException("Mobile Sima Yi requires his original accepted trigger instruction.");
        var r = new MobileSimaYiReceipt(MobileSimaSource(f), f.GameplayHash, f.InstructionIndex, op, MobileSimaLearningCost(f.OwnerSeat))
            { Skills = MobileSimaMissingSkills(f.OwnerSeat) };
        frames.ReplaceTop(f = f with { MobileSimaYi = r });
        if (op == SkillProgramEffectOp.InitializeMobileShenSimaYiJilue)
        {
            var faction = facts.Faction(f.OwnerSeat);
            var bonus = faction switch { "wei" => "classic:fangzhu", "shu" => "classic:jizhi", "wu" => "classic:zhiheng", "qun" => "classic:wansha", _ => null };
            events.AdvanceFact(new MobileSimaJilueInitializedEvent(f.Id, f.OwnerSeat, f.SkillInstanceId, MobileSimaAcquisitionOrdinal(f.OwnerSeat), faction));
            frames.ReplaceTop(f = f with { MobileSimaYi = r with { Stage = MobileSimaYiStage.Children, Choice = "initialize" } });
            operations.GrantSkills(f, bonus is null ? ["classic:guicai"] : ["classic:guicai", bonus]);
            ContinueMobileSimaYi(f.Id); return SkillProgramStepOutcome.AwaitChild;
        }
        PublishMobileSimaYi(f); return SkillProgramStepOutcome.AwaitChoice;
    }
    private IReadOnlyList<PromptChoice> MobileSimaYiChoices(ProgramSkillFrame f)
    {
        var r = f.MobileSimaYi!; var owner = facts.Player(f.OwnerSeat);
        PromptChoice Choice(string key, string text) => new(new("mobile-sima." + f.Id + "." + key), text, [], [],
            new Dictionary<string, string> { ["program-action"] = "mobile-sima", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = key });
        var result = new List<PromptChoice>();
        if (r.Stage != MobileSimaYiStage.Choice) return result;
        var ren = facts.MarkerCount(f.OwnerSeat, PlayerMarkerKind.Ren);
        var learning = r.Operation == SkillProgramEffectOp.ResolveMobileShenSimaYiJilue;
        if (learning ? ren >= r.Cost : facts.HasSkill(f.OwnerSeat, MobileSimaJilue))
            foreach (var skill in r.Skills.Where(id => MobileSimaMissingSkills(f.OwnerSeat).Contains(id)))
                result.Add(Choice("learn:" + skill, (learning ? "移去" + r.Cost + "枚忍，" : "") + "获得【" + facts.Skill(skill).Name + "】"));
        if (learning)
            for (var i = Math.Min(2, ren); i >= 0; i--) result.Add(Choice("draw:" + i, "移去" + i + "枚忍，摸" + i + "张牌"));
        else if (MobileSimaCanExtraTurn(f.OwnerSeat)) result.Add(Choice("extra-turn", "此回合结束后执行一个额外回合"));
        return result;
    }
    private void PublishMobileSimaYi(ProgramSkillFrame f)
    {
        var skill = facts.Skill(f.SkillId); var choices = MobileSimaYiChoices(f);
        if (choices.Count == 0 || !facts.Player(f.OwnerSeat).IsAlive || facts.Winner != Winner.None) { FinishMobileSimaYi(f); return; }
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        prompts.Set(new(DecisionKind.ProgramTrigger, f.OwnerSeat, skill.Name + "：请选择。", [], [], f.OwnerSeat)
            { PromptId = prompts.NextId(), Choices = choices, IsPrivate = true, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) }, facts.Player(f.OwnerSeat).IsHuman); operations.PublishState();
    }
    [ProgramAnswer("mobile-sima")] private void AnswerMobileSimaYi(PromptChoice choice)
    {
        if (frames.Top is not ProgramSkillFrame { MobileSimaYi: { Stage: MobileSimaYiStage.Choice } r } f ||
            prompts.Current?.PlayerSeat != f.OwnerSeat || !MobileSimaYiChoices(f).Any(c => RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Mobile Sima Yi requires his exact current choice and unpaid original instruction.");
prompts.Clear(); var key = choice.Parameters["option"];
        var cost = key.StartsWith("draw:", StringComparison.Ordinal) ? int.Parse(key[5..], CultureInfo.InvariantCulture) :
            key.StartsWith("learn:", StringComparison.Ordinal) && r.Operation == SkillProgramEffectOp.ResolveMobileShenSimaYiJilue ? r.Cost : 0;
        if (cost > 0) operations.PayMarker(f.OwnerSeat, new(PlayerMarkerKind.Ren, cost), f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f));
        var draw = key.StartsWith("draw:", StringComparison.Ordinal) ? cost : 0;
        frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, MobileSimaYi = r with { Choice = key, DrawCount = draw,
            Stage = draw > 0 ? MobileSimaYiStage.Drawing : MobileSimaYiStage.Children } });
        events.AdvanceFact(new MobileSimaJilueChoiceEvent(f.Id, r.Source, key, cost, facts.TurnNumber));
        if (key.StartsWith("learn:", StringComparison.Ordinal)) operations.GrantSkills(f, [key[6..]]);
        else if (key == "extra-turn") operations.ExtraTurn(f, f.OwnerSeat);
        else if (draw > 0) { draws.BeginOperationProgramDraw(f, f.OwnerSeat, draw, new("skill.mobile-god.jilue.draw")); return; }
        ContinueMobileSimaYi(f.Id);
    }
    private void ContinueMobileSimaYi(long id)
    {
        if (prompts.Current is not null || frames.Top is not ProgramSkillFrame { MobileSimaYi: { } r } f || f.Id != id || r.Stage == MobileSimaYiStage.Drawing) return;
        if (operations.DrainChildWindows(id)) return;
        f = frames.ActiveProgram(id); if (r.Stage == MobileSimaYiStage.Choice) PublishMobileSimaYi(f); else FinishMobileSimaYi(f);
    }
    private void FinishMobileSimaYi(ProgramSkillFrame f) { frames.ReplaceTop(f with { MobileSimaYi = null }); frames.AdvanceProgram(f.Id); }
    [ProgramContinuation(ProgramContinuationKind.Resume, 40425)] private bool ResumeMobileSimaYi(long id)
    { if (frames.Top is not ProgramSkillFrame { MobileSimaYi: not null } f || f.Id != id) return false; ContinueMobileSimaYi(id); return true; }
    [ProgramContinuation(ProgramContinuationKind.Movement, 40425)] private bool ReturnMobileSimaYiMovement(ProgramSkillFrame f) => f.MobileSimaYi is not null && ResumeMobileSimaYi(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShenSimaYiJilue)] private void ReturnMobileSimaYiDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MobileSimaYi is not { Stage: MobileSimaYiStage.Drawing } r || r.DrawCount != o.RequestedCount || o.RecipientSeat != f.OwnerSeat ||
            facts.History<MobileSimaJilueDrawReturnedEvent>().Any(e => e.FrameId == f.Id)) throw new InvalidOperationException("Jilue requires its exact once-paid native draw return.");
        frames.ReplaceTop(f with { MobileSimaYi = r with { Stage = MobileSimaYiStage.Children, DrawOutcome = o } });
        events.AdvanceFact(new MobileSimaJilueDrawReturnedEvent(f.Id, f.OwnerSeat, o.RequestedCount, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter));
        ResumeMobileSimaYi(f.Id);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40425)] [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40425)]
    private bool RetainMobileSimaYi(ProgramSkillFrame f) => f.MobileSimaYi is not null;
    [ProgramAiChoice(SkillProgramEffectOp.InitializeMobileShenSimaYiJilue)] private PromptChoice ChooseMobileSimaInitial(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenSimaYiJilue)] private PromptChoice ChooseMobileSimaJilue(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenSimaYiLianpo)] private PromptChoice ChooseMobileSimaLianpo(PendingDecision p, ProgramSkillFrame f) => p.Choices.Last();
}
