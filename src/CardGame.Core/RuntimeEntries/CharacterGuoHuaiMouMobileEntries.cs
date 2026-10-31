#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> _resolve;
    private CharacterGuoHuaiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiMouMobileRuntime> resolve) => new(resolve);
}
