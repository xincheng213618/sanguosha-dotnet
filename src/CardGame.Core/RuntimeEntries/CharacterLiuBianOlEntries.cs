#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuBianOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBianOlRuntime> _resolve;
    private CharacterLiuBianOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBianOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuBianOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuBianOlRuntime> resolve) => new(resolve);
}
