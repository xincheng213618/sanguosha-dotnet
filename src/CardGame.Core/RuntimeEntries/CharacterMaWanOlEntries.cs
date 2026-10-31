#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterMaWanOlRuntime))]
internal sealed partial class CharacterMaWanOlEntries
{
    private readonly Func<EngineOwners.CharacterMaWanOlRuntime> _resolve;
    private CharacterMaWanOlEntries(Func<EngineOwners.CharacterMaWanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMaWanOlEntries Create(Func<EngineOwners.CharacterMaWanOlRuntime> resolve) => new(resolve);
}
