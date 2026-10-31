#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunRuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunRuOlRuntime> _resolve;
    private CharacterSunRuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunRuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunRuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunRuOlRuntime> resolve) => new(resolve);
}
