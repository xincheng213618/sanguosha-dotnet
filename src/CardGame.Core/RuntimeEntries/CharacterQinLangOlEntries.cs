#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterQinLangOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> _resolve;
    private CharacterQinLangOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterQinLangOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterQinLangOlRuntime> resolve) => new(resolve);
}
