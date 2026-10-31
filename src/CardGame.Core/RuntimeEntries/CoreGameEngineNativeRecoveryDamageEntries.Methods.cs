#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeRecoveryDamageEntries
{
    internal bool ExactPaidNativeRecoveryReplacementEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _resolve().ExactPaidNativeRecoveryReplacementEdge(parent, child, history);
    internal bool ExactPaidNativeRecoveryUseEdge(ResolutionFrame parent, ResolutionFrame child, IReadOnlyList<IGameEvent> history) => _resolve().ExactPaidNativeRecoveryUseEdge(parent, child, history);
    internal bool IsOriginalDyingSuspendedByRecoveryDamage(DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByRecoveryDamage(dying);
    internal bool NativeCardRecoveryReturnMatches(CardUseFrame use, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _resolve().NativeCardRecoveryReturnMatches(use, hp, history);
    internal void RecordNativeCardRecovery(long useId, long recoveryId, int source, int target, CardKind kind, int requested, int before, int after) => _resolve().RecordNativeCardRecovery(useId, recoveryId, source, target, kind, requested, before, after);
    internal bool TryGetNativeRescueRecovery(int hpIndex, IReadOnlyList<IGameEvent> history, scoped out DyingFrame? dying) => _resolve().TryGetNativeRescueRecovery(hpIndex, history, out dying);
    internal NativeHealthRecoveryRuntime NativeHealthRecovery { get => _resolve().NativeHealthRecovery; }
}
