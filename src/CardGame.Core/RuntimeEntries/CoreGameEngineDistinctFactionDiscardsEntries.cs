#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime))]
internal sealed partial class CoreGameEngineDistinctFactionDiscardsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> _resolve;
    private CoreGameEngineDistinctFactionDiscardsEntries(Func<EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDistinctFactionDiscardsEntries Create(Func<EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> resolve) => new(resolve);
}
