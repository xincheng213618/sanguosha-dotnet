using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IMobileZhuHuanPingkouHost { SkillProgramStepOutcome Begin(ProgramSkillFrame frame); }
internal sealed class ResolveMobileZhuHuanPingkouDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileZhuHuanPingkou;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileZhuHuanPingkouHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy => new(ProgramOperationAiSemantic.SelectTarget, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterTurnEnded)];
}
internal sealed class MobileZhuHuanPingkouHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileZhuHuanPingkou;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IMobileZhuHuanPingkouHost>().Begin(frame);
}
public sealed record MobileZhuHuanPingkouReceipt(CardConversionSource Source, string GameplayHash, int ActualTurn, long EndedFrame, int Maximum, bool Sealed = false, int Index = 0)
{
    private readonly IReadOnlyList<int> _targets = Array.AsReadOnly(Array.Empty<int>());
    public IReadOnlyList<int> Targets { get => _targets; init => _targets = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileZhuHuanPingkouReceipt? MobileZhuHuanPingkou { get; init; }
}
public sealed record MobileZhuHuanPingkouStartedEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash, int ActualTurn, long EndedFrame, int Maximum) : IGameEvent;
public sealed record MobileZhuHuanPingkouTargetEvent(long FrameId, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouSealedEvent(long FrameId, int Count) : IGameEvent;
public sealed record MobileZhuHuanPingkouDamageIssuedEvent(long FrameId, int Index, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouDamageReturnedEvent(long FrameId, int Index, int Target) : IGameEvent;
public sealed record MobileZhuHuanPingkouCompletedEvent(long FrameId) : IGameEvent;
