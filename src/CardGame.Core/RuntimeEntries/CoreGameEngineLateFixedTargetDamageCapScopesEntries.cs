#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime))]
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> _resolve;
    private CoreGameEngineLateFixedTargetDamageCapScopesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLateFixedTargetDamageCapScopesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve) => new(resolve);
}
