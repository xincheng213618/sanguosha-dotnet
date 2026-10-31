#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> _resolve;
    private CoreGameEngineLateFixedTargetDamageCapScopesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLateFixedTargetDamageCapScopesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve) => new(resolve);
}
