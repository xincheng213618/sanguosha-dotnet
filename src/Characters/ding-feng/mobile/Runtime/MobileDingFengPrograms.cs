namespace CardGame.Core;

public sealed record MobileFenxunDistanceGrantedEvent(long ProgramFrameId, CardConversionSource Source,
    string GameplayHash, int ActualTurnNumber, int ActualTurnOwnerSeat, int TargetSeat, int PaidCardId) : IGameEvent;
internal interface IMobileFenxunHost { SkillProgramStepOutcome Grant(ProgramSkillFrame frame); }
internal sealed class MobileFenxunDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.GrantMobileFenxunDistance;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileFenxunHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTarget)
            throw new InvalidOperationException("Mobile Fenxun requires its selected other target.");
        var effect = new SkillProgramEffect(Op, SkillProgramEffectTarget.SelectedTarget, 0, r.Condition());
        RequireAlways(effect, r.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget()];
}
internal sealed class MobileFenxunHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.GrantMobileFenxunDistance;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h) => h.GetCapability<IMobileFenxunHost>().Grant(f);
}
internal static class MobileFenxunComposition
{
    [ProgramCatalogValidation(75600)]
    private static void Validate(string path, SkillProgram program)
    {
        foreach (var a in program.Activations.Where(a => a.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantMobileFenxunDistance)))
            if (a.Effects is not [{ Op: SkillProgramEffectOp.DiscardSelected, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always },
                                 { Op: SkillProgramEffectOp.GrantMobileFenxunDistance, Target: SkillProgramEffectTarget.SelectedTarget, Condition.Kind: SkillProgramConditionKind.Always }] ||
                a.MinCards != 1 || a.MaxCards != 1 || a.MinTargets != 1 || a.MaxTargets != 1 ||
                a.TargetKind != SkillProgramTargetKind.OtherLiving || a.UsesPerPhase != 1 || a.UsesPerTurn is not null || a.UsesPerGame is not null ||
                !a.SourceZones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment]) || a.Condition.Kind != SkillProgramConditionKind.Always || a.MarkerCost is not null || a.ContinueAfterOwnerDeath)
                throw new InvalidOperationException(path + ": mobile Fenxun requires one completed HE discard and one other target, once per Play phase.");
        if (program.Triggers.Any(t => t.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantMobileFenxunDistance)))
            throw new InvalidOperationException(path + ": mobile Fenxun requires an actual Play activation.");
    }
}
