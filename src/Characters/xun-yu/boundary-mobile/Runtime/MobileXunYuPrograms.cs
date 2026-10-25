namespace CardGame.Core;

internal interface IMobileXunYuProgramHost { SkillProgramStepOutcome DrawIfRecipientShort(ProgramSkillFrame frame); }
internal sealed class DrawMobileJiemingOwnerIfRecipientShortDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileXunYuJiemingHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new RequireTriggerWindow(SkillProgramTriggerWindow.AfterDamageApplied), new ReadSelectedTarget()];
}
internal sealed class MobileXunYuJiemingHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileXunYuProgramHost>().DrawIfRecipientShort(frame);
}
internal static class MobileXunYuComposition
{
    [ProgramCatalogValidation(65209)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort)) return;
        if (p.Id != "boundary:jieming-mobile" || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 ||
            p.Triggers is not [{ Id: "per-damage-point", Window: SkillProgramTriggerWindow.AfterDamageApplied, Subject: SkillProgramTriggerSubject.Owner,
                DamageOccurrence: SkillProgramDamageOccurrence.PerDamagePoint, Optional: true,
                Effects: [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.AnyLiving },
                    { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 2 },
                    { Op: SkillProgramEffectOp.DrawMobileJiemingOwnerIfRecipientShort, Target: SkillProgramEffectTarget.Owner }] }])
            throw new InvalidOperationException(path + ": mobile Jieming requires each applied damage point, one living recipient's actual two-card draw and the owner's conditional one-card draw.");
    }
}
