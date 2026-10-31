#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuangaoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuangaoRuntime> _resolve;
    private CoreGameEngineShanheGuangaoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuangaoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineShanheGuangaoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheGuangaoRuntime> resolve) => new(resolve);
}
