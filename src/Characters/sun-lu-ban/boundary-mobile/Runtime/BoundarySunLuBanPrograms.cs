namespace CardGame.Core;

internal interface IBoundarySunLuBanHost
{ SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect); }

internal sealed class ChooseBoundarySunLuBanZenhuiDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui;
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeActionAppearance);
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationInteraction Interaction => ProgramOperationInteraction.Choice;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChooseOption, static (_, context) => context.PublicControlValue(1));
    public override ISkillProgramEffectHandler Handler { get; } = new BoundarySunLuBanChoiceHandler();
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "resultBind", "condition");
        var effect = new SkillProgramEffect(Op, reader.RequiredEnum<SkillProgramEffectTarget>("target"), 0, reader.Condition(), resultBind: reader.RequiredIdentifier("resultBind"));
        if (effect.Target != SkillProgramEffectTarget.Owner) throw new InvalidOperationException(reader.Path + ": mobile Zenhui is chosen by Sun Lu Ban herself.");
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardUseTargetsFinalized), new ReadSelectedTarget(), new CreateChoiceResult(effect.ResultBind!, ["become-user", "extra-target"])];
}
internal sealed class BoundarySunLuBanChoiceHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host) => host.GetCapability<IBoundarySunLuBanHost>().Choose(frame, effect);
}

public sealed record BoundarySunLuBanChoiceEvent(long FrameId, int OwnerSeat, string SkillInstanceId, string GameplayHash,
    long UseFrameId, long ActionId, int SelectedSeat, string ResultBind, string OptionId) : IGameEvent;

internal static class BoundarySunLuBanComposition
{
    [ProgramCatalogValidation(54100)]
    private static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(trigger => trigger.Effects).Concat(program.Activations.SelectMany(activation => activation.Effects))
                .Any(effect => effect.Op == SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui)) return;
        if (program.Id != "boundary:zenhui-mobile" || program.Activations.Count != 0 || program.Triggers is not [
            { Id: "own-choice-role", Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, OwnerRelation: SkillProgramCardActionOwnerRelation.Actor,
                Optional: true, UsageScope: SkillUsageScope.Phase, UsageLimit: 1, Effects: [
                    { Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLegalCurrentCardTarget },
                    { Op: SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui, ResultBind: "own-role-choice" },
                    { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, AwaitMovementTriggers: true, Destination: SkillProgramCardDestination.OwnerHand,
                        ChooserRef.Kind: ProgramParticipantRef.Owner, CardOwnerRef.Kind: ProgramParticipantRef.SelectedTarget, Zones: [CardZoneKind.Hand, CardZoneKind.Equipment],
                        Condition.Kind: SkillProgramConditionKind.ChoiceIs, Condition.SourceBind: "own-role-choice", Condition.OptionId: "become-user" },
                    { Op: SkillProgramEffectOp.ReplaceCurrentCardUseActor, Condition.Kind: SkillProgramConditionKind.ChoiceIs, Condition.SourceBind: "own-role-choice", Condition.OptionId: "become-user" },
                    { Op: SkillProgramEffectOp.AddCurrentCardUseTarget, Condition.Kind: SkillProgramConditionKind.ChoiceIs, Condition.SourceBind: "own-role-choice", Condition.OptionId: "extra-target" }] }])
            throw new InvalidOperationException(path + ": mobile Zenhui requires its original sole-target phase-once use, owner's choice, awaited obtain and native role mutation.");
    }
}
