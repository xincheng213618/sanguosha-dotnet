namespace CardGame.Core;

internal static class BoundaryGaoShunComposition
{
    internal const string Xianzhen = "boundary:xianzhen-mobile",Jinjiu = "boundary:jinjiu-mobile";
    [ProgramCatalogValidation(57300)]
    internal static void Validate(string path, SkillProgram p)
    {
        var effects = p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).ToArray();
        if (!effects.Any(e => e.Op is SkillProgramEffectOp.IssueBoundaryGaoShunXianzhen or SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash)) return;
        var valid = p.Id switch
        {
            Xianzhen => p.Activations is [{ Id: "contest", MinCards: 1, MaxCards: 1, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLivingWithHand, UsesPerTurn: null, UsesPerPhase: 1,
                Effects: [{ Op: SkillProgramEffectOp.Pindian, Target: SkillProgramEffectTarget.SelectedTarget, Amount: 1 },
                    { Op: SkillProgramEffectOp.IssueBoundaryGaoShunXianzhen, Target: SkillProgramEffectTarget.Owner }] }] && p.Triggers.Count == 0 &&
                p.Activations[0].SourceZones.SequenceEqual([CardZoneKind.Hand]) && p.CardIdentities.Count == 0 && p.CardPolicies.Count == 0,
            Jinjiu => p.Activations.Count == 0 && p.Triggers is [{ Id: "remove-real-alcohol-boost", Window: SkillProgramTriggerWindow.BeforeDamageApplied,
                Subject: SkillProgramTriggerSubject.DamageTarget, Optional: false, Effects: [{ Op: SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash, Target: SkillProgramEffectTarget.Owner }] }] &&
                p.CardIdentities is [{ OutputKind: CardKind.Slash }] && p.CardIdentities[0].InputKinds.SequenceEqual([CardKind.Alcohol]) &&
                p.CardIdentities[0].Zones.SequenceEqual([CardZoneKind.Hand]) && p.CardPolicies is [{ Kind: SkillProgramCardPolicyKind.ForeignTurnAlcoholUseProhibition }],
            _ => false
        };
        if (!valid || effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always) || p.Modifiers.Count != 0 || p.ViewAs.Count != 0 || p.DamageModifiers.Count != 0)
            throw new InvalidOperationException(path + ": mobile Gao Shun needs his phase-scoped native Pindian and exact consumed-Alcohol damage reduction, without OL target additions or K identity.");
    }
}
internal interface IBoundaryGaoShunHost { void Execute(ProgramSkillFrame frame, SkillProgramEffectOp op); }
internal sealed class IssueBoundaryGaoShunXianzhenDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueBoundaryGaoShunXianzhen;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryGaoShunHandler(SkillProgramEffectOp.IssueBoundaryGaoShunXianzhen);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(3));
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadPindianResult("__active-pindian-result")];
}
internal sealed class ReduceBoundaryGaoShunAlcoholSlashDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryGaoShunHandler(SkillProgramEffectOp.ReduceBoundaryGaoShunAlcoholSlash);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.PreventCurrentDamage, static (_, c) => c.PublicControlValue(2));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new RequireTriggerWindow(SkillProgramTriggerWindow.BeforeDamageApplied)];
}
internal sealed class BoundaryGaoShunHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host)
    { host.GetCapability<IBoundaryGaoShunHost>().Execute(frame, op); return SkillProgramStepOutcome.Continue; }
}

public sealed record BoundaryGaoShunPindianCapturedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    long PindianFrameId, int TargetSeat, int SourceCardId, CardKind SourceCardKind, int SourceRank, int OpponentRank,
    ActualDiscardRecoveryPhaseKey Phase, long DebitPhase) : IGameEvent;
public sealed record BoundaryGaoShunXianzhenIssuedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    long PindianFrameId, int TargetSeat, bool Won, bool SlashHandLimitExempt,
    ActualDiscardRecoveryPhaseKey Phase, long DebitPhase) : IGameEvent;
