#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDingFengMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingFengMobileRuntime> _resolve;
    private CharacterDingFengMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingFengMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterDingFengMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDingFengMobileRuntime> resolve) => new(resolve);
}
