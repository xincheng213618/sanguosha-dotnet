namespace CardGame.Core;

// Keep the published operation name and loader contract for existing content and replays.
internal sealed class ClaimBoundaryCaoCaoJianxiongDamageCardsDescriptor : ProgramOperationDescriptorBase, ISkillProgramEffectHandler
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards;
    public override ISkillProgramEffectHandler Handler => this;
    public override ProgramContextCapability RequiredCapabilities => ProgramContextCapability.Damage;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ClaimDamageCards, static (e, c) => c.ClaimDamageCards(e));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied)];

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IProgramDamageCardClaims>().Claim(frame.Id, frame.OwnerSeat,
            new($"skill-program.{frame.SkillId}.ClaimDamageCards"), retainInvocationOnDelivery: true);
        return host.GetCapability<IBoundCardMovementContinuationHost>().AwaitBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    [ProgramCatalogValidation(58700)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards)) return;
        if (p.Id != "boundary:jianxiong-mobile" || p.Activations.Count != 0 || p.Triggers is not [{ Id: "claim-then-draw", Window: SkillProgramTriggerWindow.AfterDamageApplied,
            Subject: SkillProgramTriggerSubject.Owner, DamageOccurrence: SkillProgramDamageOccurrence.PerDamage, Optional: true, Effects: [
                { Op: SkillProgramEffectOp.ClaimBoundaryCaoCaoJianxiongDamageCards, Target: SkillProgramEffectTarget.Owner, Condition.Kind: SkillProgramConditionKind.Always },
                { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] }])
            throw new InvalidOperationException(path + ": mobile Jianxiong requires optional per-damage native claim, its movement children, then exactly one draw.");
    }
}
