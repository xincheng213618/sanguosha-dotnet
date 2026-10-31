#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSharedGameEngineOlMoRumoEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> resolve) => new(resolve);
}
