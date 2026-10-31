#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPanShuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanShuOlRuntime> _resolve;
    private CharacterPanShuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanShuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPanShuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPanShuOlRuntime> resolve) => new(resolve);
}
