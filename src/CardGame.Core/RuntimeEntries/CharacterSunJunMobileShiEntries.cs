#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunJunMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> _resolve;
    private CharacterSunJunMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJunMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> resolve) => new(resolve);
}
