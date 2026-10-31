#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProvenanceClaimObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> _resolve;
    private CoreGameEngineProvenanceClaimObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceClaimObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> resolve) => new(resolve);
}
