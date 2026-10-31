#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiuZanMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> _resolve;
    private CharacterLiuZanMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiuZanMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiuZanMobileRuntime> resolve) => new(resolve);
}
