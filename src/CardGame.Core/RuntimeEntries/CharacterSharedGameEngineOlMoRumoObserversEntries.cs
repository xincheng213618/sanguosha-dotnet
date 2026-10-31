#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime))]
internal sealed partial class CharacterSharedGameEngineOlMoRumoObserversEntries
{
    private readonly Func<EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoObserversEntries(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoObserversEntries Create(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> resolve) => new(resolve);
}
