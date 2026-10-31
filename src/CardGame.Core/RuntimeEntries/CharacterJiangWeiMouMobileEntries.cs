#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJiangWeiMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime> _resolve;
    private CharacterJiangWeiMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterJiangWeiMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJiangWeiMouMobileRuntime> resolve) => new(resolve);
}
