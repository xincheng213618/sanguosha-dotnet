namespace CardGame.Core;

public sealed record PopulationHpIncreasedEvent(int PlayerSeat, int Amount, int RemainingHp) : IGameEvent;

internal sealed class GrowMaximumHpAndHpProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrowMaximumHpAndHp;
    public override ISkillProgramEffectHandler Handler { get; } = new GrowMaximumHpAndHpSkillProgramEffectHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChangeMaximumHp, static (_, _) => { });
    public override ProgramSkillInstruction Compile(SkillProgramEffect effect) =>
        new GrowMaximumHpAndHpProgramInstruction(effect.NumberExpression ??
            throw new InvalidOperationException("Maximum-HP population growth has no numeric expression."));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "numberExpression", "condition");
        var expression = r.RequiredEnum<SkillProgramNumberExpression>("numberExpression");
        if (expression != SkillProgramNumberExpression.LivingFactionCount)
            throw new InvalidOperationException("Maximum-HP population growth requires livingFactionCount.");
        return new(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 0, r.Condition(), numberExpression: expression);
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.GameStarting)];
}
public sealed class GrowMaximumHpAndHpSkillProgramEffectHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrowMaximumHpAndHp;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        if (effect.CompiledInstruction is not GrowMaximumHpAndHpProgramInstruction instruction)
            throw new InvalidOperationException("Maximum-HP population growth has no compiled operation instruction.");
        host.GrowMaximumHpAndHp(frame, instruction.PopulationExpression);
        return SkillProgramStepOutcome.Continue;
    }
}
