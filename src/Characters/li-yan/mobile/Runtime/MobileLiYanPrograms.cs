namespace CardGame.Core;

public sealed record MobileLiYanNextDrawBonusIssuedEvent(long ProgramFrameId, int InstructionIndex,
    CardConversionSource Source, string GameplayHash, int TargetSeat, int Amount) : IGameEvent;
public sealed record MobileLiYanNextDrawBonusAssignedEvent(long ProgramFrameId, int InstructionIndex,
    ActualDiscardRecoveryPhaseKey Phase) : IGameEvent;
internal static class MobileLiYanComposition
{
    [ProgramCatalogValidation(77900)]
    internal static void Validate(string path, SkillProgram p)
    {
        var op = SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus;
        if (!p.Activations.SelectMany(a => a.Effects).Concat(p.Triggers.SelectMany(t => t.Effects)).Any(e => e.Op == op)) return;
        if (p.Id != "mobile:duliang" || p.Triggers.Count != 0 ||
            p.Activations is not [{ Id: "take-hand-and-benefit", MinCards: 0, MaxCards: 0, MinTargets: 1, MaxTargets: 1,
                TargetKind: SkillProgramTargetKind.OtherLivingWithHand, UsesPerTurn: null, UsesPerPhase: 1 } a] ||
            a.Effects.Count(e => e.Op == op) != 1)
            throw new InvalidOperationException(path + ": mobile Duliang issues its future Draw bonus only from one zero-card, one-other-hand-target, once-per-Play activation.");
    }
}
internal interface IMobileLiYanNextDrawHost { void Issue(ProgramSkillFrame frame, int targetSeat, int amount); }
internal sealed class MobileLiYanNextDrawDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus;
    public override ISkillProgramEffectHandler Handler { get; } = new MobileLiYanNextDrawHandler();
    public override ProgramFactRequirements FactRequirements => new(Activated: ProgramTrackedFact.ActualDiscardRecoveryPhase);
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.GrantTurnRuleModifier, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "amount", "condition");
        if (r.RequiredEnum<SkillProgramEffectTarget>("target") != SkillProgramEffectTarget.SelectedTarget || r.RequiredInt("amount") != 1)
            throw new InvalidOperationException("Mobile Duliang requires one future Draw bonus for the selected other recipient.");
        return new(Op, SkillProgramEffectTarget.SelectedTarget, 1, r.Condition());
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect e) => [new ReadSelectedTarget()];
}
internal sealed class MobileLiYanNextDrawHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus;
    public SkillProgramStepOutcome Execute(SkillProgramEffect e, ProgramSkillFrame f, int seat, ISkillProgramEffectHost h)
    { h.GetCapability<IMobileLiYanNextDrawHost>().Issue(f, seat, e.Amount); return SkillProgramStepOutcome.Continue; }
}
