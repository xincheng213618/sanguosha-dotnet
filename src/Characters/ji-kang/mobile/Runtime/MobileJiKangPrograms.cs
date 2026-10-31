using System.Text.Json.Serialization;
namespace CardGame.Core;

public enum MobileJiKangStage { Targets, Cards, CostChildren, Compare, Health, HealthChildren, Drawing, Reward, RewardDrawing, KillerDiscardChildren, KillerHealth, KillerHealthChildren, Recipient, GrantChildren, Club, ClubChildren, FinalGrantChildren }
public sealed record MobileJiKangReceipt
{
    public CardConversionSource Source { get; init; } = null!;
    public string Hash { get; init; } = "";
    public MobileJiKangStage Stage { get; init; }
    public int InitialHp { get; init; }
    public int Cursor { get; init; }
    public int Subject { get; init; } = -1;
    public int Comparison { get; init; }
    public int? Killer { get; init; }
    public bool Paid { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    private IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>()),_cards = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MobileJiKangIssuedEvent(long FrameId, CardConversionSource Source, string Hash, int InitialHp, int? Killer) : IGameEvent;
public sealed record MobileJiKangTargetIssuedEvent(long FrameId, CardConversionSource Source, int Index, int TargetSeat) : IGameEvent;
public sealed record MobileJiKangProgressEvent(long FrameId, string Step, int Cursor, int Subject, int Value, int Other, long Before = 0, long After = 0) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileJiKangReceipt? MobileJiKang { get; init; }
}
internal interface IMobileJiKangHost { SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal abstract class MobileJiKangDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ISkillProgramEffectHandler Handler { get; } = new MobileJiKangHandler(op);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, c) => c.PublicControlValue(3));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => op == SkillProgramEffectOp.ResolveMobileJiKangQingxian
        ? [new RequireActivationEntry()] : [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];
}
internal sealed class MobileJiKangQingxianDescriptor() : MobileJiKangDescriptor(SkillProgramEffectOp.ResolveMobileJiKangQingxian);
internal sealed class MobileJiKangJuexiangDescriptor() : MobileJiKangDescriptor(SkillProgramEffectOp.ResolveMobileJiKangJuexiang)
{
    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) =>
        context.Frame.SkillId == "mobile:juexiang" && context.Frame.TriggerId == "actual-death-bequest" &&
        context.Frame.WindowContext is { Window: SkillProgramTriggerWindow.OwnerDied } death &&
        death.OwnerSeat == context.Frame.OwnerSeat;
}
internal sealed class MobileJiKangHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileJiKangHost>().Resolve(f, op);
}
internal static class MobileJiKangComposition
{
    [ProgramCatalogValidation(80900)]
    internal static void Validate(string path, SkillProgram p)
    {
        var pulse = SkillProgramEffectOp.ResolveMobileJiKangQingxian; var bequest = SkillProgramEffectOp.ResolveMobileJiKangJuexiang;
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == pulse || e.Op == bequest)) return;
        bool Plain(IReadOnlyList<SkillProgramEffect> effects, SkillProgramEffectOp op) => effects is [var e] && e.Op == op &&
            e.Target == SkillProgramEffectTarget.Owner && e.Condition.Kind == SkillProgramConditionKind.Always;
        var valid = p.Id switch
        {
            "mobile:qingxian" or "mobile:canyun" => p.Triggers.Count == 0 && p.Activations is [var a] && a.Id == "equipment-comparison" &&
                a.MinCards == 0 && a.MaxCards == 0 && a.MinTargets == 0 && a.MaxTargets == 0 && a.TargetKind == SkillProgramTargetKind.AnyLiving &&
                a.UsesPerTurn is null && a.UsesPerPhase == 1 && a.UsesPerGame is null && a.Condition.Kind == SkillProgramConditionKind.Always && Plain(a.Effects, pulse),
            "mobile:juexiang" => p.Activations.Count == 0 && p.Triggers is [var t] && t.Id == "actual-death-bequest" &&
                t.Window == SkillProgramTriggerWindow.OwnerDied && t.Subject == SkillProgramTriggerSubject.Owner && !t.Optional &&
                t.UsageScope is null && t.UsageLimit is null && t.Condition.Kind == SkillProgramTriggerConditionKind.Always && Plain(t.Effects, bequest),
            _ => false
        };
        if (!valid || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.Contributions.Count != 0 || p.BooleanStates.Count != 0 ||
            p.CardPolicies.Count != 0 || p.CardIdentities.Count != 0 || p.DamageModifiers.Count != 0 || p.HpSkillGrants.Count != 0 || p.EquipmentSuitSkillGrants.Count != 0)
            throw new InvalidOperationException($"Invalid skill program at {path}: mobile Ji Kang requires his complete original phase activation or mandatory death bequest.");
    }
}
