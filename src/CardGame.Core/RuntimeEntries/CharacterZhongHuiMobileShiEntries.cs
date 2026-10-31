#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhongHuiMobileShiRuntime))]
internal sealed partial class CharacterZhongHuiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZhongHuiMobileShiRuntime> _resolve;
    private CharacterZhongHuiMobileShiEntries(Func<EngineOwners.CharacterZhongHuiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhongHuiMobileShiEntries Create(Func<EngineOwners.CharacterZhongHuiMobileShiRuntime> resolve) => new(resolve);
}
