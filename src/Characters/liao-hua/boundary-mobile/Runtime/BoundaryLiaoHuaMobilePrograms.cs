using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IBoundaryLiaoHuaMobileHost
{
    SkillProgramStepOutcome Execute(ProgramSkillFrame frame, SkillProgramEffectOp op);
}

internal abstract class BoundaryLiaoHuaMobileDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new BoundaryLiaoHuaMobileHandler(Op);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class ObtainBoundaryLiaoHuaDiscardSlashDescriptor : BoundaryLiaoHuaMobileDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ObtainBoundaryLiaoHuaDiscardSlash;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
}
internal sealed class BoundaryLiaoHuaMobileHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundaryLiaoHuaMobileHost>().Execute(frame, Op);
}

public sealed record BoundaryLiaoHuaDiscardSlashReceipt(CardConversionSource Source, string GameplayHash, long WindowFrameId, int InstructionIndex)
{
    private readonly IReadOnlyList<int> _candidates = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Candidates { get => _candidates; init => _candidates = Array.AsReadOnly(value.ToArray()); }
    public int? CardId { get; init; }
    public long Before { get; init; }
    public long After { get; init; }
    public long BatchId { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryLiaoHuaDiscardSlashReceipt? BoundaryLiaoHuaDiscardSlash { get; init; }
}
public sealed record BoundaryLiaoHuaDiscardSlashObtainedEvent(long FrameId, int InstructionIndex, int OwnerSeat,
    int CardId, long BatchId, long Before, long After) : IGameEvent;
