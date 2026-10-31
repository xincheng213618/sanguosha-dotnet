#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed partial class CoreGameEngineShanheStartingResourcesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _resolve;
    private CoreGameEngineShanheStartingResourcesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheStartingResourcesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> resolve) => new(resolve);
}
