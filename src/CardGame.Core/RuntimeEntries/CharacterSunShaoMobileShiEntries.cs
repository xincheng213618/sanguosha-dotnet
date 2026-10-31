#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterSunShaoMobileShiRuntime))]
internal sealed partial class CharacterSunShaoMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterSunShaoMobileShiRuntime> _resolve;
    private CharacterSunShaoMobileShiEntries(Func<EngineOwners.CharacterSunShaoMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterSunShaoMobileShiEntries Create(Func<EngineOwners.CharacterSunShaoMobileShiRuntime> resolve) => new(resolve);
}
