#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunJunMobileShiRuntime))]
internal sealed partial class CharacterSunJunMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterSunJunMobileShiRuntime> _resolve;
    private CharacterSunJunMobileShiEntries(Func<EngineOwners.CharacterSunJunMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunJunMobileShiEntries Create(Func<EngineOwners.CharacterSunJunMobileShiRuntime> resolve) => new(resolve);
}
