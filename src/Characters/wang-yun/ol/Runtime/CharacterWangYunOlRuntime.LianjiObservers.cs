#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangYunOlRuntime
{
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 110)]
    internal void ObserveWangYunLianjiDamage(DamageAppliedEvent damage)
    {
        if (!HasWangYunPlanCapability || damage.Amount <= 0 || _coreProgramAttackAttemptOperations.CurrentDamageAttempt is not { } attack || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { WangYunNativeOrigin: { Kind: "slash", PaymentCompleted: true } r } use || !ValidWangYunNativeUse(use))
            return;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WangYunLianjiDamageEvent(r.ProgramFrameId, r.UseId, r.ActionId, r.Source.OwnerSeat, damage.SourceSeat, damage.TargetSeat, damage.Amount));
    }
}
