namespace CardGame.Core;

internal sealed class DrawAllHandSelectedBonusProgramOperationDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawAllHandSelectedBonus;
    public override ISkillProgramEffectHandler Handler { get; } = new DrawAllHandSelectedBonusProgramHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(
        ProgramOperationAiSemantic.GainCards, static (effect, context) => context.Draw(effect));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "amount", "condition");
        if (reader.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.Owner)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.target: the bonus belongs to owner.");
        return new(Op, SkillProgramEffectTarget.Owner, DrawProgramOperationDescriptor.Amount(reader, 20), reader.Condition());
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [];
}

public sealed class DrawAllHandSelectedBonusProgramHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DrawAllHandSelectedBonus;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame,
        int targetSeat, ISkillProgramEffectHost host)
    {
        host.DrawAllHandSelectedBonus(frame, effect.Amount);
        return SkillProgramStepOutcome.Continue;
    }
}

public sealed partial class GameEngine
{
}
