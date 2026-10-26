namespace CardGame.Core;

internal interface IShanheXiaoruiProgramHost
{
    SkillProgramStepOutcome GrantActualTurnSlashQuota(ProgramSkillFrame frame);
}

internal sealed class GrantShanheXiaoruiSlashQuotaDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantShanheXiaoruiSlashQuota;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier,
        static (_, context) => context.PublicControlValue(5));
    public override ISkillProgramEffectHandler Handler { get; } = new GrantShanheXiaoruiSlashQuotaHandler();
}

internal sealed class GrantShanheXiaoruiSlashQuotaHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantShanheXiaoruiSlashQuota;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IShanheXiaoruiProgramHost>().GrantActualTurnSlashQuota(frame);
}
