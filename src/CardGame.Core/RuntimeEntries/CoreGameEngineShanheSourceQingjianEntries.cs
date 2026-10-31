#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceQingjianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> _resolve;
    private CoreGameEngineShanheSourceQingjianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceQingjianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceQingjianRuntime> resolve) => new(resolve);
}
