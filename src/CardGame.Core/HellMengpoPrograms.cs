using System.Text.Json.Serialization;

namespace CardGame.Core;

internal interface IHellMengpoProgramHost
{
    SkillProgramStepOutcome Forget(ProgramSkillFrame frame);
    SkillProgramStepOutcome BanCategory(ProgramSkillFrame frame);
    SkillProgramStepOutcome Restore(ProgramSkillFrame frame);
}

internal abstract class HellMengpoOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override ISkillProgramEffectHandler Handler => new HellMengpoEffectHandler(Op);
}
internal sealed class ResolveHellWanghunLossDescriptor : HellMengpoOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveHellWanghunLoss;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Death;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.OwnerDied)];

    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}
internal sealed class BanHellWangshiCategoryDescriptor : HellMengpoOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.BanHellWangshiCategory;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}
internal sealed class RestoreHellWanghunSkillsDescriptor : HellMengpoOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RestoreHellWanghunSkills;
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}
internal sealed class HellMengpoEffectHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => op switch
    {
        SkillProgramEffectOp.ResolveHellWanghunLoss => host.GetCapability<IHellMengpoProgramHost>().Forget(frame),
        SkillProgramEffectOp.BanHellWangshiCategory => host.GetCapability<IHellMengpoProgramHost>().BanCategory(frame),
        SkillProgramEffectOp.RestoreHellWanghunSkills => host.GetCapability<IHellMengpoProgramHost>().Restore(frame),
        _ => throw new InvalidOperationException("Unknown Mengpo operation.")
    };
}

public enum HellWanghunStage { LossChildren, InjectionChildren }
public sealed record HellWanghunReceipt(int InstructionIndex, long DeathFrameId, long DeathWindowId, HellWanghunStage Stage,
    int LostSkillCount, int LostGrantCount, int InjectedCards = 0);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HellWanghunReceipt? HellWanghun { get; init; }
}

// Every removed grant has its own immutable fact. These are durable lost
// ownership receipts, not an engine-side pending restore list.
public sealed record HellWanghunTargetChosenEvent(long FrameId, int MengpoSeat, int TargetSeat, string? SkillId) : IGameEvent;
public sealed record HellWanghunGrantLostEvent(long FrameId, int MengpoSeat, int TargetSeat,
    string GeneralId, string? SecondaryGeneralId, SkillGrant Grant) : IGameEvent;
public sealed record HellWanghunIssuedEvent(long FrameId, long DeathFrameId, int MengpoSeat,
    int LostSkillCount, int LostGrantCount) : IGameEvent;
public sealed record HellReturnSoulInjectedEvent(long FrameId, int MengpoSeat, int CardId) : IGameEvent;
public sealed record HellWanghunCompletedEvent(long FrameId, int LostSkillCount, int InjectedCards) : IGameEvent;
public sealed record HellWangshiCategoryBanEvent(long FrameId, int MengpoSeat, int TurnNumber,
    int TurnOwnerSeat, string Category) : IGameEvent;

public sealed record HellShiyouRedirectReceipt(ActualDiscardRecoveryPhaseKey Phase, CardConversionSource Source,
    string GameplayHash, int CardId, CardKind CardKind, Suit EffectiveSuit,
    CardLocation OriginalSource, CardLocation ActualFrom, CardLocation Destination, CardMoveReason Reason);
internal sealed record HellShiyouTransferPlan
{
    internal long PlanId { get; init; }
    internal IReadOnlyList<CardTransfer> Transfers { get; init; } = Array.AsReadOnly(Array.Empty<CardTransfer>());
    internal IReadOnlyList<HellShiyouRedirectReceipt> Redirects { get; init; } = Array.AsReadOnly(Array.Empty<HellShiyouRedirectReceipt>());
}
public sealed record HellShiyouInterceptedEvent(long PlanId, long MovementSequence,
    HellShiyouRedirectReceipt Receipt) : IGameEvent;

public sealed partial class GameEngine
{
}
