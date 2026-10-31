using System.Text.Json.Serialization;

namespace CardGame.Core;
public sealed record AdoptedChildRelationshipReceipt(long FrameId, CardConversionSource Source, string GameplayHash, long ParentFrameId, int ActualTurn, int? PreviousHolder, bool Transfer);
public sealed record AdoptedChildSlashUseReceipt(long CardUseFrameId, int ActorSeat, int ActualTurn, int PhaseInstance);
public sealed record AdoptedChildDamageReceipt(long DamageFrameId, long CardUseFrameId, int ActorSeat, int TargetSeat, int ActualTurn, int PhaseInstance, string SkillInstanceId);
public sealed record AdoptedChildDamageIssue(long CardUseFrameId, int ActorSeat, int TargetSeat, int ActualTurn, int PhaseInstance, string SkillInstanceId);
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AdoptedChildRelationshipReceipt? AdoptedChildRelationship { get; init; }
}

public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AdoptedChildSlashUseReceipt? AdoptedChildSlashUse { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AdoptedChildDamageIssue? AdoptedChildDamageIssue { get; init; }
}

public sealed partial record DamageFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public AdoptedChildDamageReceipt? AdoptedChildDamage { get; init; }
}

public sealed record AdoptedChildAssignedEvent(long FrameId, CardConversionSource Source, int? PreviousHolder, int HolderSeat, int ActualTurn) : IGameEvent;
public sealed record AdoptedChildSkillGrantChangedEvent(int HolderSeat, string GrantId, string SkillInstanceId, bool Granted) : IGameEvent;
public sealed record RoundFirstSourcedDamageEvent(int RoundNumber, long DamageFrameId, int SourceSeat, int TargetSeat, int Amount) : IGameEvent;
public sealed record FirstRoundDamageQualifiedEvent(long FrameId, long DamageFrameId, int RoundNumber, int OwnerSeat, string SkillInstanceId, string BindingId) : IGameEvent;
public sealed record AdoptedChildPlayEndRequestedEvent(long DamageFrameId, long CardUseFrameId, int ActorSeat, int TargetSeat, int ActualTurn, int PhaseInstance) : IGameEvent;
internal interface IDingYuanProgramHost
{
    void PrepareAdoptedChild(ProgramSkillFrame frame, bool transfer);
    void AssignAdoptedChild(ProgramSkillFrame frame);
    void QualifyFirstRoundDamage(ProgramSkillFrame frame);
}

internal abstract class DingYuanOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) =>
    {
    });
    public override ISkillProgramEffectHandler Handler => new DingYuanOperationHandler(Op);
}

internal sealed class PrepareAdoptedChildAppointmentDescriptor : DingYuanOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareAdoptedChildAppointment;

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class PrepareAdoptedChildTransferDescriptor : DingYuanOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareAdoptedChildTransfer;

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)];
}

internal sealed class AssignAdoptedChildDescriptor : DingYuanOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.AssignAdoptedChild;

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow), new ReadSelectedTarget()];
}

internal sealed class QualifyFirstRoundDamageDescriptor : DingYuanOperationDescriptor
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.QualifyFirstRoundDamage;

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}

internal sealed class DingYuanOperationHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        var capability = host.GetCapability<IDingYuanProgramHost>();
        switch (op)
        {
            case SkillProgramEffectOp.PrepareAdoptedChildAppointment:
                capability.PrepareAdoptedChild(frame, false);
                break;
            case SkillProgramEffectOp.PrepareAdoptedChildTransfer:
                capability.PrepareAdoptedChild(frame, true);
                break;
            case SkillProgramEffectOp.AssignAdoptedChild:
                capability.AssignAdoptedChild(frame);
                break;
            case SkillProgramEffectOp.QualifyFirstRoundDamage:
                capability.QualifyFirstRoundDamage(frame);
                break;
            default:
                throw new InvalidOperationException("Unknown adopted-child instruction.");
        }

        return SkillProgramStepOutcome.Continue;
    }
}

internal readonly record struct AdoptedChildParticipant(int Seat, bool Alive, bool HasMark, bool HasCixiao);
internal interface IAdoptedChildSelectionQuery : IProgramSelectionQuery
{
    IReadOnlyList<AdoptedChildParticipant> Participants { get; }
}

[ProgramTargetResolver(SkillProgramTargetKind.AdoptedChildSuccessor, SkillProgramTargetKind.LivingCixiaoOwner)]
internal sealed class AdoptedChildTargetResolver : IProgramTargetResolver
{
    public IReadOnlyList<int> Resolve(ProgramTargetResolverContext context, IProgramSelectionQueries queries) => Array.AsReadOnly(queries.Get<IAdoptedChildSelectionQuery>().Participants.Where(player => player.Alive && (context.TargetKind == SkillProgramTargetKind.AdoptedChildSuccessor ? !player.HasMark && (player.Seat != context.OwnerSeat || queries.Get<IAdoptedChildSelectionQuery>().Participants.Any(person => person.Alive && person.HasMark)) : player.Seat != context.OwnerSeat && player.HasCixiao)).Select(player => player.Seat).ToArray());
}

public sealed partial class GameEngine
{
    private ref bool _synchronizingAdoptedChildGrants => ref _characterDingYuanOl.ReferenceToSynchronizingAdoptedChildGrants;
}
