#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuBuMouMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMouMobileRuntime> _resolve;
    private CharacterLuBuMouMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMouMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuBuMouMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLuBuMouMobileRuntime> resolve) => new(resolve);
}
