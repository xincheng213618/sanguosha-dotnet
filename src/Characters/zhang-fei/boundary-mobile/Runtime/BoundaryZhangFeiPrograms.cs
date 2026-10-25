using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record BoundaryLiyongReserve(long ProducerFrameId, CardConversionSource Source, string GameplayHash,
    int TurnNumber, int PhaseInstanceId, long CancelledUseId, long CancelledActionId, int TargetSeat);
public sealed record BoundaryLiyongUse(BoundaryLiyongReserve Reserve, long CardUseFrameId, long CardActionId);
public sealed record BoundaryLiyongPenalty(long CardUseFrameId, long CardActionId, long DamageWindowId,
    long DamageFrameId, int TargetSeat, int InstructionIndex, bool Paid = false, int? HpBefore = null, int? HpAfter = null);
public sealed record BoundaryLiyongReservedEvent(BoundaryLiyongReserve Reserve) : IGameEvent;
public sealed record BoundaryLiyongUseIssuedEvent(BoundaryLiyongUse Use) : IGameEvent;
public sealed record BoundaryLiyongTargetIssuedEvent(long ProducerFrameId, long CardUseFrameId, long CardActionId, int TargetSeat) : IGameEvent;
public sealed record BoundaryLiyongPenaltyPaidEvent(long ProducerFrameId, long CardUseFrameId, long DamageWindowId,
    int TargetSeat, int OwnerSeat, int HpBefore, int HpAfter, int ActualLost) : IGameEvent;
public sealed partial record CardUseFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryLiyongUse? BoundaryLiyong { get; init; }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public BoundaryLiyongPenalty? BoundaryLiyongPenalty { get; init; }
}
internal interface IBoundaryZhangFeiHost
{
    SkillProgramStepOutcome ResolveBoundaryZhangFei(ProgramSkillFrame frame, SkillProgramEffectOp operation);
}
internal abstract class BoundaryZhangFeiDescriptor : ProgramOperationDescriptorBase
{
    public override ISkillProgramEffectHandler Handler => new BoundaryZhangFeiHandler(Op);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, new(SkillProgramConditionKind.Always, 0, []));
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => Op switch {
        SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong => [new RequireTriggerWindow(SkillProgramTriggerWindow.SlashFullyDodged), new RequireCardActionActor()],
        SkillProgramEffectOp.PayBoundaryZhangFeiLiyong => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)],
        _ => [new RequireCardActionActor()]
    };
}
internal sealed class ReserveBoundaryZhangFeiLiyongDescriptor : BoundaryZhangFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong; }
internal sealed class IssueBoundaryZhangFeiLiyongTargetDescriptor : BoundaryZhangFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueBoundaryZhangFeiLiyongTarget; }
internal sealed class PayBoundaryZhangFeiLiyongDescriptor : BoundaryZhangFeiDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.PayBoundaryZhangFeiLiyong; }
internal sealed class BoundaryZhangFeiHandler(SkillProgramEffectOp operation) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => operation;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) => host.GetCapability<IBoundaryZhangFeiHost>().ResolveBoundaryZhangFei(frame, operation);
}

internal static class BoundaryZhangFeiComposition
{
    [ProgramCatalogValidation(58600)]
    private static void Validate(string path, SkillProgram program)
    {
        var owned = program.Triggers.SelectMany(trigger => trigger.Effects).Concat(program.Activations.SelectMany(activation => activation.Effects))
            .Any(effect => effect.Op is SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong or SkillProgramEffectOp.IssueBoundaryZhangFeiLiyongTarget or SkillProgramEffectOp.PayBoundaryZhangFeiLiyong);
        if (!owned) return;
        if (program.Id != "boundary:liyong-mobile" || program.Activations.Count != 0 || program.Triggers.Count != 4)
            throw new InvalidOperationException(path + ": mobile Liyong requires its original four native timing bindings.");
        foreach (var trigger in program.Triggers)
        {
            var valid = trigger.Id switch {
                "reserve-after-real-dodge" => trigger.Window == SkillProgramTriggerWindow.SlashFullyDodged && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor &&
                    trigger.Effects is [{ Op: SkillProgramEffectOp.ReserveBoundaryZhangFeiLiyong }],
                "issue-next-real-use" => trigger.Window == SkillProgramTriggerWindow.CardUseCommitted && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor &&
                    trigger.Effects is [{ Op: SkillProgramEffectOp.IssueBoundaryZhangFeiLiyongTarget }],
                "issued-target-rights" => trigger.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized && trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor &&
                    trigger.Effects is [{ Op: SkillProgramEffectOp.IssueBoundaryZhangFeiLiyongTarget }, { Op: SkillProgramEffectOp.AddCurrentTargetSlashDamage, Amount: 1, FinalTargetComparison: ProgramFinalTargetComparison.Always }],
                "issued-surviving-damage" => trigger.Window == SkillProgramTriggerWindow.AfterDamageApplied && trigger.Subject == SkillProgramTriggerSubject.Any && trigger.DamageOccurrence == SkillProgramDamageOccurrence.PerDamage &&
                    trigger.Effects is [{ Op: SkillProgramEffectOp.PayBoundaryZhangFeiLiyong }],
                _ => false
            };
            if (!valid || trigger.Optional || trigger.Condition.Kind != SkillProgramTriggerConditionKind.Always ||
                trigger.Id != "issued-surviving-damage" && !ProgramCardKindMatch.IsFullSlashFamily(trigger.CardKinds) ||
                trigger.Effects.Any(effect => effect.Target != SkillProgramEffectTarget.Owner || effect.Condition.Kind != SkillProgramConditionKind.Always))
                throw new InvalidOperationException(path + ": mobile Liyong must preserve mandatory original cancellation, next-use, per-target and surviving-damage instructions.");
        }
    }
}
