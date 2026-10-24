namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool HasBoundaryCaoZhi => _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SelectBoundaryCaoZhiTrick);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 56700)]
    private void CaptureBoundaryCaoZhiDamageFace(DamageAppliedEvent applied)
    {
        if (!HasBoundaryCaoZhi || applied.Amount <= 0 || CurrentDamageAttempt is not { } attack ||
            _resolutionStack.LastOrDefault() is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId ||
            damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount) return;
        AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiDamageFaceEvent(damage.Id, attack.ResolutionId, applied.TargetSeat, _players[applied.TargetSeat].IsFaceDown));
    }
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 56700, SkillProgramEffectOp.TurnOver)]
    private bool QualifyBoundaryCaoZhiDamage(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId is not (BoundaryCaoZhiComposition.Jiushi or BoundaryCaoZhiComposition.Upgraded) || c.BindingId != BoundaryCaoZhiComposition.Damage) return true;
        return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && _players[c.OwnerSeat].IsFaceDown &&
            ProgramEventHistory<BoundaryCaoZhiDamageFaceEvent>().Any(e => e.DamageFrameId == context.DamageFrameId && e.OwnerSeat == c.OwnerSeat && e.WasFaceDown);
    }
    private int BoundaryCaoZhiGameDamage(int owner) => ProgramEventHistory<DamageAppliedEvent>()
        .Sum(e => (e.TargetSeat == owner ? e.Amount : 0) + (!e.SourceLess && e.SourceSeat == owner ? e.Amount : 0));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56701, SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi)]
    private bool QualifyBoundaryCaoZhiChengzhang(ProgramTriggerCandidate c, ProgramSkillWindowContext context) =>
        c.SkillId == BoundaryCaoZhiComposition.Chengzhang && context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow &&
        context.OwnerSeat == c.OwnerSeat && _currentSeat == c.OwnerSeat && _players[c.OwnerSeat].IsAlive && BoundaryCaoZhiGameDamage(c.OwnerSeat) >= 7;

    [ProgramCapability]
    private sealed class BoundaryCaoZhiCapability(GameEngine engine) : IBoundaryCaoZhiHost
    {
        public void SelectTrick(ProgramSkillFrame frame, string bind) => engine.SelectBoundaryCaoZhiTrick(frame, bind);
        public void Upgrade(ProgramSkillFrame frame) => engine.UpgradeBoundaryCaoZhiJiushi(frame);
    }
    private void SelectBoundaryCaoZhiTrick(ProgramSkillFrame supplied, string bind)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        var pool = _cardZones.CardsAt(CardLocation.DrawPile).Where(c => GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Trick).ToArray();
        var before = _random.State;
        int? index = pool.Length == 0 ? null : _random.Next(pool.Length);
        var selected = index is { } at ? pool[at] : null;
        SetProgramCardSet(f.Id, bind, selected is null ? [] : [selected.Id], SkillProgramCardSetVisibility.Private,
            selected is null ? [] : [CardLocation.DrawPile]);
        AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiTrickSelectedEvent(f.Id,
            new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.InstructionIndex,
            pool.Length, index, selected?.Id, before, _random.State));
    }
    private void UpgradeBoundaryCaoZhiJiushi(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != BoundaryCaoZhiComposition.Chengzhang || f.WindowContext?.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow ||
            f.InstructionIndex != 3 || BoundaryCaoZhiGameDamage(f.OwnerSeat) < 7)
            throw new InvalidOperationException("Chengzhang requires its original once-game preparation frame after native recovery and draw.");
        ReplaceProgramSkills(f, [BoundaryCaoZhiComposition.Jiushi], [BoundaryCaoZhiComposition.Upgraded],
            ProgramSkillSourceRemoval.Enabled, acquireFirst: false, announceAwakening: true);
        AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiJiushiUpgradedEvent(f.Id,
            new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), BoundaryCaoZhiGameDamage(f.OwnerSeat)));
    }
}
