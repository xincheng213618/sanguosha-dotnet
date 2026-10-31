#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterChenZhiMobileShiRuntime))]
internal sealed partial class CharacterChenZhiMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterChenZhiMobileShiRuntime> _resolve;
    private CharacterChenZhiMobileShiEntries(Func<EngineOwners.CharacterChenZhiMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterChenZhiMobileShiEntries Create(Func<EngineOwners.CharacterChenZhiMobileShiRuntime> resolve) => new(resolve);
}
