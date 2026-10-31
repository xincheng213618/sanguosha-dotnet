#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterWeiZiOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiZiOlRuntime> _resolve;
    private CharacterWeiZiOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiZiOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterWeiZiOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterWeiZiOlRuntime> resolve) => new(resolve);
}
