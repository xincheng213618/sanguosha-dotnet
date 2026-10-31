#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime))]
internal sealed partial class CoreGameEngineDynamicDiscardDamageObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> _resolve;
    private CoreGameEngineDynamicDiscardDamageObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDynamicDiscardDamageObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> resolve) => new(resolve);
}
