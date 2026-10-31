#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterNiuJinOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuJinOlRuntime> _resolve;
    private CharacterNiuJinOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuJinOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterNiuJinOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterNiuJinOlRuntime> resolve) => new(resolve);
}
