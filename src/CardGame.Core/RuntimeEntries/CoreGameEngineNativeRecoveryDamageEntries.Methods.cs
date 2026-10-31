#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNativeRecoveryDamageEntries
{
    internal bool ExactPaidNativeRecoveryReplacementEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ExactPaidNativeRecoveryReplacementEdge(parent, child, history);
    internal bool ExactPaidNativeRecoveryUseEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().ExactPaidNativeRecoveryUseEdge(parent, child, history);
    internal bool IsOriginalDyingSuspendedByRecoveryDamage(global::CardGame.Core.DyingFrame dying) => _resolve().IsOriginalDyingSuspendedByRecoveryDamage(dying);
    internal bool NativeCardRecoveryReturnMatches(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.HpChangedTriggerWindowFrame hp, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history) => _resolve().NativeCardRecoveryReturnMatches(use, hp, history);
    internal void RecordNativeCardRecovery(long useId, long recoveryId, int source, int target, global::CardGame.Core.CardKind kind, int requested, int before, int after) => _resolve().RecordNativeCardRecovery(useId, recoveryId, source, target, kind, requested, before, after);
    internal bool TryGetNativeRescueRecovery(int hpIndex, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.IGameEvent> history, scoped out global::CardGame.Core.DyingFrame? dying) => _resolve().TryGetNativeRescueRecovery(hpIndex, history, out dying);
    internal global::CardGame.Core.NativeHealthRecoveryRuntime NativeHealthRecovery { get => _resolve().NativeHealthRecovery; }
}
