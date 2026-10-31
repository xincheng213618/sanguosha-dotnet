#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhugeGuoMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> _resolve;
    private CharacterZhugeGuoMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhugeGuoMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeGuoMobileRuntime> resolve) => new(resolve);
}
