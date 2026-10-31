#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMuShunAnniversaryRuntime
{
    internal MuShunDamageReceipt? MuShunDamageState(long id) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == id) switch
    {
        CardUseFrame f => f.MuShunDamage,
        ProgramSkillFrame f => f.MuShunDamage,
        JudgmentFrame f => f.MuShunDamage,
        _ => null
    };
    internal void SetMuShunDamageState(long id, MuShunDamageReceipt? r)
    {
        var f = _resolutionStackOperations._resolutionStack.Single(x => x.Id == id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(id, f switch
        {
            CardUseFrame c => c with { MuShunDamage = r },
            ProgramSkillFrame p => p with { MuShunDamage = r },
            JudgmentFrame j => j with { MuShunDamage = r },
            _ => throw new InvalidOperationException("Shizhao damage needs its original native attack frame.")});
    }

    internal int MuShunChainCursor(GameEngine.IDamageAttempt a) => !a.IsChainPropagation ? -1 : a is NativeOwners.ProgramAttackHandle ? _coreProgramAttackAttemptOperations.GetProgramAttackState(a.ResolutionId).ChainedTargetIndex : _coreCardAttackStateOperations.GetCardAttackState(a.ResolutionId).ChainedTargetIndex;
    internal MuShunShizhaoIssuedEvent? MuShunDamageArm(GameEngine.IDamageAttempt attack)
    {
        if (!HasMuShunShizhaoCapability)
            return null;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MuShunShizhaoIssuedEvent>().FirstOrDefault(e => !e.Draw && e.Source.OwnerSeat == attack.TargetSeat && e.ActualTurn == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber && e.ActualTurnOwner == _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat && e.MarkerBefore == 0 && e.MarkerAfter == 0 && _contentRegistryOperations._contentRegistry.Skills.TryGetValue(e.Source.SkillId, out var skill) && e.Source.SkillId == MuShunComposition.Shizhao && e.Source.BindingId == MuShunComposition.Loss && skill.Program?.GameplayHash == e.Hash && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<MuShunShizhaoDamageSpentEvent>().Any(spent => spent.ArmId == e.FrameId) && !_coreProgramEventHistoryOperations.Value.ProgramEventHistory<TurnEndedEvent>().Any(end => end.TurnNumber == e.ActualTurn && end.ActorSeat == e.ActualTurnOwner));
    }

    [ProgramDamageHook(ProgramDamagePhase.FactModifiers, 45300)]
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> MuShunNextReceivedDamage(GameEngine.IDamageAttempt attack)
    {
        var arm = MuShunDamageArm(attack);
        if (arm is null || attack.DamageAmount <= 0)
            return[];
        var cursor = MuShunChainCursor(attack);
        if (MuShunDamageState(attack.ResolutionId)is not { } r || r.TargetSeat != attack.TargetSeat || r.ChainCursor != cursor || r.ArmId != arm.FrameId)
            SetMuShunDamageState(attack.ResolutionId, new(attack.ResolutionId, attack.TargetSeat, cursor, arm.FrameId, attack.DamageAmount));
        return[(new CardUseEffectSource(arm.Source.SkillId, arm.Source.BindingId, arm.Source.OwnerSeat, arm.Source.SkillInstanceId), 1)];
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 45300)]
    internal void ConsumeMuShunDamage(IGameEvent payload)
    {
        if (!HasMuShunShizhaoCapability || payload is not DamageAppliedEvent { Amount: > 0 } applied || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _resolutionStackOperations._resolutionStack.LastOrDefault()is not DamageFrame damage || damage.ParentFrameId != attack.ResolutionId || damage.TargetSeat != applied.TargetSeat || damage.SourceSeat != applied.SourceSeat || damage.Amount != applied.Amount || MuShunDamageState(attack.ResolutionId)is not { } r || r.TargetSeat != attack.TargetSeat || r.TargetSeat != applied.TargetSeat || r.ChainCursor != MuShunChainCursor(attack) || MuShunDamageArm(attack)is not { } arm || arm.FrameId != r.ArmId)
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MuShunShizhaoDamageSpentEvent(arm.FrameId, attack.ResolutionId, applied.TargetSeat, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, damage.Id));
    }

    [ProgramDamageHook(ProgramDamagePhase.BeforeChainAdvance, 45300)]
    internal void BeforeMuShunChain(GameEngine.IDamageAttempt attack)
    {
        if (!HasMuShunShizhaoCapability || MuShunDamageState(attack.ResolutionId)is not { } r)
            return;
        if (attack.IsChainPropagation && r.TargetSeat == attack.TargetSeat && r.ChainCursor == MuShunChainCursor(attack))
        {
            if (attack is NativeOwners.ProgramAttackHandle)
                _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = r.InheritedAmount });
            else
                _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = r.InheritedAmount });
        }

        SetMuShunDamageState(attack.ResolutionId, null);
    }

    [ProgramDamageHook(ProgramDamagePhase.AfterChainAdvance, 45300)]
    internal void AfterMuShunChain(GameEngine.IDamageAttempt attack)
    {
        if (!HasMuShunShizhaoCapability || !attack.IsChainPropagation || !attack.DamageAmountFinalized)
            return;
        var modifiers = MuShunNextReceivedDamage(attack).ToArray();
        if (modifiers.Length == 0)
            return;
        if (attack is NativeOwners.ProgramAttackHandle)
            _coreProgramAttackAttemptOperations.UpdateProgramAttackState(attack.ResolutionId, s => s with { DamageAmount = checked(s.DamageAmount + 1) });
        else
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, s => s!with { DamageAmount = checked(s.DamageAmount + 1) });
    }
}
