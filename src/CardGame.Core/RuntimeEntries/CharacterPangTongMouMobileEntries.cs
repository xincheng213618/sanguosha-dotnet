#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterPangTongMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> _resolve;
    private CharacterPangTongMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterPangTongMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterPangTongMouMobileRuntime> resolve) => new(resolve);
}
