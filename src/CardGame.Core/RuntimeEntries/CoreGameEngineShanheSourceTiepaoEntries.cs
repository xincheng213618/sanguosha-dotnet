#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheSourceTiepaoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> _resolve;
    private CoreGameEngineShanheSourceTiepaoEntries(Func<EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheSourceTiepaoEntries Create(Func<EngineOwners.CoreGameEngineShanheSourceTiepaoRuntime> resolve) => new(resolve);
}
