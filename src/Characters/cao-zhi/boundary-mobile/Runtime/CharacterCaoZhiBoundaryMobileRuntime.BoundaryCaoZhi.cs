#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoZhiBoundaryMobileRuntime
{
    internal bool HasBoundaryCaoZhi => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.SelectBoundaryCaoZhiTrick);

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 56700)]
    internal void CaptureBoundaryCaoZhiDamageFace(DamageAppliedEvent applied)
    {
        if (!HasBoundaryCaoZhi || applied.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != applied.TargetSeat || damage.Amount != applied.Amount)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiDamageFaceEvent(damage.Id, attack.ResolutionId, applied.TargetSeat, _coreMatchParticipantsPlayersAccess._players[applied.TargetSeat].IsFaceDown));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 56700, SkillProgramEffectOp.TurnOver)]
    internal bool QualifyBoundaryCaoZhiDamage(ProgramTriggerCandidate c, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (c.SkillId is not (BoundaryCaoZhiComposition.Jiushi or BoundaryCaoZhiComposition.Upgraded) || c.BindingId != BoundaryCaoZhiComposition.Damage)
            return true;
        return context.Window == SkillProgramTriggerWindow.AfterDamageApplied && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsFaceDown && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<BoundaryCaoZhiDamageFaceEvent>().Any(e => e.DamageFrameId == context.DamageFrameId && e.OwnerSeat == c.OwnerSeat && e.WasFaceDown);
    }

    internal int BoundaryCaoZhiGameDamage(int owner) => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Sum(e => (e.TargetSeat == owner ? e.Amount : 0) + (!e.SourceLess && e.SourceSeat == owner ? e.Amount : 0));
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56701, SkillProgramEffectOp.UpgradeBoundaryCaoZhiJiushi)]
    internal bool QualifyBoundaryCaoZhiChengzhang(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => c.SkillId == BoundaryCaoZhiComposition.Chengzhang && context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && context.OwnerSeat == c.OwnerSeat && _coreActualTurnProgressCurrentSeatAccess._currentSeat == c.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && BoundaryCaoZhiGameDamage(c.OwnerSeat) >= 7;
    void IBoundaryCaoZhiHost.SelectTrick(ProgramSkillFrame frame, string bind) => SelectBoundaryCaoZhiTrick(frame, bind);
    void IBoundaryCaoZhiHost.Upgrade(ProgramSkillFrame frame) => UpgradeBoundaryCaoZhiJiushi(frame);
    internal void SelectBoundaryCaoZhiTrick(ProgramSkillFrame supplied, string bind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(c.Kind) == SkillProgramCardCategory.Trick).ToArray();
        var before = _randomOperations._random.State;
        int? index = pool.Length == 0 ? null : _randomOperations._random.Next(pool.Length);
        var selected = index is { } at ? pool[at] : null;
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, bind, selected is null ? [] : [selected.Id], SkillProgramCardSetVisibility.Private, selected is null ? [] : [CardLocation.DrawPile]);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiTrickSelectedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, f.InstructionIndex, pool.Length, index, selected?.Id, before, _randomOperations._random.State));
    }

    internal void UpgradeBoundaryCaoZhiJiushi(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        _coreProgramSkillChangesOperations.ReplaceProgramSkills(f, [BoundaryCaoZhiComposition.Jiushi], [BoundaryCaoZhiComposition.Upgraded], ProgramSkillSourceRemoval.Enabled, acquireFirst: false, announceAwakening: true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryCaoZhiJiushiUpgradedEvent(f.Id, new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId), BoundaryCaoZhiGameDamage(f.OwnerSeat)));
    }
}
