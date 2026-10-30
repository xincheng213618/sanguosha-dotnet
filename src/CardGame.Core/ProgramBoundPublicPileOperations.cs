namespace CardGame.Core;

internal interface IBoundPublicPileProgramHost
{
    SkillProgramStepOutcome SelectPublicPile(SkillProgramEffect effect, ProgramSkillFrame frame);
    SkillProgramStepOutcome DistributeBoundCards(SkillProgramEffect effect, ProgramSkillFrame frame);
}

internal sealed class SelectPublicPileDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.SelectPublicPile;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new SelectPublicPileHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "amount", "skillIds", "resultBind", "condition");
        var owner = FilterBoundCardsProgramOperationDescriptor.Owner(r);
        var capacity = r.RequiredInt("amount");
        var sources = r.RequiredIdentifierArray("skillIds");
        if (capacity is < 1 or > 16 || sources.Count != 1)
            throw new InvalidOperationException($"Public pile selection requires a bounded capacity and one source skill at {r.Path}.");
        var effect = new SkillProgramEffect(Op, owner, capacity, r.Condition(), skillIds: sources,
            resultBind: r.RequiredIdentifier("resultBind"), visibility: SkillProgramCardSetVisibility.Public);
        RequireAlways(effect, r.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new CreateCardSet(effect.ResultBind!, effect.Amount, false, CardOwner: SkillProgramEffectTarget.Owner)];
}

internal sealed class DistributeBoundCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeBoundCards;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ISkillProgramEffectHandler Handler { get; } = new DistributeBoundCardsHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "sourceBind", "targetKind", "condition");
        var owner = FilterBoundCardsProgramOperationDescriptor.Owner(r);
        var kind = r.RequiredEnum<SkillProgramTargetKind>("targetKind");
        if (kind != SkillProgramTargetKind.OtherLiving)
            throw new InvalidOperationException($"Public pile distribution requires other living recipients at {r.Path}.");
        return new(Op, owner, 0, r.Condition(), sourceBind: r.RequiredIdentifier("sourceBind"), targetKind: kind);
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.SelectedTargetHand)];

    [ProgramCatalogValidation(900002)]
    private static void ValidateSources(string id, SkillProgram program)
    {
        foreach (var effects in program.Activations.Select(activation => activation.Effects)
            .Concat(program.Triggers.Select(trigger => trigger.Effects)))
        for (var index = 0; index < effects.Count; index++)
        {
            if (effects[index].Op != SkillProgramEffectOp.DistributeBoundCards) continue;
            var producer = effects.Take(index).SingleOrDefault(effect => effect.ResultBind == effects[index].SourceBind);
            if (producer?.Op != SkillProgramEffectOp.SelectPublicPile)
                throw new InvalidOperationException($"Public pile distribution in '{id}' requires an original selected public-pile binding.");
        }
    }
}

internal sealed class SelectPublicPileHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.SelectPublicPile;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundPublicPileProgramHost>().SelectPublicPile(effect, frame);
}

internal sealed class DistributeBoundCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DistributeBoundCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int seat, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundPublicPileProgramHost>().DistributeBoundCards(effect, frame);
}
