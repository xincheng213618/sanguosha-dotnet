namespace CardGame.Core;

internal interface IShanheSourceNamanHost
{
    SkillProgramStepOutcome Claim(ProgramSkillFrame frame);
}

internal sealed class ClaimShanheSourceNamanResponseCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimShanheSourceNamanResponseCards;
    public override ISkillProgramEffectHandler Handler { get; } = new ClaimShanheSourceNamanResponseCardsHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.CardAction;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards,
        static (_, context) => context.Draw(new(SkillProgramEffectOp.Draw, SkillProgramEffectTarget.Owner, 1,
            new(SkillProgramConditionKind.Always, 0, []))));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.CardResponseCompleted),
         new RequireCardActionRelation(SkillProgramCardActionOwnerRelation.Observer,
             [CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash, CardKind.ColdSlash])];
}

internal sealed class ClaimShanheSourceNamanResponseCardsHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimShanheSourceNamanResponseCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat,
        ISkillProgramEffectHost host) => host.GetCapability<IShanheSourceNamanHost>().Claim(frame);
}
