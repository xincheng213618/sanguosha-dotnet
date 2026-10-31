#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime))]
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoProofsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoProofsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> resolve) => new(resolve);
}
