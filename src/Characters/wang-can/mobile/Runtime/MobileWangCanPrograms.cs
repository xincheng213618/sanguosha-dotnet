using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum MobileWangCanXiStage { Mark, Choice, Cards, GiftChildren, Health, HealthChildren }
public sealed record MobileWangCanXiReceipt(CardConversionSource Source, string Hash, long ChangeId, int Subject, MobileWangCanXiStage Stage,
    long Before = 0, long After = 0, int ActualCount = 0)
{
    private IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MobileWangCanXiMovedEvent(long FrameId, CardConversionSource Source, string Hash, int TargetSeat) : IGameEvent;
public sealed record MobileWangCanXiQualifiedEvent(long ChangeId, CardConversionSource Source, string Hash, int TargetSeat) : IGameEvent;
public sealed record MobileWangCanXiIssuedEvent(long FrameId, CardConversionSource Source, string Hash, long ChangeId, int TargetSeat) : IGameEvent;
public sealed record MobileWangCanXiPaidEvent(long FrameId, int TargetSeat, bool Gift, int ActualCount, long Before, long After) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileWangCanXiReceipt? MobileWangCanXi { get; init; }
}
internal interface IMobileWangCanHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class MobileWangCanDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new MobileWangCanHandler(op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, c) => c.PublicControlValue(2));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(op == SkillProgramEffectOp.TransferMobileWangCanXi ? SkillProgramTriggerWindow.PlayPhaseStarting : SkillProgramTriggerWindow.AfterHpRecovered)];
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext c) => c.State.CanContinueAfterOwnerDeath(c.Frame);
}
internal sealed class TransferMobileWangCanXiDescriptor() : MobileWangCanDescriptor(SkillProgramEffectOp.TransferMobileWangCanXi);
internal sealed class ResolveMobileWangCanXiRecoveryDescriptor() : MobileWangCanDescriptor(SkillProgramEffectOp.ResolveMobileWangCanXiRecovery);
internal sealed class MobileWangCanHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileWangCanHost>().Resolve(f, op);
}
internal static class MobileWangCanComposition
{
    [ProgramCatalogValidation(81500)]
    private static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Any(e => e.Op is SkillProgramEffectOp.TransferMobileWangCanXi or SkillProgramEffectOp.ResolveMobileWangCanXiRecovery)) return;
        if (p.Id != "mobile:shanxi-wang-can" || p.Activations.Count != 0 || p.Triggers is not [
            { Id: "move-xi", Window: SkillProgramTriggerWindow.PlayPhaseStarting, Subject: SkillProgramTriggerSubject.Owner, Optional: true,
                Effects: [{ Op: SkillProgramEffectOp.TransferMobileWangCanXi, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] },
            { Id: "marked-recovery", Window: SkillProgramTriggerWindow.AfterHpRecovered, Subject: SkillProgramTriggerSubject.Owner, Optional: false, HpChangeOccurrence: SkillProgramHpChangeOccurrence.PerEvent,
                Effects: [{ Op: SkillProgramEffectOp.ResolveMobileWangCanXiRecovery, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always }] }] ||
            p.Triggers.Any(t => t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.UsageScope is not null || t.UsageLimit is not null))
            throw new InvalidOperationException(path + ": mobile Shanxi requires its exact optional Play-start mark and mandatory once-per-recovery consequence. " +
                System.Text.Json.JsonSerializer.Serialize(new { p.Id, Activations = p.Activations.Count, Triggers = p.Triggers.Select(t => new {
                    t.Id, t.Window, t.Subject, t.Optional, Condition = t.Condition.Kind, t.UsageScope, t.UsageLimit, t.HpChangeOccurrence,
                    Effects = t.Effects.Select(e => new { e.Op, e.Target, Condition = e.Condition.Kind }).ToArray() }).ToArray() }));
    }
}
