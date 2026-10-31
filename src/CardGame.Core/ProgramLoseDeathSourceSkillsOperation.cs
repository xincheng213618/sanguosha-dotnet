namespace CardGame.Core;

public sealed record CharacterSkillsLostEvent(int Seat, int SourceSeat, IReadOnlyList<string> SkillIds) : IGameEvent;

internal sealed class LoseDeathSourceSkillsProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.LoseDeathSourceSkills;
    public override ISkillProgramEffectHandler Handler { get; } = new LoseDeathSourceSkillsSkillProgramEffectHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Death;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.ClaimDeathCleanupCards, static (_, _) => { });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "condition");
        var target = reader.RequiredEnum<SkillProgramEffectTarget>("target");
        if (target != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.target: death source skill loss requires owner.");
        var effect = new SkillProgramEffect(Op, target, 0, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];

    public override bool AllowsDeadTarget(in ProgramEffectContinuationContext context) => true;
}

public sealed class LoseDeathSourceSkillsSkillProgramEffectHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.LoseDeathSourceSkills;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host)
    {
        host.LoseDeathSourceSkills(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
