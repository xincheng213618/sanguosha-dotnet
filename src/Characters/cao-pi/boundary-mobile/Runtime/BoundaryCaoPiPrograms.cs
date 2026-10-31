using System.Text.Json.Serialization;
namespace CardGame.Core;

internal static class BoundaryCaoPiComposition
{
    internal const string Xingshang = "boundary:xingshang-mobile",Fangzhu = "boundary:fangzhu-mobile",Songwei = "boundary:songwei-mobile";
    [ProgramCatalogValidation(54500)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (p.Id == Xingshang && (p.Activations.Count != 0 || p.Triggers is not [{ Id: "death-claim-or-recover", Window: SkillProgramTriggerWindow.CharacterDied, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
            Effects: [{ Op: SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang, ResultBind: "xingshang-result" }, { Op: SkillProgramEffectOp.ClaimDeathCleanupCards }, { Op: SkillProgramEffectOp.Recover, Target: SkillProgramEffectTarget.Owner, Amount: 1 }] } x] ||
            !x.Effects[1].Zones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || !Branch(x.Effects[1], "xingshang-result", "claim") || !Branch(x.Effects[2], "xingshang-result", "recover")))
            throw new InvalidOperationException(path + ": mobile Xingshang requires one exact native death choice and HE claim or owner recovery.");
        if (p.Id == Fangzhu && (p.Activations.Count != 0 || p.Triggers is not [{ Id: "injury-recipient-choice", Window: SkillProgramTriggerWindow.AfterDamageApplied, Subject: SkillProgramTriggerSubject.Owner, DamageOccurrence: SkillProgramDamageOccurrence.PerDamage, Optional: true,
            Effects: [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving }, { Op: SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu, ResultBind: "fangzhu-result" },
                { Op: SkillProgramEffectOp.TurnOver, Target: SkillProgramEffectTarget.SelectedTarget }, { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] } f] ||
            !Branch(f.Effects[2], "fangzhu-result", "drawn") || !Branch(f.Effects[3], "fangzhu-result", "discarded")))
            throw new InvalidOperationException(path + ": mobile Fangzhu requires the injured owner's selected role, its frozen-X draw then flip or paid discard then HP loss.");
        if (p.Id == Songwei && (p.Triggers is not [{ Id: "other-wei-black-judgment", Window: SkillProgramTriggerWindow.JudgmentFinalized, Subject: SkillProgramTriggerSubject.Any, Optional: false, IsSkillInvocation: false,
            Effects: [{ Op: SkillProgramEffectOp.ChooseOption, ChooserRef.Kind: ProgramParticipantRef.EventTarget, ResultBind: "songwei-answer" }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1 }] } s] ||
            !s.Suits.SequenceEqual([Suit.Spade, Suit.Club]) || !s.Effects[0].Options.Select(o => o.Id).SequenceEqual(["draw", "decline"]) || !Branch(s.Effects[1], "songwei-answer", "draw")))
            throw new InvalidOperationException(path + ": Songwei requires the actual foreign Wei final-black-judgment subject's own optional answer and native lord draw.");
        if (p.Id is not (Xingshang or Fangzhu) && p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op is SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu or SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang))
            throw new InvalidOperationException(path + ": mobile Cao Pi instructions belong only to their exact printed rules.");
    }
    private static bool Branch(SkillProgramEffect e, string bind, string option) => e.Condition.Kind == SkillProgramConditionKind.ChoiceIs && e.Condition.SourceBind == bind && e.Condition.OptionId == option;
}
internal interface IBoundaryCaoPiHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffect e); }
internal abstract class BoundaryCaoPiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "resultBind", "condition"); var e = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), resultBind: r.RequiredIdentifier("resultBind")); RequireAlways(e, r.Path); return e;
    }
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler => new BoundaryCaoPiHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, c) => c.PublicControlValue(3));
}
internal sealed class ResolveBoundaryCaoPiFangzhuDescriptor : BoundaryCaoPiDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveBoundaryCaoPiFangzhu;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeLogicalDiscard);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied), new ReadSelectedTarget(), new CreateChoiceResult(e.ResultBind!, ["drawn", "discarded"])];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.Frame.BoundaryFangzhu is { Stage: not BoundaryFangzhuStage.Choice };
}
internal sealed class ChooseBoundaryCaoPiXingshangDescriptor : BoundaryCaoPiDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseBoundaryCaoPiXingshang;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CharacterDied), new CreateChoiceResult(e.ResultBind!, ["claim", "recover"])];
}
internal sealed class BoundaryCaoPiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h) => h.GetCapability<IBoundaryCaoPiHost>().Resolve(f, e);
}
public enum BoundaryFangzhuStage { Choice, DiscardChildren, DrawRequest, DrawChildren, Complete }
public sealed record BoundaryFangzhuReceipt(CardConversionSource Source, string Hash, long DamageWindow, long? DamageId, int Target, int X, string ResultBind)
{
    public BoundaryFangzhuStage Stage { get; init; }
    private readonly IReadOnlyList<int> _selected = [];
    public IReadOnlyList<int> Selected { get => _selected; init => _selected = Array.AsReadOnly(value.ToArray()); }
    public string? Outcome { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public long? DiscardInvocation { get; init; }
    public int Actual { get; init; }
    public long? DrawAttempt { get; init; }
    public bool DrawReplaced { get; init; }
}
public sealed record BoundaryXingshangReceipt(CardConversionSource Source, string Hash, long DeathWindow, long DeathId, int Victim, string ResultBind);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryFangzhuReceipt? BoundaryFangzhu { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryXingshangReceipt? BoundaryXingshang { get; init; }
}
public sealed record BoundaryFangzhuStartedEvent(long FrameId, CardConversionSource Source, string Hash, long DamageWindow, long? DamageId, int Target, int X, string ResultBind) : IGameEvent;
public sealed record BoundaryFangzhuPaidEvent(long FrameId, int Target, int X, long? DiscardInvocation, int Actual, long Before, long After) : IGameEvent;
public sealed record BoundaryFangzhuDrawReturnedEvent(long FrameId, int Target, int Requested, int Actual, long? Attempt, bool Replaced, long Before, long After) : IGameEvent;
public sealed record BoundaryCaoPiChoiceResultEvent(long FrameId, string SkillId, string Bind, string Outcome, int Chooser, int Subject, int Count) : IGameEvent;
