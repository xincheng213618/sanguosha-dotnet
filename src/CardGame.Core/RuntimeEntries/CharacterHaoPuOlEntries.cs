#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterHaoPuOlRuntime))]
internal sealed partial class CharacterHaoPuOlEntries
{
    private readonly Func<EngineOwners.CharacterHaoPuOlRuntime> _resolve;
    private CharacterHaoPuOlEntries(Func<EngineOwners.CharacterHaoPuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHaoPuOlEntries Create(Func<EngineOwners.CharacterHaoPuOlRuntime> resolve) => new(resolve);
}
