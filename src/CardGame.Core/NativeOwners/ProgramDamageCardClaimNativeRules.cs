#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramDamageCardClaimNativeRules(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries> getCoreDamageCursorRuntimeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries> getCoreNativeDamageHandlesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries> getCoreNativeCardContinuationCursorsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineExtraEffectMaterialClaimsEntries> getCoreGameEngineExtraEffectMaterialClaimsRuntime) : IProgramDamageCardClaimNativeRules
{
    public DamageTriggerWindowFrame? CurrentDamageWindow => getCoreDamageCursorRuntimeRuntime().ActiveDamageTrigger;

    public ProgramDamageCardOwner SelectAttackOwner(DamageTriggerWindowFrame window)
    {
        var attack = getCoreNativeDamageHandlesRuntime().GetDamageTriggerAttack(window);
        return new(attack.ResolutionId, attack is global::CardGame.Core.NativeOwners.CardAttackHandle);
    }

    public long? ActiveGroupOwnerId => getCoreNativeCardContinuationCursorsRuntime().ActiveGroupCard?.OwnerFrameId;

    public void CaptureExtraEffectClaims(ProgramSkillFrame frame, DamageTriggerWindowFrame window, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason) => getCoreGameEngineExtraEffectMaterialClaimsRuntime().CaptureExtraEffectMaterialClaims(frame, getCoreNativeDamageHandlesRuntime().GetDamageTriggerAttack(window), cards, sequenceBefore, reason);
}
