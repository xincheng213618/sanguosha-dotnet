using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record OlSpLiuBei664Draft
{
    private readonly IReadOnlyList<int> _cards = Array.Empty<int>();
    private readonly IReadOnlyList<PromptChoice> _choices = Array.Empty<PromptChoice>();
    public string Stage { get; init; } = "materials";
    public int Minimum { get; init; }
    public int? TargetSeat { get; init; }
    public int TurnNumber { get; init; }
    public bool AllClub { get; init; }
    public long? UseId { get; init; }
    public long? DyingFrameId { get; init; }
    public long SequenceBefore { get; init; }
    public long SequenceAfter { get; init; }
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<PromptChoice> Choices { get => _choices; init => _choices = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public OlSpLiuBei664Draft? OlSpLiuBei664 { get; init; }
}
public sealed record OlSpLiuBeiZhujiuIssuedEvent(long FrameId, int OwnerSeat, long UseId, int Minimum, int PaidCount, bool AllClub, int TurnNumber) : IGameEvent;
public sealed record OlSpLiuBeiZhujiuDisabledEvent(long FrameId, int OwnerSeat, int TurnNumber) : IGameEvent;
public sealed record OlSpLiuBeiZhujiuReturnedEvent(long FrameId, int OwnerSeat, long UseId, long? DyingFrameId) : IGameEvent;
public sealed record OlSpLiuBeiJingleiPaidEvent(long FrameId, int OwnerSeat, long AlcoholUseId) : IGameEvent;

internal interface IOlSpLiuBei664Host
{ SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffect effect); }
internal abstract class OlSpLiuBei664Descriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance,
        Activated: ProgramTrackedFact.NativeActionAppearance);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, context) => context.PublicControlValue(2));
    public override ISkillProgramEffectHandler Handler => new OlSpLiuBei664Handler(Op);
}
internal sealed class UseOlSpLiuBeiZhujiuDescriptor : OlSpLiuBei664Descriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.UseOlSpLiuBeiZhujiu;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}
internal sealed class GrantOlSpLiuBeiXudaiDescriptor : OlSpLiuBei664Descriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantOlSpLiuBeiXudai;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}
internal sealed class ResolveOlSpLiuBeiJingleiDescriptor : OlSpLiuBei664Descriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "stage", "condition");
        var stage = reader.RequiredIdentifier("stage");
        if (stage is not ("pay" or "adjust")) throw new InvalidOperationException(reader.Path + ": Jinglei requires pay/adjust stage.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(), stateId: stage);
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCompleted)];
}
internal sealed class OlSpLiuBei664Handler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IOlSpLiuBei664Host>().Execute(frame, effect);
}
internal static class OlSpLiuBei664Composition
{
    [ProgramCatalogValidation(5431)]
    internal static void Validate(string path, SkillProgram program)
    {
        var relevant = program.Activations.SelectMany(entry => entry.Effects).Concat(program.Triggers.SelectMany(entry => entry.Effects))
            .Any(effect => effect.Op is SkillProgramEffectOp.UseOlSpLiuBeiZhujiu or SkillProgramEffectOp.GrantOlSpLiuBeiXudai or SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei);
        if (!relevant) return;
        if (program.Id == "ol:xudai-3576" && program.Activations.Count == 0 && program.Triggers.Count == 2 && program.Triggers.All(trigger =>
            trigger.Optional && trigger.UsageScope == SkillUsageScope.Game && trigger.UsageLimit == 1 && trigger.NamedUsageGroup == "xudai" &&
            trigger.Window is SkillProgramTriggerWindow.CardResponseCompleted or SkillProgramTriggerWindow.AfterDamageApplied && trigger.Effects is
                [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving }, { Op: SkillProgramEffectOp.GrantOlSpLiuBeiXudai }])) return;
        if (program.Id == "ol:zhujiu-3577" && program.Triggers is [{ Window: SkillProgramTriggerWindow.SelfDyingResponse,
                Subject: SkillProgramTriggerSubject.Owner, Optional: true, Effects: [{ Op: SkillProgramEffectOp.UseOlSpLiuBeiZhujiu }] }] && program.Activations is
            [{ MinCards: 0, MaxCards: 0, MinTargets: 0, MaxTargets: 0, UsesPerTurn: null, Effects: [{ Op: SkillProgramEffectOp.UseOlSpLiuBeiZhujiu }] }]) return;
        if (program.Id == "ol:jinglei-3578" && program.Activations.Count == 0 && program.Triggers is
            [{ Window: SkillProgramTriggerWindow.CardUseCompleted, Optional: true, UsageScope: SkillUsageScope.Turn, UsageLimit: 1,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Observer, Effects: [{ Op: SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei, StateId: "pay" }, { Op: SkillProgramEffectOp.ResolveOlSpLiuBeiJinglei, StateId: "adjust" }] }]) return;
        throw new InvalidOperationException(path + ": SP Liu Bei 664 requires its original zero-card brewing entry or paid completed-alcohol damage/adjustment chain.");
    }
}
