#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuangaoEntries
{
    private readonly Func<EngineOwners.CoreGameEngineShanheGuangaoRuntime> _resolve;
    private CoreGameEngineShanheGuangaoEntries(Func<EngineOwners.CoreGameEngineShanheGuangaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuangaoEntries Create(Func<EngineOwners.CoreGameEngineShanheGuangaoRuntime> resolve) => new(resolve);
}
