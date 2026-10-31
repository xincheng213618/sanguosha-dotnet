#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDiscardedProvenanceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _resolve;
    private CoreGameEngineDiscardedProvenanceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardedProvenanceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> resolve) => new(resolve);
}
