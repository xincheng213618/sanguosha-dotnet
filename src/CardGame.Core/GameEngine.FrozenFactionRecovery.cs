namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class FrozenFactionRecoveryProgramCapability(GameEngine engine) : IFrozenFactionRecoveryProgramHost
    {
        public void FreezeLivingFactionRecovery(ProgramSkillFrame f) => engine.FreezeProgramLivingFactionRecovery(f);
        public void DrawToFrozenFactionCount(ProgramSkillFrame f) => engine.DrawProgramToFrozenFactionCount(f);
    }
    private void FreezeProgramLivingFactionRecovery(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.FrozenFactionRecovery is not null || f.InstructionIndex != 1 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context ||
            ActiveDying is not { } dying || context.ParentFrameId != dying.Id || context.TargetSeat != f.OwnerSeat ||
            dying.VictimSeat != f.OwnerSeat || dying.ResponderSeat != f.OwnerSeat || _players[f.OwnerSeat].Hp > 0)
            throw new InvalidOperationException("Frozen faction recovery lost its exact fresh self-dying producer.");
        var x = GetLivingFactionCount();
        var points = CompleteProgramEventHistory().OfType<DamageAppliedEvent>()
            .Where(e => !e.SourceLess && e.SourceSeat == f.OwnerSeat && e.Amount > 0).Sum(e => e.Amount);
        CaptureProgramExpressionNumber(f.Id, FrozenFactionRecoveryComposition.FactionNumber, x);
        CaptureProgramExpressionNumber(f.Id, FrozenFactionRecoveryComposition.DamageNumber, points);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { FrozenFactionRecovery = new(f.InstructionIndex, dying.Id) });
        AdvanceEventRulesAndQueueFact(new FrozenFactionRecoveryCapturedEvent(f.Id,
            new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), dying.Id, x, points));
        RecoverProgramTargetTo(f.Id, f.OwnerSeat, f.OwnerSeat, SkillProgramNumberExpression.IntegerConstant, x, true);
    }
    private void DrawProgramToFrozenFactionCount(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        var receipt = f.FrozenFactionRecovery ?? throw new InvalidOperationException("Frozen faction draw lost its completed recovery producer.");
        var factions = ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.FactionNumber) ??
            throw new InvalidOperationException("The frozen faction hand target lost its issued quantity.");
        if (f.InstructionIndex != receipt.InstructionIndex + 1 || receipt.DrawIssued)
            throw new InvalidOperationException("The frozen faction hand target can be issued only once after recovery children.");
        ReplaceRuntimeTop(f with { FrozenFactionRecovery = receipt with { DrawIssued = true } });
        var count = _players[f.OwnerSeat].IsAlive && _winner == Winner.None ? Math.Max(0, factions - GetHand(_players[f.OwnerSeat]).Count) : 0;
        AdvanceEventRulesAndQueueFact(new FrozenFactionHandDrawIssuedEvent(f.Id, f.OwnerSeat, factions, count));
        if (count > 0) DrawProgramCards(f.Id, f.OwnerSeat, count, null, null,
            SkillProgramCardSetVisibility.Private, new("program.frozen-faction-recovery.draw"));
    }
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 250)]
    private void AssertFrozenFactionRecovery(ProgramSkillFrame f)
    {
        if (f.FrozenFactionRecovery is not { } r) return;
        var factions = ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.FactionNumber);
        var damage = ReadProgramExpressionNumber(f, FrozenFactionRecoveryComposition.DamageNumber);
        if (r.InstructionIndex != 1 || factions is not { } factionCount || factionCount < 1 || factionCount > _players.Count ||
            damage is not { } damagePoints || damagePoints < 0 ||
            f.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } context || context.ParentFrameId != r.DyingFrameId ||
            f.InstructionIndex is < 1 or > 3 || r.DrawIssued != (f.InstructionIndex >= 2) ||
            !CompleteProgramEventHistory().OfType<FrozenFactionRecoveryCapturedEvent>().Any(e => e.FrameId == f.Id &&
                e.Source == new CardConversionSource(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) &&
                e.DyingFrameId == r.DyingFrameId && e.FactionCount == factionCount && e.GameDamagePoints == damagePoints))
            throw new InvalidOperationException("A frozen faction recovery lost its exact immutable amount/history receipt.");
    }
}
