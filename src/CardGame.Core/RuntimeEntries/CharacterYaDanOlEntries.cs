#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterYaDanOlRuntime))]
internal sealed partial class CharacterYaDanOlEntries
{
    private readonly Func<EngineOwners.CharacterYaDanOlRuntime> _resolve;
    private CharacterYaDanOlEntries(Func<EngineOwners.CharacterYaDanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYaDanOlEntries Create(Func<EngineOwners.CharacterYaDanOlRuntime> resolve) => new(resolve);
}
