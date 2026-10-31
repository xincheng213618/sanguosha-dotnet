#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceShennuEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime> _resolve;
    private CoreGameEngineShanheSourceShennuEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceShennuEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceShennuRuntime> resolve) => new(resolve);
}
