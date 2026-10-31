using System.Collections.ObjectModel;

namespace CardGame.Core;
public interface IParticipantPhaseProgramEffectHost
{
    void ConsumeTargetPhaseLedger(ProgramSkillFrame frame, string usageId);
    void GrantTurnBoundSuitUseProhibition(ProgramSkillFrame frame, string sourceBind);
}

internal sealed class ConsumeTargetPhaseLedgerDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeTargetPhaseLedger;
    public override ISkillProgramEffectHandler Handler { get; } = new ConsumeTargetPhaseLedgerHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.CaptureSelectedCards, static (_, _) =>
    {
    });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "usageId", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), stateId: r.RequiredIdentifier("usageId"));
        RequireAlways(effect, r.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}

public sealed class ConsumeTargetPhaseLedgerHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeTargetPhaseLedger;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IParticipantPhaseProgramEffectHost>().ConsumeTargetPhaseLedger(frame, effect.StateId!);
        return SkillProgramStepOutcome.Continue;
    }
}

internal sealed class GrantTurnBoundSuitUseProhibitionDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantTurnBoundSuitUseProhibitionHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnCardActionProhibition, static (_, _) =>
    {
    });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, r.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadCardSet(effect.SourceBind!)];
}

public sealed class GrantTurnBoundSuitUseProhibitionHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantTurnBoundSuitUseProhibition;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IParticipantPhaseProgramEffectHost>().GrantTurnBoundSuitUseProhibition(frame, effect.SourceBind!);
        return SkillProgramStepOutcome.Continue;
    }
}
