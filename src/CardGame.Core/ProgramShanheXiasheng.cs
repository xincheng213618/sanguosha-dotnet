using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record ShanheXiashengReceipt(long ParentWindowId, long ActionId, string Stage, int RedCount, int BlackCount)
{
    private IReadOnlyList<PromptChoice> _choices = Array.AsReadOnly(Array.Empty<PromptChoice>());
    private IReadOnlyList<int> _originalTargets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<PromptChoice> Choices { get => _choices; init => _choices = Array.AsReadOnly(value.Select(PromptChoiceProjection.Freeze).ToArray()); }
    public IReadOnlyList<int> OriginalTargets { get => _originalTargets; init => _originalTargets = Array.AsReadOnly(value.ToArray()); }
    public NativeDrawOutcome? DrawOutcome { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheXiashengReceipt? ShanheXiasheng { get; init; }
}
public sealed record ShanheXiashengBranchIssuedEvent(long FrameId, int OwnerSeat, long ActionId, string Branch, int RedCount, int BlackCount) : IGameEvent;
internal interface IShanheXiashengHost
{ SkillProgramStepOutcome ResolveXiasheng(ProgramSkillFrame frame, string stage); }
internal sealed class ResolveShanheXiashengDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheXiasheng;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "stage", "condition");
        var stage = reader.RequiredIdentifier("stage");
        if (stage is not ("draw" or "target")) throw new InvalidOperationException(reader.Path + ": Xiasheng requires draw/target.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 1, reader.Condition(), stateId: stage);
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(effect.StateId == "draw" ? SkillProgramTriggerWindow.CardUseCommitted : SkillProgramTriggerWindow.CardUseTargetsFinalized)];
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveShanheXiashengHandler();
}
internal sealed class ResolveShanheXiashengHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheXiasheng;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheXiashengHost>().ResolveXiasheng(frame, effect.StateId!);
}
internal static class ShanheXiashengComposition
{
    [ProgramCatalogValidation(5408)]
    internal static void Validate(string path, SkillProgram program)
    {
        var entries = program.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheXiasheng)).ToArray();
        if (entries.Length == 0) return;
        if (program.Id != "shanhe:xiasheng-3362" || program.Activations.Count != 0 || entries.Length != 2 ||
            entries.Any(trigger => trigger.Optional || trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || trigger.Effects.Count != 1) ||
            !entries.Any(trigger => trigger.Window == SkillProgramTriggerWindow.CardUseCommitted && trigger.Effects[0].StateId == "draw") ||
            !entries.Any(trigger => trigger.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && trigger.Effects[0].StateId == "target"))
            throw new InvalidOperationException(path + ": Xiasheng requires the mandatory committed draw and finalized target branches.");
    }
}
internal sealed class ShanheXiashengCommittedUseContract : IProgramCommittedUseContract
{
    public string SkillId => "shanhe:xiasheng-3362";
    public void ValidateTrigger(SkillProgramTrigger trigger)
    {
        if (trigger is not { Id: "red-hand-black-use", Window: SkillProgramTriggerWindow.CardUseCommitted,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, IncludeResponseUses: true, Optional: false,
                UsageScope: null, Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.ResolveShanheXiasheng, Target: SkillProgramEffectTarget.Owner,
                    StateId: "draw", Condition.Kind: SkillProgramConditionKind.Always }] })
            throw new InvalidOperationException("Xiasheng's committed response use requires its one mandatory actual-actor draw operation.");
    }
}
