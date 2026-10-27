namespace CardGame.Core;

internal interface IGouXiangArmorHost
{
    void ObtainAttackerHandCard(ProgramSkillFrame frame);
}

internal sealed class ResolveGouXiangResponseDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveGouXiangResponse;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardResponseCompleted),
         new RequireCardActionRelation(SkillProgramCardActionOwnerRelation.Actor, [CardKind.Dodge])];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(3));
    public override ISkillProgramEffectHandler Handler { get; } = new ResolveGouXiangResponseHandler();
}

internal sealed class ResolveGouXiangResponseHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveGouXiangResponse;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host)
    {
        host.GetCapability<IGouXiangArmorHost>().ObtainAttackerHandCard(frame);
        return SkillProgramStepOutcome.Continue;
    }
}

[ProgramPassiveFactRequirements("shanhe:gouxiang-armor-14088", ProgramTrackedFact.NativeActionAppearance)]
internal static class GouXiangArmorContract
{
    [ProgramCatalogValidation(62052)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.Any(trigger => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveGouXiangResponse)) &&
            !program.Activations.Any(activation => activation.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveGouXiangResponse))) return;
        if (program.Id != "shanhe:gouxiang-armor-14088" || program.Activations.Count != 0 || program.ViewAs.Count != 0 ||
            program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 ||
            program.Triggers is not [{ Id: "matching-dodge", Window: SkillProgramTriggerWindow.CardResponseCompleted,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Actor, Optional: false,
                Condition.Kind: SkillProgramTriggerConditionKind.Always, CardKinds: [CardKind.Dodge],
                Effects: [{ Op: SkillProgramEffectOp.ResolveGouXiangResponse, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": Gouxiang requires its one locked actual-Dodge-completed owner instruction.");
    }
}
