#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed partial class CoreGameEngineDiscardedProvenanceEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _resolve;
    private CoreGameEngineDiscardedProvenanceEntries(Func<EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDiscardedProvenanceEntries Create(Func<EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> resolve) => new(resolve);
}
