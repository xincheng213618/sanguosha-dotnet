#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDamageCardClaimNativeRules(Func<RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, Func<RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, Func<RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, Func<RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries> getCoreGameEngineExtraEffectMaterialClaimsRuntime) : IProgramDamageCardClaimNativeRules
{
    public DamageTriggerWindowFrame? CurrentDamageWindow => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger;

    public ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        var attack = getCoreNativeDamageHandlesRuntime().GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is CardAttackHandle);
    }

    public long? ActiveGroupOwnerId => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard?.OwnerFrameId;

    public void CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason) => getCoreGameEngineExtraEffectMaterialClaimsRuntime().CaptureExtraEffectMaterialClaims(frame, getCoreNativeDamageHandlesRuntime().GetDamageTriggerAttack(window), cards, sequenceBefore, reason);
}
