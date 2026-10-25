namespace CardGame.Core;

internal interface IMobileDianWeiProgramHost { void ConsumeTarget(ProgramSkillFrame frame); }
internal static class MobileDianWeiQiangxi { internal const string Skill = "boundary:qiangxi-mobile", Ledger = "mobile-qiangxi"; }
internal sealed class ConsumeMobileQiangxiTargetDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeMobileQiangxiTarget;
    public override ISkillProgramEffectHandler Handler { get; } = new ConsumeMobileQiangxiTargetHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.CaptureSelectedCards, static (_, _) => { });
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new ReadSelectedTarget()];
}
internal sealed class ConsumeMobileQiangxiTargetHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.ConsumeMobileQiangxiTarget;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IMobileDianWeiProgramHost>().ConsumeTarget(frame); return SkillProgramStepOutcome.Continue; }
}
[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 65301, SkillProgramEffectOp.ConsumeMobileQiangxiTarget)]
internal sealed class MobileDianWeiQiangxiTargetFilter : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        var usage = queries.Get<ISelectionUsageQueries>();
        candidates.Targets = candidates.Targets.Where(seat => usage.CanUsePhaseTarget(context.OwnerSeat, context.SkillId, MobileDianWeiQiangxi.Ledger, seat)).ToArray();
        return true;
    }
}
internal static class MobileDianWeiComposition
{
    [ProgramCatalogValidation(65309)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == SkillProgramEffectOp.ConsumeMobileQiangxiTarget)) return;
        var valid = p.Id == MobileDianWeiQiangxi.Skill && p.Triggers.Count == 0 && p.Activations.Count == 2 && p.ViewAs.Count == 0 && p.Modifiers.Count == 0;
        foreach (var a in p.Activations)
            valid &= a.MinTargets == 1 && a.MaxTargets == 1 && a.TargetKind == SkillProgramTargetKind.OtherLivingInAttackRange &&
                a.UsesPerTurn is null && a.UsesPerPhase is null && a.UsesPerGame is null && a.Effects[0].Op == SkillProgramEffectOp.ConsumeMobileQiangxiTarget &&
                (a.Id == "lose-hp-and-damage" && a.MinCards == 0 && a.MaxCards == 0 && a.Effects is [_,
                    { Op: SkillProgramEffectOp.LoseHp, Target: SkillProgramEffectTarget.Owner, Amount: 1 }, { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }] ||
                 a.Id == "discard-weapon-and-damage" && a.MinCards == 1 && a.MaxCards == 1 && a.EquipmentSlots.SequenceEqual(new[] { EquipmentSlot.Weapon }) &&
                    a.Effects is [_, { Op: SkillProgramEffectOp.CaptureSelectedCards, ResultBind: "weapon" },
                        { Op: SkillProgramEffectOp.MoveBoundCards, SourceBind: "weapon", Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true },
                        { Op: SkillProgramEffectOp.Damage, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 }]);
        if (!valid) throw new InvalidOperationException(path + ": mobile Qiangxi requires a shared per-target Play-phase ledger, actual HP loss or one weapon discard, and attack-range damage.");
    }
}
