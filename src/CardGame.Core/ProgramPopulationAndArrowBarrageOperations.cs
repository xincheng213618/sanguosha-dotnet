using System.Collections.ObjectModel;
namespace CardGame.Core;

public sealed record ProgramPopulationMarkerGrant(int OwnerSeat, string SkillId);
public sealed record ProgramArrowBarrageExclusion(long ProducerFrameId, int OwnerSeat, string SkillId,
    string SkillInstanceId, long CardActionId, int TargetSeat);
public sealed record ProgramArrowBarrageTargetRemovedEvent(long CardUseFrameId, long CardActionId,
    int OwnerSeat, string SkillId, int TargetSeat) : IGameEvent;

public interface IPopulationAndArrowBarrageProgramHost
{
    void GrantFactionPopulationMarker(ProgramSkillFrame frame, SkillProgramEffect effect);
    void RemoveSelectedCurrentArrowBarrageTarget(ProgramSkillFrame frame);
}
internal sealed class GrantFactionPopulationMarkerDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantFactionPopulationMarker;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantFactionPopulationMarkerHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChangeAttributedMarker, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "factionId", "marker", "multiplier", "condition");
        var multiplier = r.RequiredInt("multiplier");
        if (multiplier < 0) throw new InvalidOperationException("Population marker multiplier must be nonnegative.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), multiplier,
            r.Condition(), marker: r.RequiredEnum<PlayerMarkerKind>("marker"), providerFactionId: r.RequiredIdentifier("factionId"));
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)];
}
internal sealed class RemoveSelectedCurrentArrowBarrageTargetDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget;
    public override ISkillProgramEffectHandler Handler { get; } = new RemoveSelectedCurrentArrowBarrageTargetHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.NullifySelectedCardEffects,
        static (_, context) => context.NullifySelectedCardEffects());
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTargets)
            throw new InvalidOperationException("ArrowBarrage exclusion requires the selected target set.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.SelectedTargets, 0, r.Condition());
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new ReadTargetSet(1, 1), new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseTargetsFinalized),
         new RequireCardActionRelation(SkillProgramCardActionOwnerRelation.Actor, [CardKind.ArrowBarrage])];
}
public sealed class GrantFactionPopulationMarkerHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantFactionPopulationMarker;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IPopulationAndArrowBarrageProgramHost>().GrantFactionPopulationMarker(f, e); return SkillProgramStepOutcome.Continue; }
}
public sealed class RemoveSelectedCurrentArrowBarrageTargetHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RemoveSelectedCurrentArrowBarrageTarget;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IPopulationAndArrowBarrageProgramHost>().RemoveSelectedCurrentArrowBarrageTarget(f); return SkillProgramStepOutcome.Continue; }
}
