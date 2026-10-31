#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuZhiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime> _resolve;
    private CharacterLuZhiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuZhiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuZhiMouMobileRuntime> resolve) => new(resolve);
}
