#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunChenMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> _resolve;
    private CharacterSunChenMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunChenMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunChenMobileShiRuntime> resolve) => new(resolve);
}
