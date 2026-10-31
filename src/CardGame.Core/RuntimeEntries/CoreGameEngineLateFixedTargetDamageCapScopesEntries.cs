#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime))]
internal sealed partial class CoreGameEngineLateFixedTargetDamageCapScopesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> _resolve;
    private CoreGameEngineLateFixedTargetDamageCapScopesEntries(Func<EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLateFixedTargetDamageCapScopesEntries Create(Func<EngineOwners.CoreGameEngineLateFixedTargetDamageCapScopesRuntime> resolve) => new(resolve);
}
