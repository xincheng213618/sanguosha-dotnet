#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourcePojunEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourcePojunRuntime> _resolve;
    private CoreGameEngineShanheSourcePojunEntries(Func<EngineOwners.CoreGameEngineShanheSourcePojunRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourcePojunEntries Create(Func<EngineOwners.CoreGameEngineShanheSourcePojunRuntime> resolve) => new(resolve);
}
