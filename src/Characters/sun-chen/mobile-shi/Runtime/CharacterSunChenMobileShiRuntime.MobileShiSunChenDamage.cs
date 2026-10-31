#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunChenMobileShiRuntime
{
    internal ShiScDamageReceipt? ShiScDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame c => c.ShiSunChenDamage,
        ProgramSkillFrame p => p.ShiSunChenDamage,
        JudgmentFrame j => j.ShiSunChenDamage,
        _ => null
    };
    internal void SetShiScDamageState(long id, ShiScDamageReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(x => x.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { ShiSunChenDamage = r },
            ProgramSkillFrame p => p with { ShiSunChenDamage = r },
            JudgmentFrame j => j with { ShiSunChenDamage = r },
            _ => throw new InvalidOperationException("Nigu damage requires its original native attack frame.")});
    }

    internal int ShiScDamageCursor(GameEngine.IDamageAttempt a) => !a.IsChainPropagation ? -1 : a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    internal IEnumerable<ShiScArmedEvent> LiveShiScArms(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScArmedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.Count > 0 && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(ShiScNigu, out var skill) && skill.Program?.GameplayHash == e.Hash && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScStartedEvent>().Count(s => s.FrameId == e.FrameId && s.Source == e.Source && s.Hash == e.Hash && s.Op == SkillProgramEffectOp.ResolveMobileShiSunChenNigu && s.Turn == e.Turn && s.TurnOwner == e.TurnOwner) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScCompletedEvent>().Count(c => c.FrameId == e.FrameId) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(t => t.TurnNumber == e.Turn && t.ActorSeat == e.TurnOwner));
    internal ShiScArmedEvent? AvailableShiScArm(GameEngine.IDamageAttempt a) => a.IsSourceLess ? null : LiveShiScArms(a.SourceSeat).FirstOrDefault(e => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScDamageConsumedEvent>().Count(c => c.ArmId == e.FrameId) < e.Count);
    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 49100)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> ShiScNextDamage(GameEngine.IDamageAttempt a)
    {
        if (a.IsSourceLess || a.DamageAmount <= 0 || AvailableShiScArm(a)is not { } arm)
            return[];
        var cursor = ShiScDamageCursor(a);
        if (ShiScDamageState(a.ResolutionId)is not { } prior || prior.Source != a.SourceSeat || prior.Target != a.TargetSeat || prior.ChainCursor != cursor || prior.ArmId != arm.FrameId)
        {
            SetShiScDamageState(a.ResolutionId, new(a.ResolutionId, arm.FrameId, a.SourceSeat, a.TargetSeat, cursor, a.DamageAmount));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScDamageIssuedEvent(arm.FrameId, a.ResolutionId, a.SourceSeat, a.TargetSeat, cursor, a.DamageAmount));
        }

        return[(new(arm.Source.SkillId, arm.Source.BindingId, arm.Source.OwnerSeat, arm.Source.SkillInstanceId), 1)];
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 49100)]
    internal void ConsumeShiScDamage(IGameEvent payload)
    {
        if (payload is ExtraCardEffectStartedEvent extra && _coreCardUseLifecycleOperations.LifecycleCardUse(extra.CardUseFrameId)is { ShiSunChenDamage: not null } repeated && _coreExtraCardEffectRoundsOperations.IsExtraCardEffectRound(repeated.Id))
            SetShiScDamageState(repeated.Id, null);
        if (payload is ProgramDamageTransferredEvent moved && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } a && ShiScDamageState(a.ResolutionId)is { TransferredTarget: null } original && a.DamageRedirected && moved.ResolutionId == a.ResolutionId && moved.SourceSeat == a.SourceSeat && moved.TargetSeat == a.TargetSeat && original.ChainCursor == ShiScDamageCursor(a))
            SetShiScDamageState(a.ResolutionId, original with { TransferredTarget = moved.TargetSeat });
        if (payload is not DamageAppliedEvent { Amount: > 0, SourceLess: false } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { IsSourceLess: false } attack || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != applied.TargetSeat || damage.SourceSeat != applied.SourceSeat || damage.Amount != applied.Amount || ShiScDamageState(attack.ResolutionId)is not { } r || r.Source != applied.SourceSeat || (r.TransferredTarget ?? r.Target) != applied.TargetSeat || r.ChainCursor != ShiScDamageCursor(attack) || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScDamageConsumedEvent>().Any(e => e.DamageId == damage.Id && e.ArmId == r.ArmId))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiScDamageConsumedEvent(r.ArmId, attack.ResolutionId, damage.Id, applied.TargetSeat, r.ChainCursor));
    }

    internal void SetShiScChainAmount(GameEngine.IDamageAttempt a, int amount)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(a, _coreProgramAttackAttemptOperations.CurrentDamageAttempt) || !a.IsChainPropagation || amount < 0)
            throw new InvalidOperationException("Nigu may rebase only its exact current native chained recipient.");
        if (a is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(a.ResolutionId, s => s with { DamageAmount = amount });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(a.ResolutionId, s => s!with { DamageAmount = amount });
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 49100)]
    internal void BeforeShiScChain(GameEngine.IDamageAttempt a)
    {
        if (ShiScDamageState(a.ResolutionId)is not { } r)
            return;
        if (a.IsChainPropagation && r.ChainCursor == ShiScDamageCursor(a) && (r.TransferredTarget ?? r.Target) == a.TargetSeat)
            SetShiScChainAmount(a, r.Inherited);
        SetShiScDamageState(a.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 49100)]
    internal void AfterShiScChain(GameEngine.IDamageAttempt a)
    {
        if (!a.IsChainPropagation || !a.DamageAmountFinalized)
            return;
        if (ShiScNextDamage(a).Any())
            SetShiScChainAmount(a, checked(a.DamageAmount + 1));
    }

    internal IReadOnlyList<ProgramPublicRuleStateSnapshot> MobileShiSunChenPublicStates(CharacterState owner, string skillId) => skillId != ShiScNigu ? [] : LiveShiScArms(owner.Seat).Select(e => new ProgramPublicRuleStateSnapshot(e.Source.SkillInstanceId, "nigu-damage-remaining", ProgramPublicRuleStateKind.ActivationLimit, e.Count, _coreProgramEventHistoryOperations.ProgramEventHistory<ShiScDamageConsumedEvent>().Count(c => c.ArmId == e.FrameId), SkillUsageScope.Turn)).ToArray();
}
