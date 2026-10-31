#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterZhangYanMobileShiRuntime))]
internal sealed partial class CharacterZhangYanMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterZhangYanMobileShiRuntime> _resolve;
    private CharacterZhangYanMobileShiEntries(Func<EngineOwners.CharacterZhangYanMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterZhangYanMobileShiEntries Create(Func<EngineOwners.CharacterZhangYanMobileShiRuntime> resolve) => new(resolve);
}
