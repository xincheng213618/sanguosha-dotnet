#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuMengMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime> _resolve;
    private CharacterLuMengMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuMengMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuMengMouMobileRuntime> resolve) => new(resolve);
}
