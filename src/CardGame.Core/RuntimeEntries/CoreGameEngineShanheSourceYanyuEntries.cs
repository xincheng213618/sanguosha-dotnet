#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceYanyuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> _resolve;
    private CoreGameEngineShanheSourceYanyuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceYanyuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceYanyuRuntime> resolve) => new(resolve);
}
