namespace CardGame.Core;

internal interface IMobileZhaoYunProgramHost
{
    SkillProgramStepOutcome GiveRevealedCard(ProgramSkillFrame frame, string sourceBind);
}

internal sealed class GiveMobileYajiaoRevealedCardDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GiveMobileYajiaoRevealedCard;
    public override ISkillProgramEffectHandler Handler { get; } = new GiveMobileYajiaoRevealedCardHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "condition");
        var target = FilterBoundCardsProgramOperationDescriptor.Owner(reader);
        var effect = new SkillProgramEffect(Op, target, 0, reader.Condition(), sourceBind: reader.RequiredIdentifier("sourceBind"));
        RequireAlways(effect, reader.Path);
        return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadSelectedTarget(), new MoveCardSet(effect.SourceBind!, null, SkillProgramCardDestination.SelectedTargetHand)];
}

internal sealed class GiveMobileYajiaoRevealedCardHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GiveMobileYajiaoRevealedCard;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileZhaoYunProgramHost>().GiveRevealedCard(frame, effect.SourceBind!);
}

internal static class MobileZhaoYunComposition
{
    [ProgramCatalogValidation(60509)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects))
            .Any(e => e.Op == SkillProgramEffectOp.GiveMobileYajiaoRevealedCard)) return;
        var valid = program.Id == "boundary:yajiao-mobile" && program.Triggers.Count == 2 && program.Activations.Count == 0 &&
            program.ViewAs.Count == 0 && program.Modifiers.Count == 0 && program.CardPolicies.Count == 0 &&
            program.Triggers.Select(t => t.Window).Order().SequenceEqual(new[] {
                SkillProgramTriggerWindow.CardUseCommitted, SkillProgramTriggerWindow.CardResponseAccepted }.Order());
        foreach (var trigger in program.Triggers)
            valid &= trigger.OwnerRelation == SkillProgramCardActionOwnerRelation.Actor && trigger.Optional &&
                trigger.Condition is { Kind: SkillProgramTriggerConditionKind.All, Children.Count: 2 } condition &&
                condition.Children[0] is { Kind: SkillProgramTriggerConditionKind.Not, Children.Count: 1 } foreign &&
                foreign.Children[0].Kind == SkillProgramTriggerConditionKind.OwnerIsTurnPlayer &&
                condition.Children[1].Kind == SkillProgramTriggerConditionKind.CardActionFromOwnerHand &&
                trigger.Effects is [
                    { Op: SkillProgramEffectOp.RevealTopCards, Amount: 1, ResultBind: "revealed", Visibility: SkillProgramCardSetVisibility.Public },
                    { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.AnyLiving },
                    { Op: SkillProgramEffectOp.GiveMobileYajiaoRevealedCard, SourceBind: "revealed" },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.DiscardPile,
                        ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.Owner,
                        AwaitMovementTriggers: true, SkipIfNoCards: true, Condition.Kind: SkillProgramConditionKind.Not } cost] &&
                cost.Zones.SequenceEqual(new[] { CardZoneKind.Hand, CardZoneKind.Equipment }) && cost.Condition.Children is [
                    { Kind: SkillProgramConditionKind.BoundCardCategoryMatchesCardAction, SourceBind: "revealed" }];
        if (!valid) throw new InvalidOperationException(path + ": mobile Yajiao requires one public top card after a foreign-turn hand action, a mandatory living recipient and the native own HE mismatch discard.");
    }
}
