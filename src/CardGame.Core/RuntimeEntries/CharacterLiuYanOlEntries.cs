#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuYanOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> _resolve;
    private CharacterLiuYanOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuYanOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuYanOlRuntime> resolve) => new(resolve);
}
