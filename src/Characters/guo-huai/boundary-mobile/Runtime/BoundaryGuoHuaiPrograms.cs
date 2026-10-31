namespace CardGame.Core;

internal static class BoundaryGuoHuaiComposition
{
    internal const string Skill = "boundary:jingce-mobile",Response = "record-native-response-materials",Ending = "actual-turn-discard-count";
    [ProgramCatalogValidation(56400)]
    internal static void Validate(string path, SkillProgram p)
    {
        if (!p.Triggers.SelectMany(t => t.Effects).Concat(p.Activations.SelectMany(a => a.Effects)).Any(e => e.Op == SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce)) return;
        if (p.Id != Skill || p.Triggers.Count != 2 || p.Activations.Count != 0 || p.ViewAs.Count != 0 || p.Modifiers.Count != 0 || p.CardPolicies.Count != 0 || p.CardIdentities.Count != 0 || p.DamageModifiers.Count != 0 || p.HpSkillGrants.Count != 0 ||
            p.Triggers.SingleOrDefault(t => t.Id == Response) is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, OwnerRelation: SkillProgramCardActionOwnerRelation.Observer, Optional: false,
                IsSkillInvocation: false, Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce, Target: SkillProgramEffectTarget.Owner }] } ||
            p.Triggers.SingleOrDefault(t => t.Id == Ending) is not { Window: SkillProgramTriggerWindow.TurnEnding, Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true,
                Effects: [{ Op: SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce, Target: SkillProgramEffectTarget.Owner }, { Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 2, NumberExpression: null }] } ||
            p.Triggers.Any(t => t.Condition.Kind != SkillProgramTriggerConditionKind.Always || t.Priority != 0 || t.UsageScope is not null || t.UsageLimit is not null || t.MarkerCost is not null || t.SingleActionInstance ||
                t.Effects.Any(e => e.Condition.Kind != SkillProgramConditionKind.Always)))
            throw new InvalidOperationException(path + ": mobile Jingce needs its native response-material observer and optional own actual Ending draw-two path.");
    }
}

internal interface IBoundaryGuoHuaiHost { void Prepare(ProgramSkillFrame frame); }
internal sealed class PrepareBoundaryGuoHuaiJingceDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce;
    public override ISkillProgramEffectHandler Handler { get; } = new BoundaryGuoHuaiHandler();
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r) => ParseOwnerAlways(r);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GainCards, static (_, _) => { });
    public override ProgramFactRequirements FactRequirements => new(Triggered: ProgramTrackedFact.NativeTrueUseProcessingEntry);
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) =>
        [new RequireTriggerWindows([SkillProgramTriggerWindow.CardResponseCompleted, SkillProgramTriggerWindow.TurnEnding])];
}
internal sealed class BoundaryGuoHuaiHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareBoundaryGuoHuaiJingce;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int target, ISkillProgramEffectHost host)
    { host.GetCapability<IBoundaryGuoHuaiHost>().Prepare(frame); return SkillProgramStepOutcome.Continue; }
}

public sealed record BoundaryGuoHuaiDiscardedEntityEvent(int TurnNumber, int ActualTurnOwnerSeat,
    long NativeParentFrameId, long ActionId, int CardId, int MovementSequence) : IGameEvent;
public sealed record BoundaryGuoHuaiEndingQualifiedEvent(long FrameId, CardConversionSource Source, string GameplayHash,
    long EndingFrameId, int TurnNumber, int ActualTurnOwnerSeat, int Count, int Hp, long MovementCutoff) : IGameEvent;
