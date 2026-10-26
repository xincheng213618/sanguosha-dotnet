using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NativeDrawOutcome? ShanheBuwuDrawOutcome { get; init; }
}
public sealed record ShanheBuwuDrawIssuedEvent(long FrameId, int OwnerSeat, long DamageFrameId, int Amount) : IGameEvent;
public sealed record ShanheShuanghunUseRestoredEvent(long FrameId, int OwnerSeat) : IGameEvent;

internal interface IShanheGuideSkillsHost
{ SkillProgramStepOutcome ResolveGuideSkill(ProgramSkillFrame frame, SkillProgramEffect effect); }
internal abstract class ShanheGuideSkillDescriptor : ProgramOperationDescriptorBase
{
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler => new ShanheGuideSkillHandler(Op);
}
internal sealed class ResolveShanheBuwuDescriptor : ShanheGuideSkillDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveShanheBuwu;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "stage", "condition");
        var stage = reader.RequiredIdentifier("stage");
        if (stage is not ("before" or "after")) throw new InvalidOperationException(reader.Path + ": Buwu requires before/after damage.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 1, reader.Condition(), stateId: stage);
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(effect.StateId == "before" ? SkillProgramTriggerWindow.BeforeDamageApplied : SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class RecoverShanheShuanghunDescriptor : ShanheGuideSkillDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecoverShanheShuanghun;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.SelfDyingResponse)];
}
internal sealed class RestoreShanheShuanghunUseDescriptor : ShanheGuideSkillDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RestoreShanheShuanghunUse;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.SelfDyingResponse)];
}
internal sealed class ShanheGuideSkillHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheGuideSkillsHost>().ResolveGuideSkill(frame, effect);
}
internal static class ShanheGuideSkillsComposition
{
    [ProgramCatalogValidation(5409)]
    internal static void Validate(string path, SkillProgram program)
    {
        var operations = program.Triggers.SelectMany(trigger => trigger.Effects).Where(effect => effect.Op is
            SkillProgramEffectOp.ResolveShanheBuwu or SkillProgramEffectOp.RecoverShanheShuanghun or SkillProgramEffectOp.RestoreShanheShuanghunUse).ToArray();
        if (operations.Length == 0) return;
        if (program.Activations.Count != 0) throw new InvalidOperationException(path + ": guide passives have no activations.");
        if (program.Id == "shanhe:buwu-3365" && program.Triggers.Count == 2 && program.Triggers.All(trigger => !trigger.Optional &&
            trigger.Subject == SkillProgramTriggerSubject.Any && trigger.Effects is [{ Op: SkillProgramEffectOp.ResolveShanheBuwu }]) &&
            operations.Select(effect => effect.StateId).ToHashSet().SetEquals(["before", "after"])) return;
        if (program.Id == "shanhe:shuanghun-3366" && program.Triggers is [{ Window: SkillProgramTriggerWindow.SelfDyingResponse,
                Optional: false, UsageScope: SkillUsageScope.Game, UsageLimit: 1, NamedUsageGroup: "shuanghun", Effects: [
                    { Op: SkillProgramEffectOp.SetChainedState, Chained: false }, { Op: SkillProgramEffectOp.SetFaceState, FaceDown: false },
                    { Op: SkillProgramEffectOp.RecoverShanheShuanghun }, { Op: SkillProgramEffectOp.Draw, NumberExpression: SkillProgramNumberExpression.TargetMaxHpMinusHandCount },
                    { Op: SkillProgramEffectOp.RestoreShanheShuanghunUse }] }]) return;
        throw new InvalidOperationException(path + ": guide skills require their exact damage or paid rebirth composition.");
    }
}
