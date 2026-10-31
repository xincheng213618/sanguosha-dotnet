#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunRuOlRuntime))]
internal sealed partial class CharacterSunRuOlEntries
{
    private readonly Func<EngineOwners.CharacterSunRuOlRuntime> _resolve;
    private CharacterSunRuOlEntries(Func<EngineOwners.CharacterSunRuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunRuOlEntries Create(Func<EngineOwners.CharacterSunRuOlRuntime> resolve) => new(resolve);
}
