#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime))]
internal sealed partial class CharacterSunJunMobileShiEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> _resolve;
    private CharacterSunJunMobileShiEntries(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJunMobileShiEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CharacterSunJunMobileShiRuntime> resolve) => new(resolve);
}
