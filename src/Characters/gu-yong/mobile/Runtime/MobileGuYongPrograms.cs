namespace CardGame.Core;

internal interface IMobileGuYongHost
{ SkillProgramStepOutcome CompleteMixedHand(ProgramSkillFrame frame, string binding); }
internal sealed class CompleteMobileGuYongOnMixedHandDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.CompleteMobileGuYongOnMixedHand;
    public override ISkillProgramEffectHandler Handler { get; } = new CompleteMobileGuYongOnMixedHandHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0,
            reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadCardSet(effect.SourceBind!)];
}
internal sealed class CompleteMobileGuYongOnMixedHandHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.CompleteMobileGuYongOnMixedHand;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileGuYongHost>().CompleteMixedHand(frame, effect.SourceBind!);
}
internal static class MobileGuYongComposition
{
    [ProgramCatalogValidation(77300)]
    private static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects))
            .Any(e => e.Op == SkillProgramEffectOp.CompleteMobileGuYongOnMixedHand)) return;
        if (program.Id != "mobile:bingyi" || program.Activations.Count != 0 || program.Triggers is not
            [{ Id: "ending-reveal-and-share", Window: SkillProgramTriggerWindow.TurnEnding,
               Effects: [{ Op: SkillProgramEffectOp.SelectOwnedCards, ResultBind: "shown-hand" } selection,
                   { Op: SkillProgramEffectOp.RevealBoundCards, SourceBind: "shown-hand" },
                   { Op: SkillProgramEffectOp.CompleteMobileGuYongOnMixedHand, SourceBind: "shown-hand" },
                   { Op: SkillProgramEffectOp.SelectTargets, TargetKind: SkillProgramTargetKind.AnyLiving,
                     MinimumTargets: 0, MaximumTargets: 8, NumberExpression: SkillProgramNumberExpression.CurrentHandCount },
                   { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTargets, Amount: 1 }] }] ||
            selection.NumberExpression != SkillProgramNumberExpression.AllOwnedZoneCards || !selection.Zones.SequenceEqual([CardZoneKind.Hand]))
            throw new InvalidOperationException(path + ": Bingyi must reveal its whole hand, complete a mixed hand, then choose optional recipients and draw.");
    }
}
