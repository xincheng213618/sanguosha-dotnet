#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBaOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> _resolve;
    private CharacterLiuBaOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuBaOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBaOlRuntime> resolve) => new(resolve);
}
