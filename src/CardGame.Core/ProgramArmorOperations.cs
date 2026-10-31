namespace CardGame.Core;

public interface IArmorProgramHost
{
    void GrantArmor(ProgramSkillFrame frame);
}

internal sealed class GrantArmorDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantArmor;
    public override ISkillProgramEffectHandler Handler { get; } = new GrantArmorHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantArmor,
        static (effect, context) => context.GrantArmor(effect));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "amount", "condition");
        var target = reader.RequiredEnum<SkillProgramEffectTarget>("target");
        var amount = reader.RequiredInt("amount");
        if (target is not (SkillProgramEffectTarget.Owner or SkillProgramEffectTarget.SelectedTarget or
                SkillProgramEffectTarget.SelectedTargets) || amount is < 1 or > 20)
            throw new InvalidOperationException("Armor grants require an owner or selected recipient and amount 1..20.");
        var effect = new SkillProgramEffect(Op, target, amount, reader.Condition());
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        effect.Target == SkillProgramEffectTarget.SelectedTargets ? [new ReadTargetSet(0)] : WithSelectedTarget(effect);
}

public sealed class GrantArmorHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantArmor;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<IArmorProgramHost>().GrantArmor(frame);
        return SkillProgramStepOutcome.Continue;
    }
}
