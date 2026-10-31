#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMengHuoMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> _resolve;
    private CharacterMengHuoMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterMengHuoMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterMengHuoMouMobileRuntime> resolve) => new(resolve);
}
