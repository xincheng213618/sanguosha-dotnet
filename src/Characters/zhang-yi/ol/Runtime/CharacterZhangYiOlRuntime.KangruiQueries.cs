#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangYiOlRuntime
{
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 100)]
    internal int ZhangYiKangruiDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasZhangYiCapability || attack.IsSourceLess)
            return 0;
        var grants = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiArmedEvent>().Where(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.RecipientSeat == attack.SourceSeat && g.DamageBonus && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiDamageConsumedEvent>().Any(e => e.GrantFrameId == g.FrameId)).ToArray();
        foreach (var grant in grants)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiKangruiDamageConsumedEvent(grant.FrameId, attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, attack.IsChainPropagation, attack.IsChainPropagation ? ZhangYiChainCursor(attack) : 0));
        return grants.Length;
    }

    internal int ZhangYiChainCursor(GameEngine.IDamageAttempt attack) => attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetIndex;
    internal void SetZhangYiChainAmount(GameEngine.IDamageAttempt attack, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(attack, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !attack.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Kangrui can only restore its exact native chained damage instance.");
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 60)]
    internal void PrepareZhangYiChainAdvance(GameEngine.IDamageAttempt attack)
    {
        if (!HasZhangYiCapability || !attack.IsChainPropagation || attack.IsSourceLess)
            return;
        var cursor = ZhangYiChainCursor(attack);
        var remaining = attack is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(attack.ResolutionId).ChainedTargetSeats : _coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId).ChainedTargetSeats;
        if (remaining is null || !remaining.Skip(cursor).Any(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive))
            return;
        var issued = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiChainInstanceIssuedEvent>().SingleOrDefault(e => e.AttackFrameId == attack.ResolutionId && e.ChainCursor == cursor);
        if (issued is null || !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiDamageConsumedEvent>().Any(e => e.AttackFrameId == attack.ResolutionId && e.ChainPropagation && e.ChainCursor == cursor))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiChainInstanceRebasedEvent>().Any(e => e.AttackFrameId == attack.ResolutionId && e.ChainCursor == cursor))
            return;
        SetZhangYiChainAmount(attack, issued.InheritedAmount);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiChainInstanceRebasedEvent(attack.ResolutionId, cursor, attack.TargetSeat, issued.InheritedAmount));
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 10)]
    internal void ApplyZhangYiChainedDamageBonus(GameEngine.IDamageAttempt attack)
    {
        if (!HasZhangYiCapability || !attack.IsChainPropagation || attack.IsSourceLess || !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiArmedEvent>().Any(g => g.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && g.RecipientSeat == attack.SourceSeat && g.DamageBonus && !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiDamageConsumedEvent>().Any(e => e.GrantFrameId == g.FrameId)))
            return;
        var cursor = ZhangYiChainCursor(attack);
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiChainInstanceIssuedEvent>().Any(e => e.AttackFrameId == attack.ResolutionId && e.ChainCursor == cursor))
            throw new InvalidOperationException("A chained Kangrui instance cannot issue its increment twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangYiChainInstanceIssuedEvent(attack.ResolutionId, cursor, attack.TargetSeat, attack.DamageAmount));
        // Formal armor recomputes this recipient in the ordinary finalizer.
        // Legacy already-finalized propagation gets only the newly armed bonus.
        if (attack.DamageAmountFinalized)
            SetZhangYiChainAmount(attack, checked(attack.DamageAmount + ZhangYiKangruiDamageBonus(attack)));
    }

    internal RuleQueryEvaluation ApplyZhangYiKangruiHandLimit(CharacterState player, RuleQueryEvaluation ordinary)
    {
        if (!HasZhangYiCapability || !_coreProgramEventHistoryOperations.ProgramEventHistory<ZhangYiKangruiHandLimitSetEvent>().Any(e => e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.RecipientSeat == player.Seat))
            return ordinary;
        var set = new FiniteRuleQueryContribution($"turn:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:zhang-yi:kangrui:{(player.Seat)}", SkillRuleOperation.Set, 0, 200);
        var frozen = Array.AsReadOnly(ordinary.Value.Contributions.Append(set).OrderBy(c => c.SourceId, StringComparer.Ordinal).ToArray());
        var stage = new RuleQueryStageSnapshot("kangrui-hand-limit-zero", ordinary.Value is FiniteRuleQueryValue finite ? finite.Value : 0, false, 0, Array.AsReadOnly<RuleQueryContribution>([set]));
        return new(new FiniteRuleQueryValue(0, frozen), ordinary.BaseTerms, Array.AsReadOnly(ordinary.Stages.Append(stage).ToArray()));
    }
}
