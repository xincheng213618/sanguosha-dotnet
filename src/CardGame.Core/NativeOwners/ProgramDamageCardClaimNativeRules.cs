#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDamageCardClaimNativeRules(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreDamageCursorRuntimeRuntime>> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreNativeDamageHandlesRuntime>> getCoreNativeDamageHandlesRuntime, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, Func<RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries> getCoreGameEngineExtraEffectMaterialClaimsRuntime) : IProgramDamageCardClaimNativeRules
{
    public DamageTriggerWindowFrame? CurrentDamageWindow => getCoreDamageCursorRuntimeRuntime().Value.ActiveDamageTrigger;

    public ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        var attack = getCoreNativeDamageHandlesRuntime().Value.GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is CardAttackHandle);
    }

    public long? ActiveGroupOwnerId => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard?.OwnerFrameId;

    public void CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason) => getCoreGameEngineExtraEffectMaterialClaimsRuntime().CaptureExtraEffectMaterialClaims(frame, getCoreNativeDamageHandlesRuntime().Value.GetDamageTriggerAttack(window), cards, sequenceBefore, reason);
}
