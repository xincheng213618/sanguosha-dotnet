#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunChenMobileShiRuntime))]
internal sealed partial class CharacterSunChenMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterSunChenMobileShiRuntime> _resolve;
    private CharacterSunChenMobileShiEntries(Func<EngineOwners.CharacterSunChenMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunChenMobileShiEntries Create(Func<EngineOwners.CharacterSunChenMobileShiRuntime> resolve) => new(resolve);
}
