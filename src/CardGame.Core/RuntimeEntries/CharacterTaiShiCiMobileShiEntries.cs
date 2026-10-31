#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterTaiShiCiMobileShiRuntime))]
internal sealed partial class CharacterTaiShiCiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterTaiShiCiMobileShiRuntime> _resolve;
    private CharacterTaiShiCiMobileShiEntries(Func<EngineOwners.CharacterTaiShiCiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterTaiShiCiMobileShiEntries Create(Func<EngineOwners.CharacterTaiShiCiMobileShiRuntime> resolve) => new(resolve);
}
