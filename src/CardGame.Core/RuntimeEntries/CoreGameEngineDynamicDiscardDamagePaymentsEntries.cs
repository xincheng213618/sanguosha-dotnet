#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime))]
internal sealed partial class CoreGameEngineDynamicDiscardDamagePaymentsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> _resolve;
    private CoreGameEngineDynamicDiscardDamagePaymentsEntries(Func<EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDynamicDiscardDamagePaymentsEntries Create(Func<EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> resolve) => new(resolve);
}
