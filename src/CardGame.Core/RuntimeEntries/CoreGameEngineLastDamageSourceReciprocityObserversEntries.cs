#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime))]
internal sealed partial class CoreGameEngineLastDamageSourceReciprocityObserversEntries
{
    private readonly Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> _resolve;
    private CoreGameEngineLastDamageSourceReciprocityObserversEntries(Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLastDamageSourceReciprocityObserversEntries Create(Func<EngineOwners.CoreGameEngineLastDamageSourceReciprocityObserversRuntime> resolve) => new(resolve);
}
