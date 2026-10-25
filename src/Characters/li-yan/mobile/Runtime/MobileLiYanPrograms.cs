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
public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class MobileLiYanNextDrawHost(GameEngine engine) : IMobileLiYanNextDrawHost
    { public void Issue(ProgramSkillFrame frame, int targetSeat, int amount) => engine.IssueMobileLiYanNextDrawBonus(frame, targetSeat, amount); }
    private void IssueMobileLiYanNextDrawBonus(ProgramSkillFrame supplied, int targetSeat, int amount)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.TriggerId is not null || f.SelectedTargetSeats is not [var selected] || selected != targetSeat ||
            f.OwnerSeat == targetSeat || !IsValidPlayerSeat(targetSeat) || amount != 1 || f.InstructionIndex < 1 ||
            ProgramInstructionResolver.Default.Resolve(f, _contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op != SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus ||
            ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("Mobile Duliang lost its original one-time selected-target future Draw instruction.");
        if (_winner != Winner.None || !_players[targetSeat].IsAlive) return;
        AdvanceEventRulesAndQueueFact(new MobileLiYanNextDrawBonusIssuedEvent(f.Id, f.InstructionIndex,
            new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, targetSeat, amount));
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 77900)]
    private void AssignMobileLiYanNextDrawBonus(ActualDiscardRecoveryPhaseStartedEvent e)
    {
        if (e.Phase.Kind != ActualDiscardRecoveryPhaseKind.Draw || !_players[e.Phase.ActorSeat].IsAlive ||
            !_contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus)) return;
        var assigned = ProgramEventHistory<MobileLiYanNextDrawBonusAssignedEvent>().Select(x => (x.ProgramFrameId, x.InstructionIndex)).ToHashSet();
        foreach (var grant in ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Where(x => x.TargetSeat == e.Phase.ActorSeat && !assigned.Contains((x.ProgramFrameId, x.InstructionIndex))).ToArray())
            AdvanceEventRulesAndQueueFact(new MobileLiYanNextDrawBonusAssignedEvent(grant.ProgramFrameId, grant.InstructionIndex, e.Phase));
    }
    private int MobileLiYanNextDrawBonus(CharacterState player)
    {
        if (!_contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.IssueMobileLiYanNextDrawBonus) ||
            CurrentActualDiscardRecoveryPhase() is not { Kind: ActualDiscardRecoveryPhaseKind.Draw } phase || phase.ActorSeat != player.Seat) return 0;
        var assigned = ProgramEventHistory<MobileLiYanNextDrawBonusAssignedEvent>().Where(x => x.Phase == phase)
            .Select(x => (x.ProgramFrameId, x.InstructionIndex)).ToHashSet();
        return ProgramEventHistory<MobileLiYanNextDrawBonusIssuedEvent>().Where(x => x.TargetSeat == player.Seat && assigned.Contains((x.ProgramFrameId, x.InstructionIndex))).Sum(x => x.Amount);
    }
}
