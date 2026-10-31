#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed partial class CoreGameEngineShanheStartingResourcesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _resolve;
    private CoreGameEngineShanheStartingResourcesEntries(Func<EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheStartingResourcesEntries Create(Func<EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> resolve) => new(resolve);
}
