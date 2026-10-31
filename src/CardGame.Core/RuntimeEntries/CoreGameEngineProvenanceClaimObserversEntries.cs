#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime))]
internal sealed partial class CoreGameEngineProvenanceClaimObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> _resolve;
    private CoreGameEngineProvenanceClaimObserversEntries(Func<EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProvenanceClaimObserversEntries Create(Func<EngineOwners.CoreGameEngineProvenanceClaimObserversRuntime> resolve) => new(resolve);
}
