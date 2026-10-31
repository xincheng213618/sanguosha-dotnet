#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGaoLanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoLanOlRuntime> _resolve;
    private CharacterGaoLanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoLanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGaoLanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGaoLanOlRuntime> resolve) => new(resolve);
}
