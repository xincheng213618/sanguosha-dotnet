#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQingjianEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> _resolve;
    private CoreGameEngineShanheSourceQingjianEntries(Func<EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceQingjianEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> resolve) => new(resolve);
}
