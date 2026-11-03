#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageCardClaimNativeRulesRuntime
{
    DamageTriggerWindowFrame? IProgramDamageCardClaimNativeRules.CurrentDamageWindow => _coreDamageCursorRuntimeOperations.Value.ActiveDamageTrigger;

    ProgramDamageCardOwner IProgramDamageCardClaimNativeRules.SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        GameEngine.IDamageAttempt attack = _coreNativeDamageHandlesOperations.Value.GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is NativeOwners.CardAttackHandle);
    }

    long? IProgramDamageCardClaimNativeRules.ActiveGroupOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.OwnerFrameId;

}
