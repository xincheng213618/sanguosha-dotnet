#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterKongShuOlEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> _resolve;
    private CharacterKongShuOlEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterKongShuOlEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterKongShuOlRuntime> resolve) => new(resolve);
}
