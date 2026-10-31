using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record MobileSpXuShuJianyanMovementReceipt(string Kind, long BatchId, long Before, long After, IReadOnlyList<CardMovementRecord> Movements)
{
    private readonly IReadOnlyList<CardMovementRecord> _movements = Array.AsReadOnly(Movements.ToArray());
    public IReadOnlyList<CardMovementRecord> Movements { get => _movements; init => _movements = Array.AsReadOnly(value.ToArray()); }
}
public sealed record MobileSpXuShuJianyanReceipt(string Stage, string? Criterion, IReadOnlyList<int> RevealedIds,
    int? MatchedCardId = null, int? RecipientSeat = null)
{
    private readonly IReadOnlyList<int> _revealed = Array.AsReadOnly(RevealedIds.ToArray());
    public IReadOnlyList<int> RevealedIds { get => _revealed; init => _revealed = Array.AsReadOnly(value.ToArray()); }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileSpXuShuJianyanMovementReceipt? Movement { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public MobileSpXuShuJianyanReceipt? MobileSpXuShuJianyan { get; init; }
}
public sealed record MobileSpXuShuJianyanRevealedEvent(long FrameId, string Criterion, int CardId, bool Matched, long MovementSequence) : IGameEvent;
public sealed record MobileSpXuShuJianyanGiftPaidEvent(long FrameId, int RecipientSeat, int CardId, long MovementSequence) : IGameEvent;
public sealed record MobileSpXuShuJianyanCleanupPaidEvent(long FrameId, int Count, long Before, long After) : IGameEvent;
public sealed record MobileSpXuShuJianyanNativeBatchIssuedEvent(long FrameId, string Kind, long BatchId, long ParentFrameId,
    long AwaitingProgramFrameId, int OwnerSeat, string SkillId, string SkillInstanceId, long Before, long After, int Count) : IGameEvent;
internal interface IMobileSpXuShuJianyanHost { SkillProgramStepOutcome Begin(ProgramSkillFrame f); }
internal sealed class ResolveMobileSpXuShuJianyanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSpXuShuJianyan;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileSpXuShuJianyanHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new CreateCardSet("mobile-jianyan-revealed", int.MaxValue, false)];
}
internal sealed class MobileSpXuShuJianyanHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileSpXuShuJianyan;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileSpXuShuJianyanHost>().Begin(f);
}

public sealed partial class GameEngine
{
}
