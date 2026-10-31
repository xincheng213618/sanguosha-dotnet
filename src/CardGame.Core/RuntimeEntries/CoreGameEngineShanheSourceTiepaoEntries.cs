#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceTiepaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> _resolve;
    private CoreGameEngineShanheSourceTiepaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceTiepaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> resolve) => new(resolve);
}
