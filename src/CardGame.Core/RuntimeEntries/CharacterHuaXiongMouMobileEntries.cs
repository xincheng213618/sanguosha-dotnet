#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuaXiongMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> _resolve;
    private CharacterHuaXiongMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterHuaXiongMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaXiongMouMobileRuntime> resolve) => new(resolve);
}
