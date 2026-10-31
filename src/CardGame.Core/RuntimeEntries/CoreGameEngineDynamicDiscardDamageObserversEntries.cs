#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime))]
internal sealed partial class CoreGameEngineDynamicDiscardDamageObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> _resolve;
    private CoreGameEngineDynamicDiscardDamageObserversEntries(Func<EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDynamicDiscardDamageObserversEntries Create(Func<EngineOwners.CoreGameEngineDynamicDiscardDamageObserversRuntime> resolve) => new(resolve);
}
