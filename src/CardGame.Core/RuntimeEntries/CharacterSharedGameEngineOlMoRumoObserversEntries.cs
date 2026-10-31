#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoObserversRuntime> resolve) => new(resolve);
}
