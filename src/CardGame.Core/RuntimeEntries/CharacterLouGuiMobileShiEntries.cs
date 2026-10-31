#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLouGuiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterLouGuiMobileShiRuntime> _resolve;
    private CharacterLouGuiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLouGuiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLouGuiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterLouGuiMobileShiRuntime> resolve) => new(resolve);
}
