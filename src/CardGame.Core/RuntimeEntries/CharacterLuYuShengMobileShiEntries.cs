#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CharacterLuYuShengMobileShiRuntime))]
internal sealed partial class CharacterLuYuShengMobileShiEntries
{
    private readonly Func<EngineOwners.CharacterLuYuShengMobileShiRuntime> _resolve;
    private CharacterLuYuShengMobileShiEntries(Func<EngineOwners.CharacterLuYuShengMobileShiRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CharacterLuYuShengMobileShiEntries Create(Func<EngineOwners.CharacterLuYuShengMobileShiRuntime> resolve) => new(resolve);
}
