#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime))]
internal sealed partial class CoreGameEngineDistinctFactionDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> _resolve;
    private CoreGameEngineDistinctFactionDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDistinctFactionDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDistinctFactionDiscardsRuntime> resolve) => new(resolve);
}
