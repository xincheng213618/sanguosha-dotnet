#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterBuLianShiMobileRuntime))]
internal sealed partial class CharacterBuLianShiMobileEntries
{
    private readonly Func<EngineOwners.CharacterBuLianShiMobileRuntime> _resolve;
    private CharacterBuLianShiMobileEntries(Func<EngineOwners.CharacterBuLianShiMobileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterBuLianShiMobileEntries Create(Func<EngineOwners.CharacterBuLianShiMobileRuntime> resolve) => new(resolve);
}
