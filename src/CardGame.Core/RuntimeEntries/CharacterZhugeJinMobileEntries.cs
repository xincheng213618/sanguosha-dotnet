#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeJinMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime> _resolve;
    private CharacterZhugeJinMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeJinMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeJinMobileRuntime> resolve) => new(resolve);
}
