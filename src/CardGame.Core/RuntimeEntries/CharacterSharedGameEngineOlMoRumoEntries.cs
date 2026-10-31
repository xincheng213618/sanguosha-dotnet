#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime))]
internal sealed partial class CharacterSharedGameEngineOlMoRumoEntries
{
    private readonly Func<EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> _resolve;
    private CharacterSharedGameEngineOlMoRumoEntries(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSharedGameEngineOlMoRumoEntries Create(Func<EngineOwners.CharacterSharedGameEngineOlMoRumoRuntime> resolve) => new(resolve);
}
