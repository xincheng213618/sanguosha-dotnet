#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> _resolve;
    private CharacterLiYiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiYiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYiOlRuntime> resolve) => new(resolve);
}
