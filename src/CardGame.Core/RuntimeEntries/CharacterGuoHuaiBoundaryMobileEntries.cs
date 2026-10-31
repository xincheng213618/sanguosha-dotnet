#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGuoHuaiBoundaryMobileEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> _resolve;
    private CharacterGuoHuaiBoundaryMobileEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterGuoHuaiBoundaryMobileEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoHuaiBoundaryMobileRuntime> resolve) => new(resolve);
}
