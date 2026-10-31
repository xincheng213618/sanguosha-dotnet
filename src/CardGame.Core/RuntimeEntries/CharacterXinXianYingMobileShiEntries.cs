#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterXinXianYingMobileShiRuntime))]
internal sealed partial class CharacterXinXianYingMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterXinXianYingMobileShiRuntime> _resolve;
    private CharacterXinXianYingMobileShiEntries(Func<EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterXinXianYingMobileShiEntries Create(Func<EngineOwners.CharacterXinXianYingMobileShiRuntime> resolve) => new(resolve);
}
