#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiDianMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiDianMobileRuntime> _resolve;
    private CharacterLiDianMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiDianMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLiDianMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLiDianMobileRuntime> resolve) => new(resolve);
}
