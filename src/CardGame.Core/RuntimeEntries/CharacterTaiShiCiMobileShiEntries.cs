#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTaiShiCiMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime> _resolve;
    private CharacterTaiShiCiMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaiShiCiMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterTaiShiCiMobileShiRuntime> resolve) => new(resolve);
}
