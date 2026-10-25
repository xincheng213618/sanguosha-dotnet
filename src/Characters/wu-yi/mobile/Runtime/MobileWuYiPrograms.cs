namespace CardGame.Core;

internal interface IMobileWuYiBenxiHost { SkillProgramStepOutcome Apply(ProgramSkillFrame frame); }
internal sealed class MobileWuYiBenxiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyMobileWuYiBenxi;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileWuYiBenxiHandler();
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(3));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "amount", "condition");
        if (reader.RequiredInt("amount") != 2) throw new InvalidOperationException("Mobile Benxi requires its fixed armor benefit and at most one extra Slash target.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 2, reader.Condition());
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseCommitted), new RequireCardActionActor()];
}
internal sealed class MobileWuYiBenxiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ApplyMobileWuYiBenxi;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileWuYiBenxiHost>().Apply(frame);
}
internal static class MobileWuYiBenxiComposition
{
    [ProgramCatalogValidation(74110)]
    private static void Validate(string path, SkillProgram program)
    {
        foreach (var trigger in program.Triggers.Where(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.ApplyMobileWuYiBenxi)))
            if (program.Id != "mobile:benxi" || trigger.Window != SkillProgramTriggerWindow.CardUseCommitted ||
                trigger.OwnerRelation != SkillProgramCardActionOwnerRelation.Actor || trigger.Optional ||
                trigger.Condition.Kind != SkillProgramTriggerConditionKind.CardActionActorIsCurrentTurn ||
                trigger.Effects is not [{ Op: SkillProgramEffectOp.ApplyMobileWuYiBenxi, Target: SkillProgramEffectTarget.Owner, Amount: 2, Condition.Kind: SkillProgramConditionKind.Always }])
                throw new InvalidOperationException(path + ": mobile Benxi fixed benefits require the owner's real in-turn committed use.");
        if (program.Activations.Any(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.ApplyMobileWuYiBenxi)))
            throw new InvalidOperationException(path + ": mobile Benxi cannot issue a benefit outside a real card use.");
    }
}
