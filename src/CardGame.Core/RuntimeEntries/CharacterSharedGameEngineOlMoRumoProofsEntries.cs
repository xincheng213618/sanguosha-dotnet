#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime))]
internal sealed partial class CharacterSharedGameEngineOlMoRumoProofsEntries
{
    private readonly Func<EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoProofsEntries(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoProofsEntries Create(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoProofsRuntime> resolve) => new(resolve);
}
