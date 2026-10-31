#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime))]
internal sealed partial class CoreGameEngineDynamicDiscardDamagePaymentsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> _resolve;
    private CoreGameEngineDynamicDiscardDamagePaymentsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDynamicDiscardDamagePaymentsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamagePaymentsRuntime> resolve) => new(resolve);
}
