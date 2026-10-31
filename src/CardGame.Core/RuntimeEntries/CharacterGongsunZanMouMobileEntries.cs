#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGongsunZanMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> _resolve;
    private CharacterGongsunZanMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGongsunZanMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGongsunZanMouMobileRuntime> resolve) => new(resolve);
}
