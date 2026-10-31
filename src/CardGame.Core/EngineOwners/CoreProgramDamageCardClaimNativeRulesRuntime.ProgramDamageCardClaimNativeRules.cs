#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramDamageCardClaimNativeRulesRuntime
{
    DamageTriggerWindowFrame? IProgramDamageCardClaimNativeRules.CurrentDamageWindow => _coreDamageCursorRuntimeOperations.ActiveDamageTrigger;

    ProgramDamageCardOwner IProgramDamageCardClaimNativeRules.SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        GameEngine.IDamageAttempt attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is NativeOwners.CardAttackHandle);
    }

    long? IProgramDamageCardClaimNativeRules.ActiveGroupOwnerId => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.OwnerFrameId;

    void IProgramDamageCardClaimNativeRules.CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason) => _coreExtraEffectMaterialClaimsOperations.CaptureExtraEffectMaterialClaims(frame, _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(window), cards, sequenceBefore, reason);
}
