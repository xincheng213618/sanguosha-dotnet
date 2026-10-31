#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterYuJinMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime> _resolve;
    private CharacterYuJinMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterYuJinMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterYuJinMouMobileRuntime> resolve) => new(resolve);
}
