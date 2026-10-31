using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundaryZhangZhaoPhaseDiscardEvent(ActualDiscardRecoveryPhaseKey Phase, int CardId, long MovementSequence, CardLocation OriginalFrom) : IGameEvent;
public sealed record BoundaryZhangZhaoReturnedEvent(long FrameId, int PhaseOwnerSeat, int CardId, long MovementSequence) : IGameEvent;
public sealed record BoundaryZhangZhaoClaimFinishedEvent(long FrameId, bool Taken, int Count, long Before, long After) : IGameEvent;
public sealed record BoundaryZhangZhaoPhaseReceipt(ActualDiscardRecoveryPhaseKey Phase, int PhaseOwnerSeat, string Stage,
    IReadOnlyList<DiscardRecoveryEntity> OriginalEntities, int? ReturnedCardId = null, long ReturnMovementSequence = 0)
{
    private readonly IReadOnlyList<DiscardRecoveryEntity> _entities = Array.AsReadOnly(OriginalEntities.ToArray());
    public IReadOnlyList<DiscardRecoveryEntity> OriginalEntities { get => _entities; init => _entities = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public BoundaryZhangZhaoPhaseReceipt? BoundaryZhangZhaoPhase { get; init; }
}
internal interface IBoundaryZhangZhaoMobileHost { SkillProgramStepOutcome Restore(ProgramSkillFrame f); }
internal sealed class CaptureBoundaryZhangZhaoPhaseDiscardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryZhangZhaoPhaseHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.DiscardPhaseEnded)];
}
internal sealed class BoundaryZhangZhaoPhaseHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.CaptureBoundaryZhangZhaoPhaseDiscards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryZhangZhaoMobileHost>().Restore(f);
}
