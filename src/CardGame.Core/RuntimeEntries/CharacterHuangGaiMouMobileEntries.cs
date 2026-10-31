#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> _resolve;
    private CharacterHuangGaiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuangGaiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuangGaiMouMobileRuntime> resolve) => new(resolve);
}
