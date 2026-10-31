#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhuRongMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongMouMobileRuntime> _resolve;
    private CharacterZhuRongMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhuRongMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhuRongMouMobileRuntime> resolve) => new(resolve);
}
