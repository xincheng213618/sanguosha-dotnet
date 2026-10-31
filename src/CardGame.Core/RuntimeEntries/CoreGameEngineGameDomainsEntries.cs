#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineGameDomainsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime> _resolve;
    private CoreGameEngineGameDomainsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineGameDomainsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGameDomainsRuntime> resolve) => new(resolve);
}
