using System.Text.Json.Serialization;
namespace CardGame.Core;
public sealed partial record ProgramSkillFrame
{ [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public LeChengyanReceipt? LeChengyan { get; init; } }

internal interface ILeZhugeGuoHost
{
    SkillProgramStepOutcome Reorder(ProgramSkillFrame f);
    SkillProgramStepOutcome Chengyan(ProgramSkillFrame f);
}
internal abstract class LeZhugeGuoDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [];
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(3));
    public override ISkillProgramEffectHandler Handler => new LeZhugeGuoHandler(Op);
}
internal sealed class LeZhugeGuoHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost h) => op switch
    {
        SkillProgramEffectOp.ReorderLeZhugeGuoXidi => h.GetCapability<ILeZhugeGuoHost>().Reorder(f),
        _ => h.GetCapability<ILeZhugeGuoHost>().Chengyan(f)
    };
}
