namespace CardGame.Core;

internal interface IBoundaryCaoCaoHost
{ SkillProgramStepOutcome ClaimDamageCards(ProgramSkillFrame frame); }

internal sealed class ClaimBoundaryCaoCaoJianxiongDamageCardsDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryCaoCaoClaimHandler();
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ClaimDamageCards, static (e, c) => c.ClaimDamageCards(e));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];
}
internal sealed class BoundaryCaoCaoClaimHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int target, ISkillProgramEffectHost host) =>
        host.GetCapability<IBoundaryCaoCaoHost>().ClaimDamageCards(f);
}
internal static class BoundaryCaoCaoMobile
{
    internal const string Jianxiong = "boundary:jianxiong-mobile";
    internal const string ClaimReason = "skill-program.boundary:jianxiong-mobile.ClaimDamageCards";
    [ProgramCatalogValidation(58700)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards)) return;
        if (p.Id != Jianxiong || p.Activations.Count != 0 || p.Triggers is not [{ Id: "claim-then-draw", Window: SkillProgramTriggerWindow.AfterDamageApplied,
            Subject: SkillProgramTriggerSubject.Owner, DamageOccurrence: SkillProgramDamageOccurrence.PerDamage, Optional: true, Effects: [
                { Op: SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Jianxiong requires optional per-damage native claim, its movement children, then exactly one draw.");
    }
}
