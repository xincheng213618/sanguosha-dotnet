#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRanMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanMouMobileRuntime> _resolve;
    private CharacterZhuRanMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRanMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRanMouMobileRuntime> resolve) => new(resolve);
}
