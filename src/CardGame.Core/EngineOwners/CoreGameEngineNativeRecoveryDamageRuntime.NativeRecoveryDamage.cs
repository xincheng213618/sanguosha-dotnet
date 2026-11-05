#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeRecoveryDamageRuntime
{
    internal NativeHealthRecoveryRuntime NativeHealthRecovery => _nativeHealthRecoveryOperations.Get();

    internal void RecordNativeCardRecovery(long useId, long recoveryId, int source, int target, CardKind kind, int requested, int before, int after) => NativeHealthRecovery.RecordNativeCardRecovery(useId, recoveryId, source, target, kind, requested, before, after);
    internal bool TryGetNativeRescueRecovery(int hpIndex, out DyingFrame? dying) => NativeHealthRecovery.TryGetNativeRescueRecovery(hpIndex, out dying);
    internal bool NativeCardRecoveryReturnMatches(CardUseFrame use, HpChangedTriggerWindowFrame hp) => NativeHealthRecovery.NativeCardRecoveryReturnMatches(use, hp);
    internal static bool NativeRecoveryMaterialGroupMatches(CardUseFrame use) => RuntimeSkills.RuntimeRules_Core_NativeRecoveryDamage.NativeRecoveryMaterialGroupMatches(use);
    internal bool ExactPaidNativeRecoveryUseEdge(ResolutionFrame parent, ResolutionFrame child, Func<IReadOnlyList<IGameEvent>> history) => NativeHealthRecovery.ExactPaidNativeRecoveryUseEdge(parent, child, history);
    internal bool ExactPaidNativeRecoveryReplacementEdge(ResolutionFrame parent, ResolutionFrame child, Func<IReadOnlyList<IGameEvent>> history) => NativeHealthRecovery.ExactPaidNativeRecoveryReplacementEdge(parent, child, history);
    internal bool IsOriginalDyingSuspendedByRecoveryDamage(DyingFrame dying) => NativeHealthRecovery.IsOriginalDyingSuspendedByRecoveryDamage(dying);
}
