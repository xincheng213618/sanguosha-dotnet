#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiYanMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYanMobileRuntime> _resolve;
    private CharacterLiYanMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiYanMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiYanMobileRuntime> resolve) => new(resolve);
}
