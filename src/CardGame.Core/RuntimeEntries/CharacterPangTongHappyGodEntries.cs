#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongHappyGodEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime> _resolve;
    private CharacterPangTongHappyGodEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongHappyGodEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongHappyGodRuntime> resolve) => new(resolve);
}
