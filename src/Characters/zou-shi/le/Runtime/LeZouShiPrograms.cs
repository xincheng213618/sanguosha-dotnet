using System.Text.Json.Serialization;

namespace CardGame.Core;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HuoxinReceipt? Huoxin { get; init; } }

internal interface ILeZouShiHost
{
    SkillProgramStepOutcome ResolveHuoxin(ProgramSkillFrame frame);
}
internal abstract class LeZouShiDescriptor(SkillProgramEffectOp op) : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => op;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override ISkillProgramEffectHandler Handler { get; } = new LeZouShiHandler(op);
}
internal sealed class LeZouShiHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<ILeZouShiHost>().ResolveHuoxin(frame);
}
internal static class LeZouShiComposition
{
    [ProgramCatalogValidation(870)]
    internal static void ValidateProgram(string path, SkillProgram p)
    {
        foreach (var trigger in p.Triggers.Where(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveHuoxinNonEquipmentUse)))
            new HuoxinCommittedUseContract().ValidateTrigger(trigger);
    }
}
