namespace CardGame.Core;

internal interface IMobileZhuRongProgramHost { SkillProgramStepOutcome Exchange(ProgramSkillFrame frame); }
internal sealed class ExchangeMobileLierenPindianCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ExchangeMobileLierenPindianCards;
    public override ISkillProgramEffectHandler Handler { get; } = new ExchangeMobileLierenPindianCardsHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"));
        if (effect.SourceBind != "contest" || effect.Condition is not { Kind: SkillProgramConditionKind.PindianNotWon, SourceBind: "contest" })
            throw new InvalidOperationException(reader.Path + ": mobile Lieren exchanges only its original non-winning contest.");
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadPindianResult(effect.SourceBind!)];
}
internal sealed class ExchangeMobileLierenPindianCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ExchangeMobileLierenPindianCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) => host.GetCapability<IMobileZhuRongProgramHost>().Exchange(frame);
}
internal static class MobileZhuRongComposition
{
    [ProgramCatalogValidation(66209)]
    internal static void Validate(string path, SkillProgram program)
    {
        var uses = program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ExchangeMobileLierenPindianCards);
        if (!uses && program.Id != "boundary:lieren-mobile") return;
        if (program.Id != "boundary:lieren-mobile" || program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Modifiers.Count != 0 || program.CardPolicies.Count != 0 ||
            program.Triggers is not [{ Id: "contest-on-slash-target", Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor,
                Optional: true, Effects: [{ Op: SkillProgramEffectOp.StartCardActionPindian, ResultBind: "contest", Visibility: SkillProgramCardSetVisibility.Public },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef: { Kind: ProgramParticipantRef.ResultOpponent, ResultBind: "contest" },
                        Destination: SkillProgramCardDestination.OwnerHand, SkipIfNoCards: true, AwaitMovementTriggers: true, Condition: { Kind: SkillProgramConditionKind.PindianWon, SourceBind: "contest" } },
                    { Op: SkillProgramEffectOp.ExchangeMobileLierenPindianCards, SourceBind: "contest" }] } trigger] ||
            !trigger.CardKinds.Order().SequenceEqual(new[] { CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash }.Order()))
            throw new InvalidOperationException(path + ": mobile Lieren requires its real Slash target's public contest, winning card gain and non-winning original-card exchange.");
    }
}
